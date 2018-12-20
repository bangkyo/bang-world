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
using ComLib.clsMgr;
using C1.Win.C1FlexGrid;
using System.Data.OracleClient;
using SystemControlClassLibrary.Popup;

namespace SystemControlClassLibrary
{
    public partial class frmRB_RELABEL : Form
    {
        public frmRB_RELABEL()
        {
            InitializeComponent();
        }
        
        #region "변수 선언"

        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();

        DataTable olddt;
        DataTable moddt;

        DataTable grdMain_dt;

        // 셀의 수정전 값
        private static string ownerNM = "";
        private static string titleNM = "";

        C1FlexGrid selectedGrd = null;

        //전역변수 설정
        string FindHeat = string.Empty;

        #endregion

        #region "생성자.. 개중요함"

        //생성자!!! 중요함!!! 이걸로 메뉴 매핑해서 화면을 띄워줌
        public frmRB_RELABEL(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();

            //selectedGrd = grdMain;
        }

        #endregion

        #region "폼 로드"

        //폼 더블 클릭 시 LOAD함.. 화면이 첨 생성될 때 실행된다.
        private void frmRB_RELABEL_Load(object sender, EventArgs e)
        {
            InitControl();

            //MakeInitgrdData();

            //btnDisplay_Click(null, null);
            rdoCircle.Checked = true;

            this.WindowState = FormWindowState.Maximized;
        }

        #endregion

        #region "InitControl 설정"
        private void InitControl()
        {
            //grdMain.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange;

            //clsStyle.Style.InitPicture(pictureBox1);
            //clsStyle.Style.InitTitle(title_lb, ownerNM, titleNM);
            clsStyle.Style.InitGrid_search(grdMain);

            //clsStyle.Style.InitPanel(panel1);
            clsStyle.Style.InitPanel(panel2);

            //clsStyle.Style.InitButton(btnExcel);
            clsStyle.Style.InitButton(btnDisplay);
            //clsStyle.Style.InitButton(btnSave);
            clsStyle.Style.InitButton(btnClose);

            clsStyle.Style.InitCombo(cboFactory, StringAlignment.Near);
            clsStyle.Style.InitCombo(cboWorkTeam, StringAlignment.Near);

            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;

            //콤보박스 셋팅

            cd.SetCombo(cboWorkTeam, "14", false, "1");

            string sql = string.Empty;

            sql = string.Format(" SELECT  '' COMMON, '' CODE_NAME ");
            sql += string.Format(" FROM    DUAL ");
            sql += string.Format(" UNION ALL ");
            sql += string.Format(" SELECT  'PB3' COMMON, '봉강' CODE_NAME ");
            sql += string.Format(" FROM    DUAL ");
            sql += string.Format(" UNION ALL ");
            sql += string.Format(" SELECT  'PB4' COMMON, '대형강' CODE_NAME ");
            sql += string.Format(" FROM    DUAL ");
            sql += string.Format(" UNION ALL ");
            sql += string.Format(" SELECT  'PB5' COMMON, '중형강' CODE_NAME ");
            sql += string.Format(" FROM    DUAL ");

            //cd.SetComboOle(cboFactory, sql, false, "0", "MAIN");

            cboSquare.SelectedIndex = 0;
            cboCircle.SelectedIndex = 0;
            
            InitGrd_grdMain();
        }
        #endregion

        #region "버튼 컨트롤"

        //조회버튼 클릭
        public void btnDisplay_Click(object sender, EventArgs e)
        {
            //grpInput1.Enabled = false;

            //cd.InsertLogForSearch("MAIN", ck.UserID, btnDisplay);

            //string chk = cd.SELECT_CONTROL(start_dt.Text.Replace("-", ""), end_dt.Text.Replace("-", ""), "MAIN");

            //if (chk == "N")
            //{
            //    string sql = string.Empty;

            //    sql = string.Format(" SELECT   CODE_NAME ");
            //    sql += string.Format(" FROM    BARCODE_TB_CM_COM_dETAIL  ");
            //    sql += string.Format(" WHERE   CATEGORY = 'ZX' ");
            //    sql += string.Format(" AND     COMMON = 'ZX01'  ");

            //    //string LimitDay = cd.Redone("MAIN", sql);

            //    MessageBox.Show("조회기간은 " + LimitDay + "일 입니다. 조회일자 변경 후 다시 조회하세요!!!");
            //    return;
            //}

            //InitGridData();

            this.Cursor = Cursors.WaitCursor;

            SetDataBinding();

            //SelectedGrdMain(grdMain, grdMain.RowSel);

            this.Cursor = Cursors.Default;
        }
     
        //발행 버튼
        private void btnLabel_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            //발행 함수 호출
            dolabel();

