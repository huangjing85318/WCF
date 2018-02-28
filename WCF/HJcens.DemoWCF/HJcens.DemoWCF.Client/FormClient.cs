using HJcens.DemoWCF.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;

namespace HJcens.DemoWCF.Client
{
    
    public partial class FormClient : Form, MessageService.IMessageServiceCallback
    {
        //服务器服务
        private MessageService.MessageServiceClient mService = null;

        #region 窗体事件
        public FormClient()
        {
            InitializeComponent();
        }
        private void FormClient_Load(object sender, EventArgs e)
        {
            InstanceContext context = new InstanceContext(this);
            mService = new MessageService.MessageServiceClient(context);
            mService.Register();
        }
        #endregion

        #region 收发消息、设置消息      
        /// <summary>
        /// 收到服务器消息
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            SetDisplayMessage("服务器:" + message);
        }

        /// <summary>
        /// 设置显示消息
        /// </summary>
        private void SetDisplayMessage(string message)
        {
            if (this.rtxtResult.InvokeRequired)
            {
                this.rtxtResult.BeginInvoke(new MethodInvoker(delegate
                {
                    SetDisplayMessage(message);
                }));
            }
            else
            {
                this.rtxtResult.Text += string.Format("{0}\r\n{1}\r\n", message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }

        #endregion

        #region 发送按钮
        /// <summary>
        /// 发送按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            mService.ClientSendMessage(this.textBox1.Text);
            SetDisplayMessage(this.textBox1.Text);
            this.textBox1.Text = "";
        }
        #endregion

    }
}
