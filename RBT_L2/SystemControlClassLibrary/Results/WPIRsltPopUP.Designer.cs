using System.Drawing;


namespace SystemControlClassLibrary.Popup
{
    partial class WPIRsltPopUP
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WPIRsltPopUP));
            this.txtPcs = new C1.Win.C1Input.C1TextBox();
            this.txtLength = new C1.Win.C1Input.C1TextBox();
            this.txtSize = new C1.Win.C1Input.C1TextBox();
            this.c1Label10 = new C1.Win.C1Input.C1Label();
            this.lblMeas_Date = new C1.Win.C1Input.C1Label();
            this.c1Label7 = new C1.Win.C1Input.C1Label();
            this.c1Label6 = new C1.Win.C1Input.C1Label();
            this.c1Label1 = new C1.Win.C1Input.C1Label();
            this.c1Label11 = new C1.Win.C1Input.C1Label();
            this.txtSteel = new C1.Win.C1Input.C1TextBox();
            this.c1Label12 = new C1.Win.C1Input.C1Label();
            this.c1Label13 = new C1.Win.C1Input.C1Label();
            this.txtSteel_Nm = new C1.Win.C1Input.C1TextBox();
            this.c1Label14 = new C1.Win.C1Input.C1Label();
            this.txtHeat = new C1.Win.C1Input.C1TextBox();
            this.txtTheory_Wgt = new C1.Win.C1Input.C1TextBox();
            this.c1Label16 = new C1.Win.C1Input.C1Label();
            this.c1Label2 = new C1.Win.C1Input.C1Label();
            this.cboLine_GP = new System.Windows.Forms.ComboBox();
            this.txtPoc_No = new C1.Win.C1Input.C1TextBox();
            this.dtpMeas_Date = new System.Windows.Forms.DateTimePicker();
            this.lblDel = new C1.Win.C1Input.C1Label();
            this.cboDel = new System.Windows.Forms.ComboBox();
            this.txtBundle_No = new C1.Win.C1Input.C1TextBox();
            this.txtItem = new C1.Win.C1Input.C1TextBox();
            this.lblItem = new C1.Win.C1Input.C1Label();
            this.btnReg = new C1.Win.C1Input.C1Button();
            this.btnClose = new C1.Win.C1Input.C1Button();
            this.btnConfirm1 = new C1.Win.C1Input.C1Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtPcs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMeas_Date)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSteel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSteel_Nm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTheory_Wgt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoc_No)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBundle_No)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblItem)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPcs
            // 
            this.txtPcs.EditFormat.FormatType = C1.Win.C1Input.FormatTypeEnum.GeneralNumber;
            this.txtPcs.EditFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)(((((C1.Win.C1Input.FormatInfoInheritFlags.CustomFormat | C1.Win.C1Input.FormatInfoInheritFlags.NullText) 
            | C1.Win.C1Input.FormatInfoInheritFlags.EmptyAsNull) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimStart) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd)));
            this.txtPcs.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPcs.Location = new System.Drawing.Point(115, 203);
            this.txtPcs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPcs.Name = "txtPcs";
            this.txtPcs.ReadOnly = true;
            this.txtPcs.Size = new System.Drawing.Size(137, 24);
            this.txtPcs.TabIndex = 14;
            this.txtPcs.TabStop = false;
            this.txtPcs.Tag = null;
            this.txtPcs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPcs.TextDetached = true;
            this.txtPcs.VerticalAlign = C1.Win.C1Input.VerticalAlignEnum.Middle;
            this.txtPcs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPcs_KeyDown);
            // 
            // txtLength
            // 
            this.txtLength.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtLength.Location = new System.Drawing.Point(115, 172);
            this.txtLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLength.Name = "txtLength";
            this.txtLength.ReadOnly = true;
            this.txtLength.Size = new System.Drawing.Size(137, 24);
            this.txtLength.TabIndex = 5;
            this.txtLength.TabStop = false;
            this.txtLength.Tag = null;
            this.txtLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLength.TextDetached = true;
            this.txtLength.VerticalAlign = C1.Win.C1Input.VerticalAlignEnum.Middle;
            // 
            // txtSize
            // 
            this.txtSize.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSize.Location = new System.Drawing.Point(115, 142);
            this.txtSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSize.Name = "txtSize";
            this.txtSize.ReadOnly = true;
            this.txtSize.Size = new System.Drawing.Size(137, 24);
            this.txtSize.TabIndex = 4;
            this.txtSize.TabStop = false;
            this.txtSize.Tag = null;
            this.txtSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSize.TextDetached = true;
            this.txtSize.VerticalAlign = C1.Win.C1Input.VerticalAlignEnum.Middle;
            // 
            // c1Label10
            // 
            this.c1Label10.AutoSize = true;
            this.c1Label10.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label10.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label10.Location = new System.Drawing.Point(35, 207);
            this.c1Label10.Name = "c1Label10";
            this.c1Label10.Size = new System.Drawing.Size(65, 20);
            this.c1Label10.TabIndex = 34;
            this.c1Label10.Tag = null;
            this.c1Label10.Text = "NG본수";
            this.c1Label10.TextDetached = true;
            // 
            // lblMeas_Date
            // 
            this.lblMeas_Date.AutoSize = true;
            this.lblMeas_Date.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblMeas_Date.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMeas_Date.Location = new System.Drawing.Point(312, 117);
            this.lblMeas_Date.Name = "lblMeas_Date";
            this.lblMeas_Date.Size = new System.Drawing.Size(74, 20);
            this.lblMeas_Date.TabIndex = 32;
            this.lblMeas_Date.Tag = null;
            this.lblMeas_Date.Text = "정정일자";
            this.lblMeas_Date.TextDetached = true;
            // 
            // c1Label7
            // 
            this.c1Label7.AutoSize = true;
            this.c1Label7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label7.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label7.Location = new System.Drawing.Point(35, 174);
            this.c1Label7.Name = "c1Label7";
            this.c1Label7.Size = new System.Drawing.Size(69, 20);
            this.c1Label7.TabIndex = 31;
            this.c1Label7.Tag = null;
            this.c1Label7.Text = "길이(m)";
            this.c1Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.c1Label7.TextDetached = true;
            // 
            // c1Label6
            // 
            this.c1Label6.AutoSize = true;
            this.c1Label6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label6.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label6.Location = new System.Drawing.Point(35, 146);
            this.c1Label6.Name = "c1Label6";
            this.c1Label6.Size = new System.Drawing.Size(43, 20);
            this.c1Label6.TabIndex = 30;
            this.c1Label6.Tag = null;
            this.c1Label6.Text = "규격";
            this.c1Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.c1Label6.TextDetached = true;
            // 
            // c1Label1
            // 
            this.c1Label1.AutoSize = true;
            this.c1Label1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label1.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label1.Location = new System.Drawing.Point(35, 27);
            this.c1Label1.Name = "c1Label1";
            this.c1Label1.Size = new System.Drawing.Size(43, 20);
            this.c1Label1.TabIndex = 25;
            this.c1Label1.Tag = null;
            this.c1Label1.Text = "라인";
            this.c1Label1.TextDetached = true;
            // 
            // c1Label11
            // 
            this.c1Label11.AutoSize = true;
            this.c1Label11.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label11.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label11.Location = new System.Drawing.Point(4, 84);
            this.c1Label11.Name = "c1Label11";
            this.c1Label11.Size = new System.Drawing.Size(105, 20);
            this.c1Label11.TabIndex = 49;
            this.c1Label11.Tag = null;
            this.c1Label11.Text = "재공번들번호";
            this.c1Label11.TextDetached = true;
            // 
            // txtSteel
            // 
            this.txtSteel.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSteel.Location = new System.Drawing.Point(411, 53);
            this.txtSteel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSteel.Name = "txtSteel";
            this.txtSteel.ReadOnly = true;
            this.txtSteel.Size = new System.Drawing.Size(131, 24);
            this.txtSteel.TabIndex = 7;
            this.txtSteel.TabStop = false;
            this.txtSteel.Tag = null;
            this.txtSteel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSteel.TextDetached = true;
            this.txtSteel.VerticalAlign = C1.Win.C1Input.VerticalAlignEnum.Middle;
            // 
            // c1Label12
            // 
            this.c1Label12.AutoSize = true;
            this.c1Label12.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label12.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label12.Location = new System.Drawing.Point(312, 54);
            this.c1Label12.Name = "c1Label12";
            this.c1Label12.Size = new System.Drawing.Size(43, 20);
            this.c1Label12.TabIndex = 52;
            this.c1Label12.Tag = null;
            this.c1Label12.Text = "강종";
            this.c1Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.c1Label12.TextDetached = true;
            // 
            // c1Label13
            // 
            this.c1Label13.AutoSize = true;
            this.c1Label13.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label13.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label13.Location = new System.Drawing.Point(312, 27);
            this.c1Label13.Name = "c1Label13";
            this.c1Label13.Size = new System.Drawing.Size(53, 17);
            this.c1Label13.TabIndex = 51;
            this.c1Label13.Tag = null;
            this.c1Label13.Text = "HEAT";
            this.c1Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.c1Label13.TextDetached = true;
            // 
            // txtSteel_Nm
            // 
            this.txtSteel_Nm.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSteel_Nm.Location = new System.Drawing.Point(411, 83);
            this.txtSteel_Nm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSteel_Nm.Name = "txtSteel_Nm";
            this.txtSteel_Nm.ReadOnly = true;
            this.txtSteel_Nm.Size = new System.Drawing.Size(131, 24);
            this.txtSteel_Nm.TabIndex = 8;
            this.txtSteel_Nm.TabStop = false;
            this.txtSteel_Nm.Tag = null;
            this.txtSteel_Nm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSteel_Nm.TextDetached = true;
            this.txtSteel_Nm.VerticalAlign = C1.Win.C1Input.VerticalAlignEnum.Middle;
            // 
            // c1Label14
            // 
            this.c1Label14.AutoSize = true;
            this.c1Label14.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label14.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label14.Location = new System.Drawing.Point(312, 84);
            this.c1Label14.Name = "c1Label14";
            this.c1Label14.Size = new System.Drawing.Size(58, 20);
            this.c1Label14.TabIndex = 55;
            this.c1Label14.Tag = null;
            this.c1Label14.Text = "강종명";
            this.c1Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.c1Label14.TextDetached = true;
            // 
            // txtHeat
            // 
            this.txtHeat.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtHeat.Location = new System.Drawing.Point(411, 23);
            this.txtHeat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtHeat.Name = "txtHeat";
            this.txtHeat.ReadOnly = true;
            this.txtHeat.Size = new System.Drawing.Size(131, 24);
            this.txtHeat.TabIndex = 6;
            this.txtHeat.TabStop = false;
            this.txtHeat.Tag = null;
            this.txtHeat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtHeat.TextDetached = true;
            this.txtHeat.VerticalAlign = C1.Win.C1Input.VerticalAlignEnum.Middle;
            // 
            // txtTheory_Wgt
            // 
            this.txtTheory_Wgt.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTheory_Wgt.Location = new System.Drawing.Point(411, 142);
            this.txtTheory_Wgt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTheory_Wgt.Name = "txtTheory_Wgt";
            this.txtTheory_Wgt.ReadOnly = true;
            this.txtTheory_Wgt.Size = new System.Drawing.Size(131, 24);
            this.txtTheory_Wgt.TabIndex = 16;
            this.txtTheory_Wgt.TabStop = false;
            this.txtTheory_Wgt.Tag = null;
            this.txtTheory_Wgt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTheory_Wgt.TextDetached = true;
            this.txtTheory_Wgt.VerticalAlign = C1.Win.C1Input.VerticalAlignEnum.Middle;
            // 
            // c1Label16
            // 
            this.c1Label16.AutoSize = true;
            this.c1Label16.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label16.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label16.Location = new System.Drawing.Point(312, 146);
            this.c1Label16.Name = "c1Label16";
            this.c1Label16.Size = new System.Drawing.Size(103, 20);
            this.c1Label16.TabIndex = 62;
            this.c1Label16.Tag = null;
            this.c1Label16.Text = "이론중량(kg)";
            this.c1Label16.TextDetached = true;
            // 
            // c1Label2
            // 
            this.c1Label2.AutoSize = true;
            this.c1Label2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label2.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label2.Location = new System.Drawing.Point(35, 57);
            this.c1Label2.Name = "c1Label2";
            this.c1Label2.Size = new System.Drawing.Size(45, 17);
            this.c1Label2.TabIndex = 66;
            this.c1Label2.Tag = null;
            this.c1Label2.Text = "POC";
            this.c1Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.c1Label2.TextDetached = true;
            // 
            // cboLine_GP
            // 
            this.cboLine_GP.Enabled = false;
            this.cboLine_GP.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.cboLine_GP.FormattingEnabled = true;
            this.cboLine_GP.Location = new System.Drawing.Point(115, 23);
            this.cboLine_GP.Name = "cboLine_GP";
            this.cboLine_GP.Size = new System.Drawing.Size(137, 23);
            this.cboLine_GP.TabIndex = 0;
            this.cboLine_GP.SelectedIndexChanged += new System.EventHandler(this.cboLine_GP_SelectedIndexChanged);
            // 
            // txtPoc_No
            // 
            this.txtPoc_No.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPoc_No.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPoc_No.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtPoc_No.Location = new System.Drawing.Point(115, 52);
            this.txtPoc_No.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPoc_No.Name = "txtPoc_No";
            this.txtPoc_No.Size = new System.Drawing.Size(137, 24);
            this.txtPoc_No.TabIndex = 2;
            this.txtPoc_No.Tag = null;
            this.txtPoc_No.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPoc_No.TextDetached = true;
            this.txtPoc_No.VerticalAlign = C1.Win.C1Input.VerticalAlignEnum.Middle;
            this.txtPoc_No.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPoc_No_KeyDown);
            // 
            // dtpMeas_Date
            // 
            this.dtpMeas_Date.CustomFormat = "";
            this.dtpMeas_Date.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.dtpMeas_Date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpMeas_Date.Location = new System.Drawing.Point(411, 112);
            this.dtpMeas_Date.Name = "dtpMeas_Date";
            this.dtpMeas_Date.Size = new System.Drawing.Size(131, 24);
            this.dtpMeas_Date.TabIndex = 9;
            this.dtpMeas_Date.Value = new System.DateTime(2016, 8, 25, 9, 27, 0, 0);
            // 
            // lblDel
            // 
            this.lblDel.AutoSize = true;
            this.lblDel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblDel.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDel.Location = new System.Drawing.Point(312, 175);
            this.lblDel.Name = "lblDel";
            this.lblDel.Size = new System.Drawing.Size(74, 20);
            this.lblDel.TabIndex = 71;
            this.lblDel.Tag = null;
            this.lblDel.Text = "삭제여부";
            this.lblDel.TextDetached = true;
            // 
            // cboDel
            // 
            this.cboDel.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.cboDel.FormattingEnabled = true;
            this.cboDel.Location = new System.Drawing.Point(411, 172);
            this.cboDel.Name = "cboDel";
            this.cboDel.Size = new System.Drawing.Size(131, 23);
            this.cboDel.TabIndex = 72;
            this.cboDel.SelectedIndexChanged += new System.EventHandler(this.cboDel_SelectedIndexChanged);
            // 
            // txtBundle_No
            // 
            this.txtBundle_No.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBundle_No.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtBundle_No.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtBundle_No.Location = new System.Drawing.Point(115, 82);
            this.txtBundle_No.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBundle_No.Name = "txtBundle_No";
            this.txtBundle_No.Size = new System.Drawing.Size(137, 24);
            this.txtBundle_No.TabIndex = 73;
            this.txtBundle_No.Tag = null;
            this.txtBundle_No.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBundle_No.TextDetached = true;
            this.txtBundle_No.VerticalAlign = C1.Win.C1Input.VerticalAlignEnum.Middle;
            // 
            // txtItem
            // 
            this.txtItem.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtItem.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtItem.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtItem.Location = new System.Drawing.Point(115, 113);
            this.txtItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtItem.Name = "txtItem";
            this.txtItem.ReadOnly = true;
            this.txtItem.Size = new System.Drawing.Size(137, 24);
            this.txtItem.TabIndex = 74;
            this.txtItem.TabStop = false;
            this.txtItem.Tag = null;
            this.txtItem.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtItem.TextDetached = true;
            this.txtItem.VerticalAlign = C1.Win.C1Input.VerticalAlignEnum.Middle;
            // 
            // lblItem
            // 
            this.lblItem.AutoSize = true;
            this.lblItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblItem.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblItem.Location = new System.Drawing.Point(35, 114);
            this.lblItem.Name = "lblItem";
            this.lblItem.Size = new System.Drawing.Size(43, 20);
            this.lblItem.TabIndex = 75;
            this.lblItem.Tag = null;
            this.lblItem.Text = "품목";
            this.lblItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblItem.TextDetached = true;
            // 
            // btnReg
            // 
            this.btnReg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnReg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnReg.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnReg.Image = ((System.Drawing.Image)(resources.GetObject("btnReg.Image")));
            this.btnReg.Location = new System.Drawing.Point(182, 270);
            this.btnReg.Name = "btnReg";
            this.btnReg.Size = new System.Drawing.Size(98, 30);
            this.btnReg.TabIndex = 77;
            this.btnReg.Text = "저장";
            this.btnReg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReg.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReg.UseVisualStyleBackColor = true;
            this.btnReg.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = true;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(288, 270);
            this.btnClose.Margin = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(98, 30);
            this.btnClose.TabIndex = 78;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnConfirm1
            // 
            this.btnConfirm1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnConfirm1.Image = global::SystemControlClassLibrary.Properties.Resources.filesave1;
            this.btnConfirm1.Location = new System.Drawing.Point(261, 203);
            this.btnConfirm1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfirm1.Name = "btnConfirm1";
            this.btnConfirm1.Size = new System.Drawing.Size(32, 24);
            this.btnConfirm1.TabIndex = 79;
            this.btnConfirm1.UseVisualStyleBackColor = true;
            this.btnConfirm1.Click += new System.EventHandler(this.btnConfirm1_Click);
            // 
            // WPIRsltPopUP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(587, 309);
            this.Controls.Add(this.btnConfirm1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReg);
            this.Controls.Add(this.txtItem);
            this.Controls.Add(this.lblItem);
            this.Controls.Add(this.txtBundle_No);
            this.Controls.Add(this.cboDel);
            this.Controls.Add(this.lblDel);
            this.Controls.Add(this.dtpMeas_Date);
            this.Controls.Add(this.txtPoc_No);
            this.Controls.Add(this.cboLine_GP);
            this.Controls.Add(this.c1Label2);
            this.Controls.Add(this.txtTheory_Wgt);
            this.Controls.Add(this.c1Label16);
            this.Controls.Add(this.txtHeat);
            this.Controls.Add(this.txtSteel_Nm);
            this.Controls.Add(this.c1Label14);
            this.Controls.Add(this.txtSteel);
            this.Controls.Add(this.c1Label12);
            this.Controls.Add(this.c1Label13);
            this.Controls.Add(this.c1Label11);
            this.Controls.Add(this.txtPcs);
            this.Controls.Add(this.txtLength);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.c1Label10);
            this.Controls.Add(this.lblMeas_Date);
            this.Controls.Add(this.c1Label7);
            this.Controls.Add(this.c1Label6);
            this.Controls.Add(this.c1Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "WPIRsltPopUP";
            this.Text = "재공번들등록";
            this.Load += new System.EventHandler(this.WPIRsltPopUP_Load);
            this.Shown += new System.EventHandler(this.WPIRsltPopUP_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.txtPcs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMeas_Date)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSteel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSteel_Nm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTheory_Wgt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoc_No)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBundle_No)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblItem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private C1.Win.C1Input.C1TextBox txtPcs;
        private C1.Win.C1Input.C1TextBox txtLength;
        private C1.Win.C1Input.C1TextBox txtSize;
        private C1.Win.C1Input.C1Label c1Label10;
        private C1.Win.C1Input.C1Label lblMeas_Date;
        private C1.Win.C1Input.C1Label c1Label7;
        private C1.Win.C1Input.C1Label c1Label6;
        private C1.Win.C1Input.C1Label c1Label1;
        private C1.Win.C1Input.C1Label c1Label11;
        private C1.Win.C1Input.C1TextBox txtSteel;
        private C1.Win.C1Input.C1Label c1Label12;
        private C1.Win.C1Input.C1Label c1Label13;
        private C1.Win.C1Input.C1TextBox txtSteel_Nm;
        private C1.Win.C1Input.C1Label c1Label14;
        private C1.Win.C1Input.C1TextBox txtHeat;
        private C1.Win.C1Input.C1TextBox txtTheory_Wgt;
        private C1.Win.C1Input.C1Label c1Label16;
        private C1.Win.C1Input.C1Label c1Label2;
        private System.Windows.Forms.ComboBox cboLine_GP;
        private C1.Win.C1Input.C1TextBox txtPoc_No;
        private System.Windows.Forms.DateTimePicker dtpMeas_Date;
        private C1.Win.C1Input.C1Label lblDel;
        private System.Windows.Forms.ComboBox cboDel;
        private C1.Win.C1Input.C1TextBox txtBundle_No;
        private C1.Win.C1Input.C1TextBox txtItem;
        private C1.Win.C1Input.C1Label lblItem;
        private C1.Win.C1Input.C1Button btnReg;
        private C1.Win.C1Input.C1Button btnClose;
        private C1.Win.C1Input.C1Button btnConfirm1;
    }
}