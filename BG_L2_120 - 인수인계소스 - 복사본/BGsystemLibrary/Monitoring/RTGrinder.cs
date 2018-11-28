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
    public partial class RTGrinder : Form
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

        enum ShowMode
        {
                 NoTurn
                ,Turn
        }

        Point rightPoint;
        Point bottomPoint;

        public RTGrinder(string titleNm, string scrAuth, string factCode, string ownerNm)
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

            rightPoint = uC_KeyValueNoTHeight.Location;
            bottomPoint = uC_KeyValueNoTWidth.Location;
        }

        private void RTGrinder_Click(object sender, EventArgs e)
        {

        }

        private void RTGrinder_Load(object sender, EventArgs e)
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
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(GRD_DRV_MODE             , '')AS MONEY), 1) , '.00', '')   AS       GRD_DRV_MODE           --   그라인딩 운전 모드
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(HEIGHT_SETV              , '')AS MONEY), 1) , '.00', '')   AS       HEIGHT_SETV            --   높이 설정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(WIDTH_SETV               , '')AS MONEY), 1) , '.00', '')   AS       WIDTH_SETV             --   폭 설정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(HEIGHT_MV                , '')AS MONEY), 1) , '.00', '')   AS       HEIGHT_MV              --   높이 측정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(WIDTH_MV                 , '')AS MONEY), 1) , '.00', '')   AS       WIDTH_MV               --   폭 측정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(LENGTH_MV                , '')AS MONEY), 1) , '.00', '')   AS       LENGTH_MV              --   길이 측정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(FACE_PASS_CNT            , '')AS MONEY), 1) , '.00', '')   AS       FACE_PASS_CNT          --   면 Pass수
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(CORN_PASS_CNT            , '')AS MONEY), 1) , '.00', '')   AS       CORN_PASS_CNT          --   코너 Pass수
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(WORK_SEQ                 , '')AS MONEY), 1) , '.00', '')   AS       WORK_SEQ               --   작업 순서
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(WORK_Track_NO            , '')AS MONEY), 1) , '.00', '')   AS       WORK_Track_NO          --   작업 Track NO
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(RBGW_SPEED_SETV          , '')AS MONEY), 1) , '.00', '')   AS       RBGW_SPEED_SETV        --   연마지석 속도 설정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(RBGW_ADV_SETV            , '')AS MONEY), 1) , '.00', '')   AS       RBGW_ADV_SETV          --   연마지석 전진 설정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(RBGW_DIA_MV              , '')AS MONEY), 1) , '.00', '')   AS       RBGW_DIA_MV            --   연마지석 직경 측정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(RBGW_ENGY_MV             , '')AS MONEY), 1) , '.00', '')   AS       RBGW_ENGY_MV           --   연마지석 에너지 측정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(RBGW_VIBR_MV             , '')AS MONEY), 1) , '.00', '')   AS       RBGW_VIBR_MV           --   연마지석 진동 측정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(ROTANGLE_SETV            , '')AS MONEY), 1) , '.00', '')   AS       ROTANGLE_SETV          --   회전각도 설정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(ROTANGLE_MV              , '')AS MONEY), 1) , '.00', '')   AS       ROTANGLE_MV            --   회전각도 측정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(GRD_PRES_SURFACE_SETV    , '')AS MONEY), 1) , '.00', '')   AS       GRD_PRES_SURFACE_SETV  --   그라인딩 압력 표면 설정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(GRD_PRES_ENDP_SETV       , '')AS MONEY), 1) , '.00', '')   AS       GRD_PRES_ENDP_SETV     --   그라인딩 압력 끝단 설정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(GRD_PRES_CORN_SETV       , '')AS MONEY), 1) , '.00', '')   AS       GRD_PRES_CORN_SETV     --   그라인딩 압력 코너 설정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(SET_GRD_Force            , '')AS MONEY), 1) , '.00', '')   AS       SET_GRD_Force          --   그라인드 압력 설정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(GRD_PRES_MV              , '')AS MONEY), 1) , '.00', '')   AS       GRD_PRES_MV            --   그라인딩 압력 측정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(GRD_MTR_ELEC             , '')AS MONEY), 1) , '.00', '')   AS       GRD_MTR_ELEC           --   그라인딩 모터 전력값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(TRUCKSPEED_SETV          , '')AS MONEY), 1) , '.00', '')   AS       TRUCKSPEED_SETV        --   대차속도 설정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(TRUCKSPEED_MV            , '')AS MONEY), 1) , '.00', '')   AS       TRUCKSPEED_MV          --   대차속도 측정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(GRD_LOC                  , '')AS MONEY), 1) , '.00', '')   AS       GRD_LOC                --   그라인딩 위치
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(COOL_DOWN_HR_SETV        , '')AS MONEY), 1) , '.00', '')   AS       COOL_DOWN_HR_SETV      --   쿨다운 시간 설정값
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(COOL_DOWN_SPEED_SETV     , '')AS MONEY), 1) , '.00', '')   AS       COOL_DOWN_SPEED_SETV   --   쿨다운 속도 설정값 
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(BELT_SLIP_WARN_SETV      , '')AS MONEY), 1) , '.00', '')   AS       BELT_SLIP_WARN_SETV    --   벨트 슬립 경보 설정값 
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(CORN_GRD_3TRK1bs         , '')AS MONEY), 1) , '.00', '')   AS       CORN_GRD_3TRK1bs       --   코너 그라인드 3트랙1bs
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(CORN_GRD_3TRK2bs         , '')AS MONEY), 1) , '.00', '')   AS       CORN_GRD_3TRK2bs       --   코너 그라인드 3트랙2bs
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(CORN_GRD_3TRK3fs         , '')AS MONEY), 1) , '.00', '')   AS       CORN_GRD_3TRK3fs       --   코너 그라인드 3트랙3fs
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(CORN_GRD_5TRK1bs         , '')AS MONEY), 1) , '.00', '')   AS       CORN_GRD_5TRK1bs       --   코너 그라인드 5트랙1bs
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(CORN_GRD_5TRK2bs         , '')AS MONEY), 1) , '.00', '')   AS       CORN_GRD_5TRK2bs       --   코너 그라인드 5트랙2bs
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(CORN_GRD_5TRK3bs         , '')AS MONEY), 1) , '.00', '')   AS       CORN_GRD_5TRK3bs       --   코너 그라인드 5트랙3bs
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(CORN_GRD_5TRK4fs         , '')AS MONEY), 1) , '.00', '')   AS       CORN_GRD_5TRK4fs       --   코너 그라인드 5트랙4fs
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(CORN_GRD_5TRK5fs         , '')AS MONEY), 1) , '.00', '')   AS       CORN_GRD_5TRK5fs       --   코너 그라인드 5트랙5fs
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(OutsIDe_back             , '')AS MONEY), 1) , '.00', '')   AS       OutsIDe_back           --   Outside back
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(OutsIDe_front            , '')AS MONEY), 1) , '.00', '')   AS       OutsIDe_front          --   Outside front
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(WORK_SETV_Top            , '')AS MONEY), 1) , '.00', '')   AS       WORK_SETV_Top          --   작업 설정값 Top
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(WORK_SETV_Right          , '')AS MONEY), 1) , '.00', '')   AS       WORK_SETV_Right        --   작업 설정값 Right
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(WORK_SETV_Bottom         , '')AS MONEY), 1) , '.00', '')   AS       WORK_SETV_Bottom       --   작업 설정값 Bottom
                                                  , REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(WORK_SETV_Left           , '')AS MONEY), 1) , '.00', '')   AS       WORK_SETV_Left         --   작업 설정값 Left
                                               FROM TB_GRD_OPER_INFO
                                             ORDER BY WORK_DDTT DESC");

                olddtMain1 = cd.FindDataTable(sql);
                moddtMain1 = olddtMain1.Copy();

                Color normalColor = Color.LightGray;
                Color RunColor = Color.Lime;

                Color DrNormalColor = Color.Silver;
                Color DrRunColor = Color.Lime;
                


                if (moddtMain1.Rows.Count > 0)
                {
                    foreach (var item in vf.GetAllChildrens(this))
                    {
                        if (item.GetType().ToString() == "ComLib.UC_KeyValue")
                        {
                            var uc_keyValue = item as UC_KeyValue;

                            uc_keyValue.PLC_ITEM_VALUE = moddtMain1.Rows[0][uc_keyValue.PLC_CD].ToString();

                        }

                        if (item.GetType().ToString() == "ComLib.UC_KeyValueNoT")
                        {
                            var uc_keyValue = item as UC_KeyValueNoT;

                            uc_keyValue.PLC_ITEM_VALUE = moddtMain1.Rows[0][uc_keyValue.PLC_CD].ToString();

                        }

                    }

                    if (moddtMain1.Rows[0]["WORK_SETV_Top"].ToString().Equals("1"))
                    {
                        FaceT.BackColor = RunColor;
                    }
                    else
                    {
                        FaceT.BackColor = normalColor;
                    }

                    if (moddtMain1.Rows[0]["WORK_SETV_Right"].ToString().Equals("1"))
                    {
                        FaceR.BackColor = RunColor;
                    }
                    else
                    {
                        FaceR.BackColor = normalColor;
                    }

                    if (moddtMain1.Rows[0]["WORK_SETV_Bottom"].ToString().Equals("1"))
                    {
                        FaceB.BackColor = RunColor;
                    }
                    else
                    {
                        FaceB.BackColor = normalColor;
                    }

                    if (moddtMain1.Rows[0]["WORK_SETV_Left"].ToString().Equals("1"))
                    {
                        FaceL.BackColor = RunColor;
                    }
                    else
                    {
                        FaceL.BackColor = normalColor;
                    }

                    switch (moddtMain1.Rows[0]["GRD_LOC"].ToString())
                    {
                        case "1":
                            //STATE
                            this.pictureBox1.Image = global::BGsystemLibrary.Properties.Resources.GRD_State;

                            SetWidthHeightLocation(ShowMode.NoTurn);

                            break;
                        case "2":
                            //STATE1
                            this.pictureBox1.Image = global::BGsystemLibrary.Properties.Resources.GRD_State1;
                            SetWidthHeightLocation(ShowMode.Turn);
                            break;
                        case "3":
                            //STATE2
                            this.pictureBox1.Image = global::BGsystemLibrary.Properties.Resources.GRD_State2;
                            SetWidthHeightLocation(ShowMode.NoTurn);
                            break;
                        case "4":
                            //STATE3
                            this.pictureBox1.Image = global::BGsystemLibrary.Properties.Resources.GRD_State3;
                            SetWidthHeightLocation(ShowMode.Turn);
                            break;
               
                    }

                    switch (moddtMain1.Rows[0]["GRD_DRV_MODE"].ToString())
                    {
                        case "1":
                            btnFull.BackColor = DrRunColor;
                            btnFace.BackColor = DrNormalColor;
                            btnCorn.BackColor = DrNormalColor;
                            btnPar.BackColor = DrNormalColor;
                            break;
                        case "2":
                            btnFull.BackColor = DrNormalColor;
                            btnFace.BackColor = DrRunColor;
                            btnCorn.BackColor = DrNormalColor;
                            btnPar.BackColor = DrNormalColor;
                            break;
                        case "3":
                            btnFull.BackColor = DrNormalColor;
                            btnFace.BackColor = DrNormalColor;
                            btnCorn.BackColor = DrRunColor;
                            btnPar.BackColor = DrNormalColor;
                            break;
                        case "4":
                            btnFull.BackColor = DrNormalColor;
                            btnFace.BackColor = DrNormalColor;
                            btnCorn.BackColor = DrNormalColor;
                            btnPar.BackColor = DrRunColor;
                            break;
                    
                    }

                }

            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private void SetWidthHeightLocation(ShowMode mode)
        {
            switch (mode)
            {
                case ShowMode.NoTurn:
                    SetUCLocation(uC_KeyValueNoTHeight, rightPoint);
                    SetUCLocation(uC_KeyValueNoTWidth, bottomPoint);
                    break;
                case ShowMode.Turn:
                    SetUCLocation(uC_KeyValueNoTHeight, bottomPoint);
                    SetUCLocation(uC_KeyValueNoTWidth, rightPoint);
                    break;
            }
        }

        private void FaceL_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        public delegate void SetUCLocationDelegate(UC_KeyValueNoT uc, Point pt);
        public void SetUCLocation(UC_KeyValueNoT uc, Point pt)
        {
            if (this.InvokeRequired)
            {
                SetUCLocationDelegate r = new SetUCLocationDelegate(SetUCLocation);
                this.Invoke(r, new object[] {uc, pt });
            }
            else SetUCRELocation(uc, pt);
        }

        private void SetUCRELocation(UC_KeyValueNoT uc, Point pt)
        {
            uc.Location = pt;
        }
    }

}
