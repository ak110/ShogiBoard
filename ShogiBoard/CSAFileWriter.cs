using ShogiCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ShogiBoard {
    /// <summary>
    /// 通信対局の結果をCSA棋譜化するためのクラス
    /// </summary>
    class CSAFileWriter : IDisposable {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        StreamWriter writer;

        /// <summary>
        /// Create済みかどうか
        /// </summary>
        public bool Created { get { return writer != null; } }

        public CSAFileWriter() {
        }

        /// <summary>
        /// 初期化(ファイル作成)
        /// </summary>
        /// <param name="nameP">先手の名前</param>
        /// <param name="nameN">先手の名前</param>
        /// <param name="gameID">ゲームID</param>
        /// <param name="initialPositionString">初期局面の棋譜</param>
        public void Create(string nameP, string nameN, string gameID, string initialPositionString) {
            try {
                if (writer != null) {
                    writer.Dispose();
                }

                using (Mutex m = new Mutex(false, "{6CF923FD-7A26-42A0-8614-67AE6004CF7C}")) {
                    try {
                        m.WaitOne();
                    } catch { // 微妙だけど無視
                    }
                    try {
                        // 将棋所風ファイル名：「20130104_152323 BlunderXX vs BlunderXX-Test.csa」
                        string name = DateTime.Now.ToString("yyyyMMdd_HHmmss") + " " + nameP + " vs " + nameN;
                        name = SanitizeFileName(name);
                        string pathWithoutExtension = Path.Combine(Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory, "Logs"), name);
                        string path = pathWithoutExtension + ".csa";
                        if (File.Exists(path)) {
                            for (int i = 1; ; i++) {
                                string t = pathWithoutExtension + " - (" + i + ")" + ".csa";
                                if (!File.Exists(t)) {
                                    path = t;
                                    break;
                                }
                            }
                        }
                        writer = new StreamWriter(path, false, Encoding.GetEncoding(932));
                    } finally {
                        m.ReleaseMutex();
                    }
                }

                writer.WriteLine("V2.2");
                writer.Write("N+");
                writer.WriteLine(nameP);
                writer.Write("N-");
                writer.WriteLine(nameN);
                if (!string.IsNullOrEmpty(gameID)) {
                    writer.Write("$EVENT:");
                    writer.WriteLine(gameID);
                }
                writer.Write("$START_TIME:");
                writer.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                if (string.IsNullOrEmpty(initialPositionString)) {
                    writer.WriteLine("PI"); // 適当
                } else {
                    writer.WriteLine(initialPositionString.TrimEnd());
                }
            } catch (Exception e) {
                logger.Warn("CSA棋譜の書き込みに失敗(1)", e);
                writer = null;
            }
        }

        /// <summary>
        /// ファイル名を適当サニタイジング。こんなもんでいいんだろうか…。
        /// </summary>
        private string SanitizeFileName(string name) {
            if (Regex.IsMatch(name, @"^(AUX|CON|NUL|PRN|COM[1-9]|LPT[1-9])(\..*)?$")) {
                return DateTime.Now.ToString("yyyyMMdd_HHmmssfff"); // どうしようもないので適当
            }
            StringBuilder str = new StringBuilder(name);
            foreach (char c in Path.GetInvalidFileNameChars()) {
                str.Replace(c, '_');
            }
            return str.ToString();
        }

        /// <summary>
        /// 終了(ファイル閉じる)
        /// </summary>
        public void Dispose() {
            try {
                if (writer != null) {
                    // TODO: floodgateを真似て下記のような感じにしたい
                    //'P1 *  *  *  *  *  *  * -HI-KY
                    //'P2 * +NY+KA * -KI *  *  *  * 
                    //'P3 *  *  *  * -OU-FU-FU *  * 
                    //'P4 * +FU-FU-FU-GI-KI-KI * -FU
                    //'P5+KY+KE+FU * -KE * -GI+KE * 
                    //'P6+FU+OU * +FU * +FU * +FU+FU
                    //'P7 * +KA+KE *  *  *  *  *  * 
                    //'P8 *  *  *  *  * +KI *  *  * 
                    //'P9 * +KY * +GI *  *  *  *  * 
                    //'P+00FU00FU00FU00FU00FU00GI00HI
                    //'P-00FU
                    //'+
                    //'summary:toryo:Gekisashi lose:Apery_2700K_4c win
                    //'$END_TIME:2013/01/04 05:21:25

                    // とりあえずEND_TIMEだけ。
                    writer.Write("'$END_TIME:");
                    writer.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    writer.Dispose();
                    writer = null;
                }
            } catch (Exception ex) {
                logger.Warn("CSA棋譜の書き込みに失敗(3)", ex);
            }
        }

        /// <summary>
        /// 指し手・消費時間の書き込み
        /// </summary>
        /// <param name="moveString">指し手・消費時間</param>
        /// <param name="moveComment">指し手のコメント(「* {評価値} {PV}」)</param>
        public void AppendMove(string moveString, string moveComment = null) {
            try {
                if (writer != null) {
                    writer.WriteLine(moveString);
                    if (!string.IsNullOrEmpty(moveComment)) {
                        writer.WriteLine("'*" + moveComment);
                    }
                    writer.Flush();
                }
            } catch (Exception e) {
                logger.Warn("CSA棋譜の書き込みに失敗(2)", e);
            }
        }
    }
}
