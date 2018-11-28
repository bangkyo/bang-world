using System;
using System.ComponentModel;
using System.Windows.Forms;
using ComLib;
using System.Drawing;

namespace ComLib
{
    public partial class UC_KeyValueTiTle1 : UserControl
    {
        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();

        public event EventHandler PopupEvent;

        private string plcCD= "";

        public UC_KeyValueTiTle1()
        {
            InitializeComponent();

            //SetFontColor();

            //SetLabel();
        }

        private void SetLabel()
        {


            // Set the border to a three-dimensional border.
            lbTitle.BorderStyle = BorderStyle.Fixed3D;
            // Set the ImageList to use for displaying an image.
            //lbTitle.ImageList = imageList1;
            // Use the second image in imageList1.
            //lbTitle.ImageIndex = 1;
            // Align the image to the top left corner.
            //lbTitle.ImageAlign = ContentAlignment.TopLeft;

            // Specify that the text can display mnemonic characters.
            //lbTitle.UseMnemonic = true;
            // Set the text of the control and specify a mnemonic character.
            //lbTitle.Text = "First &Name:";

            /* Set the size of the control based on the PreferredHeight and PreferredWidth values. */
            lbTitle.Size = new Size(lbTitle.PreferredWidth, lbTitle.PreferredHeight);

        }

        [Description("TITLE BackColor"), Category("Contents")]
        public Color TitleBackColor
        {
            get
            {
                return lbTitle.BackColor;
            }
            set
            {
                lbTitle.BackColor = value;
                this.Invalidate();
            }
        }

        [Description("TITLE ForeColor"), Category("Contents")]
        public Color TitleForeColor
        {
            get
            {
                return lbTitle.ForeColor;
            }
            set
            {
                lbTitle.ForeColor = value;
                this.Invalidate();
            }
        }

        [Description("Title Text"), Category("Contents")]
        public string TITEL_TEXT
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



        //[Description("PLC_ITEM_NM"), Category("Contents")]
        //public string PLC_ITEM_NM
        //{
        //    get
        //    {
        //        return lbTitle.Text;
        //    }
        //    set
        //    {
        //        SetText(lbTitle, value);
        //        this.Invalidate();

        //    }
        //}

        //[Description("PLC_ITEM_VALUE"), Category("Contents")]
        //public string PLC_ITEM_VALUE
        //{
        //    get
        //    {
        //        return lbValue.Text;
        //    }
        //    set
        //    {
        //        SetText(lbValue, value);

        //        this.Invalidate();
        //    }
        //}

        //[Description("PLC_ITEM_UOM"), Category("Contents")]
        //public string PLC_ITEM_UOM
        //{
        //    get
        //    {
        //        return lbUom.Text;
        //    }
        //    set
        //    {
        //        SetText(lbUom, value);
        //        this.Invalidate();
        //    }
        //}

        //[Description("PLC_ITEM_STANDARD"), Category("Contents")]
        //public string ZoneBackColor
        //{
        //    get
        //    {
        //        return lbLimit.Text;
        //    }
        //    set
        //    {
        //        SetText(lbLimit, value);
        //        this.Invalidate();
        //    }
        //}




        //// This delegate enables asynchronous calls for setting
        //// the text property on a TextBox control.
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

        //// This delegate enables asynchronous calls for setting
        //// the text property on a TextBox control.
        delegate void SetTextCallback(Label lb, string text);

        //// This method demonstrates a pattern for making thread-safe
        //// calls on a Windows Forms control. 
        ////
        //// If the calling thread is different from the thread that
        //// created the TextBox control, this method creates a
        //// SetTextCallback and calls itself asynchronously using the
        //// Invoke method.
        ////
        //// If the calling thread is the same as the thread that created
        //// the TextBox control, the Text property is set directly. 

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

        private void UC_KeyValueTiTle1_Resize(object sender, EventArgs e)
        {
            lbTitle.Height = this.Size.Height - 2;
            lbTitle.Width = this.Size.Width - 2;
            this.Invalidate();
        }


        //private void lbZone_DoubleClick(object sender, EventArgs e)
        //{
        //    if (this.PopupEvent != null && lbUom.Text.Length > 0)
        //        PopupEvent(sender, e);
        //}

        //private void lbPieceNo_DoubleClick(object sender, EventArgs e)
        //{
        //    if (this.PopupEvent != null && lbUom.Text.Length > 0)
        //        PopupEvent(sender, e);
        //}
    }
}
