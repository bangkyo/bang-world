using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ComLib;
using C1.Win.C1FlexGrid;
using System.Collections;

namespace SystemControlClassLibrary.phqms
{
    public partial class TableInfo : Form
    {
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable olddt;
        DataTable moddt;
        DataTable logdt;

        private int selectedrow = 0;
        private string poc_state = "";
        private string gangjung_id = "";
        clsStyle cs = new clsStyle();

        // 셀별 수정 정보(이전/이후)
        private static string[][] strModi = new string[0][];

        private static string ownerNM = "";
        private static string titleNM = "";

        string op_biz_gp, err_yn, diff_yn;

        C1FlexGrid selectedGrd = null;

        string _op_biz_gp, _sel, _sp_id;

        public TableInfo(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();

            Load += TableInfo_Load;

            btnDisplay.Click += Button_Click;
            btnExcel.Click += Button_Click;

            grdMain.RowColChange += GrdMain_RowColChange;

        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TableInfo_Load(object sender, System.EventArgs e)
        {
            InitControl();

            //SetComboBox1();

            Button_Click(btnDisplay, null);

           // EventCreate();      //사용자정의 이벤트
        }

        private void Button_Click(object sender, EventArgs e)
        {

            switch (((Button)sender).Name)
            {
                case "btnDisplay":
                    //cd.InsertLogForSearch(ck.UserID, btnDisplay);
                    SetDataBinding();

                    break;

                case "btnExcel":
                    SaveExcel();
                    break;
            }
        }

        private void SaveExcel()
        {
            vf.SaveExcel(titleNM, grdMain);
        }

        private void GrdMain_RowColChange(object sender, EventArgs e)
        {
            int maxrow = 0;
            int oldSel = 0;
            string str = string.Empty;
            string temp = string.Empty;

            selectedrow = grdMain.RowSel;
        }

        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitLabel(label1);
            cs.InitLabel(label4);

            cs.InitTextBox(txtTbId);

            cs.InitCombo(cbOpBiz_gp);

            // Button Color Set
            cs.InitButton(btnExcel);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);

            cs.InitCombo(cbOpBiz_gp, StringAlignment.Near);

            SetComboEdit(cbOpBiz_gp, "OP_BIZ_GP", true, ck.Op_Biz_gp);

            InitGrid();
        }

