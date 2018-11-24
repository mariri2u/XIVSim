using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace xivsim
{
    public class Logger
    {
        struct Log
        {
            public LogType type;
            public double value_f;
            public string value_s;
            public int value_i;
        }

        private StreamWriter fp;
        private List<string> keys;
        private Dictionary<string, Log> logs;
        private Boolean dumping;

        enum LogType
        {
            INT, DOUBLE, TEXT
        }

        public Logger(string fname)
        {
            keys = new List<string>();
            logs = new Dictionary<string, Log>();
            dumping = false;
            fp = new StreamWriter(fname, false, System.Text.Encoding.GetEncoding("shift_jis"));
        }

        public void AddText(string key, string value)
        {
            Log log = new Log();
            log.type = LogType.TEXT;
            log.value_s = value;
            logs[key] = log;
            if (dumping == false)
            {
                keys.Add(key);
            }
        }

        public void AddInt(string key, int value)
        {
            Log log = new Log();
            log.type = LogType.INT;
            log.value_i = value;
            logs[key] = log;
            if (dumping == false)
            {
                keys.Add(key);
            }
        }

        public void AddDouble(string key, double value)
        {
            Log log = new Log();
            log.type = LogType.DOUBLE;
            log.value_f = value;
            logs[key] = log;
            if(dumping == false)
            {
                keys.Add(key);
            }
        }

        public void Dump()
        {
            if(dumping == false)
            {
                foreach (string key in keys)
                {
                    fp.Write(key + ",");
                }
                fp.Write("\r\n");
                dumping = true;
            }

            foreach (string key in keys)
            {
                switch (logs[key].type)
                {
                    case LogType.DOUBLE:
                        fp.Write(logs[key].value_f + ",");
                        break;
                    case LogType.INT:
                        fp.Write(logs[key].value_i + ",");
                        break;
                    case LogType.TEXT:
                        fp.Write(logs[key].value_s + ",");
                        break;
                    default:
                        fp.Write("-,");
                        break;
                }
            }
            fp.Write("\r\n");
            fp.Flush();
            logs.Clear();
        }

        public void Clear()
        {
            if(dumping == false)
            {
                keys.Clear();
            }
            logs.Clear();
        }

        public void Close()
        {
            fp.Close();
        }
    }
}
