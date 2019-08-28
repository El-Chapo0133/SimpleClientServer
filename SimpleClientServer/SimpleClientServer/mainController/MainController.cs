using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace SimpleClientServer.mainController
{
    class MainController
    {
        // Private variables
        private Ip ip = new Ip();
        private Port port = new Port();
        private MainForm mainForm;
        private Socket socket;
        private EndPoint epLocal, epRemote;
        private byte[] buffer;
        // private consts
        private const byte MAXCHAR = 255;
        // public variables
        // public consts

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_mainForm">Form to attache the program</param>
        public MainController(MainForm p_mainForm)
        {
            this.mainForm = p_mainForm;
        }

        /// <summary>
        /// set local variables 
        /// </summary>
        public void getObjects()
        {
            this.localIp = ip.getLocalIp();
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            port.Local = "1000";
            port.Remote = "1001";
        }

        /// <summary>
        /// Connect From local ip and port and remote ip and port
        /// </summary>
        /// <param name="p_localPort">local port to use</param>
        /// <param name="p_remoteIp">remote ip to connect</param>
        /// <param name="p_remotePort">remote port to use</param>
        public void Connect(String p_localPort, String p_remoteIp, String p_remotePort)
        {
            // get object of local network
            this.epLocal = new IPEndPoint(IPAddress.Parse(ip.Local), Convert.ToInt32(port.Local));
            this.socket.Bind(epLocal);
            // get object pf remoted network
            this.epRemote = new IPEndPoint(IPAddress.Parse(ip.Remote), Convert.ToInt32(port.Remote));
            this.socket.Connect(epRemote);
            // set buffer
            this.buffer = new byte[MAXCHAR];
            // start listening
            this.socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
        }
        /// <summary>
        /// 
        /// </summary>
        private void MessageCallBack(IAsyncResult aResult)
        {
            try
            {
                // set an message array
                byte[] receivedData = new byte[MAXCHAR];
                // get data [format byte]
                receivedData = (byte[])aResult.AsyncState;
                // array converter ASCII
                ASCIIEncoding aEncoding = new ASCIIEncoding();
                // get char from byte got
                string receivedMessage = aEncoding.GetString(receivedData);
                // reset the buffer
                this.buffer = new byte[MAXCHAR];
                // restart listening
                this.socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
                //return receivedMessage;
            }
            catch (Exception ex)
            {
                // show the error
                MessageBox.Show(ex.ToString());
                //return "error!";
            }
        }
    }
}