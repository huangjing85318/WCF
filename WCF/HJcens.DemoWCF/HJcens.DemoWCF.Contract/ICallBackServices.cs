using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace HJcens.DemoWCF.Contract
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ICallBackServices))]
    public interface ICallBackServices
    {
        /// <summary>
        /// 服务器向客户端发送信息(异步)
        /// </summary>
        /// <param name="Message"></param>
        [OperationContract(IsOneWay = true)]
        void SendMessage(string message);
    }
}
