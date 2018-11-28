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
    public partial class RTSHT : Form
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

        public RTSHT(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            //string[] scrAuthParams = scrAuth.Split(',');

            //this.scrAuthInq = scrAuthParams[1];   //조회 권한 저장
            //this.scrAuthReg = scrAuthParams[2];   //등록(추가) 권한 저장
            //this.scrAuthMod = scrAuthParams[3];   //수정 권한 저장
            //this.scrAuthDel = scrAuthParams[4];   //삭제 권한 저장

            InitializeComponent();
        }

        private void RTSHT_Click(object sender, EventArgs e)
        {

        }

        private void RTSHT_Load(object sender, EventArgs e)
        {


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


            Invalidate();
        }



        private void SetDataBinding()
        {
            //GrdDataClear1_2();

            try
            {
                string sql = string.Format(@"       SELECT TOP 1 WORK_DDTT
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(ROLLER_CONVR_SPEED      ,0) AS MONEY), 1) , '.00', '')   AS   ROLLER_CONVR_SPEED        --롤러_컨베어_속도
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(IMPELLER_NO1_SPEED	    ,0) AS MONEY), 1) , '.00', '')   AS  IMPELLER_NO1_SPEED	       --임펠라_NO1_속도
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(IMPELLER_NO2_SPEED	    ,0) AS MONEY), 1) , '.00', '')   AS  IMPELLER_NO2_SPEED	       --임펠라_NO2_속도
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(IMPELLER_NO3_SPEED	    ,0) AS MONEY), 1) , '.00', '')   AS  IMPELLER_NO3_SPEED	       --임펠라_NO3_속도
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(IMPELLER_NO4_SPEED	    ,0) AS MONEY), 1) , '.00', '')   AS  IMPELLER_NO4_SPEED	       --임펠라_NO4_속도
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(ROLLER_CONVR_CURRENT    ,0) AS MONEY), 1) , '.00', '')   AS  ROLLER_CONVR_CURRENT	   --롤러_컨베어_전류
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(IMPELLER_NO1_CURRENT    ,0) AS MONEY), 1) , '.00', '')   AS  IMPELLER_NO1_CURRENT	   --임펠라_NO1_전류
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(IMPELLER_NO2_CURRENT    ,0) AS MONEY), 1) , '.00', '')   AS  IMPELLER_NO2_CURRENT	   --임펠라_NO2_전류
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(IMPELLER_NO3_CURRENT    ,0) AS MONEY), 1) , '.00', '')   AS  IMPELLER_NO3_CURRENT	   --임펠라_NO3_전류
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(IMPELLER_NO4_CURRENT    ,0) AS MONEY), 1) , '.00', '')   AS  IMPELLER_NO4_CURRENT	   --임펠라_NO4_전류
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(ROTY_SCR_CURRENT	    ,0) AS MONEY), 1) , '.00', '')   AS  ROTY_SCR_CURRENT		   --로타리_스크린_전류
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(BCKT_LVTR_CURRENT	    ,0) AS MONEY), 1) , '.00', '')   AS  BCKT_LVTR_CURRENT		   --버켓_엘리베이터_전류
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(SCREW_CONVR1_CURRENT    ,0) AS MONEY), 1) , '.00', '')   AS  SCREW_CONVR1_CURRENT	   --스크류_컨베어1_전류
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(SCREW_CONVR2_CURRENT    ,0) AS MONEY), 1) , '.00', '')   AS  SCREW_CONVR2_CURRENT	   --스크류_컨베어2_전류
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(FAN_CURRENT			    ,0) AS MONEY), 1) , '.00', '')   AS  FAN_CURRENT			   --송풍기_전류
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(ROLLER_CONVR_SPEED_SETV ,0) AS MONEY), 1) , '.00', '')   AS  ROLLER_CONVR_SPEED_SETV   --롤러_컨베어_속도_설정값
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(IMPELLER_NO1_SPEED_SETV ,0) AS MONEY), 1) , '.00', '')   AS  IMPELLER_NO1_SPEED_SETV   --임펠라_NO1_속도_설정값
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(IMPELLER_NO2_SPEED_SETV ,0) AS MONEY), 1) , '.00', '')   AS  IMPELLER_NO2_SPEED_SETV   --임펠라_NO2_속도_설정값
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(IMPELLER_NO3_SPEED_SETV ,0) AS MONEY), 1) , '.00', '')   AS  IMPELLER_NO3_SPEED_SETV   --임펠라_NO3_속도_설정값
                                                          ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(IMPELLER_NO4_SPEED_SETV ,0) AS MONEY), 1) , '.00', '')   AS  IMPELLER_NO4_SPEED_SETV   --임펠라_NO4_속도_설정값
                                                      FROM TB_SHT_OPER_INFO
                                                    ORDER BY WORK_DDTT DESC");

                olddtMain1 = cd.FindDataTable(sql);
                moddtMain1 = olddtMain1.Copy();

                if (moddtMain1.Rows.Count > 0)
                {
                    foreach (var item in vf.GetAllChildrens(this))
                    {
                        if (item.GetType().ToString() == "ComLib.UC_KeyValue_S")
                        {
                            var uc_keyValue = item as UC_KeyValue_S;

                            uc_keyValue.PLC_ITEM_VALUE = moddtMain1.Rows[0][uc_keyValue.PLC_CD].ToString();

                        }

                    }
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
        }

    }

}
