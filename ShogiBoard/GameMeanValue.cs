using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShogiBoard {
    /// <summary>
    /// 1局分の情報を蓄えておいて全体・序盤・終盤の平均値を算出するためのクラス。
    /// </summary>
    /// <remarks>
    /// 平均はしきい値以上離れているものを除外して算出する。（詰み絡みなどを除外したいため）
    /// </remarks>
    public class GameMeanValue {
        /// <summary>
        /// 平均値の計算結果
        /// </summary>
        public struct ResultValues {
            public double? MeanOfAll { get; set; }
            public double? MeanOfOpening { get; set; }
            public double? MeanOfEndGame { get; set; }
        }

        /// <summary>
        /// 値の集合
        /// </summary>
        public List<double?> Values { get; private set; }
        /// <summary>
        /// 中央値からこの割合以上離れている値は除外する。初期値は0.3。（中央値から3割以上離れていれば異常値扱い）
        /// </summary>
        public double MedianThreshold { get; set; }
        /// <summary>
        /// 平均値の計算結果
        /// </summary>
        public ResultValues Result { get; private set; }

        /// <summary>
        /// 初期値を設定
        /// </summary>
        public GameMeanValue() {
            Values = new List<double?>();
            MedianThreshold = 0.3;
        }

        /// <summary>
        /// 平均値を計算して結果をまとめて返す
        /// </summary>
        public void Calculate() {
            Result = new ResultValues {
                MeanOfAll = GetMean(Values),
                MeanOfOpening = GetMean(Values.Take(Values.Count / 2)),
                MeanOfEndGame = GetMean(Values.Skip(Values.Count / 2)),
            };
        }

        /// <summary>
        /// 平均を算出。特異値の影響を避けるために中央値からmedianThreshold以上外れているものは除外して平均。
        /// </summary>
        private double? GetMean(IEnumerable<double?> list) {
            try {
                // nullを除外して要素数チェック
                var values = list.Where(x => x.HasValue).Select(x => x.Value);
                if (!values.Any()) return null;
                // 中央値を求める
                var npsOrdered = values.OrderBy(x => x);
                int c = values.Count();
                double median = c % 2 == 0 ?
                    npsOrdered.Skip(c / 2 - 1).Take(2).Average() :
                    npsOrdered.Skip(c / 2).First();
                // 中央値から±3割以上離れている値は除外して平均。偶数で中央値付近が無い場合はそのまま平均。
                double th = median * MedianThreshold;
                var meanValues = values.Where(x => Math.Abs(median - x) <= th);
                return meanValues.Any() ? meanValues.Average() : values.Average();
            } catch {
                return null;
            }
        }
    }
}
