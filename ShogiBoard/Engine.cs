using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShogiBoard {
    /// <summary>
    /// USIエンジン
    /// </summary>
    public class Engine : ICloneable, IComparable<Engine> {
        /// <summary>
        /// option
        /// </summary>
        public class Option {
            /// <summary>
            /// 名前
            /// </summary>
            [XmlElement("name")]
            public string Name { get; set; }
            /// <summary>
            /// 値
            /// </summary>
            [XmlElement("value")]
            public string Value { get; set; }
        }

        /// <summary>
        /// エンジン名
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }
        /// <summary>
        /// 作者名
        /// </summary>
        [XmlElement("author")]
        public string Author { get; set; }
        /// <summary>
        /// 実行ファイルのパス
        /// </summary>
        [XmlElement("path")]
        public string Path { get; set; }

        /// <summary>
        /// USI_Ponder
        /// </summary>
        [XmlElement("usiPonder")]
        public bool USIPonder { get; set; }
        /// <summary>
        /// USI_Hash
        /// </summary>
        [XmlElement("usiHash")]
        public int USIHash { get; set; }
        /// <summary>
        /// setoption
        /// </summary>
        [XmlElement("options")]
        public List<Option> Options { get; set; }

        public Engine() {
            USIPonder = true;
            USIHash = 128;
            Options = new List<Option>();
        }

        /// <summary>
        /// ドロップダウンリスト表示など用文字列化
        /// </summary>
        public override string ToString() {
            int len = Encoding.GetEncoding(932).GetByteCount(Name);
            string pad = len < 32 ? new string(' ', 32 - len) : "";
            return Name + pad + " <" + Path + ">";
        }

        public Engine Clone() {
            Engine copy = (Engine)MemberwiseClone();
            copy.Options = new List<Option>(Options.ToArray());
            return copy;
        }

        object ICloneable.Clone() {
            return Clone();
        }

        /// <summary>
        /// ソート用比較演算
        /// </summary>
        public int CompareTo(Engine other) {
            int c1 = string.CompareOrdinal(Name, other.Name);
            if (c1 != 0) return c1;
            return string.CompareOrdinal(Path, other.Path);
        }
    }
}
