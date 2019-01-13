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
    public partial class AnalDataMgmt : Form
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

        public AnalDataMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();

            Load += AnalDataMgmt_Load;

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

        private void AnalDataMgmt_Load(object sender, System.EventArgs e)
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

            cs.InitTextBox(txtSpNm);

            cs.InitCombo(cbOpBiz_gp);
            cs.InitCombo(cbErr_yn);
            cs.InitCombo(cbDiff_yn);

            // Button Color Set
            cs.InitButton(btnExcel);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);
            cs.InitButton(btnADReg);
            cs.InitButton(btnSpRedo);
            cs.InitButton(btnRecord);

            cs.InitCombo(cbOpBiz_gp, StringAlignment.Near);
            cs.InitCombo(cbErr_yn, StringAlignment.Near);
            cs.InitCombo(cbDiff_yn, StringAlignment.Near);

            cd.SetCombo(cbOpBiz_gp, "OP_BIZ_GP", true, ck.Op_Biz_gp);
            cd.SetCombo(cbErr_yn, "ERR_YN", true, ck.Err_yn);
            cd.SetCombo(cbDiff_yn, "DIFF_YN", true, ck.Diff_yn);

            InitGrid();
        }

        private void cbOpBiz_gp_SelectedIndexChanged(object sender, EventArgs e)
        {
            op_biz_gp = ((ComLib.DictionaryList)cbOpBiz_gp.SelectedItem).fnValue;
            ck.Op_Biz_gp = op_biz_gp;
        }

        private void cbErr_yn_SelectedIndexChanged(object sender, EventArgs e)
        {
            err_yn = ((ComLib.DictionaryList)cbErr_yn.SelectedItem).fnValue;
            ck.Err_yn = err_yn;
        }

        private void cbDiff_yn_SelectedIndexChanged(object sender, EventArgs e)
        {
            diff_yn = ((ComLib.DictionaryList)cbDiff_yn.SelectedItem).fnValue;
            ck.Oper_gp = diff_yn;
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
            grdMain.Cols["SEL"].Caption = "선택";
            grdMain.Cols["OP_BIZ_GP"].Caption = "업무구분ID";
            grdMain.Cols["OP_BIZ_GP_NM"].Caption = "업무구분명";
            grdMain.Cols["SP_ID"].Caption = "SP ID";
            grdMain.Cols["SP_NM"].Caption = "한글명";
            grdMain.Cols["MAIN_BI_TABLE"].Caption = "BI테이블ID";
            grdMain.Cols["MAIN_MV_TABLE"].Caption = "MV테이블ID";
            grdMain.Cols["BI_DATA_CNT"].Caption = "BI데이터건수";
            grdMain.Cols["MV_DATA_CNT"].Caption = "MV데이터건수";
            grdMain.Cols["ERROR_YN"].Caption = "오류여부";
            grdMain.Cols["ERROR_MSG"].Caption = "오류메시지";
            grdMain.Cols["BEF_EXEC_DDTT"].Caption = "처리한시작일시";
            grdMain.Cols["LAST_EXEC_DDTT"].Caption = "처리할시작일시";
            grdMain.Cols["MOD_DDTT"].Caption = "데이터집계일시";
            grdMain.Cols["SUB_BIZ_GP"].Caption = "SUB업무구분";
            grdMain.Cols["WORK_SEQ"].Caption = "SP실행순서";
            grdMain.Cols["MV_SELECT_WHERE"].Caption = "MV_SELECT조건";


            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.Cols["L_NO"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["SEL"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["OP_BIZ_GP"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["OP_BIZ_GP_NM"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["SP_ID"].TextAlign = TextAlignEnum.LeftCenter;
            grdMain.Cols["SP_NM"].TextAlign = TextAlignEnum.LeftCenter;
            grdMain.Cols["MAIN_BI_TABLE"].TextAlign = TextAlignEnum.LeftCenter;
            grdMain.Cols["MAIN_MV_TABLE"].TextAlign = TextAlignEnum.LeftCenter;
            grdMain.Cols["BI_DATA_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["MV_DATA_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["ERROR_YN"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["ERROR_MSG"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["BEF_EXEC_DDTT"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["LAST_EXEC_DDTT"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["MOD_DDTT"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["SUB_BIZ_GP"].TextAlign = TextAlignEnum.LeftCenter;
            grdMain.Cols["WORK_SEQ"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["MV_SELECT_WHERE"].TextAlign = TextAlignEnum.LeftCenter;


            grdMain.Cols["L_NO"].Width = 50;
            grdMain.Cols["SEL"].Width = 50;
            grdMain.Cols["OP_BIZ_GP"].Width = 100;
            grdMain.Cols["OP_BIZ_GP_NM"].Width = 100;
            grdMain.Cols["SP_ID"].Width = 250;
            grdMain.Cols["SP_NM"].Width = 200;
            grdMain.Cols["MAIN_BI_TABLE"].Width = 280;
            grdMain.Cols["MAIN_MV_TABLE"].Width = 280;
            grdMain.Cols["BI_DATA_CNT"].Width = 150;
            grdMain.Cols["MV_DATA_CNT"].Width = 150;
            grdMain.Cols["ERROR_YN"].Width = 100;
            grdMain.Cols["ERROR_MSG"].Width = 200;
            grdMain.Cols["BEF_EXEC_DDTT"].Width = 200;
            grdMain.Cols["LAST_EXEC_DDTT"].Width = 200;
            grdMain.Cols["MOD_DDTT"].Width = 200;
            grdMain.Cols["SUB_BIZ_GP"].Width = 100;
            grdMain.Cols["WORK_SEQ"].Width = 100;
            grdMain.Cols["MV_SELECT_WHERE"].Width = 600;

        }

        private bool SetDataBinding()
        {
            try
            {
                txtSpNm.Value = txtSpNm.Text;

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
                                                       ,SUB_BIZ_GP
                                                       ,WORK_SEQ
                                                       ,MV_SELECT_WHERE
                                                FROM   PHQMS.TB_SP_TABLE A
                                                       LEFT OUTER JOIN TB_CM_COM_CD B ON A.BIZ_GP = B.CD_ID
                                                WHERE  A.USE_YN       = 'Y'
                                                AND    A.BIZ_GP    LIKE '{0}' || '%'
                                                AND    B.CD_NM     LIKE '%' || '{1}' || '%'
                                                AND    A.ERROR_YN  LIKE '{2}' || '%'
                                                AND    DECODE(NVL(MV_DATA_CNT-BI_DATA_CNT,0),0,'Y','N') LIKE '{3}'
                                                ORDER BY BIZ_GP,SP_ID,MAIN_BI_TABLE,MAIN_MV_TABLE
                                              )X   ", op_biz_gp, txtSpNm.Text, err_yn, diff_yn);

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
