using ComLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using ComLib.clsMgr;
using System.Collections;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace BGsystemLibrary.EQPCheckMgmt
{
    public partial class EQPCheckRslt : Form
    {
        #region 공통 생성자
        //공통변수
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();

        //데이터테이블
        DataTable olddtMain;
        DataTable moddtMain;

        //그리드 변수
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();
        private int main_GridRowsCount = 2;
        private int main_GridColsCount = 15;
        private int main_RowsFixed = 2;
        private int main_RowsFrozen = 0;
        private int main_ColsFixed = 0;
        private int main_ColsFrozen = 0;



        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;

        // 프로그램 명 변수
        private static string ownerNM = "";
        private static string titleNM = "";

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한

        #endregion 공통 생성자
        public EQPCheckRslt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            string[] scrAuthParams = scrAuth.Split(',');

            this.scrAuthInq = scrAuthParams[1];   //조회 권한 저장
            this.scrAuthReg = scrAuthParams[2];   //등록(추가) 권한 저장
            this.scrAuthMod = scrAuthParams[3];   //수정 권한 저장
            this.scrAuthDel = scrAuthParams[4];   //삭제 권한 저장

            InitializeComponent();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
        }

        private void EQPCheckRslt_Load(object sender, EventArgs e)
        {
            //그리드 초기화
            DrawMainGrid(grdMain);
            //초기화
            InitControl();
        }

        private void InitControl()
        {

            cd.SetCombo(cbx_Eq_Gp, GetEqData(), "", true, 0, 1);

            SetCheckItemCB();
        }

        private string GetEqItemData()
        {
            string sql = string.Empty;

            sql = string.Format(@"SELECT DISTINCT ITEM_CD
                                       , CHECK_ITEM
                                    FROM TB_EQP_CHECK_ITEM A
                                   WHERE EQP_CD LIKE '{0}'
                                     AND USE_YN = 'Y'", ((ComLib.DictionaryList)this.cbx_Eq_Gp.SelectedItem).fnValue);

            return sql;
        }

        private string GetEqData()
        {
            string sql = string.Empty;

            sql = string.Format(@"SELECT A.EQP_CD
                                       , (SELECT EQP_NM FROM TB_EQP_INFO WHERE EQP_CD = A.EQP_CD) AS EQP_NM
                                    FROM (
                                    		SELECT DISTINCT EQP_CD 
                                    		  FROM TB_EQP_CHECK_ITEM
                                    		 WHERE USE_YN = 'Y'
                                         ) A ");

            return sql;
        }

        private void DrawMainGrid(C1FlexGrid grdItem)
        {
            grdItem.BeginInit();
            try
            {
                int _GridRowsCount = main_GridRowsCount;
                int _GridColsCount = main_GridColsCount;
                int _RowsFixed = main_RowsFixed;
                int _RowsFrozen = main_RowsFrozen;
                int _ColsFixed = main_ColsFixed;
                int _ColsFrozen = main_ColsFrozen;

                //그리드 그리기
                if (grdItem.Rows.Count == 1)
                {
                    clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);
                }
                else
                {
                    clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);
                }
                //컬럼 스타일 세팅
                FlexMainGridCol(grdItem);
                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                MessageBox.Show(ex.ToString() + ex.Message);
            }
            finally
            {
                grdItem.EndInit();
                grdItem.Invalidate();
            }
        }

        private void FlexMainGridCol(C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 100;
            grdItem.Cols[2].Width = 100;
            grdItem.Cols[3].Width = 100;
            grdItem.Cols[4].Width = 100;
            grdItem.Cols[5].Width = 100;
            grdItem.Cols[6].Width = 200;
            grdItem.Cols[7].Width = 150;
            grdItem.Cols[8].Width = 100;
            grdItem.Cols[9].Width = 100;
            grdItem.Cols[10].Width = 100;
            grdItem.Cols[11].Width = 100;
            grdItem.Cols[12].Width = 400;
            grdItem.Cols[13].Width = 100;
            grdItem.Cols[14].Width = 100;
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "설비코드";
            grdItem[1, 2] = "항목코드";
            grdItem[1, 3] = "점검순서";
            grdItem[1, 4] = "공정명";
            grdItem[1, 5] = "설비명";
            grdItem[1, 6] = "점검항목";
            grdItem[1, 7] = "점검계획일자";
            grdItem[1, 8] = "점검간격";
            grdItem[1, 9] = "점검주기";
            grdItem[1, 10] = "점검여부";
            grdItem[1, 11] = "점검일자";
            grdItem[1, 12] = "점검내용";
            grdItem[1, 13] = "사용횟수";
            grdItem[1, 14] = "상태";

            for (int col = 0; col < grdItem.Cols.Count; col++)
            {
                if (col == 10 ||  col == 12)
                {
                    grdItem.Cols[col].AllowEditing = true;
                }
                else
                    grdItem.Cols[col].AllowEditing = false;

                //grdItem.Cols[col].AllowEditing = false;

            }

            grdItem.Cols[1].Visible = false;
            grdItem.Cols[2].Visible = false;
            grdItem.Cols[3].Visible = false;
            grdItem.Cols[4].Visible = false;


            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 14);


            TextBox tbEditor = new TextBox();

            tbEditor.MaxLength = 50;
            //tbEditor.KeyPress += TbEditor_KeyPress;

            grdItem.Cols[12].Editor = tbEditor;

        }



        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnDisplay":

                    if (this.scrAuthInq != "Y")
                    {
                        MessageBox.Show("조회 권한이 없습니다");
                        return;
                    }

                    cd.InsertLogForSearch(ck.UserID, btnDisplay, "");

                    SetDataBinding();

                    break;



                case "btnSave":
                    if (this.scrAuthMod != "Y")
                    {
                        MessageBox.Show("수정 권한이 없습니다");
                        return;
                    }
                    SaveData();
                    break;


                case "btnExcel":
                    SaveExcel();
                    break;
                case "btnAddRes":
                    AddNewResult();
                    break;
            }
        }

        private void AddNewResult()
        {
            var temp_form = GetForm("EqmCheckResultReg");

            if (temp_form == null)
            {
                var sub = new Common.EqmCheckResultReg("설비점검실적등록", "", "");

                //sub.MdiParent = this;
                sub.StartPosition = FormStartPosition.CenterScreen;

                sub.FormBorderStyle = FormBorderStyle.FixedDialog;
                // Set the MaximizeBox to false to remove the maximize box.
                sub.MaximizeBox = false;
                // Set the MinimizeBox to false to remove the minimize box.
                sub.MinimizeBox = false;
                //sub.ShowDialog();
                sub.Show();
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }

        public static Form GetForm(string formName)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == formName)
                {
                    return frm;
                }
            }
            return null;

        }


        private void SaveExcel()
        {
            vf.SaveExcel(titleNM, grdMain);
        }

        private void SaveData()
        {
            if (this.scrAuthMod != "Y")
            {
                MessageBox.Show("수정 권한이 없습니다");
                return;
            }

            if (!IsSaveItem())
            {
                MessageBox.Show("수정된 항목이 없습니다.");
                return;
            }


            string _eqp_cd = "";
            string _itemCd = "";
            string _checkSeq = "";
            string _check_date = "";
            string _check_contents = "";
            string _check_YN = "";
            string sql = "";
            int UpCnt = 0;

            SqlConnection conn = cd.OConnect();

            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = null;

            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                this.Cursor = Cursors.AppStarting;
                for (int row = main_GridRowsCount; row < grdMain.Rows.Count; row++)
                {
                    if (grdMain.GetData(row, "L_NUM").ToString() == "수정")
                    {
                        _eqp_cd = grdMain.GetData(row, "EQP_CD").ToString();
                        _itemCd = grdMain.GetData(row, "ITEM_CD").ToString();
                        _checkSeq = grdMain.GetData(row, "CHECK_SEQ").ToString();
                        _check_date = vf.Format((DateTime)grdMain.GetData(row, "CHECK_WR_DATE"), "yyyyMMdd");
                        _check_contents = grdMain.GetData(row, "CHECK_CNTS").ToString().Trim();
                        _check_YN = vf.StringToString(grdMain.GetData(row, "CHECK_YN").ToString());

                        sql = string.Format(" UPDATE TB_EQP_CHECK_WR                                      ");
                        sql += string.Format(" SET                                                         ");
                        sql += string.Format("        CHECK_WR_DATE = '{0}'                                ", _check_date);
                        sql += string.Format("       ,PROG_STAT = '{0}'                                    ", "END");
                        sql += string.Format("       ,CHECK_CNTS = '{0}'                                   ", _check_contents);
                        sql += string.Format("       ,CHECK_YN = '{0}'                                     ", _check_YN);
                        sql += string.Format("       ,MODIFIER = '{0}'                                     ", ck.UserID);
                        sql += string.Format("       ,MOD_DDTT = (SELECT CONVERT(VARCHAR, GETDATE(), 120)) ");
                        sql += string.Format(" WHERE EQP_CD = '{0}'                                       ", _eqp_cd);
                        sql += string.Format("   AND ITEM_CD = '{0}'                                      ", _itemCd);
                        sql += string.Format("   AND CHECK_SEQ = '{0}'                                    ", _checkSeq);

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        UpCnt++;
                    }

                }

                this.Cursor = Cursors.Default;

                //lastSearchHeatNo = _heat_no;

                //실행후 성공
                transaction.Commit();

                //SetDataBinding();

                // 성공시에 추가 수정 삭제 상황을 초기화시킴

                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), message);

                Button_Click(btnDisplay, null);

            }
            catch (SqlException ex)
            {
                Console.WriteLine("ex.message : {0}", ex.Message);
            }
            finally
            {
                
            }
        }

        private bool IsSaveItem()
        {
            bool rtn = false;

            DataTable dt = (DataTable)grdMain.DataSource;

            if (dt == null) return false;

            foreach (DataRow row in dt.Rows)
            {
                if (row[0].ToString() == "수정")
                {
                    return rtn = true;
                }
            }

            return rtn;
        }

        private void SetDataBinding()
        {
            clsFlexGrid.grdDataClearForBind(grdMain);

            try
            {

                string _start_dt = vf.Format(start_dt.Value, "yyyyMMdd");
                string _end_dt = vf.Format(end_dt.Value.AddDays(1), "yyyyMMdd");

                string sql = "";

                sql += string.Format(@"  SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY B.EQP_CD, B.ITEM_CD, B.CHECK_SEQ ASC)) AS L_NUM
                                                 , B.EQP_CD
                                                 , B.ITEM_CD
                                                 , B.CHECK_SEQ
                                            	 , B.ROUTING_NM
                                                 , B.EQP_NM
                                                 , B.CHECK_ITEM
                                                 , CONVERT(DATE, B.CHECK_PLN_DATE) AS CHECK_PLN_DATE
                                                 , B.CHECK_GAP
                                            	 , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_CYCLE' AND USE_YN = 'Y' AND CD_ID = CHECK_CYCLE) AS CHECK_CYCLE_NM
                                            	 , B.CHECK_YN
                                                 , CONVERT(DATE, B.CHECK_WR_DATE) AS CHECK_WR_DATE
                                                 , B.CHECK_CNTS
                                                 , B.USE_CNT
                                                 , CASE  WHEN B.CHECK_CYCLE = 'W' AND MAX_CHECK_END_DATE < CONVERT(CHAR(8), GETDATE(), 112) THEN (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_STAT' AND CD_ID = 'FIN' AND USE_YN = 'Y')
												         WHEN B.CHECK_CYCLE = 'W' AND B.PROG_STAT = 'END' AND B.CHECK_WR_DATE = CONVERT(CHAR(8), GETDATE(), 112) THEN (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_STAT' AND CD_ID = 'CHK' AND USE_YN = 'Y')
                                                         WHEN B.CHECK_CYCLE = 'W' AND B.PROG_STAT = 'WAT' AND B.CHECK_PLN_DATE < CONVERT(CHAR(8), GETDATE(), 112) THEN (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_STAT' AND CD_ID = 'DELAY' AND USE_YN = 'Y')
														 WHEN B.CHECK_CYCLE = 'C' AND A.MAX_CHECK_GAP < B.USE_CNT THEN (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_STAT' AND CD_ID = 'DELAY' AND USE_YN = 'Y')
														 ELSE (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_STAT' AND CD_ID = B.PROG_STAT AND USE_YN = 'Y') 
														 END AS PROG_STAT 
                                              FROM (
						                                SELECT A.EQP_CD
                                            				 , A.ITEM_CD
                                            				 , MAX(A.CHECK_SEQ)     AS LAST_CHECK_SEQ
                                                             , MAX(A.CHECK_WR_DATE) AS LAST_CHECK_WR_DATE
															 , MAX(B.CHECK_END_DATE) AS MAX_CHECK_END_DATE
															 , MAX(B.CHECK_GAP) AS MAX_CHECK_GAP
                                            			  FROM TB_EQP_CHECK_WR A
														     LEFT OUTER JOIN  TB_EQP_CHECK_ITEM B
													         ON A.EQP_CD  = B.EQP_CD
								                             AND A.ITEM_CD = B.ITEM_CD
                                            			GROUP BY A.EQP_CD
                                            				 , A.ITEM_CD
                                            	   ) A
                                            	 , TB_EQP_CHECK_WR B
                                             WHERE A.EQP_CD = B.EQP_CD
                                               AND A.ITEM_CD = B.ITEM_CD
                                               AND A.LAST_CHECK_SEQ = B.CHECK_SEQ
                                               AND '%' = '{0}'
                                         UNION ALL
                                            SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY M.EQP_CD, M.ITEM_CD, M.CHECK_SEQ ASC)) AS L_NUM
											     , M.EQP_CD
												 , M.ITEM_CD
												 , M.CHECK_SEQ
												 , M.ROUTING_NM
												 , M.EQP_NM
												 , M.CHECK_ITEM
												 , M.CHECK_PLN_DATE
												 , M.CHECK_GAP
												 , M.CHECK_CYCLE_NM
												 , M.CHECK_YN
												 , M.CHECK_WR_DATE
												 , M.CHECK_CNTS
												 , M.USE_CNT
												 , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_STAT' AND CD_ID = M.PROG_STAT AND USE_YN = 'Y')  AS PROG_STAT
										      FROM (
													SELECT B.EQP_CD
														 , B.ITEM_CD
														 , B.CHECK_SEQ
														 , B.ROUTING_NM
														 , B.EQP_NM
														 , B.CHECK_ITEM
														 , CONVERT(DATE, B.CHECK_PLN_DATE) AS CHECK_PLN_DATE
														 , B.CHECK_GAP
														 , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_CYCLE' AND USE_YN = 'Y' AND CD_ID = B.CHECK_CYCLE) AS CHECK_CYCLE_NM
														 , B.CHECK_YN
														 , CONVERT(DATE, B.CHECK_WR_DATE) AS CHECK_WR_DATE
														 , B.CHECK_CNTS
														 , B.USE_CNT
														 , CASE  WHEN B.CHECK_CYCLE = 'W' AND MAX_CHECK_END_DATE < CONVERT(CHAR(8), GETDATE(), 112) THEN 'FIN' -- (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_STAT' AND CD_ID = 'FIN' AND USE_YN = 'Y')
																 WHEN B.CHECK_CYCLE = 'W' AND B.PROG_STAT = 'END' AND B.CHECK_WR_DATE = CONVERT(CHAR(8), GETDATE(), 112) THEN 'CHK'-- (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_STAT' AND CD_ID = 'CHK' AND USE_YN = 'Y')
																 WHEN B.CHECK_CYCLE = 'W' AND B.PROG_STAT = 'WAT' AND B.CHECK_PLN_DATE < CONVERT(CHAR(8), GETDATE(), 112) THEN  'DELAY'--(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_STAT' AND CD_ID = 'DELAY' AND USE_YN = 'Y')
																 WHEN B.CHECK_CYCLE = 'C' AND A.MAX_CHECK_GAP < B.USE_CNT THEN 'DELAY'-- (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_STAT' AND CD_ID = 'DELAY' AND USE_YN = 'Y')
																 ELSE B.PROG_STAT --(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_STAT' AND CD_ID = B.PROG_STAT AND USE_YN = 'Y') 
																 END AS PROG_STAT 
													  FROM (
																SELECT A.EQP_CD
                                            						 , A.ITEM_CD
                                            						 , MAX(A.CHECK_SEQ)      AS LAST_CHECK_SEQ
																	 , MAX(A.CHECK_WR_DATE)  AS LAST_CHECK_WR_DATE
																	 , MAX(B.CHECK_END_DATE) AS MAX_CHECK_END_DATE
																	 , MAX(B.CHECK_GAP)      AS MAX_CHECK_GAP
                                            					  FROM TB_EQP_CHECK_WR A
																	 LEFT OUTER JOIN  TB_EQP_CHECK_ITEM B
																	 ON A.EQP_CD  = B.EQP_CD
																	 AND A.ITEM_CD = B.ITEM_CD
                                            					GROUP BY A.EQP_CD
                                            						 , A.ITEM_CD
                                            			   ) A
                                            			 , TB_EQP_CHECK_WR B
													 WHERE A.EQP_CD  = B.EQP_CD
													   AND A.ITEM_CD = B.ITEM_CD 
												   ) M
											 WHERE '%' != '{1}'
										       AND M.CHECK_PLN_DATE  >= '{2}' 
                                               AND M.CHECK_PLN_DATE  <  '{3}'
                                               AND M.EQP_CD          LIKE '{4}'
                                               AND M.ITEM_CD         LIKE '{5}'
                                               AND (
														(
															    'Y' = '{6}'
															AND M.PROG_STAT = 'DELAY'
														)
														OR
														(
														        'N' = '{6}'
														)
												   )"
                                                               , ((ComLib.DictionaryList)this.cbx_Eq_Gp.SelectedItem).fnValue
                                                               , ((ComLib.DictionaryList)this.cbx_Eq_Gp.SelectedItem).fnValue
                                                               , _start_dt
                                                               , _end_dt
                                                               , ((ComLib.DictionaryList)this.cbx_Eq_Gp.SelectedItem).fnValue
                                                               , ((ComLib.DictionaryList)this.cbx_CheckItem_Gp.SelectedItem).fnValue
                                                               , (cb_DelayYN_Gp.Checked) ? "Y" : "N"
                                                               );
                //SQL 쿼리 조회 후 데이터셋 return
                olddtMain = cd.FindDataTable(sql);
                moddtMain = olddtMain.Copy();


                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain, moddtMain);

                //PROG_STAT: 지연(DELAY) 인 경우 붉은 색으로 처리
                SetDelayStateInGrd();

                grdMain.Row = -1;
                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} : {moddtMain.Rows.Count} 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void SetDelayStateInGrd()
        {
           
            for (int row = main_GridRowsCount; row < grdMain.Rows.Count; row++)
            {

                if (grdMain.GetData(row, "PROG_STAT").ToString().Equals("지연"))
                {
                    SetDelayRowMode(grdMain, row, grdMain.Cols["PROG_STAT"].Index);
                }
            }
        }

        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {

                DataTable dt = GetCheckCycle();
                ListDictionary dataMap = new ListDictionary();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataMap.Add(dt.Rows[i].ItemArray[0].ToString(), dt.Rows[i].ItemArray[1].ToString());
                }

                //그리드 콤보박스 지정
                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("CHECK_PLN_DATE", FlexGridCellTypeEnum.TimePicker, DateTime.Now.Date);
                FlexGridColumns.Add("CHECK_WR_DATE", FlexGridCellTypeEnum.TimePicker, DateTime.Now.Date);
                FlexGridColumns.Add("CHECK_YN", FlexGridCellTypeEnum.CheckBox, "Y");
                //FlexGridColumns.Add("CHECK_CYCLE", FlexGridCellTypeEnum.ComboBox, dataMap);

                //그리드 데이터테이블 바인딩
                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, FlexGridColumns, true);
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                //그리드 스크롤바 세팅
                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;

                //상태창 검색 로우수 출력
                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), RowsCount, "조회하였습니다.");
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                MessageBox.Show(ex.ToString() + ex.Message);
            }
            finally
            {
                grdItem.EndUpdate();
                grdItem.Invalidate();
            }
        }

        private DataTable GetCheckCycle()
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "";
                sql += string.Format(@" SELECT CD_ID
                                             , CD_NM
                                          FROM TB_CM_COM_CD
                                         WHERE CATEGORY = 'CHECK_CYCLE'
                                           AND USE_YN = 'Y'");

                dt = cd.FindDataTable(sql);
            }
            catch (Exception ex)
            {

            }


            return dt;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void cbx_Eq_Gp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComLib.DictionaryList)this.cbx_Eq_Gp.SelectedItem).fnValue == "%")
            {
                SetCheckItemCB();

                cbx_CheckItem_Gp.Enabled = false;

                cb_DelayYN_Gp.Enabled = false;

                Button_Click(btnDisplay, null);

            }else
            {
                cbx_CheckItem_Gp.Enabled = true;

                cb_DelayYN_Gp.Enabled = true;

                SetCheckItemCB();
            }
            
        }

        private void SetCheckItemCB()
        {
            cd.SetCombo(cbx_CheckItem_Gp, GetEqItemData(), "", true, 0, 1);
        }
        private string strBefValue;
        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;
           
            int editedRow = e.Row;
            int editedCol = e.Col;

            if (editedRow < main_GridRowsCount)
            {
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue = grd.GetData(editedRow, editedCol).ToString();
        }

        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            string strAftValue = editedGrd.GetData(editedRow, editedCol).ToString().Trim();

            if (string.Equals(strBefValue, strAftValue, StringComparison.CurrentCulture))
            {
                return;
            }
            else
            {
                SetModifiedMode(editedGrd, editedRow);
            }

            strBefValue = "";
        }

        private void SetModifiedMode(C1FlexGrid _editedGrd, int _editedRow)
        {
            _editedGrd.SetData(_editedRow, 0, "수정");

            // Update 배경색 지정
            //editedGrd.Rows[editedRow].Style = editedGrd.Styles["UpColor"];
            clsFlexGrid.GridCellRangeStyleColor(_editedGrd, _editedRow, 0, _editedRow, _editedGrd.Cols.Count - 1, Color.Green, Color.Black);
        }

        private void SetDelayRowMode(C1FlexGrid _editedGrd, int _editedRow)
        {
            // Update 배경색 지정
            //editedGrd.Rows[editedRow].Style = editedGrd.Styles["UpColor"];
            clsFlexGrid.GridCellRangeStyleColor(_editedGrd, _editedRow, 0, _editedRow, _editedGrd.Cols.Count - 1, Color.Red, Color.Black);
        }

        private void SetDelayRowMode(C1FlexGrid _editedGrd, int _editedRow, int ColNm)
        {
            // Update 배경색 지정
            //editedGrd.Rows[editedRow].Style = editedGrd.Styles["UpColor"];
            clsFlexGrid.GridCellRangeStyleColor(_editedGrd, _editedRow, ColNm, _editedRow, ColNm, Color.Red, Color.Black);
        }

        private void grdMain_CellChecked(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            // 점검체크시 현재 날자로 실적 등록

            if (editedGrd.GetCellCheck(editedRow, editedCol).ToString() == "Checked")
            {
                //editedGrd.Rows[editedRow][""] = "True";
                editedGrd.SetData(editedRow, "CHECK_WR_DATE", DateTime.Now.Date);
            }
            else
            {
                //editedGrd.Rows[editedRow][""] = "False";
                editedGrd.SetData(editedRow, "CHECK_WR_DATE", DateTime.Now.Date);
            }
        }

        private void start_dt_ValueChanged(object sender, EventArgs e)
        {
            if (DateTime.Compare(start_dt.Value, end_dt.Value) > 0)
            {
                start_dt.Value = end_dt.Value;
            }
        }

        private void end_dt_ValueChanged(object sender, EventArgs e)
        {
            if (DateTime.Compare(start_dt.Value, end_dt.Value) > 0)
            {
                end_dt.Value = start_dt.Value;
            }
        }
    }
}
