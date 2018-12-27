using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace light.asynctcp
{
    public partial class ServerModule
    {


        /// <summary>  
        /// 监听Socket，用于接受客户端的连接请求  
        /// </summary>  
        private Socket socketListen;

        public ServerModule(int capacity = 1000)
        {
            InitEventArgsPool(capacity);
            InitProcess();
        }
        //监听
        public void Listen(string ip, int port)
        {
            if (this.socketListen != null)
            {
                throw new Exception("already in listen");
            }
            socketListen = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse(ip);
            var endPoint = new IPEndPoint(ipAddress, port);
            if (endPoint.AddressFamily == AddressFamily.InterNetworkV6)
            {
                socketListen.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
                socketListen.Bind(new IPEndPoint(IPAddress.IPv6Any, endPoint.Port));
            }
            else
            {
                socketListen.Bind(endPoint);
            }
            socketListen.Listen(port);

            StartAccept(null);


        }
        void StartAccept(SocketAsyncEventArgs args)
        {
            if (args == null)
            {
                args = GetFreeEventArgs();
            }
            args.AcceptSocket = null;
            if (!socketListen.AcceptAsync(args))
            {
                ProcessAccept(args);
            }

            _maxAcceptedClients.WaitOne();

            //不断执行检查是否有无效连接
            var thread = new System.Threading.Thread(_DaemonThread);
            thread.IsBackground = true;
            thread.Start();
        }
        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                //lock (AcceptLocker)//这个线程锁要检查是否必要
                {
                    ProcessAccept(e);
                }
            }
            catch (Exception Err)
            {
                Console.WriteLine("error:" + Err.Message);
            }
        }
     
        void _DaemonThread()
        {
            while (true)
            {
                //加上超时检测代码

                for (int i = 0; i < 60 * 1000 / 10; i++) //每分钟检测一次
                {
                    //if (!m_thread.IsAlive)
                    //    break;
                    Thread.Sleep(10);
                }
            }
        }
    }
}
