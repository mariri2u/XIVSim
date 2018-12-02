using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace xivsim.config
{
    public class AIElement
    {
        public AIElement()
        {
            Group = "common";
            Class = null;
            Relation = null;
            Threshold = 0.0;
        }

        [XmlAttribute("group")]
        public string Group { get; set; }

        [XmlAttribute("class")]
        public string Class { get; set; }

        [XmlAttribute("action")]
        public string Relation { get; set; }

        [XmlAttribute("value")]
        public double Threshold { get; set; }
    }
}
