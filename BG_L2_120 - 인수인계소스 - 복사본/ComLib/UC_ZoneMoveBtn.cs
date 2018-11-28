using System;
using System.ComponentModel;
using System.Windows.Forms;
using ComLib;
using System.Drawing;

namespace ComLib
{
    public partial class UC_ZoneMoveBtn : UserControl
    {
        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();

        public event EventHandler PopupEvent;
        private string tc_cd = "";
        private string before_Zone = "";
        private string after_Zone = "";
        private Image background_imge = null;
        private Arrow _arrow =Arrow.Right;

        public enum Arrow
        {
            Left, Right, Up, Down
        }

        public UC_ZoneMoveBtn()
        {
            InitializeComponent();

            SetFontColor();
        }

        private void SetFontColor()
        {
            //throw new NotImplementedException();
        }

        [Description("TC_CD"), Category("Contents")]
        public string TC_CD
        {
            get
            {
                return tc_cd;
            }
            set
            {
                tc_cd = value;
            }
        }

        [Description("Before Zone"), Category("Contents")]
        public string Before_Zone
        {
            get
            {
                return before_Zone;
            }
            set
            {
                before_Zone = value;

            }
        }

        [Description("After Zone"), Category("Contents")]
        public string After_Zone
        {
            get
            {
                return after_Zone;
            }
            set
            {
                after_Zone = value;
            }
        }

        [Description("Select Arrow"), Category("Contents")]
        public Arrow Select_Arrow
        {
            get { return _arrow; }
            set
            {
                _arrow = value;
                switch (_arrow)
                {
                    case Arrow.Left:
                        btnArrow.BackgroundImage = ComLib.Properties.Resources.if_33_61497_left;
                        break;
                    case Arrow.Right:
                        btnArrow.BackgroundImage = ComLib.Properties.Resources.if_34_61498_right;
                        break;
                    case Arrow.Up:
                        btnArrow.BackgroundImage = ComLib.Properties.Resources.if_32_61496_up;
                        break;
                    case Arrow.Down:
                        btnArrow.BackgroundImage = ComLib.Properties.Resources.if_35_61499_down;
                        break;
                    default:
                        btnArrow.BackgroundImage = ComLib.Properties.Resources.if_34_61498_right;
                        break;
                }
            }
        }



        protected void lbBundleNo_Click(object sender, EventArgs e)
        {
            //ClickEvent(sender, e);
            //if (this.UpdateParentPage != null)
            //    UpdateParentPage(sender, e);
        }



        protected void lbPieceNo_Click(object sender, EventArgs e)
        {
            //ClickEvent(sender, e);
        }

        protected void lbZone_Click(object sender, EventArgs e)
        {
            //ClickEvent(sender, e);
        }

        private void ClickEvent(object sender, EventArgs e)
        {
            
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            
        }


        private void SetCursorState(bool isOn)
        {
            if (isOn)
            {
                //SetupLabelCursor(lbPieceNo, Cursors.Hand);
                //SetupLabelCursor(lbZone, Cursors.Hand);
                //SetupLabelCursor(lbBundleNo, Cursors.Hand);
                //lbPieceNo.Cursor = Cursors.Hand;
                //lbZone.Cursor = Cursors.Hand;
                //lbBundleNo.Cursor = Cursors.Hand;
            }
            else
            {
                //SetupLabelCursor(lbPieceNo, Cursors.Default);
                //SetupLabelCursor(lbZone, Cursors.Default);
                //SetupLabelCursor(lbBundleNo, Cursors.Default);
                ////lbPieceNo.Cursor = Cursors.Default;
                //lbZone.Cursor = Cursors.Default;
                //lbBundleNo.Cursor = Cursors.Default;
            }
            
        }

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetLabelCurserCallback(Label lb, Cursor _cursor);


        private void SetupLabelCursor(Label lb, Cursor _cursor)
        {
            if (lb.InvokeRequired)
            {
                SetLabelCurserCallback d = new SetLabelCurserCallback(SetupLabelCursor);

                this.Invoke(d, new object[] { lb, _cursor });

            }
            else
            {
                lb.Cursor = _cursor;
            }
        }

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(Label lb, string text);

        // This method demonstrates a pattern for making thread-safe
        // calls on a Windows Forms control. 
        //
        // If the calling thread is different from the thread that
        // created the TextBox control, this method creates a
        // SetTextCallback and calls itself asynchronously using the
        // Invoke method.
        //
        // If the calling thread is the same as the thread that created
        // the TextBox control, the Text property is set directly. 

        private void SetText(Label lb, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (lb.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { lb, text});
            }
            else
            {
                lb.Text = text;
            }
        }

        private void btnArrow_Click(object sender, EventArgs e)
        {
            Popup(sender, e);
        }

        private void Popup(object sender, EventArgs e)
        {
            if (this.PopupEvent != null)
                PopupEvent(sender, e);
        }
    }
}
