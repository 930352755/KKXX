using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIHelp
{
    public class AIHelpLogData : DataBase
{
        private static AIHelpLogData instance = null;
        public static AIHelpLogData Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new AIHelpLogData();
                }
                return instance;
            }
        }
        private AIHelpLogData()
        {
            OnLoad();
        }
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