using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace xivsim.config
{
    public class AIPair
    {
        [XmlAttribute("name")]
        public string Action { get; set; }

        [XmlAttribute("comment")]
        public string Comment { get; set; }

        [XmlElement("AI")]
        public List<AIElement> AI { get; set; }
    }
}
