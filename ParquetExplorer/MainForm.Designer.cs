namespace ParquetExplorer
{
    partial class MainForm
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
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            compareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            toolStrip1 = new System.Windows.Forms.ToolStrip();
            btnOpen = new System.Windows.Forms.ToolStripButton();
            btnCompare = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            lblFilterLabel = new System.Windows.Forms.ToolStripLabel();
            txtFilter = new System.Windows.Forms.ToolStripTextBox();
            lblColumnLabel = new System.Windows.Forms.ToolStripLabel();
            cmbFilterColumn = new System.Windows.Forms.ToolStripComboBox();
            btnApplyFilter = new System.Windows.Forms.ToolStripButton();

            dataGridView1 = new System.Windows.Forms.DataGridView();
            pnlBottom = new System.Windows.Forms.Panel();
            btnPrev = new System.Windows.Forms.Button();
            btnNext = new System.Windows.Forms.Button();
            lblPageInfo = new System.Windows.Forms.Label();
            lblPageSizeLabel = new System.Windows.Forms.Label();
            cmbPageSize = new System.Windows.Forms.ComboBox();

            lblFilePath = new System.Windows.Forms.Label();
            lblStatus = new System.Windows.Forms.Label();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();

            menuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            pnlBottom.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();

            // menuStrip1
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(1200, 24);
            menuStrip1.TabIndex = 0;

            // fileToolStripMenuItem
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                openToolStripMenuItem, compareToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem
            });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Text = "&File";

            // openToolStripMenuItem
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            openToolStripMenuItem.Text = "&Open Parquet...";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;

            // compareToolStripMenuItem
            compareToolStripMenuItem.Name = "compareToolStripMenuItem";
            compareToolStripMenuItem.Text = "&Compare Files...";
            compareToolStripMenuItem.Click += compareToolStripMenuItem_Click;

            // toolStripSeparator1
            toolStripSeparator1.Name = "toolStripSeparator1";

            // exitToolStripMenuItem
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;

            // toolStrip1
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                btnOpen, btnCompare, toolStripSeparator2,
                lblFilterLabel, txtFilter, lblColumnLabel, cmbFilterColumn, btnApplyFilter
            });
            toolStrip1.Location = new System.Drawing.Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(1200, 25);
            toolStrip1.TabIndex = 1;

            // btnOpen
            btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnOpen.Name = "btnOpen";
            btnOpen.Text = "Open";
            btnOpen.ToolTipText = "Open Parquet File";
            btnOpen.Click += btnOpen_Click;

            // btnCompare
            btnCompare.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnCompare.Name = "btnCompare";
            btnCompare.Text = "Compare";
            btnCompare.ToolTipText = "Compare Two Parquet Files";
            btnCompare.Click += btnCompare_Click;

            // toolStripSeparator2
            toolStripSeparator2.Name = "toolStripSeparator2";

            // lblFilterLabel
            lblFilterLabel.Name = "lblFilterLabel";
            lblFilterLabel.Text = "Filter:";

            // txtFilter
            txtFilter.Name = "txtFilter";
            txtFilter.Size = new System.Drawing.Size(180, 25);
            txtFilter.KeyDown += txtFilter_KeyDown;

            // lblColumnLabel
            lblColumnLabel.Name = "lblColumnLabel";
            lblColumnLabel.Text = "Column:";

            // cmbFilterColumn
            cmbFilterColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbFilterColumn.Name = "cmbFilterColumn";
            cmbFilterColumn.Size = new System.Drawing.Size(160, 25);
            cmbFilterColumn.Items.Add("(All Columns)");
            cmbFilterColumn.SelectedIndex = 0;

            // btnApplyFilter
            btnApplyFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnApplyFilter.Name = "btnApplyFilter";
            btnApplyFilter.Text = "Apply Filter";
            btnApplyFilter.Click += btnApplyFilter_Click;

            // dataGridView1
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 40;
            dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // lblFilePath
            lblFilePath = new System.Windows.Forms.Label();
            lblFilePath.AutoSize = false;
            lblFilePath.Dock = System.Windows.Forms.DockStyle.Top;
            lblFilePath.Height = 22;
            lblFilePath.Padding = new System.Windows.Forms.Padding(4, 3, 4, 0);
            lblFilePath.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Italic);
            lblFilePath.ForeColor = System.Drawing.Color.DimGray;
            lblFilePath.Text = "(no file loaded)";

            // pnlBottom
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Height = 36;
            pnlBottom.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            lblPageSizeLabel.Location = new System.Drawing.Point(440, 8);
            lblPageSizeLabel.Text = "Page size:";

            // cmbPageSize
            cmbPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPageSize.Location = new System.Drawing.Point(510, 5);
            cmbPageSize.Size = new System.Drawing.Size(70, 24);
            cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "500" });
            cmbPageSize.SelectedItem = "25";
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;

            // statusStrip1
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Spring = true;
            toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            statusStrip1.Items.Add(toolStripStatusLabel1);
            statusStrip1.SizingGrip = true;

            // lblStatus (alias for status label)
            lblStatus = new System.Windows.Forms.Label();
            lblStatus.AutoSize = true;

            // MainForm
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1200, 700);
            MainMenuStrip = menuStrip1;
            Text = "Parquet Explorer";
            Controls.Add(dataGridView1);
            Controls.Add(pnlBottom);
            Controls.Add(lblFilePath);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(statusStrip1);

            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.MenuStrip menuStrip1 = null!;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem = null!;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem = null!;
        private System.Windows.Forms.ToolStripMenuItem compareToolStripMenuItem = null!;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1 = null!;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem = null!;
        private System.Windows.Forms.ToolStrip toolStrip1 = null!;
        private System.Windows.Forms.ToolStripButton btnOpen = null!;
        private System.Windows.Forms.ToolStripButton btnCompare = null!;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2 = null!;
        private System.Windows.Forms.ToolStripLabel lblFilterLabel = null!;
        private System.Windows.Forms.ToolStripTextBox txtFilter = null!;
        private System.Windows.Forms.ToolStripLabel lblColumnLabel = null!;
        private System.Windows.Forms.ToolStripComboBox cmbFilterColumn = null!;
        private System.Windows.Forms.ToolStripButton btnApplyFilter = null!;
        private System.Windows.Forms.DataGridView dataGridView1 = null!;
        private System.Windows.Forms.Panel pnlBottom = null!;
        private System.Windows.Forms.Button btnPrev = null!;
        private System.Windows.Forms.Button btnNext = null!;
        private System.Windows.Forms.Label lblPageInfo = null!;
        private System.Windows.Forms.Label lblPageSizeLabel = null!;
        private System.Windows.Forms.ComboBox cmbPageSize = null!;
        private System.Windows.Forms.Label lblFilePath = null!;
        private System.Windows.Forms.Label lblStatus = null!;
        private System.Windows.Forms.StatusStrip statusStrip1 = null!;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1 = null!;
    }
}
