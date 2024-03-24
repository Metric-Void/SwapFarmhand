namespace SwapFarmhand
{
    partial class Import
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMainFarmhand = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstFarmHands = new System.Windows.Forms.ListBox();
            this.btnSwap = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblMainFarmhand);
            this.groupBox1.Location = new System.Drawing.Point(33, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 59);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Main Player";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // lblMainFarmhand
            // 
            this.lblMainFarmhand.AutoSize = true;
            this.lblMainFarmhand.Location = new System.Drawing.Point(19, 23);
            this.lblMainFarmhand.Name = "lblMainFarmhand";
            this.lblMainFarmhand.Size = new System.Drawing.Size(63, 20);
            this.lblMainFarmhand.TabIndex = 0;
            this.lblMainFarmhand.Text = "Loading";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstFarmHands);
            this.groupBox2.Location = new System.Drawing.Point(33, 146);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(310, 209);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Farm Hands";
            // 
            // lstFarmHands
            // 
            this.lstFarmHands.FormattingEnabled = true;
            this.lstFarmHands.ItemHeight = 20;
            this.lstFarmHands.Items.AddRange(new object[] {
            "Loading"});
            this.lstFarmHands.Location = new System.Drawing.Point(19, 26);
            this.lstFarmHands.Name = "lstFarmHands";
            this.lstFarmHands.Size = new System.Drawing.Size(270, 164);
            this.lstFarmHands.TabIndex = 0;
            // 
            // btnSwap
            // 
            this.btnSwap.Location = new System.Drawing.Point(140, 96);
            this.btnSwap.Name = "btnSwap";
            this.btnSwap.Size = new System.Drawing.Size(94, 29);
            this.btnSwap.TabIndex = 2;
            this.btnSwap.Text = "↕ Swap ↕";
            this.btnSwap.UseVisualStyleBackColor = true;
            this.btnSwap.Click += new System.EventHandler(this.btnSwap_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(87, 368);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(203, 29);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import to Stardew Valley";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // Import
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 414);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnSwap);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Import";
            this.Text = "Import";
            this.Load += new System.EventHandler(this.Import_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private Label lblMainFarmhand;
        private GroupBox groupBox2;
        private ListBox lstFarmHands;
        private Button btnSwap;
        private Button btnImport;
    }
}