using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace SimpleClientServer.mainController
{
    class MainController
    {
        // private variables
        private Ip ip = new Ip();
        private Port port = new Port();
        private Buffer buffer = new Buffer(this);
        private MainForm mainForm;
        private Socket socket;
        private EndPoint epLocal, epRemote;
        // private consts
        private const byte MAXCHARINMESSAGE = 255;
        // public variables
        // public consts

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_mainForm">Form to attache the program</param>
        public MainController(MainForm p_mainForm)
        {
            epLocal = epRemote = null;
            this.mainForm = p_mainForm;
        }

        /// <summary>
        /// set local variables 
        /// </summary>
        public void getObjects()
        {
            ip.Local = ip.getLocalIp();
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            // default port used -> will not be checked for tcp connection
            this.port.Local = "1000";
            this.port.Remote = "1001";
        }
        /// <param name="p_localPort">local port to use</param>
        public void setLocalPort(String p_localPort) {
            this.port.Local = p_localPort;
        }
        /// <param name="p_remotePort">remote port to use</param>
        public void setRemotePort(String p_remotePort) {
            this.port.Remote = p_remotePort;
        }
        /// <param name="p_remoteIp">remote ip to connect</param>
        public void setRemoteIp(String p_remoteIp) {
            this.ip.Remote = p_remoteIp;
        }

        /// <summary>
        /// Connect From local ip and port and remote ip and port
        /// </summary>
        public void fullConnect() {
            setConnectVariables();
            bindLocalEndPoint();
            if (canConnect()) {
                connect();
                startListening();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void MessageCallBack(IAsyncResult aResult)
        {
            try
            {
                tryReceiveMessage();
                startListening();
            }
            catch (Exception ex)
            {
                displayMessage(ex.ToString());
            }
        }
        public void displayMessageInForm(String p_message) {
            // TODO : add item into form
            mainForm.displayMessage(p_message);
        }
        private bool canConnect() {
            if (epLocal != null && epRemote != null)
                return true;
            else
                return false;
        }
        private void connect() {
            this.socket.Connect(epRemote);
        }
        private void setConnectVariables() {
            // get object of local network
            this.epLocal = new IPEndPoint(IPAddress.Parse(ip.Local), Convert.ToInt32(port.Local));
            // get object pf remoted network
            this.epRemote = new IPEndPoint(IPAddress.Parse(ip.Remote), Convert.ToInt32(port.Remote));
        }
        private void bindLocalEndPoint() {
            this.socket.Bind(epLocal);
        }
        private void tryReceiveMessage() {
            byte[] receivedData = new byte[MAXCHARINMESSAGE];
            // format byte
            receivedData = (byte[])aResult.AsyncState;
            // TODO : trigger event on this buffer set
            //buffer = receivedMessage;
        }
        private void startListening() {
            // restart listening
            this.socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
        }
        private void displayMessage(String message) {
            MessageBox.Show(message);
        }
    }
}