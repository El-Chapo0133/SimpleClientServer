namespace SimpleClientServer._object {
    public class Port {
        private string local, remote;
        public Port() {
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
    }
}