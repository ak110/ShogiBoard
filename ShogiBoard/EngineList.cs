using ShogiCore.USI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShogiBoard {
    /// <summary>
    /// エンジンのリスト
    /// </summary>
    [ConfigSerializeFileName("Engine.xml")]
    [XmlType("engineList")]
    public class EngineList {
        /// <summary>
        /// エンジンのリスト
        /// </summary>
        [XmlElement("engine")]
        public List<Engine> Engines { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        public EngineList() {
            Engines = new List<Engine>();
        }

        /// <summary>
        /// エンジン名とパスからエンジンを特定する。失敗時null。
        /// </summary>
        public Engine Select(string name, string path) {
            // name, path
            {
                var list = Engines.Where(x => x.Name == name && x.Path == path);
                int count = list.Count();
                if (1 < count) return null; // 1つに特定出来なければnull。
                if (count == 1) return list.First(); // 1つに特定出来たならそれを返却。
            }
            // name
            {
                var list = Engines.Where(x => x.Name == name);
                int count = list.Count();
                if (1 < count) return null; // 1つに特定出来なければnull。
                if (count == 1) return list.First(); // 1つに特定出来たならそれを返却。
            }
            return null;
        }

        /// <summary>
        /// エンジンが存在するかチェック
        /// </summary>
        public bool EngineExists(string name, string path) {
            var engine = Select(name, path);
            return engine != null && File.Exists(USIDriver.NormalizeEnginePath(engine.Path));
        }
    }
}
