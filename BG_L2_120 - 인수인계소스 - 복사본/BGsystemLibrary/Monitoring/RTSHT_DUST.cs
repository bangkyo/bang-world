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
    public partial class RTSHT_DUST : Form
    {
        //공통변수
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();
        clsUtil cu = new clsUtil();

        //데이터테이블
        DataTable olddtMain1 = null;
        DataTable moddtMain1 = null;

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

        public RTSHT_DUST(string titleNm, string scrAuth, string factCode, string ownerNm)
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

        private void RTSHT_DUST_Click(object sender, EventArgs e)
        {

        }

        private void RTSHT_DUST_Load(object sender, EventArgs e)
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


                string sql = string.Format(@"   SELECT TOP 1 WORK_DDTT
                                                      ,ISNULL(Bag_BEF_STATICP       ,0) AS Bag_BEF_STATICP         	--Bag 전단 정압계
                                                      ,ISNULL(INH_TEMP				,0) AS INH_TEMP					--흡입 온도
                                                      ,ISNULL(Screw_Conveyor_STAT	,0) AS Screw_Conveyor_STAT		--Screw Conveyor 상태
                                                      ,ISNULL(Rotary_Valve_STAT		,0) AS Rotary_Valve_STAT		--Rotary Valve 상태
                                                      ,ISNULL(Vibrator_STAT			,0) AS Vibrator_STAT			--Vibrator 상태
                                                      ,ISNULL(Pulse_Controller_STAT1	,0) AS Pulse_Controller_STAT1	--Pulse Controller1 상태
                                                      ,ISNULL(DEF_PRES				,0) AS DEF_PRES					--차압
                                                      ,ISNULL(OPRN_RT				,0) AS OPRN_RT					--가동율
                                                      ,ISNULL(SETV					,0) AS SETV						--세팅값
                                                      ,ISNULL(LOAD_BRG_TEMP			,0) AS LOAD_BRG_TEMP			--부하측 베어링 온도
                                                      ,ISNULL(H_LOAD_BRG_TEMP		,0) AS H_LOAD_BRG_TEMP			--반부하측 베어링 온도
                                                      ,ISNULL(IVT_SPEED_SETV		,0) AS IVT_SPEED_SETV			--인버터 속도 설정값
                                                      ,ISNULL(LOAD_VIBR				,0) AS LOAD_VIBR				--부하 진동
                                                      ,ISNULL(H_LOAD_VIBR			,0) AS H_LOAD_VIBR				--반부하 진동
                                                  FROM TB_SHT_DUST_INFO
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

                            if (!string .IsNullOrEmpty(uc_keyValue.PLC_CD))
                            {
                                uc_keyValue.PLC_ITEM_VALUE = moddtMain1.Rows[0][uc_keyValue.PLC_CD].ToString();
                            }

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
