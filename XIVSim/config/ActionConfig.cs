using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using xivsim.action;

namespace xivsim.config
{
    [XmlRoot("ActionList")]
    public class ActionConfig
    {
        [XmlArray("Actions")]
        [XmlArrayItem("Action")]
        public List<ActionElement> List { get; set; }

        public ActionConfig()
        {
            List = new List<ActionElement>();
        }

        public void Add(Action act)
        {
            ActionElement elm = new ActionElement();

            elm.Class = act.GetType().Name;
            act.SaveConfig(elm);

            List.Add(elm);
        }

        public void AddAll(List<Action> acts)
        {
            foreach(Action act in acts)
            {
                Add(act);
            }
        }

        public List<Action> GetAll()
        {
            List<Action> acts = new List<Action>();

            foreach(ActionElement elm in List)
            {
                Type type = Type.GetType("xivsim.action." + elm.Class);
                Action act = (Action)Activator.CreateInstance(type);
                act.LoadConfig(elm);
                acts.Add(act);
            }

            return acts;
        }

        public void Save(string fname)
        {
            XmlSerializer s = new XmlSerializer(typeof(ActionConfig));
            using (StreamWriter w = new StreamWriter(fname, false, new UTF8Encoding(true)))
            {
                s.Serialize(w, this);
            }
        }

        public static ActionConfig Load(string fname)
        {
            XmlSerializer s = new XmlSerializer(typeof(ActionConfig));
            ActionConfig ai = null;
            if (File.Exists(fname))
            {
                using (StreamReader w = new StreamReader(fname, new UTF8Encoding(true)))
                {
                    ai = (ActionConfig)s.Deserialize(w);
                }
            }
            return ai;
        }
    }
}
