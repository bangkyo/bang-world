using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLib;

namespace SystemControlClassLibrary.UC
{
    public partial class UC_Zone1 : UserControl
    {

        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();

        public event EventHandler PopupEvent;
        public UC_Zone1()
        {
            InitializeComponent();

            //SetFontColor();
        }

        private void SetFontColor()
        {

            lbZone.BackColor = Color.FromArgb(0, 122, 204);
            lbZone.ForeColor = Color.White;


            //throw new NotImplementedException();
            lbZone.BackColor = Color.FromArgb(218, 233, 245);
            lbZone.ForeColor = Color.Blue ;


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

        [Description("Piece No(Count)"), Category("Contents")]
        public string PCS
        {
            get
            {
                return lbPieceNo.Text;
            }
            set
            {
                lbPieceNo.Text = value;

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

        [Description("Mill No(압연번들번호)"), Category("Contents")]
        public string MillNo
        {
            get
            {
                return lbBundleNo.Text;
            }
            set
            {
                lbBundleNo.Text = value;


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

        private void SetCursorState(bool isOn)
        {
            if (isOn)
            {
                lbPieceNo.Cursor = Cursors.Hand;
                lbZone.Cursor = Cursors.Hand;
                lbBundleNo.Cursor = Cursors.Hand;
            }
            else
            {
                lbPieceNo.Cursor = System.Windows.Forms.Cursors.Default;
                lbZone.Cursor = System.Windows.Forms.Cursors.Default;
                lbBundleNo.Cursor = System.Windows.Forms.Cursors.Default;
            }

        }


        private void lbZone_DoubleClick(object sender, EventArgs e)
        {
            Popup(sender, e);
        }


        private void lbBundleNo_DoubleClick(object sender, EventArgs e)
        {
            Popup(sender, e);
        }

        private void lbPieceNo_DoubleClick(object sender, EventArgs e)
        {
            Popup(sender, e);
        }

        private void Popup(object sender, EventArgs e)
        {
            if (this.PopupEvent != null && lbBundleNo.Text.Length > 0)
                PopupEvent(sender, e);
        }
    }
}
