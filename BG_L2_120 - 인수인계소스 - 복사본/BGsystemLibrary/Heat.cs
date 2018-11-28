using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGsystemLibrary
{
    public class Heat
    {
        private string num = string.Empty;
        private BloomIDs bloomids;
        public Heat(string number)
        {
            this.num = number;
            bloomids = new BloomIDs();
        }

        public Heat(string number, BloomIDs _bloomids)
        {
            this.num = number;
            bloomids = _bloomids;
        }

        public string No { get { return this.num; } }
        public BloomIDs BloomIDs { get { return bloomids; }  }
        //public string MyProperty { get; set; }

        public void AddBloomid(BloomID bloomid)
        {
            bloomids.Add(bloomid);
        }

        public string GetBloomList()
        {
            string bloomList = string.Empty;
            int counts = 0;
            foreach (BloomID bloom in bloomids)
            {

                bloomList += bloom.Heat_Seq + ",";
                counts++;

            }
            if (counts >= 1)
            {
                bloomList = bloomList.Substring(0, bloomList.Length - 1);
            }

            return bloomList;
        }
    }
}
