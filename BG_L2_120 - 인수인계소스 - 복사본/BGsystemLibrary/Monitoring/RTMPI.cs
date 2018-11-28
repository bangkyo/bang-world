using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BGsystemLibrary.Monitoring
{
    public partial class RTMPI : Form
    {
        //공통변수
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();
        clsUtil cu = new clsUtil();

        //데이터테이블
        DataTable olddt;
        DataTable moddt;
        DataTable olddt_sub;
        DataTable moddt_sub;
        DataTable grdMainDT;
        DataTable grdSubDT;

        DataTable olddtMain3 = null;
        DataTable moddtMain3 = null;

        DataTable olddtMain1 = null;
        DataTable moddtMain1 = null;

        TextBox tbCategory;
        TextBox tbCD_ID;


        List<string> msg;
        bool _CanSaveSearchLog = false;
        string selected_Category = "";

        // 셀의 수정전 값
        string strBefValue = "";
        string strBefValue2 = "";

        string ownerNM = "";
        string titleNM = "";

        //그리드 변수
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();
        static C1FlexGrid selectedGrd;

        public delegate void ReFreshDelegate(string content);



        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한
        //권한관련 add ]]


        private Task t;
        private Stopwatch sw;
        private TextBox IsConnected = new TextBox();

        public RTMPI(string titleNm, string scrAuth, string factCode, string ownerNm)
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

        private void RTMPI_Click(object sender, EventArgs e)
        {

        }

        private void RTMPI_Load(object sender, EventArgs e)
        {

            //UC_Zone_Setup();

            //Setup_UCZoneMoveBtn();

            //InitGrid();

            IsConnected.TextChanged += IsConnected_TextChanged;

            sw = new Stopwatch();
            t = Task.Factory.StartNew(AsyncUpdateUIData);
        }
        
        


        private void IsConnected_TextChanged(object sender, EventArgs e)
        {
            if (IsConnected.Text == "Connected")
            {
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + " 데이터베이스 접속되었습니다.");
            }
            else
            {
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + " 데이터베이스 접속이 해제되었습니다.");
            }
        }

        private void AsyncUpdateUIData()
        {
            int test = 0;
            while (!IsDisposed)
            {
                // do something
                //Text = DateTime.Now.ToString();
                sw.Reset();
                sw.Start();
                //Console.WriteLine("update start"+DateTime.Now.ToString());
                UpdateUIData();
                //Text = DateTime.Now.ToString();
                sw.Stop();
                //Console.WriteLine("update Elapsed:" + sw.Elapsed.TotalMilliseconds);
                //Console.WriteLine("update Time:" + DateTime.Now);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }

        private void UpdateUIData()
        {
            if (!cd.CheckDbConnection())
            {
                IsConnected.Text = "DisConnected";
                this.Enable(false);
                return;
            }


            //InitGrd();
            this.Enable(true);
            IsConnected.Text = "Connected";

            SetDataBinding();

            //cd.CheckDbConnection();
            //현재의 근조 표시
            //SetWorkBinding(line_gp, uC_WorkShow1);

            //SetDataBinding();

            //SetDataBinding_Zone();

            Invalidate();
        }



        private void SetDataBinding()
        {
            //GrdDataClear1_2();

            try
            {


                string sql = string.Format(@"SELECT TOP 1 
                                                    WORK_DDTT
                                                   ,ISNULL(CLWTR_PMP1_STAT          , 0) AS CLWTR_PMP1_STAT         --  세척용수_펌프1_상태    
                                                   ,ISNULL(CLWTR_PMP2_STAT		    , 0) AS CLWTR_PMP2_STAT		    --  세척용수_펌프2_상태    
                                                   ,ISNULL(FIL_MTR_STAT			    , 0) AS FIL_MTR_STAT			--  필터용_모터_상태       
                                                   ,ISNULL(CLWTR_PMP_HDR_STAT	    , 0) AS CLWTR_PMP_HDR_STAT	    --  세척용수_펌프_헤더_상태
                                                   ,ISNULL(MAGNET_PMP1_STAT		    , 0) AS MAGNET_PMP1_STAT		--  자화_펌프1_상태        
                                                   ,ISNULL(MAGNET_PMP2_STAT		    , 0) AS MAGNET_PMP2_STAT		--  자화_펌프2_상태        
                                                   ,ISNULL(STIR_PMP_HDR_STAT	    , 0) AS STIR_PMP_HDR_STAT		--  교반_펌프_헤더_상태    
                                                   ,ISNULL(Entry_Motor_FWD		    , 0) AS Entry_Motor_FWD		    --  Entry_Motor_FWD        
                                                   ,ISNULL(Entry_Motor_REV		    , 0) AS Entry_Motor_REV		    --  Entry_Motor_REV        
                                                   ,ISNULL(Inspection_Motor_FWD	    , 0) AS Inspection_Motor_FWD    --  Inspection_Motor_FWD   
                                                   ,ISNULL(Inspection_Motor_REV	    , 0) AS Inspection_Motor_REV    --  Inspection_Motor_REV   
                                                   ,ISNULL(OPRS_Motor1_STAT		    , 0) AS OPRS_Motor1_STAT		--  유압_Motor1_상태       
                                                   ,ISNULL(OPRS_Motor2_STAT		    , 0) AS OPRS_Motor2_STAT		--  유압_Motor2_상태       
                                                   ,ISNULL(OPRS_Fan_STAT		    , 0) AS OPRS_Fan_STAT			--  유압_Fan_상태          
                                                   ,ISNULL(Dark_Room_Hood_Fan	    , 0) AS Dark_Room_Hood_Fan	    --  Dark_Room_Hood_Fan     
                                                   ,ISNULL(Yoke_TRANS_Motor1_FWD	, 0) AS Yoke_TRANS_Motor1_FWD   --  Yoke_이송_Motor1_FWD   
                                                   ,ISNULL(Yoke_TRANS_Motor1_BWD	, 0) AS Yoke_TRANS_Motor1_BWD   --  Yoke_이송_Motor1_BWD   
                                                   ,ISNULL(Yoke_TRANS_Motor2_BWD	, 0) AS Yoke_TRANS_Motor2_BWD   --  Yoke_이송_Motor2_BWD   
                                                   ,ISNULL(Yoke_TRANS_Motor2_FWD	, 0) AS Yoke_TRANS_Motor2_FWD   --  Yoke_이송_Motor2_FWD   
                                                   ,ISNULL(OPRS_MTR_HDR_STAT		, 0) AS OPRS_MTR_HDR_STAT		--  유압_모터_헤더_상태    
                                                   ,ISNULL(OPRS_SUP_HDR_STAT		, 0) AS OPRS_SUP_HDR_STAT		--  유압_공급용_헤더_상태  
                                                   ,ISNULL(Yoke_MTR_NO1_CURRENT	    , 0) AS Yoke_MTR_NO1_CURRENT	--  Yoke_모터_NO1_전류     
                                                   ,ISNULL(Yoke_MTR_NO2_CURRENT	    , 0) AS Yoke_MTR_NO2_CURRENT	--  Yoke_모터_NO2_전류     
                                                   ,ISNULL(Yoke_MTR_NO3_CURRENT	    , 0) AS Yoke_MTR_NO3_CURRENT	--  Yoke_모터_NO3_전류     
                                                   ,ISNULL(Yoke_MTR_NO4_CURRENT	    , 0) AS Yoke_MTR_NO4_CURRENT	--  Yoke_모터_NO4_전류     
                                                   ,ISNULL(COIL_NO1_2_CURRENT	    , 0) AS COIL_NO1_2_CURRENT	    --  코일_NO1_2_전류        
                                               FROM TB_MPI_OPER_INFO                
                                             ORDER BY WORK_DDTT DESC");

                olddtMain1 = cd.FindDataTable(sql);
                moddtMain1 = olddtMain1.Copy();

                if (moddtMain1.Rows.Count > 0)
                {
                    foreach (var item in vf.GetAllChildrens(this))
                    {
                        if (item.GetType().ToString() == "ComLib.UC_KeyValue_Status1")
                        {
                            var uc_keyValue = item as UC_KeyValue_Status1;

                            if (uc_keyValue.PLC_CD.Length > 0)
                            {
                                uc_keyValue.PLC_STATE = (int)moddtMain1.Rows[0][uc_keyValue.PLC_CD];
                                //Console.WriteLine("{0}:{1}", uc_keyValue.Name, uc_keyValue.PLC_STATE);
                            }

                           

                        }else if (item.GetType().ToString() == "ComLib.UC_KeyValue_S01")
                        {
                            var uc_keyValue = item as UC_KeyValue_S01;

                            if (uc_keyValue.PLC_CD.Length > 0)
                            uc_keyValue.PLC_ITEM_VALUE = moddtMain1.Rows[0][uc_keyValue.PLC_CD].ToString();

                        }

                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

    }

}
