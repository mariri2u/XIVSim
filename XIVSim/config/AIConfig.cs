using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using xivsim.ai;

namespace xivsim.config
{
    [XmlRoot("ActionAI")]
    public class AIConfig
    {
        [XmlArray("Actions")]
        [XmlArrayItem("Action")]
        public List<AIPair> List { get; set; }

        public AIConfig()
        {
            List = new List<AIPair>();
        }

        public void Add(string key, AI ai)
        {
            AIPair pair = null;
            foreach(AIPair p in List)
            {
                if(p.Action == key)
                {
                    pair = p;
                    break;
                }
            }

            if (pair == null)
            {
                pair = new AIPair();
                pair.Action = key;
                pair.AI = new List<AIElement>();
                List.Add(pair);
            }

            AIElement elm = new AIElement();
            elm.Class = ai.GetType().Name;
            pair.AI.Add(elm);
        }

        public List<AI> Get(string key)
        {
            List<AIElement> aicfg = null;
            foreach(AIPair pair in List)
            {
                if(pair.Action == key)
                {
                    aicfg = pair.AI;
                    break;
                }
            }

            if(aicfg == null)
            {
                aicfg = new List<AIElement>();
            }

            List<AI> ais = new List<AI>();
            foreach(AIElement elm in aicfg)
            {
                Type type = Type.GetType("xivsim.ai." + elm.Class);
                AI ai = (AI)Activator.CreateInstance(type);
                ai.Name = key;
                ai.LoadConfig(elm);
                ais.Add(ai);
            }

            return ais;
        }

        public void Save(string fname)
        {
            XmlSerializer s = new XmlSerializer(typeof(AIConfig));
            using (StreamWriter w = new StreamWriter(fname, false, new UTF8Encoding(true)))
            {
                s.Serialize(w, this);
            }
        }

        public static AIConfig Load(string fname)
        {
            XmlSerializer s = new XmlSerializer(typeof(AIConfig));
            AIConfig ai = null;
            if (File.Exists(fname))
            {
                using (StreamReader w = new StreamReader(fname, new UTF8Encoding(true)))
                {
                    ai = (AIConfig)s.Deserialize(w);
                }
            }
            return ai;
        }
    }
}
