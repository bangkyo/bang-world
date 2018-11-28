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
    public partial class UC_Zone_BS_R : UserControl
    {
        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();

        public event EventHandler PopupEvent;

        public UC_Zone_BS_R()
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


        [Description("HEAT DATA1"), Category("Contents")]
        public string HEAT_DATA1
        {
            get
            {
                return lb_HEAT_DATA1.Text;
            }
            set
            {
                //lbBundleNo.Text = value;
                SetText(lb_HEAT_DATA1, value);

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

        [Description("HEAT DATA2"), Category("Contents")]
        public string HEAT_DATA2
        {
            get
            {
                return lb_HEAT_DATA2.Text;
            }
            set
            {
                //lbBundleNo.Text = value;
                SetText(lb_HEAT_DATA2, value);

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

        [Description("HEAT DATA3"), Category("Contents")]
        public string HEAT_DATA3
        {
            get
            {
                return lb_HEAT_DATA3.Text;
            }
            set
            {
                //lbBundleNo.Text = value;
                SetText(lb_HEAT_DATA3, value);

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

        [Description("HEAT DATA4"), Category("Contents")]
        public string HEAT_DATA4
        {
            get
            {
                return lb_HEAT_DATA4.Text;
            }
            set
            {
                //lbBundleNo.Text = value;
                SetText(lb_HEAT_DATA4, value);

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

        [Description("HEAT DATA5"), Category("Contents")]
        public string HEAT_DATA5
        {
            get
            {
                return lb_HEAT_DATA5.Text;
            }
            set
            {
                //lbBundleNo.Text = value;
                SetText(lb_HEAT_DATA5, value);

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
                SetupLabelCursor(lb_HEAT_DATA5, Cursors.Hand);
                SetupLabelCursor(lb_HEAT_DATA4, Cursors.Hand);
                SetupLabelCursor(lb_HEAT_DATA3, Cursors.Hand);
                SetupLabelCursor(lb_HEAT_DATA2, Cursors.Hand);
                SetupLabelCursor(lb_HEAT_DATA1, Cursors.Hand);
                //lbPieceNo.Cursor = Cursors.Hand;
                //lbZone.Cursor = Cursors.Hand;
                //lbBundleNo.Cursor = Cursors.Hand;
            }
            else
            {
                SetupLabelCursor(lbZone, Cursors.Default);
                SetupLabelCursor(lb_HEAT_DATA5, Cursors.Default);
                SetupLabelCursor(lb_HEAT_DATA4, Cursors.Default);
                SetupLabelCursor(lb_HEAT_DATA3, Cursors.Default);
                SetupLabelCursor(lb_HEAT_DATA2, Cursors.Default);
                SetupLabelCursor(lb_HEAT_DATA1, Cursors.Default);
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


        private void UC_Zone_BS_R_DoubleClick(object sender, EventArgs e)
        {
            if (this.PopupEvent != null)
                PopupEvent(sender, e);
        }

        private void lbZone_DoubleClick(object sender, EventArgs e)
        {
            if (this.PopupEvent != null)
                PopupEvent(sender, e);
        }
    }
}
