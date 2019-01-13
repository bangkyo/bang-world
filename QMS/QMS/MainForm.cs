using System;
using System.Drawing;
using System.IO;
using System.Data;
using System.Data.OracleClient;
using System.Reflection;
using System.Windows.Forms;
using ComLib;
using ComLib.clsMgr;
using System.Collections;
using System.Collections.Generic;
using QMS;
using SystemControlClassLibrary.monitoring;
using System.Linq;
using System.Windows.Documents;

namespace QMS
{
    #region 참조사항
    // 화면 Design (프로그램을 추가할 경우)
    // 1. 메뉴Item명 -> "tsm" + 프로그램(페이지)ID
    // 2. 하위Item이 있는 상위Item의 Tag -> "X" 입력
    // 3. 하위Item의 Tag -> 프로그램ID, 조회권한, 등록권한, 수정권한, 삭제권한 -> 실행시 자동입력
    // 4. 버튼명은 표준화(통일): btnDisplay, btnSave, btnRowAdd, btnRowAdd1, btnRowAdd2, btnDelRow , btnDelRow1, btnDelRow2, btnReg
    #endregion

    public partial class MainForm : Form
    {
        #region 변수선언
        private clsStyle cs = new clsStyle();
        private clsCom ck = new clsCom();
        private ConnectDB cd = new ConnectDB();

        private int MenuCnt = 0;
        private string[,] MenuArr;

        private ToolStripMenuItem tsmWindow;
        private ToolStripMenuItem tsmCascade;
        private ToolStripMenuItem tsmTileHorizontal;
        private ToolStripMenuItem tsmTileVertical;
        private ToolStripMenuItem tsmArrangeIcons;
        private ToolStripMenuItem tsmCloseAll;

        private ToolStripMenuItem tsmInformation;
        private ToolStripMenuItem tsmPassword;
        private ToolStripMenuItem tsmAbout;

        // InqAcl    : 등록권한
        // RegAcl    : 등록권한
        // ModAcl    : 수정권한
        // DelAcl    : 삭제권한
        List<string> buttonListINInqAcl;
        List<string> buttonListINRegAcl;
        List<string> buttonListINModAcl;
        List<string> buttonListINDelAcl;

        private List<Form> childFormList;
        #endregion

        #region 화면
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            childFormList = new List<Form>();
            // 초기 메모리 (베이스)
            long mem = GC.GetTotalMemory(false);
            Console.WriteLine("Initial Memory: {0}", mem);

            InitACL();

            cs.InitMsgLabel(msg_lb);

