﻿using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemControlClassLibrary.UC;

namespace SystemControlClassLibrary.monitoring
{
    public partial class Line3WholeTrk_Temp : Form
    {
        public Line3WholeTrk_Temp(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            InitializeComponent();
        }

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();

        DataTable moddt = null;

        string line_gp = "#3";

        private UC.UC_WorkShow uC_WorkShow1;

        TextBox IsConnected = new TextBox();

        #region main form load


        private void Line3WholeTrk_Temp_Load(object sender, EventArgs e)
        {
            ck.StrKey1 = "";
            //panel2.Visible = false;

            //uc_zone setting
            UC_Zone_Setup();

            WorkControl();

            InitGrd();

            //cd.InsertLogForSearch(ck.UserID, btnDisplay);

            timer1.Interval = 5000;
            timer1.Start();
            timer1_Tick(null, EventArgs.Empty); // Simulate a timer tick event

            // connect indicater
            IsConnected.TextChanged += IsConnected_TextChanged;

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

            uC_WorkShow1 = new UC_WorkShow();
            //uC_Work_Type_sc1 = new SystemControlClassLibrary.UC.sub_UC.UC_Work_Type_sc();
            //uC_Work_Team_s1 = new SystemControlClassLibrary.UC.sub_UC.UC_Work_Team_s();
            // 
            // uC_WorkShow1
            // 
            //uC_WorkShow1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            uC_WorkShow1.BackColor = Color.FromArgb(0, 122, 204);

            uC_WorkShow1.Location = new System.Drawing.Point(1038, 128);
            uC_WorkShow1.Name = "uC_WorkShow1";
            uC_WorkShow1.Size = new System.Drawing.Size(197, 31);
            uC_WorkShow1.TabIndex = 163;
            uC_WorkShow1.PopupPanelEvent += UC_WorkShow1_PopupPanelEvent;

            //// 
            //// uC_Work_Type_s1
            //// 

            //uC_Work_Type_sc1.BackColor = System.Drawing.Color.Transparent;
            //uC_Work_Type_sc1.cb_Enable = true;
            //uC_Work_Type_sc1.Location = new System.Drawing.Point(5, 6);
            //uC_Work_Type_sc1.Name = "uC_Work_Type_s1";
            //uC_Work_Type_sc1.Size = new System.Drawing.Size(141, 27);
            //uC_Work_Type_sc1.TabIndex = 0;

            //// 
            //// uC_Work_Team_s1
            //// 
            //uC_Work_Team_s1.BackColor = System.Drawing.Color.Transparent;
            //uC_Work_Team_s1.cb_Enable = true;
            //uC_Work_Team_s1.Location = new System.Drawing.Point(5, 40);
            //uC_Work_Team_s1.Name = "uC_Work_Team_s1";
            //uC_Work_Team_s1.Size = new System.Drawing.Size(141, 27);
            //uC_Work_Team_s1.TabIndex = 1;


            panel1.Controls.Add(uC_WorkShow1);

            //panel2.Controls.Add(uC_Work_Team_s1);

            //panel2.Controls.Add(uC_Work_Type_sc1);

            //uC_Work_Type_sc1.Work_Type = "%";
            //uC_Work_Team_s1.Work_Team = "A";
            //uC_WorkShow1.WorkTeam = "A조";
            //uC_WorkShow1.WorkType = "1근";
            #endregion
            //uC_Work_Type_sc1.Work_Type = "3";

            // 현재의 근조를 표시해준다.
            SetWorkBinding();

        }

        private void UC_WorkShow1_PopupPanelEvent(object sender, EventArgs e)
        {
            //if (!panel2.Visible)
            //{
            //    //uC_Work_Type_sc1.Work_Type = uC_WorkShow1.WorkType.Substring(0, 1);
            //    //uC_Work_Team_s1.Work_Team = uC_WorkShow1.WorkTeam.Substring(0, 1);
            //    panel2.Visible = true;
            //    panel2.BringToFront();
            //}
            //else
            //{
            //    panel2.Visible = false;

            //}

            Change_Work_popup popup = new Change_Work_popup(line_gp, uC_WorkShow1.WorkType, uC_WorkShow1.WorkTeam);
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();

        }

        

        #region UC_Zone Setting
        private void UC_Zone_Setup()
        {
            foreach (var item in vf.GetAllChildrens(this))
            {
                if (item.GetType().ToString() == "SystemControlClassLibrary.UC.UC_Zone")
                {
                    var uc_zone = item as UC_Zone;
                    uc_zone.PopupEvent += PopupEvent;

                }
            }
        }

        private void btnZoneMove_Click(object sender, EventArgs e)
        {
            ck.StrKey1 = line_gp;

            ck.StrKey2 = "";

            //if (((Control)sender).Parent =="")

            OpenPopup();

        }

        private void OpenPopup()
        {
            InsRegPopup popup = new InsRegPopup();
            //popup.Owner = this; //A폼을 지정하게 된다.
            //popup.StartPosition = FormStartPosition.CenterScreen;
            //popup.Show();
            popup.ShowDialog();


        }

        private void PopupEvent(object sender, EventArgs e)
        {
            ck.StrKey1 = line_gp;

            if (!string.IsNullOrEmpty(((Control)sender).Parent.Name))
            {
                //ck.StrKey2 = ((Control)sender).Parent.Name;

                var uc_zone = (Control)((Control)sender).Parent as UC_Zone;
                ck.StrKey2 = uc_zone.ZoneCD;
            }

            OpenPopup();
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

        }

