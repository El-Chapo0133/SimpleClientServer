namespace SimpleClientServer.Ip {
    public class Ip {
        private string local, remote;
        public Ip() {
            this.local = this.remote = "";
        }
        public string Local {
            get { return this.local; }
            set { this.local = value; }
        }
        public string Remote {
            get { return this.remote; }
            set { this.remote = value; }
        }
        /// <summary>
        /// get local ip if there is an ip
        /// give 127.0.0.1 if there is no ip
        /// </summary>
        public string getLocalIp() {
            IPHostEntry host;
            // get dns ip
            host = Dns.GetHostEntry(Dns.GetHostName());
            // try to get local ip
            foreach (IPAddress ip in host.AddressList)
            {
                // if the pc got an ip
                if (isOurIp(ip))
                    return ip.ToString();
            }
            // else, return local ip
            return "127.0.0.1";
        }
        private bool isOurIp(IPAddress ip) {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return true;
            else
                return false;
        }
    }
}