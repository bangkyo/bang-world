using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemControlClassLibrary.UC
{
    public partial class UC_WorkShow : UserControl
    {
        public UC_WorkShow()
        {
            InitializeComponent();
        }

        public event EventHandler PopupPanelEvent;
        public string WorkType
        {
            get
            {
                return tbWorkType.Text;
            }

            set
            {
                //tbWorkType.Text = value;
                SetText(tbWorkType, value);
                this.Invalidate();
            }
        }

        public string WorkTeam
        {
            get
            {
                return tbWorkTeam.Text;
            }

            set
            {
                //tbWorkTeam.Text = value;
                SetText(tbWorkTeam, value);
                this.Invalidate();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            PopupPanelEvent?.Invoke(sender, e);

        }

        private void UC_WorkShow_Click(object sender, EventArgs e)
        {
            
        }

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(TextBox tb, string text);

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

        private void SetText(TextBox tb, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (tb.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { tb, text });
            }
            else
            {
                tb.Text = text;
            }
        }

    }
}
