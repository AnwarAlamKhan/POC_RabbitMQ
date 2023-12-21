namespace Reply_Server
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnSubscribeEmailQueue = new System.Windows.Forms.Button();
            this.lstEmail = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(212, 52);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(821, 55);
            this.btnConnect.TabIndex = 30;
            this.btnConnect.Text = "Connect RabbitMQ";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnSubscribeEmailQueue
            // 
            this.btnSubscribeEmailQueue.Location = new System.Drawing.Point(212, 167);
            this.btnSubscribeEmailQueue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSubscribeEmailQueue.Name = "btnSubscribeEmailQueue";
            this.btnSubscribeEmailQueue.Size = new System.Drawing.Size(821, 55);
            this.btnSubscribeEmailQueue.TabIndex = 29;
            this.btnSubscribeEmailQueue.Text = "Subscribe Email";
            this.btnSubscribeEmailQueue.UseVisualStyleBackColor = true;
            this.btnSubscribeEmailQueue.Click += new System.EventHandler(this.btnSubscribeEmailQueue_Click);
            // 
            // lstEmail
            // 
            this.lstEmail.FormattingEnabled = true;
            this.lstEmail.ItemHeight = 20;
            this.lstEmail.Location = new System.Drawing.Point(212, 253);
            this.lstEmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstEmail.Name = "lstEmail";
            this.lstEmail.Size = new System.Drawing.Size(820, 164);
            this.lstEmail.TabIndex = 28;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1311, 702);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnSubscribeEmailQueue);
            this.Controls.Add(this.lstEmail);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnConnect;
        private Button btnSubscribeEmailQueue;
        private ListBox lstEmail;
    }
}