        //private void InitgrdMain6_1()
        //{
        //    //InitGrid(grdMain6_1, true);
        //    cs.InitGrid_search(grdMain6_1, true);

        //    CellStyle cs_fix = grdMain6_1.Styles.Add("Fixed");
        //    cs_fix.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;

        //    // row head 설정
        //    for (int row = 1; row < grdMain6_1.Rows.Count; row++)
        //    {
        //        grdMain6_1.SetCellStyle(row, 0, cs_fix);

        //    }

        //    grdMain6_1.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
        //    grdMain6_1.Rows[0].AllowMerging = true;

        //    //for (int i = 0; i < grdMain6_1.Cols.Count; i++)
        //    //{
        //    //    grdMain6_1.Cols[i].AllowMerging = true;

        //    //}
        //    grdMain6_1.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

        //    grdMain6_1.Cols[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
        //    grdMain6_1.Cols[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;

        //    grdMain6_1.Row = -1;
        //}


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
            //cs.InitGrid_search(grdMain7, true, 1, 1);
            cs.InitGrid_search(grdMain7, true, 1, 0);
            //cs.InitGrid_search(grdMain7, true);

            grdMain7.Rows.Fixed = 1;

            //grdMain7[0, 0] = grdMain7[0, 1] = grdMain7[0, 2] = "교정대기번들";

            grdMain7[0, 0] = "압연번들번호"; grdMain7[0, 1] = "압연본수"; grdMain7[0, 2] = "교정본수";

            grdMain7.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            //grdMain7.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain7.Cols["MILL_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain7.Cols["MILL_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["STR_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;


            grdMain7.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;

            //for (int i = 0; i < grdMain7.Cols.Count; i++)
            //{
            //    grdMain7.Cols[i].AllowMerging = true;

            //}

            grdMain7.Rows[0].AllowMerging = true;

            //for (int i = 0; i < 20; i++)
            //{
            //    grdMain7.Rows.Add();
            //}
            grdMain7.ExtendLastCol = true;
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



