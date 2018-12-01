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
            Group = "default";
            Class = null;
            Remain = 0.0;
            Recast = 0.0;
            Relation = null;
            Stack = 0;
            Amplifier = 0.0;
        }

        [XmlAttribute("group")]
        public string Group { get; set; }

        [XmlAttribute("class")]
        public string Class { get; set; }

        [XmlAttribute("remain")]
        public double Remain { get; set; }

        [XmlAttribute("recast")]
        public double Recast { get; set; }

        [XmlAttribute("relation")]
        public string Relation { get; set; }

        [XmlAttribute("stack")]
        public int Stack { get; set; }

        [XmlAttribute("amplifier")]
        public double Amplifier { get; set; }
    }
}
