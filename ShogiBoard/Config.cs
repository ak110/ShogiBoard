using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShogiBoard {
    /// <summary>
    /// コンフィグ(比較的変えなそうなもの)
    /// </summary>
    [ConfigSerializeFileName("ShogiBoard.xml")]
    public class Config {
        /// <summary>
        /// 接続先設定
        /// </summary>
        public class NetworkGameConnection {
            /// <summary>
            /// アドレス[：ポート番号]
            /// </summary>
            public string Address { get; set; }
            /// <summary>
            /// ユーザ名
            /// </summary>
            public string User { get; set; }
            /// <summary>
            /// パスワード
            /// </summary>
            public string Pass { get; set; }
            /// <summary>
            /// 読み筋を送る
            /// </summary>
            public bool SendPV { get; set; }
            /// <summary>
            /// KeepAlive（無通信時に改行１個を定期的に送信）する
            /// </summary>
            public bool KeepAlive { get; set; }
            /// <summary>
            /// 初期値を設定
            /// </summary>
            public NetworkGameConnection() {
                Address = "wdoor.c.u-tokyo.ac.jp:4081";
                User = "<username>";
                Pass = "floodgate-900-0,<password>";
                SendPV = true;
                KeepAlive = Environment.OSVersion.Platform != PlatformID.Win32NT;
            }
        }
        /// <summary>
        /// 接続先設定
        /// </summary>
        public NetworkGameConnection[] NetworkGameConnections { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Ready() {
            if (NetworkGameConnections == null) {
                NetworkGameConnections = new NetworkGameConnection[6];
            }
            if (NetworkGameConnections.Length != 6) {
                var a = NetworkGameConnections;
                Array.Resize(ref a, 6);
                NetworkGameConnections = a;
            }
            // 無ければ作る
            for (int i = 0; i < NetworkGameConnections.Length; i++) {
                if (NetworkGameConnections[i] == null) {
                    NetworkGameConnections[i] = new NetworkGameConnection(); // デフォルト値
                }
            }
        }
    }
}
