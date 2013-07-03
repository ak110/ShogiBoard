using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ShogiBoard {
    static class Program {
        static log4net.ILog logger = null;

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main() {
            try {
                // log4netのためにBlunder.Core.dllを読み込む
                GC.KeepAlive(new ShogiCore.Diagnostics.Log4netLazyMinimalLock());
                // log4net初期化
                log4net.Config.XmlConfigurator.Configure();

                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.ThreadException += Application_ThreadException;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            } catch (Exception e) {
                WriteLog("例外発生(Main)。", e);
                MessageBox.Show(e.Message, "異常終了");
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            WriteLog("例外発生(CurrentDomain_UnhandledException)。", e.ExceptionObject as Exception);
            Exception ex = e.ExceptionObject as Exception;
            MessageBox.Show(ex == null ? Convert.ToString(e.ExceptionObject) : ex.Message,
                e.IsTerminating ? "異常終了" : "エラー");
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
            WriteLog("例外発生(Application_ThreadException)。", e.Exception);
            MessageBox.Show(e.Exception.Message, "エラー");
        }

        static void WriteLog(string msg, Exception e) {
            if (logger == null) {
                string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                string log = Path.Combine(logDir, "Error.log");
                Directory.CreateDirectory(logDir);
                File.AppendAllText(log, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + " [ERROR] " + msg + Environment.NewLine + e.ToString() + Environment.NewLine);
            } else {
                logger.Error(msg, e);
            }
        }
    }
}
