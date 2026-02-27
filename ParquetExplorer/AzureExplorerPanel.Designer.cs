namespace ParquetExplorer
{
    partial class AzureExplorerPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader        = new System.Windows.Forms.Panel();
            lblPanelTitle    = new System.Windows.Forms.Label();
            btnClosePanel    = new System.Windows.Forms.Button();
            pnlAuth          = new System.Windows.Forms.Panel();
            btnSignIn        = new System.Windows.Forms.Button();
            btnRefresh       = new System.Windows.Forms.Button();
            lblSignInStatus  = new System.Windows.Forms.Label();
            tblContent       = new System.Windows.Forms.TableLayoutPanel();
            pnlAccounts      = new System.Windows.Forms.Panel();
            lblAccountsLabel = new System.Windows.Forms.Label();
            lstAccounts      = new System.Windows.Forms.ListBox();
            pnlContainers    = new System.Windows.Forms.Panel();
            lblContainersLabel = new System.Windows.Forms.Label();
            lstContainers    = new System.Windows.Forms.ListBox();
            pnlBlobs         = new System.Windows.Forms.Panel();
            lblBlobsLabel    = new System.Windows.Forms.Label();
            lstBlobs         = new System.Windows.Forms.ListBox();
            pnlOpen          = new System.Windows.Forms.Panel();
            btnOpen          = new System.Windows.Forms.Button();
            lblStatus        = new System.Windows.Forms.Label();

            pnlHeader.SuspendLayout();
            pnlAuth.SuspendLayout();
            tblContent.SuspendLayout();
            pnlAccounts.SuspendLayout();
            pnlContainers.SuspendLayout();
            pnlBlobs.SuspendLayout();
            pnlOpen.SuspendLayout();
            SuspendLayout();

            // ── pnlHeader ──────────────────────────────────────────────────
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Height = 36;
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(40, 56, 72);
            pnlHeader.Controls.Add(lblPanelTitle);
            pnlHeader.Controls.Add(btnClosePanel);

            lblPanelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            lblPanelTitle.Text = "☁  Azure Explorer";
            lblPanelTitle.Font = new System.Drawing.Font("Segoe UI", 9.5f, System.Drawing.FontStyle.Bold);
            lblPanelTitle.ForeColor = System.Drawing.Color.White;
            lblPanelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblPanelTitle.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);

            btnClosePanel.Dock = System.Windows.Forms.DockStyle.Right;
            btnClosePanel.Width = 36;
            btnClosePanel.Text = "✕";
            btnClosePanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClosePanel.FlatAppearance.BorderSize = 0;
            btnClosePanel.BackColor = System.Drawing.Color.FromArgb(40, 56, 72);
            btnClosePanel.ForeColor = System.Drawing.Color.White;
            btnClosePanel.Font = new System.Drawing.Font("Segoe UI", 10f);
            btnClosePanel.Cursor = System.Windows.Forms.Cursors.Hand;
            btnClosePanel.Click += btnClosePanel_Click;

            // ── pnlAuth ────────────────────────────────────────────────────
            pnlAuth.Dock = System.Windows.Forms.DockStyle.Top;
            pnlAuth.Height = 44;
            pnlAuth.BackColor = System.Drawing.Color.FromArgb(240, 244, 251);
            pnlAuth.Padding = new System.Windows.Forms.Padding(6, 8, 6, 4);
            pnlAuth.Controls.Add(btnSignIn);
            pnlAuth.Controls.Add(btnRefresh);

            btnSignIn.Location = new System.Drawing.Point(6, 8);
            btnSignIn.Size = new System.Drawing.Size(150, 28);
            btnSignIn.Text = "🔑 Sign in with Azure";
            btnSignIn.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnSignIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSignIn.BackColor = System.Drawing.Color.FromArgb(44, 123, 229);
            btnSignIn.ForeColor = System.Drawing.Color.White;
            btnSignIn.FlatAppearance.BorderSize = 0;
            btnSignIn.Click += btnSignIn_Click;

            btnRefresh.Location = new System.Drawing.Point(162, 8);
            btnRefresh.Size = new System.Drawing.Size(90, 28);
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.BackColor = System.Drawing.Color.FromArgb(240, 244, 251);
            btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(190, 200, 215);
            btnRefresh.Click += btnRefresh_Click;

            // ── lblSignInStatus ────────────────────────────────────────────
            lblSignInStatus.Dock = System.Windows.Forms.DockStyle.Top;
            lblSignInStatus.Height = 22;
            lblSignInStatus.Padding = new System.Windows.Forms.Padding(8, 3, 4, 0);
            lblSignInStatus.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Italic);
            lblSignInStatus.ForeColor = System.Drawing.Color.FromArgb(80, 100, 130);
            lblSignInStatus.BackColor = System.Drawing.Color.FromArgb(240, 244, 251);
            lblSignInStatus.Text = "Sign in to discover storage accounts.";

            // ── pnlAccounts ────────────────────────────────────────────────
            pnlAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlAccounts.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            pnlAccounts.Controls.Add(lstAccounts);
            pnlAccounts.Controls.Add(lblAccountsLabel);

            lblAccountsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            lblAccountsLabel.Height = 18;
            lblAccountsLabel.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            lblAccountsLabel.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold);
            lblAccountsLabel.ForeColor = System.Drawing.Color.FromArgb(40, 56, 72);
            lblAccountsLabel.Text = "Storage Accounts";

            lstAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            lstAccounts.Font = new System.Drawing.Font("Segoe UI", 9f);
            lstAccounts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstAccounts.SelectedIndexChanged += lstAccounts_SelectedIndexChanged;

            // ── pnlContainers ──────────────────────────────────────────────
            pnlContainers.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlContainers.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            pnlContainers.Controls.Add(lstContainers);
            pnlContainers.Controls.Add(lblContainersLabel);

            lblContainersLabel.Dock = System.Windows.Forms.DockStyle.Top;
            lblContainersLabel.Height = 18;
            lblContainersLabel.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            lblContainersLabel.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold);
            lblContainersLabel.ForeColor = System.Drawing.Color.FromArgb(40, 56, 72);
            lblContainersLabel.Text = "Containers";

            lstContainers.Dock = System.Windows.Forms.DockStyle.Fill;
            lstContainers.Font = new System.Drawing.Font("Segoe UI", 9f);
            lstContainers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstContainers.SelectedIndexChanged += lstContainers_SelectedIndexChanged;

            // ── pnlBlobs ───────────────────────────────────────────────────
            pnlBlobs.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBlobs.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            pnlBlobs.Controls.Add(lstBlobs);
            pnlBlobs.Controls.Add(lblBlobsLabel);

            lblBlobsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            lblBlobsLabel.Height = 18;
            lblBlobsLabel.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            lblBlobsLabel.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold);
            lblBlobsLabel.ForeColor = System.Drawing.Color.FromArgb(40, 56, 72);
            lblBlobsLabel.Text = "Blobs";

            lstBlobs.Dock = System.Windows.Forms.DockStyle.Fill;
            lstBlobs.Font = new System.Drawing.Font("Segoe UI", 9f);
            lstBlobs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstBlobs.SelectedIndexChanged += lstBlobs_SelectedIndexChanged;
            lstBlobs.DoubleClick += lstBlobs_DoubleClick;

            // ── tblContent (3 equal rows) ──────────────────────────────────
            tblContent.Dock = System.Windows.Forms.DockStyle.Fill;
            tblContent.ColumnCount = 1;
            tblContent.RowCount = 3;
            tblContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(
                System.Windows.Forms.SizeType.Percent, 100f));
            tblContent.RowStyles.Add(new System.Windows.Forms.RowStyle(
                System.Windows.Forms.SizeType.Percent, 33f));
            tblContent.RowStyles.Add(new System.Windows.Forms.RowStyle(
                System.Windows.Forms.SizeType.Percent, 33f));
            tblContent.RowStyles.Add(new System.Windows.Forms.RowStyle(
                System.Windows.Forms.SizeType.Percent, 34f));
            tblContent.BackColor = System.Drawing.Color.White;
            tblContent.Controls.Add(pnlAccounts,   0, 0);
            tblContent.Controls.Add(pnlContainers, 0, 1);
            tblContent.Controls.Add(pnlBlobs,      0, 2);

            // ── pnlOpen ────────────────────────────────────────────────────
            pnlOpen.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlOpen.Height = 42;
            pnlOpen.BackColor = System.Drawing.Color.FromArgb(248, 249, 252);
            pnlOpen.Controls.Add(lblStatus);
            pnlOpen.Controls.Add(btnOpen);

            btnOpen.Location = new System.Drawing.Point(6, 7);
            btnOpen.Size = new System.Drawing.Size(120, 28);
            btnOpen.Text = "📂 Open Blob";
            btnOpen.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOpen.BackColor = System.Drawing.Color.FromArgb(44, 123, 229);
            btnOpen.ForeColor = System.Drawing.Color.White;
            btnOpen.FlatAppearance.BorderSize = 0;
            btnOpen.Enabled = false;
            btnOpen.Click += btnOpen_Click;

            lblStatus.AutoSize = false;
            lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Left
                             | System.Windows.Forms.AnchorStyles.Right
                             | System.Windows.Forms.AnchorStyles.Top;
            lblStatus.Location = new System.Drawing.Point(134, 11);
            lblStatus.Size = new System.Drawing.Size(150, 20);
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Italic);
            lblStatus.ForeColor = System.Drawing.Color.FromArgb(80, 100, 130);
            lblStatus.Text = string.Empty;

            // ── UserControl ────────────────────────────────────────────────
            // Controls.Add order: Fill first, Bottom next, Top controls last
            // (highest z-order = first docked, i.e. topmost for DockTop).
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Font = new System.Drawing.Font("Segoe UI", 9f);
            Controls.Add(tblContent);       // Fill  (index 0)
            Controls.Add(pnlOpen);          // Bottom (index 1)
            Controls.Add(lblSignInStatus);  // Top – innermost (index 2)
            Controls.Add(pnlAuth);          // Top – middle    (index 3)
            Controls.Add(pnlHeader);        // Top – outermost (index 4)

            pnlHeader.ResumeLayout(false);
            pnlAuth.ResumeLayout(false);
            tblContent.ResumeLayout(false);
            pnlAccounts.ResumeLayout(false);
            pnlContainers.ResumeLayout(false);
            pnlBlobs.ResumeLayout(false);
            pnlOpen.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader = null!;
        private System.Windows.Forms.Label lblPanelTitle = null!;
        private System.Windows.Forms.Button btnClosePanel = null!;
        private System.Windows.Forms.Panel pnlAuth = null!;
        private System.Windows.Forms.Button btnSignIn = null!;
        private System.Windows.Forms.Button btnRefresh = null!;
        private System.Windows.Forms.Label lblSignInStatus = null!;
        private System.Windows.Forms.TableLayoutPanel tblContent = null!;
        private System.Windows.Forms.Panel pnlAccounts = null!;
        private System.Windows.Forms.Label lblAccountsLabel = null!;
        private System.Windows.Forms.ListBox lstAccounts = null!;
        private System.Windows.Forms.Panel pnlContainers = null!;
        private System.Windows.Forms.Label lblContainersLabel = null!;
        private System.Windows.Forms.ListBox lstContainers = null!;
        private System.Windows.Forms.Panel pnlBlobs = null!;
        private System.Windows.Forms.Label lblBlobsLabel = null!;
        private System.Windows.Forms.ListBox lstBlobs = null!;
        private System.Windows.Forms.Panel pnlOpen = null!;
        private System.Windows.Forms.Button btnOpen = null!;
        private System.Windows.Forms.Label lblStatus = null!;
    }
}
