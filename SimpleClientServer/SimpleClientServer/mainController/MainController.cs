using SimpleClientServer._object;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Buffer = SimpleClientServer._object.Buffer;

namespace SimpleClientServer.mainController
{
    public class MainController
    {
        // private consts
        private const byte MAXCHARINMESSAGE = 255;
        private const String XMLCONTROLSFILEPATH = "../../_resources/formControls.xml";
        // private variables
        private Ip ip = new Ip();
        private Port port = new Port();
        private Buffer buffer;
        private MainForm mainForm;
        private Socket socket;
        private EndPoint epLocal, epRemote;
        // public variables
        // public consts

        /*
         * *************************************************************
         * * Public functions
         */
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_mainForm">Form to attache the program</param>
        public MainController(MainForm p_mainForm)
        {
            epLocal = epRemote = null;
            this.mainForm = p_mainForm;
            this.buffer = new Buffer(this);

            this.mainForm.mainController = this;
        }

        /// <summary>
        /// set local variables 
        /// </summary>
        public void getObjects()
        {
            ip.Local = ip.getLocalIp();
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            // default port usobjected -> will not be checked for tcp connection
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
        /// Receive message and restart listen
        /// </summary>
        private void MessageCallBack(IAsyncResult aResult)
        {
            try
            {
                tryReceiveMessage(aResult);
                startListening();
            }
            catch (Exception ex)
            {
                displayMessage(ex.ToString());
            }
        }
        public String convertStringMessage(byte[] p_message) {
            // array converter ASCII
            ASCIIEncoding aEncoding = new ASCIIEncoding();
            // get char from byte got
            return aEncoding.GetString(p_message);
        }
        public byte[] convertByteMessage(string p_message) {
            // array converter ASCII
            ASCIIEncoding aEncoding = new ASCIIEncoding();
            // get bytes from the string got
            return aEncoding.GetBytes(p_message);
        }
        public void displayMessageInForm(string p_message) {
            this.mainForm.addMessage(this.ip.Remote + ": " + p_message);
        }
        /*
         * *************************************************************
         * * Private functions
         */
        private bool canConnect() {
            if (epLocal != null && epRemote != null)
                return true;
            else
                return false;
        }
        private void connect() {
            this.socket.Connect(epRemote);
        }
        public void SendMessage(string p_message)
        {
            // get a sendable message from string
            byte[] converted_message = this.convertByteMessage(p_message);
            // send the message from the socket
            this.socket.Send(converted_message);
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
        private void tryReceiveMessage(IAsyncResult aResult) {
            byte[] receivedData = new byte[MAXCHARINMESSAGE];
            // format byte
            receivedData = (byte[])aResult.AsyncState;
            // TODO : trigger event on this buffer set
            this.buffer.BufferByte = receivedData;
        }
        private void startListening() {
            // restart listening
            this.socket.BeginReceiveFrom(buffer.BufferByte, 0, buffer.BufferByte.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer.BufferByte);
        }
        private void displayMessage(String message) {
            MessageBox.Show(message);
        }
    }
}