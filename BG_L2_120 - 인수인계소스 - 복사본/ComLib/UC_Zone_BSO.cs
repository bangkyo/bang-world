using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComLib
{
    public partial class UC_Zone_BSO : UserControl
    {
        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();

        public event EventHandler PopupEvent;

        public UC_Zone_BSO()
        {
            InitializeComponent();

            SetFontColor();
        }

        private void SetFontColor()
        {
            //throw new NotImplementedException();
        }

        [Description("Zone CD"), Category("Contents")]
        public string ZoneCD
        {
            get
            {
                return lbZone.Text;
            }
            set
            {

                lbZone.Text = value;
                this.Invalidate();
            }
        }


        [Description("Bloom No0"), Category("Contents")]
        public string BloomNo0
        {
            get
            {
                return lb_BloomNo0.Text;
            }
            set
            {
                //lbBundleNo.Text = value;
                SetText(lb_BloomNo0, value);

                if (value.Length > 0)
                {
                    SetCursorState(true);

                }
                else
                {
                    SetCursorState(false);
                }
                this.Invalidate();
            }
        }

        [Description("Bloom No1"), Category("Contents")]
        public string BloomNo1
        {
            get
            {
                return lb_BloomNo1.Text;
            }
            set
            {
                //lbBundleNo.Text = value;
                SetText(lb_BloomNo1, value);

                if (value.Length > 0)
                {
                    SetCursorState(true);

                }
                else
                {
                    SetCursorState(false);
                }
                this.Invalidate();
            }
        }

        [Description("Bloom No2"), Category("Contents")]
        public string BloomNo2
        {
            get
            {
                return lb_BloomNo2.Text;
            }
            set
            {
                //lbBundleNo.Text = value;
                SetText(lb_BloomNo2, value);

                if (value.Length > 0)
                {
                    SetCursorState(true);

                }
                else
                {
                    SetCursorState(false);
                }
                this.Invalidate();
            }
        }

        [Description("Bloom No3"), Category("Contents")]
        public string BloomNo3
        {
            get
            {
                return lb_BloomNo3.Text;
            }
            set
            {
                //lbBundleNo.Text = value;
                SetText(lb_BloomNo3, value);

                if (value.Length > 0)
                {
                    SetCursorState(true);

                }
                else
                {
                    SetCursorState(false);
                }
                this.Invalidate();
            }
        }

        [Description("Bloom No4"), Category("Contents")]
        public string BloomNo4
        {
            get
            {
                return lb_BloomNo4.Text;
            }
            set
            {
                //lbBundleNo.Text = value;
                SetText(lb_BloomNo4, value);

                if (value.Length > 0)
                {
                    SetCursorState(true);

                }
                else
                {
                    SetCursorState(false);
                }
                this.Invalidate();
            }
        }

        [Description("Bloom No5"), Category("Contents")]
        public string BloomNo5
        {
            get
            {
                return lb_BloomNo5.Text;
            }
            set
            {
                //lbBundleNo.Text = value;
                SetText(lb_BloomNo5, value);

                if (value.Length > 0)
                {
                    SetCursorState(true);

                }
                else
                {
                    SetCursorState(false);
                }
                this.Invalidate();
            }
        }

        [Description("Bloom No6"), Category("Contents")]
        public string BloomNo6
        {
            get
            {
                return lb_BloomNo6.Text;
            }
            set
            {
                //lbBundleNo.Text = value;
                SetText(lb_BloomNo6, value);

                if (value.Length > 0)
                {
                    SetCursorState(true);

                }
                else
                {
                    SetCursorState(false);
                }
                this.Invalidate();
            }
        }

        [Description("Bloom No7"), Category("Contents")]
        public string BloomNo7
        {
            get
            {
                return lb_BloomNo7.Text;
            }
            set
            {
                //lbBundleNo.Text = value;
                SetText(lb_BloomNo7, value);

                if (value.Length > 0)
                {
                    SetCursorState(true);

                }
                else
                {
                    SetCursorState(false);
                }
                this.Invalidate();
            }
        }

        [Description("Bloom Cnt(Count)"), Category("Contents")]
        public string BloomCnt
        {
            get
            {
                return lbBloomCnt.Text;
            }
            set
            {
                //lbPieceNo.Text = value;
                SetText(lbBloomCnt, value);
                if (vf.CInt2(value) > 0)
                {
                    SetCursorState(true);

                }
                else
                {
                    SetCursorState(false);
                }
                this.Invalidate();

            }
        }

        [Description("Zone ForeColor"), Category("Contents")]
        public Color ZoneForeColor
        {
            get
            {
                return lbZone.ForeColor;
            }
            set
            {
                lbZone.ForeColor = value;
                this.Invalidate();
            }
        }

        [Description("Zone BackColor"), Category("Contents")]
        public Color ZoneBackColor
        {
            get
            {
                return lbZone.BackColor;
            }
            set
            {
                lbZone.BackColor = value;
                this.Invalidate();
            }
        }


        protected void lbBundleNo_Click(object sender, EventArgs e)
        {
            //ClickEvent(sender, e);
            //if (this.UpdateParentPage != null)
            //    UpdateParentPage(sender, e);
        }

        protected void UC_Zone_Click(object sender, EventArgs e)
        {
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

        private void lbBundleNo_DoubleClick(object sender, EventArgs e)
        {
            if (this.PopupEvent != null)
                PopupEvent(sender, e);
        }

        private void SetCursorState(bool isOn)
        {
            if (isOn)
            {
                SetupLabelCursor(lbZone, Cursors.Hand);
                SetupLabelCursor(lb_BloomNo7, Cursors.Hand);
                SetupLabelCursor(lb_BloomNo6, Cursors.Hand);
                SetupLabelCursor(lb_BloomNo5, Cursors.Hand);
                SetupLabelCursor(lb_BloomNo4, Cursors.Hand);
                SetupLabelCursor(lb_BloomNo3, Cursors.Hand);
                SetupLabelCursor(lb_BloomNo2, Cursors.Hand);
                SetupLabelCursor(lb_BloomNo1, Cursors.Hand);
                //lbPieceNo.Cursor = Cursors.Hand;
                //lbZone.Cursor = Cursors.Hand;
                //lbBundleNo.Cursor = Cursors.Hand;
            }
            else
            {
                SetupLabelCursor(lbZone, Cursors.Default);
                SetupLabelCursor(lb_BloomNo7, Cursors.Default);
                SetupLabelCursor(lb_BloomNo6, Cursors.Default);
                SetupLabelCursor(lb_BloomNo5, Cursors.Default);
                SetupLabelCursor(lb_BloomNo4, Cursors.Default);
                SetupLabelCursor(lb_BloomNo3, Cursors.Default);
                SetupLabelCursor(lb_BloomNo2, Cursors.Default);
                SetupLabelCursor(lb_BloomNo1, Cursors.Default);
                //lbPieceNo.Cursor = Cursors.Default;
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
                this.Invoke(d, new object[] { lb, text });
            }
            else
            {
                lb.Text = text;
            }
        }


        private void UC_Zone_BSO_DoubleClick(object sender, EventArgs e)
        {
            if (this.PopupEvent != null )
                PopupEvent(sender, e);
        }

        private void lbZone_DoubleClick(object sender, EventArgs e)
        {
            if (this.PopupEvent != null)
                PopupEvent(sender, e);
        }
    }
}
