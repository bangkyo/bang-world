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

namespace SystemControlClassLibrary.phqms
{
    public partial class ColDataMgmt : Form
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

        string biz_gp, err_yn, oper_gp;

        C1FlexGrid selectedGrd = null;

        string _biz_gp, _sel, _mv_table_id;

        public ColDataMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();

            Load += ColDataMgmt_Load;

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

        private void ColDataMgmt_Load(object sender, System.EventArgs e)
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
            cs.InitLabel(label2);
            cs.InitLabel(label3);
            cs.InitLabel(label4);

            cs.InitTextBox(txtMView);

            cs.InitCombo(cbBiz_gp);
            cs.InitCombo(cbErr_yn);
            cs.InitCombo(cbOper_gp);

            // Button Color Set
            cs.InitButton(btnExcel);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);
            cs.InitButton(btnCDReg);
            cs.InitButton(btnRefresh);
            cs.InitButton(btnRecord);

            cs.InitCombo(cbBiz_gp, StringAlignment.Near);
            cs.InitCombo(cbErr_yn, StringAlignment.Near);
            cs.InitCombo(cbOper_gp, StringAlignment.Near);

            cd.SetCombo(cbBiz_gp, "BIZ_GP", true, ck.Biz_gp);
            cd.SetCombo(cbErr_yn, "ERR_YN", true, ck.Err_yn);
            cd.SetCombo(cbOper_gp, "OPER_GP", true, ck.Oper_gp);

            InitGrid();
        }

        private void cbBiz_gp_SelectedIndexChanged(object sender, EventArgs e)
        {
            biz_gp = ((ComLib.DictionaryList)cbBiz_gp.SelectedItem).fnValue;
            ck.Biz_gp = biz_gp;
        }

        private void cbErr_yn_SelectedIndexChanged(object sender, EventArgs e)
        {
            err_yn = ((ComLib.DictionaryList)cbErr_yn.SelectedItem).fnValue;
            ck.Err_yn = err_yn;
        }

        private void cbOper_gp_SelectedIndexChanged(object sender, EventArgs e)
        {
            oper_gp = ((ComLib.DictionaryList)cbOper_gp.SelectedItem).fnValue;
            ck.Oper_gp = oper_gp;
        }

        private void btnCDReg_Click(object sender, EventArgs e)
        {
            ColDataReg popup = new ColDataReg();
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (_sel == "True")
            {
                ColDataRefresh cdr = new ColDataRefresh(_biz_gp, _mv_table_id);
                cdr.Owner = this;
                cdr.StartPosition = FormStartPosition.CenterScreen;
                cdr.ShowDialog();
            }
            else
            {
                MessageBox.Show("Refresh 항목을 선택해주세요.");
                return;
            }

            
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (_sel == "True")
            {
                ColDataRecord cdRec = new ColDataRecord(_biz_gp, _mv_table_id);
                cdRec.Owner = this;
                cdRec.StartPosition = FormStartPosition.CenterScreen;
                cdRec.ShowDialog();
            }
            else
            {
                MessageBox.Show("이력조회 항목을 선택해주세요.");
                return;
            }
        }

        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {

        }

        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {

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
            if (grd.GetData(selected_Row, "BIZ_GP") != null) _biz_gp = grd.GetData(selected_Row, "BIZ_GP").ToString();
            if (grd.GetData(selected_Row, "SEL") != null) _sel = grd.GetData(selected_Row, "SEL").ToString();
            if (grd.GetData(selected_Row, "MV_TABLE_ID") != null) _mv_table_id = grd.GetData(selected_Row, "MV_TABLE_ID").ToString();

            //MessageBox.Show(_sel);
            //InitGridData(false);
            //SetDataBindingGrdSub(_line_gp, _work_date, _work_rank, _item_size, _steel, _length);
        }

        private void InitGrid()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            grdMain.AllowEditing = true;

            grdMain.Cols["L_NO"].Caption = "NO";
            grdMain.Cols["SEL"].Caption = "선택";
            grdMain.Cols["BIZ_GP"].Caption = "업무구분ID";
            grdMain.Cols["BIZ_GP_NM"].Caption = "업무구분명";
            grdMain.Cols["MV_TABLE_ID"].Caption = "MV테이블ID";
            grdMain.Cols["MV_TABLE_NM"].Caption = "MV테이블명";
            grdMain.Cols["OPER_TABLE_ID"].Caption = "운영TABLE ID";
            grdMain.Cols["DB_LINK_ID"].Caption = "DB LINK ID";
            grdMain.Cols["OPER_DATA_CNT"].Caption = "운영데이터건수";
            grdMain.Cols["MV_DATA_CNT"].Caption = "MV데이터건수";
            grdMain.Cols["MLOG_DATA_CNT"].Caption = "MLOG건수";
            grdMain.Cols["ERR_YN"].Caption = "오류여부";
            grdMain.Cols["ERR_MSG"].Caption = "오류메세지";
            grdMain.Cols["LAST_REFRESH"].Caption = "최종Refresh일시";
            

            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.Cols["L_NO"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["SEL"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["BIZ_GP"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["BIZ_GP_NM"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["MV_TABLE_ID"].TextAlign = TextAlignEnum.LeftCenter;
            grdMain.Cols["MV_TABLE_NM"].TextAlign = TextAlignEnum.LeftCenter;
            grdMain.Cols["OPER_TABLE_ID"].TextAlign = TextAlignEnum.LeftCenter;
            grdMain.Cols["DB_LINK_ID"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["OPER_DATA_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["MV_DATA_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["MLOG_DATA_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["ERR_YN"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["ERR_MSG"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["LAST_REFRESH"].TextAlign = TextAlignEnum.LeftCenter;

            grdMain.Cols["L_NO"].Width = 50;
            grdMain.Cols["SEL"].Width = 50;
            grdMain.Cols["BIZ_GP"].Width = 100;
            grdMain.Cols["BIZ_GP_NM"].Width = 100;
            grdMain.Cols["MV_TABLE_ID"].Width = 280;
            grdMain.Cols["MV_TABLE_NM"].Width = 250;
            grdMain.Cols["OPER_TABLE_ID"].Width = 280;
            grdMain.Cols["DB_LINK_ID"].Width = 100;
            grdMain.Cols["OPER_DATA_CNT"].Width = 150;
            grdMain.Cols["MV_DATA_CNT"].Width = 150;
            grdMain.Cols["MLOG_DATA_CNT"].Width = 100;
            grdMain.Cols["ERR_YN"].Width = 100;
            grdMain.Cols["ERR_MSG"].Width = 200;
            grdMain.Cols["LAST_REFRESH"].Width = 200;
        }

        private bool SetDataBinding()
        {
            try
            {
                txtMView.Value = txtMView.Text;

                string sql1 = "";
                sql1 += string.Format(@" SELECT ROWNUM AS L_NO                                                                         
                                             ,X.*                                                                                            
                                         FROM (                                                                                              
                                                SELECT                                                                                       
                                                    'False' AS SEL                                                                           
                                                    , BIZ_GP
                                                    , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'BIZ_GP' AND CD_ID = BIZ_GP) AS BIZ_GP_NM                                                                    
                                                    , TABLE_ID AS MV_TABLE_ID                                                                
                                                    , TABLE_NM AS MV_TABLE_NM                                                                
                                                    , OPER_TABLE_ID                                                                          
                                                    , DB_LINK_ID                                                                             
                                                    , OPER_DATA_CNT                                                                          
                                                    , GATH_DATA_CNT AS MV_DATA_CNT                                                           
                                                    , MLOG_DATA_CNT                                                                          
                                                    , ERROR_YN AS ERR_YN                                                                     
                                                    , ERROR_MSG AS ERR_MSG                                                                   
                                                    , MOD_DDTT AS LAST_REFRESH                                                               
                                                    , DECODE(GATH_DATA_CNT - OPER_DATA_CNT, 0, 'MATCH', 'MISMATCH') AS OPER_GP               
                                                FROM TB_MV_TABLE                                                                             
                                                WHERE BIZ_GP LIKE '{0}'
                                                  AND ERROR_YN LIKE '{1}'
                                                  AND DECODE(OPER_DATA_CNT - GATH_DATA_CNT, 0, 'MATCH', 'MISMATCH') LIKE '{2}'
                                                  AND BIZ_GP LIKE '%' || '{3}' || '%' 
                                                  AND USE_YN = 'Y'
                                                ORDER BY BIZ_GP, TABLE_ID, OPER_TABLE_ID
                                              )X   ", biz_gp, err_yn, oper_gp, txtMView.Text);

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
