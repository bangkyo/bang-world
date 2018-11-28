using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using BGsystemLibrary.Common;
using System.Collections;
using BGsystemLibrary.MatMgmt;

namespace BGsystemLibrary.SystemMgmt
{
    public partial class UserMgmt : Form
    {
        //공통
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();
        //임시 데이터테이블
        DataTable olddt;
        DataTable moddt;
        DataTable olddt_sub;
        DataTable moddt_sub;

        // 셀의 수정전 값
        private string strBefValue = "";

        //타이틀명
        private static string ownerNM = "";
        private static string titleNM = "";

        //변수
        private string txtid = "";      //ID
        private string txtnm = "";      //유저명
        public string txtdeptCd = "";   //부서코드
        public string txtdept = "";     //부서명
        int selected_row;               //선택된 로우
        string selected_user_id = "";   //선택된 USER_ID
        string selected_passwd = "";    //선택된 패스워드
        string selected_tel_no = "";    //선택된 전화번호
        string selected_mail_id = "";   //선택된 메일
        string selected_dept_cd = "";   //선택된 부서코드
        string selected_dept_value = "";   //선택된 부서코드에 대한 이름
        string selected_user_nm = "";   //선택된 이름
        string selected_post_cd = "";   //선택된 직위코드
        string selected_post_value = "";   //선택된 직위코드에 대한 이름
        string selected_auth_id = "";   //선택된 직위코드
        string selected_auth_nm = "";   //선택된 직위코드에 대한 이름
                                           //Hashtable Sys_cd = null;
                                           //private static string cbxSys_id = "";   //조회조건시스템 구분key
                                           //private static string cbxSys_nm = "";   //조회조건시스템 구분value



        //그리드 변수
        private int GridRowsCount = 2;
        private int GridColsCount = 12;
        private int RowsFixed = 2;
        private int RowsFrozen = 0;
        private int ColsFixed = 0;
        private int ColsFrozen = 0;
        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한
                                        //권한관련 add ]]


        public UserMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            //권한관련 add [[
            string[] scrAuthParams = scrAuth.Split(',');

            this.scrAuthInq = scrAuthParams[1];   //조회 권한 저장
            this.scrAuthReg = scrAuthParams[2];   //등록(추가) 권한 저장
            this.scrAuthMod = scrAuthParams[3];   //수정 권한 저장
            this.scrAuthDel = scrAuthParams[4];   //삭제 권한 저장
                                                  //권한관련 add ]]

            InitializeComponent();
            //this.FormBorderStyle = FormBorderStyle.None;//윈도우테두리제거방법

        }
        /// <summary>
        /// 화면 초기화
        /// </summary>
        private void UserMgmt_Load(object sender, EventArgs e)
        {
            InitControl();
            Button_Click(btnDisplay, null);
        }
        /// <summary>
        /// 프로그램 초기화
        /// </summary>
        private void InitControl()
        {
            //조회조건 판넬 그리기
            clsStyle.Style.InitPanel(panel1);
            // combobox 직위코드
            setCbx_Post();
            // combobox 권한
            setCbx_Auth();

            //그리드 초기화
            DrawGrid(grdMain);
            //DrawGrid(grdSub);

            //초기화버튼 툴팁 메세지내용
            Reset_toolTip.SetToolTip(bt_Reset, "'a!+사용자ID'로 초기화 합니다.");

        }

        private void setCbx_Auth()
        {
            DataTable dt = InitCbx_Auth();

            if (dt == null || dt.Rows.Count == 0) return;

            Dictionary<string, string>  dic = new Dictionary<string, string>();

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                dic.Add(dt.Rows[row].ItemArray[0].ToString(), dt.Rows[row].ItemArray[1].ToString());
            };

            cbx_Auth.DataSource = new BindingSource(dic, null);
            cbx_Auth.DisplayMember = "Value";
            cbx_Auth.ValueMember = "Key";

