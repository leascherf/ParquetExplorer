namespace ParquetExplorer
{
    partial class FilePickerDialog
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
            // ── Tab control ────────────────────────────────────────────────
            tabSources      = new System.Windows.Forms.TabControl();
            tabLocal        = new System.Windows.Forms.TabPage();
            tabSftp         = new System.Windows.Forms.TabPage();
            tabAzure        = new System.Windows.Forms.TabPage();

            // ── Local tab ──────────────────────────────────────────────────
            pnlLocalTop     = new System.Windows.Forms.Panel();
            lblLocalPath    = new System.Windows.Forms.Label();
            txtLocalPath    = new System.Windows.Forms.TextBox();
            btnBrowseLocal  = new System.Windows.Forms.Button();
            lblLocalFormats = new System.Windows.Forms.Label();

            // ── SFTP tab ───────────────────────────────────────────────────
            pnlSftpConn     = new System.Windows.Forms.Panel();
            lblSftpHost     = new System.Windows.Forms.Label();
            txtSftpHost     = new System.Windows.Forms.TextBox();
            lblSftpPort     = new System.Windows.Forms.Label();
            txtSftpPort     = new System.Windows.Forms.TextBox();
            lblSftpUser     = new System.Windows.Forms.Label();
            txtSftpUser     = new System.Windows.Forms.TextBox();
            lblSftpPass     = new System.Windows.Forms.Label();
            txtSftpPass     = new System.Windows.Forms.TextBox();
            btnSftpConnect  = new System.Windows.Forms.Button();
            pnlSftpNav      = new System.Windows.Forms.Panel();
            lblSftpPath     = new System.Windows.Forms.Label();
            txtSftpPath     = new System.Windows.Forms.TextBox();
            btnSftpUp       = new System.Windows.Forms.Button();
            lstSftp         = new System.Windows.Forms.ListBox();

            // ── Azure tab ─────────────────────────────────────────────────
            pnlAzureAuth    = new System.Windows.Forms.Panel();
            btnAzureSignIn  = new System.Windows.Forms.Button();
            btnAzureRefresh = new System.Windows.Forms.Button();
            lblAzureStatus  = new System.Windows.Forms.Label();
            tblAzureLists   = new System.Windows.Forms.TableLayoutPanel();
            pnlAzureAccounts   = new System.Windows.Forms.Panel();
            lblAzureAccounts   = new System.Windows.Forms.Label();
            lstAzureAccounts   = new System.Windows.Forms.ListBox();
            pnlAzureContainers = new System.Windows.Forms.Panel();
            lblAzureContainers = new System.Windows.Forms.Label();
            lstAzureContainers = new System.Windows.Forms.ListBox();
            pnlAzureBlobs      = new System.Windows.Forms.Panel();
            lblAzureBlobs      = new System.Windows.Forms.Label();
            lstAzureBlobs      = new System.Windows.Forms.ListBox();

            // ── Bottom panel (shared) ──────────────────────────────────────
            pnlBottom      = new System.Windows.Forms.Panel();
            lblPickStatus  = new System.Windows.Forms.Label();
            btnSelect      = new System.Windows.Forms.Button();
            btnCancelPick  = new System.Windows.Forms.Button();

            tabSources.SuspendLayout();
            tabLocal.SuspendLayout();
            tabSftp.SuspendLayout();
            tabAzure.SuspendLayout();
            pnlLocalTop.SuspendLayout();
            pnlSftpConn.SuspendLayout();
            pnlSftpNav.SuspendLayout();
            pnlAzureAuth.SuspendLayout();
            tblAzureLists.SuspendLayout();
            pnlAzureAccounts.SuspendLayout();
            pnlAzureContainers.SuspendLayout();
            pnlAzureBlobs.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();

            // ── LOCAL TAB ─────────────────────────────────────────────────

            // pnlLocalTop
            pnlLocalTop.Dock = System.Windows.Forms.DockStyle.Top;
            pnlLocalTop.Height = 54;
            pnlLocalTop.Padding = new System.Windows.Forms.Padding(8, 10, 8, 6);
            pnlLocalTop.BackColor = System.Drawing.Color.FromArgb(240, 244, 251);
            pnlLocalTop.Controls.Add(lblLocalPath);
            pnlLocalTop.Controls.Add(txtLocalPath);
            pnlLocalTop.Controls.Add(btnBrowseLocal);

            lblLocalPath.AutoSize = true;
            lblLocalPath.Location = new System.Drawing.Point(8, 16);
            lblLocalPath.Font = new System.Drawing.Font("Segoe UI", 9f);
            lblLocalPath.Text = "File:";

            txtLocalPath.Location = new System.Drawing.Point(44, 13);
            txtLocalPath.Size = new System.Drawing.Size(590, 23);
            txtLocalPath.Font = new System.Drawing.Font("Segoe UI", 9f);
            txtLocalPath.ReadOnly = true;
            txtLocalPath.BackColor = System.Drawing.Color.White;
            txtLocalPath.PlaceholderText = "(no file selected)";
            txtLocalPath.TabStop = false;

            btnBrowseLocal.Location = new System.Drawing.Point(642, 12);
            btnBrowseLocal.Size = new System.Drawing.Size(100, 26);
            btnBrowseLocal.Text = "📁 Browse...";
            btnBrowseLocal.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnBrowseLocal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnBrowseLocal.BackColor = System.Drawing.Color.FromArgb(44, 123, 229);
            btnBrowseLocal.ForeColor = System.Drawing.Color.White;
            btnBrowseLocal.FlatAppearance.BorderSize = 0;
            btnBrowseLocal.Click += btnBrowseLocal_Click;

            lblLocalFormats.Dock = System.Windows.Forms.DockStyle.Fill;
            lblLocalFormats.Padding = new System.Windows.Forms.Padding(8, 12, 8, 0);
            lblLocalFormats.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Italic);
            lblLocalFormats.ForeColor = System.Drawing.Color.FromArgb(80, 100, 130);
            lblLocalFormats.Text = "Supported formats: Parquet (.parquet), CSV (.csv), TSV (.tsv), Text (.txt)";

            tabLocal.Text = "💻  Local";
            tabLocal.BackColor = System.Drawing.Color.White;
            tabLocal.Controls.Add(lblLocalFormats);
            tabLocal.Controls.Add(pnlLocalTop);

            // ── SFTP TAB ──────────────────────────────────────────────────

            // pnlSftpConn — connection inputs
            pnlSftpConn.Dock = System.Windows.Forms.DockStyle.Top;
            pnlSftpConn.Height = 46;
            pnlSftpConn.BackColor = System.Drawing.Color.FromArgb(240, 244, 251);
            pnlSftpConn.Padding = new System.Windows.Forms.Padding(6, 8, 6, 4);

            lblSftpHost.AutoSize = true;
            lblSftpHost.Location = new System.Drawing.Point(8, 14);
            lblSftpHost.Font = new System.Drawing.Font("Segoe UI", 9f);
            lblSftpHost.Text = "Host:";

            txtSftpHost.Location = new System.Drawing.Point(46, 11);
            txtSftpHost.Size = new System.Drawing.Size(200, 23);
            txtSftpHost.Font = new System.Drawing.Font("Segoe UI", 9f);
            txtSftpHost.PlaceholderText = "sftp.example.com";

            lblSftpPort.AutoSize = true;
            lblSftpPort.Location = new System.Drawing.Point(254, 14);
            lblSftpPort.Font = new System.Drawing.Font("Segoe UI", 9f);
            lblSftpPort.Text = "Port:";

            txtSftpPort.Location = new System.Drawing.Point(288, 11);
            txtSftpPort.Size = new System.Drawing.Size(46, 23);
            txtSftpPort.Font = new System.Drawing.Font("Segoe UI", 9f);
            txtSftpPort.Text = "22";

            lblSftpUser.AutoSize = true;
            lblSftpUser.Location = new System.Drawing.Point(342, 14);
            lblSftpUser.Font = new System.Drawing.Font("Segoe UI", 9f);
            lblSftpUser.Text = "User:";

            txtSftpUser.Location = new System.Drawing.Point(378, 11);
            txtSftpUser.Size = new System.Drawing.Size(140, 23);
            txtSftpUser.Font = new System.Drawing.Font("Segoe UI", 9f);

            lblSftpPass.AutoSize = true;
            lblSftpPass.Location = new System.Drawing.Point(526, 14);
            lblSftpPass.Font = new System.Drawing.Font("Segoe UI", 9f);
            lblSftpPass.Text = "Password:";

            txtSftpPass.Location = new System.Drawing.Point(594, 11);
            txtSftpPass.Size = new System.Drawing.Size(140, 23);
            txtSftpPass.Font = new System.Drawing.Font("Segoe UI", 9f);
            txtSftpPass.UseSystemPasswordChar = true;

            btnSftpConnect.Location = new System.Drawing.Point(742, 9);
            btnSftpConnect.Size = new System.Drawing.Size(100, 26);
            btnSftpConnect.Text = "🔗 Connect";
            btnSftpConnect.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnSftpConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSftpConnect.BackColor = System.Drawing.Color.FromArgb(44, 123, 229);
            btnSftpConnect.ForeColor = System.Drawing.Color.White;
            btnSftpConnect.FlatAppearance.BorderSize = 0;
            btnSftpConnect.Click += btnSftpConnect_Click;

            pnlSftpConn.Controls.Add(lblSftpHost);
            pnlSftpConn.Controls.Add(txtSftpHost);
            pnlSftpConn.Controls.Add(lblSftpPort);
            pnlSftpConn.Controls.Add(txtSftpPort);
            pnlSftpConn.Controls.Add(lblSftpUser);
            pnlSftpConn.Controls.Add(txtSftpUser);
            pnlSftpConn.Controls.Add(lblSftpPass);
            pnlSftpConn.Controls.Add(txtSftpPass);
            pnlSftpConn.Controls.Add(btnSftpConnect);

            // pnlSftpNav — current path + Up button
            pnlSftpNav.Dock = System.Windows.Forms.DockStyle.Top;
            pnlSftpNav.Height = 36;
            pnlSftpNav.BackColor = System.Drawing.Color.FromArgb(248, 249, 252);
            pnlSftpNav.Padding = new System.Windows.Forms.Padding(6, 6, 6, 4);

            lblSftpPath.AutoSize = true;
            lblSftpPath.Location = new System.Drawing.Point(8, 10);
            lblSftpPath.Font = new System.Drawing.Font("Segoe UI", 9f);
            lblSftpPath.Text = "Path:";

            txtSftpPath.Location = new System.Drawing.Point(46, 7);
            txtSftpPath.Size = new System.Drawing.Size(700, 23);
            txtSftpPath.Font = new System.Drawing.Font("Segoe UI", 9f);
            txtSftpPath.ReadOnly = true;
            txtSftpPath.BackColor = System.Drawing.Color.White;
            txtSftpPath.Text = "/";

            btnSftpUp.Location = new System.Drawing.Point(754, 6);
            btnSftpUp.Size = new System.Drawing.Size(82, 24);
            btnSftpUp.Text = "↑ Up";
            btnSftpUp.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnSftpUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSftpUp.BackColor = System.Drawing.Color.FromArgb(240, 243, 248);
            btnSftpUp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(190, 200, 215);
            btnSftpUp.Enabled = false;
            btnSftpUp.Click += btnSftpUp_Click;

            pnlSftpNav.Controls.Add(lblSftpPath);
            pnlSftpNav.Controls.Add(txtSftpPath);
            pnlSftpNav.Controls.Add(btnSftpUp);

            lstSftp.Dock = System.Windows.Forms.DockStyle.Fill;
            lstSftp.Font = new System.Drawing.Font("Segoe UI", 9f);
            lstSftp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lstSftp.SelectedIndexChanged += lstSftp_SelectedIndexChanged;
            lstSftp.DoubleClick += lstSftp_DoubleClick;

            tabSftp.Text = "🔒  SFTP";
            tabSftp.BackColor = System.Drawing.Color.White;
            tabSftp.Controls.Add(lstSftp);
            tabSftp.Controls.Add(pnlSftpNav);
            tabSftp.Controls.Add(pnlSftpConn);

            // ── AZURE TAB ─────────────────────────────────────────────────

            // pnlAzureAuth
            pnlAzureAuth.Dock = System.Windows.Forms.DockStyle.Top;
            pnlAzureAuth.Height = 44;
            pnlAzureAuth.BackColor = System.Drawing.Color.FromArgb(240, 244, 251);
            pnlAzureAuth.Padding = new System.Windows.Forms.Padding(6, 8, 6, 4);

            btnAzureSignIn.Location = new System.Drawing.Point(6, 8);
            btnAzureSignIn.Size = new System.Drawing.Size(155, 28);
            btnAzureSignIn.Text = "🔑 Sign in with Azure";
            btnAzureSignIn.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnAzureSignIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAzureSignIn.BackColor = System.Drawing.Color.FromArgb(44, 123, 229);
            btnAzureSignIn.ForeColor = System.Drawing.Color.White;
            btnAzureSignIn.FlatAppearance.BorderSize = 0;
            btnAzureSignIn.Click += btnAzureSignIn_Click;

            btnAzureRefresh.Location = new System.Drawing.Point(168, 8);
            btnAzureRefresh.Size = new System.Drawing.Size(90, 28);
            btnAzureRefresh.Text = "🔄 Refresh";
            btnAzureRefresh.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnAzureRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAzureRefresh.BackColor = System.Drawing.Color.FromArgb(240, 244, 251);
            btnAzureRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(190, 200, 215);
            btnAzureRefresh.Click += btnAzureRefresh_Click;

            lblAzureStatus.AutoSize = false;
            lblAzureStatus.Location = new System.Drawing.Point(266, 14);
            lblAzureStatus.Size = new System.Drawing.Size(590, 20);
            lblAzureStatus.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Italic);
            lblAzureStatus.ForeColor = System.Drawing.Color.FromArgb(80, 100, 130);
            lblAzureStatus.Text = "Sign in with your Azure account to browse storage accounts.";

            pnlAzureAuth.Controls.Add(btnAzureSignIn);
            pnlAzureAuth.Controls.Add(btnAzureRefresh);
            pnlAzureAuth.Controls.Add(lblAzureStatus);

            // tblAzureLists — three equal columns for Accounts / Containers / Blobs
            tblAzureLists.Dock = System.Windows.Forms.DockStyle.Fill;
            tblAzureLists.ColumnCount = 3;
            tblAzureLists.RowCount = 1;
            tblAzureLists.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33f));
            tblAzureLists.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33f));
            tblAzureLists.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34f));
            tblAzureLists.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
            tblAzureLists.BackColor = System.Drawing.Color.White;
            tblAzureLists.Controls.Add(pnlAzureAccounts,   0, 0);
            tblAzureLists.Controls.Add(pnlAzureContainers, 1, 0);
            tblAzureLists.Controls.Add(pnlAzureBlobs,      2, 0);

            // pnlAzureAccounts
            pnlAzureAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlAzureAccounts.Padding = new System.Windows.Forms.Padding(4, 4, 2, 4);
            lblAzureAccounts.Dock = System.Windows.Forms.DockStyle.Top;
            lblAzureAccounts.Height = 18;
            lblAzureAccounts.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            lblAzureAccounts.Text = "Storage Accounts";
            lstAzureAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            lstAzureAccounts.Font = new System.Drawing.Font("Segoe UI", 9f);
            lstAzureAccounts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstAzureAccounts.SelectedIndexChanged += lstAzureAccounts_SelectedIndexChanged;
            pnlAzureAccounts.Controls.Add(lstAzureAccounts);
            pnlAzureAccounts.Controls.Add(lblAzureAccounts);

            // pnlAzureContainers
            pnlAzureContainers.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlAzureContainers.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            lblAzureContainers.Dock = System.Windows.Forms.DockStyle.Top;
            lblAzureContainers.Height = 18;
            lblAzureContainers.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            lblAzureContainers.Text = "Containers";
            lstAzureContainers.Dock = System.Windows.Forms.DockStyle.Fill;
            lstAzureContainers.Font = new System.Drawing.Font("Segoe UI", 9f);
            lstAzureContainers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstAzureContainers.SelectedIndexChanged += lstAzureContainers_SelectedIndexChanged;
            pnlAzureContainers.Controls.Add(lstAzureContainers);
            pnlAzureContainers.Controls.Add(lblAzureContainers);

            // pnlAzureBlobs
            pnlAzureBlobs.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlAzureBlobs.Padding = new System.Windows.Forms.Padding(2, 4, 4, 4);
            lblAzureBlobs.Dock = System.Windows.Forms.DockStyle.Top;
            lblAzureBlobs.Height = 18;
            lblAzureBlobs.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            lblAzureBlobs.Text = "Blobs";
            lstAzureBlobs.Dock = System.Windows.Forms.DockStyle.Fill;
            lstAzureBlobs.Font = new System.Drawing.Font("Segoe UI", 9f);
            lstAzureBlobs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstAzureBlobs.SelectedIndexChanged += lstAzureBlobs_SelectedIndexChanged;
            lstAzureBlobs.DoubleClick += lstAzureBlobs_DoubleClick;
            pnlAzureBlobs.Controls.Add(lstAzureBlobs);
            pnlAzureBlobs.Controls.Add(lblAzureBlobs);

            tabAzure.Text = "☁  Azure Blob";
            tabAzure.BackColor = System.Drawing.Color.White;
            tabAzure.Controls.Add(tblAzureLists);
            tabAzure.Controls.Add(pnlAzureAuth);

            // ── TabControl ────────────────────────────────────────────────
            tabSources.Dock = System.Windows.Forms.DockStyle.Fill;
            tabSources.Font = new System.Drawing.Font("Segoe UI", 9f);
            tabSources.TabPages.Add(tabLocal);
            tabSources.TabPages.Add(tabSftp);
            tabSources.TabPages.Add(tabAzure);
            tabSources.SelectedIndexChanged += tabSources_SelectedIndexChanged;

            // ── Bottom panel ──────────────────────────────────────────────
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Height = 44;
            pnlBottom.BackColor = System.Drawing.Color.FromArgb(248, 249, 252);
            pnlBottom.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);

            lblPickStatus.AutoSize = false;
            lblPickStatus.Anchor = System.Windows.Forms.AnchorStyles.Left
                                  | System.Windows.Forms.AnchorStyles.Right
                                  | System.Windows.Forms.AnchorStyles.Top;
            lblPickStatus.Location = new System.Drawing.Point(6, 12);
            lblPickStatus.Size = new System.Drawing.Size(660, 20);
            lblPickStatus.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Italic);
            lblPickStatus.ForeColor = System.Drawing.Color.FromArgb(80, 100, 130);
            lblPickStatus.Text = string.Empty;

            btnSelect.Location = new System.Drawing.Point(678, 8);
            btnSelect.Size = new System.Drawing.Size(120, 28);
            btnSelect.Text = "✓ Select File";
            btnSelect.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSelect.BackColor = System.Drawing.Color.FromArgb(44, 123, 229);
            btnSelect.ForeColor = System.Drawing.Color.White;
            btnSelect.FlatAppearance.BorderSize = 0;
            btnSelect.Enabled = false;
            btnSelect.Click += btnSelect_Click;

            btnCancelPick.Location = new System.Drawing.Point(806, 8);
            btnCancelPick.Size = new System.Drawing.Size(80, 28);
            btnCancelPick.Text = "Cancel";
            btnCancelPick.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnCancelPick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancelPick.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(190, 200, 215);
            btnCancelPick.Click += btnCancelPick_Click;

            pnlBottom.Controls.Add(lblPickStatus);
            pnlBottom.Controls.Add(btnSelect);
            pnlBottom.Controls.Add(btnCancelPick);

            // ── Dialog ────────────────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(900, 520);
            MinimumSize = new System.Drawing.Size(800, 460);
            Text = "Select File";
            Font = new System.Drawing.Font("Segoe UI", 9f);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            MaximizeBox = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Controls.Add(tabSources);
            Controls.Add(pnlBottom);

            tabSources.ResumeLayout(false);
            tabLocal.ResumeLayout(false);
            tabSftp.ResumeLayout(false);
            tabAzure.ResumeLayout(false);
            pnlLocalTop.ResumeLayout(false);
            pnlLocalTop.PerformLayout();
            pnlSftpConn.ResumeLayout(false);
            pnlSftpConn.PerformLayout();
            pnlSftpNav.ResumeLayout(false);
            pnlSftpNav.PerformLayout();
            pnlAzureAuth.ResumeLayout(false);
            pnlAzureAuth.PerformLayout();
            tblAzureLists.ResumeLayout(false);
            pnlAzureAccounts.ResumeLayout(false);
            pnlAzureContainers.ResumeLayout(false);
            pnlAzureBlobs.ResumeLayout(false);
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        // ── Tab control ────────────────────────────────────────────────────
        private System.Windows.Forms.TabControl tabSources = null!;
        private System.Windows.Forms.TabPage tabLocal = null!;
        private System.Windows.Forms.TabPage tabSftp = null!;
        private System.Windows.Forms.TabPage tabAzure = null!;

        // ── Local ──────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlLocalTop = null!;
        private System.Windows.Forms.Label lblLocalPath = null!;
        private System.Windows.Forms.TextBox txtLocalPath = null!;
        private System.Windows.Forms.Button btnBrowseLocal = null!;
        private System.Windows.Forms.Label lblLocalFormats = null!;

        // ── SFTP ───────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlSftpConn = null!;
        private System.Windows.Forms.Label lblSftpHost = null!;
        private System.Windows.Forms.TextBox txtSftpHost = null!;
        private System.Windows.Forms.Label lblSftpPort = null!;
        private System.Windows.Forms.TextBox txtSftpPort = null!;
        private System.Windows.Forms.Label lblSftpUser = null!;
        private System.Windows.Forms.TextBox txtSftpUser = null!;
        private System.Windows.Forms.Label lblSftpPass = null!;
        private System.Windows.Forms.TextBox txtSftpPass = null!;
        private System.Windows.Forms.Button btnSftpConnect = null!;
        private System.Windows.Forms.Panel pnlSftpNav = null!;
        private System.Windows.Forms.Label lblSftpPath = null!;
        private System.Windows.Forms.TextBox txtSftpPath = null!;
        private System.Windows.Forms.Button btnSftpUp = null!;
        private System.Windows.Forms.ListBox lstSftp = null!;

        // ── Azure ──────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlAzureAuth = null!;
        private System.Windows.Forms.Button btnAzureSignIn = null!;
        private System.Windows.Forms.Button btnAzureRefresh = null!;
        private System.Windows.Forms.Label lblAzureStatus = null!;
        private System.Windows.Forms.TableLayoutPanel tblAzureLists = null!;
        private System.Windows.Forms.Panel pnlAzureAccounts = null!;
        private System.Windows.Forms.Label lblAzureAccounts = null!;
        private System.Windows.Forms.ListBox lstAzureAccounts = null!;
        private System.Windows.Forms.Panel pnlAzureContainers = null!;
        private System.Windows.Forms.Label lblAzureContainers = null!;
        private System.Windows.Forms.ListBox lstAzureContainers = null!;
        private System.Windows.Forms.Panel pnlAzureBlobs = null!;
        private System.Windows.Forms.Label lblAzureBlobs = null!;
        private System.Windows.Forms.ListBox lstAzureBlobs = null!;

        // ── Bottom ─────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlBottom = null!;
        private System.Windows.Forms.Label lblPickStatus = null!;
        private System.Windows.Forms.Button btnSelect = null!;
        private System.Windows.Forms.Button btnCancelPick = null!;
    }
}
