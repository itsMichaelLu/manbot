using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manbot
{

    public class MacroList
    {
        // Thread-safe callbacks
        delegate void AddKeyCallback(int key, KeyStates state);
        delegate void AddTimeCallback(int time);
        delegate void RemoveItemCallback();
        delegate System.Windows.Forms.ListViewItem GetItemCallback(int index);
        delegate Tuple<KeyStates, int, string, int> GetEntryListItemCallback(int index);

        private System.Windows.Forms.ListView mls;




        public MacroList(System.Windows.Forms.ListView lv)
        {
            this.mls = lv;
        }

        public void AddKey(int key, KeyStates state)
        {
            if (this.mls.InvokeRequired)
            {
                AddKeyCallback d = new AddKeyCallback(AddKey);
                this.mls.Invoke(d, new object[] { key, state });
                return;
            }

            if (!Globals.ksV2N.ContainsKey(state))
            {
                //Should return -1 as error...
                return;
            }

            string kn = ((System.Windows.Forms.Keys)key).ToString();
            string[] r = { Globals.ksV2N[state], key.ToString(), kn};
            mls.Items.Add(new System.Windows.Forms.ListViewItem(r));
            Globals.logger.Log("Added items");
        }

        public void AddTime(int time)
        {
            if (this.mls.InvokeRequired)
            {
                AddTimeCallback d = new AddTimeCallback(AddTime);
                this.mls.Invoke(d, new object[] { time });
                return;
            }


            string[] row = { "", "", "", time.ToString()};
            mls.Items.Add(new System.Windows.Forms.ListViewItem(row));
            Globals.logger.Log("Added Delay");
        }

        public void RemoveItem()
        {
            if (this.mls.InvokeRequired)
            {
                RemoveItemCallback d = new RemoveItemCallback(RemoveItem);
                this.mls.Invoke(d, new object[] { });
                return;
            }

            var k = mls.SelectedIndices;
            int idx;

            if (mls.Items.Count < 1)
            {
                Globals.logger.Log("There are no items to remove");
                return;
            }

            if (k.Count < 1)
            {
                Globals.logger.Log("Select something to remove");
                return;
            }
            idx = mls.SelectedIndices[0];
            Globals.logger.Log(idx.ToString());
            mls.Items[idx].Remove();
            mls.SelectedItems.Clear();
            /* Currently doesnt select properly. Fix later?
            if (mls.Items.Count >= 1)
            {
                if (mls.Items.Count > idx)
                {
                    //mls.Items[idx].Focused = true;
                    mls.Items[idx].Selected = true;

                }
                else
                {
                   // mls.Items[idx - 1].Focused = true;
                    mls.Items[idx - 1].Selected = true;
                }
            }
            */
        }

        public void RemoveAllItems()
        {
            if (this.mls.InvokeRequired)
            {
                RemoveItemCallback d = new RemoveItemCallback(RemoveAllItems);
                this.mls.Invoke(d, new object[] { });
                return;
            }

            mls.SelectedItems.Clear();
            while (mls.Items.Count > 0)
            {
                mls.Items[0].Remove();
            }
            Globals.logger.Log("Removed all items");
        }


        public void ShiftItemUp()
        {
            if (this.mls.InvokeRequired)
            {
                RemoveItemCallback d = new RemoveItemCallback(RemoveAllItems);
                this.mls.Invoke(d, new object[] { });
                return;
            }
            var k = mls.SelectedIndices;
            int idx;

            if (mls.Items.Count < 1)
            {
                Globals.logger.Error("Can't shift item");
                return;
            }

            if (k.Count < 1)
            {
                Globals.logger.Error("Select something to shift");
                return;
            }
            idx = mls.SelectedIndices[0];

            if (idx <= 0)
            {
                Globals.logger.Error("Cant shift Item");
                mls.SelectedItems.Clear();
                return;
            }

            var tmp = mls.Items[idx];
            mls.Items.RemoveAt(idx);
            mls.Items.Insert(idx - 1, tmp);

            mls.Items[idx - 1].Selected = true;
            mls.Select();
        }

        public void ShiftItemDown()
        {
            if (this.mls.InvokeRequired)
            {
                RemoveItemCallback d = new RemoveItemCallback(RemoveAllItems);
                this.mls.Invoke(d, new object[] { });
                return;
            }
            var k = mls.SelectedIndices;
            int idx;

            if (mls.Items.Count < 1)
            {
                Globals.logger.Error("Can't shift item");
                return;
            }

            if (k.Count < 1)
            {
                Globals.logger.Error("Select something to shift");
                return;
            }
            idx = mls.SelectedIndices[0];

            if (idx >= mls.Items.Count - 1)
            {
                Globals.logger.Error("Cant shift Item");
                mls.SelectedItems.Clear();
                return;
            }

            var tmp = mls.Items[idx];
            mls.Items.RemoveAt(idx);
            mls.Items.Insert(idx + 1, tmp);

            mls.Items[idx + 1].Selected = true;
            mls.Select();
        }

        public System.Windows.Forms.ListViewItem GetItem(int index)
        {
            if (this.mls.InvokeRequired)
            {
                GetItemCallback d = new GetItemCallback(GetItem);
                return (System.Windows.Forms.ListViewItem)this.mls.Invoke(d, new object[] { index });
            }

            if (index >= mls.Items.Count)
            {
                Globals.logger.Log("Index too large");
                return null;
            }
            return mls.Items[index];
        }

        public Tuple<KeyStates, int, string, int> GetEntryList(int index)
        {
            if (this.mls.InvokeRequired)
            {
                GetEntryListItemCallback d = new GetEntryListItemCallback(GetEntryList);
                return (Tuple<KeyStates, int, string, int>)this.mls.Invoke(d, new object[] { index });
            }

            if (index >= mls.Items.Count)
            {
                Globals.logger.Log("Index too large");
                return null;
            }

            KeyStates item1;
            int item2;
            string item3;
            int item4;

            var ls = mls.Items[index].SubItems;
            if (ls[0].Text.ToString().Length > 0)
            {
                if (!int.TryParse(ls[1].Text.ToString(), out item2))
                {
                    Globals.logger.Log("Failed to convert string '" + ls[1].Text.ToString() + "' to int");
                    return null;
                }
                item1 = Globals.ksN2V[ls[0].Text.ToString().ToLower()];
                item3 = ls[2].Text.ToString();
                item4 = -1; // Maybe set this to null?
                return new Tuple<KeyStates, int, string, int>(item1, item2, item3, item4);
            }
            else if (int.TryParse(ls[3].Text.ToString(), out item4))
            {
                item1 = KeyStates.NONE;
                item2 = -1;
                item3 = "";
                return new Tuple<KeyStates, int, string, int>(item1, item2, item3, item4);
            }
            return null;
        }

        public int GetSize()
        {
            return mls.Items.Count;
        }

        public int Verify()
        {
            // Verify that all downs have a corresponding up key
            Dictionary<int, int> verkeys = new Dictionary<int, int>();
            for (int i = 0; i < this.GetSize(); i++)
            {
                var tupls = this.GetEntryList(i);

                if (tupls.Item4 >= 0)
                {
                    // If its a delay, then skip
                    continue;
                }

                int num;
                if (!verkeys.TryGetValue(tupls.Item2, out num))
                {
                    // If it doesnt exist, then set it
                    verkeys.Add(tupls.Item2, 0);
                }

                if (tupls.Item1 == KeyStates.UP)
                {
                    verkeys[tupls.Item2]++;
                }
                if (tupls.Item1 == KeyStates.DOWN)
                {
                    verkeys[tupls.Item2]--;
                }
            }
            // Verify everything is zero
            foreach (var iter in verkeys)
            {
                if (iter.Value != 0)
                {
                    Globals.logger.Log("Number of key down presses do not match key ups");
                    return -1;
                }
            }
            Globals.logger.Log("Macro list is verified");
            return 0; //success

        }
    }
}
