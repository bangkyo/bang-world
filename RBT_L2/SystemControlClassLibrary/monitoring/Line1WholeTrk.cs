using ComLib;
using ComLib.clsMgr;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OracleClient;
using WindowsFormsApplication15;
using SystemControlClassLibrary.UC;
using C1.Win.C1Input;
using System.Threading.Tasks;
using C1.Win.C1FlexGrid;
using SystemControlClassLibrary.Order;

namespace SystemControlClassLibrary.monitoring
{
    public partial class Line1WholeTrk : Form
    {

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();

        string ownerNM = "";
        string titleNM = "";
        string poc_no_nm = "";
        string length_no_nm = "";

        DataTable moddt = null;

        readonly string line_gp = "#1";

        private UC.UC_WorkShow uC_WorkShow1;

        private DataTable Acltable;
        private bool CanMODAcl;
        private bool CanZoneMoveAcl;
        private TextBox IsConnected = new TextBox();

        private Task t;
        private Stopwatch sw;
        private Stopwatch testsw;
        private bool CanShowZoneMoveAcl;

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public Line1WholeTrk(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void Line1WholeTrk_Load(object sender, System.EventArgs e)
        {
            #region button Setup
            btnInsertReg.Tag = "Line1CrtInRsltPopUP";
            btnZoneMove.Tag = "Line1InsRegPopup";
            btnPOCfin.Tag = "Line1POCFinReg";
            btnReWork.Tag = "Line1ProdReWorkMgmt";
            btnRsltCancel.Tag = "Line1WorkRsltCancel";
            btnZ03Fin.Tag = "Line1Z03Fin";

            //다중스레드무시
            CheckForIllegalCrossThreadCalls = false;

            Acltable = cd.GetScreeAcl(ck.UserGrp);

            CanMODAcl = Line3WholeTrk.SetModifyACL(Name, Acltable, "MOD_ACL");

            //btnZoneMove.Tag = "Line1InsRegPopup";

            //존 이동 POPUP의 저장가능 여부
            CanZoneMoveAcl = Line3WholeTrk.SetModifyACL(btnZoneMove.Tag.ToString(), Acltable, "MOD_ACL");

            //존 이동 UC 의 조회가능 여부
            //CanShowZoneMoveAcl = Line3WholeTrk.SetModifyACL(btnZoneMove.Tag.ToString(), Acltable, "INQ_ACL");

            #endregion

            ck.StrKey1 = "";

            //uc_zone setting
            UC_Zone_Setup();

            InitGrd();

            WorkControl();

            timer1.Interval = 5000;
            //timer1.Start();
            //timer1_Tick(null, EventArgs.Empty); // Simulate a timer tick event

            // connect indicater
            IsConnected.TextChanged += IsConnected_TextChanged;

            InitButton();
            sw = new Stopwatch();
            testsw = new Stopwatch();
            
            btnDisplay_Click(null, null);
            t = Task.Factory.StartNew(AsyncUpdateUIData);
        }

        private void AsyncUpdateUIData()
        {
            while (!IsDisposed)
            {
                // do something
                sw.Reset();
                sw.Start();
                UpdateUIData();
                //Text = DateTime.Now.ToString();
                //Console.WriteLine(DateTime.Now.ToString());
                sw.Stop();
                Console.WriteLine("Line1 update Elapsed:" + sw.Elapsed.TotalMilliseconds);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            }


            
        }

        private void UpdateUIData()
        {
            // 데이터를 뿌리기전에 디비 연결상태를 확인해서 연결되지않은 상태에서는 접속하지 않게 한다.
            if (!cd.CheckDbConnection())
            {
                IsConnected.Text = "DisConnected";
                this.Enable(false);
                //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + " 데이터베이스 접속이 해제되었습니다.");
                return;
            }

            this.Enable(true);
            IsConnected.Text = "Connected";


            //cd.CheckDbConnection();
            //현재의 근조 표시
            //SetWorkBinding(line_gp, uC_WorkShow1, this);

            Line3WholeTrk.SetWorkBinding(line_gp, uC_WorkShow1);

            SetDataBinding();

            SetDataBinding_Zone();

            this.Invalidate();
        }

        private void InitButton()
        {
            SetupButton(btnInsertReg, Acltable);
            SetupButton(btnZoneMove, Acltable);
            SetupButton(btnPOCfin, Acltable);
            SetupButton(btnReWork, Acltable);
            SetupButton(btnRsltCancel, Acltable);
            SetupButton(btnZ03Fin, Acltable);
        }

        private void SetupButton(Button btn, DataTable AclDT)
        {
            string sql = string.Empty;

            sql = string.Format("PAGE_ID = '{0}'", btn.Tag);

            DataRow[] result = AclDT.Select(sql);

            if (result.Length > 0)
            {
                foreach (DataRow row in result)
                {
                    //btn.Enabled = (row["INQ_ACL"].ToString() == "Y");
                    btn.Enabled = (row["REG_ACL"].ToString() == "Y");
                    //btn.Enabled = (row["MOD_ACL"].ToString() == "Y");
                    //btn.Enabled = (row["DEL_ACL"].ToString() == "Y");
                }
            }
            else btn.Enabled = false;
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

        private void WorkControl()
        {
            #region 유저컨트롤 설정

            uC_WorkShow1 = new SystemControlClassLibrary.UC.UC_WorkShow();

            uC_WorkShow1.BackColor = Color.FromArgb(0, 122, 204);

            uC_WorkShow1.Location = new System.Drawing.Point(1038, 155);
            uC_WorkShow1.Name = "uC_WorkShow1";
            uC_WorkShow1.Size = new System.Drawing.Size(197, 31);
            uC_WorkShow1.TabIndex = 163;
            if (CanMODAcl)
            {
                uC_WorkShow1.PopupPanelEvent += UC_WorkShow1_PopupPanelEvent;
            }

            panel1.Controls.Add(uC_WorkShow1);


            #endregion


            // 현재의 근조를 표시해준다.
            Line3WholeTrk.SetWorkBinding(line_gp, uC_WorkShow1);
        }

        private void UC_WorkShow1_PopupPanelEvent(object sender, EventArgs e)
        {
            Line3WholeTrk.ChangeWork(line_gp, uC_WorkShow1.WorkType, uC_WorkShow1.WorkTeam, this);
        }



        private void UC_Zone_Setup()
        {
            foreach (var item in vf.GetAllChildrens(this))
            {
                if (item.GetType().ToString() == "WindowsFormsApplication15.UC_Zone")
                {
                    var uc_zone = item as WindowsFormsApplication15.UC_Zone;

                    uc_zone.PopupEvent += PopupEvent;
                    

                }
            }
        }

        private void PopupEvent(object sender, EventArgs e)
        {
            ck.StrKey1 = line_gp;

            if (!string.IsNullOrEmpty(((Control)sender).Parent.Name))
            {
                //ck.StrKey2 = ((Control)sender).Parent.Name;

                var uc_zone = ((Control)sender).Parent as WindowsFormsApplication15.UC_Zone;
                ck.StrKey2 = uc_zone.ZoneCD;
            }

            Line3WholeTrk.OpenPopup(CanZoneMoveAcl);
        }


        private void InitGrd()
        {

            //grdMain1
            InitgrdMain1();

            //grdMain1_1
            InitgrdMain1_1();

            //grdMain1_2
            InitgrdMain1_2();

            //grdMain2
            InitgrdMain2();

            //grdMain3
            InitgrdMain3();

            //grdMain6
            InitgrdMain6();
            //grdMain7
            InitgrdMain7();

            //grdMain8
            InitgrdMain8();

            //grdMain9
            InitgrdMain9();

            //grdMain10
            InitgrdMain10();

            //grdMain12
            InitgrdMain12();
        }
        private void InitgrdMain1()
        {
            cs.InitGrid_search(grdMain1, true);

            //grdMain1.Rows.Frozen = 5;
            //grdMain1.Cols.Fixed = 1;

            //grdMain1[0, 0] = "상롤 GAP";
            grdMain1[1, 0] = "상롤 GAP";
            grdMain1[2, 0] = "상롤 ANGLE";
            grdMain1[3, 0] = "하롤 ANGLE";
            grdMain1[4, 0] = "전류(A)";

            //grdMain1[4, 0] = "상롤 GAP";
            grdMain1.Row = -1;
        }

        private void InitgrdMain1_1()
        {
            //cs.InitGrid_search(grdMain1_1, true);
            grdMain1_1[0, 0] = "쓰레드 속도";//"Thread Speed";

            grdMain1_1.Cols[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain1_1.Row = -1;
        }

        private void InitgrdMain1_2()
        {
            //cs.InitGrid_search(grdMain1_2, true);
            grdMain1_2[0, 0] = "머신 속도";//"Machine Speed";
            grdMain1_2.Cols[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain1_2.Row = -1;
        }
        private void InitgrdMain2()
        {
            cs.InitGrid_search(grdMain2, true);

            //grdMain2.Cols.Fixed = 1;

            grdMain2[1, 0] = "SERVO (서보)";
            grdMain2[2, 0] = "SCREW 롤 드라이버";// "SCREW Roll Drive";
            grdMain2[3, 0] = "롤 드라이브";//"Roll Drive";
            grdMain2[4, 0] = "출구 롤 테이블";//"EXIT Roller Table";

            //grdMain2.Cols[0].Width = 
            grdMain2.Rows[4].AllowMerging = true;

            grdMain2.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Custom;

            grdMain2.MergedRanges.Add(grdMain2.GetCellRange(4, 1, 4, 2));

            grdMain2.Row = -1;

        }
        private void InitgrdMain3()
        {
            cs.InitGrid_search(grdMain3, true);

            //grdMain3.Cols.Fixed = 1;

            //grdMain3[0, 0] = "비파괴";
            grdMain3[1, 0] = "MAT";
            grdMain3[2, 0] = "MLFT";
            grdMain3[3, 0] = "UT";

            grdMain3.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain3.Cols[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain3.Cols[2].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain3.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Custom;

            grdMain3.MergedRanges.Add(grdMain3.GetCellRange(1, 2, 3, 2));

            grdMain3.Cols[2].AllowMerging = true;
            grdMain3.Row = -1;
        }

        private void InitgrdMain6()
        {
            //InitGrid(grdMain6, true);
            cs.InitGrid_search(grdMain6, true);


            grdMain6.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain6.Cols[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain6.Cols[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;

            grdMain6.Row = -1;
        }

        private void InitgrdMain7()
        {

            cs.InitGrid_search(grdMain7, true, 1, 0);

            grdMain7[0, 0] = "압연번들번호"; grdMain7[0, 1] = "압연본수"; grdMain7[0, 2] = "교정본수";

            grdMain7.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain7.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain7.Cols["MILL_PCS"].TextAlign = cs.MILL_PCS_TextAlign;
            grdMain7.Cols["STR_PCS"].TextAlign = cs.STR_PCS_TextAlign;
        }

        private void InitgrdMain8()
        {
            cs.InitGrid_search(grdMain8, true);

            grdMain8.Cols["STEEL_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain8.Cols["HEAT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain8.Cols["ITEM_SIZE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain8.Cols["LENGTH"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain8.Cols["SURFACE_LEVEL"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain8.Cols["POC_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain8.Row = -1;
        }

        private void InitgrdMain12()
        {
            cs.InitGrid_search(grdMain12, true, 1, 0);

            grdMain12[0, 0] = "POC_NO"; grdMain12[0, 1] = "압연번들"; grdMain12[0, 2] = "교정번들";

            grdMain12.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain12.Cols["POC"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain12.Cols["QTY"].TextAlign = cs.WGT_TextAlign;
            grdMain12.Cols["CO"].TextAlign = cs.WGT_TextAlign;

            //grdMain7.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;

            //grdMain7.Rows[0].AllowMerging = true;

        }

        private void InitgrdMain9()
        {
            cs.InitGrid_search(grdMain9, true, 2, 1);

            grdMain9[1, 0] = grdMain9[0, 0] = "POC";
            grdMain9[1, 1] = grdMain9[0, 1] = " 검사\n본수";
            grdMain9[1, 2] = grdMain9[0, 2] = " OK\n본수";
            grdMain9[1, 3] = grdMain9[0, 3] = " NG\n본수";
            //grdMain9[1, 4] = grdMain9[0, 4] = " 쇼트\n본수";
            grdMain9[0, 4] = "MAT";
            grdMain9[1, 4] = "NG";

            grdMain9[0, 5] = "MLFT";
            grdMain9[1, 5] = "NG";

            grdMain9[0, 6] = "UT";
            grdMain9[1, 6] = "NG";

            //grdMain9[0, 8] =  grdMain9[1, 8] = " NG\n본수";


            grdMain9.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain9.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain9.Cols["POC_NO"].TextAlign = cs.MILL_NO_TextAlign;
            //grdMain9.Cols["MILL_PCS"].TextAlign = cs.MILL_PCS_TextAlign;
            grdMain9.Cols["MAT_NG_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain9.Cols["MLFT_NG_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain9.Cols["UT_NG_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain9.Cols["NDT_NG_PCS"].TextAlign = cs.PCS_TextAlign;


            grdMain9.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            //c1FlexGrid8.Rows[1].AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            //c1FlexGrid8.Rows[0].AllowMerging = c1FlexGrid8.Rows[1].AllowMerging = true;

            for (int i = 0; i < grdMain9.Cols.Count; i++)
            {
                grdMain9.Cols[i].AllowMerging = true;

            }

            grdMain9.Rows[0].AllowMerging = true;

            grdMain9.Row = -1;
        }

        private void InitgrdMain10()
        {
            cs.InitGrid_search(grdMain10, true);

            grdMain10.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain10.Cols["BUNDLE_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain10.Cols["PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain10.Cols["THEORY_WGT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain10.Cols["NET_WGT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;

            grdMain10.Row = -1;
        }

        private void timer1_Tick(object p, EventArgs empty)
        {
            //InitGrd();

            
        }



        private void SetDataBinding_Zone()
        {

            string sql1 = string.Empty;
            sql1 = cd.GetZoneDatasql();

            //moddt = new DataTable();

            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1, parm);

            if (moddt != null)
            {
                foreach (var item in vf.GetAllChildrens(this))
                {
                    if (item.GetType().ToString() == "WindowsFormsApplication15.UC_Zone")
                    {
                        var uc_zone = item as WindowsFormsApplication15.UC_Zone;
                        //zone 초기화
                        uc_zone.PCS = "";
                        uc_zone.MillNo = "";

                        foreach (DataRow row in moddt.Rows)
                        {
                            if (uc_zone.ZoneCD == row["ZONE_CD"].ToString())
                            {
                                if (uc_zone.ZoneCD == "1Z24")
                                {
                                    uc_zone.PCS = row["BUNDLE_PCS"].ToString();
                                    uc_zone.MillNo = row["BUNDLE_NO"].ToString();
                                }
                                else
                                {
                                    uc_zone.PCS = row["PCS"].ToString();
                                    uc_zone.MillNo = row["MILL_NO"].ToString();
                                }
                            }

                        }
                    }
                }
            }
            
        }

        private void SetDataBinding()
        {
            testsw.Start();
            //교정 + 쓰레드 속도 + 스피드 속도
            SetDataBinding_grdMain1();

            testsw.Stop();
            Console.WriteLine("test1:" + testsw.ElapsedMilliseconds);

            testsw.Reset();
            testsw.Start();

            //면취
            SetDataBinding_grdMain2();

            testsw.Stop();
            //Console.WriteLine("test2:" + testsw.ElapsedMilliseconds);
            testsw.Reset();
            testsw.Start();

            SetDataBinding_grdMain3();

            testsw.Stop();
            //Console.WriteLine("test3:" + testsw.ElapsedMilliseconds);
            testsw.Reset();
            testsw.Start();

            SetDataBinding_grdMain6();

            testsw.Stop();
            //Console.WriteLine("test6:" + testsw.ElapsedMilliseconds);
            testsw.Reset();
            testsw.Start();

            // grdMain7
            SetDataBinding_grdMain7();

            testsw.Stop();
           // Console.WriteLine("test7:" + testsw.ElapsedMilliseconds);
            testsw.Reset();
            testsw.Start();

            //grdMain8
            SetDataBinding_grdMain8();

            testsw.Stop();
            //Console.WriteLine("test8:" + testsw.ElapsedMilliseconds);
            testsw.Reset();
            testsw.Start();

            //grdMain9
            SetDataBinding_grdMain9();

            testsw.Stop();
            //Console.WriteLine("test9:" + testsw.ElapsedMilliseconds);
            testsw.Reset();
            testsw.Start();

            //각 위치에 데이터 입력한다. 

            SetDataBinding_grdMain10();
            testsw.Stop();
            //Console.WriteLine("test10:" + testsw.ElapsedMilliseconds);

            testsw.Start();

            //각 위치에 데이터 입력한다. 

            SetDataBinding_grdMain12();
            testsw.Stop();
            //Console.WriteLine("test10:" + testsw.ElapsedMilliseconds);
        }

        private void SetDataBinding_grdMain1()
        {
            string sql1 = string.Empty;
            sql1 += string.Format("SELECT A.TOP_ROLL_GAP_1   ");
            sql1 += string.Format("      ,A.TOP_ROLL_GAP_2   ");
            sql1 += string.Format("      ,A.TOP_ROLL_GAP_3   ");
            sql1 += string.Format("      ,A.TOP_ROLL_GAP_4   ");
            sql1 += string.Format("      ,A.TOP_ROLL_GAP_5   ");
            sql1 += string.Format("      ,A.TOP_ROLL_GAP_6   ");
            sql1 += string.Format("      ,A.TOP_ROLL_ANGLE_1 ");
            sql1 += string.Format("      ,A.TOP_ROLL_ANGLE_2 ");
            sql1 += string.Format("      ,A.TOP_ROLL_ANGLE_3 ");
            sql1 += string.Format("      ,A.TOP_ROLL_ANGLE_4 ");
            sql1 += string.Format("      ,A.TOP_ROLL_ANGLE_5 ");
            sql1 += string.Format("      ,A.TOP_ROLL_ANGLE_6 ");
            sql1 += string.Format("      ,A.BOT_ROLL_ANGLE_1 ");
            sql1 += string.Format("      ,A.BOT_ROLL_ANGLE_2 ");
            sql1 += string.Format("      ,A.BOT_ROLL_ANGLE_3 ");
            sql1 += string.Format("      ,A.INLET_ROLL_CURRENT ");
            sql1 += string.Format("      ,A.MID_ROLL_CURRENT ");
            sql1 += string.Format("      ,A.OUTLET_ROLL_CURRENT ");
            sql1 += string.Format("      ,A.THREAD_SPEED     ");
            sql1 += string.Format("      ,A.MACHINE_SPEED    ");
            sql1 += string.Format("FROM TB_STR_OPERINFO_NO1 A ");
            sql1 += string.Format("WHERE  WORK_DDTT = (SELECT MAX(WORK_DDTT) FROM   TB_STR_OPERINFO_NO1) ");




            moddt = cd.FindDataTable(sql1);
            //Cursor = Cursors.AppStarting;
            if (moddt != null && moddt.Rows.Count > 0)
            {
                grdMain1[1, 1]   = moddt.Rows[0]["TOP_ROLL_GAP_1"  ];
                grdMain1[1, 2]   = moddt.Rows[0]["TOP_ROLL_GAP_2"  ];
                grdMain1[1, 3]   = moddt.Rows[0]["TOP_ROLL_GAP_3"  ];
                grdMain1[1, 4]   = moddt.Rows[0]["TOP_ROLL_GAP_4"  ];
                grdMain1[1, 5]   = moddt.Rows[0]["TOP_ROLL_GAP_5"  ];
                grdMain1[1, 6]   = moddt.Rows[0]["TOP_ROLL_GAP_6"  ];
                grdMain1[2, 1]   = moddt.Rows[0]["TOP_ROLL_ANGLE_1"];
                grdMain1[2, 2]   = moddt.Rows[0]["TOP_ROLL_ANGLE_2"];
                grdMain1[2, 3]   = moddt.Rows[0]["TOP_ROLL_ANGLE_3"];
                grdMain1[2, 4]   = moddt.Rows[0]["TOP_ROLL_ANGLE_4"];
                grdMain1[2, 5]   = moddt.Rows[0]["TOP_ROLL_ANGLE_5"];
                grdMain1[2, 6]   = moddt.Rows[0]["TOP_ROLL_ANGLE_6"];
                grdMain1[3, 1]   = moddt.Rows[0]["BOT_ROLL_ANGLE_1"];
                grdMain1[3, 2]   = moddt.Rows[0]["BOT_ROLL_ANGLE_2"];
                grdMain1[3, 3]   = moddt.Rows[0]["BOT_ROLL_ANGLE_3"];
                grdMain1[4, 1]   = moddt.Rows[0]["INLET_ROLL_CURRENT"];
                grdMain1[4, 2]   = moddt.Rows[0]["MID_ROLL_CURRENT"];
                grdMain1[4, 3]   = moddt.Rows[0]["OUTLET_ROLL_CURRENT"];

                grdMain1_1[0, 1] = moddt.Rows[0]["THREAD_SPEED"    ];
                grdMain1_2[0, 1] = moddt.Rows[0]["MACHINE_SPEED"   ];
            }
            else
            {
                grdMain1[1, 1]   = string.Empty;
                grdMain1[1, 2]   = string.Empty;
                grdMain1[1, 3]   = string.Empty;
                grdMain1[1, 4]   = string.Empty;
                grdMain1[1, 5]   = string.Empty;
                grdMain1[1, 6]   = string.Empty;
                grdMain1[2, 1]   = string.Empty;
                grdMain1[2, 2]   = string.Empty;
                grdMain1[2, 3]   = string.Empty;
                grdMain1[2, 4]   = string.Empty;
                grdMain1[2, 5]   = string.Empty;
                grdMain1[2, 6]   = string.Empty;
                grdMain1[3, 1]   = string.Empty;
                grdMain1[3, 2]   = string.Empty;
                grdMain1[3, 3]   = string.Empty;
                grdMain1[4, 1] = string.Empty;
                grdMain1[4, 2] = string.Empty;
                grdMain1[4, 3] = string.Empty;
                grdMain1_1[0, 1] = string.Empty;
                grdMain1_2[0, 1] = string.Empty;
            }
            //Cursor = Cursors.Default;
            SetGridRowSelect(grdMain1, -1);
            SetGridRowSelect(grdMain1_1, -1);
            SetGridRowSelect(grdMain1_2, -1);

        }
        private void SetDataBinding_grdMain2()
        {
            string sql1 = string.Empty;
            sql1 += string.Format(" SELECT A.SERVO_ACTV_1 ");
            sql1 += string.Format("       ,A.SERVO_ACTV_2 ");
            sql1 += string.Format("       ,A.SCREW_ACTV_1 ");
            sql1 += string.Format("       ,A.SCREW_ACTV_2 ");
            sql1 += string.Format("       ,A.RT_ACTV_1    ");
            sql1 += string.Format("       ,A.RT_ACTV_2    ");
            sql1 += string.Format("       ,A.RT_ACTV_EXIT ");
            sql1 += string.Format(" FROM   TB_CHF_OPERINFO_NO1 A ");
            sql1 += string.Format(" WHERE  WORK_DDTT = (SELECT MAX(WORK_DDTT) FROM   TB_CHF_OPERINFO_NO1) ");


            moddt = cd.FindDataTable(sql1);

            //moddt = cd.FindDataTable(sql1);
            if (moddt != null && moddt.Rows.Count > 0)
            {
                //this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain2[1, 1] = moddt.Rows[0]["SERVO_ACTV_1"];
                grdMain2[1, 2] = moddt.Rows[0]["SERVO_ACTV_2"];
                grdMain2[2, 1] = moddt.Rows[0]["SCREW_ACTV_1"];
                grdMain2[2, 2] = moddt.Rows[0]["SCREW_ACTV_2"];
                grdMain2[3, 1] = moddt.Rows[0]["RT_ACTV_1"];
                grdMain2[3, 2] = moddt.Rows[0]["RT_ACTV_2"];
                grdMain2[4, 1] = grdMain2[4, 2] = moddt.Rows[0]["RT_ACTV_EXIT"];
                //this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            else
            {
                grdMain2[1, 1] = string.Empty;
                grdMain2[1, 2] = string.Empty;
                grdMain2[2, 1] = string.Empty;
                grdMain2[2, 2] = string.Empty;
                grdMain2[3, 1] = string.Empty;
                grdMain2[3, 2] = string.Empty;
                grdMain2[4, 1] = grdMain2[4, 2] = string.Empty;
            }
            SetGridRowSelect(grdMain2, -1);


        }
        private void SetDataBinding_grdMain3()
        {

            string sql1 = string.Empty;
            sql1 += string.Format("SELECT X.MILL_NO AS MILL_NO ");
            sql1 += string.Format("      ,X.PIECE_NO AS PIECE_NO ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.PRII_GOOD_NG) AS PRII_GOOD_NG ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.MAT_GOOD_NG) AS MAT_GOOD_NG ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.MLFT_GOOD_NG) AS MLFT_GOOD_NG ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.UT_GOOD_NG) AS UT_GOOD_NG ");
            sql1 += string.Format("FROM ( ");
            sql1 += string.Format("      SELECT /*+RULE */ MILL_NO ");
            sql1 += string.Format("            ,PIECE_NO ");
            sql1 += string.Format("            ,'OK' AS PRII_GOOD_NG ");
            sql1 += string.Format("            ,MAT_GOOD_NG ");
            sql1 += string.Format("            ,MLFT_GOOD_NG ");
            sql1 += string.Format("            ,UT_GOOD_NG ");
            sql1 += string.Format("      FROM   TB_CR_PIECE_WR A ");
            sql1 += string.Format("      WHERE  LINE_GP     = :P_LINE_GP ");
            sql1 += string.Format("      AND    ROUTING_CD  = 'F2' "); // NDT
            sql1 += string.Format("      AND A.POC_NO = (SELECT POC_NO FROM TB_PROG_POC_MGMT WHERE LINE_GP = :P_LINE_GP ) "); // NDT
            sql1 += string.Format("      ORDER BY EXIT_DDTT DESC ");
            sql1 += string.Format("      ) X ");
            sql1 += string.Format("      WHERE     ROWNUM <= 1 ");




            //sql1 += string.Format("SELECT X.MILL_NO AS MILL_NO ");
            //sql1 += string.Format("      ,X.PIECE_NO AS PIECE_NO ");
            //sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.MAT_GOOD_NG) AS MAT_GOOD_NG ");
            //sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.MLFT_GOOD_NG) AS MLFT_GOOD_NG ");
            //sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.UT_GOOD_NG) AS UT_GOOD_NG ");
            //sql1 += string.Format("FROM ( ");
            //sql1 += string.Format("SELECT /*+RULE */ MILL_NO ");
            //sql1 += string.Format("      ,PIECE_NO ");
            //sql1 += string.Format("      ,MAT_GOOD_NG ");
            //sql1 += string.Format("      ,MLFT_GOOD_NG ");
            //sql1 += string.Format("      ,UT_GOOD_NG ");
            //sql1 += string.Format("FROM   TB_CR_PIECE_WR A ");
            //sql1 += string.Format("WHERE  LINE_GP     = :P_LINE_GP ");
            //sql1 += string.Format("AND    ROUTING_CD  = 'F2' ");
            //sql1 += string.Format("AND    EXIT_DDTT   = (SELECT MAX(EXIT_DDTT) FROM TB_CR_PIECE_WR ");
            //sql1 += string.Format("                      WHERE  LINE_GP    = A.LINE_GP ");
            //sql1 += string.Format("                      AND    ROUTING_CD = A.ROUTING_CD) ");
            //sql1 += string.Format("AND    ROWNUM      = 1 ");
            //sql1 += string.Format(") X ");

            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1, parm);


            if (moddt != null && moddt.Rows.Count > 0)
            {
                //this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain3[1, 1] = moddt.Rows[0]["MAT_GOOD_NG"];
                grdMain3[2, 1] = moddt.Rows[0]["MLFT_GOOD_NG"];
                grdMain3[3, 1] = moddt.Rows[0]["UT_GOOD_NG"];
                grdMain3[1, 2] = grdMain3[2, 2] = grdMain3[3, 2] = moddt.Rows[0]["PIECE_NO"];
                //this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            else
            {
                grdMain3[1, 1] = string.Empty;
                grdMain3[2, 1] = string.Empty;
                grdMain3[3, 1] = string.Empty;
                grdMain3[1, 2] = grdMain3[2, 2] = grdMain3[3, 2] = string.Empty;
            }
            SetGridRowSelect(grdMain3, -1);
        }
        private void SetDataBinding_grdMain6()
        {
            string sql1 = string.Empty;
            sql1 += string.Format("SELECT BUNDLE_NO ");
            sql1 += string.Format("      ,PCS ");
            sql1 += string.Format("FROM   TB_BND_WR  A ");
            sql1 += string.Format("WHERE  LINE_GP     = '{0}' ", line_gp);
            sql1 += string.Format("AND    REG_DDTT   = (SELECT MAX(REG_DDTT)  FROM TB_BND_WR ");
            sql1 += string.Format("                     WHERE  LINE_GP   = A.LINE_GP ");
            sql1 += string.Format("                     AND    NVL(DEL_YN,'N') <> 'Y') ");
            sql1 += string.Format("AND    NVL(A.DEL_YN,'N') <> 'Y' ");
            sql1 += string.Format("AND    ROWNUM      = 1 ");

            moddt = cd.FindDataTable(sql1);
            //this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            if (moddt != null)
            {
                try
                {
                    grdMain6.SetDataBinding(moddt, null, true);
                }
                catch (Exception)
                {

                    return;
                }
                
            }
            SetGridRowSelect(grdMain6, -1);
        }

        private void SetDataBinding_grdMain7()
        {

            string sql1 = string.Empty;

            sql1 = cd.GetReadyBundlesql();

            //moddt = new DataTable();
            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1, parm);

            if (moddt != null)
            {
                try
                {
                    grdMain7.SetDataBinding(moddt, null, true);
                }
                catch (Exception)
                {

                    return;
                }

                
            }
            SetGridRowSelect(grdMain7, -1);
        }

        private void SetDataBinding_grdMain8()
        {

            string sql1 = string.Empty;
            //--실시간트레킹중간_POC정보 공통쿼리를 가져온다
            sql1 = cd.GetPOCsql_12();

            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1, parm);

            if (moddt != null)
            {
                try
                {
                    grdMain8.SetDataBinding(moddt, null, true);
                    poc_no_nm = moddt.Rows[0]["POC_NO"].ToString();
                    length_no_nm = moddt.Rows[0]["LENGTH"].ToString();
                    double length = Convert.ToDouble(length_no_nm);
                    if (moddt.Rows[0]["STEEL_NM"].ToString() == "SAE51B35")
                    {
                        if (length <= 6.1)
                        {
                            label5.Text = "현재 POC(" + poc_no_nm + ")는 단척제품입니다 참고바랍니다.";
                            label5.Visible = true;
                        }
                        else
                        {
                            label5.Visible = false;
                        }
                        //label5.Text = "현재 POC(" + poc_no_nm + ") ND(SAE51B35) 강종은 난단척 주의 강종입니다";
                        //label5.Visible = true;
                    }
                    else
                    {
                        label5.Visible = false;
                    }

                   
                }
                catch (Exception)
                {
                    label5.Visible = false;
                    return;
                }
                //grdMain8.SetDataBinding(moddt, null, true);
            }
            SetGridRowSelect(grdMain8, -1);

        }

        private void SetDataBinding_grdMain9()
        {
            string sql1 = string.Empty;
            sql1 += string.Format(" SELECT /*+RULE */  A.POC_NO, B.SEQ_NO ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',1,0)),0)  AS  NDT_INSP_PCS    ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',DECODE(GOOD_YN,'OK',1,0),0)),0)  AS  NDT_OK_PCS ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',DECODE(A.MAT_GOOD_NG,'NG',1,0)) ), 0) AS MAT_NG_PCS ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',DECODE(A.MLFT_GOOD_NG,'NG',DECODE(A.MAT_GOOD_NG,'NG',0,DECODE(A.UT_GOOD_NG,'NG',0,1)),0)) ), 0)  AS MLFT_NG_PCS ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',DECODE(A.UT_GOOD_NG,'NG',DECODE(A.MAT_GOOD_NG,'NG',0,1),0))  ), 0) AS UT_NG_PCS ");
            sql1 += string.Format("       ,NVL(SUM(CASE WHEN A.MAT_GOOD_NG = 'NG' OR A.MLFT_GOOD_NG = 'NG' OR A.UT_GOOD_NG = 'NG' THEN 1 ");
            sql1 += string.Format("             ELSE 0  END), 0) AS NDT_NG_PCS ");
            sql1 += string.Format(" FROM   TB_CR_PIECE_WR A ");
            sql1 += string.Format("       ,(SELECT POC_NO AS POC_NO ,'1' AS SEQ_NO ");
            sql1 += string.Format("         FROM   TB_PROG_POC_MGMT ");
            sql1 += string.Format("         WHERE  LINE_GP     =  :P_LINE_GP ");
            sql1 += string.Format("         UNION ");
            sql1 += string.Format("         SELECT BEF_POC_NO AS POC_NO ,'2' AS SEQ_NO ");
            sql1 += string.Format("         FROM   TB_PROG_POC_MGMT  ");
            sql1 += string.Format("         WHERE  LINE_GP     =  :P_LINE_GP ) B ");
            sql1 += string.Format(" WHERE  A.LINE_GP     =  :P_LINE_GP ");
            sql1 += string.Format(" AND    A.POC_NO      = B.POC_NO ");
            //sql1 += string.Format(" AND    A.POC_SEQ      = B.POC_SEQ ");
            sql1 += string.Format(" AND    A.REWORK_SEQ  = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            sql1 += string.Format("                         WHERE  MILL_NO    = A.MILL_NO ");
            sql1 += string.Format("                         AND    PIECE_NO   = A.PIECE_NO ");
            sql1 += string.Format("                         AND    LINE_GP    = A.LINE_GP ");
            sql1 += string.Format("                         AND    ROUTING_CD = A.ROUTING_CD) ");
            sql1 += string.Format(" AND    A.ROUTING_CD  IN ('F2') ");
            sql1 += string.Format(" GROUP BY A.POC_NO, B.SEQ_NO ");
            sql1 += string.Format(" ORDER BY SEQ_NO ");
            //moddt = new DataTable();
            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;
            

            moddt = cd.FindDataTable(sql1, parm);


            if (moddt != null)
            {
                try
                {
                    grdMain9.SetDataBinding(moddt, null, true);
                }
                catch (Exception)
                {

                    return;
                }
                //grdMain9.SetDataBinding(moddt, null, true);

            }
            SetGridRowSelect(grdMain9, -1);
        }

        private void SetDataBinding_grdMain12()
        {

            string sql1 = string.Empty;
            sql1 += string.Format(" SELECT POC, QTY, COUNT(*) CO ");
            sql1 += string.Format(" FROM( ");
            sql1 += string.Format("         select A.POC_NO POC, F.BUNDLE_QTY QTY, A.MILL_NO, COUNT(*) STR_BON, C.ORD_PCS, C.ORD_PCS - (COUNT(*)) SETT ");
            sql1 += string.Format("               FROM TB_CR_PIECE_WR A, TB_CR_INPUT_WR B, TB_CR_ORD_BUNDLEINFO C, TB_CR_ORD F ");
            sql1 += string.Format("               WHERE A.POC_NO = B.POC_NO ");
            sql1 += string.Format("               AND B.POC_NO = C.POC_NO ");
            sql1 += string.Format("               AND A.POC_NO = F.POC_NO ");
            sql1 += string.Format("               AND A.MILL_NO = B.MILL_NO ");
            sql1 += string.Format("               AND B.MILL_NO = C.MILL_NO ");
            sql1 += string.Format("               AND A.POC_NO = (select POC_NO from TB_PROG_POC_MGMT WHERE LINE_GP = :P_LINE_GP) ");
            //sql1 += string.Format("               AND A.POC_NO = (SELECT MAX(POC_NO) FROM TB_RL_TM_TRACKING WHERE ZONE_CD = '3Z01' AND LINE_GP =  '#3' AND PROG_STAT = 'WAT') ");
            sql1 += string.Format("               AND A.ROUTING_CD = 'A1' ");
            sql1 += string.Format("               AND A.LINE_GP = :P_LINE_GP ");
            sql1 += string.Format("               GROUP BY A.POC_NO, A.MILL_NO, C.ORD_PCS, F.BUNDLE_QTY ");
            sql1 += string.Format("  ) D ");
            sql1 += string.Format(" WHERE SETT <= 0 ");
            sql1 += string.Format(" GROUP BY POC, QTY ");


            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;
            moddt = cd.FindDataTable(sql1, parm);

            if (moddt != null)
            {
                try
                {
                    grdMain12.SetDataBinding(moddt, null, true);
                }
                catch (Exception)
                {

                    return;
                }
                grdMain12.Invalidate();
            }
            //grdMain7.Row = -1;
            SetGridRowSelect(grdMain12, -1);

        }

        private void SetDataBinding_grdMain10()
        {

            string sql1 = string.Empty;
            sql1 += string.Format("SELECT X.* ");
            sql1 += string.Format("FROM  ( ");
            sql1 += string.Format("    SELECT A.POC_NO ");
            sql1 += string.Format("          ,A.BUNDLE_NO ");
            sql1 += string.Format("          ,A.PCS ");
            sql1 += string.Format("          ,A.THEORY_WGT ");
            sql1 += string.Format("          ,A.NET_WGT ");
            sql1 += string.Format("          ,A.REG_DDTT ");
            sql1 += string.Format("    FROM   TB_BND_WR A ");
            sql1 += string.Format("    WHERE  A.MFG_DATE > TO_CHAR(SYSDATE-1,'YYYYMMDD') ");
            sql1 += string.Format("    AND    A.LINE_GP  =  :P_LINE_GP ");
            sql1 += string.Format("    AND    A.POC_NO = ( SELECT /*+RULE */ DISTINCT POC_NO AS POC_NO ");
            sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
            sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT') ");
            sql1 += string.Format("                        AND    LINE_GP     =  :P_LINE_GP ");
            sql1 += string.Format("                        AND    ROWNUM      = 1 ) ");
            sql1 += string.Format("    AND    NVL(A.DEL_YN,'N') <> 'Y' ");
            sql1 += string.Format("    ORDER BY REG_DDTT DESC ");
            sql1 += string.Format(") X ");
            sql1 += string.Format("WHERE ROWNUM < 11 ");


            //moddt = new DataTable();
            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1, parm);
            if (moddt != null)
            {
                try
                {
                    grdMain10.SetDataBinding(moddt, null, true);
                }
                catch (Exception)
                {

                    return;
                }
                //grdMain10.SetDataBinding(moddt, null, true);
            }
            SetGridRowSelect(grdMain10, -1);
        }

        delegate void SetGridRowSelectCallback(C1FlexGrid grid, int selectrow);

        private void SetGridRowSelect(C1FlexGrid grid, int selectrow)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (grid.InvokeRequired)
            {
                SetGridRowSelectCallback d = new SetGridRowSelectCallback(SetGridRowSelect);
                grid.Invoke(d, new object[] { grid, selectrow });
            }
            else
            {
                grid.Row = selectrow;
            }
        }

        private void btnInsertReg_Click(object sender, EventArgs e)
        {

            Line3WholeTrk.InSertCRT(line_gp);
            ////CrtInRsltInqPopUP popup = new CrtInRsltInqPopUP(this as MyInterface);
            //CrtInRsltPopUP popup = new CrtInRsltPopUP(line_gp);
            //popup.StartPosition = FormStartPosition.CenterScreen;
            //popup.Show();
        }

        private void btnZoneMove_Click(object sender, EventArgs e)
        {
            ck.StrKey1 = line_gp;

            ck.StrKey2 = "";

            //if (((Control)sender).Parent =="")

            Line3WholeTrk.OpenPopup();
        }

        private void btnPOCFin_Click(object sender, EventArgs e)
        {

            Line3WholeTrk.POCFinREG(line_gp);
        }

        private void orientedTextLabel5_Click(object sender, EventArgs e)
        {
            //temp testpopup = new temp();
            //testpopup.Show();
        }

        private void btnReWork_Click(object sender, EventArgs e)
        {
            Line3WholeTrk.ReWorkMgmt(line_gp);
        }

        private void btnRsltCancel_Click(object sender, EventArgs e)
        {
            // 현재 grdMain8의 행이 있다면, 첫행의 poc no를 가져와 초기검색에 사용
            string poc_No = string.Empty;

            if (grdMain8.Rows.Count > 1)
            {
                poc_No = grdMain8.GetData(1, "POC_NO").ToString().Trim();
            }

            Line3WholeTrk.RsltCancel(line_gp, poc_No);
        }

        private void grdMain8_DoubleClick(object sender, EventArgs e)
        {
            if (grdMain8.Row < 1)
            {
                return;
            }

            //선택된 행의 POC 와 POC_SEQ 데이터를 전달해서 해당정보를 조회하는 창 OPEN
            string poc_No = grdMain8.GetData(grdMain8.Row, "POC_NO").ToString().Trim();
            //string poc_seq_No = grdMain8.GetData(grdMain8.Row, "POC_SEQ").ToString().Trim();
            //string poc_seq_No = "1";
            string sql1 = string.Empty;
            sql1 += string.Format("SELECT POC_SEQ ");
            sql1 += string.Format("FROM TB_PROG_POC_MGMT ");
            sql1 += string.Format("WHERE LINE_GP = :P_LINE_GP");

            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1, parm);
            string poc_seq_No = moddt.Rows[0]["POC_SEQ"].ToString();


            CrtInBundleInfo popup = new CrtInBundleInfo(poc_No, poc_seq_No);
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.MinimizeBox = false;
            popup.MaximizeBox = false;
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {

            cd.InsertLogForSearch(ck.UserID, btnDisplay);
            //timer1_Tick(null, null);
            UpdateUIData();
        }

        private void btnZ03Fin_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(" Z03 이후 트레킹이 종료됩니다 진행하시겠습니까? ", Text, MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            if (CallZ03FinSP(line_gp, ck.UserID))
            {
                btnDisplay_Click(null, null);
            }
        }
        /// <summary>
        /// Z14이후의 트래킹을 종료
        /// </summary>
        /// <param name="_line_gp"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        private bool CallZ03FinSP(string _line_gp, string _userID)
        {

            bool success = false;

            OracleConnection conn = cd.OConnect();

            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;


            string result_stat = string.Empty;
            string result_msg = string.Empty;

            string spName = "SP_NO1_END_PROC";
            OracleParameter op;
            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;


                #region "SP_NO1_END_PROC" sp를 실행
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear();
                op = new OracleParameter("P_LINE_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = _line_gp;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_USER_ID", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = _userID;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_STAT", OracleType.VarChar);
                op.Direction = ParameterDirection.Output;
                op.Size = 4000;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_MSG", OracleType.VarChar);
                op.Direction = ParameterDirection.Output;
                op.Size = 4000;
                cmd.Parameters.Add(op);

                cmd.ExecuteNonQuery();

                result_stat = Convert.ToString(cmd.Parameters["P_PROC_STAT"].Value);
                result_msg = Convert.ToString(cmd.Parameters["P_PROC_MSG"].Value);
                #endregion

                //실행후 성공
                transaction.Commit();

                if (result_stat == "ERR")
                {
                    MessageBox.Show(result_msg);
                    success = false;
                }

                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), result_msg);
                success = true;
            }
            catch (Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
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

            return success;

        }
    }
}
