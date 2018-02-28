namespace HJcens.DemoWCF.Client
{
    partial class FormClient
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.rtxtResult = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtxtResult
            // 
            this.rtxtResult.Location = new System.Drawing.Point(12, 3);
            this.rtxtResult.Name = "rtxtResult";
            this.rtxtResult.Size = new System.Drawing.Size(574, 297);
            this.rtxtResult.TabIndex = 0;
            this.rtxtResult.Text = "";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 318);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(574, 35);
            this.textBox1.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(13, 373);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(113, 41);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 443);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.rtxtResult);
            this.Name = "FormClient";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxtResult;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSend;
    }
}

