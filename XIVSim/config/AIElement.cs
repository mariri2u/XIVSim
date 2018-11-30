using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace xivsim.config
{
    public class AIElement
    {
        [XmlAttribute("class")]
        public string Class { get; set; }

        [XmlAttribute("remain")]
        public double Remain { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("recast")]
        public double Recast { get; set; }
    }
}
