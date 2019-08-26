/** ****************************************************************************
 * Autor 				: Loris Levêque
 * Creation Date 		: 25.08.2019
 * Modification Date : dd.mm.jjjj
 * Place 				: ETML - Lausanne - CH
 * Description			: Main code of the application
 * 
 ** ****************************************************************************/

using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using SimpleClientServer.Mainform;

namespace SimpleClientServer.MainController {
	class MainController {
		// Private variables
		private MainForm mainForm;
		private Socket socket;
		private EndPoint epLocal, epRemote;
		private byte[] buffer;
		private string localIp, localPort, remotePort;
		// private consts
		private const byte MAXCHAR = 255;
		// public variables
		// public consts

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_mainForm">Form to attache the program</param>
		public Main(MainForm p_mainForm) {
			this.mainForm = p_mainForm;
		}

		/// <summary>
		/// set local variables 
		/// </summary>
		public void getObjects() {
			this.localIp = getLocalIp();
			this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			this.localPort = "1000";
			this.remotePort = "1001";
		}

		/// <summary>
		/// get local ip if there is an ip
		/// give 127.0.0.1 if there is no ip
		/// </summary>
		private getLocalIp() {
			//crée une variable
         IPHostEntry host;
         // get dns ip
         host = Dns.GetHostEntry(Dns.GetHostName());
         // try to get local ip
         foreach (IPAddress ip in host.AddressList)
         {
            // if the pc got an ip
            if (ip.AddressFamily == AddressFamily.InterNetwork)
               return ip.ToString();
         }
         // else, return local ip
         return "127.0.0.1";
		}

		/// <summary>
		/// Connect From local ip and port and remote ip and port
		/// </summary>
		/// <param name="p_localPort">local port to use</param>
		/// <param name="p_remoteIp">remote ip to connect</param>
		/// <param name="p_remotePort">remote port to use</param>
		public void Connect(p_localPort, p_remoteIp, p_remotePort) {
			// get object of local network
			this.epLocal = new IPEndPoint(IPAddress.Parse(this.localIp), Convert.ToInt32(p_localPort));
			this.socket.Bind(epLocal);
			// get object pf remoted network
         this.epRemote = new IPEndPoint(IPAddress.Parse(p_remoteIp), Convert.ToInt32(p_remotePort));
         this.socket.Connect(epRemote);
         // set buffer
         this.buffer = new byte[MAXCHARINMESS];
         // start listening
         this.socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
		}
      /// <summary>
      /// 
      /// </summary>
      private string MessageCallBack(IAsyncResult aResult)
      {
         try
         {
            // set an message array
            byte[] receivedData = new byte[MAXCHARINMESS];
            // get data [format byte]
            receivedData = (byte[])aResult.AsyncState;
            // array converter ASCII
            ASCIIEncoding aEncoding = new ASCIIEncoding();
            // get char from byte got
            string receivedMessage = aEncoding.GetString(receivedData);
            // reset the buffer
            buffer = new byte[MAXCHARINMESS];
            // restart listening
            sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
            return receivedMessage;
         }
         catch (Exception ex)
         {
            // show the error
            MessageBox.Show(ex.ToString());
            return "error!";
         }
      }        
	}
}