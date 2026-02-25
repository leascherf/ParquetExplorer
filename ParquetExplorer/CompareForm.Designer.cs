namespace ParquetExplorer
{
    partial class CompareForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlTop = new System.Windows.Forms.Panel();
            btnOpenLeft = new System.Windows.Forms.Button();
            btnOpenRight = new System.Windows.Forms.Button();
            btnCompare = new System.Windows.Forms.Button();
            lblFilter = new System.Windows.Forms.Label();
            cmbFilter = new System.Windows.Forms.ComboBox();
            chkShowEmptyColumns = new System.Windows.Forms.CheckBox();

            pnlFiles = new System.Windows.Forms.SplitContainer();
            pnlLeftFile = new System.Windows.Forms.Panel();
            lblLeftFile = new System.Windows.Forms.Label();
            dgvLeft = new System.Windows.Forms.DataGridView();
            pnlRightFile = new System.Windows.Forms.Panel();
            lblRightFile = new System.Windows.Forms.Label();
            dgvRight = new System.Windows.Forms.DataGridView();

            pnlLegend = new System.Windows.Forms.Panel();
            lblSummary = new System.Windows.Forms.Label();
            lblLegendDiff = new System.Windows.Forms.Label();
            lblLegendLeft = new System.Windows.Forms.Label();
            lblLegendRight = new System.Windows.Forms.Label();
            lblLegendSame = new System.Windows.Forms.Label();
            pnlBottom = new System.Windows.Forms.Panel();
            btnPrev = new System.Windows.Forms.Button();
            btnNext = new System.Windows.Forms.Button();
            lblPageInfo = new System.Windows.Forms.Label();
            lblPageSizeLabel = new System.Windows.Forms.Label();
            cmbPageSize = new System.Windows.Forms.ComboBox();

            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlFiles).BeginInit();
            pnlFiles.Panel1.SuspendLayout();
            pnlFiles.Panel2.SuspendLayout();
            pnlLeftFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLeft).BeginInit();
            pnlRightFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRight).BeginInit();
            pnlLegend.SuspendLayout();
            SuspendLayout();

            // pnlTop
            pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTop.Height = 44;
            pnlTop.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            pnlTop.BackColor = System.Drawing.Color.FromArgb(248, 249, 252);
            pnlTop.Controls.Add(btnOpenLeft);
            pnlTop.Controls.Add(btnOpenRight);
            pnlTop.Controls.Add(btnCompare);
            pnlTop.Controls.Add(lblFilter);
            pnlTop.Controls.Add(cmbFilter);
            pnlTop.Controls.Add(chkShowEmptyColumns);

            // btnOpenLeft
            btnOpenLeft.Text = "📂 Open Left";
            btnOpenLeft.Location = new System.Drawing.Point(6, 7);
            btnOpenLeft.Size = new System.Drawing.Size(120, 28);
            btnOpenLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOpenLeft.BackColor = System.Drawing.Color.FromArgb(220, 232, 248);
            btnOpenLeft.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(160, 190, 230);
            btnOpenLeft.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnOpenLeft.Click += btnOpenLeft_Click;

            // btnOpenRight
            btnOpenRight.Text = "📂 Open Right";
            btnOpenRight.Location = new System.Drawing.Point(134, 7);
            btnOpenRight.Size = new System.Drawing.Size(120, 28);
            btnOpenRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOpenRight.BackColor = System.Drawing.Color.FromArgb(214, 242, 214);
            btnOpenRight.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(160, 220, 160);
            btnOpenRight.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnOpenRight.Click += btnOpenRight_Click;

            // btnCompare
            btnCompare.Text = "⇄ Compare";
            btnCompare.Location = new System.Drawing.Point(262, 7);
            btnCompare.Size = new System.Drawing.Size(100, 28);
            btnCompare.BackColor = System.Drawing.Color.FromArgb(44, 123, 229);
            btnCompare.ForeColor = System.Drawing.Color.White;
            btnCompare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCompare.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(30, 100, 200);
            btnCompare.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            btnCompare.Click += btnCompare_Click;

            // lblFilter
            lblFilter.AutoSize = true;
            lblFilter.Location = new System.Drawing.Point(378, 13);
            lblFilter.Font = new System.Drawing.Font("Segoe UI", 9f);
            lblFilter.Text = "Filter:";

            // cmbFilter
            cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbFilter.Location = new System.Drawing.Point(415, 10);
            cmbFilter.Size = new System.Drawing.Size(130, 23);
            cmbFilter.Font = new System.Drawing.Font("Segoe UI", 9f);

            // chkShowEmptyColumns
            chkShowEmptyColumns.AutoSize = true;
            chkShowEmptyColumns.Location = new System.Drawing.Point(560, 12);
            chkShowEmptyColumns.Text = "Show Empty Columns";
            chkShowEmptyColumns.Font = new System.Drawing.Font("Segoe UI", 9f);
            chkShowEmptyColumns.CheckedChanged += chkShowEmptyColumns_CheckedChanged;

            // pnlLegend
            pnlLegend.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlLegend.Height = 92;
            pnlLegend.Padding = new System.Windows.Forms.Padding(6);
            pnlLegend.BackColor = System.Drawing.Color.FromArgb(248, 249, 252);
            pnlLegend.Controls.Add(lblSummary);
            pnlLegend.Controls.Add(lblLegendDiff);
            pnlLegend.Controls.Add(lblLegendLeft);
            pnlLegend.Controls.Add(lblLegendRight);
            pnlLegend.Controls.Add(lblLegendSame);

            // lblSummary
            lblSummary.AutoSize = false;
            lblSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            lblSummary.Height = 58;
            lblSummary.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular);
            lblSummary.BackColor = System.Drawing.Color.FromArgb(240, 244, 251);
            lblSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblSummary.Text = "Open two files and click ⇄ Compare to calculate match percentages.";
            lblSummary.Padding = new System.Windows.Forms.Padding(8);
            lblSummary.ForeColor = System.Drawing.Color.FromArgb(60, 80, 110);

            // Legend labels
            lblLegendDiff.AutoSize = true;
            lblLegendDiff.Location = new System.Drawing.Point(6, 5);
            lblLegendDiff.BackColor = System.Drawing.Color.FromArgb(255, 255, 180);
            lblLegendDiff.Text = "  ⚠ Different  ";
            lblLegendDiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblLegendDiff.Font = new System.Drawing.Font("Segoe UI", 9f);

            lblLegendLeft.AutoSize = true;
            lblLegendLeft.Location = new System.Drawing.Point(126, 5);
            lblLegendLeft.BackColor = System.Drawing.Color.FromArgb(255, 182, 182);
            lblLegendLeft.Text = "  ◁ Left only  ";
            lblLegendLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblLegendLeft.Font = new System.Drawing.Font("Segoe UI", 9f);

            lblLegendRight.AutoSize = true;
            lblLegendRight.Location = new System.Drawing.Point(244, 5);
            lblLegendRight.BackColor = System.Drawing.Color.FromArgb(182, 255, 182);
            lblLegendRight.Text = "  ▷ Right only  ";
            lblLegendRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblLegendRight.Font = new System.Drawing.Font("Segoe UI", 9f);

            lblLegendSame.AutoSize = true;
            lblLegendSame.Location = new System.Drawing.Point(370, 5);
            lblLegendSame.BackColor = System.Drawing.Color.White;
            lblLegendSame.Text = "  ✓ Same  ";
            lblLegendSame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblLegendSame.Font = new System.Drawing.Font("Segoe UI", 9f);

            // pnlFiles (SplitContainer)
            pnlFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlFiles.Orientation = System.Windows.Forms.Orientation.Vertical;
            pnlFiles.SplitterDistance = 600;

            // Left panel
            pnlLeftFile.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlLeftFile.Controls.Add(dgvLeft);
            pnlLeftFile.Controls.Add(lblLeftFile);
            pnlFiles.Panel1.Controls.Add(pnlLeftFile);

            // lblLeftFile
            lblLeftFile.Dock = System.Windows.Forms.DockStyle.Top;
            lblLeftFile.Height = 24;
            lblLeftFile.Padding = new System.Windows.Forms.Padding(6, 4, 4, 0);
            lblLeftFile.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            lblLeftFile.Text = "Left file: (none selected)";
            lblLeftFile.BackColor = System.Drawing.Color.FromArgb(210, 228, 255);
            lblLeftFile.ForeColor = System.Drawing.Color.FromArgb(30, 60, 120);

            // dgvLeft
            dgvLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvLeft.AllowUserToAddRows = false;
            dgvLeft.AllowUserToDeleteRows = false;
            dgvLeft.ReadOnly = true;
            dgvLeft.AllowUserToResizeColumns = true;
            dgvLeft.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            dgvLeft.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            dgvLeft.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvLeft.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLeft.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvLeft.EnableHeadersVisualStyles = false;
            dgvLeft.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(50, 80, 130);
            dgvLeft.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvLeft.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            dgvLeft.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(50, 80, 130);
            dgvLeft.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvLeft.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(160, 195, 240);
            dgvLeft.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(20, 40, 80);
            dgvLeft.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9f);
            dgvLeft.GridColor = System.Drawing.Color.FromArgb(220, 225, 235);
            dgvLeft.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLeft.BackgroundColor = System.Drawing.Color.White;
            dgvLeft.CellFormatting += DgvLeft_CellFormatting;
            dgvLeft.CellDoubleClick += Dgv_CellDoubleClick;
            dgvLeft.CellToolTipTextNeeded += Dgv_CellToolTipTextNeeded;
            dgvLeft.Scroll += Dgv_Scroll;
            dgvLeft.SelectionChanged += Dgv_SelectionChanged;

            // Right panel
            pnlRightFile.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlRightFile.Controls.Add(dgvRight);
            pnlRightFile.Controls.Add(lblRightFile);
            pnlFiles.Panel2.Controls.Add(pnlRightFile);

            // lblRightFile
            lblRightFile.Dock = System.Windows.Forms.DockStyle.Top;
            lblRightFile.Height = 24;
            lblRightFile.Padding = new System.Windows.Forms.Padding(6, 4, 4, 0);
            lblRightFile.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            lblRightFile.Text = "Right file: (none selected)";
            lblRightFile.BackColor = System.Drawing.Color.FromArgb(200, 240, 205);
            lblRightFile.ForeColor = System.Drawing.Color.FromArgb(20, 80, 40);

            // dgvRight
            dgvRight.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvRight.AllowUserToAddRows = false;
            dgvRight.AllowUserToDeleteRows = false;
            dgvRight.ReadOnly = true;
            dgvRight.AllowUserToResizeColumns = true;
            dgvRight.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            dgvRight.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            dgvRight.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvRight.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRight.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvRight.EnableHeadersVisualStyles = false;
            dgvRight.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(40, 110, 60);
            dgvRight.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvRight.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            dgvRight.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(40, 110, 60);
            dgvRight.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvRight.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(170, 230, 190);
            dgvRight.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(20, 70, 30);
            dgvRight.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9f);
            dgvRight.GridColor = System.Drawing.Color.FromArgb(220, 225, 235);
            dgvRight.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRight.BackgroundColor = System.Drawing.Color.White;
            dgvRight.CellFormatting += DgvRight_CellFormatting;
            dgvRight.CellDoubleClick += Dgv_CellDoubleClick;
            dgvRight.CellToolTipTextNeeded += Dgv_CellToolTipTextNeeded;
            dgvRight.Scroll += Dgv_Scroll;
            dgvRight.SelectionChanged += Dgv_SelectionChanged;

            // pnlBottom
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Height = 38;
            pnlBottom.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            pnlBottom.BackColor = System.Drawing.Color.FromArgb(248, 249, 252);
            pnlBottom.Controls.Add(lblPageInfo);
            pnlBottom.Controls.Add(btnNext);
            pnlBottom.Controls.Add(btnPrev);
            pnlBottom.Controls.Add(cmbPageSize);
            pnlBottom.Controls.Add(lblPageSizeLabel);

            // btnPrev
            btnPrev.Text = "◀ Prev";
            btnPrev.Size = new System.Drawing.Size(76, 26);
            btnPrev.Location = new System.Drawing.Point(6, 5);
            btnPrev.Enabled = false;
            btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPrev.BackColor = System.Drawing.Color.FromArgb(240, 243, 248);
            btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(190, 200, 215);
            btnPrev.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnPrev.Click += btnPrev_Click;

            // btnNext
            btnNext.Text = "Next ▶";
            btnNext.Size = new System.Drawing.Size(76, 26);
            btnNext.Location = new System.Drawing.Point(86, 5);
            btnNext.Enabled = false;
            btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNext.BackColor = System.Drawing.Color.FromArgb(240, 243, 248);
            btnNext.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(190, 200, 215);
            btnNext.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnNext.Click += btnNext_Click;

            // lblPageInfo
            lblPageInfo.AutoSize = false;
            lblPageInfo.Location = new System.Drawing.Point(170, 9);
            lblPageInfo.Size = new System.Drawing.Size(300, 20);
            lblPageInfo.Font = new System.Drawing.Font("Segoe UI", 9f);
            lblPageInfo.ForeColor = System.Drawing.Color.FromArgb(60, 80, 110);
            lblPageInfo.Text = "Page 1 of 1  (0 rows)";

            // lblPageSizeLabel
            lblPageSizeLabel.AutoSize = true;
            lblPageSizeLabel.Location = new System.Drawing.Point(474, 9);
            lblPageSizeLabel.Font = new System.Drawing.Font("Segoe UI", 9f);
            lblPageSizeLabel.ForeColor = System.Drawing.Color.FromArgb(80, 100, 130);
            lblPageSizeLabel.Text = "Rows per page:";

            // cmbPageSize
            cmbPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPageSize.Location = new System.Drawing.Point(570, 6);
            cmbPageSize.Size = new System.Drawing.Size(70, 24);
            cmbPageSize.Font = new System.Drawing.Font("Segoe UI", 9f);
            cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "500", "1000" });
            cmbPageSize.SelectedItem = "500";
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;

            // CompareForm
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1400, 800);
            MinimumSize = new System.Drawing.Size(900, 600);
            Text = "Parquet File Comparison";
            Font = new System.Drawing.Font("Segoe UI", 9f);
            BackColor = System.Drawing.Color.FromArgb(248, 249, 252);
            Controls.Add(pnlFiles);
            Controls.Add(pnlBottom);
            Controls.Add(pnlLegend);
            Controls.Add(pnlTop);

            pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pnlFiles).EndInit();
            pnlFiles.Panel1.ResumeLayout(false);
            pnlFiles.Panel2.ResumeLayout(false);
            pnlLeftFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLeft).EndInit();
            pnlRightFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRight).EndInit();
            pnlLegend.ResumeLayout(false);
            pnlLegend.PerformLayout();
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTop = null!;
        private System.Windows.Forms.Button btnOpenLeft = null!;
        private System.Windows.Forms.Button btnOpenRight = null!;
        private System.Windows.Forms.Button btnCompare = null!;
        private System.Windows.Forms.Label lblFilter = null!;
        private System.Windows.Forms.ComboBox cmbFilter = null!;
        private System.Windows.Forms.CheckBox chkShowEmptyColumns = null!;
        private System.Windows.Forms.SplitContainer pnlFiles = null!;
        private System.Windows.Forms.Panel pnlLeftFile = null!;
        private System.Windows.Forms.Label lblLeftFile = null!;
        private System.Windows.Forms.DataGridView dgvLeft = null!;
        private System.Windows.Forms.Panel pnlRightFile = null!;
        private System.Windows.Forms.Label lblRightFile = null!;
        private System.Windows.Forms.DataGridView dgvRight = null!;
        private System.Windows.Forms.Panel pnlLegend = null!;
        private System.Windows.Forms.Label lblSummary = null!;
        private System.Windows.Forms.Label lblLegendDiff = null!;
        private System.Windows.Forms.Label lblLegendLeft = null!;
        private System.Windows.Forms.Label lblLegendRight = null!;
        private System.Windows.Forms.Label lblLegendSame = null!;
        private System.Windows.Forms.Panel pnlBottom = null!;
        private System.Windows.Forms.Button btnPrev = null!;
        private System.Windows.Forms.Button btnNext = null!;
        private System.Windows.Forms.Label lblPageInfo = null!;
        private System.Windows.Forms.Label lblPageSizeLabel = null!;
        private System.Windows.Forms.ComboBox cmbPageSize = null!;
    }
}
