﻿using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication15;
using SystemControlClassLibrary.information;
using SystemControlClassLibrary.Order;

namespace SystemControlClassLibrary.monitoring
{
    public partial class Line3WholeTrk : Form
    {

        protected ConnectDB cd = new ConnectDB();
        protected VbFunc vf = new VbFunc();
        protected clsStyle cs = new clsStyle();
        protected clsCom ck = new clsCom();

        protected DataTable moddt = null;
        protected DataTable Acltable;

        protected bool CanMODAcl;
        protected bool CanCrtInAcl;

        protected bool CanShowZoneMoveAcl;
        protected bool CanZoneMoveAcl;

        protected readonly string line_gp = "#3";

        protected UC.UC_WorkShow uC_WorkShow1;

        protected TextBox IsConnected = new TextBox();

        protected Task t;
        protected Stopwatch sw;

        #region main form load
        public Line3WholeTrk(string titleNm, string scrAuth, string factCode, string ownerNm)
        {   
            InitializeComponent();
        }

        protected virtual void Line3WholeTrk_Load(object sender, EventArgs e)
        {
            #region button Setup
            btnInsertReg.Tag = "Line3CrtInRsltPopUP";
            btnZoneMove.Tag = "Line3InsRegPopup";
            btnPOCfin.Tag = "Line3POCFinReg";
            btnReWork.Tag = "Line3ProdReWorkMgmt";
            btnRsltCancel.Tag = "Line3WorkRsltCancel";
            btnZ14Fin.Tag = "Line3Z14Fin";

            //다중스레드무시
            CheckForIllegalCrossThreadCalls = false;


            Acltable = cd.GetScreeAcl(ck.UserGrp);

            CanMODAcl = SetModifyACL(Name, Acltable, "MOD_ACL");

            //btnZoneMove.Tag = "Line1InsRegPopup";

            //존 이동 POPUP의 저장가능 여부
            CanZoneMoveAcl = SetModifyACL(btnZoneMove.Tag.ToString(), Acltable, "MOD_ACL");

            //존 이동 UC 의 조회가능 여부
            CanShowZoneMoveAcl = SetModifyACL(btnZoneMove.Tag.ToString(), Acltable, "INQ_ACL");
            #endregion

            ck.StrKey1 = "";
            //panel2.Visible = false;

            //uc_zone setting
            UC_Zone_Setup();

            WorkControl();

            InitGrd();

            //timer1.Interval = 5000;
            //timer1.Start();
            //timer1_Tick(null, EventArgs.Empty); // Simulate a timer tick event

            // connect indicater
            IsConnected.TextChanged += IsConnected_TextChanged;

            InitButton();
            btnDisplay_Click(null, null);

            sw = new Stopwatch();
            t = Task.Factory.StartNew(AsyncUpdateUIData);

        }

        private void AsyncUpdateUIData()
        {
           
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
                Console.WriteLine("Line3 update Elapsed:" + sw.Elapsed.TotalMilliseconds);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            }

        }

