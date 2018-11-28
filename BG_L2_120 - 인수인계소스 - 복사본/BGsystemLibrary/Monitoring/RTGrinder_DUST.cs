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
    public partial class RTGrinder_DUST : Form
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


        string ownerNM = "";
        string titleNM = "";

        //그리드 변수
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();


        public delegate void ReFreshDelegate(string content);


        private int main1_GridRowsCount = 6;
        private int main1_GridColsCount = 2;
        private int main1_RowsFixed = 1;
        private int main1_RowsFrozen = 0;
        private int main1_ColsFixed = 1;
        private int main1_ColsFrozen = 0;
        private int main1_TopRowsHeight = 2;
        private int main1_DataRowsHeight = 36;

        private int main2_GridRowsCount = 6;
        private int main2_GridColsCount = 2;
        private int main2_RowsFixed = 1;
        private int main2_RowsFrozen = 0;
        private int main2_ColsFixed = 1;
        private int main2_ColsFrozen = 0;
        private int main2_TopRowsHeight = 2;
        private int main2_DataRowsHeight = 36;

        private int main3_GridRowsCount = 3;
        private int main3_GridColsCount = 13;
        private int main3_RowsFixed = 3;
        private int main3_RowsFrozen = 0;
        private int main3_ColsFixed = 0;
        private int main3_ColsFrozen = 0;
        private int main3_TopRowsHeight = 2;
        private int main3_DataRowsHeight = 35;

        private int TopRowsHeight = 2;
        private int DataRowsHeight = 36;

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한
        //권한관련 add ]]


        private Task t;
        private Stopwatch sw;
        private TextBox IsConnected = new TextBox();

        public RTGrinder_DUST(string titleNm, string scrAuth, string factCode, string ownerNm)
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

        private void RTGrinder_DUST_Click(object sender, EventArgs e)
        {

        }

        private void RTGrinder_DUST_Load(object sender, EventArgs e)
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


                string sql = string.Format(@"SELECT TOP 1 WORK_DDTT
                                                  ,ISNULL(Bag_BEF_STATICP      , 0) AS 	 Bag_BEF_STATICP       --  Bag 전단 정압계
                                                  ,ISNULL(INH_TEMP			   , 0) AS 	 INH_TEMP			   --  흡입 온도
                                                  ,ISNULL(Rotary_Valve_NO1_STAT, 0) AS 	 Rotary_Valve_NO1_STAT --  Rotary Valve NO1 상태
                                                  ,ISNULL(Rotary_Valve_NO2_STAT, 0) AS 	 Rotary_Valve_NO2_STAT --  Rotary Valve NO2 상태
                                                  ,ISNULL(Rotary_Valve_NO3_STAT, 0) AS 	 Rotary_Valve_NO3_STAT --  Rotary Valve NO3 상태
                                                  ,ISNULL(Rotary_Valve_NO4_STAT, 0) AS 	 Rotary_Valve_NO4_STAT --  Rotary Valve NO4 상태
                                                  ,ISNULL(Vibrator_NO1_STAT	   , 0) AS 	 Vibrator_NO1_STAT	   --  Vibrator NO1 상태
                                                  ,ISNULL(Vibrator_NO2_STAT	   , 0) AS 	 Vibrator_NO2_STAT	   --  Vibrator NO2 상태
                                                  ,ISNULL(Vibrator_NO3_STAT	   , 0) AS 	 Vibrator_NO3_STAT	   --  Vibrator NO3 상태
                                                  ,ISNULL(Pulse_Controller_STAT1, 0) AS 	 Pulse_Controller_STAT1 --  Pulse Controller 상태
                                                  ,ISNULL(Popet_Damper_NO1_STAT, 0) AS 	 Popet_Damper_NO1_STAT --  Popet Damper NO1 상태
                                                  ,ISNULL(Popet_Damper_NO2_STAT, 0) AS 	 Popet_Damper_NO2_STAT --  Popet Damper NO2 상태
                                                  ,ISNULL(Popet_Damper_NO3_STAT, 0) AS 	 Popet_Damper_NO3_STAT --  Popet Damper NO3 상태
                                                  ,ISNULL(DEF_PRES_NO1		   , 0) AS 	 DEF_PRES_NO1		   --  차압 NO1
                                                  ,ISNULL(DEF_PRES_NO2		   , 0) AS 	 DEF_PRES_NO2		   --  차압 NO2
                                                  ,ISNULL(DEF_PRES_NO3		   , 0) AS 	 DEF_PRES_NO3		   --  차압 NO3
                                                  ,ISNULL(OPRN_RT			   , 0) AS 	 OPRN_RT			   --  가동율
                                                  ,ISNULL(SETV				   , 0) AS 	 SETV				   --  세팅값
                                                  ,ISNULL(LOAD_BRG_TEMP		   , 0) AS 	 LOAD_BRG_TEMP		   --  부하측 베어링 온도
                                                  ,ISNULL(H_LOAD_BRG_TEMP	   , 0) AS 	 H_LOAD_BRG_TEMP	   --  반부하측 베어링 온도
                                                  ,ISNULL(IVT_SPEED_SETV	   , 0) AS 	 IVT_SPEED_SETV	   	   --  인버터 속도 설정값
                                                  ,ISNULL(LOAD_VIBR			   , 0) AS 	 LOAD_VIBR			   --  부하 진동
                                                  ,ISNULL(H_LOAD_VIBR		   , 0) AS 	 H_LOAD_VIBR		   --  반부하 진동
                                              FROM TB_GRD_DUST_INFO
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

                            if (!string.IsNullOrEmpty(uc_keyValue.PLC_CD))
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
