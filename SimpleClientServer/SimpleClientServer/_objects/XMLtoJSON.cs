namespace SimpleClientServerLtoJSON {
    public class XMLtoJSON {
        private Json buffer;
        public XMLtoJSON() {

        }
        public Json Buffer {
            get {
                if (bufferNotEmpty()) {
                    return buffer;
                } else {
                    return null;
                }
            }
        }
        private bool bufferNotEmpty() {
            if (buffer != null) {
                return true;
            } else {
                return false;
            }
        }
        public Json Convert(XMLDocument xml) {
            // TODO : check xml
            if (isXmlRight(xml)) {
                r_json = JsonConvert.SerializeXmlNode(xml);
                buffer = r_json;
                return r_json;
            }
        }
        private bool isXmlRight(XMLDocuemnt xml) {
            if (xml != null) {
                return true;
            } else {
                return false;
            }
        }
    }
}