using System;

namespace Tools.Core
{
    [Serializable]
    public class InvokeResult
    {
        #region 公共属性
        public bool Result { get; protected set; }

        public string Message { get; protected set; }
        #endregion


        #region 私有方法
        private void SetResult(bool state, string message = "")
        {
            Result = state;
            Message = message;
        }
        #endregion


        #region 公共方法
        public void Success(string message = "")
        {
            SetResult(true, message);
        }

        public void Fail(string message = "")
        {
            SetResult(false, message);
        }
        #endregion
    }

    [Serializable]
    public class InvokeResult<T> : InvokeResult
    {
        public T Data { get; set; }

        public void Success(T value = default(T), string message = "")
        {
            Data = value;
            Success(message);
        }
    }
}
