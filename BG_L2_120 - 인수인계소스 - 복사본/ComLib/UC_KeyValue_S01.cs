using System;
using System.ComponentModel;
using System.Windows.Forms;
using ComLib;
using System.Drawing;

namespace ComLib
{
    public partial class UC_KeyValue_S01 : UserControl
    {
        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();

        public event EventHandler PopupEvent;

        private string plcCD= "";

        public UC_KeyValue_S01()
        {
            InitializeComponent();

            SetFontColor();

            PLC_ITEM_LIMIT_VISIBLE = true;
        }

        private void SetFontColor()
        {
            //throw new NotImplementedException();
        }

        [Description("PLC_CD"), Category("Contents")]
        public string PLC_CD
        {
            get
            {
                return plcCD;
            }
            set
            {
                plcCD = value;
                //this.Invalidate();
            }
        }

        [Description("PLC_ITEM_NM"), Category("Contents")]
        public string PLC_ITEM_NM
        {
            get
            {
                return lbTitle.Text;
            }
            set
            {
                SetText(lbTitle, value);
                this.Invalidate();

            }
        }

        [Description("PLC_ITEM_VALUE"), Category("Contents")]
        public string PLC_ITEM_VALUE
        {
            get
            {
                return lbValue.Text;
            }
            set
            {
                SetText(lbValue, value);

                this.Invalidate();
            }
        }



        [Description("PLC_ITEM_LIMIT"), Category("Contents")]
        public string PLC_ITEM_LIMIT
        {
            get
            {
                return lbLimit.Text;
            }
            set
            {
                SetText(lbLimit, value);
                this.Invalidate();
            }
        }

        [Description("PLC_ITEM_LIMIT_VISIBLE"), Category("Contents")]
        public bool PLC_ITEM_LIMIT_VISIBLE
        {
            get
            {
                return lbLimit.Visible ;
            }
            set
            {
                SetLabelVisible(lbLimit, value);
                this.Invalidate();
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
        [Description("Value BackColor"), Category("Contents")]
        public Color ValueBackColor
        {
            get
            {
                return lbValue.BackColor;
            }
            set
            {
                lbValue.BackColor = value;
                this.Invalidate();
            }
        }

        [Description("Value ForeColor"), Category("Contents")]
        public Color ValueForeColor
        {
            get
            {
                return lbValue.ForeColor;
            }
            set
            {
                lbValue.ForeColor = value;
                this.Invalidate();
            }
        }
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(Label lb, string text);
        delegate void SetVisibleCallback(Label lb, bool visible);


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

        private void SetLabelVisible(Label lb, bool visible)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (lb.InvokeRequired)
            {
                SetVisibleCallback d = new SetVisibleCallback(SetLabelVisible);
                this.Invoke(d, new object[] { lb, visible });
            }
            else
            {
                if (visible)
                {
                    lb.Visible = true;
                    //this.Width = 416;
                }
                else
                {
                    lb.Visible = false;
                    //this.Width = 416 - 121 -2;
                }
                
            }
        }

        private void UC_KeyValue_S01_Resize(object sender, EventArgs e)
        {
            //lbTitle.Height = this.Size.Height - 2;
            lbTitle.Width = this.Size.Width - 2;
            //lbValue.Height = this.Size.Height - 2;
            lbValue.Width = this.Size.Width - 2;
            this.Invalidate();
        }
    }
}
