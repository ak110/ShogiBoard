using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShogiBoard {
    /// <summary>
    /// コンフィグ(頻繁に変わる項目)
    /// </summary>
    [ConfigSerializeFileName("ShogiBoard.Volatile.xml")]
    public class VolatileConfig {
        /// <summary>
        /// 対局の時間設定
        /// </summary>
        public class GameTime {
            /// <summary>
            /// 持ち時間[秒]
            /// </summary>
            public int TimeASeconds { get; set; }
            /// <summary>
            /// 秒読み[秒]
            /// </summary>
            public int TimeBSeconds { get; set; }
        }

        /// <summary>
        /// 対局で前回選択したエンジンの名前
        /// </summary>
        public string GameEngine1Name { get; set; }
        /// <summary>
        /// 対局で前回選択したエンジンの実行ファイルパス
        /// </summary>
        public string GameEngine1Path { get; set; }
        /// <summary>
        /// 対局で前回選択したエンジンの名前
        /// </summary>
        public string GameEngine2Name { get; set; }
        /// <summary>
        /// 対局で前回選択したエンジンの実行ファイルパス
        /// </summary>
        public string GameEngine2Path { get; set; }
        /// <summary>
        /// 対局で前回選択した時間設定のindex
        /// </summary>
        public int GameTimeIndex { get; set; }
        /// <summary>
        /// 対局の時間設定
        /// </summary>
        public GameTime[] GameTimes { get; set; }

        /// <summary>
        /// 時間切れを負けとするのかどうか
        /// </summary>
        [XmlIgnore] // 移行用のため記録しない
        private bool GameJudgeTimeUp { get; set; }
        /// <summary>
        /// 時間切れの扱い。0:判定しない、1:負け扱い、2:引き分け扱い
        /// </summary>
        public int GameTimeUpType { get; set; }

        /// <summary>
        /// 通信対局の回数。0で無制限。
        /// </summary>
        public int GameCount { get; set; }
        /// <summary>
        /// 開始局面。0で初期局面、1で棋譜の局面。
        /// </summary>
        public int GameStartPosType { get; set; }
        /// <summary>
        /// 開始局面の棋譜。
        /// </summary>
        public string GameStartPosNotationPath { get; set; }
        /// <summary>
        /// 開始局面の棋譜のシャッフル
        /// </summary>
        public bool GameStartPosNotationShuffle { get; set; }
        /// <summary>
        /// 開始局面の棋譜の開始手数。
        /// </summary>
        public int GameStartPosNotationStartCount { get; set; }

        /// <summary>
        /// 通信対局で前回選択したエンジンの名前
        /// </summary>
        public string NetworkGameEngineName { get; set; }
        /// <summary>
        /// 通信対局で前回選択したエンジンの実行ファイルパス
        /// </summary>
        public string NetworkGameEnginePath { get; set; }
        /// <summary>
        /// 通信対局で前回選択した接続先
        /// </summary>
        public int NetworkGameConnectionIndex { get; set; }
        /// <summary>
        /// 通信対局の回数。0で無制限。
        /// </summary>
        public int NetworkGameCount { get; set; }

        /// <summary>
        /// 検討で前回選択したエンジンの名前
        /// </summary>
        public string ThinkEngineName { get; set; }
        /// <summary>
        /// 検討で前回選択したエンジンの実行ファイルパス
        /// </summary>
        public string ThinkEnginePath { get; set; }

        /// <summary>
        /// 詰将棋解答で前回選択したエンジンの名前
        /// </summary>
        public string MateEngineName { get; set; }
        /// <summary>
        /// 詰将棋解答で前回選択したエンジンの実行ファイルパス
        /// </summary>
        public string MateEnginePath { get; set; }

        /// <summary>
        /// 初期値の設定
        /// </summary>
        public VolatileConfig() {
            GameJudgeTimeUp = true;
            GameStartPosNotationShuffle = true;
            GameStartPosNotationStartCount = 30;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Ready() {
            if (GameTimes == null) {
                GameTimes = new GameTime[3];
            }
            if (GameTimes.Length != 3) {
                var a = GameTimes;
                Array.Resize(ref a, 3);
                GameTimes = a;
            }
            // 無ければ作る
            for (int i = 0; i < GameTimes.Length; i++) {
                if (GameTimes[i] == null) {
                    GameTimes[i] = new GameTime() { TimeASeconds = 25, TimeBSeconds = 0 };
                }
            }

            // 移行
            if (!GameJudgeTimeUp)
                GameTimeUpType = 0; // 判定しない
        }
    }
}
