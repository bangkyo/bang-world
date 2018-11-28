using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BG_L2_120
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private clsStyle cs = new clsStyle();
        ConnectDB cd = new ConnectDB();
        private clsCom ck = new clsCom();
        string dllFileNm = "";
        private Color backColor = Color.FromArgb(0, 140, 204);

        private ToolStripMenuItem tsmWindow;
        private ToolStripMenuItem tsmCascade;
        private ToolStripMenuItem tsmTileHorizontal;
        private ToolStripMenuItem tsmTileVertical;
        private ToolStripMenuItem tsmArrangeIcons;
        private ToolStripMenuItem tsmCloseAll;

        private ToolStripMenuItem tsmInformation;
        private ToolStripMenuItem tsmPassword;
        private ToolStripMenuItem tsmAbout;

        private ToolStripMenuItem tsmUser;
        private ToolStripMenuItem tsmExit;

        private bool isExitLogOut = false; //로그아웃클릭인지 여부판단(클릭시 true)


        private void MainForm_Resize(object sender, EventArgs e)
        {
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
        }

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == null) return;

            RealizeMaxForm(this.ActiveMdiChild.Text);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            dllFileNm = ck.dllFileNm;

            InitMenu();

            InitToolStrip();

            InitMsgShow_LB();

            Timer_Set();


        }

        private void Timer_Set()
        {
            var timer = new Timer { Interval = 1000 };
            timer.Tick += Timer_Tick;
            timer.Enabled = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = clsUtil.Utl.GetCurrTime(1);
        }

        private void InitMsgShow_LB()
        {
            msgshow_lb.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall, FontStyle.Bold);
            lblDateTime.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall, FontStyle.Bold);

            clsMsg.Log.Msglabel = msgshow_lb;
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "프로그램을 시작합니다.");
        }

        private void InitToolStrip()
        {
            
            // 
            // tsBtnAllDelete
            // 
            this.tsBtnAllDelete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsBtnAllDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnAllDelete.Image = global::BG_L2_120.Properties.Resources.smallBlackX;
            this.tsBtnAllDelete.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tsBtnAllDelete.Name = "tsBtnAllDelete";
            this.tsBtnAllDelete.Size = new System.Drawing.Size(23, 22);
            this.tsBtnAllDelete.Tag = "allClose";
            this.tsBtnAllDelete.Text = "모든창 닫기";
            this.tsBtnAllDelete.ToolTipText = "모든창 닫기";
            this.tsBtnAllDelete.Click += new System.EventHandler(this.tsBtnAllDelete_Click);

            tsTop.Items.Add(tsBtnAllDelete);
        }

        private void InitMenu()
        {

            mnuMain.Renderer = new MyRenderer();

            InitACL();

            Setmenu();

            SetuserInfo();
        }

        private void Setmenu()
        {


            string sql = "";
            sql += string.Format(" WITH TB_MENU AS                                       ");
            sql += string.Format(" (                                                     ");
            sql += string.Format(" 	SELECT A.MENU_SEQ                                    ");
            sql += string.Format(" 		 , A.MENU_ID                                     ");
            sql += string.Format(" 		 , A.MENU_NM                                     ");
            sql += string.Format(" 		 , A.UP_MENU_ID                                  ");
            sql += string.Format(" 		 , A.SCR_ID                                      ");
            sql += string.Format(" 		 , B.SCR_NM                                      ");
            sql += string.Format(" 		 , B.BIZ_GP                                      ");
            sql += string.Format(" 		 , B.PAGE_ID                                     ");
            sql += string.Format(" 		 , B.USE_YN                                      ");
            sql += string.Format(" 	  FROM TB_CM_MENU A                                  ");
            sql += string.Format(" 	  LEFT OUTER JOIN TB_CM_SCR B                        ");
            sql += string.Format(" 		ON A.SCR_ID = B.SCR_ID                           ");
            sql += string.Format(" 	   AND B.USE_YN = 'Y'                                ");
            sql += string.Format("  )                                                    ");
            sql += string.Format("  , TB_ACL_T AS                                        ");
            sql += string.Format("  (                                                    ");
            sql += string.Format(" 	SELECT A.USER_ID                                     ");
            sql += string.Format(" 	     , B.BIZ_GP                                      ");
            sql += string.Format(" 		 , B.PAGE_ID                                     ");
            sql += string.Format(" 		 , B.USE_YN                                      ");
            sql += string.Format(" 		 , B.SCR_ID                                      ");
            sql += string.Format(" 		 , B.SCR_NM                                      ");
            sql += string.Format(" 		 , C.ACL_GRP_ID                                  ");
            sql += string.Format(" 	     , C.INQ_ACL                                     ");
            sql += string.Format(" 		 , C.REG_ACL                                     ");
            sql += string.Format(" 		 , C.MOD_ACL                                     ");
            sql += string.Format(" 		 , C.DEL_ACL                                     ");
            sql += string.Format(" 	  FROM TB_CM_USER A                                  ");
            sql += string.Format(" 	     , TB_CM_SCR B                                   ");
            sql += string.Format(" 		 , TB_CM_ACL C                                   ");
            sql += string.Format(" 	 WHERE A.ACL_GRP_ID = C.ACL_GRP_ID                   ");
            sql += string.Format(" 	   AND B.SCR_ID     = C.SCR_ID                       ");
            sql += string.Format(" 	   AND A.USER_ID    = '{0}'                          ", ck.UserID);
            sql += string.Format(" 	   AND B.USE_YN     = 'Y'                            ");
            sql += string.Format("  )                                                    ");
            sql += string.Format("  SELECT *                                             ");
            sql += string.Format("    FROM TB_MENU  A                                    ");
            sql += string.Format(" 	  LEFT OUTER JOIN TB_ACL_T B                         ");
            sql += string.Format(" 		ON A.SCR_ID = B.SCR_ID                           ");
            sql += string.Format("  ORDER BY A.UP_MENU_ID, A.MENU_SEQ, A.MENU_ID         ");

            DataTable dt = cd.FindDataTable(sql);

            if (dt == null || dt.Rows.Count < 1)
            {
                return;
            }


            DataRow[] topMenuItems = dt.Select(" UP_MENU_ID IS NULL  OR LEN(UP_MENU_ID) = 0");
            int menu_seq = 0;
            foreach (DataRow item in topMenuItems)
            {
                ToolStripMenuItem topmenuStripItem = new ToolStripMenuItem(item["MENU_NM"].ToString(), null, null, item["MENU_ID"].ToString());

                //topmenuStripItem.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
                topmenuStripItem.BackColor = System.Drawing.Color.FromArgb(0, 140, 204);
                topmenuStripItem.ForeColor = Color.Black;
                topmenuStripItem.Font = new Font(cs.OHeadfontName, cs.FontSizeMiddle, FontStyle.Bold);

                if (menu_seq == 0)
                {
                    topmenuStripItem.Margin = new System.Windows.Forms.Padding(400, 3, 3, 3);
                }
                else
                {
                    topmenuStripItem.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
                }

                //topmenuStripItem.MouseEnter += new System.EventHandler(this.menuStripMouseEnter);
                //topmenuStripItem.MouseLeave += new System.EventHandler(this.menuStripMouseLeave);

                mnuMain.Items.Add(topmenuStripItem);

                SubMenu(dt, topmenuStripItem, item["MENU_ID"].ToString());
                menu_seq++;
            }
        }



        private void menuStripMouseLeave(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.BackColor = System.Drawing.Color.FromArgb(0, 140, 204);
            item.ForeColor = Color.White;
        }

        private void menuStripMouseEnter(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.BackColor = Color.White;
            item.ForeColor = System.Drawing.Color.FromArgb(0, 122, 204);
        }

        private void SubMenu(DataTable _dt, ToolStripMenuItem menu, string parentMenuNId)
        {
            //if (string.IsNullOrEmpty(parentMenuNId)) return;

            string Sql = string.Format(" UP_MENU_ID = '{0}'", parentMenuNId);

            DataRow[] _SubMenuItems = _dt.Select(Sql);

            foreach (DataRow dr in _SubMenuItems)
            {
                
                ToolStripMenuItem Submenu = new ToolStripMenuItem(dr["MENU_NM"].ToString(), null, new EventHandler(ChildClick), dr["MENU_ID"].ToString());
                
                
                //SUB HEAD MENU 인경우 SCR_ID 가없음 자동 ENABLE SET
                if (string.IsNullOrEmpty(dr["SCR_ID"].ToString()))
                {
                    Submenu.Enabled = true;
                }
                else
                {
                    Submenu.Enabled = (dr["INQ_ACL"].ToString() == "Y") ? true : false;
                }

                Submenu.Tag = dr["PAGE_ID"].ToString() + "," + dr["INQ_ACL"].ToString() + "," +
                              dr["REG_ACL"].ToString() + "," + dr["MOD_ACL"].ToString() + "," +
                              dr["DEL_ACL"].ToString();
                Submenu.BackColor = System.Drawing.Color.FromArgb(0, 140, 204);
                Submenu.ForeColor = Color.White;
                Submenu.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall, FontStyle.Bold);
                
                menu.DropDownItems.Add(Submenu);

                SSubMenu(_dt, Submenu, dr["MENU_ID"].ToString());
            }
        }

        private void SSubMenu(DataTable _dt, ToolStripMenuItem menu, string parentMenuNId)
        {
            //if (string.IsNullOrEmpty(parentMenuNId)) return;

            string Sql = string.Format(" UP_MENU_ID = '{0}'", parentMenuNId);

            DataRow[] _SubMenuItems = _dt.Select(Sql);

            foreach (DataRow dr in _SubMenuItems)
            {
                ToolStripMenuItem SSubmenu = new ToolStripMenuItem(dr["MENU_NM"].ToString(), null, new EventHandler(ChildClick), dr["MENU_ID"].ToString());

                //SUB HEAD MENU 인경우 SCR_ID 가없음 자동 ENABLE SET
                if (string.IsNullOrEmpty(dr["SCR_ID"].ToString()))
                {
                    SSubmenu.Enabled = true;
                }
                else
                {
                    SSubmenu.Enabled = (dr["INQ_ACL"].ToString() == "Y") ? true : false;
                }
                SSubmenu.Tag = dr["PAGE_ID"].ToString() + "," + dr["INQ_ACL"].ToString() + "," +
                              dr["REG_ACL"].ToString() + "," + dr["MOD_ACL"].ToString() + "," +
                              dr["DEL_ACL"].ToString();

                SSubmenu.BackColor = System.Drawing.Color.FromArgb(0, 140, 204);
                SSubmenu.ForeColor = Color.White;
                SSubmenu.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall, FontStyle.Bold);
                menu.DropDownItems.Add(SSubmenu);
            }
        }


        public void ChildClick(object sender, EventArgs e)
        {
            if (!CanOpenScreen(sender)) return;

            ToolStripMenuItem tsm = (ToolStripMenuItem)sender;

            //프로그램ID / 단위프로그램별 버튼사용 권한
            string[] arrText = System.Text.RegularExpressions.Regex.Split(tsm.Tag.ToString(), ",");
            string sProgramId = arrText[0];
            string InqAcl = arrText[1];
            string RegAcl = arrText[2];
            string ModAcl = arrText[3];
            string DelAcl = arrText[4];

            string screen_ACL = tsm.Tag.ToString();//string.Format("{0},{1},{2},{3}", InqAcl, RegAcl, ModAcl, DelAcl);

            string formName = sProgramId;
            string formText = tsm.Text;
            string formMsg = "『" + formText + "(" + formName + ")』\n";

            try
            {
                Assembly MyAssembly = Assembly.LoadFile(dllFileNm);

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
                    //clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), tsTop.Items[2].Name);
                    clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), "화면은 동시에 10개까지만 가능합니다.");
                    RemoveFirstSCR();
                    //return;
                    //tsTop.Items[2].PerformClick();
                    //this.MdiChildren[0].Close();
                }

                foreach (Type type in MyAssembly.GetTypes())
                {
                        //Console.WriteLine(String.Format("type.Name : {0},{1} ", type.Name, type.BaseType));
                    if (type.BaseType == typeof(Form) & type.Name == sProgramId)
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
                        ExeForm.ControlBox = false;
                        ExeForm.FormClosed += FrmSCR_FormClosed;
                        ExeForm.Show();

                        //자식 사이즈 {Width = 1273 Height = 901} 1273,901
                        ExeForm.WindowState = FormWindowState.Maximized;
                        ExeForm.BringToFront();


                        if (CanApplyAcl(sProgramId))
                        {
                            FindButton(ExeForm.Controls, InqAcl, RegAcl, ModAcl, DelAcl);      //버튼사용 권한
                        }


                        //top Toolbar Button 생성
                        if (tsTop.Items.IndexOfKey(formText) < 0)
                        {
                            string formText_tag = formText;
                            if (tsTop.Items.Count > 1)
                            {
                                ToolStripSeparator sp1 = new ToolStripSeparator();

                                //sp1.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
                                sp1.BackColor = System.Drawing.Color.FromArgb(0, 140, 204);
                                sp1.ForeColor = System.Drawing.Color.FromArgb(0, 122, 204);
                                sp1.Name = formText;
                                sp1.Tag = formText_tag;

                                tsTop.Items.Add(sp1);
                            }

                            ToolStripButton tsbBtn = new ToolStripButton(formText, null, tsmWinScr_Click, formText); // var tsbtop
                                                                                                                     //tsbBtn.CheckOnClick = true;
                            tsbBtn.CheckedChanged += toolStripButton_CheckedChanged;
                            //tsbBtn.Click += TsbBtn_Click;
                            tsbBtn.Tag = formText_tag;
                            tsbBtn.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall, FontStyle.Bold);
                            tsTop.Items.Add(tsbBtn);

                            // 화면 닫기 버튼 추가
                            ToolStripButton tsbCloseBtn = new ToolStripButton(null, global::BG_L2_120.Properties.Resources.smallBlackX, tsmWinScr_Close, formText);
                            tsbCloseBtn.Tag = formText_tag;
                            tsTop.Items.Add(tsbCloseBtn);

                            

                            ////Main Menu 창 MenuItem 생성
                            //if (tsmWindow.DropDownItems.Count < 7)
                            //{
                            //    ToolStripSeparator sp2 = new ToolStripSeparator();
                            //    //sp2.BackColor = System.Drawing.Color.FromArgb(0,122,204);
                            //    //sp2.ForeColor = System.Drawing.Color.White;

                            //    tsmWindow.DropDownItems.Add(sp2);
                            //}

                            //ToolStripMenuItem temp = new ToolStripMenuItem(formText, null, tsmWinScr_Click, formText);
                            //temp.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
                            //temp.Font = new System.Drawing.Font("돋움체", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                            //temp.ForeColor = System.Drawing.Color.White;
                            //tsmWindow.DropDownItems.Add(temp);

                            RealizeMaxForm(formText);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), formMsg +"화면을 Open하던 중 오류가 발생하였습니다. ", ex.Message);
                //clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), formMsg + "화면을 Open하던 중 오류가 발생하였습니다. ");
                MessageBox.Show(ex.ToString() + ex.Message);
                return;
            }
        }

        private bool CanApplyAcl(string _sProgramId)
        {
            bool canApplyAcl = true;

            foreach (var scrName in aclIgnoreSCRIDList)
            {
                if (_sProgramId == scrName)
                {
                    canApplyAcl= false;
                    break;
                }
            }

            return canApplyAcl;
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

        private void RemoveFirstSCR()
        {
            string frmTag = tsTop.Items[1].Tag.ToString();
            RemoveSCR(frmTag);
        }

        private void SetuserInfo()
        {
            tsmUser = new ToolStripMenuItem();
            tsmExit = new ToolStripMenuItem();

            //mnuMain.Items.AddRange(new ToolStripItem[] {
            //tsmUser});

            //mnuMain.Items.AddRange(new ToolStripItem[] {
            //tsmExit});

            string userid = clsMsg.Log.__ReadUserId();
            tsmUser.Margin = new System.Windows.Forms.Padding(200, 3, 3, 3);
            tsmUser.Font = new Font("돋움체", 9F, FontStyle.Bold);
            //tsmUser.ForeColor = Color.Red;

            //2017.07.17 OCJ 추가--
            tsmUser.BackColor = Color.Transparent;
            tsmUser.ForeColor = Color.White;
            //--
            tsmUser.Name = "tsmUser";
            tsmUser.Size = new Size(37, 23);
            tsmUser.Tag = "U";
            //tsmUser.Text = userid + " 님";
            tsmUser.Text = ck.UserID + " 님";

            //2017.07.17 OCJ 추가
            mnuMain.Items.Add(tsmUser);

            tsmExit.Margin = new System.Windows.Forms.Padding((tsmUser.Margin.Left) - 200, 3, 3, 3);
            //tsmExit.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            tsmExit.Font = new Font("돋움체", 9F, FontStyle.Bold);
            //tsmExit.ForeColor = Color.Red;
            //2017.07.17 OCJ 추가--
            tsmExit.BackColor = Color.Transparent;
            tsmExit.ForeColor = Color.White;
            
            //--
            tsmExit.Name = "tsmExit";
            tsmExit.Size = new Size(37, 23);
            tsmExit.Tag = "E";
            tsmExit.Text = "로그아웃";
            tsmExit.Click += tsmExit_Click;
            
            

            //2017.07.17 OCJ 추가
            mnuMain.Items.Add(tsmExit);
            
        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("로그아웃 하시겠습니까? ", Text, MessageBoxButtons.YesNo) == DialogResult.Yes) //로그아웃 팝업->"예"
            {
                isExitLogOut = true;
                // 프로세스 종료 
                //Application.Exit();
                Application.Restart();
            }
            else//로그아웃 팝업->"아니오"
            {
                isExitLogOut = false;
            }
        }

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

        private void RemoveSCR(string frmTag)
        {
            while (IsItem(tsTop, frmTag))
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

        private bool IsItem(ToolStrip toolstrip, string frmTag)
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

        private static void InitializeToolBtns(List<ToolStripButton> _listButt)
        {
            foreach (ToolStripButton tbBut in _listButt)
            {

                tbBut.Checked = false;
                tbBut.BackColor = Color.Transparent;

            }
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

        private bool SelectForm(string frmText)
        {
            //Open Form이 있으면 선택
            foreach (Form chdForm in this.MdiChildren)
            {
                if (chdForm.Name == frmText || chdForm.Text == frmText)
                {
                    //chdForm.FormClosed += ChdForm_FormClosed;
                    chdForm.WindowState = FormWindowState.Maximized;
                    chdForm.BringToFront();
                    chdForm.Focus();
                    RealizeMaxForm(frmText);
                    return true;
                }
            }

            return false;
        }

        private void RealizeMaxForm(string formName)
        {
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

        

        private void ChdForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        // InqAcl    : 등록권한
        // RegAcl    : 등록권한
        // ModAcl    : 수정권한
        // DelAcl    : 삭제권한
        List<string> buttonListINInqAcl;
        List<string> buttonListINRegAcl;
        List<string> buttonListINModAcl;
        List<string> buttonListINDelAcl;

        //권한 재조정되는 화면
        List<string> aclIgnoreSCRIDList;

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
            buttonListINRegAcl.Add("btResultReWork");
            buttonListINRegAcl.Add("btnAddRes");
            //buttonListINRegAcl.Add("btnPOCFin");
            //buttonListINRegAcl.Add("btnApply");
            //buttonListINRegAcl.Add("btnApplyCancel");
            //buttonListINRegAcl.Add("btnPLCSend");
            //buttonListINRegAcl.Add("btnBNDSend");



            //수정
            buttonListINModAcl.Add("btnZoneMove");
            buttonListINModAcl.Add("bt_BloomMgmt");
            //buttonListINModAcl.Add("btnRowAdd2");
            //buttonListINModAcl.Add("btnRowAdd2");

            //삭제
            buttonListINDelAcl.Add("btHeatFin");
            //buttonListINDelAcl.Add("btnDelRow1");
            //buttonListINDelAcl.Add("btnDelRow2");
            //buttonListINDelAcl.Add("btnDelRow2");
            //buttonListINDelAcl.Add("btnExcel");

            aclIgnoreSCRIDList = new List<string>
            {
                "MatTakeOverMgmt"
            };


        }

        //private void FindButton(Control.ControlCollection cont, string InqAcl, string RegAcl, string ModAcl, string DelAcl)
        //{
        //    try
        //    {
        //        foreach (Control ct in cont)
        //        {
        //            if (ct.Controls.Count > 0)
        //            {
        //                FindButton(ct.Controls, InqAcl, RegAcl, ModAcl, DelAcl);
        //            }

        //            if (ct.GetType() == typeof(C1.Win.C1Input.C1Button) || ct.GetType() == typeof(Button))
        //            {
        //                //조회
        //                if (IsItem(buttonListINInqAcl, ct.Name))
        //                {
        //                    if (InqAcl == "Y") ct.Enabled = true;
        //                    else ct.Enabled = false;
        //                    //Console.WriteLine(ct.Name + "=" + ct.Enabled);
        //                }
        //                //저장, 투입및취소
        //                else if (IsItem(buttonListINRegAcl, ct.Name))
        //                {

        //                    if (RegAcl == "Y") ct.Enabled = true;
        //                    else ct.Enabled = false;
        //                    //Console.WriteLine(ct.Name + "=" + ct.Enabled);
        //                }
        //                //행추가, 행추가1, 행추가2
        //                else if (IsItem(buttonListINModAcl, ct.Name))
        //                {
        //                    if (ModAcl == "Y") ct.Enabled = true;
        //                    else ct.Enabled = false;
        //                    //Console.WriteLine(ct.Name + "=" + ct.Enabled);
        //                }


        //                //행삭제, 행삭제1, 행삭제2
        //                else if (IsItem(buttonListINDelAcl, ct.Name))
        //                {
        //                    if (DelAcl == "Y") ct.Enabled = true;
        //                    else ct.Enabled = false;
        //                    //Console.WriteLine(ct.Name + "=" + ct.Enabled);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("[" + ex.ToString() + "]");
        //    }
        //}

        private bool IsItem(List<string> list, string itemName)
        {
            if (list == null || string.IsNullOrEmpty(itemName)) return false;

            foreach (string item in list)
            {
                if (item == itemName)
                {
                    return true;
                }
            }

            return false;
        }

        private void FrmSCR_FormClosed(object sender, FormClosedEventArgs e)
        {

        }


        private bool CanOpenScreen(object _sender)
        {

            if (!File.Exists(dllFileNm))
            {
                MessageBox.Show("『" + dllFileNm + "』\nFile이 존재하지 않습니다.");
                return false;
            }

            //10개 초과하여 Form을 Open 할 수 없다.
            if (this.MdiChildren.Length > 10/*9*/)
            {
                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), "화면은 동시에 10개까지만 가능합니다.");
                //this.MdiChildren[1].Close();
                return false;
            }

            ToolStripMenuItem tsm = (ToolStripMenuItem)_sender;
            if (tsm.Tag == null || tsm.Tag.ToString() == "X") return false;       //하위프로그램이 있는 상위Item

            //Assembly assembly = Assembly.LoadFile(dllFileNm);


            return true;

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void mnuMain_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void mnuMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void tsBtnAllDelete_Click(object sender, EventArgs e)
        {
            RemoveFormAll();

            RemoveToolBtnAll();

            msgshow_lb.Text = "";
        }

        private void RemoveToolBtnAll()
        {
            while(IsItem())
            {
                for (int i = 0; i < tsTop.Items.Count; i++)
                {
                    if (tsTop.Items[i].Name == null || tsTop.Items[i].Name =="tsBtnAllDelete") continue;

                    //Console.WriteLine("Exist_Name_" + tsTop.Items[i].Name + "-" + i.ToString());
                    //Console.WriteLine("Exist_Tag_" + tsTop.Items[i].Tag + "-" + i.ToString());
                    //Console.WriteLine("Removed_Name_" + tsTop.Items[i].Name + "-" + i.ToString());
                    //Console.WriteLine("Removed_Tag_" + tsTop.Items[i].Tag + "-" + i.ToString());
                    tsTop.Items.RemoveAt(i);
                }
            }
        }

        private bool IsItem()
        {
            // all delete btn 제외한 총수
            if (tsTop.Items.Count - 1 > 0) return true;
            return false;
        }

        private void RemoveFormAll()
        {
            foreach (Form chdForm in this.MdiChildren)
            {
                chdForm.Close();
            }
        }

        private void menuStripMouseHover(object sender, EventArgs e)
        {
            
        }

        private void delete_ts_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

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
                get { return Color.FromArgb(255, 0, 140, 204); }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.FromArgb(255, 0, 140, 204); }
            }
            public override Color ToolStripDropDownBackground
            {
                get { return Color.FromArgb(255, 0, 140, 204); }
            }
            public override Color MenuItemPressedGradientBegin
            {
                get { return Color.FromArgb(255, 0, 140, 204); }
            }

            public override Color MenuItemPressedGradientMiddle
            {
                get { return Color.FromArgb(255, 0, 140, 204); }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get { return Color.FromArgb(255, 0, 140, 204); }
            }

            public override Color MenuItemBorder
            {
                get { return Color.FromArgb(255, 0, 140, 204); }
            }
        }
        #endregion
    }
}
