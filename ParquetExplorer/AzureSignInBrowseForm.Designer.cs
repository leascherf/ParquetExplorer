namespace ParquetExplorer
{
    partial class AzureSignInBrowseForm
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
            btnSignIn = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            lblSignInStatus = new System.Windows.Forms.Label();

            pnlLists = new System.Windows.Forms.Panel();
            lblAccountsLabel = new System.Windows.Forms.Label();
            lstAccounts = new System.Windows.Forms.ListBox();
            lblContainersLabel = new System.Windows.Forms.Label();
            lstContainers = new System.Windows.Forms.ListBox();
            lblBlobsLabel = new System.Windows.Forms.Label();
            lstBlobs = new System.Windows.Forms.ListBox();

            pnlBottom = new System.Windows.Forms.Panel();
            lblStatus = new System.Windows.Forms.Label();
            btnOpen = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();

            pnlTop.SuspendLayout();
            pnlLists.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();

            // btnSignIn
            btnSignIn.Location = new System.Drawing.Point(12, 11);
            btnSignIn.Size = new System.Drawing.Size(160, 28);
            btnSignIn.Text = "🔑 Sign in with Azure";
            btnSignIn.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnSignIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSignIn.BackColor = System.Drawing.Color.FromArgb(44, 123, 229);
            btnSignIn.ForeColor = System.Drawing.Color.White;
            btnSignIn.FlatAppearance.BorderSize = 0;
            btnSignIn.Click += btnSignIn_Click;
            pnlTop.Controls.Add(btnSignIn);

            // btnRefresh
            btnRefresh.Location = new System.Drawing.Point(180, 11);
            btnRefresh.Size = new System.Drawing.Size(100, 28);
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.BackColor = System.Drawing.Color.FromArgb(240, 244, 251);
            btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(190, 200, 215);
            btnRefresh.Click += btnRefresh_Click;
            pnlTop.Controls.Add(btnRefresh);

            // lblSignInStatus
            lblSignInStatus.AutoSize = false;
            lblSignInStatus.Location = new System.Drawing.Point(290, 14);
            lblSignInStatus.Size = new System.Drawing.Size(560, 20);
            lblSignInStatus.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Italic);
            lblSignInStatus.ForeColor = System.Drawing.Color.FromArgb(80, 100, 130);
            lblSignInStatus.Text = "Click \"Sign in with Azure\" to discover your storage accounts.";
            pnlTop.Controls.Add(lblSignInStatus);

            // pnlTop
            pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTop.Height = 52;
            pnlTop.Padding = new System.Windows.Forms.Padding(4);
            pnlTop.BackColor = System.Drawing.Color.FromArgb(240, 244, 251);

            // lblAccountsLabel
            lblAccountsLabel.AutoSize = true;
            lblAccountsLabel.Location = new System.Drawing.Point(4, 4);
            lblAccountsLabel.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            lblAccountsLabel.Text = "Storage Accounts";
            pnlLists.Controls.Add(lblAccountsLabel);

            // lstAccounts
            lstAccounts.Location = new System.Drawing.Point(4, 24);
            lstAccounts.Size = new System.Drawing.Size(240, 330);
            lstAccounts.Font = new System.Drawing.Font("Segoe UI", 9f);
            lstAccounts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstAccounts.SelectedIndexChanged += lstAccounts_SelectedIndexChanged;
            pnlLists.Controls.Add(lstAccounts);

            // lblContainersLabel
            lblContainersLabel.AutoSize = true;
            lblContainersLabel.Location = new System.Drawing.Point(252, 4);
            lblContainersLabel.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            lblContainersLabel.Text = "Containers";
            pnlLists.Controls.Add(lblContainersLabel);

            // lstContainers
            lstContainers.Location = new System.Drawing.Point(252, 24);
            lstContainers.Size = new System.Drawing.Size(200, 330);
            lstContainers.Font = new System.Drawing.Font("Segoe UI", 9f);
            lstContainers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstContainers.SelectedIndexChanged += lstContainers_SelectedIndexChanged;
            pnlLists.Controls.Add(lstContainers);

            // lblBlobsLabel
            lblBlobsLabel.AutoSize = true;
            lblBlobsLabel.Location = new System.Drawing.Point(460, 4);
            lblBlobsLabel.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            lblBlobsLabel.Text = "Blobs";
            pnlLists.Controls.Add(lblBlobsLabel);

            // lstBlobs
            lstBlobs.Location = new System.Drawing.Point(460, 24);
            lstBlobs.Size = new System.Drawing.Size(320, 330);
            lstBlobs.Font = new System.Drawing.Font("Segoe UI", 9f);
            lstBlobs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstBlobs.SelectedIndexChanged += lstBlobs_SelectedIndexChanged;
            pnlLists.Controls.Add(lstBlobs);

            // pnlLists
            pnlLists.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlLists.Padding = new System.Windows.Forms.Padding(4);
            pnlLists.BackColor = System.Drawing.Color.White;

            // lblStatus
            lblStatus.AutoSize = false;
            lblStatus.Location = new System.Drawing.Point(8, 10);
            lblStatus.Size = new System.Drawing.Size(520, 20);
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Italic);
            lblStatus.ForeColor = System.Drawing.Color.FromArgb(80, 100, 130);
            lblStatus.Text = string.Empty;
            pnlBottom.Controls.Add(lblStatus);

            // btnOpen
            btnOpen.Location = new System.Drawing.Point(548, 6);
            btnOpen.Size = new System.Drawing.Size(100, 28);
            btnOpen.Text = "📂 Open Blob";
            btnOpen.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOpen.BackColor = System.Drawing.Color.FromArgb(44, 123, 229);
            btnOpen.ForeColor = System.Drawing.Color.White;
            btnOpen.FlatAppearance.BorderSize = 0;
            btnOpen.Enabled = false;
            btnOpen.Click += btnOpen_Click;
            pnlBottom.Controls.Add(btnOpen);

            // btnCancel
            btnCancel.Location = new System.Drawing.Point(656, 6);
            btnCancel.Size = new System.Drawing.Size(80, 28);
            btnCancel.Text = "Cancel";
            btnCancel.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(190, 200, 215);
            btnCancel.Click += btnCancel_Click;
            pnlBottom.Controls.Add(btnCancel);

            // pnlBottom
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Height = 42;
            pnlBottom.BackColor = System.Drawing.Color.FromArgb(248, 249, 252);

            // AzureSignInBrowseForm
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 460);
            MinimumSize = new System.Drawing.Size(800, 460);
            Text = "Open from Azure (Sign In)";
            Font = new System.Drawing.Font("Segoe UI", 9f);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Controls.Add(pnlLists);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);

            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlLists.ResumeLayout(false);
            pnlLists.PerformLayout();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTop = null!;
        private System.Windows.Forms.Button btnSignIn = null!;
        private System.Windows.Forms.Button btnRefresh = null!;
        private System.Windows.Forms.Label lblSignInStatus = null!;
        private System.Windows.Forms.Panel pnlLists = null!;
        private System.Windows.Forms.Label lblAccountsLabel = null!;
        private System.Windows.Forms.ListBox lstAccounts = null!;
        private System.Windows.Forms.Label lblContainersLabel = null!;
        private System.Windows.Forms.ListBox lstContainers = null!;
        private System.Windows.Forms.Label lblBlobsLabel = null!;
        private System.Windows.Forms.ListBox lstBlobs = null!;
        private System.Windows.Forms.Panel pnlBottom = null!;
        private System.Windows.Forms.Label lblStatus = null!;
        private System.Windows.Forms.Button btnOpen = null!;
        private System.Windows.Forms.Button btnCancel = null!;
    }
}
