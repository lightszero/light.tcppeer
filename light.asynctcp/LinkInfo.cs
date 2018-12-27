using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace light.asynctcp
{
    class LinkInfo
    {
        public Socket Socket;
        public long Handle;
        public DateTime ActiveDateTime;
        public DateTime ConnectDateTime;
    }
}
