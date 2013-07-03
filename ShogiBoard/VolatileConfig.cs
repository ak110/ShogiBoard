using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            public int TimeA { get; set; }
            /// <summary>
            /// 秒読み[秒]
            /// </summary>
            public int TimeB { get; set; }
        }

        /// <summary>
        /// 通信対局で前回選択したエンジンの名前
        /// </summary>
        public string GameEngine1Name { get; set; }
        /// <summary>
        /// 通信対局で前回選択したエンジンの実行ファイルパス
        /// </summary>
        public string GameEngine1Path { get; set; }
        /// <summary>
        /// 通信対局で前回選択したエンジンの名前
        /// </summary>
        public string GameEngine2Name { get; set; }
        /// <summary>
        /// 通信対局で前回選択したエンジンの実行ファイルパス
        /// </summary>
        public string GameEngine2Path { get; set; }
        /// <summary>
        /// 通信対局で前回選択した接続先
        /// </summary>
        public int GameTimeIndex { get; set; }
        /// <summary>
        /// 対局の時間設定
        /// </summary>
        public GameTime[] GameTimes { get; set; }
        /// <summary>
        /// 通信対局の回数。0で無制限。
        /// </summary>
        public int GameCount { get; set; }

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
                    GameTimes[i] = new GameTime() { TimeA = 25 };
                }
            }
        }
    }
}
