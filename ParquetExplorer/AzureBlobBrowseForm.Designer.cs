namespace ParquetExplorer
{
    partial class AzureBlobBrowseForm
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
            lblConnectionStringLabel = new System.Windows.Forms.Label();
            txtConnectionString = new System.Windows.Forms.TextBox();
            btnConnect = new System.Windows.Forms.Button();
            lblContainersLabel = new System.Windows.Forms.Label();
            lstContainers = new System.Windows.Forms.ListBox();
            lblBlobsLabel = new System.Windows.Forms.Label();
            lstBlobs = new System.Windows.Forms.ListBox();
            lblStatus = new System.Windows.Forms.Label();
            btnOpen = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            pnlTop = new System.Windows.Forms.Panel();
            pnlLists = new System.Windows.Forms.Panel();
            pnlBottom = new System.Windows.Forms.Panel();

            pnlTop.SuspendLayout();
            pnlLists.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();

            // lblConnectionStringLabel
            lblConnectionStringLabel.AutoSize = true;
            lblConnectionStringLabel.Location = new System.Drawing.Point(12, 14);
            lblConnectionStringLabel.Font = new System.Drawing.Font("Segoe UI", 9f);
            lblConnectionStringLabel.Text = "Connection String:";
            pnlTop.Controls.Add(lblConnectionStringLabel);

            // txtConnectionString
            txtConnectionString.Location = new System.Drawing.Point(120, 11);
            txtConnectionString.Size = new System.Drawing.Size(480, 23);
            txtConnectionString.Font = new System.Drawing.Font("Segoe UI", 9f);
            txtConnectionString.PlaceholderText = "DefaultEndpointsProtocol=https;AccountName=...";
            txtConnectionString.UseSystemPasswordChar = false;
            pnlTop.Controls.Add(txtConnectionString);

            // btnConnect
            btnConnect.Location = new System.Drawing.Point(608, 10);
            btnConnect.Size = new System.Drawing.Size(90, 26);
            btnConnect.Text = "🔗 Connect";
            btnConnect.Font = new System.Drawing.Font("Segoe UI", 9f);
            btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnConnect.BackColor = System.Drawing.Color.FromArgb(44, 123, 229);
            btnConnect.ForeColor = System.Drawing.Color.White;
            btnConnect.FlatAppearance.BorderSize = 0;
            btnConnect.Click += btnConnect_Click;
            pnlTop.Controls.Add(btnConnect);

            // pnlTop
            pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTop.Height = 48;
            pnlTop.Padding = new System.Windows.Forms.Padding(4);
            pnlTop.BackColor = System.Drawing.Color.FromArgb(240, 244, 251);

            // lblContainersLabel
            lblContainersLabel.AutoSize = true;
            lblContainersLabel.Location = new System.Drawing.Point(4, 4);
            lblContainersLabel.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            lblContainersLabel.Text = "Containers";
            pnlLists.Controls.Add(lblContainersLabel);

            // lstContainers
            lstContainers.Location = new System.Drawing.Point(4, 24);
            lstContainers.Size = new System.Drawing.Size(200, 330);
            lstContainers.Font = new System.Drawing.Font("Segoe UI", 9f);
            lstContainers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstContainers.SelectedIndexChanged += lstContainers_SelectedIndexChanged;
            pnlLists.Controls.Add(lstContainers);

            // lblBlobsLabel
            lblBlobsLabel.AutoSize = true;
            lblBlobsLabel.Location = new System.Drawing.Point(212, 4);
            lblBlobsLabel.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            lblBlobsLabel.Text = "Blobs";
            pnlLists.Controls.Add(lblBlobsLabel);

            // lstBlobs
            lstBlobs.Location = new System.Drawing.Point(212, 24);
            lstBlobs.Size = new System.Drawing.Size(480, 330);
            lstBlobs.Font = new System.Drawing.Font("Segoe UI", 9f);
            lstBlobs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstBlobs.SelectedIndexChanged += lstBlobs_SelectedIndexChanged;
            pnlLists.Controls.Add(lstBlobs);

            // pnlLists
            pnlLists.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlLists.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnlLists.BackColor = System.Drawing.Color.White;

            // lblStatus
            lblStatus.AutoSize = false;
            lblStatus.Location = new System.Drawing.Point(8, 10);
            lblStatus.Size = new System.Drawing.Size(450, 20);
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Italic);
            lblStatus.ForeColor = System.Drawing.Color.FromArgb(80, 100, 130);
            lblStatus.Text = "Enter a connection string and click Connect.";
            pnlBottom.Controls.Add(lblStatus);

            // btnOpen
            btnOpen.Location = new System.Drawing.Point(470, 6);
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
            btnCancel.Location = new System.Drawing.Point(578, 6);
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

            // AzureBlobBrowseForm
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(700, 460);
            MinimumSize = new System.Drawing.Size(700, 460);
            Text = "Open from Azure Blob Storage";
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

        private System.Windows.Forms.Label lblConnectionStringLabel = null!;
        private System.Windows.Forms.TextBox txtConnectionString = null!;
        private System.Windows.Forms.Button btnConnect = null!;
        private System.Windows.Forms.Label lblContainersLabel = null!;
        private System.Windows.Forms.ListBox lstContainers = null!;
        private System.Windows.Forms.Label lblBlobsLabel = null!;
        private System.Windows.Forms.ListBox lstBlobs = null!;
        private System.Windows.Forms.Label lblStatus = null!;
        private System.Windows.Forms.Button btnOpen = null!;
        private System.Windows.Forms.Button btnCancel = null!;
        private System.Windows.Forms.Panel pnlTop = null!;
        private System.Windows.Forms.Panel pnlLists = null!;
        private System.Windows.Forms.Panel pnlBottom = null!;
    }
}
