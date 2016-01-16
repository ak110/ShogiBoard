using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ShogiBoard {
    /// <summary>
    /// コンフィグを非同期読み込みするためのクラス
    /// </summary>
    class ConfigLoader {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// コンフィグ読み込み失敗イベント
        /// </summary>
        public class ConfigLoadFailedEventArgs : EventArgs {
            public string ConfigFileName { get; set; }
            public bool ToBeQuit { get; set; }
        }

        /// <summary>
        /// コンフィグ読み込み失敗イベント
        /// </summary>
        public event EventHandler<ConfigLoadFailedEventArgs> ConfigLoadFailed;

        object syncObject = new object();
        EngineList engineList = null;
        Config config = null;
        VolatileConfig volatileConfig = null;

        /// <summary>
        /// 読み込み開始
        /// </summary>
        public void StartThread() {
            ThreadPool.QueueUserWorkItem(OnThread);
        }

        /// <summary>
        /// 読み込み処理
        /// </summary>
        private void OnThread(object arg) {
            lock (syncObject) {
                try {
                    // Engine.xmlの読み込み
                    try {
                        engineList = ConfigSerializer.Deserialize<EngineList>();
                        engineList.Engines.Sort();
                    } catch (Exception ex) {
                        logger.Warn("Engine.xmlの読み込みに失敗。", ex);
                        ConfigLoadFailed(this, new ConfigLoadFailedEventArgs { ConfigFileName = "Engine.xml", ToBeQuit = true });
                        return;
                    }
                    // ShogiBoard.xmlの読み込み
                    try {
                        config = ConfigSerializer.Deserialize<Config>();
                        config.Ready();
                    } catch (Exception ex) {
                        logger.Warn("ShogiBoard.xmlの読み込みに失敗。", ex);
                        ConfigLoadFailed(this, new ConfigLoadFailedEventArgs { ConfigFileName = "ShogiBoard.xml", ToBeQuit = true });
                        return;
                    }
                    // ShogiBoard.Volatile.xmlの読み込み
                    try {
                        volatileConfig = ConfigSerializer.Deserialize<VolatileConfig>();
                    } catch (Exception ex) {
                        logger.Warn("ShogiBoard.Volatile.xmlの読み込みに失敗。", ex);
                        ConfigLoadFailed(this, new ConfigLoadFailedEventArgs { ConfigFileName = "ShogiBoard.Volatile.xml", ToBeQuit = false });
                        volatileConfig = new VolatileConfig(); // 初期値にしてしまう
                    }
                    volatileConfig.Ready();
                } catch (Exception ex) {
                    logger.Warn("ShogiBoard.Volatile.xmlの読み込みに失敗。", ex);
                }

                Monitor.PulseAll(syncObject);
            }
        }

        /// <summary>
        /// 取得
        /// </summary>
        public EngineList EngineList {
            get {
                if (engineList == null) {
                    lock (syncObject) {
                        while (engineList == null) {
                            Monitor.Wait(syncObject);
                        }
                    }
                }
                return engineList;
            }
        }

        /// <summary>
        /// 取得
        /// </summary>
        public Config Config {
            get {
                if (config == null) {
                    lock (syncObject) {
                        while (config == null) {
                            Monitor.Wait(syncObject);
                        }
                    }
                }
                return config;
            }
        }

        /// <summary>
        /// 取得
        /// </summary>
        public VolatileConfig VolatileConfig {
            get {
                if (volatileConfig == null) {
                    lock (syncObject) {
                        while (volatileConfig == null) {
                            Monitor.Wait(syncObject);
                        }
                    }
                }
                return volatileConfig;
            }
        }

        /// <summary>
        /// 検討用のエンジンが存在するかチェック
        /// </summary>
        public bool IsThinkEngineExists() {
            return EngineList.EngineExists(VolatileConfig.ThinkEngineName, VolatileConfig.ThinkEnginePath);
        }

        /// <summary>
        /// 詰将棋解答用のエンジンが存在するかチェック
        /// </summary>
        public bool IsMateEngineExists() {
            return EngineList.EngineExists(VolatileConfig.MateEngineName, VolatileConfig.MateEnginePath);
        }
    }
}
