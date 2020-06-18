using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manbot
{
    class DetectKey
    {
        private int keysDown;
        private bool[] keysList;
        private const int kmax = char.MaxValue;
        
        public DetectKey()
        {
            this.keysDown = 0;
            this.keysList = Enumerable.Repeat(false, kmax).ToArray();
        }

        private void Constrain()
        {
            if (this.keysDown < 0)
            {
                this.keysDown = 0;
            }
            else if (this.keysDown > kmax - 1)
            {
                this.keysDown = kmax - 1;
            }
        }

        public bool Increase(System.Windows.Forms.KeyEventArgs e)
        {
            // When key is pressed
            if (e.KeyValue < kmax && !this.keysList[e.KeyValue])
            {
                this.keysList[e.KeyValue] = true;
                this.keysDown++;
                this.Constrain();
                return true;
            }
            return false;
        }


        public bool Decrease(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyValue < kmax && this.keysList[e.KeyValue])
            {
                this.keysList[e.KeyValue] = false;
                this.keysDown--;
                this.Constrain();
                return true;
            }
            return false;
        }

        public bool GetState(int idx)
        {
            if (idx < 0 || idx >= kmax)
            {
                return false;
            }
            return this.keysList[idx];
        }

        public int Count()
        {
            return this.keysDown;
        }

        public string DoStringActions()
        {
            string s = "";
            int i = this.keysDown;
            for (i = this.keysDown; i < kmax; i++)
            {
                if (this.keysList[i])
                {
                    var item = (System.Windows.Forms.Keys)i;
                    s += item.ToString();
                }
                if (i != kmax -1)
                {
                    s += " +";
                }
            }
            return s;
        }

    }
}
