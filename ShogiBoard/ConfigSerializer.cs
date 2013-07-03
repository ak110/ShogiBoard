using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShogiBoard {
    /// <summary>
    /// ConfigSerializerで使用するファイル名を指定するための属性
    /// </summary>
    class ConfigSerializeFileNameAttribute : Attribute {
        /// <summary>
        /// ConfigSerializerで使用するファイル名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// ConfigSerializerで使用するファイル名を指定する
        /// </summary>
        /// <param name="fileName">ConfigSerializerで使用するファイル名</param>
        public ConfigSerializeFileNameAttribute(string fileName) {
            FileName = fileName;
        }
    }
    /// <summary>
    /// XmlSerializerを簡単にラッピングしたもの
    /// </summary>
    class ConfigSerializer {
        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="fileName">ファイル名。「Engine.xml」など。</param>
        /// <returns>読み込んだインスタンス</returns>
        public static T Deserialize<T>() where T : new() {
            string path = GetFilePath<T>();
            if (!File.Exists(path)) {
                return new T();
            }
            using (FileStream stream = File.OpenRead(path)) {
                return (T)new XmlSerializer(typeof(T)).Deserialize(stream);
            }
        }

        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="fileName">ファイル名。「Engine.xml」など。</param>
        /// <param name="config">書き込むオブジェクト</param>
        public static void Serialize<T>(T config) {
            using (FileStream stream = File.Create(GetFilePath<T>())) {
                new XmlSerializer(config.GetType()).Serialize(stream, config);
            }
        }

        /// <summary>
        /// 型からファイルパスを取得
        /// </summary>
        private static string GetFilePath<T>() {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GetFileName<T>());
            return path;
        }

        /// <summary>
        /// 型からファイル名を取得
        /// </summary>
        private static string GetFileName<T>() {
            return typeof(T)
                .GetCustomAttributes(typeof(ConfigSerializeFileNameAttribute), true)
                .Cast<ConfigSerializeFileNameAttribute>()
                .First()
                .FileName;
        }
    }
}