            grdMain8.Cols["STEEL_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain8.Cols["HEAT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain8.Cols["ITEM_SIZE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain8.Cols["LENGTH"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain8.Cols["SURFACE_LEVEL"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain8.Cols["POC_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

        }

        private void InitgrdMain9()
        {
            cs.InitGrid_search(grdMain9, true, 2, 1);

            grdMain9[1, 0] = grdMain9[0, 0] = "압연번들번호";
            grdMain9[1, 1] = grdMain9[0, 1] = " 압연\n본수";
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

            grdMain9.Cols["MILL_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain9.Cols["MILL_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
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

            grdMain10.Cols["BUNDLE_NO"].TextAlign = cs.BUNDLE_QTY_TextAlign;
            grdMain10.Cols["PCS"].TextAlign = cs.FINISH_PCS_TextAlign;
            grdMain10.Cols["THEORY_WGT"].TextAlign = cs.WGT_TextAlign;
            grdMain10.Cols["NET_WGT"].TextAlign = cs.WGT_TextAlign;

            grdMain10.ExtendLastCol = true;
        }



        #endregion grid 초기 스타일 설정


        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        #region 타이머 설정
        private void timer1_Tick(object sender, EventArgs e)
        {
            InitGrd();

            // 데이터를 뿌리기전에 디비 연결상태를 확인해서 연결되지않은 상태에서는 접속하지 않게 한다.
            if (!cd.CheckDbConnection())
            {
                IsConnected.Text = "disConnected";
                this.Enable(false);
                //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + " 데이터베이스 접속이 해제되었습니다.");
                return;
            }
            else
            {
                this.Enable(true);
                IsConnected.Text = "Connected";
            }


            //cd.CheckDbConnection();
            //현재의 근조 표시
            SetWorkBinding();

            SetDataBinding();

            SetDataBinding_Zone();

            this.Invalidate();
        }
        #endregion

        #region grid SetDataBinding
        private void SetWorkBinding()
        {
            string sql1 = string.Empty;
            try
            {

                sql1 += string.Format("SELECT WORK_TYPE ");
                sql1 += string.Format("      ,WORK_TEAM ");
                sql1 += string.Format("FROM TB_LINE_WORK_TEAM ");
                sql1 += string.Format("WHERE LINE_GP = '{0}' ", line_gp);

                moddt = cd.FindDataTable(sql1);
                if (moddt.Rows.Count > 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                    uC_WorkShow1.WorkType = moddt.Rows[0]["WORK_TYPE"].ToString() + "근";
                    uC_WorkShow1.WorkTeam = moddt.Rows[0]["WORK_TEAM"].ToString() + "조";

                    //uC_WorkShow1.WorkTeam = "A조";
                    //uC_WorkShow1.WorkType = "1근";

                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
            catch (Exception)
            {


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
            //각 위치에 데이터 입력한다. 

            SetDataBinding_grdMain10();
        }
        private void SetDataBinding_grdMain1()
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT A.TOP_ROLL_POS_ACTV       ");//--상롤위치
                sql1 += string.Format("      ,A.TOP_ROLL_ANGLE_ACTV     ");//--상롤각도
                sql1 += string.Format("      ,A.BOT_ROLL_ANGLE_ACTV     ");//--하롤각도
                sql1 += string.Format("      ,A.TOP_ROLL_MTR_RPM_ACTV   ");//--상롤모터(RPM)
                sql1 += string.Format("      ,A.BOT_ROLL_MTR_RPM_ACTV   ");//--하롤모터(RPM)
                sql1 += string.Format("FROM   TB_STR_OPERINFO_NO3 A ");
                sql1 += string.Format("      ,TB_CR_INPUT_WR B ");
                sql1 += string.Format("WHERE  WORK_DDTT = (SELECT MAX(WORK_DDTT) FROM   TB_STR_OPERINFO_NO3) ");
                sql1 += string.Format("AND    A.MILL_NO    = B.MILL_NO ");
                sql1 += string.Format("AND    B.POC_NO     = ( SELECT MAX(POC_NO)  ");
                sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
                sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
                sql1 += string.Format("                        AND    LINE_GP     = '{0}' ) ", line_gp);


                moddt = cd.FindDataTable(sql1);
                if (moddt.Rows.Count > 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                    grdMain1[1, 1] = moddt.Rows[0]["TOP_ROLL_POS_ACTV"];
                    grdMain1[2, 1] = moddt.Rows[0]["TOP_ROLL_ANGLE_ACTV"];
                    grdMain1[3, 1] = moddt.Rows[0]["BOT_ROLL_ANGLE_ACTV"];
                    grdMain1[4, 1] = moddt.Rows[0]["TOP_ROLL_MTR_RPM_ACTV"];
                    grdMain1[5, 1] = moddt.Rows[0]["BOT_ROLL_MTR_RPM_ACTV"];
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }

                //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + "test".ToString() + "]");
            }
            catch (Exception ex)
            {

                //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + ex.ToString() + "]");
                return;
            }


            // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
            //grdMain_Row_Selected(grdMain.Row);
            grdMain1.Row = -1;
            return;
        }
        private void SetDataBinding_grdMain2()
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT A.SCREW_FEED_HZ_ACTV_1    ");   //--#1스크류(Hz)
                sql1 += string.Format("      ,A.SERVO_ACTV_1            ");   //--#1서보
                sql1 += string.Format("      ,A.SCREW_FEED_HZ_ACTV_2    ");   //--#2TMZMFB(Hz)
                sql1 += string.Format("      ,A.SERVO_ACTV_2            ");   //--#2서보
                sql1 += string.Format("FROM   TB_CHF_OPERINFO_NO3 A ");
                sql1 += string.Format("      ,TB_CR_INPUT_WR B ");
                sql1 += string.Format("WHERE  WORK_DDTT = (SELECT MAX(WORK_DDTT) FROM   TB_CHF_OPERINFO_NO3) ");
                sql1 += string.Format("AND    A.MILL_NO    = B.MILL_NO ");
                sql1 += string.Format("AND    B.POC_NO     = ( SELECT MAX(POC_NO)  ");
                sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
                sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
                sql1 += string.Format("                        AND    LINE_GP     = '{0}' ) ", line_gp);

                moddt = cd.FindDataTable(sql1);
                if (moddt.Rows.Count > 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                    grdMain2[1, 1] = moddt.Rows[0]["SCREW_FEED_HZ_ACTV_1"];
                    grdMain2[2, 1] = moddt.Rows[0]["SERVO_ACTV_1"];
                    grdMain2[3, 1] = moddt.Rows[0]["SCREW_FEED_HZ_ACTV_2"];
                    grdMain2[4, 1] = moddt.Rows[0]["SERVO_ACTV_2"];

                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }


            }
            catch (Exception ex)
            {

                //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + ex.ToString() + "]");
                return;
            }


            // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
            //grdMain_Row_Selected(grdMain.Row);
            grdMain2.Row = -1;
            return;
        }
        private void SetDataBinding_grdMain3()
        {
            try
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
                sql1 += string.Format("SELECT MILL_NO ");
                sql1 += string.Format("      ,PIECE_NO ");
                sql1 += string.Format("      ,(SELECT GOOD_YN FROM TB_CR_PIECE_WR ");
                sql1 += string.Format("        WHERE MILL_NO = A.MILL_NO AND PIECE_NO   = A.PIECE_NO ");
                sql1 += string.Format("        AND   LINE_GP = A.LINE_GP AND ROUTING_CD = 'D2' AND REWORK_SEQ = A.REWORK_SEQ) AS PRII_GOOD_NG ");// D2: PRII
                sql1 += string.Format("      ,MAT_GOOD_NG ");
                sql1 += string.Format("      ,MLFT_GOOD_NG ");
                sql1 += string.Format("      ,UT_GOOD_NG ");
                sql1 += string.Format("FROM   TB_CR_PIECE_WR A ");
                sql1 += string.Format("WHERE  LINE_GP     = :P_LINE_GP ");
                sql1 += string.Format("AND    ROUTING_CD  = 'F2' "); // NDT
                sql1 += string.Format("AND    EXIT_DDTT   = (SELECT MAX(EXIT_DDTT) FROM TB_CR_PIECE_WR ");
                sql1 += string.Format("                      WHERE  LINE_GP    = A.LINE_GP ");
                sql1 += string.Format("                      AND    ROUTING_CD = A.ROUTING_CD) ");
                sql1 += string.Format("AND    A.POC_NO     = ( SELECT MAX(POC_NO)  ");
                sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
                sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
                sql1 += string.Format("                        AND    LINE_GP     = A.LINE_GP ) ");
                sql1 += string.Format("AND    ROWNUM      = 1 ");
                sql1 += string.Format(") X ");

                string[] parm = new string[1];
                parm[0] = ":P_LINE_GP|" + line_gp;

                moddt = cd.FindDataTable(sql1, parm);


                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                if (moddt.Rows.Count > 0)
                {
                    //grdMain3[1, 1] = moddt.Rows[0]["PRII_GOOD_NG"];
                    //grdMain3[2, 1] = moddt.Rows[0]["MAT_GOOD_NG"];
                    //grdMain3[3, 1] = moddt.Rows[0]["MLFT_GOOD_NG"];
                    //grdMain3[4, 1] = moddt.Rows[0]["UT_GOOD_NG"];
                    ////grdMain3[1, 2] = grdMain3[2, 2] = grdMain3[3, 2] = grdMain3[4, 2]  = moddt.Rows[0]["PIECE_NO"];
                    //grdMain3[1, 2] = grdMain3[2, 2] = grdMain3[3, 2] = grdMain3[4, 2] = moddt.Rows[0]["PIECE_NO"];

                    //grdMain3[1, 1] = moddt.Rows[0]["PRII_GOOD_NG"];
                    grdMain3[1, 1] = moddt.Rows[0]["MAT_GOOD_NG"];
                    grdMain3[2, 1] = moddt.Rows[0]["MLFT_GOOD_NG"];
                    grdMain3[3, 1] = moddt.Rows[0]["UT_GOOD_NG"];
                    //grdMain3[1, 2] = grdMain3[2, 2] = grdMain3[3, 2] = grdMain3[4, 2]  = moddt.Rows[0]["PIECE_NO"];
                    grdMain3[1, 2] = grdMain3[2, 2] = grdMain3[3, 2] = moddt.Rows[0]["PIECE_NO"];

                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
                //sql1 += string.Format("SELECT MILL_NO ");
                //sql1 += string.Format("      ,PIECE_NO ");
                //sql1 += string.Format("      ,(SELECT GOOD_YN FROM TB_CR_PIECE_WR ");
                //sql1 += string.Format("        WHERE MILL_NO = A.MILL_NO AND PIECE_NO   = A.PIECE_NO ");
                //sql1 += string.Format("        AND   LINE_GP = A.LINE_GP AND ROUTING_CD = 'D2' AND REWORK_SEQ = A.REWORK_SEQ) AS PRII_GOOD_NG "); 
                //sql1 += string.Format("      ,MAT_GOOD_NG ");
                //sql1 += string.Format("      ,MLFT_GOOD_NG ");
                //sql1 += string.Format("      ,UT_GOOD_NG ");
                //sql1 += string.Format("FROM   TB_CR_PIECE_WR A ");
                //sql1 += string.Format("WHERE  LINE_GP     = '{0}' ", line_gp);
                //sql1 += string.Format("AND    ROUTING_CD  = 'F2' ");
                //sql1 += string.Format("AND    EXIT_DDTT   = (SELECT MAX(EXIT_DDTT) FROM TB_CR_PIECE_WR ");
                //sql1 += string.Format("                      WHERE  LINE_GP    = A.LINE_GP ");
                //sql1 += string.Format("                      AND    ROUTING_CD = A.ROUTING_CD) ");
                //sql1 += string.Format("AND    ROWNUM      = 1 ");

                //moddt = cd.FindDataTable(sql1);
                //this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                //if (moddt.Rows.Count > 0)
                //{

                //    grdMain3[1, 1] = moddt.Rows[0]["PRII_GOOD_NG"];
                //    grdMain3[2, 1] = moddt.Rows[0]["MAT_GOOD_NG"];
                //    grdMain3[3, 1] = moddt.Rows[0]["MLFT_GOOD_NG"];
                //    grdMain3[4, 1] = moddt.Rows[0]["UT_GOOD_NG"];
                //    //grdMain3[1, 2] = grdMain3[2, 2] = grdMain3[3, 2] = grdMain3[4, 2]  = moddt.Rows[0]["PIECE_NO"];
                //    grdMain3[1, 2] = grdMain3[2, 2] = grdMain3[3, 2] = grdMain3[4, 2] = moddt.Rows[0]["PIECE_NO"];

                //    this.Cursor = System.Windows.Forms.Cursors.Default;
                //}

            }
            catch (Exception ex)
            {

                //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + ex.ToString() + "]");
                return;
            }

            // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
            //grdMain_Row_Selected(grdMain.Row);

            //for (int i = 0; i < grdMain3.Rows.Count; i++)
            //{
            //    grdMain3.Rows[i].AllowMerging = true;
            //}

            grdMain3.Row = -1;
            return;
        }
        private void SetDataBinding_grdMain4()
        {
            try
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
                sql1 += string.Format("SELECT MAX(DECODE(ROUTING_CD,'H2','MPI')   ) AS MPI_INSP_TIT ");
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
                sql1 += string.Format("AND    EXIT_DDTT   = (SELECT MAX(EXIT_DDTT) FROM TB_CR_PIECE_WR ");
                sql1 += string.Format("                      WHERE  LINE_GP    = A.LINE_GP ");
                sql1 += string.Format("                      AND    ROUTING_CD = A.ROUTING_CD) ");
                sql1 += string.Format("AND    A.POC_NO     = ( SELECT MAX(POC_NO)  ");
                sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
                sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
                sql1 += string.Format("                        AND    LINE_GP     = A.LINE_GP ) ");
                sql1 += string.Format(") X ");

                string[] parm = new string[1];
                parm[0] = ":P_LINE_GP|" + line_gp;

                moddt = cd.FindDataTable(sql1, parm);

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                if (moddt.Rows.Count > 0)
                {

                    grdMain4[1, 1] = moddt.Rows[0]["MPI_GOOD_NG"];
                    grdMain4[2, 1] = moddt.Rows[0]["GR_GOOD_NG"];
                    grdMain4[1, 2] = moddt.Rows[0]["MPI_PIECE_NO"];
                    grdMain4[2, 2] = moddt.Rows[0]["GR_PIECE_NO"];
                    //grdMain4[1, 2] = grdMain4[2, 2] = grdMain4[3, 2] = grdMain4[4, 2]  = moddt.Rows[0]["PIECE_NO"];
                    //grdMain4[1, 2] = grdMain4[2, 2] = grdMain4[3, 2] = grdMain4[4, 2] = moddt.Rows[0]["PIECE_NO"];

                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }

            }
            catch (Exception ex)
            {

                //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + ex.ToString() + "]");
                return;
            }

            // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
            //grdMain_Row_Selected(grdMain.Row);

            for (int i = 0; i < grdMain4.Rows.Count; i++)
            {
                grdMain4.Rows[i].AllowMerging = true;
            }

            grdMain4.Row = -1;
            return;
        }

