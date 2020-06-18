using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoHotkey.Interop;
using System.Diagnostics;

namespace manbot
{
    public enum KeyStates
    {
        DOWN,
        UP,
        PRESS,
        NONE = -1,
    }

    public static class Globals
    {
        // List of global vars
        public static AutoHotkeyEngine ahk = AutoHotkeyEngine.Instance;
        public static bool isRunning = false;
        public static Logger logger;
        public static MacroList mlist;


        // Functions (methods)
        public static int GetProcessID(String pname)
        {
            //Process p = Process.GetCurrentProcess();
            Process[] p = Process.GetProcessesByName(pname);
            if (p.Length == 1)
            {
                return p[0].Id;
            }
            return -1;
        }
        public static String GetProcessName(String pname)
        {
            Process[] p = Process.GetProcessesByName(pname);
            if (p.Length == 1)
            {
                return p[0].ProcessName;
            }
            return "";
        }

        public static Dictionary<KeyStates, string> ksV2N = new Dictionary<KeyStates, string>()
        {
            { KeyStates.DOWN, "Down" },
            { KeyStates.UP, "Up" },
            { KeyStates.PRESS, "Press" },
            { KeyStates.NONE, "None" }
        };

        public static Dictionary<string, KeyStates> ksN2V = new Dictionary<string, KeyStates>()
        {
            { "down", KeyStates.DOWN},
            { "up", KeyStates.UP},
            { "press", KeyStates.PRESS},
            { "none", KeyStates.NONE }
        };



        public static void SafeSleep(uint time)
        {
            if (time > 120000)
            {
                // 2 minutes is too long...
                return;
            }

            //System.Threading.Thread.Sleep((int)time);

            //Sleep for 10ms intervals
            for (uint itx = 0; itx < time; itx += 10)
            {
                System.Threading.Thread.Sleep(10);
                if (!isRunning)
                {
                    //Perhaps send log message?
                    Globals.logger.Log("Program Stopped inside sleep");
                    break;
                }
            }
        }

    } //End class
}