            msgshow_lb.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall, FontStyle.Bold);
            lblDateTime.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall, FontStyle.Bold);

            clsMsg.Log.Msglabel = msgshow_lb;
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "프로그램을 시작합니다.");

            //var panel = new Panel();
            mnuMain.Renderer = new MyRenderer();

            InitControls();
            this.Text = "공정품질분석 시스템 [" + ck.UserNm + "]";

            Timer_Set();

            InitMenu();

            // 함수 호출후 메모리
            mem = GC.GetTotalMemory(false);
            Console.WriteLine("Current Memory: {0}", mem);

            // 메모리 Clean Up 
            GC.Collect();
            //System.Threading.Thread.Sleep(5000);

            // 메모리 Clean Up 후 
            mem = GC.GetTotalMemory(false);
            Console.WriteLine("After GC Memory: {0}", mem);

            //Console.ReadKey();
        }

        private void SetupBackgroudImage(Image _backgroundimage)
        {
            foreach (Control ctl in this.Controls)
            {
                if (ctl is MdiClient)
                {
                    ctl.BackgroundImage = _backgroundimage;
                    break;
                }
            }
        }



        private void InitACL()
        {

            buttonListINInqAcl = new List<string>();
            buttonListINRegAcl = new List<string>();
            buttonListINModAcl = new List<string>();
            buttonListINDelAcl = new List<string>();


            //조회
            buttonListINInqAcl.Add("btnDisplay");


            //등록
            buttonListINRegAcl.Add("btnSave");
            buttonListINRegAcl.Add("btnReg");
            buttonListINRegAcl.Add("btnPOCFin");
            buttonListINRegAcl.Add("btnApply");
            buttonListINRegAcl.Add("btnApplyCancel");
            buttonListINRegAcl.Add("btnPLCSend");
            buttonListINRegAcl.Add("btnBNDSend");



            //수정
            buttonListINModAcl.Add("btnRowAdd");
            buttonListINModAcl.Add("btnRowAdd1");
            buttonListINModAcl.Add("btnRowAdd2");

            //삭제
            buttonListINDelAcl.Add("btnDelRow");
            buttonListINDelAcl.Add("btnDelRow1");
            buttonListINDelAcl.Add("btnDelRow2");
            buttonListINDelAcl.Add("btnDelRow2");
            buttonListINDelAcl.Add("btnCancel");


        }

        private bool IsItem(List<string> list, string itemName)
        {
            foreach (string item in list)
            {
                if (item == itemName)
                {
                    return true;
                }
            }

            return false;
        }

        private void InitMenu()
        {
            SetMenu();

            //하위Item Disable
            DisableItem();

            //하위Item Enable, 권한설정
            EnableItem();
        }

        private void MenuItemsRemove()
        {
            mnuMain.Items.Clear();
        }

        private void SetMenu()
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format(" SELECT ");
                sql1 += string.Format("       MENU_ID ");
                sql1 += string.Format("      ,MENU_NM ");
                sql1 += string.Format("      ,UP_MENU_ID ");
                sql1 += string.Format("      ,SCR_ID ");
                sql1 += string.Format("      ,MENU_SEQ ");
                sql1 += string.Format(" FROM ");
                sql1 += string.Format("     (");
                sql1 += string.Format("        select  A.MENU_ID    AS MENU_ID ");
                sql1 += string.Format("               ,A.MENU_NM    AS MENU_NM ");
                sql1 += string.Format("               ,A.UP_MENU_ID AS UP_MENU_ID ");
                sql1 += string.Format("               ,B.PAGE_ID    AS SCR_ID ");
                sql1 += string.Format("               ,A.MENU_SEQ   AS MENU_SEQ ");
                sql1 += string.Format("        from TB_CM_MENU A ");
                sql1 += string.Format("            ,TB_CM_SCR B ");
                sql1 += string.Format("        WHERE ");
                sql1 += string.Format("               A.SCR_ID = B.PAGE_ID(+) ");
                sql1 += string.Format("        AND    NVL(B.USE_YN, 'Y') = 'Y' ");
                sql1 += string.Format("        ORDER BY A.MENU_SEQ ");
                sql1 += string.Format("     ) ");


                DataTable dt = cd.FindDataTable(sql1);

                if (dt == null || dt.Rows.Count < 1)
                {
                    return;
                }

                int idx = 0;
                MenuCnt = dt.Rows.Count;
                MenuArr = new string[MenuCnt, 9];

                DataRow[] result = dt.Select("ISNULL(UP_MENU_ID, ' ') = ' '  AND ISNULL(SCR_ID, ' ') = ' '");
                foreach (DataRow item in result)
                {
                    ToolStripMenuItem tsmGrp = new ToolStripMenuItem(item["MENU_NM"].ToString(), null, tsmGroup_Click);
                    //tsmGrp.Name = "tsm" + row["MENU_NM"].ToString();
                    tsmGrp.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
                    tsmGrp.ForeColor = Color.White;
                    tsmGrp.Font = new Font(cs.OHeadfontName, cs.FontSizeMiddle, FontStyle.Bold);

                    mnuMain.Items.Add(tsmGrp);

                    string temp = string.Format("UP_MENU_ID = '{0}'", item["MENU_ID"].ToString());
                    DataRow[] result_sub = dt.Select(temp);
                    foreach (DataRow sub_item in result_sub)
                    {
                        ToolStripMenuItem sub_tsmGrp = new ToolStripMenuItem(sub_item["MENU_NM"].ToString(), null, tsmGroup_Click);
                        sub_tsmGrp.Name = "tsm" + sub_item["SCR_ID"].ToString();
                        sub_tsmGrp.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
                        sub_tsmGrp.ForeColor = Color.White;
                        sub_tsmGrp.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall, FontStyle.Bold);


                        string sub_temp = string.Format("UP_MENU_ID = '{0}'", sub_item["MENU_ID"].ToString());
                        DataRow[] result_sub2 = dt.Select(sub_temp);
                        //sub 아이템이 없는 경우 tag = X 로 입력해야 활성화 설정시 사용가능
                        if (result_sub2.Length > 0)
                        {
                            sub_tsmGrp.Tag = "X";
                        }
                        tsmGrp.DropDownItems.Add(sub_tsmGrp);

                        foreach (DataRow sub2_item in result_sub2)
                        {
                            ToolStripMenuItem sub2_tsmGrp = new ToolStripMenuItem(sub2_item["MENU_NM"].ToString(), null, tsmGroup_Click);
                            //tsmGrp.Name = "tsm" + row["MENU_NM"].ToString();
                            sub2_tsmGrp.Name = "tsm" + sub2_item["SCR_ID"].ToString();
                            sub2_tsmGrp.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
                            sub2_tsmGrp.ForeColor = Color.White;
                            sub2_tsmGrp.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall, FontStyle.Bold);

                            sub_tsmGrp.Tag = "X";
                            sub_tsmGrp.DropDownItems.Add(sub2_tsmGrp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("메뉴를 불러오던 중 오류가 발생하였습니다.\n" + ex.Message);
                return;
            }

            SetupWindowTool();

            SetupInfo();
        }

        private void SetupInfo()
        {
            tsmInformation = new ToolStripMenuItem();
            tsmAbout = new ToolStripMenuItem();
            tsmPassword = new ToolStripMenuItem();

            mnuMain.Items.AddRange(new ToolStripItem[] {
            tsmInformation});

            tsmInformation.DropDownItems.AddRange(new ToolStripItem[] {
            tsmPassword,
            tsmAbout
            });


            tsmInformation.Font = new Font(cs.OHeadfontName, cs.FontSizeMiddle, FontStyle.Bold);
            tsmInformation.ForeColor = Color.White;
            tsmInformation.Name = "tsmInformation";
            tsmInformation.Size = new Size(37, 23);
            tsmInformation.Tag = "X";
            tsmInformation.Text = "정보";

            tsmAbout.BackColor = Color.FromArgb(0, 122, 204);
            tsmAbout.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall, FontStyle.Bold, GraphicsUnit.Point, 129);
            tsmAbout.ForeColor = Color.White;
            tsmAbout.Name = "tsmAbout";
            tsmAbout.Image = global::QMS.Properties.Resources.info;
            tsmAbout.Size = new Size(176, 22);
            tsmAbout.Tag = "X";
            tsmAbout.Text = "프로그램 정보";
            tsmAbout.Click += TsmAbout_Click;

            tsmPassword.BackColor = Color.FromArgb(0, 122, 204);
            tsmPassword.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall, FontStyle.Bold, GraphicsUnit.Point, 129);
            tsmPassword.ForeColor = Color.White;
            tsmPassword.Name = "tsmPassword";
            tsmPassword.Image = global::QMS.Properties.Resources.password;
            tsmPassword.Size = new Size(176, 22);
            tsmPassword.Tag = "X";
            tsmPassword.Text = "비밀번호 변경";
            tsmPassword.Click += TsmPassword_Click;

        }

        private void TsmPassword_Click(object sender, EventArgs e)
        {
            Password Pwd = new Password();
            Pwd.StartPosition = FormStartPosition.CenterParent;
            Pwd.ShowDialog();
        }

        private void TsmAbout_Click(object sender, EventArgs e)
        {

            About about = new About();
            about.StartPosition = FormStartPosition.CenterParent;
            about.ShowDialog();
        }

        private void SetupWindowTool()
        {
            tsmWindow = new ToolStripMenuItem();
            tsmCloseAll = new ToolStripMenuItem();
            tsmCascade = new ToolStripMenuItem();
            tsmTileHorizontal = new ToolStripMenuItem();
            tsmTileVertical = new ToolStripMenuItem();
            tsmArrangeIcons = new ToolStripMenuItem();

            mnuMain.Items.AddRange(new ToolStripItem[] {
            tsmWindow});

            // 
            // tsmWindow
            // 
            tsmWindow.DropDownItems.AddRange(new ToolStripItem[] {
            tsmCloseAll,
            tsmCascade,
            tsmTileHorizontal,
            tsmTileVertical,
            tsmArrangeIcons});
            tsmWindow.Font = new Font("돋움", 12F, FontStyle.Bold);
            tsmWindow.ForeColor = Color.White;
            tsmWindow.Name = "tsmWindow";
            tsmWindow.Size = new Size(37, 23);
            tsmWindow.Tag = "X";
            tsmWindow.Text = "창";
            // 
            // tsmCloseAll
            // 
            tsmCloseAll.BackColor = Color.FromArgb(0, 122, 204);
            tsmCloseAll.Font = new Font("돋움", 11F, FontStyle.Bold, GraphicsUnit.Point, 129);
            tsmCloseAll.ForeColor = Color.White;
            tsmCloseAll.Image = global::QMS.Properties.Resources.Fatcow_Farm_Fresh_Application_delete;
            tsmCloseAll.Name = "tsmCloseAll";
            tsmCloseAll.Size = new Size(176, 22);
            tsmCloseAll.Tag = "X";
            tsmCloseAll.Text = "모든 창 닫기";
            tsmCloseAll.Click += new EventHandler(tsmWindow_Click);
            // 
            // tsmCascade
            // 
            tsmCascade.BackColor = Color.FromArgb(0, 122, 204);
            tsmCascade.Font = new Font("돋움", 11F, FontStyle.Bold, GraphicsUnit.Point, 129);
            tsmCascade.ForeColor = Color.White;
            tsmCascade.Image = global::QMS.Properties.Resources.Fatcow_Farm_Fresh_Application_cascade;
            tsmCascade.Name = "tsmCascade";
            tsmCascade.Size = new Size(176, 22);
            tsmCascade.Tag = "X";
            tsmCascade.Text = "계단식정렬";
            tsmCascade.Click += new EventHandler(tsmWindow_Click);
            // 
            // tsmTileHorizontal
            // 
            tsmTileHorizontal.BackColor = Color.FromArgb(0, 122, 204);
            tsmTileHorizontal.Font = new Font("돋움", 11F, FontStyle.Bold, GraphicsUnit.Point, 129);
            tsmTileHorizontal.ForeColor = Color.White;
            tsmTileHorizontal.Image = global::QMS.Properties.Resources.Fatcow_Farm_Fresh_Application_tile_vertical;
            tsmTileHorizontal.Name = "tsmTileHorizontal";
            tsmTileHorizontal.Size = new Size(176, 22);
            tsmTileHorizontal.Tag = "X";
            tsmTileHorizontal.Text = "수평배열";
            tsmTileHorizontal.Click += new EventHandler(tsmWindow_Click);
            // 
            // tsmTileVertical
            // 
            tsmTileVertical.BackColor = Color.FromArgb(0, 122, 204);
            tsmTileVertical.Font = new Font("돋움", 11F, FontStyle.Bold, GraphicsUnit.Point, 129);
            tsmTileVertical.ForeColor = Color.White;
            tsmTileVertical.Image = global::QMS.Properties.Resources.Fatcow_Farm_Fresh_Application_tile_horizontal3;
            tsmTileVertical.Name = "tsmTileVertical";
            tsmTileVertical.Size = new Size(176, 22);
            tsmTileVertical.Tag = "X";
            tsmTileVertical.Text = "수직배열";
            tsmTileVertical.Click += new EventHandler(tsmWindow_Click);
            // 
            // tsmArrangeIcons
            // 
            tsmArrangeIcons.BackColor = Color.FromArgb(0, 122, 204);
            tsmArrangeIcons.Font = new Font("돋움", 11F, FontStyle.Bold, GraphicsUnit.Point, 129);
            tsmArrangeIcons.ForeColor = Color.White;
            tsmArrangeIcons.Image = global::QMS.Properties.Resources.Fatcow_Farm_Fresh_Application_double;
            tsmArrangeIcons.Name = "tsmArrangeIcons";
            tsmArrangeIcons.Size = new Size(176, 22);
            tsmArrangeIcons.Tag = "X";
            tsmArrangeIcons.Text = "아이콘정렬";
            tsmArrangeIcons.Click += new EventHandler(tsmWindow_Click);
        }

        private void tsmGroup_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void InitControls()
        {
            mnuMain.Font = new Font(cs.OHeadfontName, cs.FontSizeMiddle, FontStyle.Bold);
            tsTop.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall);
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("종료 하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;

                //clsAdo.Ado.Disconnect();
            }
        }
        #endregion

        #region Timer
        private void Timer_Set()
        {
            var timer = new Timer { Interval = 1000 };
            timer.Tick += Timer_Tick;
            timer.Enabled = true;
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            lblDateTime.Text = clsUtil.Utl.GetCurrTime(1);
        }
        #endregion

        #region 메뉴Item Disable/Enable
        //(전체)하위Item을 비활성상태로 만든다.
        private void DisableItem()
        {

            ToolStripMenuItem tsm1;
            ToolStripMenuItem tsm2;
            ToolStripMenuItem tsm3;
            //메뉴바 Item: Enable
            for (int iCount1 = 0; iCount1 < mnuMain.Items.Count; iCount1++)
            {
                tsm1 = (ToolStripMenuItem)mnuMain.Items[iCount1];

                if ((tsm1.Name.Length > 3) && (tsm1.Name.Substring(0, 3).ToString() == "tsm") && tsm1.Tag.ToString() != "X")
                {
                    //tsm1.Enabled = false;
                    tsm1.Click -= Menu_Click;
                    tsm1.ForeColor = Color.Silver;
                }

                //1차 Item의 하위Item: Disable
                if (tsm1.DropDownItems.Count > 0)
                {
                    for (int iCount2 = 0; iCount2 < tsm1.DropDownItems.Count; iCount2++)
                    {
                        tsm2 = (ToolStripMenuItem)tsm1.DropDownItems[iCount2];

                        if (tsm2.Tag == null || tsm2.Tag.ToString() == "")
                        {
                            //tsm2.Enabled = false;
                            tsm2.Click -= Menu_Click;
                            tsm2.ForeColor = Color.Silver;
                        }

                        //2차 Item의 하위Item: Disable
                        if (tsm2.DropDownItems.Count > 0)
                        {
                            for (int iCount3 = 0; iCount3 < tsm2.DropDownItems.Count; iCount3++)
                            {
                                tsm3 = (ToolStripMenuItem)tsm2.DropDownItems[iCount3];

                                if (tsm3.Tag == null || tsm3.Tag.ToString() == "")
                                {
                                    //tsm3.Enabled = false;
                                    tsm3.Click -= Menu_Click;
                                    tsm3.ForeColor = Color.Silver;
                                }
                            }
                        }
                    }
                }
            }
            //try
            //{
                
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("[" + ex.ToString() + "]");
            //}
        }

        //사용권한이 있는 프로그램을 활성화 시킴
        //사용자 권한별 화면을 메뉴Item에 설정; (프로그램ID, 조회, 등록, 수정, 삭제)권한을 Item별 Tag에 저장
        private void EnableItem()
        {
            bool bCheck = false;
            string sql = "";
            DataTable dt = new DataTable();

            // 조회권한이 있는 화면 활성화
            // 권한 없는 메뉴는 권한화면에서 설정후 재실행해야함.
            sql = "";
            sql += "SELECT B.PAGE_ID, NVL(A.INQ_ACL, 'N') AS INQ_ACL, NVL(A.REG_ACL, 'N') AS REG_ACL, NVL(A.MOD_ACL, 'N') AS MOD_ACL, NVL(A.DEL_ACL, 'N') AS DEL_ACL";
            sql += "  FROM TB_CM_ACL A, TB_CM_SCR B";
            sql += " WHERE A.ACL_GRP_ID =  " + "'" + ck.UserGrp + "'";
            sql += "   AND A.SCR_ID = B.SCR_ID";
            sql += "   AND A.INQ_ACL = 'Y'";
            sql += "   AND B.USE_YN = 'Y'";

            dt = cd.FindDataTable(sql);

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("권한별 등록된 화면이 없습니다." + "\r\n" + "\r\n" + "사용자의 권한을 확인하세요!!", "확인");
                return;
            }

            string sItemName = string.Empty;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sItemName = "tsm" + dt.Rows[i]["PAGE_ID"].ToString();            //Item Name = "tsm" + 화면Page_id

                //if (sItemName == "tsmEqpChkItemMgmt")
                //{

                //}

                bCheck = false;
                //메뉴바 Item
                for (int iCount1 = 0; iCount1 < mnuMain.Items.Count; iCount1++)
                {
                    ToolStripMenuItem tsm1 = (ToolStripMenuItem)mnuMain.Items[iCount1];

                    //1차 Item의 하위Item
                    if (tsm1.DropDownItems.Count > 0)
                    {
                        for (int iCount2 = 0; iCount2 < tsm1.DropDownItems.Count; iCount2++)
                        {
                            ToolStripMenuItem tsm2 = (ToolStripMenuItem)tsm1.DropDownItems[iCount2];

                            if (tsm2.Name == sItemName)
                            {
                                //tsm2.Enabled = true;
                                tsm2.Click += Menu_Click;
                                tsm2.ForeColor = Color.White;
                                tsm2.Tag = dt.Rows[i]["PAGE_ID"].ToString() + "," + dt.Rows[i]["INQ_ACL"].ToString() + "," +
                                           dt.Rows[i]["REG_ACL"].ToString() + "," + dt.Rows[i]["MOD_ACL"].ToString() + "," +
                                           dt.Rows[i]["DEL_ACL"].ToString();
                                bCheck = true;
                                break;
                            }

                            //2차 Item의 하위Item
                            if (bCheck == false && tsm2.DropDownItems.Count > 0)
                            {
                                for (int iCount3 = 0; iCount3 < tsm2.DropDownItems.Count; iCount3++)
                                {
                                    ToolStripMenuItem tsm3 = (ToolStripMenuItem)tsm2.DropDownItems[iCount3];



                                    if (tsm3.Name == sItemName)
                                    {
                                        //if (tsm3.Name == "tsmLnGrProdRslt" && sItemName == "tsmLnGrProdRslt")
                                        //{

                                        //    string aaa = "";
                                        //}

                                        //tsm3.Enabled = true;
                                        tsm3.Click += Menu_Click;
                                        tsm3.ForeColor = Color.White;
                                        tsm3.Tag = dt.Rows[i]["PAGE_ID"].ToString() + "," + dt.Rows[i]["INQ_ACL"].ToString() + "," +
                                                   dt.Rows[i]["REG_ACL"].ToString() + "," + dt.Rows[i]["MOD_ACL"].ToString() + "," +
                                                   dt.Rows[i]["DEL_ACL"].ToString();
                                        bCheck = true;
                                        break;
                                    }
                                }
                            }

                            if (bCheck == true) break;
                        }
                    }

                    if (bCheck == true) break;
                }
            }

        }
        #endregion

        #region 실행
        public void Menu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsm = (ToolStripMenuItem)sender;
            if (tsm.Tag == null || tsm.Tag.ToString() == "X") return;       //하위프로그램이 있는 상위Item

            //프로그램ID / 단위프로그램별 버튼사용 권한
            string[] arrText = System.Text.RegularExpressions.Regex.Split(tsm.Tag.ToString(), ",");
            string sProgramId = arrText[0];
            string InqAcl = arrText[1];
            string RegAcl = arrText[2];
            string ModAcl = arrText[3];
            string DelAcl = arrText[4];

            string screen_ACL = tsm.Tag.ToString();//string.Format("{0},{1},{2},{3}", InqAcl, RegAcl, ModAcl, DelAcl);

            int idxScr = -1;
            string formName = sProgramId;
            string formText = tsm.Text;
            string formMsg = "『" + formText + "(" + formName + ")』\n";

            string fileNm = Application.StartupPath + "\\" + "SystemControlClassLibrary" + ".dll";

            if (!File.Exists(fileNm))
            {
                MessageBox.Show("『" + fileNm + "』\nFile이 존재하지 않습니다.");
                return;
            }

            Assembly MyAssembly = Assembly.LoadFile(fileNm);

            try
            {
                //이미 생성된 폼이존재 한다면.
                if (clsCom.Com.DefMenuID == "" || SelectForm(sProgramId))
                {
                    //RealizeMaxForm(sProgramId);
                    //선택된 화면과 연관된 버튼을 찾아 버튼의 색을 수정
                    UpdateSelectedBtn(formText);
                    return;
                }


                //10개 초과하여 Form을 Open 할 수 없다.
                if (this.MdiChildren.Length > 9)
                {
                    clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), "화면은 동시에 10개까지만 가능합니다.");
                    this.MdiChildren[0].Close();
                }

                for (int ii = 0; ii < MenuCnt; ii++)
                {
                    if (MenuArr[ii, 0] == clsCom.Com.DefMenuID)
                    {
                        idxScr = ii;
                        break;
                    }
                }
                idxScr = 1;

                if (idxScr < 0) return;               //초기 화면이 없을 경우 종료


                foreach (Type type in MyAssembly.GetTypes())
                {
                    if (type.BaseType == typeof(Form) || type.BaseType == typeof(Line3WholeTrk))
                    {
                        if (type.Name == sProgramId)
                        {
                            //호출 프로그램에 전달할 파라메터
                            object[] arrParam = new object[4];
                            arrParam[0] = formText;
                            arrParam[1] = screen_ACL;
                            arrParam[2] = "";
                            arrParam[3] = ((ToolStripMenuItem)sender).OwnerItem.ToString();


                            //폼생성
                            Form ExeForm = (Form)MyAssembly.CreateInstance(type.ToString(), false, BindingFlags.CreateInstance, null, arrParam, null, null);
                            if (ExeForm == null)
                            {
                                //MessageBox.Show(formMsg + "화면이 존재하지 않습니다.");
                                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), formMsg, "화면이 존재하지 않습니다.");
                                return;
                            }

                            ExeForm.Text = formText;
                            ExeForm.MdiParent = this;
                            ExeForm.FormClosed += FrmSCR_FormClosed;
                            //ExeForm.WindowState = FormWindowState.Minimized;
                            ExeForm.Show();

                            //자식 사이즈 {Width = 1273 Height = 901} 1273,901
                            ExeForm.WindowState = FormWindowState.Maximized;
                            ExeForm.BringToFront();


                            FindButton(ExeForm.Controls, InqAcl, RegAcl, ModAcl, DelAcl);      //버튼사용 권한

                            //top Toolbar Button 생성
                            if (tsTop.Items.IndexOfKey(formText) < 0)
                            {
                                string formText_tag = formText; // 교훈 추가
                                if (tsTop.Items.Count > 0)
                                {
                                    ToolStripSeparator sp1 = new ToolStripSeparator();

                                    sp1.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
                                    sp1.ForeColor = System.Drawing.Color.FromArgb(0, 122, 204);

                                    tsTop.Items.Add(sp1);
                                }

                                ToolStripButton tsbBtn = new ToolStripButton(formText, null, tsmWinScr_Click, formText); // var tsbtop
                                //tsbBtn.CheckOnClick = true;
                                tsbBtn.CheckedChanged += toolStripButton_CheckedChanged;
                                //tsbBtn.Click += TsbBtn_Click;

                                tsbBtn.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall, FontStyle.Bold);
                                tsTop.Items.Add(tsbBtn);

                                // 화면 닫기 버튼 추가 // 교훈 추가
                                ToolStripButton tsbCloseBtn = new ToolStripButton(null, global::QMS.Properties.Resources.smallBlackX, tsmWinScr_Close, formText);
                                tsbCloseBtn.Tag = formText_tag;
                                tsTop.Items.Add(tsbCloseBtn);

                                //Main Menu 창 MenuItem 생성
                                if (tsmWindow.DropDownItems.Count < 7)
                                {
                                    ToolStripSeparator sp2 = new ToolStripSeparator();
                                    //sp2.BackColor = System.Drawing.Color.FromArgb(0,122,204);
                                    //sp2.ForeColor = System.Drawing.Color.White;

                                    tsmWindow.DropDownItems.Add(sp2);
                                }

                                ToolStripMenuItem temp = new ToolStripMenuItem(formText, null, tsmWinScr_Click, formText);
                                temp.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
                                temp.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                                temp.ForeColor = System.Drawing.Color.White;
                                tsmWindow.DropDownItems.Add(temp);

                                RealizeMaxForm(formText);
                            }

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), formMsg +"화면을 Open하던 중 오류가 발생하였습니다. ", ex.Message);
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), formMsg + "화면을 Open하던 중 오류가 발생하였습니다. ");
                //MessageBox.Show(ex.ToString() + ex.Message);
                return;
            }


        }

        private void TsbBtn_Click(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
            if (btn.Checked)
            {
                btn.Checked = true;
                btn.CheckState = CheckState.Checked;
                btn.BackColor = Color.FromArgb(194, 224, 255);

            }
            else
            {
                btn.Checked = false;
                btn.CheckState = CheckState.Unchecked;
                btn.BackColor = Color.Transparent;
            }
        }

        private void toolStripButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSelectedBtn(((ToolStripButton)sender).Text);
        }

        private void UpdateSelectedBtn(string _btnText)
        {
            List<ToolStripButton> _listButt = tsTop.Items.OfType<ToolStripButton>().ToList();

            InitializeToolBtns(_listButt);

            ShowSelectedBtn(_btnText, _listButt);


        }

        private static void ShowSelectedBtn(string _btnText, List<ToolStripButton> _listButt)
        {
            foreach (ToolStripButton tbBut in _listButt)
            {
                if (tbBut.Text == _btnText)
                {
                    //tbBut.Checked = true;
                    tbBut.BackColor = Color.FromArgb(194, 224, 255);
                }
            }
        }

        private static void InitializeToolBtns(List<ToolStripButton> _listButt)
        {
            foreach (ToolStripButton tbBut in _listButt)
            {

                tbBut.Checked = false;
                tbBut.BackColor = Color.Transparent;

            }
        }

        //자식폼 버튼 사용권한
        // ChildForm : 자식폼
        // InqAcl    : 등록권한
        // RegAcl    : 등록권한
        // ModAcl    : 수정권한
        // DelAcl    : 삭제권한
        private void FindButton(Control.ControlCollection cont, string InqAcl, string RegAcl, string ModAcl, string DelAcl)
        {
            try
            {
                foreach (Control ct in cont)
                {
                    if (ct.Controls.Count > 0)
                    {
                        FindButton(ct.Controls, InqAcl, RegAcl, ModAcl, DelAcl);
                    }

                    if (ct.GetType() == typeof(C1.Win.C1Input.C1Button) || ct.GetType() == typeof(Button))
                    {
                        //조회
                        if (IsItem(buttonListINInqAcl, ct.Name))
                        {
                            if (InqAcl == "Y") ct.Enabled = true;
                            else ct.Enabled = false;
                            Console.WriteLine(ct.Name + "=" + ct.Enabled);
                        }
                        //저장, 투입및취소
                        else if (IsItem(buttonListINRegAcl, ct.Name))
                        {
                            
                            if (RegAcl == "Y") ct.Enabled = true;
                            else ct.Enabled = false;
                            Console.WriteLine(ct.Name+"="+ ct.Enabled);
                        }
                        //행추가, 행추가1, 행추가2
                        else if (IsItem(buttonListINModAcl, ct.Name))
                        {
                            if (ModAcl == "Y") ct.Enabled = true;
                            else ct.Enabled = false;
                            Console.WriteLine(ct.Name + "=" + ct.Enabled);
                        }


                        //행삭제, 행삭제1, 행삭제2
                        else if (IsItem(buttonListINDelAcl, ct.Name))
                        {
                            if (DelAcl == "Y") ct.Enabled = true;
                            else ct.Enabled = false;
                            Console.WriteLine(ct.Name + "=" + ct.Enabled);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }
        }

        //창닫기 교훈 추가
        private void tsmWinScr_Close(object sender, EventArgs e)
        {
            string frmTag = "";

            // top toolstrip item 들을 지운다.
            switch (sender.GetType().Name)
            {

                case "ToolStripButton":
                    frmTag = ((ToolStripButton)sender).Tag.ToString();
                    break;
            }

            RemoveSCR(frmTag);
        }

        //창닫기 교훈 추가
        private void RemoveSCR(string frmTag)
        {
            while (IsItem_new(tsTop, frmTag))
            {
                for (int i = 0; i < tsTop.Items.Count; i++)
                {
                    if (tsTop.Items[i].Tag == null) continue;

                    //Console.WriteLine("Exist_Name_" + tsTop.Items[i].Name + "-" + i.ToString());
                    //Console.WriteLine("Exist_Tag_" + tsTop.Items[i].Tag + "-" + i.ToString());
                    if (tsTop.Items[i].Tag.ToString() == frmTag)
                    {

                        //Console.WriteLine("Removed_Name_" + tsTop.Items[i].Name + "-" + i.ToString());
                        //Console.WriteLine("Removed_Tag_" + tsTop.Items[i].Tag + "-" + i.ToString());
                        tsTop.Items.RemoveAt(i);
                    }
                }
            }

            //Close Form 
            CloseForm(frmTag);
        }

        //창닫기 교훈 추가
        private bool IsItem_new(ToolStrip toolstrip, string frmTag)
        {
            for (int i = 0; i < toolstrip.Items.Count; i++)
            {
                if (toolstrip.Items[i].Tag == null) continue;

                if (toolstrip.Items[i].Tag.ToString() == frmTag)
                {
                    return true;
                }
            }

            return false;
        }

        //창닫기 교훈 추가
        private bool CloseForm(string frmText)
        {
            //Open Form이 있으면 선택
            foreach (Form chdForm in this.MdiChildren)
            {
                if (chdForm.Name == frmText || chdForm.Text == frmText)
                {

                    chdForm.Close();
                    return true;
                }
            }

            return false;
        }


        private bool SelectForm(string frmText)
        {
            //Open Form이 있으면 선택
            foreach (Form chdForm in this.MdiChildren)
            {
                if (chdForm.Name == frmText || chdForm.Text == frmText)
                {
                    chdForm.FormClosed += ChdForm_FormClosed;
                    chdForm.WindowState = FormWindowState.Maximized;
                    chdForm.BringToFront();
                    chdForm.Focus();
                    RealizeMaxForm(frmText);
                    return true;
                }
            }

            return false;
        }

        private void ChdForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            //if (ActiveMdiChild != null)
            //{
            //    UpdateSelectedBtn(ActiveMdiChild.Text);
            //}
        }

        // 폼을 중복생성하지 않고 보여줌
        protected void FrmShow(Form frm)
        {
            bool FrmisExist = false;

            foreach (Form form1 in Application.OpenForms)
            {
                if (form1.GetType() == frm.GetType())
                {
                    FrmisExist = true;
                    return;
                }
            }

            // 폼존재여부에 따라서 생성과 파기
            if (!FrmisExist)
            {
                frm.Show(); frm.Activate();
            }
            else
            {
                frm.Dispose();
            }
        }
        #endregion

        #region 이벤트
        private void FrmSCR_FormClosed(object sender, FormClosedEventArgs e)
        {
            string frmText = ((Form)sender).Text;

            if (tsTop.Items.Count > 1)
            {
                int idx = tsTop.Items.IndexOfKey(frmText);

                if (idx > 0) tsTop.Items.RemoveAt(idx - 1);
                else tsTop.Items.RemoveAt(idx + 1);
            }

            tsTop.Items.RemoveByKey(frmText);

            //Main Menu 창 MenuItem & Separator 삭제
            tsmWindow.DropDownItems.RemoveByKey(frmText);

            if (tsmWindow.DropDownItems.Count == 7)
            {
                tsmWindow.DropDownItems.RemoveAt(6);
            }

            childFormList.Remove(((Form)sender));
        }


        private void tsmWinScr_Click(object sender, EventArgs e)
        {
            string tsmText = "";

            switch (sender.GetType().Name)
            {
                case "ToolStripMenuItem":
                    tsmText = ((ToolStripMenuItem)sender).Text;
                    break;

                case "ToolStripButton":
                    tsmText = ((ToolStripButton)sender).Text;
                    break;
            }


            //Open Form 선택
            SelectForm(tsmText);
            //if (SelectForm(tsmText))
            //{
            //    //선택된 버튼 활성화 다른건 비활성화

            //}

            //RealizeMaxForm(tsmText);
        }

        private void RealizeMaxForm(string formName)
        {

            //if (((Form)sender).ActiveMdiChild == null)
            //{
            //    return;
            //}

            //string frmText = ((Form)sender).ActiveMdiChild.Text;

            foreach (var item in tsTop.Items)
            {
                if (item.GetType() == typeof(ToolStripButton))
                {
                    if (((ToolStripButton)item).Text == formName)
                    {
                        ((ToolStripButton)item).Checked = true;
                        ((ToolStripButton)item).ForeColor = Color.Black;
                        //((ToolStripButton)item).CheckState = CheckState.Checked;

                    }
                    else
                    {
                        ((ToolStripButton)item).ForeColor = Color.Black;
                        //((ToolStripButton)item).CheckState = CheckState.Checked;
                    }
                }

            }
        }

        private void tsmWindow_Click(object sender, EventArgs e)
        {
            switch (((ToolStripMenuItem)sender).Name)
            {
                case "tsmCloseAll":
                    foreach (Form chdForm in this.MdiChildren)
                    {
                        chdForm.Close();
                    }
                    break;

                case "tsmCascade":
                    this.LayoutMdi(MdiLayout.Cascade);
                    break;

                case "tsmTileHorizontal":
                    this.LayoutMdi(MdiLayout.TileHorizontal);
                    break;

                case "tsmTileVertical":
                    this.LayoutMdi(MdiLayout.TileVertical);
                    break;

                case "tsmArrangeIcons":
                    this.LayoutMdi(MdiLayout.ArrangeIcons);
                    break;
            }
        }

        // Menu Color Setup
        private void mnuMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.BackColor != Color.FromArgb(255, 0, 122, 204))
                e.ClickedItem.BackColor = Color.FromArgb(255, 0, 122, 204);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (Form chdForm in this.MdiChildren)
            {
                chdForm.Close();
            }

            msgshow_lb.Text = "";
        }
        #endregion

        #region 사용자 정의
        private class MyRenderer : ToolStripProfessionalRenderer
        {
            public MyRenderer() : base(new MyColors()) { }
        }

        private class MyColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Color.DarkBlue; }//Color.FromArgb(255, 218,233,245); }
            }
            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.FromArgb(255, 0, 122, 204); }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.FromArgb(255, 0, 122, 204); }
            }
            public override Color ToolStripDropDownBackground
            {
                get { return Color.FromArgb(255, 0, 122, 204); }
            }
            public override Color MenuItemPressedGradientBegin
            {
                get { return Color.FromArgb(255, 0, 122, 204); }
            }

            public override Color MenuItemPressedGradientMiddle
            {
                get { return Color.FromArgb(255, 0, 122, 204); }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get { return Color.FromArgb(255, 0, 122, 204); }
            }

            public override Color MenuItemBorder
            {
                get { return Color.FromArgb(255, 0, 122, 204); }
            }
        }
        #endregion


        private void delete_ts_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tsBtnAllDelete_Click(object sender, EventArgs e)
        {
            foreach (Form chdForm in this.MdiChildren)
            {
                chdForm.Close();
            }
        }


        private void MainForm_Resize(object sender, EventArgs e)
        {

            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            var backgroundImage = global::QMS.Properties.Resources._3__우리의안전다짐;

            SetupBackgroudImage(backgroundImage);
        }

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            Form activatedChildForm = this.ActiveMdiChild;

            if (activatedChildForm == null)
            {
                //the last child form was just closed
                return;
            }

            if (!childFormList.Contains(activatedChildForm))
            {
                UpdateSelectedBtn(activatedChildForm.Text);
            }

        }
    }
}

