namespace SampleForAcr1252
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblIdm = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.rb20inch = new System.Windows.Forms.RadioButton();
            this.rb27inch = new System.Windows.Forms.RadioButton();
            this.btnShowImage = new System.Windows.Forms.Button();
            this.btnClearDisplay = new System.Windows.Forms.Button();
            this.btnWriteData = new System.Windows.Forms.Button();
            this.btnReadData = new System.Windows.Forms.Button();
            this.btnSaveLayout = new System.Windows.Forms.Button();
            this.btnLoadLayout = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.btnShowPartial = new System.Windows.Forms.Button();
            this.rb29inch = new System.Windows.Forms.RadioButton();
            this.btnWriteData2 = new System.Windows.Forms.Button();
            this.btnReadData2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblIdm
            // 
            this.lblIdm.AutoSize = true;
            this.lblIdm.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIdm.Location = new System.Drawing.Point(50, 170);
            this.lblIdm.Name = "lblIdm";
            this.lblIdm.Size = new System.Drawing.Size(50, 18);
            this.lblIdm.TabIndex = 10;
            this.lblIdm.Text = "label1";
            // 
            // lblMessage
            // 
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(12, 139);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Padding = new System.Windows.Forms.Padding(2);
            this.lblMessage.Size = new System.Drawing.Size(587, 26);
            this.lblMessage.TabIndex = 9;
            this.lblMessage.Text = "label1";
            // 
            // lstLog
            // 
            this.lstLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstLog.FormattingEnabled = true;
            this.lstLog.HorizontalScrollbar = true;
            this.lstLog.ItemHeight = 12;
            this.lstLog.Location = new System.Drawing.Point(12, 190);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(587, 148);
            this.lstLog.TabIndex = 11;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "IDm : ";
            // 
            // rb20inch
            // 
            this.rb20inch.AutoSize = true;
            this.rb20inch.Location = new System.Drawing.Point(118, 23);
            this.rb20inch.Name = "rb20inch";
            this.rb20inch.Size = new System.Drawing.Size(50, 16);
            this.rb20inch.TabIndex = 15;
            this.rb20inch.Text = "2inch";
            this.rb20inch.UseVisualStyleBackColor = true;
            // 
            // rb27inch
            // 
            this.rb27inch.AutoSize = true;
            this.rb27inch.Location = new System.Drawing.Point(174, 23);
            this.rb27inch.Name = "rb27inch";
            this.rb27inch.Size = new System.Drawing.Size(58, 16);
            this.rb27inch.TabIndex = 15;
            this.rb27inch.Text = "2.7inch";
            this.rb27inch.UseVisualStyleBackColor = true;
            // 
            // btnShowImage
            // 
            this.btnShowImage.Location = new System.Drawing.Point(12, 57);
            this.btnShowImage.Name = "btnShowImage";
            this.btnShowImage.Size = new System.Drawing.Size(96, 32);
            this.btnShowImage.TabIndex = 18;
            this.btnShowImage.Text = "Show image";
            this.btnShowImage.UseVisualStyleBackColor = true;
            this.btnShowImage.Click += new System.EventHandler(this.btnShowImage_Click);
            // 
            // btnClearDisplay
            // 
            this.btnClearDisplay.Location = new System.Drawing.Point(118, 57);
            this.btnClearDisplay.Name = "btnClearDisplay";
            this.btnClearDisplay.Size = new System.Drawing.Size(96, 32);
            this.btnClearDisplay.TabIndex = 18;
            this.btnClearDisplay.Text = "Clear display";
            this.btnClearDisplay.UseVisualStyleBackColor = true;
            this.btnClearDisplay.Click += new System.EventHandler(this.btnClearDisplay_Click);
            // 
            // btnWriteData
            // 
            this.btnWriteData.Location = new System.Drawing.Point(224, 57);
            this.btnWriteData.Name = "btnWriteData";
            this.btnWriteData.Size = new System.Drawing.Size(126, 32);
            this.btnWriteData.TabIndex = 18;
            this.btnWriteData.Text = "Write to user area";
            this.btnWriteData.UseVisualStyleBackColor = true;
            this.btnWriteData.Click += new System.EventHandler(this.btnWriteData_Click);
            // 
            // btnReadData
            // 
            this.btnReadData.Location = new System.Drawing.Point(224, 95);
            this.btnReadData.Name = "btnReadData";
            this.btnReadData.Size = new System.Drawing.Size(126, 32);
            this.btnReadData.TabIndex = 18;
            this.btnReadData.Text = "Read from user area";
            this.btnReadData.UseVisualStyleBackColor = true;
            this.btnReadData.Click += new System.EventHandler(this.btnReadData_Click);
            // 
            // btnSaveLayout
            // 
            this.btnSaveLayout.Location = new System.Drawing.Point(503, 57);
            this.btnSaveLayout.Name = "btnSaveLayout";
            this.btnSaveLayout.Size = new System.Drawing.Size(96, 32);
            this.btnSaveLayout.TabIndex = 18;
            this.btnSaveLayout.Text = "Save layout";
            this.btnSaveLayout.UseVisualStyleBackColor = true;
            this.btnSaveLayout.Click += new System.EventHandler(this.btnSaveLayout_Click);
            // 
            // btnLoadLayout
            // 
            this.btnLoadLayout.Location = new System.Drawing.Point(503, 95);
            this.btnLoadLayout.Name = "btnLoadLayout";
            this.btnLoadLayout.Size = new System.Drawing.Size(96, 32);
            this.btnLoadLayout.TabIndex = 18;
            this.btnLoadLayout.Text = "Load layout";
            this.btnLoadLayout.UseVisualStyleBackColor = true;
            this.btnLoadLayout.Click += new System.EventHandler(this.btnLoadLayout_Click);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.SteelBlue;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(14, 23);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(2);
            this.label3.Size = new System.Drawing.Size(75, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Display type:";
            // 
            // btnShowPartial
            // 
            this.btnShowPartial.Location = new System.Drawing.Point(12, 95);
            this.btnShowPartial.Name = "btnShowPartial";
            this.btnShowPartial.Size = new System.Drawing.Size(96, 32);
            this.btnShowPartial.TabIndex = 18;
            this.btnShowPartial.Text = "Show partial";
            this.btnShowPartial.UseVisualStyleBackColor = true;
            this.btnShowPartial.Click += new System.EventHandler(this.btnShowPartial_Click);
            // 
            // rb29inch
            // 
            this.rb29inch.AutoSize = true;
            this.rb29inch.Checked = true;
            this.rb29inch.Location = new System.Drawing.Point(238, 23);
            this.rb29inch.Name = "rb29inch";
            this.rb29inch.Size = new System.Drawing.Size(58, 16);
            this.rb29inch.TabIndex = 15;
            this.rb29inch.TabStop = true;
            this.rb29inch.Text = "2.9inch";
            this.rb29inch.UseVisualStyleBackColor = true;
            this.rb29inch.CheckedChanged += new System.EventHandler(this.rb29inch_CheckedChanged);
            // 
            // btnWriteData2
            // 
            this.btnWriteData2.Location = new System.Drawing.Point(362, 57);
            this.btnWriteData2.Name = "btnWriteData2";
            this.btnWriteData2.Size = new System.Drawing.Size(126, 32);
            this.btnWriteData2.TabIndex = 18;
            this.btnWriteData2.Text = "Write to card area";
            this.btnWriteData2.UseVisualStyleBackColor = true;
            this.btnWriteData2.Click += new System.EventHandler(this.btnWriteData2_Click);
            // 
            // btnReadData2
            // 
            this.btnReadData2.Location = new System.Drawing.Point(362, 95);
            this.btnReadData2.Name = "btnReadData2";
            this.btnReadData2.Size = new System.Drawing.Size(126, 32);
            this.btnReadData2.TabIndex = 18;
            this.btnReadData2.Text = "Read from card area";
            this.btnReadData2.UseVisualStyleBackColor = true;
            this.btnReadData2.Click += new System.EventHandler(this.btnReadData2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 352);
            this.Controls.Add(this.btnShowPartial);
            this.Controls.Add(this.btnLoadLayout);
            this.Controls.Add(this.btnSaveLayout);
            this.Controls.Add(this.btnReadData2);
            this.Controls.Add(this.btnReadData);
            this.Controls.Add(this.btnWriteData2);
            this.Controls.Add(this.btnWriteData);
            this.Controls.Add(this.btnClearDisplay);
            this.Controls.Add(this.btnShowImage);
            this.Controls.Add(this.rb29inch);
            this.Controls.Add(this.rb27inch);
            this.Controls.Add(this.rb20inch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.lblIdm);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label3);
            this.Name = "Form1";
            this.Text = "Sample Program for ACR1252";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion  

        private System.Windows.Forms.Label lblIdm;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rb20inch;
        private System.Windows.Forms.RadioButton rb27inch;
        private System.Windows.Forms.Button btnShowImage;
        private System.Windows.Forms.Button btnClearDisplay;
        private System.Windows.Forms.Button btnWriteData;
        private System.Windows.Forms.Button btnReadData;
        private System.Windows.Forms.Button btnSaveLayout;
        private System.Windows.Forms.Button btnLoadLayout;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnShowPartial;
        private System.Windows.Forms.RadioButton rb29inch;
        private System.Windows.Forms.Button btnWriteData2;
        private System.Windows.Forms.Button btnReadData2;
    }
}

