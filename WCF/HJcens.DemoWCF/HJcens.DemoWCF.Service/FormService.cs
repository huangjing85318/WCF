using HJcens.DemoWCF.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HJcens.DemoWCF.Service
{
    public partial class FormService : Form
    {
        private bool isServerRun = true;
        private int ServerPort = 9900;
        private static FormService instance = null;
        public List<ICallBackServices> ListClient = new List<ICallBackServices>();

        #region 窗体事件
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static FormService GetInstance()
        {
            if (instance == null)
            {
                instance = new FormService();
            }
            return instance;
        }
        private FormService()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体刚加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrm_Load(object sender, EventArgs e)
        {
            StartServer();//开启WCF服务
            SetClientNum();
        }
        #endregion

        #region 服务器线程、获取本机IP、设置显示内容、设置在线客户量       
        /// <summary>
        ///  wcf 宿主服务线程
        /// </summary>
        /// <summary>
        /// 启动wcf服务
        /// </summary>
        private void StartServer()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    isServerRun = true;
                    //获取本机IP
                    string ip = GetIP();
                    string tcpaddress = string.Format("net.tcp://{0}:{1}/", ip, ServerPort);
                    //定义服务主机
                    ServiceHost host = new ServiceHost(typeof(MessageService), new Uri(tcpaddress));
                    //设置netTCP协议
                    NetTcpBinding tcpBinding = new NetTcpBinding();
                    tcpBinding.MaxBufferPoolSize = 2147483647;
                    tcpBinding.MaxReceivedMessageSize = 2147483647;
                    tcpBinding.MaxBufferSize = 2147483647;
                    //提供安全传输
                    tcpBinding.Security.Mode = SecurityMode.None;
                    //需要提供证书
                    tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
                    //添加多个服务终结点
                    //使用指定的协定、绑定和终结点地址将服务终结点添加到承载服务中
                    //netTcp协议终结点
                    host.AddServiceEndpoint(typeof(IMessageService), tcpBinding, tcpaddress);

                    #region 添加行为
                    //元数据发布行为
                    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    //支持get请求
                    behavior.HttpGetEnabled = false;

                    //设置到Host中
                    host.Description.Behaviors.Add(behavior);
                    #endregion

                    //设置数据交换类型
                    host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
                    //启动服务
                    host.Open();
                    SetDisplayMessage(string.Format("服务启动成功,正在运行...\r\n{0}", tcpaddress));
                }
                catch (Exception ex)
                {
                    SetDisplayMessage("服务启动失败");
                    MessageBox.Show(ex.Message, "服务启动失败，请检测配置中IP地址");
                    Environment.Exit(0);
                }
            });
        }

        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns></returns>
        public string GetIP()
        {
            string name = Dns.GetHostName();
            IPHostEntry me = Dns.GetHostEntry(name);
            IPAddress[] ips = me.AddressList;
            foreach (IPAddress ip in ips)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return (ips != null && ips.Length > 0 ? ips[0] : new IPAddress(0x0)).ToString();
        }

        /// <summary>
        /// 设置运行内容
        /// </summary>
        /// <param name="msg"></param>
        public void SetDisplayMessage(string msg)
        {
            //在线程里以安全方式调用控件
            if (this.rtxtMsg.InvokeRequired)
            {
                rtxtMsg.BeginInvoke(new MethodInvoker(delegate
                {
                    SetDisplayMessage(msg);
                }));
            }
            else
            {
                if (rtxtMsg.Lines.Length >= 200)
                {
                    rtxtMsg.Text = "";
                }
                rtxtMsg.AppendText(string.Format("{0}\r\n{1}\r\n\r\n", msg, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                rtxtMsg.SelectionStart = rtxtMsg.Text.Length;
                rtxtMsg.ScrollToCaret();
            }
        }

        /// <summary>
        /// 设置在线客户端数量
        /// </summary>
        public void SetClientNum()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate {
                    SetClientNum();
                }));
            }
            else
            {
                this.Text = string.Format("服务已启动，当前客户端数量{0}", ListClient.Count);
            }
        }
        #endregion

        #region 发布事件
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            string message = this.txtMessage.Text;
            foreach (ICallBackServices icbs in ListClient)
            {
                icbs.SendMessage(message);
            }
        }
        #endregion
    }
}
