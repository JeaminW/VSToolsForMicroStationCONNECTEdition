namespace VSTMC
{
    partial class OptionsControlPage
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
            this.tbMSCEPath = new System.Windows.Forms.TextBox();
            this.tbSDKPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboBentleyProduct = new System.Windows.Forms.ComboBox();
            this.btnMSFolder = new System.Windows.Forms.Button();
            this.btnSDKFolder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbMDLAPPSPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnMDLAppsFolder = new System.Windows.Forms.Button();
            this.tbBuildPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBentleyBuildFolder = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnMDLAPPSPathBrowser = new System.Windows.Forms.Button();
            this.btnBatchBrowser = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnReset = new System.Windows.Forms.Button();
            this.btnMDLAPPSLock = new System.Windows.Forms.Button();
            this.btnBatchLock = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbMSCEPath
            // 
            this.tbMSCEPath.Enabled = false;
            this.tbMSCEPath.Location = new System.Drawing.Point(32, 96);
            this.tbMSCEPath.Name = "tbMSCEPath";
            this.tbMSCEPath.Size = new System.Drawing.Size(320, 20);
            this.tbMSCEPath.TabIndex = 0;
            this.tbMSCEPath.TextChanged += new System.EventHandler(this.tbMSCEPath_TextChanged);
            // 
            // tbSDKPath
            // 
            this.tbSDKPath.Enabled = false;
            this.tbSDKPath.Location = new System.Drawing.Point(32, 136);
            this.tbSDKPath.Name = "tbSDKPath";
            this.tbSDKPath.Size = new System.Drawing.Size(320, 20);
            this.tbSDKPath.TabIndex = 1;
            this.tbSDKPath.TextChanged += new System.EventHandler(this.tbSDKPath_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(8, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Bentley Product";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SDK Path";
            // 
            // cboBentleyProduct
            // 
            this.cboBentleyProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBentleyProduct.FormattingEnabled = true;
            this.cboBentleyProduct.Location = new System.Drawing.Point(32, 72);
            this.cboBentleyProduct.Name = "cboBentleyProduct";
            this.cboBentleyProduct.Size = new System.Drawing.Size(320, 21);
            this.cboBentleyProduct.TabIndex = 4;
            this.cboBentleyProduct.SelectedIndexChanged += new System.EventHandler(this.cboBentleyProduct_SelectedIndexChanged);
            // 
            // btnMSFolder
            // 
            this.btnMSFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMSFolder.Image = global::VSTMC.Properties.Resources.FolderOpen;
            this.btnMSFolder.Location = new System.Drawing.Point(8, 96);
            this.btnMSFolder.Name = "btnMSFolder";
            this.btnMSFolder.Size = new System.Drawing.Size(20, 20);
            this.btnMSFolder.TabIndex = 5;
            this.btnMSFolder.UseVisualStyleBackColor = true;
            this.btnMSFolder.Click += new System.EventHandler(this.btnMSFolder_Click);
            // 
            // btnSDKFolder
            // 
            this.btnSDKFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSDKFolder.Image = global::VSTMC.Properties.Resources.FolderOpen;
            this.btnSDKFolder.Location = new System.Drawing.Point(8, 136);
            this.btnSDKFolder.Name = "btnSDKFolder";
            this.btnSDKFolder.Size = new System.Drawing.Size(20, 20);
            this.btnSDKFolder.TabIndex = 6;
            this.btnSDKFolder.UseVisualStyleBackColor = true;
            this.btnSDKFolder.Click += new System.EventHandler(this.btnSDKFolder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(8, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Bentley SDK Folder";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::VSTMC.Properties.Resources.OptionsPageImage;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 50);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // tbMDLAPPSPath
            // 
            this.tbMDLAPPSPath.Location = new System.Drawing.Point(32, 176);
            this.tbMDLAPPSPath.Name = "tbMDLAPPSPath";
            this.tbMDLAPPSPath.Size = new System.Drawing.Size(320, 20);
            this.tbMDLAPPSPath.TabIndex = 9;
            this.tbMDLAPPSPath.TextChanged += new System.EventHandler(this.tbMDLAPPSPath_TextChanged);
            this.tbMDLAPPSPath.MouseLeave += new System.EventHandler(this.tbMDLAPPSPath_MouseLeave);
            this.tbMDLAPPSPath.MouseHover += new System.EventHandler(this.tbMDLAPPSPath_MouseHover);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(8, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "MDLAPPS Build Path";
            // 
            // btnMDLAppsFolder
            // 
            this.btnMDLAppsFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMDLAppsFolder.Image = global::VSTMC.Properties.Resources.FolderOpen;
            this.btnMDLAppsFolder.Location = new System.Drawing.Point(8, 176);
            this.btnMDLAppsFolder.Name = "btnMDLAppsFolder";
            this.btnMDLAppsFolder.Size = new System.Drawing.Size(20, 20);
            this.btnMDLAppsFolder.TabIndex = 11;
            this.btnMDLAppsFolder.UseVisualStyleBackColor = true;
            this.btnMDLAppsFolder.Click += new System.EventHandler(this.btnMDLAppsFolder_Click);
            // 
            // tbBuildPath
            // 
            this.tbBuildPath.Location = new System.Drawing.Point(32, 216);
            this.tbBuildPath.Name = "tbBuildPath";
            this.tbBuildPath.Size = new System.Drawing.Size(320, 20);
            this.tbBuildPath.TabIndex = 12;
            this.tbBuildPath.TextChanged += new System.EventHandler(this.tbBuildPath_TextChanged);
            this.tbBuildPath.MouseLeave += new System.EventHandler(this.tbBuildPath_MouseLeave);
            this.tbBuildPath.MouseHover += new System.EventHandler(this.tbBuildPath_MouseHover);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(8, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "C\\C++ Bentley Build Batch File";
            // 
            // btnBentleyBuildFolder
            // 
            this.btnBentleyBuildFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBentleyBuildFolder.Image = global::VSTMC.Properties.Resources.FolderOpen;
            this.btnBentleyBuildFolder.Location = new System.Drawing.Point(8, 216);
            this.btnBentleyBuildFolder.Name = "btnBentleyBuildFolder";
            this.btnBentleyBuildFolder.Size = new System.Drawing.Size(20, 20);
            this.btnBentleyBuildFolder.TabIndex = 14;
            this.btnBentleyBuildFolder.UseVisualStyleBackColor = true;
            this.btnBentleyBuildFolder.Click += new System.EventHandler(this.btnBentleyBuildFolder_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // btnMDLAPPSPathBrowser
            // 
            this.btnMDLAPPSPathBrowser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMDLAPPSPathBrowser.Image = global::VSTMC.Properties.Resources.Browse;
            this.btnMDLAPPSPathBrowser.Location = new System.Drawing.Point(352, 176);
            this.btnMDLAPPSPathBrowser.Name = "btnMDLAPPSPathBrowser";
            this.btnMDLAPPSPathBrowser.Size = new System.Drawing.Size(20, 20);
            this.btnMDLAPPSPathBrowser.TabIndex = 15;
            this.btnMDLAPPSPathBrowser.UseVisualStyleBackColor = true;
            this.btnMDLAPPSPathBrowser.Click += new System.EventHandler(this.btnMDLAPPSPathBrowser_Click);
            // 
            // btnBatchBrowser
            // 
            this.btnBatchBrowser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBatchBrowser.Image = global::VSTMC.Properties.Resources.Browse;
            this.btnBatchBrowser.Location = new System.Drawing.Point(352, 216);
            this.btnBatchBrowser.Name = "btnBatchBrowser";
            this.btnBatchBrowser.Size = new System.Drawing.Size(20, 20);
            this.btnBatchBrowser.TabIndex = 16;
            this.btnBatchBrowser.UseVisualStyleBackColor = true;
            this.btnBatchBrowser.Click += new System.EventHandler(this.btnBatchBrowser_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnReset
            // 
            this.btnReset.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnReset.Location = new System.Drawing.Point(280, 16);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 17;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnMDLAPPSLock
            // 
            this.btnMDLAPPSLock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMDLAPPSLock.Image = global::VSTMC.Properties.Resources.OpenedPadlock;
            this.btnMDLAPPSLock.Location = new System.Drawing.Point(376, 176);
            this.btnMDLAPPSLock.Name = "btnMDLAPPSLock";
            this.btnMDLAPPSLock.Size = new System.Drawing.Size(20, 20);
            this.btnMDLAPPSLock.TabIndex = 18;
            this.btnMDLAPPSLock.UseVisualStyleBackColor = true;
            this.btnMDLAPPSLock.Click += new System.EventHandler(this.btnMDLAPPSLock_Click);
            // 
            // btnBatchLock
            // 
            this.btnBatchLock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchLock.Image = global::VSTMC.Properties.Resources.OpenedPadlock;
            this.btnBatchLock.Location = new System.Drawing.Point(376, 216);
            this.btnBatchLock.Name = "btnBatchLock";
            this.btnBatchLock.Size = new System.Drawing.Size(20, 20);
            this.btnBatchLock.TabIndex = 19;
            this.btnBatchLock.UseVisualStyleBackColor = true;
            this.btnBatchLock.Click += new System.EventHandler(this.btnBatchLock_Click);
            // 
            // OptionsControlsMSCE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBatchLock);
            this.Controls.Add(this.btnMDLAPPSLock);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnBatchBrowser);
            this.Controls.Add(this.btnMDLAPPSPathBrowser);
            this.Controls.Add(this.btnBentleyBuildFolder);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbBuildPath);
            this.Controls.Add(this.btnMDLAppsFolder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbMDLAPPSPath);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSDKFolder);
            this.Controls.Add(this.btnMSFolder);
            this.Controls.Add(this.cboBentleyProduct);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbSDKPath);
            this.Controls.Add(this.tbMSCEPath);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "OptionsControlsMSCE";
            this.Size = new System.Drawing.Size(402, 298);
            this.Load += new System.EventHandler(this.OptionsControlsMSCE_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OptionsControlsMSCE_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbMSCEPath;
        private System.Windows.Forms.TextBox tbSDKPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboBentleyProduct;
        private System.Windows.Forms.Button btnMSFolder;
        private System.Windows.Forms.Button btnSDKFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tbMDLAPPSPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnMDLAppsFolder;
        private System.Windows.Forms.TextBox tbBuildPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBentleyBuildFolder;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnMDLAPPSPathBrowser;
        private System.Windows.Forms.Button btnBatchBrowser;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnMDLAPPSLock;
        private System.Windows.Forms.Button btnBatchLock;
    }
}
