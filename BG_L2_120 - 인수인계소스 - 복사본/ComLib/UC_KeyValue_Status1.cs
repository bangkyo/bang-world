﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using ComLib;
using System.Drawing;

namespace ComLib
{
    public partial class UC_KeyValue_Status1 : UserControl
    {
        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();

        public event EventHandler PopupEvent;

        private string plcCD= "";
        private int plcState = 0;
        private string plcEq_Stat = "M";

        public UC_KeyValue_Status1()
        {
            InitializeComponent();

            SetLabel();
            
            SetFontColor();

            

            PLC_ITEM_LIMIT_VISIBLE = true;
        }

        private void SetLabel()
        {
            //var path = new System.Drawing.Drawing2D.GraphicsPath();
            //path.AddEllipse(0, 0, lbValue.Width, lbValue.Height);
            //lbValue.Region = new Region(path);
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

        [Description("PLC_EQ_STAT"), Category("Contents")]
        public string PLC_EQ_STAT
        {
            get
            {
                return plcEq_Stat;
            }
            set
            {
                plcEq_Stat = value;
                //this.Invalidate();
            }
        }

        [Description("PLC_STATE"), Category("Contents")]
        public int PLC_STATE
        {
            get
            {
                return plcState;
            }
            set
            {
                //SetText(lbValue, value);
                SetImage(plcEq_Stat,pictureBox1, value);
                plcState = value;
                this.Invalidate();
            }
        }

        private void SetImage(string plcEq_Stat, PictureBox pictureBox1, int value)
        {
            if (plcEq_Stat == "M")
            {
                switch (value)
                {
                    case 0:
                        pictureBox1.BackgroundImage =  global::ComLib.Properties.Resources.M_Green;
                        break;
                    case 1:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.M_Red;
                        break;
                    case 2:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.M_Red;
                        break;
                    case 3:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.M_Yellow;
                        break;
                    default:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.M_Gray;
                        break;
                }
            }
            else if (plcEq_Stat == "P")
            {
                switch (value)
                {
                    case 0:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.P_Green;
                        break;
                    case 1:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.P_Red;
                        break;
                    case 2:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.P_Red;
                        break;
                    case 3:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.P_Yellow;
                        break;
                    default:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.P_Gray;
                        break;
                }
            }
            else if (plcEq_Stat == "H")
            {
                switch (value)
                {
                    case 0:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.H_Green;
                        break;
                    case 1:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.H_Red;
                        break;
                    case 2:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.H_Red;
                        break;
                    case 3:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.H_Yellow;
                        break;
                    default:
                        pictureBox1.BackgroundImage = global::ComLib.Properties.Resources.H_Gray;
                        break;
                }
            }
        }



        private void SetBackColor(Label lbValue, int value)
        {
            switch (value)
            {
                case 0:
                    lbValue.BackColor = Color.Gray;
                    break;
                case 1:
                    lbValue.BackColor = Color.Red;
                    break;
                case 2:
                    lbValue.BackColor = Color.Green;
                    break;
                case 3:
                    lbValue.BackColor = Color.Yellow;
                    break;
                default:
                    lbValue.BackColor = Color.Gray;
                    break;
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



        [Description("PLC_ITEM_UOM"), Category("Contents")]
        public string PLC_ITEM_UOM
        {
            get
            {
                return lbUom.Text;
            }
            set
            {
                SetText(lbUom, value);
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
        //[Description("Value BackColor"), Category("Contents")]
        //public Color ValueBackColor
        //{
        //    get
        //    {
        //        return lbValue.BackColor;
        //    }
        //    set
        //    {
        //        lbValue.BackColor = value;
        //        this.Invalidate();
        //    }
        //}
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
                    this.Width = 416;
                }
                else
                {
                    lb.Visible = false;
                    this.Width = 416 - 121 -2;
                }
                
            }
        }

        private void lbZone_DoubleClick(object sender, EventArgs e)
        {
            if (this.PopupEvent != null && lbUom.Text.Length > 0)
                PopupEvent(sender, e);
        }

        private void lbPieceNo_DoubleClick(object sender, EventArgs e)
        {
            if (this.PopupEvent != null && lbUom.Text.Length > 0)
                PopupEvent(sender, e);
        }
    }
}
