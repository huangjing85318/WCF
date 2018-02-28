using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace HJcens.DemoWCF.Contract
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ICallBackServices))]
    public interface IMessageService
    {
        /// <summary>
        /// 客户端注册上线
        /// </summary>
        /// <param name="message"></param>
        [OperationContract(IsOneWay = true)]
        void Register();

        /// <summary>
        /// 客户端发送消息
        /// </summary>
        /// <param name="message">消息内容</param>
        [OperationContract(IsOneWay = true)]
        void ClientSendMessage(string message);
    }
}