        private void SetDataBinding_grdMain5_1()
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT BUNDLE_NO ");
                sql1 += string.Format("FROM   TB_CR_PIECE_WR  A ");
                sql1 += string.Format("WHERE  LINE_GP     = :P_LINE_GP ");
                sql1 += string.Format("AND    ROUTING_CD  = 'M3' ");
                sql1 += string.Format("AND    ENTRY_DDTT    = (SELECT MAX(ENTRY_DDTT)  FROM TB_CR_PIECE_WR ");
                sql1 += string.Format("                      WHERE  LINE_GP     = A.LINE_GP ");
                sql1 += string.Format("                      AND    ROUTING_CD  = 'M3' ) ");
                sql1 += string.Format("AND    A.POC_NO     = ( SELECT MAX(POC_NO)  ");
                sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
                sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
                sql1 += string.Format("                        AND    LINE_GP     = A.LINE_GP ) ");
                sql1 += string.Format("AND    ROWNUM      = 1 ");

                string[] parm = new string[1];
                parm[0] = ":P_LINE_GP|" + line_gp;

                moddt = cd.FindDataTable(sql1, parm);
                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                if (moddt.Rows.Count > 0)
                {

                    grdMain5[2, 1] = grdMain5[2, 2] = moddt.Rows[0]["BUNDLE_NO"];// 라벨
                    //grdMain5[1, 2] = moddt.Rows[0]["PIECE_NO"];
                    //grdMain5[3, 1] = moddt.Rows[0]["MLFT_GOOD_NG"];
                    //grdMain5[4, 1] = moddt.Rows[0]["UT_GOOD_NG"];
                    //grdMain5[1, 2] = grdMain5[2, 2] = grdMain5[3, 2] = grdMain5[4, 2]  = moddt.Rows[0]["PIECE_NO"];
                    //grdMain5[1, 2] = grdMain5[2, 2] = grdMain5[3, 2] = grdMain5[4, 2] = moddt.Rows[0]["PIECE_NO"];
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }
            catch (Exception ex)
            {

                //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + ex.ToString() + "]");
                return;
            }


