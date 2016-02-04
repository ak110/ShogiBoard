using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShogiBoard {
    /// <summary>
    /// Windows.Form関連の処理など
    /// </summary>
    static class FormUtility {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 例外起きても大丈夫なInvoke（手抜き）
        /// </summary>
        public static void SafeInvoke(Control control, Action action) {
            try {
                if (!control.Created || control.IsDisposed) return;
                control.Invoke(new MethodInvoker(() => {
                    try {
                        if (!control.Created || control.IsDisposed) return;
                        action();
                    } catch (Exception e) {
                        logger.Warn(e);
                    }
                }));
            } catch (Exception e) {
                logger.Warn(e);
            }
        }
    }
}
