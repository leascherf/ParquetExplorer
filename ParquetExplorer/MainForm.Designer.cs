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
            openAzureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            compareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            toolStrip1 = new System.Windows.Forms.ToolStrip();
            btnOpen = new System.Windows.Forms.ToolStripButton();
            btnOpenAzure = new System.Windows.Forms.ToolStripButton();
            btnCompare = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            lblFilterLabel = new System.Windows.Forms.ToolStripLabel();
            txtFilter = new System.Windows.Forms.ToolStripTextBox();
            lblColumnLabel = new System.Windows.Forms.ToolStripLabel();
            cmbFilterColumn = new System.Windows.Forms.ToolStripComboBox();
            btnApplyFilter = new System.Windows.Forms.ToolStripButton();
            btnShowEmptyColumns = new System.Windows.Forms.ToolStripButton();

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
            menuStrip1.BackColor = System.Drawing.Color.FromArgb(40, 56, 72);
            menuStrip1.ForeColor = System.Drawing.Color.White;

            // fileToolStripMenuItem
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                openToolStripMenuItem, openAzureToolStripMenuItem, compareToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem
            });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Text = "&File";
            fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;

            // openToolStripMenuItem
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            openToolStripMenuItem.Text = "&Open Local File...";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;

            // openAzureToolStripMenuItem
            openAzureToolStripMenuItem.Name = "openAzureToolStripMenuItem";
            openAzureToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.O;
            openAzureToolStripMenuItem.Text = "Open from &Azure Blob Storage...";
            openAzureToolStripMenuItem.Click += openAzureToolStripMenuItem_Click;

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
                btnOpen, btnOpenAzure, btnCompare, toolStripSeparator2,
                lblFilterLabel, txtFilter, lblColumnLabel, cmbFilterColumn, btnApplyFilter, btnShowEmptyColumns
            });
            toolStrip1.Location = new System.Drawing.Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(1200, 32);
            toolStrip1.TabIndex = 1;
            toolStrip1.BackColor = System.Drawing.Color.FromArgb(248, 249, 252);
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            toolStrip1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);

            // btnOpen
            btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnOpen.Name = "btnOpen";
            btnOpen.Text = "📂 Open Local";
            btnOpen.ToolTipText = "Open Local Parquet File (Ctrl+O)";
            btnOpen.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            btnOpen.Click += btnOpen_Click;

            // btnOpenAzure
            btnOpenAzure.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnOpenAzure.Name = "btnOpenAzure";
            btnOpenAzure.Text = "☁ Open Azure";
            btnOpenAzure.ToolTipText = "Open Parquet from Azure Blob Storage (Ctrl+Shift+O)";
            btnOpenAzure.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            btnOpenAzure.Click += btnOpenAzure_Click;

            // btnCompare
            btnCompare.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnCompare.Name = "btnCompare";
            btnCompare.Text = "⇄ Compare";
            btnCompare.ToolTipText = "Compare Two Parquet Files";
            btnCompare.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            btnApplyFilter.Text = "🔍 Apply";
            btnApplyFilter.ToolTipText = "Apply filter (press Enter in the filter box)";
            btnApplyFilter.Click += btnApplyFilter_Click;

            // btnShowEmptyColumns
            btnShowEmptyColumns.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnShowEmptyColumns.Name = "btnShowEmptyColumns";
            btnShowEmptyColumns.Text = "Show Empty Columns";
            btnShowEmptyColumns.ToolTipText = "Toggle visibility of columns with no data";
            btnShowEmptyColumns.CheckOnClick = true;
            btnShowEmptyColumns.Click += btnShowEmptyColumns_Click;

            // dataGridView1
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 40;
            dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            dataGridView1.BackgroundColor = System.Drawing.Color.White;
            dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(40, 56, 72);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(40, 56, 72);
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dataGridView1.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 248, 255);
            dataGridView1.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(44, 123, 229);
            dataGridView1.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9f);
            dataGridView1.GridColor = System.Drawing.Color.FromArgb(220, 225, 235);
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 249, 252);
            dataGridView1.RowHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(100, 110, 120);
            dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;

            // lblFilePath
            lblFilePath = new System.Windows.Forms.Label();
            lblFilePath.AutoSize = false;
            lblFilePath.Dock = System.Windows.Forms.DockStyle.Top;
            lblFilePath.Height = 26;
            lblFilePath.Padding = new System.Windows.Forms.Padding(8, 4, 4, 0);
            lblFilePath.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Italic);
            lblFilePath.ForeColor = System.Drawing.Color.FromArgb(80, 100, 130);
            lblFilePath.BackColor = System.Drawing.Color.FromArgb(240, 244, 251);
            lblFilePath.Text = "(no file loaded — use File → Open Parquet or Ctrl+O)";

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
            cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "500" });
            cmbPageSize.SelectedItem = "25";
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;

            // statusStrip1
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Spring = true;
            toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolStripStatusLabel1.ForeColor = System.Drawing.Color.White;
            statusStrip1.Items.Add(toolStripStatusLabel1);
            statusStrip1.SizingGrip = true;
            statusStrip1.BackColor = System.Drawing.Color.FromArgb(40, 56, 72);
            statusStrip1.ForeColor = System.Drawing.Color.White;

            // lblStatus (alias for status label)
            lblStatus = new System.Windows.Forms.Label();
            lblStatus.AutoSize = true;

            // MainForm
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1200, 700);
            MinimumSize = new System.Drawing.Size(900, 550);
            MainMenuStrip = menuStrip1;
            Text = "Parquet Explorer";
            Font = new System.Drawing.Font("Segoe UI", 9f);
            BackColor = System.Drawing.Color.FromArgb(248, 249, 252);
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
        private System.Windows.Forms.ToolStripMenuItem openAzureToolStripMenuItem = null!;
        private System.Windows.Forms.ToolStripMenuItem compareToolStripMenuItem = null!;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1 = null!;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem = null!;
        private System.Windows.Forms.ToolStrip toolStrip1 = null!;
        private System.Windows.Forms.ToolStripButton btnOpen = null!;
        private System.Windows.Forms.ToolStripButton btnOpenAzure = null!;
        private System.Windows.Forms.ToolStripButton btnCompare = null!;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2 = null!;
        private System.Windows.Forms.ToolStripLabel lblFilterLabel = null!;
        private System.Windows.Forms.ToolStripTextBox txtFilter = null!;
        private System.Windows.Forms.ToolStripLabel lblColumnLabel = null!;
        private System.Windows.Forms.ToolStripComboBox cmbFilterColumn = null!;
        private System.Windows.Forms.ToolStripButton btnApplyFilter = null!;
        private System.Windows.Forms.ToolStripButton btnShowEmptyColumns = null!;
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
