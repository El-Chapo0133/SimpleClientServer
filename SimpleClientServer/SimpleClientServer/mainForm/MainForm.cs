using SimpleClientServer.mainController;
using System;
using System.Windows.Forms;

namespace SimpleClientServer
{
    public partial class MainForm : Form
    {
        public MainController mainController;
        private const byte MAXCHARINMESSAGE = 255;

        public MainForm()
        {
            InitializeComponent();
        }
        public void addMessage(string p_message)
        {
            this.listBoxMessages.Items.Add(p_message);
        }
        /// <summary>
        /// Set maincontroller's variables and try to connect
        /// Show message if error :)
        /// </summary>
        private void ButtonConnect_Click(object sender, System.EventArgs e)
        {
            try
            {
                // set mainController's variables
                mainController.getObjects();
                if (isIPok(this.textBoxRemoteIP.Text))
                {
                    mainController.setRemoteIp(this.textBoxRemoteIP.Text);
                }
                mainController.setLocalPort(this.textBoxLocalPort.Text);
                mainController.setRemotePort(this.textBoxRemotePort.Text);
                // connect
                mainController.fullConnect();
            }
            catch (Exception ex)
            {
                if (this.isEmpty(this.textBoxLocalPort.Text))
                {
                    this.displayMessage("Remplissez la textbox 'local port' avec le port utilisé choisi");
                }
                else if (this.isEmpty(this.textBoxRemotePort.Text))
                {
                    this.displayMessage("Remplissez la textbox 'Remote port' avec le port utilisé choisi");
                }
                else if (this.isEmpty(this.textBoxRemoteIP.Text))
                {
                    this.displayMessage("Remplissez la textbox 'Remote ip' avec l'ip de votre contact");
                }
                else
                {
                    this.displayMessage(ex.ToString());
                }
            }
        }
        private void displayMessage(string message)
        {
            MessageBox.Show(message, "error", MessageBoxButtons.OK);
        }
        /// <summary>
        /// Check if a string is empty
        /// </summary>
        /// <param name="text">input string</param>
        /// <returns>result</returns>
        private bool isEmpty(string text)
        {
            if (text == "" || text == String.Empty ||text.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Check if an ip is ok
        /// </summary>
        /// <param name="ip">string ip</param>
        /// <returns>result</returns>
        private bool isIPok(string ip)
        {
            bool result = true;
            string[] ip_split = ip.Split('.');
            foreach(string cell in ip_split)
            {
                if (Convert.ToInt32(cell) >= 255 ||Convert.ToInt32(cell) <= 0)
                {
                    result = false;
                }
            }
            if (ip_split.Length != 4)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Send a message throught the socket
        /// </summary>
        private void ButtonSend_Click(object sender, System.EventArgs e)
        {
            if (!isMessageTooLong(this.textBoxMessage.Text))
            {
                mainController.SendMessage(this.textBoxMessage.Text);
                this.addMessage("moi: " + this.textBoxMessage.Text);
                this.cleanTextBoxMessage();
            }
            else
            {
                this.displayMessage("Message trop long [MAX 255]");
            }
        }
        private void cleanTextBoxMessage()
        {
            this.textBoxMessage.Text = "";
        }
        private bool isMessageTooLong(string p_message)
        {
            if (p_message.Length > MAXCHARINMESSAGE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
