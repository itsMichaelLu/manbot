using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace manbot
{
    public class Logger
    {
        private System.Windows.Forms.TextBox log;
        // Safe access from thread
        delegate void LogCallBack(string msg);
        delegate void LogClearCallBack();


        public Logger(System.Windows.Forms.TextBox tb)
        {
            this.log = tb;
        }

        public void Log(string msg)
        {
            string formatted_str = DateTime.Now.ToString("[yyyy/MM/dd  HH:mm:ss]   ") + msg + Environment.NewLine;

            if (this.log.InvokeRequired)
            {
                LogCallBack d = new LogCallBack(Log);
                this.log.Invoke(d, new object[] { msg });
                return;
            }
            this.log.AppendText(formatted_str);
        }

        public void Error(string msg)
        {
            // TODO Display these if error messages are enabled
            Log(msg);
        }
        
        public void Debug(string msg)
        {
            // TODO Only display these if debug mode is on
            Log(msg);
        }

        public void LogRaw(string msg)
        {
            if (this.log.InvokeRequired)
            {
                LogCallBack d = new LogCallBack(LogRaw);
                this.log.Invoke(d, new object[] { msg });
                return;
            }
            this.log.AppendText(msg);
        }

        public void ClearText()
        {
            if (this.log.InvokeRequired)
            {
                LogClearCallBack d = new LogClearCallBack(ClearText);
                this.log.Invoke(d, new object[] { });
                return;
            }
            this.log.Clear();
            this.Log("----------");
        }

        public void ClearTextRaw()
        {
            if (this.log.InvokeRequired)
            {
                LogClearCallBack d = new LogClearCallBack(ClearTextRaw);
                this.log.Invoke(d, new object[] { });
                return;
            }
            this.log.Clear();
        }

    }
}