            this.Cursor = Cursors.Default;

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + "발행이 완료되었습니다.");           
        }

        //닫기 버튼
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion      

        #region "FUNCTION"
        
        #region "쿼리데이터 그리드 바인딩"
        private void SetDataBinding()
        {
            string start_date = start_dt.Value.ToString();
            start_date = (start_date.Substring(0, 4) + start_date.Substring(5, 2) + start_date.Substring(8, 2));
            string end_date = end_dt.Value.ToString();
            end_date = (end_date.Substring(0, 4) + end_date.Substring(5, 2) + end_date.Substring(8, 2));

            string Flag = string.Empty;

            if(chkNo3.Checked == true)
            {
                Flag = "1";
            }
            else if(chkNo5.Checked == true)
            {
                Flag = "5";
            }
            else
            {
                Flag = "";
            }

            string InspectStatus = string.Empty;

            if(rdoD3PR.Checked == true)
            {
                InspectStatus = "D3PR";
            }
            else
            {
                InspectStatus = "D3RT";
            }

            string PrintStatus = string.Empty;

            if (rdoAll.Checked == true)
            {
                PrintStatus = "1";
            }
            else if (rdoPrint.Checked == true)
            {
                PrintStatus = "2";
            }
            else
            {
                PrintStatus = "3";
            }


            try
            {
                string sql = string.Empty;

                sql = string.Format(" /* kq.qms.sit.28191.getMainData */ ");
                sql += string.Format(" SELECT  I.INSPECT_STATUS2, I.INSPECT_SEQ,                                                                                "); //--00 INSPECT_SEQ 
                sql += string.Format("         I.INSPECT_LINE_NO YON,                                                                                           "); //--01 라벨출력유무
                sql += string.Format("         'False' AS P_YN,                                                                                                       "); //--02 출력여부
                sql += string.Format("         1 AS P_CNT,                                                                                                      "); //--03 출력매수
                sql += string.Format("         I.BUNDLE_NO,                                                                                                     "); //--04 번들 NO 
                sql += string.Format("         CASE WHEN I.FACTORY = 'PB3' THEN '봉강' WHEN I.FACTORY = 'PB4' THEN '대형강' WHEN I.FACTORY = 'PB5' THEN '중형강' ELSE 'ERROR' END  FACTORY,                                         "); //--05 공장
                sql += string.Format("         FN_BARCODE_GETCOMNAME(I.WORK_TEAM) WORK_TEAM,                                                                            "); //--06 근무조
                sql += string.Format("         FN_FORMATU(INSPECT_DATE,'####-##-##') INSPECT_DATE,                                                              "); //--07 검사일자
                sql += string.Format("         I.BONSU,                                                                                                         "); //--08 본수
                sql += string.Format("         I.WEIGHT,                                                                                                        "); //--09 중량
                sql += string.Format("         FN_BARCODE_GETCOMNAME(I.SIRF_CODE) SIRF_CODE_NAME,                                                                       "); //--10 합부
                sql += string.Format("         I.HEAT,                                                                                                          "); //--11 HEAT
                sql += string.Format("         I.ITEM || I.ITEM_SIZE || ':' || Z.SIZE_NAME ITEM_NAME,                                                           "); //--12 제품규격
                sql += string.Format("         I.STEEL,                                                                                                         "); //--13 강종
                sql += string.Format("         I.LENGTH,                                                                                                        "); //--14 길이 
                sql += string.Format("         FN_BARCODE_GETCOMNAME(I.UOM) LENGTHUOM,                                                                                  "); //--15 단위 
                sql += string.Format("         FN_BARCODE_GETCOMNAME(I.AFTER_ROUTING) AFTER_ROUTING_NAME,  "); //--16 공정, 
                sql += string.Format("         FN_BARCODE_GETCOMNAME(I.SURFACE_LEVEL) SURFACE_LEVEL_NAME,  "); //--17 표면등급,  
                sql += string.Format("         FN_BARCODE_GETCOMNAME(I.ITEM_USE) ITEM_USE_NAME,            "); //--18 용도 
                sql += string.Format("         (SELECT CUST_KO_NAME FROM BARCODE_COMPANY WHERE CUST_ID = I.COMPANY_SEQ) COMPANY,                         "); //--19 회사명(ENG)
                //sql += string.Format("         CASE WHEN (SELECT A.ENG_NAME FROM TB_COMPANY A WHERE A.COMPANY_TYPE = '70B' AND A.CIM_COMPANY = I.COMPANY_SEQ) IS NULL THEN  ");
                //sql += string.Format("                   (SELECT NVL(PO_CUST_ENG_NAME, CUST_EN_NAME) FROM TB_SM_CUSTINFO@DL_SALE WHERE CUST_CD = I.COMPANY_SEQ) ");
                //sql += string.Format("         ELSE (SELECT A.ENG_NAME FROM TB_COMPANY A WHERE A.COMPANY_TYPE = '70B' AND A.CIM_COMPANY = I.COMPANY_SEQ) END ENG_NAME, ");
                sql += string.Format("         (SELECT NVL(PO_CUST_ENG_NAME, CUST_EN_NAME) FROM BARCODE_COMPANY WHERE CUST_ID = I.COMPANY_SEQ) PO_CUST_ENG_NAME, ");
                sql += string.Format("         I.ITEM, Z.SIZE_NAME,                                                                                             "); // --20 제품, 21 규격명        
                sql += string.Format("         I.AFTER_ROUTING,                                                                                                 "); // --22 후처리공정 
                sql += string.Format("         I.SURFACE_LEVEL,                                                                                                 "); // --23 표면등급 
                sql += string.Format("         I.ITEM_USE,                                                                                                      "); // --24 사용용도  
                sql += string.Format("         I.SIRF_CODE ,                                                                                                    "); // --25 합부코드 
                sql += string.Format("         I.EMPLOYEE, ");
                sql += string.Format("         ST.STEEL_NAME, ");
                sql += string.Format("         ST.STLKIND_SPEC_CD COUNTRY_BASIS, ");
                sql += string.Format("         ST.STLKIND_SPEC_NAME SPEC_NUM, ");
                sql += string.Format("         ST.CERTIFY_NUM CERTIFY_NUM, ");
                sql += string.Format("         U.INITIALS AS INITIAL1 ");
                sql += string.Format(" FROM    BARCODE_INSPECT I, TB_ITEM_SIZE_V@ISPDB Z, BARCODE_BCP_IF_HIS B , TB_STEEL_TYPE_V@ISPDB ST, TB_SYS_USER@ISPDB U ");
                sql += string.Format(" WHERE   1= :P_TEST ");
                sql += string.Format(" AND     I.ITEM IN ('OB','SB','RB') ");
                sql += string.Format(" AND     I.INSPECT_REPEAT = '232' ");
                sql += string.Format(" AND     I.ITEM = Z.ITEM(+) ");
                sql += string.Format(" AND     I.ITEM_SIZE = Z.ITEM_SIZE(+)      ");
                sql += string.Format(" AND     I.BUNDLE_NO = B.BUNDLE_NO(+) ");
                sql += string.Format(" AND     I.STEEL = ST.STEEL(+)  ");
                sql += string.Format(" AND     I.INSPECT_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql += string.Format(" AND     (I.BUNDLE_NO LIKE '{0}' ||'%') ", txtBundle_no.Text);
                sql += string.Format(" AND     ('{0}' IS NULL OR I.FACTORY = '{1}') ", cboFactory.SelectedValue, cboFactory.SelectedValue);
                sql += string.Format(" AND     ('{0}' IS NULL OR I.WORK_TEAM = '{1}') ", cboWorkTeam.SelectedValue, cboWorkTeam.SelectedValue);
                sql += string.Format(" AND     ( ( '{0}' IS NULL  )                           ", Flag);
                sql += string.Format("         OR ('{0}' ='1' AND LOCATION IN ('ZT4','ZT5'))   ", Flag);
                //sql += string.Format(" --        OR (:P_FLAG ='2' AND LOCATION IN ('ZT6'))   ", Flag);
                //sql += string.Format(" --        OR (:P_FLAG ='3' AND LOCATION IN ('ZT7')) ", Flag);
                sql += string.Format("         OR ('{0}' ='5' AND LOCATION IN ('ZT8','ZT9'))   ", Flag);
                sql += string.Format("         )   ");
                //sql += string.Format(" AND     (I.INSPECT_TYPE = '{0}' OR I.INSPECT_TYPE = (CASE WHEN '{0}' = 'D3PR' THEN 'D3PW' END)) ", InspectStatus, InspectStatus);
                sql += string.Format(" AND     I.INSPECT_TYPE = '{0}' ", InspectStatus);
                sql += string.Format(" AND     I.EMPLOYEE = U.EMPLOYEE(+) ");
                sql += string.Format(" AND     I.INSPECT_STATUS2 NOT IN ('A','R') ");
                sql += string.Format(" AND     (('{0}' = 1) OR ('{1}' = 2 AND I.INSPECT_LINE_NO = 'Y') OR ('{2}' = 3 AND I.INSPECT_LINE_NO IS NULL)) ", PrintStatus, PrintStatus, PrintStatus);
                sql += string.Format(" ORDER BY I.SYSCRTDATE, I.BUNDLE_NO ");

                string[] parm = new string[1];
                parm[0] = ":P_TEST|" + "1";

                //olddt = cd.FindDataTable("MAIN", sql, parm);

                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                //SelectedGrdMain(grdMain, grdMain.RowSel);
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

                if(rdoCircle.Checked == true)
                {
                    Printing_MaeSoo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }
            return;
        }
        #endregion

        #region "그리드 셋팅"
        private void InitGrd_grdMain()
        {
            cs.InitGrid_search(grdMain);

            grdMain.AllowSorting = AllowSortingEnum.SingleColumn;

            //grdMain.AllowEditing = false;

            //grdMain.AllowSorting = AllowSortingEnum.SingleColumn;

            /*
            grdMain.Cols["L_NUM"].Width = cs.L_No_Width;
            int nwidth = (1000 - cs.L_No_Width) / 5;

            grdMain.Cols["MENU_ID"].Width = nwidth;
            grdMain.Cols["MENU_NM"].Width = nwidth;
            grdMain.Cols["UP_MENU_ID"].Width = nwidth;
            grdMain.Cols["SCR_ID"].Width = nwidth;
            grdMain.Cols["MENU_SEQ"].Width = nwidth;
            grdMain.Cols["GUBUN"].Width = nwidth;

            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["MENU_ID"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["MENU_NM"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["UP_MENU_ID"].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["SCR_ID"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["MENU_SEQ"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["GUBUN"].TextAlign = cs.WGT_TextAlign;
            grdMain.Cols["L_NUM"].TextAlign = cs.SURFACE_LEVEL_TextAlign;
            */
        }

        #endregion

        private void Printing_MaeSoo()
        {
            //  기존
            //	prtSIZE <= 44						4장짜리
            //	prtSIZE >= 45 &&  prtSIZE <= 80		2장짜리
            //  prtSIZE >= 81)						1장짜리

            //  변경
            //  prtSIZE <= 29 							//15파이  4장
            //  prtSIZE >= 30 && prtSIZE <= 50			//25파이  3장
            // 	prtSIZE >= 51 && prtSIZE <= 80			//40파이  2장
            // 	prtSIZE >= 81 && prtSIZE <= 120			//60파이	 1장
            //  prtSIZE >= 121							//100파이 1장
            // 	}
            
            var nSize = "";
            var nBonsu = 0;
            var nSteel = "";

            for (var i = 1; i < grdMain.Rows.Count; i++)
            {
                if (rdoSquare.Checked == true)
                {
                    grdMain.SetData(i, "P_CNT", "1");
                }
                else if (rdoCircle.Checked == true)
                {
                    nSize = grdMain.GetData(i, "SIZE_NAME").ToString();
                    nBonsu = int.Parse(grdMain.GetData(i, "BONSU").ToString());
                    nSteel = grdMain.GetData(i, "STEEL_NAME").ToString();

                    //MessageBox.Show(string.Format("{0:f1}", nBonsu / 3));
                    //MessageBox.Show((Convert.ToDouble(nBonsu) / 2).ToString());

                    if ((nSteel == "SM25C" && nSize == "30") || (nSteel == "SM45C" && (nSize == "30" || nSize == "32" || nSize == "35")))
                    {   
                        grdMain.SetData(i, "P_CNT", Math.Round(nBonsu / 3.0, 0));
                    }
                    else
                    {
                        //20150831 서희동 씨 요청 : 15파이, 25파이는 기본 10장만 출력되도록(예외 : SM25C 30파이, SM45C 30,32,35파이는 전량)
                        if (double.Parse(nSize) <= 29)
                        {
                            if (nBonsu < 4)
                            {   
                                grdMain.SetData(i, "P_CNT", "1");
                            }
					        else
                            {
                                grdMain.SetData(i, "P_CNT", "10");
                            }
                        }
                        else if (double.Parse(nSize) > 29 && double.Parse(nSize) <= 50)
                        {
                            if (nBonsu < 3) {
                                grdMain.SetData(i, "P_CNT", "1");
                            }
                            else {
                                //2016.01.05 나영식 대리 요청으로 본수만큼 기본 셋팅
                                //if (nBonsu / 3 >= Math.Round(nBonsu / 3.0, 0))
                                if (Convert.ToDouble(nBonsu) / 3 > Math.Round(nBonsu / 3.0, 0))
                                {
                                    grdMain.SetData(i, "P_CNT", Math.Round(nBonsu / 3.0, 0) + 1);							
                                }
                                else
                                {
                                    grdMain.SetData(i, "P_CNT", Math.Round(nBonsu / 3.0, 0));							        
                                }
                            }
                        }
                        else if (double.Parse(nSize) > 50 && double.Parse(nSize) <= 80)
                        {
                            if (nBonsu < 2) {
                                grdMain.SetData(i, "P_CNT", "1");
                            }
                            else {
                                //2016.01.05 나영식 대리 요청으로 본수만큼 기본 셋팅
                                //if (nBonsu / 2 >= Math.Round(nBonsu / 2.0, 0))
                                if (nBonsu / 2 < Math.Round(nBonsu / 2.0, 0))
                                {
                                    //grdMain.SetData(i, "P_CNT", Math.Round(nBonsu / 2.0, 0) + 1);						
                                    grdMain.SetData(i, "P_CNT", (nBonsu / 2) + 1);
                                }
                                else
                                {
                                    grdMain.SetData(i, "P_CNT", Math.Round(nBonsu / 2.0, 0));						
                                }
                            }
                        }
                        else if (double.Parse(nSize) > 80)
                        {
                            grdMain.SetData(i, "P_CNT", nBonsu);					
                        }
                    }

                    // 			if((nSteel == "SM25C" && nSize == "30") || (nSteel == "SM45C" && (nSize == "30" || nSize == "32" || nSize == "35"))) {
                    // 				$("#WiseGrid")[0].SetCellValue("P_CNT",i,comUtil_roundXL(nBonsu/3, 0));
                    // // 				$("#WiseGrid")[0].SetCellValue("P_CNT",i,comUtil_roundXL(nBonsu/4, 0));
                    // 			} else {
                    // 				if(nSize <= 44){
                    // 					if(nBonsu < 4 ) $("#WiseGrid")[0].SetCellValue("P_CNT",i,1);
                    // 					else $("#WiseGrid")[0].SetCellValue("P_CNT",i,8);

                    // 				}else if(nSize > 44 && nSize <= 59){
                    // 					if(nBonsu < 2 ) $("#WiseGrid")[0].SetCellValue("P_CNT",i,1);
                    // 					else{
                    // 						// $("#WiseGrid")[0].SetCellValue("P_CNT",i,5);
                    // 						if ( nBonsu / 2 >  comUtil_roundXL(nBonsu/2 , 0)){
                    // 							$("#WiseGrid")[0].SetCellValue("P_CNT",i, comUtil_roundXL(nBonsu / 2 , 0)+1); // 기존로직
                    // 						}else{
                    // 							$("#WiseGrid")[0].SetCellValue("P_CNT",i, comUtil_roundXL(nBonsu / 2 , 0)); // 기존로직
                    // 						}
                    // 					}
                    // 				}else if(nSize > 59 && nSize <= 80){
                    // 					if(nBonsu < 2 ) $("#WiseGrid")[0].SetCellValue("P_CNT",i,1);
                    // 					else{
                    // 						if ( nBonsu / 2 >  comUtil_roundXL(nBonsu / 2 , 0)){
                    // 							$("#WiseGrid")[0].SetCellValue("P_CNT",i, comUtil_roundXL(nBonsu / 2 , 0)+1); // 기존로직
                    // 						}else{
                    // 							$("#WiseGrid")[0].SetCellValue("P_CNT",i, comUtil_roundXL(nBonsu / 2 , 0)); // 기존로직
                    // 						}
                    // 					}
                    // 				}else if(nSize > 80){
                    // 					$("#WiseGrid")[0].SetCellValue("P_CNT",i,nBonsu);
                    // 				}
                    // 			}
                }
            }
        }

        //private void doUpdate()
        //{
        //    for(var i = 1; i < grdMain.Rows.Count; i++)
        //    {
        //        if(grdMain.GetData(i, "P_YN") != null)
        //        {
        //            //디비선언
        //            //OracleConnection conn = cd.OConnect("MAIN");

        //            OracleCommand cmd = new OracleCommand();

        //            OracleTransaction transaction = null;

        //            try
        //            {
        //                conn.Open();
        //                cmd.Connection = conn;
        //                transaction = conn.BeginTransaction();
        //                cmd.Transaction = transaction;

        //                string sql = string.Empty;

        //                //계중실적 TC 2387 발생
        //                //계중실적 L3 전송
        //                sql = string.Format(" UPDATE  BARCODE_INSPECT  ");
        //                sql += string.Format(" SET     INSPECT_LINE_NO = 'Y' ");
        //                sql += string.Format(" WHERE   ITEM IN ('OB','SB','RB') ");
        //                sql += string.Format(" AND     INSPECT_REPEAT = '232' ");
        //                sql += string.Format(" AND     BUNDLE_NO = '{0}' ", grdMain.GetData(i, "BUNDLE_NO").ToString());
        //                sql += string.Format(" AND     (INSPECT_LINE_NO = 'Y' OR INSPECT_LINE_NO IS NULL) ");

        //                cmd.CommandText = "";

        //                cmd.CommandText = sql;
        //                cmd.ExecuteNonQuery();
                      
        //                //실행후 성공
        //                cmd.Transaction.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■sql■\n");

        //                //실행후 실패 : 
        //                if (transaction != null)
        //                {
        //                    transaction.Rollback();
        //                }
        //                // 추가되었을시에 중복성으로 실패시 메시지 표시
        //                MessageBox.Show("출력 여부 저장에 실패하였습니다.");
        //            }
        //            finally
        //            {
        //                if (cmd != null)
        //                    cmd.Dispose();
        //                if (conn != null)
        //                    conn.Close();       //데이터베이스연결해제
        //                if (transaction != null)
        //                    transaction.Dispose();
        //            }                    
        //        }
        //    }
        //}

        #endregion

        private void dolabel()
        {
            var Reprint_CNT = 0;
            var Tcnt = 0;
            string selected = string.Empty;

            for (int i = 0; i < grdMain.Rows.Count; i++)
            {
                selected = string.Empty;

                if (grdMain.GetData(i, "P_YN") == null)
                {
                    selected = "False";
                }
                else
                {
                    selected = grdMain.GetData(i, "P_YN").ToString();
                }

                if (selected == "True")
                {                   
                    Reprint_CNT += 1;
                }
            }

            if (Reprint_CNT == 0)
            {
                MessageBox.Show("재발행할 BD가 선택하지 않았습니다. 선택하시기 바랍니다!");
                return;
            }

            var chk1 = rdoCircle.Checked;
            var chk2 = rdoSquare.Checked;
            
            modBarCodePrint a = new modBarCodePrint();

            //바코드 출력 확인
            DialogResult result = MessageBox.Show("바코드 출력 하시겠습니까?", "출력 확인", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                return;
            }

            selected = string.Empty;

            for (var i = 1; i < grdMain.Rows.Count; i++)
            {
                if (grdMain.GetData(i, "P_YN") == null)
                {
                    selected = "False";
                }
                else
                {
                    selected = grdMain.GetData(i, "P_YN").ToString();
                }

                if (selected == "True")
                {
                    //if (grdMain.GetData(i, "P_YN") != null)
                    //{
                        string prtINSPECT_DATE = grdMain.GetData(i, "INSPECT_DATE").ToString();
                        // prtCOMPANY = g_initVal.COMPANY;
                        string prtCOMPANY = grdMain.GetData(i, "PO_CUST_ENG_NAME").ToString();

                        string prtITEM = grdMain.GetData(i, "ITEM").ToString();
                        string prtSIZE = grdMain.GetData(i, "SIZE_NAME").ToString();
                        string prtSTEEL = grdMain.GetData(i, "STEEL_NAME").ToString();
                        string prtHEAT = grdMain.GetData(i, "HEAT").ToString();
                        string prtEMPLOYEE = grdMain.GetData(i, "INITIAL1").ToString();
                        string prtAFTER_ROUTING = grdMain.GetData(i, "AFTER_ROUTING").ToString();
                        string prtSURFACE_LEVEL = grdMain.GetData(i, "SURFACE_LEVEL").ToString();
                        string prtITEM_USE = grdMain.GetData(i, "ITEM_USE_NAME").ToString();
                        string prtSIRF_CODE = grdMain.GetData(i, "SIRF_CODE").ToString();
                        string prtUOM = grdMain.GetData(i, "LENGTHUOM").ToString();
                        string prtLENGTH = grdMain.GetData(i, "LENGTH").ToString();
                        string prtWEIGHT = grdMain.GetData(i, "WEIGHT").ToString();
                        string prtBONSU = grdMain.GetData(i, "BONSU").ToString();

                        //2015.06.30 구성욱 추가
                        string prtBUNDLE_NO = grdMain.GetData(i, "BUNDLE_NO").ToString();
                        string prtITEM_SIZE = grdMain.GetData(i, "SIZE_NAME").ToString();
                        string prtCOUNTRY_BASIS = grdMain.GetData(i, "COUNTRY_BASIS").ToString();
                        string prtSPEC_NUM = grdMain.GetData(i, "SPEC_NUM").ToString();
                        string prtCERTIFY_NUM = grdMain.GetData(i, "CERTIFY_NUM").ToString();

                        if (!(prtAFTER_ROUTING == null || prtAFTER_ROUTING.Trim() == ""))
                        {
                            prtAFTER_ROUTING = prtAFTER_ROUTING.Substring(2, 2);
                        }

                        if (!(prtSURFACE_LEVEL == null || prtSURFACE_LEVEL.Trim() == ""))
                        {
                            prtSURFACE_LEVEL = prtSURFACE_LEVEL.Substring(2, 2);
                        }

                        if (!(prtITEM_USE == null || prtITEM_USE.Trim() == ""))
                        {
                            prtITEM_USE = prtITEM_USE.Substring(0, 3);
                            //prtITEM_USE = "";
                        }

                        if (!(prtSIRF_CODE == null || prtSIRF_CODE.Trim() == ""))
                        {
                            prtSIRF_CODE = prtSIRF_CODE.Substring(2, 1);
                            if (prtSIRF_CODE.Trim() == "B")
                            {
                                prtCOMPANY = "RETURN SCRAP";
                            }
                        }

                        if (!(prtLENGTH == null || prtLENGTH.Trim() == ""))
                        {
                            prtLENGTH = (double.Parse(prtLENGTH) / 100).ToString();
                        }

                        /*
                        if (!(prtWEIGHT == null || prtWEIGHT.Trim() == ""))
                        {
                            prtWEIGHT = prtWEIGHT;
                        }

                        if (!(prtBONSU == null || prtBONSU.Trim() == ""))
                        {
                            prtBONSU = prtBONSU;
                        }
                        */

                        a.BARCODE_PRINT_COUNT = 1;

                        if (chk2)
                        {
                            // 네모라벨 발행
                            var j = 1;
                            // 		 BARCODE_COUNTRY_BASIS[j] = "";
                            a.BARCODE_COUNTRY_BASIS = prtCOUNTRY_BASIS;
                            a.BARCODE_SIZE_CODE = "";
                            // 		 BARCODE_LABEL_SIZE_NAME[j] = "";
                            // 		 BARCODE_APPORVAL_NO[j] = "";
                            a.BARCODE_LABEL_SIZE_NAME = prtSPEC_NUM;
                            a.BARCODE_APPORVAL_NO = prtCERTIFY_NUM;
                            a.BARCODE_INSPECT_DATE = prtINSPECT_DATE;
                            a.BARCODE_ITEM = prtITEM;
                            a.BARCODE_ITEM_SIZE = prtITEM_SIZE;
                            a.BARCODE_SIZE_NAME = prtSIZE;

                            //2015.12.28 구성욱 추가.. 변경 강종을 초기화
                            a.BARCODE_STEEL1 = prtSTEEL;

                            //품질 보증팀 요청으로 사각라벨 위부분과 아래부분의 출력되는 강종명을 다르게 표기_2015.12.22 김성진 과장 요청
                            switch (prtSTEEL)
                            {
                                case "10B38M2":
                                    a.BARCODE_STEEL = "SAE10B38M2"; a.BARCODE_STEEL1 = "SAE10B38M2";
                                    break;
                                case "S40CVS":
                                    a.BARCODE_STEEL = "S40CVS-HB"; a.BARCODE_STEEL1 = "S40CVS-HB";
                                    break;
                                case "SCM440HN":
                                    a.BARCODE_STEEL = "SCM440HNT"; a.BARCODE_STEEL1 = "SCM440HNT";
                                    break;
                                case "S25CVMNS":
                                    a.BARCODE_STEEL = "S25CVMNS1"; a.BARCODE_STEEL1 = "S25CVMNS1";
                                    break;
                                case "S45CVMNK":
                                    a.BARCODE_STEEL = "S45CVMN-K"; a.BARCODE_STEEL1 = "S45CVMN-K";
                                    break;
                                case "S45CVMNH":
                                    a.BARCODE_STEEL = "S45CVMN-H"; a.BARCODE_STEEL1 = "S45CVMN-H";
                                    break;
                                case "1050MJ":
                                    a.BARCODE_STEEL = "SAE1050M"; a.BARCODE_STEEL1 = "SAE1050M";
                                    break;
                                case "44MNSIV":
                                    a.BARCODE_STEEL = "44MNSIVS6"; a.BARCODE_STEEL1 = "44MNSIVS6";
                                    break;
                                case "38MNSIV":
                                    a.BARCODE_STEEL = "38MNSIVS5"; a.BARCODE_STEEL1 = "38MNSIVS5";  //--나영식요청:2013.03.18, SHARP
                                    break;
                                case "SM25CA":
                                    a.BARCODE_STEEL = "SM25C"; a.BARCODE_STEEL1 = "SM25C";  //--나영식요청:2013.04.10, SHARP
                                    break;
                                case "44MNSIVH":
                                    a.BARCODE_STEEL = "44MNSIVS6-H"; a.BARCODE_STEEL1 = "44MNSIVS6-H";//--나영식요청:2013.07.03, SHARP
                                    break;
                                case "SM45C-M3":
                                    a.BARCODE_STEEL = "SM45C-M2"; a.BARCODE_STEEL1 = "SM45C-M2";  //--김성진과장요청:2015.05.27, KSW
                                    break;
                                // 				 case "15B36M1":					a.BARCODE_STEEL = "15B37M"; 	a.BARCODE_STEEL= "15B37M";  //--나영식대리요청:2016.08.18, WJH
                                // 				 break;

                                case "SCM415HU":
                                    a.BARCODE_STEEL = "SCM415H"; a.BARCODE_STEEL1 = "SCM415HU";
                                    break;
                                case "SCM420HU":
                                    a.BARCODE_STEEL = "SCM420H"; a.BARCODE_STEEL1 = "SCM420HU";
                                    break;
                                case "SCM440HU":
                                    a.BARCODE_STEEL = "SCM440H"; a.BARCODE_STEEL1 = "SCM440HU";
                                    break;
                                case "SM20CU":
                                    a.BARCODE_STEEL = "SM20C"; a.BARCODE_STEEL1 = "SM20CU";
                                    break;
                                case "SM25CU":
                                    a.BARCODE_STEEL = "SM25C"; a.BARCODE_STEEL1 = "SM25CU";
                                    break;
                                case "SM35CU":
                                    a.BARCODE_STEEL = "SM35C"; a.BARCODE_STEEL1 = "SM35CU";
                                    break;
                                case "SM38CU":
                                    a.BARCODE_STEEL = "SM38C"; a.BARCODE_STEEL1 = "SM35CU";
                                    break;
                                case "SM45CU":
                                    a.BARCODE_STEEL = "SM45C"; a.BARCODE_STEEL1 = "SM45CU";
                                    break;
                                case "S45CU":
                                    a.BARCODE_STEEL = "S45C"; a.BARCODE_STEEL1 = "S45CU";
                                    break;
                                case "SM48CU":
                                    a.BARCODE_STEEL = "SM48C"; a.BARCODE_STEEL1 = "SM48CU";
                                    break;
                                case "SM53CU":
                                    a.BARCODE_STEEL = "SM53C"; a.BARCODE_STEEL1 = "SM53CU";
                                    break;
                                case "SM55CU":
                                    a.BARCODE_STEEL = "SM55C"; a.BARCODE_STEEL1 = "SM55CU";
                                    break;
                                case "SM50CS":
                                    a.BARCODE_STEEL = "SM50C"; a.BARCODE_STEEL1 = "SM50CS";   //--2016.04.22 나영식 대리요청
                                    break;

                                // 손삼호 수정
                                //case "SCM822H1":						 a.BARCODE_STEEL = "SCM822H";
                                //break;
                                default:
                                    a.BARCODE_STEEL = prtSTEEL;
                                    break;
                            }

                            // 		 BARCODE_PRINTED_BASIS[j] = "";
                            a.BARCODE_PRINTED_BASIS = "";
                            a.BARCODE_INSPECTER_NAME = "";
                            a.BARCODE_HEAT = prtHEAT;
                            a.BARCODE_MARKET_TYPE = "";
                            a.BARCODE_UOM = prtUOM;
                            a.BARCODE_SIRF_CODE = prtSIRF_CODE;
                            a.BARCODE_Length = prtLENGTH;
                            a.BARCODE_BONSU = prtBONSU;
                            a.BARCODE_WEIGHT = prtWEIGHT;
                            a.BARCODE_BUNDLE_NO = prtBUNDLE_NO;
                            a.BARCODE_COMPANY = prtCOMPANY;
                            a.BARCODE_AFTER_ROUTING = prtAFTER_ROUTING;
                            a.BARCODE_SURFACE_LEVEL = prtSURFACE_LEVEL;
                            a.BARCODE_ITEM_USE = prtITEM_USE;
                            a.BARCODE_INSPECTOR = prtEMPLOYEE;

                            //2015.07.02 현대자동차직납, 열처리는 STEEL : NB, SIZE : 098, LENGTH : 5.9CM만 나간다.
                            if (a.BARCODE_STEEL == "44MNSIVS6-H" && a.BARCODE_SIZE_NAME == "98" && a.BARCODE_Length == "5.9")
                            {
                                // 매수 만큼 돌면서 출력한다.
                                var nMaesu = grdMain.GetData(i, "P_CNT").ToString();

                                for (var k = 0; k < int.Parse(nMaesu); k++)
                                {
                                    a.PrintStart("RB_SQUAREHOLE", "LPT1", 1);
                                    System.Threading.Thread.Sleep(1000);
                                }
                            }
                            else
                            {
                                // 매수 만큼 돌면서 출력한다.
                                var nMaesu = grdMain.GetData(i, "P_CNT").ToString();

                                for (var k = 0; k < int.Parse(nMaesu); k++)
                                {
                                    a.PrintStart("RB_SQUARET3", "LPT1", 1);
                                    System.Threading.Thread.Sleep(1000);
                                // 			 gfn_PrintStart("RB_SQUARE", $("#S_PRINT_2").val(), j,barCodeApplet);                    
                                }
                            }
                        }

                        if (chk1)
                        {
                            var j = 1;
                            var sROUND = "";

                            if (double.Parse(prtSIZE) <= 29)
                            {
                                sROUND = "RB_ROUND_SMLT";               //15파이
                            }
                            else if (double.Parse(prtSIZE) >= 30 && double.Parse(prtSIZE) <= 50)
                            {
                                sROUND = "RB_ROUND_MIDT";                   //25파이
                                                                            //				if(prtSIZE <= 50){
                                                                            //				 	sROUND = "RB_ROUND_SML";
                            }
                            else if (double.Parse(prtSIZE) >= 51 && double.Parse(prtSIZE) <= 80)
                            {
                                sROUND = "RB_ROUND_MID1";                   //40파이
                            }
                            else if (double.Parse(prtSIZE) >= 81 && double.Parse(prtSIZE) <= 120)
                            {
                                sROUND = "RB_ROUND_LAGT";               //60파이
                            }
                            else if (double.Parse(prtSIZE) >= 121)
                            {
                                sROUND = "RB_ROUND_BIGLAGT";            //100파이
                            }

                            // 		 BARCODE_COUNTRY_BASIS[j] = "";
                            a.BARCODE_COUNTRY_BASIS = prtCOUNTRY_BASIS;
                            a.BARCODE_SIZE_CODE = "";
                            a.BARCODE_LABEL_SIZE_NAME = prtSPEC_NUM;
                            a.BARCODE_APPORVAL_NO = prtCERTIFY_NUM;
                            a.BARCODE_INSPECT_DATE = prtINSPECT_DATE;
                            a.BARCODE_ITEM = prtITEM;
                            a.BARCODE_ITEM_SIZE = prtITEM_SIZE;
                            a.BARCODE_SIZE_NAME = prtSIZE;

                            switch (prtSTEEL)
                            {
                                /*  case "SCM822H" : a.BARCODE_STEEL = "SCM822HST" ;
                                 break; //--2014.07.10, 허정윤요청*/
                                case "10B38M2":
                                    a.BARCODE_STEEL = "SAE10B38M2";
                                    break;
                                case "S40CVS":
                                    a.BARCODE_STEEL = "S40CVS-HB";
                                    break;
                                case "SCM440HN":
                                    a.BARCODE_STEEL = "SCM440HNT";
                                    break;
                                case "S25CVMNS":
                                    a.BARCODE_STEEL = "S25CVMNS1";
                                    break;
                                case "S45CVMNK":
                                    a.BARCODE_STEEL = "S45CVMN-K";
                                    break;
                                case "S45CVMNH":
                                    a.BARCODE_STEEL = "S45CVMN-H";
                                    break;
                                case "1050MJ":
                                    a.BARCODE_STEEL = "SAE1050M";
                                    break;
                                case "44MNSIV":
                                    a.BARCODE_STEEL = "44MNSIVS6";
                                    break;
                                case "38MNSIV":
                                    a.BARCODE_STEEL = "38MNSIVS5";  //--나영식요청:2013.03.18, SHARP
                                    break;
                                case "SM25CA":
                                    a.BARCODE_STEEL = "SM25C";  //--나영식요청:2013.04.10, SHARP
                                    break;
                                case "44MNSIVH":
                                    a.BARCODE_STEEL = "44MNSIVS6-H";  //--나영식요청:2013.07.03, SHARP
                                    break;
                                case "SM45C-M3":
                                    a.BARCODE_STEEL = "SM45C-M2";  //--김성진과장요청:2015.05.27, KSW
                                    break;
                                // 						 case "15B36M1":					a.BARCODE_STEEL= "15B37M"; 	 //--나영식대리요청:2016.08.18, WJH
                                // 						 break;
                                //			 			 case "SCM822H1":						a.BARCODE_STEEL= "SCM822H";
                                //			 			 break;

                                case "SCM415HU":
                                    a.BARCODE_STEEL = "SCM415H";
                                    break;
                                case "SCM420HU":
                                    a.BARCODE_STEEL = "SCM420H";
                                    break;
                                case "SCM440HU":
                                    a.BARCODE_STEEL = "SCM440H";
                                    break;
                                case "SM20CU":
                                    a.BARCODE_STEEL = "SM20C";
                                    break;
                                case "SM25CU":
                                    a.BARCODE_STEEL = "SM25C";
                                    break;
                                case "SM35CU":
                                    a.BARCODE_STEEL = "SM35C";
                                    break;
                                case "SM38CU":
                                    a.BARCODE_STEEL = "SM38C";
                                    break;
                                case "SM45CU":
                                    a.BARCODE_STEEL = "SM45C";
                                    break;
                                case "S45CU":
                                    a.BARCODE_STEEL = "S45C";
                                    break;
                                case "SM48CU":
                                    a.BARCODE_STEEL = "SM48C";
                                    break;
                                case "SM53CU":
                                    a.BARCODE_STEEL = "SM53C";
                                    break;
                                case "SM55CU":
                                    a.BARCODE_STEEL = "SM55C";
                                    break;
                                case "SM50CS":
                                    a.BARCODE_STEEL = "SM50C"; //--20160422 나영식 대리 요청
                                    break;
                                default:
                                    a.BARCODE_STEEL = prtSTEEL;
                                    break;
                            }

                            a.BARCODE_PRINTED_BASIS = "";
                            a.BARCODE_INSPECTER_NAME = "";
                            a.BARCODE_HEAT = prtHEAT;
                            a.BARCODE_MARKET_TYPE = "";
                            a.BARCODE_UOM = "";
                            a.BARCODE_SIRF_CODE = prtSIRF_CODE;
                            a.BARCODE_Length = prtLENGTH;
                            a.BARCODE_BONSU = prtBONSU;
                            a.BARCODE_WEIGHT = prtWEIGHT;
                            a.BARCODE_BUNDLE_NO = prtBUNDLE_NO;
                            a.BARCODE_COMPANY = prtCOMPANY;
                            a.BARCODE_AFTER_ROUTING = prtAFTER_ROUTING;
                            a.BARCODE_SURFACE_LEVEL = prtSURFACE_LEVEL;
                            a.BARCODE_ITEM_USE = prtITEM_USE;

                            // 매수 만큼 돌면서 출력한다.
                            var nMaesu = grdMain.GetData(i, "P_CNT").ToString();

                            for (var k = 0; k < int.Parse(nMaesu); k++)
                            {
                                a.PrintStart(sROUND, "LPT1", 1);
                                System.Threading.Thread.Sleep(200);
                            }
                        } // End if chk2   
                    //}
                }
            }

            //2015.07.23 원정호추가_ 라벨발행유무표기 요청
            //doUpdate();

            btnDisplay_Click(this, new EventArgs());

            //window.setTimeout('fn_close()', 2000);
        }

        #region "일단 사용안함"

        #region "VALIDATION 체크"
        private Boolean validation()
        {
            try
            {

                //가열로 존재 HEAT 체크

                //ANGEL용 PO 체크

                //본당 중량 계산
                /*                
                Chk_weight = Number($("#WiseGrid2")[0].GetCellValue('BD_BONSU', 0)) * Number($("#WiseGrid2")[0].GetCellValue('WEIGHT', 0));
                */

                //회수율 체크
                /*
                if (sql == "N")
                {
                    alert("[회수율 초과] 소재장입량보다 검사량이 더 많습니다.(가열로 CHECK 기준변경 : 추출 => 장입)\n(CHECK-1) 등록하고자 하는 HEAT가 가열로 장입실적에 없는 HEAT 일수 있습니다\n.(CHECK-2) 실제 회수율을 초과한 경우\n확인해보세요.!!!");
                    return;
                }
                */

                //강종체크
                /*
                var prm = { };
                prm.P_FACTORY = $("#S_FACTORY").val();
                prm.P_HEAT = $("#S_HEAT").val();
                prm.P_STEEL = $("#S_STEEL").val();
                prm.P_ITEM = $("#S_ITEM").val();
                prm.P_ITEM_SIZE = $("#S_ITEM_SIZE").val();

                var steelyon = getString("kq.qms.qit.10788.steeYON", prm);

                if (steelyon == "" || steelyon == "N")
                {
                    alert("입력한 HEAT의 강종이 틀렸습니다. 강종을 확인하시고 HEAT를 다시 입력하세요");
                    return;
                }
                */

                //재공일 시 사유입력 유무 확인
                /*
                if cboCountry_basis_I.Text = "재공" then

                     if ($("#S_REASON").val() == ""){
                        alert("재공사유를 선택하세요!");
                        return;
                    }
                }
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show("■validation 기타 Error■\n" + ex.Message);
                return false;
            }
            finally
            {

            }

            return true;
        }
        #endregion

        #endregion

        #region "EVENT"

        #region "시작일자 event"
        //시작일자가 종료일자보다 크면 시작일자로 종료일자 셋팅
        private void Start_dt_ValueChanged(object sender, EventArgs e)
        {
            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }
        #endregion

        #region "종료일자 event"
        //종료일자가 시작일자보다 작으면 종료일자로 시작일자 셋팅
        private void End_dt_ValueChanged(object sender, EventArgs e)
        {
            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }
        #endregion

        #endregion

        private void chkNo3_CheckedChanged(object sender, EventArgs e)
        {
            if(chkNo3.Checked == true)
            {
                if(chkNo5.Checked == true)
                {
                    MessageBox.Show("한개의 값만 선택하여 주십시요.");
                    chkNo3.Checked = false;
                }                
            }
        }

        private void chkNo5_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNo5.Checked == true)
            {
                if (chkNo3.Checked == true)
                {
                    MessageBox.Show("한개의 값만 선택하여 주십시요.");
                    chkNo5.Checked = false;
                }
            }
        }

        private void rdoSquare_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoSquare.Checked == true)
            {
                cboCircle.Enabled = false;
                cboSquare.Enabled = true;
                Printing_MaeSoo();
            }
        }

        private void rdoCircle_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoCircle.Checked == true)
            {
                cboCircle.Enabled = true;
                cboSquare.Enabled = false;
                Printing_MaeSoo();
            }
        }

        private void grdMain_CellChanged(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            string selected = string.Empty;

            if (grd.GetData(e.Row, "P_YN") == null)
            {
                selected = "False";
            }
            else
            {
                selected = grd.GetData(e.Row, "P_YN").ToString();
            }

            if (selected == "True")
            {

                grd.Rows[e.Row].Style = grd.Styles["CustomStyle1"];
            }
            else
            {
                grd.Rows[e.Row].Style = grd.Styles["Normal"];
            }
           
        }

        private void frmRB_RELABEL_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = 1; i < grdMain.Rows.Count; i++)
            {
                if (chkAll.Checked == true)
                {
                    grdMain.SetData(i, "P_YN", true);
                }
                else
                {
                    grdMain.SetData(i, "P_YN", false);
                }
            }
        }
    }
}