            cbx_postCd.SelectedIndex = 0;
        }

        private DataTable InitCbx_Auth()
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "";
                sql += string.Format(" /****** 권한 그룹 정보 ******/  ");
                sql += string.Format(" SELECT DISTINCT ACL_GRP_ID      ");
                sql += string.Format("      , ACL_GRP_NM               ");
                sql += string.Format("   FROM TB_CM_ACL_GRP            ");

                dt = cd.FindDataTable(sql);
            }
            catch (Exception ex)
            {

                return null;
            }


            return dt;
        }
        
        private void setCbx_Post()
        {

            DataTable dt = InitCbx();

            if (dt == null || dt.Rows.Count == 0) return;


            Dictionary<String, String> lDcbx_postCd = new Dictionary<string, string>();

            //lDcbx_postCd.Add(" ", " ");

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                lDcbx_postCd.Add(dt.Rows[row].ItemArray[0].ToString(), dt.Rows[row].ItemArray[1].ToString());
            };

            cbx_postCd.DataSource = new BindingSource(lDcbx_postCd, null);
            cbx_postCd.DisplayMember = "Value";
            cbx_postCd.ValueMember = "Key";

            cbx_postCd.SelectedIndex = 0;

        }



        /// <summary>
        /// 직위코드 가져오기
        /// </summary>
        private DataTable InitCbx()
        {
            try
            {
                string sql = "";
                sql += string.Format(" /* 직위코드 조회*/              ");
                sql += string.Format(" SELECT B.CD_ID   AS   POST_CD   ");
                sql += string.Format("       ,B.CD_NM   AS   POST_NM   ");
                sql += string.Format("   FROM TB_CM_COM_CD_GRP A       ");
                sql += string.Format("      , TB_CM_COM_CD     B       ");
                sql += string.Format("  WHERE A.CATEGORY = B.CATEGORY  ");
                sql += string.Format("    AND A.CATEGORY = 'POST_CD'   ");
                sql += string.Format("    AND A.USE_YN   = 'Y'         ");
                sql += string.Format("    AND B.USE_YN   = 'Y'         ");
                sql += string.Format(" ORDER BY SORT_SEQ DESC          ");

                olddt_sub = cd.FindDataTable(sql);
                moddt_sub = olddt_sub.Copy();
            }
            catch (Exception ex)
            {

                return null;
            }
            

            return moddt_sub;
        }

        /// <summary>
        /// 그리드 초기화
        /// </summary>
        /// <param name="grdItem"></param>
        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            grdItem.BeginInit();
            try
            {
                if (grdItem.Rows.Count == 1)
                {

                    if (grdItem.Name.ToString() == "grdMain")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                        FlexGridCol(grdItem);
                    }
                    //else
                    //{
                    //    clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    //    FlexGridCol2(grdItem);
                    //}
                }
                else
                {

                    if (grdItem.Name.ToString() == "grdMain")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                        //컬럼 스타일 세팅
                        FlexGridCol(grdItem);
                    }
                    //else
                    //{
                    //    clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    //    //컬럼 스타일 세팅
                    //    FlexGridCol2(grdItem);
                    //}
                }
                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                grdMain.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                //grdSub.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;

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

        /// <summary>
        /// 조회된 데이터 그리드에 세팅 
        /// </summary>
        /// <param name="grdItem"></param>
        /// <param name="dataTable"></param>
        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            if(grdItem.Rows.Count > GridRowsCount) grdItem.Rows.RemoveRange(GridRowsCount, grdItem.Rows.Count - GridRowsCount);

            try
            {

                grdItem.AllowEditing = false;
                if (grdItem.Name.ToString() == "grdMain")
                {
                    //grdItem.Cols[1].AllowEditing = false;
                    //grdItem.Cols[2].AllowEditing = true;
                    //grdItem.Cols[3].AllowEditing = true;
                    //grdItem.Cols[7].AllowEditing = true;


                    // 직위 콤보박스 설정

                    MakePostComBoINGrid();

                    // 권한 콤보박스 설정
                    MakeAuthComBoINGrid();

                    //직위코드 ListDictionary 생성


                    RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable);

                }
                ////else
                ////{
                ////    grdItem.Cols[1].AllowEditing = true;
                ////    grdItem.Cols[2].AllowEditing = true;
                ////    grdItem.Cols[3].AllowEditing = true;

                ////    //시스템구분 ListDictionary 생성
                ////    DataTable dt = cd.getSysGp();
                ////    ListDictionary dataMap = new ListDictionary();
                ////    for (int i = 0; i < dt.Rows.Count; i++)
                ////    {
                ////        dataMap.Add(dt.Rows[i].ItemArray[0].ToString(), dt.Rows[i].ItemArray[1].ToString());
                ////    }

                ////    //콤보박스 바인딩
                ////    clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                ////    FlexGridColumns.Add("SYSTEM_GP", FlexGridCellTypeEnum.ComboBox, dataMap);
                ////    //그리드 데이터테이블 바인딩
                ////    RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, FlexGridColumns, true);
                ////}


                //스크롤 세팅
                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;
                //로우 사이즈
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);
                //마지막행 사이즈조절, 로우공백흰색
                grdMain.ExtendLastCol = true;
                grdMain.Styles.EmptyArea.BackColor = Color.White;
                //grdSub.ExtendLastCol = true;
                //grdSub.Styles.EmptyArea.BackColor = Color.White;

                grdMain.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                //grdSub.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;

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

        private void MakeAuthComBoINGrid()
        {
            DataTable dt = InitCbx_Auth();
            ListDictionary dataMap = new ListDictionary();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataMap.Add(dt.Rows[i].ItemArray[0].ToString(), dt.Rows[i].ItemArray[1].ToString());
            }

            //콤보박스 바인딩
            clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
            //FlexGridColumns.Add("CD_NM", FlexGridCellTypeEnum.ComboBox, dataMap);
            FlexGridColumns.Add("POST_VALUE", FlexGridCellTypeEnum.ComboBox, dataMap);
        }



        private void MakePostComBoINGrid()
        {
            DataTable dt = InitCbx();
            ListDictionary dataMap = new ListDictionary();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataMap.Add(dt.Rows[i].ItemArray[0].ToString(), dt.Rows[i].ItemArray[1].ToString());
            }

            //콤보박스 바인딩
            clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
            //FlexGridColumns.Add("CD_NM", FlexGridCellTypeEnum.ComboBox, dataMap);
            FlexGridColumns.Add("POST_VALUE", FlexGridCellTypeEnum.ComboBox, dataMap);
        }

        /// <summary>
        /// grdMain그리드 컬럼 스타일 적용
        /// </summary>
        /// <param name="grdItem"></param>
        private void FlexGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 100;
            grdItem.Cols[2].Width = 100;
            grdItem.Cols[3].Width = 100;
            grdItem.Cols[4].Width = 150;
            grdItem.Cols[5].Width = 0;
            grdItem.Cols[6].Width = 0;
            grdItem.Cols[7].Width = 0;
            grdItem.Cols[8].Width = 200;
            grdItem.Cols[9].Width = 200;
            grdItem.Cols[10].Width = 0;
            grdItem.Cols[11].Width = 100;
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "사용자ID";
            grdItem[1, 2] = "성명";
            grdItem[1, 3] = "직위";
            grdItem[1, 4] = "부서";
            grdItem[1, 5] = "직위코드";
            grdItem[1, 6] = "부서코드";
            grdItem[1, 7] = "패스워드";
            grdItem[1, 8] = "메일";
            grdItem[1, 9] = "전화번호";
            grdItem[1,10] = "권한코드";
            grdItem[1,11] = "권한";

            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
            clsFlexGrid.DataGridLeftStyle(grdItem, 2);
            clsFlexGrid.DataGridCenterStyle(grdItem, 3);
            clsFlexGrid.DataGridLeftStyle(grdItem, 4, 9);
            clsFlexGrid.DataGridCenterStyle(grdItem, 11);
            //EDIT 불가 컬럼 설정
            //grdMain.Cols[4].AllowEditing = false;
            //grdMain.Cols[6].AllowEditing = false;
        }
        


        private void InitInPut()
        {
            txt_UserId.ReadOnly = false;

            txt_UserId.Text = "";
            txt_passWd.Text = "";
            txt_passWd2.Text = "";
            txt_UserNm.Text = "";
            //cbx_postCd.Text = "";
            cbx_postCd.SelectedIndex = 0;
            txt_telNo.Text = "";
            txt_mailId.Text = "";
        }

        private bool CanRowSelectGrid(C1FlexGrid grdMain, int gridRowsCount)
        {
            if (grdMain.Rows.Count > 2) return true;

            return false;
        }

        private void SetDataBinding()
        {
            SetDataBinding_Grd_byinitData();


            SetDataBinding_grdMain();
        }



        private void SetDataBinding_Grd_byinitData()
        {
            DrawGrid(grdMain);
        }

        /// <summary>
        /// 엑셀버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcel_Click(object sender, EventArgs e)
        {
            vf.SaveExcel(titleNM, grdMain);
        }

        /// <summary>
        /// 조회조건 유저명 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNM_TextChanged(object sender, EventArgs e)
        {
            txtnm = txtNM1.Text;
        }

        /// <summary>
        /// 조회조건 유저ID 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserID_TextChanged(object sender, EventArgs e)
        {
            txtid = txtUserID.Text;
        }

        /// <summary>
        /// 조회조건 부서코드 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDept_TextChanged(object sender, EventArgs e)
        {
            //txtdept = txtDept1.Text;
        }

        /// <summary>
        /// grdMain행추가 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rowadd_btn_Click(object sender, EventArgs e)
        {
            if (this.scrAuthReg != "Y")
            {
                MessageBox.Show("등록 권한이 없습니다");
                return;
            }

            for (int i = 1; i < grdMain.Rows.Count; i++)
            {
                if (grdMain.Rows[i][0].ToString() == "추가")
                {
                    MessageBox.Show("한개씩만 추가할 수 있습니다.");
                    return;
                }
            }

            // 세부 입력사항 초기화 및 기본값 입력
            // 초기화 자료를 읽어와서 새로운 행에 기본값 입력.

            grdMain.AllowEditing = false;

            //추가 행 데이터 디폴트 값넣기
            grdMain.Rows.Add();
            grdMain.Rows[grdMain.Rows.Count - 1].Height = DataRowsHeight;
            selected_row = grdMain.Rows.Count - 1;
            grdMain.SetData(selected_row, 0, "추가");

            grdMain.SetData(selected_row, 6, ((KeyValuePair<string, string>)(cbx_postCd.Items[0])).Key);
            grdMain.SetData(selected_row, 3, ((KeyValuePair<string, string>)(cbx_postCd.Items[0])).Value);
            grdMain.SetData(selected_row, 10, ((KeyValuePair<string, string>)(cbx_Auth.Items[0])).Key);
            grdMain.SetData(selected_row, 11, ((KeyValuePair<string, string>)(cbx_Auth.Items[0])).Value);


            for (int col = 1; col < grdMain.Cols.Count; col++)
                if (col == 6 || col == 3|| col ==10 || col ==11) continue;
                else
                    grdMain.SetData(grdMain.Rows.Count - 1, col, "");


            // Insert 배경색 지정
            clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Rows.Count - 1, 0, grdMain.Rows.Count - 1, grdMain.Cols.Count - 1, Color.Yellow, Color.Black);
            //// 커서위치 결정
            grdMain.Row = grdMain.Rows.Count - 1;
            grdMain.Col = 0;


            grdMain.Cols["USER_ID"].AllowEditing = true;   // 신규 유저ID 생성

            InitInputs();

            grdMain_Click(grdMain, new DataGridViewCellEventArgs(0, grdMain.Rows.Count - 1));


        }

        private void InitInputs()
        {
            //상세 초기화
            txt_UserId.ReadOnly = false;
            //txt_DeptCd.ReadOnly = false;

            txt_UserId.Text = "";
            txt_passWd.Text = "";
            txt_passWd2.Text = "";
            txt_UserNm.Text = "";
            //cbx_postCd.Text = "";
            cbx_postCd.SelectedIndex = 0;
            cbx_Auth.SelectedIndex = 0;
            txt_telNo.Text = "";
            txt_mailId.Text = "";
        }


        /// <summary>
        /// grdMain행삭제 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rowdel_btn_Click(object sender, EventArgs e)
        {
            if (this.scrAuthDel != "Y")
            {
                MessageBox.Show("삭제 권한이 없습니다");
                return;
            }

            if (grdMain.Rows.Count < 2 || grdMain.Row < 1)
            {
                return;
            }

            //행 추가 후 삭제 클릭시 바로 삭제
            if (grdMain.Rows[grdMain.Row][0].ToString() == "추가")
            {
                grdMain.RemoveItem(grdMain.Row);

                return;
            }

            // 저장시 Delete로 처리하기 위해 flag set
            grdMain.Rows[grdMain.Row][0] = "삭제";
            // Delete 배경색 지정
            clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Red, Color.Black);
        }

        
        /// <summary>
        /// grdMain BeforeEdit 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            if (e.Row < 3 || grd.GetData(e.Row, e.Col) == null)
            {
                return;
            }
            // NO COLUMN 수정불가하게..
            if (e.Col == grd.Cols["L_NUM"].Index && grd.GetData(e.Row, "L_NUM").ToString() != "추가")  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }
            // NO COLUMN 수정불가하게..
            if (e.Col == grd.Cols["USER_ID"].Index && grdMain.GetData(e.Row, grd.Cols["L_NUM"].Index).ToString() != "추가")  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue = grd.GetData(e.Row, e.Col).ToString();
            //Console.WriteLine("strBefValue1 : {0}", strBefValue);
        }

        /// <summary>
        /// grdMain AfterEdit 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
            // No,구분은 수정 불가
            if (grdMain.Col == 0)
            {
                grdMain.SetData(grdMain.Row, grdMain.Col, strBefValue);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue == grdMain.GetData(grdMain.Row, grdMain.Col).ToString())
            {
                //Console.WriteLine("strBefValue2 : {0}", strBefValue, grdMain.GetData(grdMain.Row, grdMain.Col));
                return;
            }


            // 추가된 열에 대한 수정은 INSERT 처리
            if (grdMain.GetData(grdMain.Row, "L_NUM").ToString() != "추가")
            {
                // USER_ID 수정 불가
                if (grdMain.Col == grdMain.Cols["USER_ID"].Index)
                {
                    grdMain.SetData(grdMain.Row, grdMain.Col, strBefValue);
                    return;
                }

                // 저장시 UPDATE로 처리하기 위해 flag set
                grdMain.SetData(grdMain.Row, 0, "수정");
                // Update 배경색 지정
                //grdMain.Rows[grdMain.Row].Style = grdMain.Styles["UpColor"];
                clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
            }
            if (e.Col == 3)
            {

                string key = grdMain.Rows[e.Row][e.Col].ToString();
                for (int i = 0; i < cbx_postCd.Items.Count; i++)
                {
                    if (key == ((KeyValuePair<string, string>)cbx_postCd.Items[i]).Key)
                    {
                        cbx_postCd.SelectedIndex = i;
                    }
                }

                // 추가된 열에 대한 수정은 INSERT 처리
                if (grdMain.GetData(selected_row, "L_NUM").ToString() != "추가")
                {
                    // USER_ID 수정 불가
                    if (grdMain.Col == grdMain.Cols["USER_ID"].Index)
                    {
                        return;
                    }
                    grdMain.SetData(selected_row, 0, "수정");
                    // Update 배경색 지정
                    clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
                }

            }

        }

        /// <summary>
        /// 저장클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveData()
        {
            if (this.scrAuthMod != "Y")
            {
                MessageBox.Show("수정 권한이 없습니다");
                return;
            }

            string Sql1 = string.Empty;
            string strMsg = string.Empty;

            #region 항목체크
            string check_value1 = string.Empty;
            string check_Cols_NM1 = string.Empty;
            string check_field_NM1 = string.Empty;
            string check_table_NM1 = string.Empty;

            string check_value2 = string.Empty;
            string check_Cols_NM2 = string.Empty;
            string check_field_NM2 = string.Empty;
            string check_table_NM2 = string.Empty;

            string check_value3 = string.Empty;
            string check_Cols_NM3 = string.Empty;
            string check_field_NM3 = string.Empty;
            string check_table_NM3 = string.Empty;

            string check_value4 = string.Empty;
            string check_Cols_NM4 = string.Empty;
            string check_field_NM4 = string.Empty;
            string check_table_NM4 = string.Empty;

            string check_keyColNM = string.Empty;
            string check_keyValue = string.Empty;

            string check_NM = string.Empty;

            string gubun_value = string.Empty;
            string show_msg = string.Empty;
            int checkrow = 0;

            bool isChange = false;
            //수정항목이 있는지 파악하고 물어보고 진행
            for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                gubun_value = grdMain.GetData(checkrow, "L_NUM").ToString();

                if (gubun_value == "삭제" || gubun_value == "수정")
                {
                    isChange = true;
                }


                if (gubun_value == "추가")
                {
                    #region USER_ID 체크
                    check_field_NM1 = "USER_ID";
                    check_table_NM1 = "TB_CM_USER";
                    check_value1 = grdMain.GetData(checkrow, check_field_NM1).ToString();
                    check_Cols_NM1 = grdMain[1, check_field_NM1].ToString();

                    if (string.IsNullOrEmpty(check_value1))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM1);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    if (vf.isContainHangul(check_value1))
                    {
                        MessageBox.Show("한글이 포함되어서는 안됩니다.");
                        return;
                    }

                    if (vf.getStrLen(check_value1) > 10)
                    {
                        MessageBox.Show("영문 및 숫자 10자 이하로 입력하시기 바랍니다..");
                        return;
                    }

                    if (vf.Has_Item(check_table_NM1, check_field_NM1, check_value1))
                    {
                        show_msg = string.Format("{0}가 중복되었습니다. 다시 입력해주세요.", check_Cols_NM1);
                        MessageBox.Show(show_msg);
                        return;
                    }
                    #endregion USER_ID 체크

                    #region NM 체크
                    check_field_NM2 = "NM";
                    check_table_NM2 = "TB_CM_USER";
                    check_value2 = grdMain.GetData(checkrow, check_field_NM2).ToString();
                    check_Cols_NM2 = grdMain[1, check_field_NM2].ToString();

                    if (string.IsNullOrEmpty(check_value2))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM2);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    if (vf.getStrLen(check_value2) > 10)
                    {
                        MessageBox.Show("영문 및 한글 10자 이하로 입력하시기 바랍니다..");
                        return;
                    }

                    #endregion NM 체크

                    //2017.08.18 OCJ 추가 -- 

                    #region POST_VALUE(직위) 체크
                    check_field_NM3 = "POST_VALUE";
                    check_table_NM3 = "TB_CM_USER";
                    check_value3 = grdMain.GetData(checkrow, check_field_NM3).ToString();
                    check_Cols_NM3 = grdMain[1, check_field_NM3].ToString();

                    if (string.IsNullOrEmpty(check_value3))
                    {
                        show_msg = string.Format("{0}를 선택해주세요.", check_Cols_NM3);
                        MessageBox.Show(show_msg);
                        return;
                    }
                    #endregion POST_VALUE 체크

                    #region DEPT_VALUE(부서명) 체크
                    check_field_NM4 = "DEPT_VALUE";
                    check_table_NM4 = "TB_CM_USER";
                    check_value4 = grdMain.GetData(checkrow, check_field_NM4).ToString();
                    check_Cols_NM4 = grdMain[1, check_field_NM4].ToString();

                    if (string.IsNullOrEmpty(check_value4))
                    {
                        show_msg = string.Format("{0}를 선택해주세요.", check_Cols_NM4);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    //--추가

                    //if (vf.getStrLen(check_value3) > 30)
                    //{
                    //    MessageBox.Show("영문 및 한글, 숫자 30자 이하로 입력하시기 바랍니다..");
                    //    return;
                    //}

                    #endregion

                    if (vf.Has_Item(check_table_NM2, check_field_NM2, check_value2) && vf.Has_Item(check_table_NM1, check_field_NM1, check_value1))
                    {
                        show_msg = string.Format("필수요소가 중복되었습니다. 다시 입력해주세요.");
                        MessageBox.Show(show_msg);
                        return;
                    }

                    isChange = true;
                }

            }
            if (txt_passWd.Text != txt_passWd2.Text)
            {
                show_msg = string.Format("패스워드가 동일하지 않습니다. 다시 입력해주세요.");
                MessageBox.Show(show_msg);

                return;
            }


            if (isChange)
            {
                if (MessageBox.Show("저장하시겠습니까?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

            }
            #endregion 삭제항목체크

            int row = 0;
            int InsCnt = 0;
            int UpCnt = 0;
            int DelCnt = 0;


            DataTable dt = null;

            //디비선언
            SqlConnection conn = cd.OConnect();

            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = null;

            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
                //정호준
                #region grdMain1
                for (row = 1; row < grdMain.Rows.Count; row++)
                {
                    // Update 처리
                    if (grdMain.GetData(row, "L_NUM").ToString() == "추가")
                    {

                        Sql1 += string.Format("/* 2017.06.02 사용자관리           */                                        ");
                        Sql1 += string.Format("INSERT INTO TB_CM_USER                                                       ");
                        Sql1 += string.Format("             (                                                               ");
                        Sql1 += string.Format("                USER_ID                                                      ");
                        Sql1 += string.Format("               ,NM                                                           ");
                        Sql1 += string.Format("               ,POST_CD                                                      ");
                        Sql1 += string.Format("               ,DEPT_CD                                                      ");
                        Sql1 += string.Format("               ,TEL_NO                                                       ");
                        Sql1 += string.Format("               ,MAIL_ID                                                      ");
                        Sql1 += string.Format("               ,PASSWD                                                       ");
                        Sql1 += string.Format("               ,ACL_GRP_ID                                                   ");
                        Sql1 += string.Format("               ,REGISTER                                                     ");
                        Sql1 += string.Format("               ,REG_DDTT                                                     ");
                        Sql1 += string.Format("              )                                                              ");
                        Sql1 += string.Format("              VALUES                                                         ");
                        Sql1 += string.Format("              (                                                              ");
                        Sql1 += string.Format("                '" + grdMain.GetData(row, "USER_ID") + "'      /* USER_ID  */");
                        Sql1 += string.Format("               ,'" + grdMain.GetData(row, "NM") + "'                /* NM  */");
                        Sql1 += string.Format("               ,'" + grdMain.GetData(row, "POST_CD") + "'      /* POST_CD  */");
                        Sql1 += string.Format("               ,'" + grdMain.GetData(row, "DEPT_CD") + "'      /* DEPT_CD  */");
                        Sql1 += string.Format("               ,'" + grdMain.GetData(row, "TEL_NO") + "'      /* TEL_NO   */");
                        Sql1 += string.Format("               ,'" + grdMain.GetData(row, "MAIL_ID") + "'      /* MAIL_ID  */");
                        Sql1 += string.Format("               ,'" + grdMain.GetData(row, "PASSWD") + "'      /* PASSWD   */");
                        Sql1 += string.Format("               ,'" + grdMain.GetData(row, "ACL_GRP_ID") + "'  /* ACL_GRP_ID*/");
                        Sql1 += string.Format("               ,'" + ck.UserID + "'                            /* REGISTER */");
                        Sql1 += string.Format("               ,(SELECT CONVERT(VARCHAR, GETDATE(), 120))      /* REG_DDTT */");
                        Sql1 += string.Format("              )                                                              ");

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        InsCnt++;
                    }
                    else if (grdMain.GetData(row, "L_NUM").ToString() == "수정")
                    {
                        Sql1 = string.Format(" UPDATE TB_CM_USER                                   ");
                        Sql1 += string.Format(" SET                                                         ");
                        Sql1 += string.Format("        NM = '" + grdMain.GetData(row, "NM") + "'  ");
                        Sql1 += string.Format("       ,PASSWD  = '" + grdMain.GetData(row, "PASSWD") + "'   ");
                        Sql1 += string.Format("       ,DEPT_CD = '" + grdMain.GetData(row, "DEPT_CD") + "'  ");
                        Sql1 += string.Format("       ,POST_CD = '" + grdMain.GetData(row, "POST_CD") + "'  ");
                        Sql1 += string.Format("       ,TEL_NO  = '" + grdMain.GetData(row, "TEL_NO") + "'   ");
                        Sql1 += string.Format("       ,MAIL_ID = '" + grdMain.GetData(row, "MAIL_ID") + "'  ");
                        Sql1 += string.Format("       ,ACL_GRP_ID = '" + grdMain.GetData(row, "ACL_GRP_ID") + "'  ");
                        Sql1 += string.Format("       ,MODIFIER = '" + ck.UserID + "'                       ");
                        Sql1 += string.Format("       ,MOD_DDTT = (SELECT CONVERT(VARCHAR, GETDATE(), 120)) ");
                        Sql1 += string.Format(" WHERE USER_ID = '" + grdMain.GetData(row, "USER_ID") + "'   ");

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        UpCnt++;
                    }
                    else if (grdMain.GetData(row, "L_NUM").ToString() == "삭제")
                    {
                        //Sql1 = string.Format(" /* 2017.06.02 사용자권한 삭제  */                    ");
                        //Sql1 += string.Format(" DELETE FROM TB_CM_USER                          ");
                        //Sql1 += string.Format("  WHERE USER_ID = '" + grdMain.GetData(row, "USER_ID") + "'   ");

                        //cmd.CommandText = Sql1;
                        //cmd.ExecuteNonQuery();


                        Sql1 = string.Format(" /* 2017.06.02 사용자관리 삭제  */                                                ");
                        Sql1 += string.Format(" DELETE FROM TB_CM_USER WHERE USER_ID = '" + grdMain.GetData(row, "USER_ID") + "'");

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();

                        DelCnt++;
                    }
                }
                #endregion grdMain1 

                
                //실행후 성공
                transaction.Commit();

                //SetDataBinding();

                // 성공시에 추가 수정 삭제 상황을 초기화시킴

                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), message);

                MessageBox.Show("저장되었습니다");

                ////커서유지 add [[
                //if (grdMain.GetData(grdMain.Row, "L_NUM").ToString().Trim() == "삭제") //현재 줄이 삭제이면 재조회만
                //{
                //    btnDisplay_Click(null, null);
                //}
                //else                                                                  //현재 줄이 삭제가 아니라면 재조회해서 현재줄로 커서 옮김
                //{
                //    int selRow = grdMain.Row;

                //    btnDisplay_Click(null, null);

                //    grdMain.Row = selRow;
                //    grdMain_Row_Selected(grdMain.Row, 0);
                //}

                Button_Click(btnDisplay, null);

                //커서유지 add ]]
            }
            catch (Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                // 추가되었을시에 중복성으로 실패시 메시지 표시
                MessageBox.Show("저장에 실패하였습니다. Error: " + ex.Message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();

            }

        }

        /// <summary>
        /// 조회
        /// </summary>
        private void SetDataBinding_grdMain()
        {

            try
            {

                //txt_UserId.ReadOnly = false;
                //txt_DeptValue.ReadOnly = false;
                //txt_UserId.Text = "";
                //txt_passWd.Text = "";
                //txt_passWd2.Text = "";
                //txt_UserNm.Text = "";
                //cbx_postCd.Text = "";

                //txt_DeptValue.Text = "";

                //txt_telNo.Text = "";
                //txt_mailId.Text = "";

                //SQL
                string sql = "";
                sql += string.Format(" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY USER_ID ASC)) AS L_NUM                                       ");
                sql += string.Format("      , USER_ID                                                                                                   ");
                sql += string.Format("      , NM                                                                                                        ");
                sql += string.Format(" 	 , POST_VALUE                                                                                                   ");
                sql += string.Format(" 	 , DEPT_VALUE                                                                                                   ");
                sql += string.Format(" 	 , DEPT_CD                                                                                                      ");
                sql += string.Format(" 	 , POST_CD                                                                                                      ");
                sql += string.Format(" 	 , PASSWD                                                                                                       ");
                sql += string.Format(" 	 , MAIL_ID                                                                                                      ");
                sql += string.Format(" 	 , TEL_NO                                                                                                       ");
                sql += string.Format(" 	 , ACL_GRP_ID                                                                                                   ");
                sql += string.Format(" 	 , ACL_GRP_NM                                                                                                   ");
                sql += string.Format("   FROM                                                                                                           ");
                sql += string.Format(" 	    (                                                                                                           ");
                sql += string.Format(" 		   SELECT	A.USER_ID                                                                                       ");
                sql += string.Format(" 				,	A.NM                                                                                            ");
                sql += string.Format(" 				,	(SELECT CD_NM FROM TB_CM_COM_CD WHERE CD_ID = A.POST_CD AND USE_YN = 'Y') AS POST_VALUE         ");
                sql += string.Format(" 				,	(SELECT CD_NM FROM TB_CM_COM_CD WHERE CD_ID = A.DEPT_CD AND USE_YN = 'Y') AS DEPT_VALUE         ");
                sql += string.Format(" 				,	A.DEPT_CD                                                                                       ");
                sql += string.Format(" 				,	A.POST_CD                                                                                       ");
                sql += string.Format(" 				,	A.PASSWD                                                                                        ");
                sql += string.Format(" 				,	A.MAIL_ID                                                                                       ");
                sql += string.Format(" 				,	A.TEL_NO                                                                                        ");
                sql += string.Format(" 				,	A.ACL_GRP_ID                                                                                    ");
                sql += string.Format(" 				,	(SELECT ACL_GRP_NM FROM TB_CM_ACL_GRP WHERE ACL_GRP_ID = A.ACL_GRP_ID) AS ACL_GRP_NM            ");
                sql += string.Format(" 			 FROM   TB_CM_USER  A                                                                                   ");
                sql += string.Format(" 		)   B                                                                                                       ");
                sql += string.Format(" WHERE B.USER_ID LIKE '%" + txtid.Trim() + "%'                                                                    ");
                sql += string.Format("   AND B.NM LIKE '%" + txtnm.Trim() + "%'                                                                         ");
                sql += string.Format("   AND B.DEPT_VALUE LIKE '%" + txtDept1.Text.Trim() + "%'                                                               ");

                //grdItem[1, 0] = "NO";
                //grdItem[1, 1] = "사용자ID";
                //grdItem[1, 2] = "성명";
                //grdItem[1, 3] = "직위";
                //grdItem[1, 4] = "부서";
                //grdItem[1, 5] = "직위코드";
                //grdItem[1, 6] = "부서코드";
                //grdItem[1, 7] = "패스워드";
                //grdItem[1, 8] = "메일";
                //grdItem[1, 9] = "전화번호";

                //SQL 쿼리 조회 후 데이터셋 return
                olddt = cd.FindDataTable(sql);

                if (olddt == null ) return;

                moddt = olddt.Copy();

                Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain, moddt);
                Cursor = Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} : {moddt.Rows.Count} 건 조회 되었습니다.");

                //grdSub 조회
                //if (moddt.Rows.Count > 0)
                //{
                //    grdMain_Row_Selected(2, 1);
                //}


                //txt_UserId.ReadOnly = true;
                //txt_DeptValue.ReadOnly = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }

        }

        /// <summary>
        /// grdMain 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMain_Click(object sender, EventArgs e)
        {
            if (grdMain.Rows.Count < 2 || grdMain.RowSel < 0) { return; }

            grdMain_Row_Selected(grdMain.RowSel, grdMain.ColSel);
        }

        /// <summary>
        /// grdMain 로우 선택 후 상세 및 권한 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMain_Row_Selected(int selectedRow, int selectedCell)
        {
            string Sql = "";

            try
            {
                txt_UserId.ReadOnly = false;
                txt_DeptValue.ReadOnly = false;

                selected_row = selectedRow;


                if (grdMain.Rows[selectedRow]["L_NUM"].ToString() == "추가")
                {
                    selected_user_id = "";
                }
                else
                {
                    selected_user_id = grdMain.Rows[selectedRow]["USER_ID"].ToString().Trim();
                }

                selected_user_nm = grdMain.Rows[selectedRow]["NM"].ToString();
                selected_passwd = grdMain.Rows[selectedRow]["PASSWD"].ToString();
                selected_tel_no = grdMain.Rows[selectedRow]["TEL_NO"].ToString();
                selected_mail_id = grdMain.Rows[selectedRow]["MAIL_ID"].ToString();
                selected_dept_cd = grdMain.Rows[selectedRow]["DEPT_CD"].ToString();
                selected_dept_value = grdMain.Rows[selectedRow]["DEPT_VALUE"].ToString();
                selected_post_cd = grdMain.Rows[selectedRow]["POST_CD"].ToString();
                selected_post_value = grdMain.Rows[selectedRow]["POST_VALUE"].ToString();
                selected_auth_id = grdMain.Rows[selectedRow]["ACL_GRP_ID"].ToString();
                selected_auth_nm = grdMain.Rows[selectedRow]["ACL_GRP_NM"].ToString();

                txt_DeptValue.ReadOnly = false;

                txt_UserId.Text = grdMain.Rows[selectedRow]["USER_ID"].ToString();
                txt_passWd.Text = grdMain.Rows[selectedRow]["PASSWD"].ToString();
                txt_passWd2.Text = grdMain.Rows[selectedRow]["PASSWD"].ToString();
                txt_UserNm.Text = grdMain.Rows[selectedRow]["NM"].ToString();
                txt_DeptValue.Text = grdMain.Rows[selectedRow]["DEPT_VALUE"].ToString();


                // 선택된 행의 직위코드가 설정되지않은경우 초기값(첫번째 코드)를 선택한다.
                // 그리드의 콤보박스 상세의 콤보박스 동기화 by 정호준
                
                //string key = ((KeyValuePair<string, string>)cbx_postCd.SelectedItem).Key;
                for (int i = 0; i < cbx_postCd.Items.Count; i++)
                {
                    if (((KeyValuePair<string, string>)cbx_postCd.Items[i]).Key.Equals(grdMain.Rows[selectedRow]["POST_CD"].ToString()))
                    {
                        cbx_postCd.SelectedIndex = i;
                    }
                }

                for (int i = 0; i < cbx_Auth.Items.Count; i++)
                {
                    if (((KeyValuePair<string, string>)cbx_Auth.Items[i]).Key.Equals(selected_auth_id))
                    {
                        cbx_Auth.SelectedIndex = i;
                    }
                }

                txt_telNo.Text = grdMain.Rows[selectedRow]["TEL_NO"].ToString();
                txt_mailId.Text = grdMain.Rows[selectedRow]["MAIL_ID"].ToString();



                //추가일때만 ID입력 가능
                if (grdMain.Rows[selectedRow]["L_NUM"].ToString() == "추가")
                {
                    txt_UserId.ReadOnly = false;
                }
                else
                {
                    txt_UserId.ReadOnly = true;
                }

                txt_DeptValue.ReadOnly = true;

                ////grdSub 시스템구분에 따른 권한 콤보 조회
                //if (grdSub.Rows.Count > 2)
                //{
                //    DataTable dt = cd.getAclGrp(grdSub.Rows[grdSub.RowSel][1].ToString());
                //    grdSubCombo2(dt);
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }
        }

        /// <summary>
        /// 우측 상세 ID 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_UserId_TextChanged(object sender, EventArgs e)
        {
            if(IsSelectedRow(grdMain))
                grdMain.Rows[selected_row][1] = txt_UserId.Text;
        }

        private bool IsSelectedRow(C1FlexGrid grdMain)
        {
            if (selected_row == 0) return false;
            if(selected_row > GridRowsCount)
               return true;

            return false;
        }

        /// <summary>
        /// 우측 상세 이름 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_UserNm_TextChanged(object sender, EventArgs e)
        {

            if (selected_user_nm == txt_UserNm.Text)
                return;

            if (selected_user_nm != txt_UserNm.Text)
            {
                grdMain.Rows[selected_row]["NM"] = txt_UserNm.Text;


                // 추가된 열에 대한 수정은 INSERT 처리
                if (grdMain.GetData(selected_row, "L_NUM").ToString() != "추가")
                {
                    // USER_ID 수정 불가
                    if (grdMain.Col == grdMain.Cols["USER_ID"].Index)
                    {
                        //grdMain.SetData(selected_row, "USER_ID", strBefValue);
                        return;
                    }

                    // 저장시 UPDATE로 처리하기 위해 flag set
                    grdMain.SetData(selected_row, 0, "수정");
                    // Update 배경색 지정
                    clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
                }
            }
        }

        /// <summary>
        /// 부서코드 돋보기 선택 이벤트
        /// </summary>
        /// <param name="sender"></param>
        private void pbx_DeptSearch_Click(object sender, EventArgs e)
        {
            if (this.scrAuthMod != "Y")
            {
                MessageBox.Show("수정 권한이 없습니다");
                return;
            }
            //부서코드 팝업 호출
            showDeptPopup();
        }

        /// <summary>
        /// 부서코드 팝업 호출
        /// </summary>
        /// <param name="sender"></param>
        private void showDeptPopup()
        {
            string titleNM = "부서선택";
            string menuNM = "부서명";
            string categoryNM = "DEPT_CD";
            PopUp dia = new PopUp(titleNM, menuNM, categoryNM);
            dia.frm = this;
            dia.StartPosition = FormStartPosition.CenterScreen;
            dia.FormSendEvent += new PopUp.FormSendDataHandler(callback);
            dia.ShowDialog();
        }

        /// <summary>
        /// 부서코드 팝업 콜백 함수
        /// </summary>
        /// <param name="sender"></param>
        private void callback(object sender)
        {
            


            SetParm();

            //grdMain.BeginUpdate();
            if (selected_dept_value == txt_DeptValue.Text)
                return;

            if (selected_dept_value != txt_DeptValue.Text)
            {
                //grdMain.Rows[selected_row]["DEPT_CD"] = txtdeptCd;


                // 추가된 열에 대한 수정은 INSERT 처리
                if (grdMain.GetData(selected_row, "L_NUM").ToString() != "추가")
                {
                    //// USER_ID 수정 불가
                    //if (grdMain.Col == grdMain.Cols["USER_ID"].Index)
                    //{
                    //    grdMain.SetData(selected_row, "USER_ID", strBefValue);
                    //    return;
                    //}

                    // 저장시 UPDATE로 처리하기 위해 flag set
                    grdMain.SetData(selected_row, 0, "수정");
                    // Update 배경색 지정
                    clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);

                }
            }
            //grdMain.EndUpdate();
            //grdMain.Invalidate();

        }

        private void SetParm()
        {

            //grdMain.BeginUpdate();
            txt_DeptValue.ReadOnly = false;
            txt_DeptValue.Text = txtdept;
            grdMain.Rows[selected_row]["DEPT_VALUE"] = txtdept;
            grdMain.Rows[selected_row]["DEPT_CD"] = txtdeptCd;
            txt_DeptValue.ReadOnly = true;

            //grdMain.EndUpdate();
            //grdMain.Invalidate();
        }

        private void cbx_postCd_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbx_postCd.SelectedIndex > 0)
            {

                string key = ((KeyValuePair<string, string>)cbx_postCd.SelectedItem).Key;
                string value = ((KeyValuePair<string, string>)cbx_postCd.SelectedItem).Value;
                //MessageBox.Show("key : " + key+" value : "+ value);
                //MessageBox.Show("selected_post_cd : " + selected_post_cd + " / "+key);
                if (selected_post_cd == key)
                    return;

                if (selected_post_cd != key)
                {
                    grdMain.Rows[selected_row]["POST_CD"] = key;
                    grdMain.Rows[selected_row]["POST_VALUE"] = value;

                    // 추가된 열에 대한 수정은 INSERT 처리
                    if (grdMain.GetData(selected_row, "L_NUM").ToString() != "추가")
                    {
                        // USER_ID 수정 불가
                        if (grdMain.Col == grdMain.Cols["USER_ID"].Index)
                        {
                            //grdMain.SetData(selected_row, "USER_ID", strBefValue);
                            return;
                        }

                        // 저장시 UPDATE로 처리하기 위해 flag set
                        // grdMain.SetData(grdMain.Row, grdMain.Cols.Count - 1, "수정");
                        grdMain.SetData(selected_row, 0, "수정");
                        // Update 배경색 지정
                        //grdMain.Rows[selected_row].Style = grdMain.Styles["UpColor"];
                        clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
                    }
                }

            }
        }

        private void txt_passWd_TextChanged(object sender, EventArgs e)
        {
            //// 수정된 내용이 없으면 Update 처리하지 않는다.
            if (selected_passwd == txt_passWd.Text)
                return;

            if (selected_passwd != txt_passWd.Text)
            {
                grdMain.Rows[selected_row]["PASSWD"] = txt_passWd.Text;


                // 추가된 열에 대한 수정은 INSERT 처리
                if (grdMain.GetData(selected_row, "L_NUM").ToString() != "추가")
                {
                    // USER_ID 수정 불가
                    if (grdMain.Col == grdMain.Cols["USER_ID"].Index)
                    {
                        //grdMain.SetData(selected_row, "USER_ID", strBefValue);
                        return;
                    }

                    // 저장시 UPDATE로 처리하기 위해 flag set
                    // grdMain.SetData(grdMain.Row, grdMain.Cols.Count - 1, "수정");
                    grdMain.SetData(selected_row, 0, "수정");
                    // Update 배경색 지정
                    //grdMain.Rows[selected_row].Style = grdMain.Styles["UpColor"];
                    clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
                }
            }

        }

        private void txt_telNo_TextChanged(object sender, EventArgs e)
        {
            if (selected_tel_no == txt_telNo.Text)
                return;

            if (selected_tel_no != txt_telNo.Text)
            {
                grdMain.Rows[selected_row]["TEL_NO"] = txt_telNo.Text;
                //// 수정된 내용이 없으면 Update 처리하지 않는다.


                // 추가된 열에 대한 수정은 INSERT 처리
                if (grdMain.GetData(selected_row, "L_NUM").ToString() != "추가")
                {
                    // USER_ID 수정 불가
                    if (grdMain.Col == grdMain.Cols["USER_ID"].Index)
                    {
                        //grdMain.SetData(selected_row, "USER_ID", strBefValue);
                        return;
                    }

                    // 저장시 UPDATE로 처리하기 위해 flag set
                    // grdMain.SetData(grdMain.Row, grdMain.Cols.Count - 1, "수정");
                    grdMain.SetData(selected_row, 0, "수정");
                    // Update 배경색 지정
                    //grdMain.Rows[selected_row].Style = grdMain.Styles["UpColor"];
                    clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
                }
            }
        }

        private void txt_mailId_TextChanged(object sender, EventArgs e)
        {

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (selected_mail_id == txt_mailId.Text)
                return;

            if (selected_mail_id != txt_mailId.Text)
            {
                grdMain.Rows[selected_row]["MAIL_ID"] = txt_mailId.Text;


                // 추가된 열에 대한 수정은 INSERT 처리
                if (grdMain.GetData(selected_row, "L_NUM").ToString() != "추가")
                {
                    // USER_ID 수정 불가
                    if (grdMain.Col == grdMain.Cols["USER_ID"].Index)
                    {
                        //grdMain.SetData(selected_row, "USER_ID", strBefValue);
                        return;
                    }

                    // 저장시 UPDATE로 처리하기 위해 flag set
                    // grdMain.SetData(grdMain.Row, grdMain.Cols.Count - 1, "수정");
                    grdMain.SetData(selected_row, 0, "수정");
                    // Update 배경색 지정
                    //grdMain.Rows[selected_row].Style = grdMain.Styles["UpColor"];
                    clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
                }
            }
        }



        //부서 컬럼에는 손모양 마우스로 변경
        private void grdMain_MouseMove(object sender, MouseEventArgs e)
        {
            //HitTestInfo ht = grdMain.HitTest(e.X, e.Y);
            //bool hand = false;
            //if (ht.Column == 4)
            //{
            //    hand = true;
            //}
            //Cursor = (hand) ? Cursors.Hand : Cursors.Default;

        }

        private void grdMain_DoubleClick(object sender, EventArgs e)
        {

            if (grdMain.Rows.Count < 2 || grdMain.RowSel < 2) { return; }
            //if (grdMain.ColSel == 4)
            //{
            //    //부서선택 팝업
            //    showDeptPopup();
            //}


        }

        private void bt_Reset_Click(object sender, EventArgs e)
        {
            if (this.scrAuthMod != "Y")
            {
                MessageBox.Show("수정 권한이 없습니다");
                return;
            }

            if (txt_UserId.Text == "")
            {
                MessageBox.Show("사용자ID를 입력해 주십시오.");
                return;
            }


            txt_passWd.Text = "a!" + txt_UserId.Text.ToString();
            txt_passWd2.Text = "a!" + txt_UserId.Text.ToString();

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Format("부서명 {0}", grdMain[selected_row, "DEPT_VALUE"]));
        }

        private void cbx_Auth_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void txt_DeptValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbx_Auth_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;


            if (cb.SelectedIndex >= 0)
            {
                string key = ((KeyValuePair<string, string>)cb.SelectedItem).Key;
                string value = ((KeyValuePair<string, string>)cb.SelectedItem).Value;
                //MessageBox.Show("key : " + key+" value : "+ value);
                //MessageBox.Show("selected_post_cd : " + selected_post_cd + " / "+key);
                if (!CanRowSelectGrid(grdMain, GridRowsCount)) return;

                grdMain.Rows[selected_row]["ACL_GRP_ID"] = key;
                grdMain.Rows[selected_row]["ACL_GRP_NM"] = value;

                // 추가된 열에 대한 수정은 INSERT 처리
                if (grdMain.GetData(selected_row, "L_NUM").ToString() != "추가")
                {
                    // USER_ID 수정 불가
                    if (grdMain.Col == grdMain.Cols["USER_ID"].Index)
                    {
                        //grdMain.SetData(selected_row, "USER_ID", strBefValue);
                        return;
                    }

                    // 저장시 UPDATE로 처리하기 위해 flag set
                    // grdMain.SetData(grdMain.Row, grdMain.Cols.Count - 1, "수정");
                    grdMain.SetData(selected_row, 0, "수정");
                    // Update 배경색 지정
                    //grdMain.Rows[selected_row].Style = grdMain.Styles["UpColor"];
                    clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
                }
            }
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

                    if (CanRowSelectGrid(grdMain, GridRowsCount))
                        grdMain_Row_Selected(GridRowsCount, 1);
                    else
                    {
                        // 입련란 초기화
                        InitInPut();
                    }

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
            }
        }



        private void SaveExcel()
        {
        }
    }
}
