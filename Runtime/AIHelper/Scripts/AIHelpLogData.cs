using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIHelp
{
    public class AIHelpLogData : DataBase<AIHelpLogData>
    {
        private AIHelpLogData() { }
        public override string DataPath => "AIHelpLog.txt";
        public List<string> logs = new List<string>();

        protected override void OnLoad()
        {
            base.OnLoad();
        }

        public void AddLog(string msg)
        {
            logs.Add(msg);
            Save();
        }

        public void ClearLog()
        {
            logs = new List<string>();
            Save();
        }
    }
}