            grdMain5.Row = -1;
            return;
        }

        private void SetDataBinding_grdMain5()
        {
            try
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
                sql1 += string.Format("AND    B.POC_NO     = ( SELECT MAX(POC_NO)  ");
                sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
                sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
                sql1 += string.Format("                        AND    LINE_GP     = A.LINE_GP ) ");
                sql1 += string.Format("AND    ROWNUM      = 1 ");
                sql1 += string.Format(") X ");

                string[] parm = new string[1];
                parm[0] = ":P_LINE_GP|" + line_gp;

                moddt = cd.FindDataTable(sql1, parm);


                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                if (moddt.Rows.Count > 0)
                {

                    grdMain5[1, 1] = moddt.Rows[0]["CHM_GOOD_NG"];
                    grdMain5[1, 2] = moddt.Rows[0]["PIECE_NO"];
                    //grdMain5[3, 1] = moddt.Rows[0]["MLFT_GOOD_NG"];
                    //grdMain5[4, 1] = moddt.Rows[0]["UT_GOOD_NG"];
                    //grdMain5[1, 2] = grdMain5[2, 2] = grdMain5[3, 2] = grdMain5[4, 2]  = moddt.Rows[0]["PIECE_NO"];
                    //grdMain5[1, 2] = grdMain5[2, 2] = grdMain5[3, 2] = grdMain5[4, 2] = moddt.Rows[0]["PIECE_NO"];
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }
            catch (Exception ex)
            {

                //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + ex.ToString() + "]");
                return;
            }


            grdMain5.Row = -1;
            return;
        }


