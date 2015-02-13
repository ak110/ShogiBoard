using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShogiBoard {
    /// <summary>
    /// 対局してるエンジンの統計情報（複数対局分の平均値）
    /// </summary>
    public class EngineStatisticsForAllGames {
        /// <summary>
        /// 平均値を算出するためのクラス
        /// </summary>
        public class MeanValue {
            double sum = 0.0;
            long count = 0;

            /// <summary>
            /// 値を追加する（nullなら無視）
            /// </summary>
            /// <param name="value">値</param>
            public void Add(double? value) {
                if (value.HasValue) {
                    sum += value.Value;
                    count++;
                }
            }

            /// <summary>
            /// 平均値を返す
            /// </summary>
            public double? Mean {
                get {
                    if (count == 0)
                        return null;
                    return sum / count;
                }
            }
        }
        /// <summary>
        /// 全体、序盤、終盤の平均値を算出するためのクラス
        /// </summary>
        public class MeanValueSet {
            public readonly MeanValue All = new MeanValue();
            public readonly MeanValue Opening = new MeanValue();
            public readonly MeanValue EndGame = new MeanValue();
            /// <summary>
            /// 値を追加
            /// </summary>
            /// <param name="result"></param>
            public void Add(GameMeanValue.ResultValues result) {
                All.Add(result.MeanOfAll);
                Opening.Add(result.MeanOfOpening);
                EndGame.Add(result.MeanOfEndGame);
            }
        }

        public readonly MeanValueSet TimeReal = new MeanValueSet();
        public readonly MeanValueSet TimeUSI = new MeanValueSet();
        public readonly MeanValueSet Depth = new MeanValueSet();
        public readonly MeanValueSet Nodes = new MeanValueSet();
        public readonly MeanValueSet NPS = new MeanValueSet();

        /// <summary>
        /// 1局分のデータを追加
        /// </summary>
        public void Add(EngineStatisticsForGame stat) {
            TimeReal.Add(stat.TimeReal.Result);
            TimeUSI.Add(stat.TimeUSI.Result);
            Depth.Add(stat.Depth.Result);
            Nodes.Add(stat.Nodes.Result);
            NPS.Add(stat.NPS.Result);
        }
    }
}
