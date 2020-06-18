using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoHotkey.Interop;

namespace manbot
{
    class SimpleAhkWrapper
    {
        private AutoHotkeyEngine ahk;
        const string keySendMsg = "ControlSend,, {{{0} {1}}}, ahk_pid {2}";

        public SimpleAhkWrapper(AutoHotkeyEngine ahk)
        {
            this.ahk = ahk;
        }

        private string KeyToString(int key)
        {
            switch(key)
            {
                case 0:
                    return "Hello";
                default:
                    return "bad";
            }
        }

        public void KeyDown(int key, int pid)
        {
            //Send a message to press a key down
            string msg = string.Format(keySendMsg, "VK" + key.ToString("X3"), "down", pid);
            //Globals.logger.Log(msg);
            this.ahk.ExecRaw(msg);

        }

        public void KeyUp(int key, int pid)
        {
            string msg = string.Format(keySendMsg, "VK" + key.ToString("X3"), "up", pid);
            //Globals.logger.Log(msg);
            this.ahk.ExecRaw(msg);
        }

        public void KeyPress(int key, int pid, uint delay = 50)
        {
            KeyDown(key, pid);
            Globals.SafeSleep(delay);
            KeyUp(key, pid);
        }
    }
}
