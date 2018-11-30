using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace xivsim.config
{
    public class ActionElement
    {
        public ActionElement()
        {
            Class = "";
            Name = "";
            Power = 0;
            Slip = 0;
            Cast = 0;
            Recast = 0;
            Motion = 0.1;
            Amplifier = 1.0;
            Duration = 0;
        }

        [XmlAttribute("class")]
        public string Class { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("power")]
        public int Power { get; set; }

        [XmlAttribute("slip")]
        public int Slip { get; set; }

        [XmlAttribute("cast")]
        public double Cast { get; set; }

        [XmlAttribute("recast")]
        public double Recast { get; set; }

        [XmlAttribute("motion")]
        public double Motion { get; set; }

        [XmlAttribute("amplifier")]
        public double Amplifier { get; set; }

        [XmlAttribute("duration")]
        public int Duration { get; set; }

        [XmlAttribute("relation")]
        public string Relation { get; set; }

        [XmlAttribute("require")]
        public int Require { get; set; }
    }
}
