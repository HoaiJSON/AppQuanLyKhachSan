namespace AppQuanLyKhachSan.CTQuanLyKhachSan
{
    partial class UC_Statistics
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_Statistics));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.ChartBDC = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxThongke = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dtpKetThuc = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.dtpBatDau = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnthongke = new Guna.UI2.WinForms.Guna2Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.checkAll = new System.Windows.Forms.CheckBox();
            this.checkDV = new System.Windows.Forms.CheckBox();
            this.checkPhong = new System.Windows.Forms.CheckBox();
            this.chartBDT = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.ChartBDC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartBDT)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 30;
            this.guna2Elipse1.TargetControl = this;
            // 
            // ChartBDC
            // 
            chartArea2.Name = "ChartArea1";
            this.ChartBDC.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.ChartBDC.Legends.Add(legend2);
            this.ChartBDC.Location = new System.Drawing.Point(1392, 151);
            this.ChartBDC.Name = "ChartBDC";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.LegendText = "Biểu đồ cột";
            series2.Name = "ChartBDC";
            this.ChartBDC.Series.Add(series2);
            this.ChartBDC.Size = new System.Drawing.Size(472, 296);
            this.ChartBDC.TabIndex = 0;
            this.ChartBDC.Text = "chart1";
            title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            title2.ForeColor = System.Drawing.Color.Red;
            title2.Name = "Title1";
            title2.Text = "Biểu đồ cột";
            this.ChartBDC.Titles.Add(title2);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century", 19.8F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(32, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(354, 40);
            this.label1.TabIndex = 3;
            this.label1.Text = "Thống kê doanh thu";
            // 
            // cbxThongke
            // 
            this.cbxThongke.BackColor = System.Drawing.Color.Transparent;
            this.cbxThongke.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxThongke.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxThongke.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbxThongke.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbxThongke.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbxThongke.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbxThongke.ItemHeight = 30;
            this.cbxThongke.Location = new System.Drawing.Point(50, 151);
            this.cbxThongke.Name = "cbxThongke";
            this.cbxThongke.Size = new System.Drawing.Size(492, 36);
            this.cbxThongke.TabIndex = 4;
            // 
            // dtpKetThuc
            // 
            this.dtpKetThuc.Checked = true;
            this.dtpKetThuc.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.dtpKetThuc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpKetThuc.Location = new System.Drawing.Point(517, 96);
            this.dtpKetThuc.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpKetThuc.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpKetThuc.Name = "dtpKetThuc";
            this.dtpKetThuc.Size = new System.Drawing.Size(256, 36);
            this.dtpKetThuc.TabIndex = 5;
            this.dtpKetThuc.Value = new System.DateTime(2024, 10, 30, 21, 34, 20, 314);
            // 
            // dtpBatDau
            // 
            this.dtpBatDau.Checked = true;
            this.dtpBatDau.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.dtpBatDau.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpBatDau.Location = new System.Drawing.Point(135, 96);
            this.dtpBatDau.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpBatDau.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpBatDau.Name = "dtpBatDau";
            this.dtpBatDau.Size = new System.Drawing.Size(231, 36);
            this.dtpBatDau.TabIndex = 9;
            this.dtpBatDau.Value = new System.DateTime(2024, 10, 30, 21, 34, 20, 314);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(45, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 25);
            this.label2.TabIndex = 10;
            this.label2.Text = "Từ ngày";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.Location = new System.Drawing.Point(415, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 25);
            this.label3.TabIndex = 11;
            this.label3.Text = "Đến ngày";
            // 
            // btnthongke
            // 
            this.btnthongke.BorderRadius = 10;
            this.btnthongke.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnthongke.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnthongke.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnthongke.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnthongke.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnthongke.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnthongke.Image = ((System.Drawing.Image)(resources.GetObject("btnthongke.Image")));
            this.btnthongke.Location = new System.Drawing.Point(593, 142);
            this.btnthongke.Name = "btnthongke";
            this.btnthongke.Size = new System.Drawing.Size(180, 45);
            this.btnthongke.TabIndex = 12;
            this.btnthongke.Text = "Thống kê";
            this.btnthongke.Click += new System.EventHandler(this.btnthongke_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(23, 213);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1353, 569);
            this.dataGridView1.TabIndex = 13;
            // 
            // checkAll
            // 
            this.checkAll.AutoSize = true;
            this.checkAll.Checked = true;
            this.checkAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAll.Location = new System.Drawing.Point(819, 100);
            this.checkAll.Name = "checkAll";
            this.checkAll.Size = new System.Drawing.Size(67, 20);
            this.checkAll.TabIndex = 14;
            this.checkAll.Text = "Tất cả";
            this.checkAll.UseVisualStyleBackColor = true;
            this.checkAll.CheckedChanged += new System.EventHandler(this.checkAll_CheckedChanged);
            // 
            // checkDV
            // 
            this.checkDV.AutoSize = true;
            this.checkDV.Checked = true;
            this.checkDV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkDV.Location = new System.Drawing.Point(841, 131);
            this.checkDV.Name = "checkDV";
            this.checkDV.Size = new System.Drawing.Size(73, 20);
            this.checkDV.TabIndex = 15;
            this.checkDV.Text = "Dịch vụ";
            this.checkDV.UseVisualStyleBackColor = true;
            this.checkDV.CheckedChanged += new System.EventHandler(this.checkedcon_CheckedChanged);
            // 
            // checkPhong
            // 
            this.checkPhong.AutoSize = true;
            this.checkPhong.Checked = true;
            this.checkPhong.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkPhong.Location = new System.Drawing.Point(841, 161);
            this.checkPhong.Name = "checkPhong";
            this.checkPhong.Size = new System.Drawing.Size(68, 20);
            this.checkPhong.TabIndex = 16;
            this.checkPhong.Text = "Phòng";
            this.checkPhong.UseVisualStyleBackColor = true;
            this.checkPhong.CheckedChanged += new System.EventHandler(this.checkedcon_CheckedChanged);
            // 
            // chartBDT
            // 
            chartArea1.Name = "ChartArea1";
            this.chartBDT.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartBDT.Legends.Add(legend1);
            this.chartBDT.Location = new System.Drawing.Point(1392, 464);
            this.chartBDT.Name = "chartBDT";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Legend = "Legend1";
            series1.Name = "ChartBDT";
            this.chartBDT.Series.Add(series1);
            this.chartBDT.Size = new System.Drawing.Size(472, 318);
            this.chartBDT.TabIndex = 1;
            this.chartBDT.Text = "chart2";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            title1.ForeColor = System.Drawing.Color.Red;
            title1.Name = "Title1";
            title1.Text = "Biểu đồ tròn";
            this.chartBDT.Titles.Add(title1);
            // 
            // UC_Statistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkPhong);
            this.Controls.Add(this.checkDV);
            this.Controls.Add(this.checkAll);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnthongke);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpBatDau);
            this.Controls.Add(this.dtpKetThuc);
            this.Controls.Add(this.cbxThongke);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chartBDT);
            this.Controls.Add(this.ChartBDC);
            this.Name = "UC_Statistics";
            this.Size = new System.Drawing.Size(1882, 852);
            this.Load += new System.EventHandler(this.UC_Statistics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ChartBDC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartBDT)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartBDC;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpBatDau;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpKetThuc;
        private System.Windows.Forms.DataGridView dataGridView1;
        private Guna.UI2.WinForms.Guna2Button btnthongke;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkPhong;
        private System.Windows.Forms.CheckBox checkDV;
        private System.Windows.Forms.CheckBox checkAll;
        private Guna.UI2.WinForms.Guna2ComboBox cbxThongke;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartBDT;
    }
}
