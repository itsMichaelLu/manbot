using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoHotkey.Interop;

namespace manbot
{
    public partial class Form1 : Form
    {
        Logger logger;
        DetectKey keys;
        int pid = -1;
        string pname = "";
        AutoHotkeyEngine ahk;
        SimpleAhkWrapper ahki;
        MacroList mls;
        int lastkeypressed = -1;
        //Create a thread
        System.Threading.Thread thread;


        public Form1()
        {
            InitializeComponent();
            //Initialise global vars
            Globals.logger = new Logger(textBox1);
            Globals.mlist = new MacroList(listView1);

            //Instance vars
            this.mls = Globals.mlist;
            this.logger = Globals.logger;
            this.keys = new DetectKey();
            this.ahk = Globals.ahk;
            this.ahki = new SimpleAhkWrapper(this.ahk);

            // Main thread
            this.thread = null;
            //this.thread = new System.Threading.Thread(this.Run);
            //this.thread.IsBackground = true;
        }

        private void Run()
        {
            int iterations;

            if (!int.TryParse(textBox6.Text, out iterations))
            {
                iterations = 1;
            }

            for (int iter = 0; iter < iterations; iter++)
            {
                logger.Log($"Loop number {iter}");
                //Iterate through the macro list
                for (int i = 0; i < this.mls.GetSize(); i++)
                {
                    var tupls = this.mls.GetEntryList(i);
                    if (tupls == null)
                    {
                        logger.Log("Somethings wrong...");
                        return;
                    }
                    //Globals.logger.Log(tupls.Item1.ToString());


                    // If it is a delay
                    if (tupls.Item4 >= 0)
                    {
                        Globals.SafeSleep((uint)tupls.Item4);
                        if (!Globals.isRunning)
                        {
                            logger.Log("Exiting on request of user....");
                            return;
                        }
                        continue;
                    }

                    // Then do the macro
                    switch (tupls.Item1)
                    {
                        case KeyStates.DOWN:
                            this.ahki.KeyDown(tupls.Item2, this.pid);
                            break;
                        case KeyStates.UP:
                            this.ahki.KeyUp(tupls.Item2, this.pid);
                            break;
                        case KeyStates.PRESS:
                            this.ahki.KeyPress(tupls.Item2, this.pid);
                            break;
                        default:
                            logger.Log("Unknown key command: " + tupls.Item1.ToString());
                            break;
                    }
                    // Check if its still valid to run
                    if (!Globals.isRunning)
                    {
                        logger.Log("Exiting on request of user....");
                        return;
                    }
                }
            }
            // Done
            Globals.isRunning = false;
            logger.Log("Finished Macro");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //Button 1 Attach Process

            if (this.pid >= 0)
            {
                logger.Log("Reattaching Process...");
            }

            int pid = ProcessAttach.GetProcessID(textBox2.Text);
            string pname = ProcessAttach.GetProcessName(textBox2.Text);
            if (pid < 0)
            {
                logger.Log($"Error Retrieving PID for process \"{textBox2.Text}\"");
                this.pid = -1;
                this.pname = "";
                labelPid.Text = "-";
                labelPname.Text = "-";
            }
            else
            {
                logger.Log("Process found!");
                //logger.Log(String.Format("PID: {0}", pid));
                logger.Log($"PID: {pid}");
                //logger.Log(String.Format("Process name: {0}", pname));
                logger.Log($"Process name: {pname}");

                this.pid = pid;
                this.pname = pname;
                labelPid.Text = this.pid.ToString();
                labelPname.Text = this.pname;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Do nothing
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Do nothing
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Do nothing
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Do nothing
        }

        private void labelPname_Click(object sender, EventArgs e)
        {
            // Do nothing
        }

        private void labelPid_Click(object sender, EventArgs e)
        {
            // Do nothing
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Do nothing
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Do nothing
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Do nothing
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allow backspace key
            if (e.KeyChar == (char)8)
            {
                return;
            }

            // press button when enter key is pressed
            if (e.KeyChar == (char)13)
            {
                return;
            }

            // only allow numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            // only allow 2 decimal places
            if (((sender as TextBox).Text.IndexOf('.') > -1) && ((sender as TextBox).Text.IndexOf('.') < (sender as TextBox).Text.Length - 2))
            {
                e.Handled = true;
            }

            // only allow up to 2 digits
            if (((sender as TextBox).Text.IndexOf('.') < 0) && ((sender as TextBox).Text.Length > 1) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox4_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            (sender as TextBox).Clear();
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            // Do Nothing...
            //keys.Decrease(e);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != lastkeypressed)
            {
                lastkeypressed = e.KeyValue;
                (sender as TextBox).Text = e.KeyCode.ToString();
                //logger.Log(e.KeyValue.ToString());
                //logger.Log(e.KeyData.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int val;
            if (!int.TryParse(textBox3.Text, out val))
            {
                return;
            }
            //Press key
            ahki.KeyPress(val, this.pid, 100);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Error Checking
            if (textBox4.Text == "")
            {
                this.logger.Log("Key field empty");
                return;
            }

            if (comboBox1.SelectedIndex < 0)
            {
                this.logger.Log("State not selected");
                return;
            }
            string strstate = comboBox1.SelectedItem.ToString().ToLower();

            // Try add...
            this.mls.AddKey(this.lastkeypressed, Globals.ksN2V[strstate]);
            textBox4.Clear();
            lastkeypressed = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allow backspace key
            if (e.KeyChar == (char)8)
            {
                return;
            }

            // press button when enter key is pressed
            if (e.KeyChar == (char)13)
            {
                return;
            }

            // only allow numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // only allow up to 6 digits
            if ((sender as TextBox).Text.Length > 6)
            {
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int time;
            if (!int.TryParse(textBox5.Text, out time))
            {
                logger.Log("Invalid time");
            }

            mls.AddTime(time);
            textBox5.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mls.RemoveItem();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            mls.RemoveAllItems();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (this.pid < 0)
            {
                logger.Log("No PID ...");
                return;
            }
            if (this.thread == null)
            {
                this.thread = new System.Threading.Thread(this.Run);
            }
            if (this.thread.IsAlive)
            {
                logger.Log("Macro is already running!!");
                return;
            }

            this.thread = new System.Threading.Thread(this.Run);

            logger.Log("Running!");
            Globals.isRunning = true;
            this.thread.Start();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            logger.Log("Stopping!");
            Globals.isRunning = false;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(textBox6.Text, "[ ^ 0-9]"))
            {
                textBox6.Text = "";
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(textBox6.Text, "[ ^ 0-9]"))
            {
                textBox6.Text = "";
            }


        }

        private void button9_Click(object sender, EventArgs e)
        {
            mls.ShiftItemUp();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            mls.ShiftItemDown();
        }
    }
}
