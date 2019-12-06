using SimpleClientServer.mainController;
using System.Text;

namespace SimpleClientServer._object {
    public class Buffer {
        private const byte MAXCHARINMESSAGE = 255;
        private byte[] buffer = new byte[MAXCHARINMESSAGE];
        private MainController mainController;
        public Buffer(MainController p_mainController) {
            this.mainController = p_mainController;
        }
        public byte[] BufferByte {
            get { return this.buffer; }
            set {
                this.buffer = value;
                // TODO : trigger event
                setBuffer_Event();
            }
        }
        private string convertMessage() {
            // array converter ASCII
            ASCIIEncoding aEncoding = new ASCIIEncoding();
            // get char from byte got
            return aEncoding.GetString(this.buffer);
        }
        public void setBuffer_Event() {
            mainController.displayMessageInForm(convertMessage());
        }
    }
}