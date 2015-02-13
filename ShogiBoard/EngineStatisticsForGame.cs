using ShogiCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShogiBoard {
    /// <summary>
    /// 対局してるエンジンの統計情報（1対局分の各手番の情報）
    /// </summary>
    public class EngineStatisticsForGame {
        public readonly GameMeanValue TimeReal = new GameMeanValue();
        public readonly GameMeanValue TimeUSI = new GameMeanValue();
        public readonly GameMeanValue Depth = new GameMeanValue();
        public readonly GameMeanValue Nodes = new GameMeanValue();
        public readonly GameMeanValue NPS = new GameMeanValue();

        public EngineStatisticsForGame() {
            TimeReal.MedianThreshold = 1.0;
            TimeUSI.MedianThreshold = 1.0;
            Depth.MedianThreshold = 1.0;
            Nodes.MedianThreshold = 1.0;
            NPS.MedianThreshold = 0.3;
        }

        /// <summary>
        /// 手番の終了時に呼び出す
        /// </summary>
        /// <param name="player">エンジン</param>
        /// <param name="timeReal">実測時間（ミリ秒）</param>
        public void Add(USIPlayer player, double? timeReal) {
            TimeReal.Values.Add(timeReal);
            TimeUSI.Values.Add(player.LastTime);
            Depth.Values.Add(player.LastDepth);
            Nodes.Values.Add(player.LastNodes);
            NPS.Values.Add(player.LastNPS);
        }

        /// <summary>
        /// 平均値などを算出する。
        /// </summary>
        /// <remarks>
        /// 対局終了時に呼び出す。
        /// </remarks>
        public void Calculate() {
            TimeReal.Calculate();
            TimeUSI.Calculate();
            Depth.Calculate();
            Nodes.Calculate();
            NPS.Calculate();
        }
    }
}
