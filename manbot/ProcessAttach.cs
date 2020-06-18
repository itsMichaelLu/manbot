using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading.Tasks;

namespace manbot
{
    class ProcessAttach
    {
        public ProcessAttach()
        {
        }

        static public int GetProcessID(String pname)
        {
            //Process p = Process.GetCurrentProcess();
            Process[] p = Process.GetProcessesByName(pname);
            if (p.Length == 1)
            {
                return p[0].Id;
            }
            return -1;
        }
        static public String GetProcessName(String pname)
        {
            Process[] p = Process.GetProcessesByName(pname);
            if (p.Length == 1)
            {
                return p[0].ProcessName;
            }
            return "";
        }
    }
}
