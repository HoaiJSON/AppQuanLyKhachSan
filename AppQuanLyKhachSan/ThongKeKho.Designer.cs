namespace AppQuanLyKhachSan
{
    partial class btnThongKeKho
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
            this.label1 = new System.Windows.Forms.Label();
            this.rbtnGanHet = new System.Windows.Forms.RadioButton();
            this.rbtnDaHet = new System.Windows.Forms.RadioButton();
            this.rbtnAll = new System.Windows.Forms.RadioButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnThongKe = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century", 16.2F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Thống kê tồn kho";
            // 
            // rbtnGanHet
            // 
            this.rbtnGanHet.AutoSize = true;
            this.rbtnGanHet.Location = new System.Drawing.Point(167, 82);
            this.rbtnGanHet.Name = "rbtnGanHet";
            this.rbtnGanHet.Size = new System.Drawing.Size(77, 20);
            this.rbtnGanHet.TabIndex = 1;
            this.rbtnGanHet.TabStop = true;
            this.rbtnGanHet.Text = "Gần hết ";
            this.rbtnGanHet.UseVisualStyleBackColor = true;
            // 
            // rbtnDaHet
            // 
            this.rbtnDaHet.AutoSize = true;
            this.rbtnDaHet.Location = new System.Drawing.Point(274, 82);
            this.rbtnDaHet.Name = "rbtnDaHet";
            this.rbtnDaHet.Size = new System.Drawing.Size(66, 20);
            this.rbtnDaHet.TabIndex = 2;
            this.rbtnDaHet.TabStop = true;
            this.rbtnDaHet.Text = "Đã hết";
            this.rbtnDaHet.UseVisualStyleBackColor = true;
            // 
            // rbtnAll
            // 
            this.rbtnAll.AutoSize = true;
            this.rbtnAll.Location = new System.Drawing.Point(18, 82);
            this.rbtnAll.Name = "rbtnAll";
            this.rbtnAll.Size = new System.Drawing.Size(124, 20);
            this.rbtnAll.TabIndex = 3;
            this.rbtnAll.TabStop = true;
            this.rbtnAll.Text = "Tất cả mặt hàng";
            this.rbtnAll.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(18, 123);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1048, 422);
            this.dataGridView1.TabIndex = 4;
            // 
            // btnThongKe
            // 
            this.btnThongKe.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnThongKe.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnThongKe.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnThongKe.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnThongKe.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnThongKe.ForeColor = System.Drawing.Color.White;
            this.btnThongKe.Location = new System.Drawing.Point(346, 57);
            this.btnThongKe.Name = "btnThongKe";
            this.btnThongKe.Size = new System.Drawing.Size(180, 45);
            this.btnThongKe.TabIndex = 5;
            this.btnThongKe.Text = "Thống kê";
            this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);
            // 
            // btnThongKeKho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 557);
            this.Controls.Add(this.btnThongKe);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.rbtnAll);
            this.Controls.Add(this.rbtnDaHet);
            this.Controls.Add(this.rbtnGanHet);
            this.Controls.Add(this.label1);
            this.Name = "btnThongKeKho";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ThongKeKho";
            this.Load += new System.EventHandler(this.ThongKeKho_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbtnGanHet;
        private System.Windows.Forms.RadioButton rbtnDaHet;
        private System.Windows.Forms.RadioButton rbtnAll;
        private System.Windows.Forms.DataGridView dataGridView1;
        private Guna.UI2.WinForms.Guna2Button btnThongKe;
    }
}