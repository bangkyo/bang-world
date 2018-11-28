using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGsystemLibrary
{
    public class BloomID
    {
        private string heat_no = string.Empty;
        private string heat_seq = string.Empty;
        private string rework_sno = string.Empty;
        private string show_rework_sno = string.Empty;
        public BloomID(string _heat_no, string _heat_seq, string _rework_sno ="0")
        {
            this.heat_no = _heat_no;
            this.heat_seq = _heat_seq;
            this.rework_sno = _rework_sno;
            this.show_rework_sno = (Convert.ToInt32(_rework_sno) +1).ToString();
        }

        public string HeatNo { get { return this.heat_no; } }
        public string Heat_Seq { get { return this.heat_seq; } }
        public string Rework_sno { get { return this.rework_sno; } }
        public string Show_Rework_sno { get { return this.show_rework_sno; } }
        public string FullBloomNo { get { return $"{this.heat_no}  {this.heat_seq}  {this.rework_sno}"; } }
    }
}