        //private void SetDataBinding_grdMain6_1()
        //{
        //    try
        //    {
        //        string sql1 = string.Empty;
        //        sql1 += string.Format("SELECT BUNDLE_NO ");
        //        sql1 += string.Format("      ,NET_WGT ");
        //        sql1 += string.Format("FROM   TB_WGT_WR  A ");
        //        sql1 += string.Format("WHERE  LINE_GP     = '{0}' ", line_gp);
        //        sql1 += string.Format("AND    REG_DDTT   = (SELECT MAX(REG_DDTT)  FROM TB_WGT_WR ");
        //        sql1 += string.Format("                     WHERE  LINE_GP   = A.LINE_GP) ");
        //        sql1 += string.Format("AND    NVL(DEL_YN,'N') <> 'Y' ");
        //        sql1 += string.Format("AND    ROWNUM      = 1 ");

        //        moddt = cd.FindDataTable(sql1);
        //        this.Cursor = System.Windows.Forms.Cursors.AppStarting;
        //        if (moddt.Rows.Count > 0)
        //        {
        //            grdMain6_1.SetDataBinding(moddt, null, true);

        //            //grdMain6_1[1, 0] = moddt.Rows[0]["BUNDLE_NO"];
        //            //grdMain6_1[1, 1] = moddt.Rows[0]["NET_WGT"];
        //            //grdMain6_1[1, 2] = moddt.Rows[0]["PIECE_NO"];
        //            //grdMain6_1[3, 1] = moddt.Rows[0]["MLFT_GOOD_NG"];
        //            //grdMain6_1[4, 1] = moddt.Rows[0]["UT_GOOD_NG"];
        //            //grdMain6_1[1, 2] = grdMain6_1[2, 2] = grdMain6_1[3, 2] = grdMain6_1[4, 2]  = moddt.Rows[0]["PIECE_NO"];
        //            //grdMain6_1[1, 2] = grdMain6_1[2, 2] = grdMain6_1[3, 2] = grdMain6_1[4, 2] = moddt.Rows[0]["PIECE_NO"];
        //        }
        //        this.Cursor = System.Windows.Forms.Cursors.Default;

        //    }
        //    catch (Exception ex)
        //    {

        //        //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + ex.ToString() + "]");
        //        return;
        //    }


        //    grdMain6_1.Row = -1;
        //    return;
        //}

        private void SetDataBinding_grdMain6()
        {
            try
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
                sql1 += string.Format("AND    A.POC_NO     = ( SELECT MAX(POC_NO)  ");
                sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
                sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
                sql1 += string.Format("                        AND    LINE_GP     = A.LINE_GP ) ");
                sql1 += string.Format("AND    NVL(A.DEL_YN,'N') <> 'Y' ");
                sql1 += string.Format("AND    ROWNUM      = 1 ");

                moddt = cd.FindDataTable(sql1);
                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                if (moddt.Rows.Count > 0)
                {

                    grdMain6.SetDataBinding(moddt, null, true);

                    //grdMain6[1, 0] = moddt.Rows[0]["BUNDLE_NO"];
                    //grdMain6[1, 1] = moddt.Rows[0]["PCS"];
                    //grdMain6[1, 2] = moddt.Rows[0]["PIECE_NO"];
                    //grdMain6[3, 1] = moddt.Rows[0]["MLFT_GOOD_NG"];
                    //grdMain6[4, 1] = moddt.Rows[0]["UT_GOOD_NG"];
                    //grdMain6[1, 2] = grdMain6[2, 2] = grdMain6[3, 2] = grdMain6[4, 2]  = moddt.Rows[0]["PIECE_NO"];
                    //grdMain6[1, 2] = grdMain6[2, 2] = grdMain6[3, 2] = grdMain6[4, 2] = moddt.Rows[0]["PIECE_NO"];
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }
            catch (Exception ex)
            {

                //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + ex.ToString() + "]");
                return;
            }


            grdMain6.Row = -1;
            return;
        }

