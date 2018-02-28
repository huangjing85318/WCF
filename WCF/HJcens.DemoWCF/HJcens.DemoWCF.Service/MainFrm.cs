using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;
using System.ServiceModel.Web;

namespace HJcens.DemoWCF.Service
{
    public partial class MainFrm : Form
    {
        private bool isServerRun = true;
        private int ServerPort = 8800;

        #region 窗体事件
        public MainFrm()
        {
            InitializeComponent();
        }
        private void MainFrm_Load(object sender, EventArgs e)
        {
            StartServer();//开启WCF服务
        }


        #endregion

        #region 服务器线程、获取本机IP、设置显示内容
        
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
                    ServiceHost host = new ServiceHost(typeof(HJcens.DemoWCF.ServiceLibrary.Service1), new Uri(tcpaddress));
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
                    host.AddServiceEndpoint(typeof(HJcens.DemoWCF.ServiceLibrary.IService1), tcpBinding, tcpaddress);

                    #region 添加行为
                    //元数据发布行为
                    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    //支持get请求
                    behavior.HttpGetEnabled = true;
                    behavior.HttpGetUrl = new Uri(string.Format("http://{0}:{1}/DataService", ip, Convert.ToInt32(ServerPort) + 1));
                    //设置到Host中
                    host.Description.Behaviors.Add(behavior);
                    #endregion

                    host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
                    //启动服务
                    host.Open();
                    SetDisplayMessage(string.Format("服务启动成功,正在运行...\r\n{0}\r\n{1}", tcpaddress, behavior.HttpGetUrl));



                    //Restful
                    string httpaddress = string.Format("http://{0}:{1}/", ip, ServerPort+2);
                    WebServiceHost host2 = new WebServiceHost(typeof(WebService), new Uri(httpaddress));
                    //启动服务
                    host2.Open();
                    SetDisplayMessage(string.Format("服务启动成功,正在运行...\r\n{0}", httpaddress));
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
        #endregion

    }
}
