using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComLib
{
    public class clsCommonCode
    {
        ConnectDB cd = new ConnectDB();
        clsParameterMember param = new clsParameterMember();

        #region 테이블에서 데이터를 가져와 DataSet을 만들어 놓는다.
        public clsCommonCode()
        {
        }
        #endregion

        private DataTable clsCommonCodeByYear(int nStartYear, int nEndYear)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("CATEGORY", typeof(string));
            tbl.Columns.Add("CD_ID", typeof(string));
            tbl.Columns.Add("CD_NM", typeof(string));

            for (int i = nStartYear; i <= nEndYear; i++)
            {
                DataRow dr = tbl.NewRow();
                dr["CATEGORY"] = "YEAR";
                dr["CD_ID"] = i.ToString();
                dr["CD_NM"] = i.ToString() + "년";

                tbl.Rows.Add(dr);
            }
            return tbl;
        }

        #region [ComboBox] 고정코드 정보로 ComboBox를 채워 넣는다.
        public void ComboBoxItemAdd(string CodeType, System.Windows.Forms.ComboBox cbo)
        {
            this.ComboBoxItemAdd(CodeType, cbo, null);
        }
        public void ComboBoxItemAdd(string CodeType, System.Windows.Forms.ComboBox cbo, string InitText)
        {
            DataSet ds = new DataSet();
            DataTable TempTbl = new DataTable();

            DataTable tbl = new DataTable();
            tbl.Columns.Add("CATEGORY", typeof(string));
            tbl.Columns.Add("CD_ID", typeof(string));
            tbl.Columns.Add("CD_NM", typeof(string));

            if (null != InitText)
            {
                tbl.Rows.Add(new object[] { "", DBNull.Value, InitText });
            }

            param.Clear();
            param.Add(SqlDbType.VarChar, "@pGUBUN", CodeType, ParameterDirection.Input);
            TempTbl = cd.ExecuteStoreProcedureDataTable("SP_BI_ComboInq", param);

            DataRow[] dataRow = TempTbl.Select();

            foreach(DataRow dr in dataRow)
            {
                tbl.Rows.Add(new object[] { Convert.ToString(dr["CATEGORY"]), Convert.ToString(dr["CD_ID"]), Convert.ToString(dr["CD_NM"]) });
            }

            ds.Tables.Add(tbl);

            cbo.DataSource = ds;
            cbo.DisplayMember = "Table1.CD_NM";
            cbo.ValueMember = "Table1.CD_ID";

            if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
        }
        #endregion

        public void ComboBoxItemAdd(System.Windows.Forms.ComboBox cbo, string InitText = null, int nStartYear=2017, int nEndYear=2017)
        {
            DataSet ds = new DataSet();
            DataTable TempTbl = new DataTable();

            DataTable tbl = new DataTable();
            tbl.Columns.Add("CATEGORY", typeof(string));
            tbl.Columns.Add("CD_ID", typeof(string));
            tbl.Columns.Add("CD_NM", typeof(string));

            if (null != InitText)
            {
                tbl.Rows.Add(new object[] { "", "", InitText });
            }

            TempTbl = clsCommonCodeByYear(nStartYear, nEndYear);

            DataRow[] dataRow = TempTbl.Select();

            foreach (DataRow dr in dataRow)
            {
                tbl.Rows.Add(new object[] { Convert.ToString(dr["CATEGORY"]), Convert.ToString(dr["CD_ID"]), Convert.ToString(dr["CD_NM"]) });
            }

            ds.Tables.Add(tbl);

            cbo.DataSource = ds;
            cbo.DisplayMember = "Table1.CD_NM";
            cbo.ValueMember = "Table1.CD_ID";

            if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
        }
    }
}