        private void SetDataBinding_grdMain7()
        {
            try
            {
                string sql1 = string.Empty;
                sql1 = cd.GetReadyBundlesql();

                //moddt = new DataTable();
                string[] parm = new string[1];
                parm[0] = ":P_LINE_GP|" + line_gp;

                moddt = cd.FindDataTable(sql1, parm);

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain7.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {

                //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + ex.ToString() + "]");
                return;
            }


            grdMain7.Row = -1;
            return;
        }
        private void SetDataBinding_grdMain8()
        {
            try
            {
                string sql1 = string.Empty;
                //--실시간트레킹중간_POC정보 공통쿼리를 가져온다
                sql1 = cd.GetPOCsql();

                //moddt = new DataTable();

                string[] parm = new string[1];
                parm[0] = ":P_LINE_GP|" + line_gp;

                moddt = cd.FindDataTable(sql1, parm);


                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain8.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {

                //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + ex.ToString() + "]");
                return;
            }


            grdMain8.Row = -1;
            return;
        }
        private void SetDataBinding_grdMain9()
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT X.MILL_NO ");
                sql1 += string.Format("      ,NVL(Y.PCS, 0) AS MILL_PCS ");
                sql1 += string.Format("      ,PRII_OK_PCS ");
                sql1 += string.Format("      ,PRII_NG_PCS ");
                sql1 += string.Format("      ,MAT_NG_PCS ");
                sql1 += string.Format("      ,MLFT_NG_PCS ");
                sql1 += string.Format("      ,UT_NG_PCS ");
                sql1 += string.Format("      ,NDT_NG_PCS ");
                sql1 += string.Format("      ,MPI_OK_PCS ");
                sql1 += string.Format("      ,MPI_NG_PCS ");
                sql1 += string.Format("      ,GR_OK_PCS ");
                sql1 += string.Format("      ,GR_NG_PCS ");
                sql1 += string.Format("FROM   TB_CR_ORD_BUNDLEINFO Y ");
                sql1 += string.Format("      ,(SELECT MILL_NO ");
                sql1 += string.Format("              ,NVL(SUM(DECODE(A.ROUTING_CD,'D2',DECODE(A.GOOD_YN,'OK',1,0))     ), 0) AS PRII_OK_PCS ");
                sql1 += string.Format("              ,NVL(SUM(DECODE(A.ROUTING_CD,'D2',DECODE(A.GOOD_YN,'NG',1,0))     ), 0) AS PRII_NG_PCS ");
                sql1 += string.Format("              ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',DECODE(A.MAT_GOOD_NG,'NG',1,0)) ), 0) AS MAT_NG_PCS ");
                //sql1 += string.Format("              ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',DECODE(A.MLFT_GOOD_NG,'NG',1,0))), 0) AS MLFT_NG_PCS ");
                sql1 += string.Format("              ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',DECODE(A.MLFT_GOOD_NG,'NG',DECODE(A.MAT_GOOD_NG,'NG',0,DECODE(A.UT_GOOD_NG,'NG',0,1)),0)) ), 0)  AS MLFT_NG_PCS ");
                //sql1 += string.Format("              ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',DECODE(A.UT_GOOD_NG,'NG',1,0))  ), 0) AS UT_NG_PCS ");
                sql1 += string.Format("              ,NVL(SUM(DECODE(A.ROUTING_CD,'F2',DECODE(A.UT_GOOD_NG,'NG',DECODE(A.MAT_GOOD_NG,'NG',0,1),0))  ), 0) AS UT_NG_PCS ");
                sql1 += string.Format("              ,NVL(SUM(CASE WHEN A.MAT_GOOD_NG = 'NG' OR A.MLFT_GOOD_NG = 'NG' OR A.UT_GOOD_NG = 'NG' THEN 1 ");
                sql1 += string.Format("                    ELSE 0  END), 0) AS NDT_NG_PCS ");
                sql1 += string.Format("              ,NVL(SUM(DECODE(A.ROUTING_CD,'H2',DECODE(A.MPI_INSP_GOOD_NG,'OK',1,0))), 0) AS MPI_OK_PCS ");
                sql1 += string.Format("              ,NVL(SUM(DECODE(A.ROUTING_CD,'H2',DECODE(A.MPI_INSP_GOOD_NG,'NG',1,0))), 0) AS MPI_NG_PCS ");
                sql1 += string.Format("              ,NVL(SUM(DECODE(A.ROUTING_CD,'K2',DECODE(A.GOOD_YN,'OK',1,0))         ), 0) AS GR_OK_PCS ");
                sql1 += string.Format("              ,NVL(SUM(DECODE(A.ROUTING_CD,'K2',DECODE(A.GOOD_YN,'NG',1,0))         ), 0) AS GR_NG_PCS ");
                sql1 += string.Format("              ,MAX(REG_DDTT)  AS REG_DDTT ");
                sql1 += string.Format("        FROM   TB_CR_PIECE_WR A ");
                sql1 += string.Format("              ,(SELECT DISTINCT POC_NO AS POC_NO ");
                sql1 += string.Format("                FROM   TB_RL_TM_TRACKING ");
                sql1 += string.Format("                WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
                sql1 += string.Format("                AND    LINE_GP     =  :P_LINE_GP ");
                sql1 += string.Format("                AND    ROWNUM      = 1 ) B ");
                sql1 += string.Format("        WHERE  A.LINE_GP     =  :P_LINE_GP ");
                //sql1 += string.Format("        --AND    A.MFG_DATE    >  TO_CHAR(SYSDATE-1,'YYYYMMDD')  --테스트기간동안COMMENT ");
                sql1 += string.Format("        AND    A.POC_NO      = B.POC_NO ");
                sql1 += string.Format("        AND    A.REWORK_SEQ  = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
                sql1 += string.Format("                                WHERE  MILL_NO    = A.MILL_NO ");
                sql1 += string.Format("                                AND    PIECE_NO   = A.PIECE_NO ");
                sql1 += string.Format("                                AND    LINE_GP    = A.LINE_GP ");
                sql1 += string.Format("                                AND    ROUTING_CD = A.ROUTING_CD) ");
                sql1 += string.Format("        AND    A.ROUTING_CD  IN ('D2','F2','H2','K2') ");
                sql1 += string.Format("        GROUP BY MILL_NO ");
                sql1 += string.Format("        ORDER BY REG_DDTT ) X ");
                sql1 += string.Format("WHERE  X.MILL_NO  = Y.MILL_NO ");
                sql1 += string.Format("AND    ROWNUM < 5 ");

                //moddt = new DataTable();
                string[] parm = new string[1];
                parm[0] = ":P_LINE_GP|" + line_gp;

                moddt = cd.FindDataTable(sql1, parm);



                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain9.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {

                //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + ex.ToString() + "]");
                return;
            }


            // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
            //grdMain_Row_Selected(grdMain.Row);
            grdMain9.Row = -1;
            return;


        }

        private void SetDataBinding_grdMain10()
        {
            try
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
                sql1 += string.Format("    WHERE  A.LINE_GP  =  :P_LINE_GP ");
                sql1 += string.Format("    AND    NVL(A.DEL_YN,'N') <> 'Y' ");
                sql1 += string.Format("    AND    A.POC_NO = ( SELECT DISTINCT POC_NO AS POC_NO ");
                sql1 += string.Format("                        FROM   TB_RL_TM_TRACKING ");
                sql1 += string.Format("                        WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
                sql1 += string.Format("                        AND    LINE_GP     =  :P_LINE_GP ");
                sql1 += string.Format("                        AND    ROWNUM      = 1 ) ");
                sql1 += string.Format("    ORDER BY REG_DDTT DESC ");
                sql1 += string.Format(") X ");
                sql1 += string.Format("WHERE ROWNUM < 6 ");


                //moddt = new DataTable();
                string[] parm = new string[1];
                parm[0] = ":P_LINE_GP|" + line_gp;

                moddt = cd.FindDataTable(sql1, parm);

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain10.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {

                //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + ex.ToString() + "]");
                return;
            }



            // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
            //grdMain_Row_Selected(grdMain.Row);
            grdMain10.Row = -1;
            return;
        }


        private bool SetDataBinding_Zone()
        {
            try
            {
                string sql1 = string.Empty;

                sql1 = cd.GetZoneDatasql();

                //moddt = new DataTable();

                string[] parm = new string[1];
                parm[0] = ":P_LINE_GP|" + line_gp;

                moddt = cd.FindDataTable(sql1, parm);

                foreach (var item in vf.GetAllChildrens(this))
                {
                    if (item.GetType().ToString() == "SystemControlClassLibrary.UC.UC_Zone")
                    {
                        var uc_zone = item as UC_Zone;
                        //zone 초기화
                        uc_zone.PCS = "";
                        uc_zone.MillNo = "";

                        uc_zone.ZoneForeColor = Color.DarkBlue;

                        uc_zone.ZoneBackColor = Color.FromArgb(218, 233, 245);

                        //Color setup1_BackColor = Color.FromArgb(218, 233, 245);

                        //Color setup1_ForeColor = Color.DarkGray;

                        foreach (DataRow row in moddt.Rows)
                        {
                            if (uc_zone.ZoneCD == row["ZONE_CD"].ToString())
                            {

                                if (uc_zone.ZoneCD == "3Z38" || uc_zone.ZoneCD == "3Z39")
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
            catch (Exception ex)
            {

                //clsMsg.Log.Alarm(Alarms.Modified, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "[" + ex.ToString() + "]");
                return false;
            }
            return true;

        }
        #endregion grid SetDataBinding

        #region 버튼이벤트
        private void btnInsertReg_Click(object sender, EventArgs e)
        {


            //CrtInRsltInqPopUP popup = new CrtInRsltInqPopUP(this as MyInterface);
            CrtInRsltPopUP popup = new CrtInRsltPopUP(line_gp);
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.Show();
            //popup.StartPosition = FormStartPosition.Manual;
            //popup.Location = new Point(600, 300);
        }

        private void btnPOCFin_Click(object sender, EventArgs e)
        {
            POCFinReg popup = new POCFinReg(line_gp, "");
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.Show();
        }

        private void grdMain2_Click(object sender, EventArgs e)
        {

        }


        private void btnReWork_Click(object sender, EventArgs e)
        {
            ProdReWorkMgmt popup = new ProdReWorkMgmt(line_gp);
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.Show();
        }

        #endregion

        private void btnRsltCancel_Click(object sender, EventArgs e)
        {
            // 현재 grdMain8의 행이 있다면, 첫행의 poc no를 가져와 초기검색에 사용

            string poc_No = string.Empty;

            if (grdMain8.Rows.Count > 1)
            {
                poc_No = grdMain8.GetData(1, "POC_NO").ToString().Trim();
            }

            WorkRsltCancel popup = new WorkRsltCancel(line_gp, poc_No);
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.Show();
        }
        //bool toggled = false;
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            timer1_Tick(null, null); 


            //Color setup1_BackColor = Color.FromArgb(218, 233, 245);

            //Color setup1_ForeColor = Color.DarkGray;

            //Color setup2_BackColor = Color.FromArgb(0, 122, 204);

            //Color setup2_ForeColor = Color.White;


            //if (toggled)
            //{
            //    foreach (var item in vf.GetAllChildrens(this))
            //    {
            //        if (item.GetType().ToString() == "SystemControlClassLibrary.UC.UC_Zone")
            //        {
            //            var uc_zone = item as UC_Zone;
            //            //zone 초기화
            //            uc_zone.ZoneForeColor = setup1_ForeColor;
            //            uc_zone.ZoneBackColor = setup1_BackColor;

            //        }
            //    }
            //    toggled = false;
            //}
            //else
            //{
            //    foreach (var item in vf.GetAllChildrens(this))
            //    {
            //        if (item.GetType().ToString() == "SystemControlClassLibrary.UC.UC_Zone")
            //        {
            //            var uc_zone = item as UC_Zone;
            //            //zone 초기화
            //            uc_zone.ZoneForeColor = setup2_ForeColor;
            //            uc_zone.ZoneBackColor = setup2_BackColor;

            //        }
            //    }
            //    toggled = true;
            //}

        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {

        }
    }



}