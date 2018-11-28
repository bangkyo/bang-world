using C1.Win.C1FlexGrid;
using ChartFX.WinForms;
using System.Collections.Generic;

namespace BGsystemLibrary.RsltMgmt
{
    public class flexgrid
    {
        private string name = string.Empty;
        public flexgrid(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }
        public int GridRowsCount { get; set; }
        public int GridColsCount { get; set; }
        public int RowsFixed { get; set; }
        public int RowsFrozen { get; set; }
        public int ColsFixed { get; set; }
        public int ColsFrozen { get; set; }
        public int TopRowsHeight { get; set; }
        public int DataRowsHeight { get; set; }
        public SearchOption SearchOption { get; set; }
        public string Sql { get; set; } = string.Empty;
        public C1FlexGrid grd { get; set; }
        public List<string> RemoveColList { get; set; }
        public Chart chart { get; set; }

    }

    public enum SearchOption
    {
                  SHT_OPER_INFO
                , SHT_DUST_INFO
                , MPI_OPER_INFO
                , GRD_OPER_INFO1
                , GRD_OPER_INFO2
                , GRD_DUST_INFO1
                , GRD_DUST_INFO2
    }
}
