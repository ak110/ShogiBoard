using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShogiBoard.Exceptions {
    /// <summary>
    /// USIエンジン起動失敗など用の例外
    /// </summary>
    [Serializable]
    public class USIEngineException : ApplicationException {
        public USIEngineException() { }
        public USIEngineException(string message) : base(message) { }
        public USIEngineException(string message, Exception inner) : base(message, inner) { }
        protected USIEngineException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
