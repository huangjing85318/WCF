using HJcens.DemoWCF.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HJcens.DemoWCF.Service
{
    /// <summary>
    ///  实例使用Single，共享一个
    ///  并发使用Mutiple, 支持多线程访问(一定要加锁)
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MessageService : IMessageService
    {
        #region 客户端注册、客户端发送消息、客户端关闭事件
        /// <summary>
        /// 客户端注册
        /// </summary>
        public void Register()
        {
            ICallBackServices client = OperationContext.Current.GetCallbackChannel<ICallBackServices>();
            string sessionid = OperationContext.Current.SessionId;//获取当前机器Sessionid--------------------------如果多个客户端在同一台机器，就使用此信息。
            string ClientHostName = OperationContext.Current.Channel.RemoteAddress.Uri.Host;//获取当前机器名称-----多个客户端不在同一台机器上，就使用此信息。
            FormService.GetInstance().SetDisplayMessage(string.Format("客户端上线:\r\n{0}\r\n{1}", sessionid, ClientHostName));
            FormService.GetInstance().ListClient.Add(client);
            OperationContext.Current.Channel.Closed += new EventHandler(Channel_Closed);
            FormService.GetInstance().SetClientNum();
        }

        /// <summary>
        /// 客户端发送消息
        /// </summary>
        /// <param name="message"></param>
        public void ClientSendMessage(string message)
        {
            ICallBackServices client = OperationContext.Current.GetCallbackChannel<ICallBackServices>();
            string sessionid = OperationContext.Current.SessionId;//获取当前机器Sessionid--------------------------如果多个客户端在同一台机器，就使用此信息。
            string ClientHostName = OperationContext.Current.Channel.RemoteAddress.Uri.Host;//获取当前机器名称-----多个客户端不在同一台机器上，就使用此信息。
            FormService.GetInstance().SetDisplayMessage(string.Format("客户端:{0}\r\n{1}\r\n{2}", message, sessionid, ClientHostName));
            client.SendMessage("你好，我是服务器");
        }

        /// <summary>
        /// 客户端关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Channel_Closed(object sender, EventArgs e)
        {
            ICallBackServices client = sender as ICallBackServices;
            FormService.GetInstance().ListClient.Remove(client);
            FormService.GetInstance().SetClientNum();
        }
        #endregion
    }
}
