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
            Class = null;
            Name = null;
            Power = 0;
            Slip = 0;
            Cast = 0;
            Recast = 0;
            Motion = 0.8;
            Amplifier = 0.0;
            Duration = 0;
            Relation = null;
            RelationA = null;
            RelationB = null;
            RelationC = null;
            Require = 0;
            Increase = 0;
            Before = null;
            Haste = 0.0;
            Combo = true;
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

        [XmlAttribute("relationA")]
        public string RelationA { get; set; }
        
        [XmlAttribute("relationB")]
        public string RelationB { get; set; }

        [XmlAttribute("relationC")]
        public string RelationC { get; set; }

        [XmlAttribute("require")]
        public int Require { get; set; }

        [XmlAttribute("increase")]
        public int Increase { get; set; }

        [XmlAttribute("before")]
        public string Before { get; set; }

        [XmlAttribute("max")]
        public int Max { get; set; }

        [XmlAttribute("haste")]
        public double Haste { get; set; }

        [XmlAttribute("combo")]
        public bool Combo { get; set; }
    }
}