        public bool SetComboEdit(ComboBox cb, string categoryNm, bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();

                DataTable dt = cd.Find_CD(categoryNm);


                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("선택", ""));
                }


                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                //cb.Selecteditem = ck.StrKey2;

                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private void cbOpBiz_gp_SelectedIndexChanged(object sender, EventArgs e)
        {
            op_biz_gp = ((ComLib.DictionaryList)cbOpBiz_gp.SelectedItem).fnValue;
            ck.Op_Biz_gp = op_biz_gp;
        }

        private void btnADReg_Click(object sender, EventArgs e)
        {
            AnalDataReg popup = new AnalDataReg();
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();
        }

        private void btnSpRedo_Click(object sender, EventArgs e)
        {
            if (_sel == "True")
            {
                AnalDataRedo adr = new AnalDataRedo(_op_biz_gp, _sp_id);
                adr.Owner = this;
                adr.StartPosition = FormStartPosition.CenterScreen;
                adr.ShowDialog();
            }
            else
            {
                MessageBox.Show("SP 항목을 선택해주세요.");
                return;
            }
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (_sel == "True")
            {
                AnalDataRecord adRec = new AnalDataRecord(_op_biz_gp, _sp_id);
                adRec.Owner = this;
                adRec.StartPosition = FormStartPosition.CenterScreen;
                adRec.ShowDialog();
            }
            else
            {
                MessageBox.Show("이력조회 항목을 선택해주세요.");
                return;
            }
        }

        private void grdMain_Click(object sender, EventArgs e)
        {
            C1FlexGrid _selectedGrd = sender as C1FlexGrid;

            selectedGrd = _selectedGrd;

            SelectedGrdMain(selectedGrd, selectedGrd.RowSel);
        }

        private void SelectedGrdMain(C1FlexGrid grd, int selected_Row)
        {
            if (grd.RowSel <= 0 || grd.Rows.Count <= 1)
            {
                return;
            }

            // subgrd data search
            if (grd.GetData(selected_Row, "OP_BIZ_GP") != null)
                _op_biz_gp = grd.GetData(selected_Row, "OP_BIZ_GP").ToString();
            if (grd.GetData(selected_Row, "SEL") != null)
                _sel = grd.GetData(selected_Row, "SEL").ToString();
            if (grd.GetData(selected_Row, "SP_ID") != null)
                _sp_id = grd.GetData(selected_Row, "SP_ID").ToString();

            //MessageBox.Show(_sel);
            //InitGridData(false);
            //SetDataBindingGrdSub(_line_gp, _work_date, _work_rank, _item_size, _steel, _length);
        }

        private void InitGrid()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            grdMain.AllowEditing = true;

            grdMain.Cols["L_NO"].Caption = "NO";
            grdMain.Cols["COLUMN"].Caption = "COLUMN";
            grdMain.Cols["NULLABLE"].Caption = "NULLABLE";
            grdMain.Cols["TYPE"].Caption = "TYPE";
            grdMain.Cols["COMMENT"].Caption = "COMMENT";


            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.Cols["L_NO"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["COLUMN"].TextAlign = TextAlignEnum.LeftCenter;
            grdMain.Cols["NULLABLE"].TextAlign = TextAlignEnum.LeftCenter;
            grdMain.Cols["TYPE"].TextAlign = TextAlignEnum.LeftCenter;
            grdMain.Cols["COMMENT"].TextAlign = TextAlignEnum.LeftCenter;

            grdMain.Cols["L_NO"].Width = 50;
            grdMain.Cols["COLUMN"].Width = 200;
            grdMain.Cols["NULLABLE"].Width = 150;
            grdMain.Cols["TYPE"].Width = 150;
            grdMain.Cols["COMMENT"].Width = 300;

        }

        private bool SetDataBinding()
        {
            try
            {
                txtTbId.Value = txtTbId.Text;

                string sql1 = "";
                sql1 += string.Format(@" SELECT ROWNUM AS L_NO                                                                         
                                             ,X.*                                                                                            
                                         FROM (                                                                                              
                                                SELECT 'False' AS SEL 
                                                       ,BIZ_GP as OP_BIZ_GP
                                                       ,B.CD_NM AS OP_BIZ_GP_NM
                                                       ,SP_ID
                                                       ,SP_NM
                                                       ,MAIN_BI_TABLE
                                                       ,MAIN_MV_TABLE
                                                       ,BI_DATA_CNT
                                                       ,MV_DATA_CNT
                                                       ,ERROR_YN
                                                       ,ERROR_MSG
                                                       ,BEF_EXEC_DDTT
                                                       ,LAST_EXEC_DDTT
                                                       ,A.MOD_DDTT
                                                       ,MV_SELECT_WHERE
                                                FROM   PHQMS.TB_SP_TABLE A
                                                       LEFT OUTER JOIN TB_CM_COM_CD B ON A.BIZ_GP = B.CD_ID
                                                WHERE  A.USE_YN       = 'Y'
                                                AND    A.BIZ_GP    LIKE '{0}' || '%'
                                                AND    B.CD_NM     LIKE '%' || '{1}' || '%'
                                                AND    A.ERROR_YN  LIKE '{2}' || '%'
                                                AND    DECODE(MV_DATA_CNT-BI_DATA_CNT,0,'N','Y') LIKE '{3}'
                                                ORDER BY BIZ_GP,SP_ID,MAIN_BI_TABLE,MAIN_MV_TABLE
                                              )X   ", op_biz_gp, txtTbId.Text, err_yn, diff_yn);

                olddt = cd.FindDataTable(sql1);

                logdt = olddt.Copy();

                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt.Rows.Count.ToString() + "건이 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }
            return true;
        }




    }
}