        private void InitButton()
        {
            //권한을 읽어온다
            //권한에 따라 버튼 enable 설정을 한다.
            //INQ_ACL
            //REG_ACL
            //MOD_ACL
            //DEL_ACL

            SetupButton(btnInsertReg, Acltable);
            SetupButton(btnZoneMove, Acltable);
            SetupButton(btnPOCfin, Acltable);
            SetupButton(btnReWork, Acltable);
            SetupButton(btnRsltCancel, Acltable);
            SetupButton(btnZ14Fin, Acltable);
            

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
                    //btn.Enabled = (row["REG_ACL"].ToString() == "Y");
                    btn.Enabled = (row["MOD_ACL"].ToString() == "Y");
                    //btn.Enabled = (row["DEL_ACL"].ToString() == "Y");
                }
            }
            else btn.Enabled = false;
        }

        

        public static bool SetModifyACL(string PageID, DataTable AclDT, string indicatorName)
        {
            string sql = string.Empty;
            bool CanModify = false;

            sql = string.Format("PAGE_ID = '{0}'", PageID);

            DataRow[] result = AclDT.Select(sql);
            foreach (DataRow row in result)
            {
                //btn.Enabled = (row["INQ_ACL"].ToString() == "Y");
                //CanModify = (row["REG_ACL"].ToString() == "Y");
                CanModify = (row[indicatorName].ToString() == "Y");
                //btn.Enabled = (row["DEL_ACL"].ToString() == "Y");
            }

            return CanModify;
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

            uC_WorkShow1.Location = new System.Drawing.Point(1038, 128);
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
            SetWorkBinding(line_gp, uC_WorkShow1);

        }

        private void UC_WorkShow1_PopupPanelEvent(object sender, EventArgs e)
        {
            ChangeWork(line_gp, uC_WorkShow1.WorkType, uC_WorkShow1.WorkTeam, this);
        }

        public static void ChangeWork(string _line, string _worktype, string _workteam, Form parent)
        {
            Change_Work_popup popup = new Change_Work_popup(_line, _worktype, _workteam);
            popup.Owner = parent; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;

            popup.FormBorderStyle = FormBorderStyle.FixedDialog;
            // Set the MaximizeBox to false to remove the maximize box.
            popup.MaximizeBox = false;
            // Set the MinimizeBox to false to remove the minimize box.
            popup.MinimizeBox = false;

            popup.ShowDialog();
        }



        #region UC_Zone Setting
        protected virtual void UC_Zone_Setup()
        {
            List<string> hideZoneList = new List<string>();

            //hideZoneList.Add("3Z02");
            //hideZoneList.Add("3Z13");
            //hideZoneList.Add("3Z14");
            //hideZoneList.Add("3Z16");
            //hideZoneList.Add("3Z17");
            //hideZoneList.Add("3Z18");
            //hideZoneList.Add("3Z22");
            //hideZoneList.Add("3Z23");
            //hideZoneList.Add("3Z25");
            //hideZoneList.Add("3Z29");
            //hideZoneList.Add("3Z31");
            //hideZoneList.Add("3Z32");

            foreach (var item in vf.GetAllChildrens(this))
            {
                if (item.GetType().ToString() == "WindowsFormsApplication15.UC_Zone")
                {
                    var uc_zone = item as UC_Zone;
                    if (CanShowZoneMoveAcl)
                    {
                        uc_zone.PopupEvent += PopupEvent;
                    }

                    foreach (var zone in hideZoneList)
                    {
                        if (uc_zone.ZoneCD == zone)
                        {
                            uc_zone.Visible = false;
                            this.Invalidate();
                        }
                    }
                    

                }
            }
        }



        private void btnZoneMove_Click(object sender, EventArgs e)
        {
            ck.StrKey1 = line_gp;

            ck.StrKey2 = "";

            OpenPopup();

        }

       

        protected virtual void PopupEvent(object sender, EventArgs e)
        {
            ck.StrKey1 = line_gp;

            if (!string.IsNullOrEmpty(((Control)sender).Parent.Name))
            {
                //ck.StrKey2 = ((Control)sender).Parent.Name;

                var uc_zone = ((Control)sender).Parent as UC_Zone;
                ck.StrKey2 = uc_zone.ZoneCD;
            }

            OpenPopup(CanZoneMoveAcl);
        }

        public static void OpenPopup(bool canZoneMoveAcl)
        {
            var temp_form = (InsRegPopup)GetForm("InsRegPopup");

            if (temp_form == null)
            {
                var sub = new InsRegPopup(canZoneMoveAcl);

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
        public static void OpenPopup()
        {

            var temp_form = (InsRegPopup)GetForm("InsRegPopup");

            if (temp_form == null)
            {
                var sub = new InsRegPopup();

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
        //public static IEnumerable<T> GetChildrens<T>(Control control)
        //{
        //    var type = typeof(T);

        //    var allControls = GetAllChildrens(control);

        //    return allControls.Where(c => c.GetType() == type).Cast<T>();
        //}

        //private static IEnumerable<Control> GetAllChildrens(Control control)
        //{
        //    var controls = control.Controls.Cast<Control>();
        //    return controls.SelectMany(c => GetAllChildrens(c))
        //      .Concat(controls);
        //}

        #endregion UC_Zone Setting

        #endregion main form load

        #region grid 초기 스타일 설정
        private void InitGrd()
        {

            //grdMain1
            InitgrdMain1();

            //grdMain2
            InitgrdMain2();

            //grdMain3
            InitgrdMain3();

            //grdMain4
            InitgrdMain4();

            //grdMain5
            InitgrdMain5();

            //grdMain6
            InitgrdMain6();

            //grdMain6_1
            //InitgrdMain6_1();

            //grdMain8
            InitgrdMain8();

            //grdMain7
            InitgrdMain7();

            //grdMain9
            InitgrdMain9();

            //grdMain10
            InitgrdMain10();

            //grdMain12
            InitgrdMain12();

        }

        private void InitgrdMain1()
        {
            //grdMain1.Cols.Fixed = 1;
            //grdMain1.Rows.Fixed = 1;

            //InitGrid(grdMain1, true);
            cs.InitGrid_search(grdMain1, true);

            //grdMain1.Cols.Fixed = 1;

            grdMain1[0, 0] = grdMain1[0, 1] = "교정";
            grdMain1[1, 0] = "상롤위치";
            grdMain1[2, 0] = "상롤각도";
            grdMain1[3, 0] = "하롤각도";
            grdMain1[4, 0] = "상롤모터(Hz)";
            grdMain1[5, 0] = "하롤모터(Hz)";
            //grdMain1.Rows[0].StyleFixed = c1FlexGrid1.Styles["Fixed1"];

            //CellStyle cs_fix = grdMain1.Styles.Add("Fixed");
            //cs_fix.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;

            //// row head 설정
            //for (int row = 1; row < grdMain1.Rows.Count; row++) 
            //{
            //    grdMain1.SetCellStyle(row, 0, cs_fix); 

            //}

            grdMain1.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            grdMain1.Rows[0].AllowMerging = true;

            //for (int i = 0; i < grdMain1.Cols.Count; i++)
            //{
            //    grdMain1.Cols[i].AllowMerging = true;

            //}
            grdMain1.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain1.Cols[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;

            grdMain1.Row = -1;

        }

        private void InitgrdMain2()
        {
            //grdMain2.Cols.Fixed = 1;
            //grdMain2.Rows.Fixed = 1;

            //InitGrid(grdMain2, true);
            cs.InitGrid_search(grdMain2, true);

            grdMain2[0, 0] = grdMain2[0, 1] = "면취";
            grdMain2[1, 0] = "#1스크류(Hz)";
            grdMain2[2, 0] = "#1서보";
            grdMain2[3, 0] = "#2스크류(Hz)";
            grdMain2[4, 0] = "#2서보";

            //grdMain2.Rows[0].StyleFixed = c1FlexGrid1.Styles["Fixed1"];

            //CellStyle cs_fix = grdMain2.Styles.Add("Fixed");
            //cs_fix.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;

            //// row head 설정
            //for (int row = 1; row < grdMain2.Rows.Count; row++)
            //{
            //    grdMain2.SetCellStyle(row, 0, cs_fix);

            //}

            grdMain2.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            grdMain2.Rows[0].AllowMerging = true;

            //for (int i = 0; i < grdMain2.Cols.Count; i++)
            //{
            //    grdMain2.Cols[i].AllowMerging = true;

            //}
            grdMain2.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain2.Cols[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;

            grdMain2.Row = -1;
        }

        private void InitgrdMain3()
        {
            //InitGrid(grdMain3, true);
            cs.InitGrid_search(grdMain3, true);

            //grdMain3[1, 0] = "PRII";
            //grdMain3[2, 0] = "MAT";
            //grdMain3[3, 0] = "MLFT";
            //grdMain3[4, 0] = "UT";

            //grdMain3.MergedRanges.Add(grdMain3.GetCellRange(1, 2, 4, 2));


            //grdMain3[1, 0] = "PRII";
            grdMain3[1, 0] = "MAT";
            grdMain3[2, 0] = "MLFT";
            grdMain3[3, 0] = "UT";


            grdMain3.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Custom;

            grdMain3.MergedRanges.Add(grdMain3.GetCellRange(1, 2, 3, 2));


            grdMain3.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain3.Cols[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain3.Cols[2].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain3.Row = -1;
        }

        private void InitgrdMain4()
        {
            //InitGrid(grdMain4, true);
            cs.InitGrid_search(grdMain4, true);

            grdMain4[1, 0] = "MPI";
            grdMain4[2, 0] = "G/R";


            //CellStyle cs_fix = grdMain4.Styles.Add("Fixed");
            //cs_fix.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;

            //// row head 설정
            //for (int row = 1; row < grdMain4.Rows.Count; row++)
            //{
            //    grdMain4.SetCellStyle(row, 0, cs_fix);

            //}

            grdMain4.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            grdMain4.Rows[0].AllowMerging = true;

            //for (int i = 0; i < grdMain4.Cols.Count; i++)
            //{
            //    grdMain4.Cols[i].AllowMerging = true;

            //}
            grdMain4.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain4.Cols[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain4.Cols[2].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain4.Row = -1;
        }
        private void InitgrdMain5()
        {
            //InitGrid(grdMain5, true);
            cs.InitGrid_search(grdMain5, true);

            grdMain5[1, 0] = "성분분석";
            grdMain5[2, 0] = "라벨";


            //CellStyle cs_fix = grdMain5.Styles.Add("Fixed");
            //cs_fix.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;

            //// row head 설정
            //for (int row = 1; row < grdMain5.Rows.Count; row++)
            //{
            //    grdMain5.SetCellStyle(row, 0, cs_fix);

            //}

            //grdMain5.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            grdMain5.Rows[0].AllowMerging = true;

            //for (int i = 0; i < grdMain5.Cols.Count; i++)
            //{
            //    grdMain5.Cols[i].AllowMerging = true;

            //}

            grdMain5.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Custom;

            grdMain5.MergedRanges.Add(grdMain5.GetCellRange(2, 1, 2, 2));

            grdMain5.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain5.Cols[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain5.Cols[2].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain5.Row = -1;

        }

        private void InitgrdMain6()
        {
            //InitGrid(grdMain6, true);
            cs.InitGrid_search(grdMain6, true);

            //CellStyle cs_fix = grdMain6.Styles.Add("Fixed");
            //cs_fix.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;

            // row head 설정
            //for (int row = 1; row < grdMain6.Rows.Count; row++)
            //{
            //    grdMain6.SetCellStyle(row, 0, cs_fix);

            //}

            //grdMain6.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            //grdMain6.Rows[0].AllowMerging = true;

            //for (int i = 0; i < grdMain6.Cols.Count; i++)
            //{
            //    grdMain6.Cols[i].AllowMerging = true;

            //}
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

            //grdMain7.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;

            //grdMain7.Rows[0].AllowMerging = true;

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

        private void InitgrdMain8()
        {
            cs.InitGrid_search(grdMain8, true);

            grdMain8.Cols["STEEL_NM"].Caption = "강종명";
            grdMain8.Cols["HEAT"].Caption = "HEAT";
            grdMain8.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain8.Cols["LENGTH"].Caption = "길이(m)";
            grdMain8.Cols["SURFACE_LEVEL"].Caption = "표면등급";
            grdMain8.Cols["POC_NO"].Caption = "POC";
            grdMain8.Cols["MILL_PCS"].Caption = "압연본수";
            grdMain8.Cols["STR_PCS"].Caption = "교정본수";
            grdMain8.Cols["OK_PCS"].Caption = "바인딩본수";
            grdMain8.Cols["NG_PCS"].Caption = "격외본수";



            grdMain8.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain8.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain8.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain8.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain8.Cols["SURFACE_LEVEL"].TextAlign = cs.SURFACE_LEVEL_TextAlign;
            grdMain8.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;

        }

        private void InitgrdMain9()
        {
            cs.InitGrid_search(grdMain9, true, 2, 1);

            grdMain9[1, 0] = grdMain9[0, 0] = "POC NO";
            grdMain9[1, 1] = grdMain9[0, 1] = " 검사\n본수";
            grdMain9[0, 3] = grdMain9[0, 2] = "PRII";
            grdMain9[1, 2] = "OK";
            grdMain9[1, 3] = "NG";

            grdMain9[0, 4] = "MAT";
            grdMain9[1, 4] = "NG";

            grdMain9[0, 5] = "MLFT";
            grdMain9[1, 5] = "NG";

            grdMain9[0, 6] = "UT";
            grdMain9[1, 6] = "NG";

            grdMain9[0, 7] = grdMain9[1, 7] = " NG\n본수";

            grdMain9[0, 8] = grdMain9[0, 9] = "MPI";

            grdMain9[1, 8] = "OK";
            grdMain9[1, 9] = "NG";

            grdMain9[0, 10] = grdMain9[0, 11] = "G/R";

            grdMain9[1, 10] = "OK";
            grdMain9[1, 11] = "NG";

            //grdMain9.Cols["MILL_NO"].Width = 88;

            //int nwidth = (grdMain9.Size.Width - 88 - 5) / 9;

            //grdMain9.Cols["MILL_PCS"].Width = nwidth;
            ////PRII HIDE      ]
            //grdMain9.Cols["PRII_OK_PCS"].Width = nwidth;
            //grdMain9.Cols["PRII_NG_PCS"].Width = nwidth;

            //grdMain9.Cols["MAT_NG_PCS"].Width = nwidth;
            //grdMain9.Cols["MLFT_NG_PCS"].Width = nwidth;
            //grdMain9.Cols["UT_NG_PCS"].Width = nwidth;
            //grdMain9.Cols["NDT_NG_PCS"].Width = nwidth;
            //grdMain9.Cols["MPI_OK_PCS"].Width = nwidth;
            //grdMain9.Cols["MPI_NG_PCS"].Width = nwidth;
            //grdMain9.Cols["GR_OK_PCS"].Width = nwidth;
            //grdMain9.Cols["GR_NG_PCS"].Width = nwidth-5;


            ////PRII HIDE
            //grdMain9.Cols["PRII_NG_PCS"].Width = 0;
            //grdMain9.Cols["PRII_OK_PCS"].Width = 0;




            grdMain9.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain9.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            //c1FlexGrid8.Cols[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain9.Cols["POC_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain9.Cols["NDT_INSP_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain9.Cols["PRII_OK_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain9.Cols["PRII_NG_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain9.Cols["MAT_NG_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain9.Cols["MLFT_NG_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain9.Cols["UT_NG_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain9.Cols["NDT_NG_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain9.Cols["MPI_OK_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain9.Cols["MPI_NG_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain9.Cols["GR_OK_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain9.Cols["GR_NG_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;


            grdMain9.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            //c1FlexGrid8.Rows[1].AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            //c1FlexGrid8.Rows[0].AllowMerging = c1FlexGrid8.Rows[1].AllowMerging = true;

            for (int i = 0; i < grdMain9.Cols.Count; i++)
            {
                grdMain9.Cols[i].AllowMerging = true;

            }

            grdMain9.Rows[0].AllowMerging = true;



            grdMain9.ExtendLastCol = true;

            //for (int i = 0; i < c1FlexGrid8.Rows.Count; i++)
            //{
            //    c1FlexGrid8.Rows[i].AllowMerging = true;
            //}
        }

        private void InitgrdMain10()
        {
            cs.InitGrid_search(grdMain10, true);

            grdMain10.Cols["BUNDLE_NO"].Caption = "제품번들번호";
            grdMain10.Cols["PCS"].Caption = "본수";
            grdMain10.Cols["THEORY_WGT"].Caption = "이론중량";
            grdMain10.Cols["NET_WGT"].Caption = "실중량";



            grdMain10.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain10.Cols["BUNDLE_NO"].TextAlign = cs.BUNDLE_NO_TextAlign;
            grdMain10.Cols["PCS"].TextAlign = cs.FINISH_PCS_TextAlign;
            grdMain10.Cols["THEORY_WGT"].TextAlign = cs.WGT_TextAlign;
            grdMain10.Cols["NET_WGT"].TextAlign = cs.WGT_TextAlign;

            grdMain10.ExtendLastCol = true;
        }

        

        #endregion grid 초기 스타일 설정

        #region 타이머 설정
        private void timer1_Tick(object sender, EventArgs e)
        {
            //UpdateUIData();
        }



        private void UpdateUIData()
        {
            //InitGrd();

            // 데이터를 뿌리기전에 디비 연결상태를 확인해서 연결되지않은 상태에서는 접속하지 않게 한다.
            if (!cd.CheckDbConnection())
            {
                IsConnected.Text = "DisConnected";
                this.Enable(false);
                return;
            }


            //InitGrd();
            this.Enable(true);
            IsConnected.Text = "Connected";


            //cd.CheckDbConnection();
            //현재의 근조 표시
            SetWorkBinding(line_gp, uC_WorkShow1);

            SetDataBinding();

            SetDataBinding_Zone();

            Invalidate();
        }
        #endregion

        #region grid SetDataBinding
        public static void SetWorkBinding(string _line_gp, UC.UC_WorkShow _workshow)
        {
            string sql1 = string.Empty;
            DataTable dt = new DataTable();
            ConnectDB cd = new ConnectDB();

            sql1 += string.Format(" SELECT WORK_TYPE ");
            sql1 += string.Format("       ,WORK_TEAM ");
            sql1 += string.Format(" FROM TB_LINE_WORK_TEAM ");
            sql1 += string.Format(" WHERE LINE_GP = '{0}' ", _line_gp);

            dt = cd.FindDataTable(sql1);
            if (dt != null && dt.Rows.Count > 0)
            {
                _workshow.WorkType = dt.Rows[0]["WORK_TYPE"] + "근";
                _workshow.WorkTeam = dt.Rows[0]["WORK_TEAM"] + "조";

            }
        }

        private void SetDataBinding()
        {
            //데이터를 가지고온다.

            //grdMain1
            SetDataBinding_grdMain1();

            //grdMain2
            SetDataBinding_grdMain2();

            //grdMain3
            SetDataBinding_grdMain3();

            //grdMain4
            SetDataBinding_grdMain4();

            //grdMain5 -1
            SetDataBinding_grdMain5();

            SetDataBinding_grdMain5_1();

            //grdMain6
            SetDataBinding_grdMain6();

            //grdMain6_1
            //SetDataBinding_grdMain6_1();

            // grdMain7
            SetDataBinding_grdMain7();

            //grdMain8
            SetDataBinding_grdMain8();

            //grdMain9
            SetDataBinding_grdMain9();

            // grdMain12
            SetDataBinding_grdMain12();
            //각 위치에 데이터 입력한다. 

            SetDataBinding_grdMain10();

            SetDataBinding_grdMain11();
        }
        private void SetDataBinding_grdMain1()
        {

            string sql1 = string.Empty;
            sql1 += string.Format("SELECT A.TOP_ROLL_POS_ACTV       ");//--상롤위치
            sql1 += string.Format("      ,A.TOP_ROLL_ANGLE_ACTV     ");//--상롤각도
            sql1 += string.Format("      ,A.BOT_ROLL_ANGLE_ACTV     ");//--하롤각도
            sql1 += string.Format("      ,A.TOP_ROLL_MTR_RPM_ACTV   ");//--상롤모터(RPM)
            sql1 += string.Format("      ,A.BOT_ROLL_MTR_RPM_ACTV   ");//--하롤모터(RPM)
            sql1 += string.Format("FROM   TB_STR_OPERINFO_NO3 A ");
            sql1 += string.Format("WHERE  WORK_DDTT = (SELECT MAX(WORK_DDTT) FROM   TB_STR_OPERINFO_NO3) ");



            moddt = cd.FindDataTable(sql1);
            if (moddt != null && moddt.Rows.Count > 0)
            {
                //this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain1[1, 1] = moddt.Rows[0]["TOP_ROLL_POS_ACTV"];
                grdMain1[2, 1] = moddt.Rows[0]["TOP_ROLL_ANGLE_ACTV"];
                grdMain1[3, 1] = moddt.Rows[0]["BOT_ROLL_ANGLE_ACTV"];
                grdMain1[4, 1] = moddt.Rows[0]["TOP_ROLL_MTR_RPM_ACTV"];
                grdMain1[5, 1] = moddt.Rows[0]["BOT_ROLL_MTR_RPM_ACTV"];
                //this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            else
            {
                grdMain1[1, 1] = string.Empty;
                grdMain1[2, 1] = string.Empty;
                grdMain1[3, 1] = string.Empty;
                grdMain1[4, 1] = string.Empty;
                grdMain1[5, 1] = string.Empty;
            }

            // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
            //grdMain_Row_Selected(grdMain.Row);
            //grdMain1.Row = -1;
            grdMain1.Invalidate();
            SetGridRowSelect(grdMain1, -1);
            return;
        }
        private void SetDataBinding_grdMain2()
        {


            string sql1 = string.Empty;
            sql1 += string.Format("SELECT A.SCREW_FEED_HZ_ACTV_1    ");   //--#1스크류(Hz)
            sql1 += string.Format("      ,A.SERVO_ACTV_1            ");   //--#1서보
            sql1 += string.Format("      ,A.SCREW_FEED_HZ_ACTV_2    ");   //--#2TMZMFB(Hz)
            sql1 += string.Format("      ,A.SERVO_ACTV_2            ");   //--#2서보
            sql1 += string.Format("FROM   TB_CHF_OPERINFO_NO3 A ");
            //sql1 += string.Format("      ,TB_CR_INPUT_WR B ");
            sql1 += string.Format("WHERE  WORK_DDTT = (SELECT MAX(WORK_DDTT) FROM   TB_CHF_OPERINFO_NO3) ");
            //sql1 += string.Format("AND    A.MILL_NO    = B.MILL_NO ");
            //sql1 += string.Format("AND    B.POC_NO     = ( SELECT MAX(POC_NO)  ");
            //sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
            //sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
            //sql1 += string.Format("                        AND    LINE_GP     = :P_LINE_GP ) ");

            //string[] parm = new string[1];
            //parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1);

            //moddt = cd.FindDataTable(sql1);
            if (moddt != null && moddt.Rows.Count > 0)
            {
                //this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain2[1, 1] = moddt.Rows[0]["SCREW_FEED_HZ_ACTV_1"];
                grdMain2[2, 1] = moddt.Rows[0]["SERVO_ACTV_1"];
                grdMain2[3, 1] = moddt.Rows[0]["SCREW_FEED_HZ_ACTV_2"];
                grdMain2[4, 1] = moddt.Rows[0]["SERVO_ACTV_2"];

                //this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            else
            {
                grdMain2[1, 1] = string.Empty;
                grdMain2[2, 1] = string.Empty;
                grdMain2[3, 1] = string.Empty;
                grdMain2[4, 1] = string.Empty;
            }

            // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
            //grdMain_Row_Selected(grdMain.Row);
            //grdMain2.Row = -1;
            grdMain2.Invalidate();
            SetGridRowSelect(grdMain2, -1);
            return;
        }
        private void SetDataBinding_grdMain3()
        {

            // NDT 영역에서의 마지막 입출된 PIECE_NO 
            // 그 PIECE_NO에 따른 합부 검색..
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
            //sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.PRII_GOOD_NG) AS PRII_GOOD_NG ");
            //sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.MAT_GOOD_NG) AS MAT_GOOD_NG ");
            //sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.MLFT_GOOD_NG) AS MLFT_GOOD_NG ");
            //sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.UT_GOOD_NG) AS UT_GOOD_NG ");
            //sql1 += string.Format("FROM ( ");
            //sql1 += string.Format("      SELECT /*+RULE */ MILL_NO ");
            //sql1 += string.Format("            ,PIECE_NO ");
            //sql1 += string.Format("            ,(SELECT GOOD_YN FROM TB_CR_PIECE_WR ");
            //sql1 += string.Format("              WHERE MILL_NO = A.MILL_NO AND PIECE_NO   = A.PIECE_NO ");
            //sql1 += string.Format("              AND   LINE_GP = A.LINE_GP AND ROUTING_CD = 'D2' AND REWORK_SEQ = A.REWORK_SEQ) AS PRII_GOOD_NG ");// D2: PRII
            //sql1 += string.Format("            ,MAT_GOOD_NG ");
            //sql1 += string.Format("            ,MLFT_GOOD_NG ");
            //sql1 += string.Format("            ,UT_GOOD_NG ");
            //sql1 += string.Format("      FROM   TB_CR_PIECE_WR A ");
            //sql1 += string.Format("      WHERE  LINE_GP     = :P_LINE_GP ");
            //sql1 += string.Format("      AND    ROUTING_CD  = 'F2' "); // NDT
            //sql1 += string.Format("      AND    EXIT_DDTT   = (SELECT MAX(EXIT_DDTT) FROM TB_CR_PIECE_WR ");
            //sql1 += string.Format("                            WHERE  LINE_GP    = A.LINE_GP ");
            //sql1 += string.Format("                            AND    ROUTING_CD = A.ROUTING_CD) ");
            //sql1 += string.Format("      AND    A.POC_NO     = ( SELECT MAX(POC_NO)  /*+RULE */ ");
            //sql1 += string.Format("                              FROM   TB_RL_TM_TRACKING ");
            //sql1 += string.Format("                              WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
            //sql1 += string.Format("                              AND    ZONE_CD   <>  '3Z99' ");
            //sql1 += string.Format("                              AND    LINE_GP     = A.LINE_GP ) ");
            //sql1 += string.Format("      AND    ROWNUM      = 1 ");
            //sql1 += string.Format("      ) X ");

            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1, parm);



            if (moddt != null && moddt.Rows.Count > 0)
            {
                grdMain3[1, 1] = moddt.Rows[0]["MAT_GOOD_NG"];
                grdMain3[2, 1] = moddt.Rows[0]["MLFT_GOOD_NG"];
                grdMain3[3, 1] = moddt.Rows[0]["UT_GOOD_NG"];
                grdMain3[1, 2] = grdMain3[2, 2] = grdMain3[3, 2] = moddt.Rows[0]["PIECE_NO"];

            }
            else
            {
                grdMain3[1, 1] = string.Empty;
                grdMain3[2, 1] = string.Empty;
                grdMain3[3, 1] = string.Empty;
                grdMain3[1, 2] = grdMain3[2, 2] = grdMain3[3, 2] = string.Empty;
            }

            //grdMain3.Row = -1;
            grdMain3.Invalidate();
            SetGridRowSelect(grdMain3, -1);
            return;
        }
        private void SetDataBinding_grdMain4()
        {


            string sql1 = string.Empty;
            sql1 += string.Format("SELECT X.MPI_INSP_TIT AS MPI_INSP_TIT ");
            sql1 += string.Format("      ,X.GR_INSP_TIT AS GR_INSP_TIT ");
            sql1 += string.Format("      ,X.MPI_MILL_NO AS MPI_MILL_NO ");
            sql1 += string.Format("      ,X.GR_MILL_NO AS GR_MILL_NO ");
            sql1 += string.Format("      ,X.MPI_PIECE_NO AS MPI_PIECE_NO ");
            sql1 += string.Format("      ,X.GR_PIECE_NO AS GR_PIECE_NO ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.MPI_GOOD_NG) AS MPI_GOOD_NG ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.GR_GOOD_NG) AS GR_GOOD_NG ");
            sql1 += string.Format("FROM ");
            sql1 += string.Format("( ");
            sql1 += string.Format("SELECT /*+RULE */ MAX(DECODE(ROUTING_CD,'H2','MPI')   ) AS MPI_INSP_TIT ");
            sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'K2','G/R')   ) AS GR_INSP_TIT ");
            sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'H2',MILL_NO) ) AS MPI_MILL_NO ");
            sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'K2',MILL_NO) ) AS GR_MILL_NO ");
            sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'H2',PIECE_NO)) AS MPI_PIECE_NO ");
            sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'K2',PIECE_NO)) AS GR_PIECE_NO ");
            sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'H2',MPI_INSP_GOOD_NG)) AS MPI_GOOD_NG ");
            sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'K2',GOOD_YN)         ) AS GR_GOOD_NG ");
            sql1 += string.Format("FROM   TB_CR_PIECE_WR A ");
            sql1 += string.Format("WHERE  LINE_GP     =  :P_LINE_GP ");
            sql1 += string.Format("AND    ROUTING_CD IN ('H2','K2') ");
            sql1 += string.Format("      AND A.POC_NO = (SELECT POC_NO FROM TB_PROG_POC_MGMT WHERE LINE_GP = :P_LINE_GP ) "); // NDT
            sql1 += string.Format("      ORDER BY EXIT_DDTT DESC ");
            sql1 += string.Format("      ) X ");
            sql1 += string.Format("      WHERE     ROWNUM <= 1 ");

            //sql1 += string.Format("SELECT X.MPI_INSP_TIT AS MPI_INSP_TIT ");
            //sql1 += string.Format("      ,X.GR_INSP_TIT AS GR_INSP_TIT ");
            //sql1 += string.Format("      ,X.MPI_MILL_NO AS MPI_MILL_NO ");
            //sql1 += string.Format("      ,X.GR_MILL_NO AS GR_MILL_NO ");
            //sql1 += string.Format("      ,X.MPI_PIECE_NO AS MPI_PIECE_NO ");
            //sql1 += string.Format("      ,X.GR_PIECE_NO AS GR_PIECE_NO ");
            //sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.MPI_GOOD_NG) AS MPI_GOOD_NG ");
            //sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.GR_GOOD_NG) AS GR_GOOD_NG ");
            //sql1 += string.Format("FROM ");
            //sql1 += string.Format("( ");
            //sql1 += string.Format("SELECT /*+RULE */ MAX(DECODE(ROUTING_CD,'H2','MPI')   ) AS MPI_INSP_TIT ");
            //sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'K2','G/R')   ) AS GR_INSP_TIT ");
            //sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'H2',MILL_NO) ) AS MPI_MILL_NO ");
            //sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'K2',MILL_NO) ) AS GR_MILL_NO ");
            //sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'H2',PIECE_NO)) AS MPI_PIECE_NO ");
            //sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'K2',PIECE_NO)) AS GR_PIECE_NO ");
            //sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'H2',MPI_INSP_GOOD_NG)) AS MPI_GOOD_NG ");
            //sql1 += string.Format("      ,MAX(DECODE(ROUTING_CD,'K2',GOOD_YN)         ) AS GR_GOOD_NG ");
            //sql1 += string.Format("FROM   TB_CR_PIECE_WR A ");
            //sql1 += string.Format("WHERE  LINE_GP     =  :P_LINE_GP ");
            //sql1 += string.Format("AND    ROUTING_CD IN ('H2','K2') ");
            //sql1 += string.Format("AND    EXIT_DDTT   = (SELECT MAX(EXIT_DDTT) FROM TB_CR_PIECE_WR ");
            //sql1 += string.Format("                      WHERE  LINE_GP    = A.LINE_GP ");
            //sql1 += string.Format("                      AND    ROUTING_CD = A.ROUTING_CD) ");
            //sql1 += string.Format("AND    A.POC_NO     = ( SELECT /*+RULE */ MAX(POC_NO)  ");
            //sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
            //sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
            //sql1 += string.Format("                        AND    ZONE_CD   <>  '3Z99' ");
            //sql1 += string.Format("                        AND    LINE_GP     = A.LINE_GP ) ");
            //sql1 += string.Format(") X ");

            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1, parm);


            if (moddt != null && moddt.Rows.Count > 0)
            {
                grdMain4[1, 1] = moddt.Rows[0]["MPI_GOOD_NG"];
                grdMain4[2, 1] = moddt.Rows[0]["GR_GOOD_NG"];
                grdMain4[1, 2] = moddt.Rows[0]["MPI_PIECE_NO"];
                grdMain4[2, 2] = moddt.Rows[0]["GR_PIECE_NO"];
            }
            else
            {
                grdMain4[1, 1] = string.Empty;
                grdMain4[2, 1] = string.Empty;
                grdMain4[1, 2] = string.Empty;
                grdMain4[2, 2] = string.Empty;
            }

            // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
            //grdMain_Row_Selected(grdMain.Row);

            for (int i = 0; i < grdMain4.Rows.Count; i++)
            {
                grdMain4.Rows[i].AllowMerging = true;
            }

            //grdMain4.Row = -1;
            grdMain4.Invalidate();
            SetGridRowSelect(grdMain4, -1);

        }

        private void SetDataBinding_grdMain5_1()
        {


            string sql1 = string.Empty;
            sql1 += string.Format("SELECT BUNDLE_NO ");
            sql1 += string.Format("FROM( ");
            sql1 += string.Format("SELECT /*+ INDEX_SS(A, XK4_TB_CR_PIECE_WR) */ BUNDLE_NO ");
            sql1 += string.Format("FROM   TB_CR_PIECE_WR  A ");
            sql1 += string.Format("WHERE  LINE_GP     = :P_LINE_GP ");
            sql1 += string.Format("AND    ROUTING_CD  = 'M3' ");
            sql1 += string.Format("      AND A.POC_NO = (SELECT POC_NO FROM TB_PROG_POC_MGMT WHERE LINE_GP = :P_LINE_GP ) "); // NDT
            sql1 += string.Format("      ORDER BY ENTRY_DDTT DESC ");
            sql1 += string.Format("      ) X ");
            sql1 += string.Format("      WHERE     ROWNUM <= 1 ");


            //sql1 += string.Format("SELECT BUNDLE_NO ");
            //sql1 += string.Format("FROM   TB_CR_PIECE_WR  A ");
            //sql1 += string.Format("WHERE  LINE_GP     = :P_LINE_GP ");
            //sql1 += string.Format("AND    ROUTING_CD  = 'M3' ");
            //sql1 += string.Format("AND    ENTRY_DDTT    = (SELECT MAX(ENTRY_DDTT)  FROM TB_CR_PIECE_WR ");
            //sql1 += string.Format("                      WHERE  LINE_GP     = A.LINE_GP ");
            //sql1 += string.Format("                      AND    ROUTING_CD  = 'M3' ) ");
            //sql1 += string.Format("AND    A.POC_NO     = ( SELECT /*+RULE */ MAX(POC_NO)  ");
            //sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
            //sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
            //sql1 += string.Format("                        AND    ZONE_CD   <>  '3Z99' ");
            //sql1 += string.Format("                        AND    LINE_GP     = A.LINE_GP ) ");
            //sql1 += string.Format("AND    ROWNUM      = 1 ");

            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1, parm);

            if (moddt != null && moddt.Rows.Count > 0)
            {
                //this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain5[2, 1] = grdMain5[2, 2] = moddt.Rows[0]["BUNDLE_NO"];// 라벨
                //this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            else
            {
                grdMain5[2, 1] = grdMain5[2, 2] = string.Empty;
            }

            //grdMain5.Row = -1;
            grdMain5.Invalidate();
            SetGridRowSelect(grdMain5, -1);

        }

        private void SetDataBinding_grdMain5()
        {

            string sql1 = string.Empty;
            sql1 += string.Format("SELECT X.MILL_NO AS MILL_NO ");
            sql1 += string.Format("      ,X.PIECE_NO AS PIECE_NO ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.CHM_GOOD_NG) AS CHM_GOOD_NG ");
            sql1 += string.Format("FROM ");
            sql1 += string.Format("( ");
            sql1 += string.Format("SELECT A.MILL_NO ");
            sql1 += string.Format("      ,A.PIECE_NO ");
            sql1 += string.Format("      ,A.CHM_GOOD_NG ");
            sql1 += string.Format("FROM   TB_CHM_WR  A ");
            sql1 += string.Format("      ,TB_CR_INPUT_WR B ");
            sql1 += string.Format("WHERE  A.LINE_GP     = :P_LINE_GP ");
            sql1 += string.Format("AND    A.ENTRY_DDTT   = (SELECT MAX(ENTRY_DDTT)  FROM TB_CHM_WR ");
            sql1 += string.Format("                     WHERE  LINE_GP   = A.LINE_GP) ");
            sql1 += string.Format("AND    A.MILL_NO    = B.MILL_NO ");
            sql1 += string.Format("AND    B.POC_NO     = ( SELECT /*+RULE */ MAX(POC_NO)  ");
            sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
            sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
            sql1 += string.Format("                        AND    ZONE_CD   <>  '3Z99' ");
            sql1 += string.Format("                        AND    ZONE_CD   <>  '3Z51' ");
            sql1 += string.Format("                        AND    LINE_GP     = A.LINE_GP ) ");
            sql1 += string.Format("AND    ROWNUM      = 1 ");
            sql1 += string.Format(") X ");

            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1, parm);



            if (moddt != null && moddt.Rows.Count > 0)
            {
                //this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain5[1, 1] = moddt.Rows[0]["CHM_GOOD_NG"];
                grdMain5[1, 2] = moddt.Rows[0]["PIECE_NO"];
                //this.Cursor = System.Windows.Forms.Cursors.Default;
                
            }
            else
            {
                grdMain5[1, 1] = string.Empty;
                grdMain5[1, 2] = string.Empty;
            }
            grdMain5.Invalidate();
            //grdMain5.Row = -1;
            SetGridRowSelect(grdMain5, -1);
            return;
        }




        private void SetDataBinding_grdMain6()
        {


            string sql1 = string.Empty;
            sql1 += string.Format("SELECT BUNDLE_NO ");
            sql1 += string.Format("      ,PCS ");
            sql1 += string.Format("      ,(SELECT ROUND(NET_WGT,1) FROM TB_WGT_WR WHERE BUNDLE_NO = A.BUNDLE_NO AND NVL(DEL_YN,'N') <> 'Y' AND ROWNUM =1) AS NET_WGT ");
            sql1 += string.Format("FROM   TB_BND_WR  A ");
            sql1 += string.Format("WHERE  LINE_GP     = '{0}' ", line_gp);
            sql1 += string.Format("AND    REG_DDTT   = (SELECT MAX(REG_DDTT)  FROM TB_BND_WR ");
            sql1 += string.Format("                     WHERE  LINE_GP   = A.LINE_GP ");
            sql1 += string.Format("                     AND    NVL(DEL_YN,'N') <> 'Y') ");
            sql1 += string.Format("AND    A.POC_NO     = ( SELECT /*+RULE */ MAX(POC_NO)  ");
            sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
            sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
            sql1 += string.Format("                        AND    ZONE_CD   <>  '3Z99' ");
            sql1 += string.Format("                        AND    ZONE_CD   <>  '3Z51' ");
            sql1 += string.Format("                        AND    LINE_GP     = A.LINE_GP ) ");
            sql1 += string.Format("AND    NVL(A.DEL_YN,'N') <> 'Y' ");
            sql1 += string.Format("AND    ROWNUM      = 1 ");

            moddt = cd.FindDataTable(sql1);
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

                //grdMain6.SetDataBinding(moddt, null, true);

                grdMain6.Invalidate();
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
                grdMain7.Invalidate();
            }
            //grdMain7.Row = -1;
            SetGridRowSelect(grdMain7, -1);

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
            //sql1 += string.Format("               AND A.POC_NO = (select POC_NO from TB_PROG_POC_MGMT WHERE LINE_GP = :P_LINE_GP) ");
            sql1 += string.Format("               AND A.POC_NO = (SELECT MAX(POC_NO) FROM TB_RL_TM_TRACKING WHERE ZONE_CD = '3Z01' AND LINE_GP =  '#3' AND PROG_STAT = 'WAT') ");
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
        private void SetDataBinding_grdMain8()
        {

            string sql1 = string.Empty;
            //--실시간트레킹중간_POC정보 공통쿼리를 가져온다
            sql1 = cd.GetPOCsql();

            //moddt = new DataTable();

            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1, parm);
            
            if (moddt != null)
            {

                try
                {
                    //grdMain8.Redraw = false;
                    grdMain8.SetDataBinding(moddt, null, true);
                    //grdMain8.Redraw = true;
                }
                catch (Exception)
                {

                    return;
                }
                //grdMain8.SetDataBinding(moddt, null, true);
                //grdMain8.Row = -1;
                grdMain8.Invalidate();

            }
            SetGridRowSelect(grdMain8, -1);
        }
        private void SetDataBinding_grdMain9()
        {


            string sql1 = string.Empty;
            sql1 += string.Format(" SELECT /*+RULE */  A.POC_NO ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',1,0)),0)  AS  NDT_INSP_PCS    ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'D2',DECODE(A.GOOD_YN,'OK',1,0))     ), 0) AS PRII_OK_PCS ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'D2',DECODE(A.GOOD_YN,'NG',1,0))     ), 0) AS PRII_NG_PCS ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',DECODE(A.MAT_GOOD_NG,'NG',1,0)) ), 0) AS MAT_NG_PCS ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',DECODE(A.MLFT_GOOD_NG,'NG',DECODE(A.MAT_GOOD_NG,'NG',0,DECODE(A.UT_GOOD_NG,'NG',0,1)),0)) ), 0)  AS MLFT_NG_PCS ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',DECODE(A.UT_GOOD_NG,'NG',DECODE(A.MAT_GOOD_NG,'NG',0,1),0))  ), 0) AS UT_NG_PCS ");
            sql1 += string.Format("       ,NVL(SUM(CASE WHEN A.MAT_GOOD_NG = 'NG' OR A.MLFT_GOOD_NG = 'NG' OR A.UT_GOOD_NG = 'NG' THEN 1 ");
            sql1 += string.Format("             ELSE 0  END), 0) AS NDT_NG_PCS ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'H2',DECODE(A.MPI_INSP_GOOD_NG,'OK',1,0))), 0) AS MPI_OK_PCS ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'H2',DECODE(A.MPI_INSP_GOOD_NG,'NG',1,0))), 0) AS MPI_NG_PCS ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'K2',DECODE(A.GOOD_YN,'OK',1,0))         ), 0) AS GR_OK_PCS ");
            sql1 += string.Format("       ,NVL(SUM(DECODE(A.ROUTING_CD,'K2',DECODE(A.GOOD_YN,'NG',1,0))         ), 0) AS GR_NG_PCS ");
            sql1 += string.Format(" FROM   TB_CR_PIECE_WR A ");
            sql1 += string.Format("       ,(SELECT /*+RULE */  DISTINCT POC_NO AS POC_NO, POC_SEQ ");
            sql1 += string.Format("         FROM   TB_RL_TM_TRACKING ");
            sql1 += string.Format("         WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
            sql1 += string.Format("         AND    LINE_GP     =  :P_LINE_GP ");
            sql1 += string.Format("         AND    ZONE_CD     <> '3Z99' ");
            sql1 += string.Format("         AND    ZONE_CD     <> '3Z51' ");
            sql1 += string.Format("         AND    ROWNUM      = 1 ) B ");
            sql1 += string.Format(" WHERE  A.LINE_GP     =  :P_LINE_GP ");
            sql1 += string.Format(" AND    A.POC_NO      = B.POC_NO ");
            sql1 += string.Format(" AND    A.POC_SEQ      = B.POC_SEQ ");
            sql1 += string.Format(" AND    A.REWORK_SEQ  = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            sql1 += string.Format("                         WHERE  MILL_NO    = A.MILL_NO ");
            sql1 += string.Format("                         AND    PIECE_NO   = A.PIECE_NO ");
            sql1 += string.Format("                         AND    LINE_GP    = A.LINE_GP ");
            sql1 += string.Format("                         AND    ROUTING_CD = A.ROUTING_CD) ");
            sql1 += string.Format(" AND    A.ROUTING_CD  IN ('D2','F2','H2','K2') ");
            sql1 += string.Format(" GROUP BY A.POC_NO ");

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

                // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
                //grdMain_Row_Selected(grdMain.Row);
                //grdMain9.Row = -1;
                grdMain9.Invalidate();
            }
            SetGridRowSelect(grdMain9, -1);
        }

        private void SetDataBinding_grdMain10()
        {


            string sql1 = string.Empty;
            sql1 += string.Format(" SELECT X.* ");
            sql1 += string.Format(" FROM  ( ");
            sql1 += string.Format("         SELECT A.POC_NO ");
            sql1 += string.Format("               ,A.BUNDLE_NO ");
            sql1 += string.Format("               ,A.PCS ");
            sql1 += string.Format("               ,A.THEORY_WGT ");
            sql1 += string.Format("               ,A.NET_WGT ");
            sql1 += string.Format("               ,A.REG_DDTT ");
            sql1 += string.Format("         FROM   TB_BND_WR A ");
            sql1 += string.Format("         WHERE  A.LINE_GP  =  :P_LINE_GP ");
            sql1 += string.Format("         AND    NVL(A.DEL_YN,'N') <> 'Y' ");
            sql1 += string.Format("         AND    A.POC_NO = ( SELECT /*+RULE */ DISTINCT POC_NO AS POC_NO ");
            sql1 += string.Format("                             FROM   TB_RL_TM_TRACKING ");
            sql1 += string.Format("                             WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
            sql1 += string.Format("                             AND    ZONE_CD   <>  '3Z99' ");
            sql1 += string.Format("                             AND    ZONE_CD   <>  '3Z51' ");
            sql1 += string.Format("                             AND    LINE_GP     =  :P_LINE_GP ");
            sql1 += string.Format("                             AND    ROWNUM      = 1 ) ");
            sql1 += string.Format("         ORDER BY REG_DDTT DESC ");
            sql1 += string.Format("        ) X ");
            sql1 += string.Format(" WHERE ROWNUM < 6 ");


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


                // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
                //grdMain_Row_Selected(grdMain.Row);
                //grdMain10.Row = -1;
                grdMain10.Invalidate();
            }
            SetGridRowSelect(grdMain10, -1);
        }

        private void SetDataBinding_grdMain11()
        {


            string sql1 = string.Empty;
            sql1 += string.Format("SELECT P_MODE ");
            sql1 += string.Format("FROM TB_PROG_POC_MGMT ");
            sql1 += string.Format("WHERE LINE_GP = :P_LINE_GP");
           


            //moddt = new DataTable();
            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1, parm);

            if (moddt != null)
            {
                try
                {
                    if (moddt.Rows[0]["P_MODE"].ToString() == "0")
                    {


                        Z41.Text = "MAT,UT";
                        Z42.Text = "MLFT";
                        Z45.Text = "PRII";
                        Z46.Text = "MLFT";
                        MODE.Text = "MODE1";
                    }
                    else
                    //if (moddt.Rows[0]["P_MODE"].ToString() == "1")
                    {
                        Z41.Text = "UT";
                        Z42.Text = "MLFT";
                        Z45.Text = "MAT";
                        Z46.Text = "MLFT";
                        MODE.Text = "MODE2";

                    }
                }
                catch (Exception)
                {

                    return;
                }
                //grdMain10.SetDataBinding(moddt, null, true);


                // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
                //grdMain_Row_Selected(grdMain.Row);
                //grdMain10.Row = -1;
                //grdMain10.Invalidate();
            }
            
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



        private void SetDataBinding_Zone()
        {

            string sql1 = string.Empty;

            sql1 = cd.GetZoneDatasql();

            //moddt = new DataTable();

            string[] parm = new string[1];
            parm[0] = ":P_LINE_GP|" + line_gp;

            moddt = cd.FindDataTable(sql1, parm);

            if (moddt != null )
            {
                foreach (var item in vf.GetAllChildrens(this))
                {
                    if (item.GetType().ToString() == "WindowsFormsApplication15.UC_Zone")
                    {
                        var uc_zone = item as UC_Zone;
                        //zone 초기화
                        uc_zone.PCS = "";
                        uc_zone.MillNo = "";

                        foreach (DataRow row in moddt.Rows)
                        {
                            if (uc_zone.ZoneCD == row["ZONE_CD"].ToString())
                            {

                                if (uc_zone.ZoneCD == "3Z39")
                                {
                                    uc_zone.PCS = row["BUNDLE_PCS"].ToString();
                                    uc_zone.MillNo = row["BUNDLE_NO"].ToString();
                                }
                                else if (uc_zone.ZoneCD == "3Z38")
                                {
                                    uc_zone.PCS = row["PCS"].ToString();
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
        #endregion grid SetDataBinding

        #region 버튼이벤트
        private void btnInsertReg_Click(object sender, EventArgs e)
        {

            ////해당라인의 화면명을 가져와서 수정가능 여부에 따라 버튼 활성화/비활성화를 결정.
            //string CrtInScreenName = CrtInBundleInfo.GetLineCrtInScreenName(ck.Line_gp);

            //CanCrtInAcl = SetModifyACL(CrtInScreenName, Acltable, "MOD_ACL");

            InSertCRT(line_gp);

        }

        public static void InSertCRT(string _line_gp)
        {
            var temp_form = GetForm("CrtInRsltPopUP");

            if (temp_form == null)
            {
                var sub = new CrtInRsltPopUP(_line_gp);

                //sub.MdiParent = this;
                sub.StartPosition = FormStartPosition.CenterScreen;
                sub.Show();
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }

        public static void InSertCRT(string _line_gp, string _mill_no, bool _CanModify)
        {

            CrtInRsltPopUP temp_form = (CrtInRsltPopUP)GetForm("CrtInRsltPopUP");

            if (temp_form == null)
            {
                var sub = new CrtInRsltPopUP(_line_gp, _mill_no, _CanModify);

                //sub.MdiParent = this;
                sub.StartPosition = FormStartPosition.CenterScreen;
                sub.Show();
            }
            else
            {
                temp_form.SetMillNo(_mill_no);
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }

            //var temp_form = GetForm("CrtInRsltPopUP");

            //if (temp_form == null)
            //{
            //    var sub = new CrtInRsltPopUP(_line_gp, _mill_no);

            //    //sub.MdiParent = this;
            //    sub.StartPosition = FormStartPosition.CenterScreen;
            //    sub.Show();
            //}
            //else
            //{

            //    //temp_form = new CrtInRsltPopUP(_line_gp, _mill_no);
            //    //temp_form.Activate();
            //    temp_form.Show();
            //    temp_form.BringToFront();
            //}
        }

        private void btnPOCFin_Click(object sender, EventArgs e)
        {
            POCFinREG(line_gp);
        }

        public static void POCFinREG(string _line_gp)
        {
            var temp_form = GetForm("POCFinReg");

            if (temp_form == null)
            {
                var sub = new POCFinReg(_line_gp, "");

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

        private void btnReWork_Click(object sender, EventArgs e)
        {
            ReWorkMgmt(line_gp);
        }

        public static void ReWorkMgmt(string _line_gp)
        {
            var temp_form = GetForm("ProdReWorkMgmt");

            if (temp_form == null)
            {
                var sub = new ProdReWorkMgmt(_line_gp);

                //sub.MdiParent = this;
                //sub.StartPosition = FormStartPosition.CenterScreen;

                //sub.FormBorderStyle = FormBorderStyle.FixedDialog;
                // Set the MaximizeBox to false to remove the maximize box.
                //sub.MaximizeBox = false;
                // Set the MinimizeBox to false to remove the minimize box.
                //sub.MinimizeBox = false;
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

       
        private void btnRsltCancel_Click(object sender, EventArgs e)
        {
            // 현재 grdMain8의 행이 있다면, 첫행의 poc no를 가져와 초기검색에 사용
            string poc_No = string.Empty;

            if (grdMain8.Rows.Count > 1)
            {
                poc_No = grdMain8.GetData(1, "POC_NO").ToString().Trim();
            }

            RsltCancel(line_gp, poc_No);

        }

        public static void RsltCancel(string _line_gp, string _poc_No)
        {

            var temp_form = GetForm("WorkRsltCancel");

            if (temp_form == null)
            {
                var sub = new WorkRsltCancel(_line_gp, _poc_No);

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
        #endregion

        //bool toggled = false;
        private void btnDisplay_Click(object sender, EventArgs e)
        {

            cd.InsertLogForSearch(ck.UserID, btnDisplay);
            //timer1_Tick(null, null);
            UpdateUIData();
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

        protected virtual void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

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

        /// <summary>
        /// Z14이후 트레킹 종료 sp Call 및 정상 처리시 화면 refreash
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZ14Fin_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(" Z14 이후 트레킹이 종료됩니다 진행하시겠습니까? ", Text, MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            if (CallZ14FinSP(line_gp, ck.UserID))
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
        private bool CallZ14FinSP(string _line_gp, string _userID)
        {

            bool success = false;

            OracleConnection conn = cd.OConnect();

            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;


            string result_stat = string.Empty;
            string result_msg = string.Empty;

            string spName = "SP_NDT_BND_END_PROC";
            OracleParameter op;
            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;


                #region "SP_NDT_BND_END_PROC" sp를 실행
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

    public static class GuiExtensionMethods
    {
        public static void Enable(this Control con, bool enable)
        {
            if (con != null)
            {
                try
                {
                    con.Invoke((MethodInvoker)(() => con.Enabled = enable));
                }
                catch
                {
                }
            }
        }



        
    }

    
}