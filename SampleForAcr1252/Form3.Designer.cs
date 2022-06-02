namespace SampleForAcr1252
{
    partial class Form3
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
            this.components = new System.ComponentModel.Container();
            this.btn_ClearCard = new System.Windows.Forms.Button();
            this.btn_LoadData = new System.Windows.Forms.Button();
            this.btn_Generate = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_log = new System.Windows.Forms.Label();
            this.lbl_LogData = new System.Windows.Forms.Label();
            this.qrCodeImage = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.time_pollingTimer = new System.Windows.Forms.Timer(this.components);
            this.time_pollingTimer2 = new System.Windows.Forms.Timer(this.components);
            this.txt_DataInput = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.qrCodeImage)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_ClearCard
            // 
            this.btn_ClearCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_ClearCard.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ClearCard.Location = new System.Drawing.Point(12, 14);
            this.btn_ClearCard.Name = "btn_ClearCard";
            this.btn_ClearCard.Size = new System.Drawing.Size(153, 98);
            this.btn_ClearCard.TabIndex = 0;
            this.btn_ClearCard.Text = "Clear Card";
            this.btn_ClearCard.UseVisualStyleBackColor = false;
            this.btn_ClearCard.Click += new System.EventHandler(this.btn_ClearCard_Click);
            // 
            // btn_LoadData
            // 
            this.btn_LoadData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_LoadData.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LoadData.Location = new System.Drawing.Point(12, 118);
            this.btn_LoadData.Name = "btn_LoadData";
            this.btn_LoadData.Size = new System.Drawing.Size(153, 98);
            this.btn_LoadData.TabIndex = 1;
            this.btn_LoadData.Text = "Load Data";
            this.btn_LoadData.UseVisualStyleBackColor = false;
            this.btn_LoadData.Click += new System.EventHandler(this.btn_LoadData_Click);
            // 
            // btn_Generate
            // 
            this.btn_Generate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_Generate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Generate.Location = new System.Drawing.Point(12, 222);
            this.btn_Generate.Name = "btn_Generate";
            this.btn_Generate.Size = new System.Drawing.Size(153, 98);
            this.btn_Generate.TabIndex = 2;
            this.btn_Generate.Text = "Generate";
            this.btn_Generate.UseVisualStyleBackColor = false;
            this.btn_Generate.Click += new System.EventHandler(this.btn_Generate_Click);
            // 
            // btn_Exit
            // 
            this.btn_Exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btn_Exit.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Exit.Location = new System.Drawing.Point(12, 326);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(153, 98);
            this.btn_Exit.TabIndex = 3;
            this.btn_Exit.Text = "Exit";
            this.btn_Exit.UseVisualStyleBackColor = false;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(192, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "INPUT";
            // 
            // lbl_log
            // 
            this.lbl_log.AutoSize = true;
            this.lbl_log.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbl_log.Location = new System.Drawing.Point(192, 411);
            this.lbl_log.Name = "lbl_log";
            this.lbl_log.Size = new System.Drawing.Size(43, 13);
            this.lbl_log.TabIndex = 7;
            this.lbl_log.Text = "Status: ";
            // 
            // lbl_LogData
            // 
            this.lbl_LogData.AutoSize = true;
            this.lbl_LogData.Location = new System.Drawing.Point(241, 411);
            this.lbl_LogData.Name = "lbl_LogData";
            this.lbl_LogData.Size = new System.Drawing.Size(35, 13);
            this.lbl_LogData.TabIndex = 8;
            this.lbl_LogData.Text = "label3";
            // 
            // qrCodeImage
            // 
            this.qrCodeImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.qrCodeImage.Location = new System.Drawing.Point(127, 3);
            this.qrCodeImage.Name = "qrCodeImage";
            this.qrCodeImage.Size = new System.Drawing.Size(331, 298);
            this.qrCodeImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.qrCodeImage.TabIndex = 0;
            this.qrCodeImage.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Controls.Add(this.qrCodeImage);
            this.panel1.Location = new System.Drawing.Point(195, 86);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(580, 301);
            this.panel1.TabIndex = 6;
            // 
            // time_pollingTimer
            // 
            this.time_pollingTimer.Tick += new System.EventHandler(this.time_pollingTimer_Tick);
            // 
            // time_pollingTimer2
            // 
            this.time_pollingTimer2.Tick += new System.EventHandler(this.time_pollingTimer2_Tick);
            // 
            // txt_DataInput
            // 
            this.txt_DataInput.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_DataInput.Location = new System.Drawing.Point(195, 36);
            this.txt_DataInput.Name = "txt_DataInput";
            this.txt_DataInput.Size = new System.Drawing.Size(580, 44);
            this.txt_DataInput.TabIndex = 9;
            this.txt_DataInput.TextChanged += new System.EventHandler(this.txt_DataInput_TextChanged);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.txt_DataInput);
            this.Controls.Add(this.lbl_LogData);
            this.Controls.Add(this.lbl_log);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.btn_Generate);
            this.Controls.Add(this.btn_LoadData);
            this.Controls.Add(this.btn_ClearCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DENSO SMART CARD";
            this.Load += new System.EventHandler(this.Form3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.qrCodeImage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_ClearCard;
        private System.Windows.Forms.Button btn_LoadData;
        private System.Windows.Forms.Button btn_Generate;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_log;
        private System.Windows.Forms.Label lbl_LogData;
        private System.Windows.Forms.PictureBox qrCodeImage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer time_pollingTimer;
        private System.Windows.Forms.Timer time_pollingTimer2;
        private System.Windows.Forms.TextBox txt_DataInput;
    }
}