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
            pnlTop.Height = 40;
            pnlTop.Padding = new System.Windows.Forms.Padding(4);
            pnlTop.Controls.Add(btnOpenLeft);
            pnlTop.Controls.Add(btnOpenRight);
            pnlTop.Controls.Add(btnCompare);
            pnlTop.Controls.Add(lblFilter);
            pnlTop.Controls.Add(cmbFilter);
            pnlTop.Controls.Add(chkShowEmptyColumns);

            // btnOpenLeft
            btnOpenLeft.Text = "Open Left File";
            btnOpenLeft.Location = new System.Drawing.Point(4, 6);
            btnOpenLeft.Size = new System.Drawing.Size(120, 28);
            btnOpenLeft.Click += btnOpenLeft_Click;

            // btnOpenRight
            btnOpenRight.Text = "Open Right File";
            btnOpenRight.Location = new System.Drawing.Point(132, 6);
            btnOpenRight.Size = new System.Drawing.Size(120, 28);
            btnOpenRight.Click += btnOpenRight_Click;

            // btnCompare
            btnCompare.Text = "Compare";
            btnCompare.Location = new System.Drawing.Point(264, 6);
            btnCompare.Size = new System.Drawing.Size(90, 28);
            btnCompare.BackColor = System.Drawing.Color.SteelBlue;
            btnCompare.ForeColor = System.Drawing.Color.White;
            btnCompare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCompare.Click += btnCompare_Click;

            // lblFilter
            lblFilter.AutoSize = true;
            lblFilter.Location = new System.Drawing.Point(370, 13);
            lblFilter.Text = "Filter:";

            // cmbFilter
            cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbFilter.Location = new System.Drawing.Point(410, 10);
            cmbFilter.Size = new System.Drawing.Size(120, 23);

            // chkShowEmptyColumns
            chkShowEmptyColumns.AutoSize = true;
            chkShowEmptyColumns.Location = new System.Drawing.Point(550, 12);
            chkShowEmptyColumns.Text = "Show Empty Columns";
            chkShowEmptyColumns.CheckedChanged += chkShowEmptyColumns_CheckedChanged;

            // pnlLegend
            pnlLegend.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlLegend.Height = 85;
            pnlLegend.Padding = new System.Windows.Forms.Padding(4);
            pnlLegend.Controls.Add(lblSummary);
            pnlLegend.Controls.Add(lblLegendDiff);
            pnlLegend.Controls.Add(lblLegendLeft);
            pnlLegend.Controls.Add(lblLegendRight);
            pnlLegend.Controls.Add(lblLegendSame);

            // lblSummary
            lblSummary.AutoSize = false;
            lblSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            lblSummary.Height = 55;
            lblSummary.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Regular);
            lblSummary.BackColor = System.Drawing.Color.WhiteSmoke;
            lblSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblSummary.Text = "Open two files and click Compare to calculate match percentages.";
            lblSummary.Padding = new System.Windows.Forms.Padding(6);

            // Legend labels
            lblLegendDiff.AutoSize = true;
            lblLegendDiff.Location = new System.Drawing.Point(4, 4);
            lblLegendDiff.BackColor = System.Drawing.Color.FromArgb(255, 255, 180);
            lblLegendDiff.Text = "  Different  ";
            lblLegendDiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            lblLegendLeft.AutoSize = true;
            lblLegendLeft.Location = new System.Drawing.Point(110, 4);
            lblLegendLeft.BackColor = System.Drawing.Color.FromArgb(255, 182, 182);
            lblLegendLeft.Text = "  Left only  ";
            lblLegendLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            lblLegendRight.AutoSize = true;
            lblLegendRight.Location = new System.Drawing.Point(216, 4);
            lblLegendRight.BackColor = System.Drawing.Color.FromArgb(182, 255, 182);
            lblLegendRight.Text = "  Right only  ";
            lblLegendRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            lblLegendSame.AutoSize = true;
            lblLegendSame.Location = new System.Drawing.Point(326, 4);
            lblLegendSame.BackColor = System.Drawing.Color.White;
            lblLegendSame.Text = "  Same  ";
            lblLegendSame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

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
            lblLeftFile.Height = 22;
            lblLeftFile.Padding = new System.Windows.Forms.Padding(4, 3, 4, 0);
            lblLeftFile.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            lblLeftFile.Text = "(no left file)";
            lblLeftFile.BackColor = System.Drawing.Color.FromArgb(220, 235, 255);

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
            lblRightFile.Height = 22;
            lblRightFile.Padding = new System.Windows.Forms.Padding(4, 3, 4, 0);
            lblRightFile.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            lblRightFile.Text = "(no right file)";
            lblRightFile.BackColor = System.Drawing.Color.FromArgb(220, 255, 220);

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
            dgvRight.CellDoubleClick += Dgv_CellDoubleClick;
            dgvRight.CellToolTipTextNeeded += Dgv_CellToolTipTextNeeded;
            dgvRight.Scroll += Dgv_Scroll;
            dgvRight.SelectionChanged += Dgv_SelectionChanged;

            // pnlBottom
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Height = 36;
            pnlBottom.Padding = new System.Windows.Forms.Padding(4);
            pnlBottom.Controls.Add(lblPageInfo);
            pnlBottom.Controls.Add(btnNext);
            pnlBottom.Controls.Add(btnPrev);
            pnlBottom.Controls.Add(cmbPageSize);
            pnlBottom.Controls.Add(lblPageSizeLabel);

            // btnPrev
            btnPrev.Text = "< Prev";
            btnPrev.Size = new System.Drawing.Size(70, 26);
            btnPrev.Location = new System.Drawing.Point(4, 4);
            btnPrev.Enabled = false;
            btnPrev.Click += btnPrev_Click;

            // btnNext
            btnNext.Text = "Next >";
            btnNext.Size = new System.Drawing.Size(70, 26);
            btnNext.Location = new System.Drawing.Point(78, 4);
            btnNext.Enabled = false;
            btnNext.Click += btnNext_Click;

            // lblPageInfo
            lblPageInfo.AutoSize = false;
            lblPageInfo.Location = new System.Drawing.Point(156, 8);
            lblPageInfo.Size = new System.Drawing.Size(280, 20);
            lblPageInfo.Text = "Page 1 of 1  (0 rows)";

            // lblPageSizeLabel
            lblPageSizeLabel.AutoSize = true;
            lblPageSizeLabel.Location = new System.Drawing.Point(450, 8);
            lblPageSizeLabel.Text = "Page size:";

            // cmbPageSize
            cmbPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPageSize.Location = new System.Drawing.Point(520, 5);
            cmbPageSize.Size = new System.Drawing.Size(70, 24);
            cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "500", "1000" });
            cmbPageSize.SelectedItem = "500";
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;

            // CompareForm
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1400, 800);
            Text = "Parquet File Comparison";
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
