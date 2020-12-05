namespace SearchMyIndex
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtSearchText = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lstSearchResult = new System.Windows.Forms.ListBox();
            this.btnConfig = new System.Windows.Forms.Button();
            this.lblMgssage = new System.Windows.Forms.Label();
            this.btnRebuildAll = new System.Windows.Forms.Button();
            this.btnRebuild = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // txtSearchText
            // 
            this.txtSearchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchText.Location = new System.Drawing.Point(13, 43);
            this.txtSearchText.Name = "txtSearchText";
            this.txtSearchText.Size = new System.Drawing.Size(808, 19);
            this.txtSearchText.TabIndex = 0;
            this.txtSearchText.Enter += new System.EventHandler(this.txtSearchText_Enter);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(835, 41);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(67, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "検索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lstSearchResult
            // 
            this.lstSearchResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSearchResult.ContextMenuStrip = this.contextMenuStrip1;
            this.lstSearchResult.FormattingEnabled = true;
            this.lstSearchResult.ItemHeight = 12;
            this.lstSearchResult.Location = new System.Drawing.Point(13, 74);
            this.lstSearchResult.Name = "lstSearchResult";
            this.lstSearchResult.Size = new System.Drawing.Size(889, 316);
            this.lstSearchResult.TabIndex = 2;
            this.lstSearchResult.SelectedIndexChanged += new System.EventHandler(this.lstSearchResult_SelectedIndexChanged);
            this.lstSearchResult.Leave += new System.EventHandler(this.lstSearchResult_Leave);
            this.lstSearchResult.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstSearchResult_MouseDoubleClick);
            // 
            // btnConfig
            // 
            this.btnConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfig.Location = new System.Drawing.Point(835, 12);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(67, 23);
            this.btnConfig.TabIndex = 5;
            this.btnConfig.Text = "設定";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // lblMgssage
            // 
            this.lblMgssage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMgssage.Location = new System.Drawing.Point(12, 17);
            this.lblMgssage.Name = "lblMgssage";
            this.lblMgssage.Size = new System.Drawing.Size(809, 18);
            this.lblMgssage.TabIndex = 0;
            // 
            // btnRebuildAll
            // 
            this.btnRebuildAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRebuildAll.Location = new System.Drawing.Point(754, 12);
            this.btnRebuildAll.Name = "btnRebuildAll";
            this.btnRebuildAll.Size = new System.Drawing.Size(67, 23);
            this.btnRebuildAll.TabIndex = 4;
            this.btnRebuildAll.Text = "全再構築";
            this.btnRebuildAll.UseVisualStyleBackColor = true;
            this.btnRebuildAll.Click += new System.EventHandler(this.btnRebuildAll_Click);
            // 
            // btnRebuild
            // 
            this.btnRebuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRebuild.Location = new System.Drawing.Point(681, 12);
            this.btnRebuild.Name = "btnRebuild";
            this.btnRebuild.Size = new System.Drawing.Size(67, 23);
            this.btnRebuild.TabIndex = 3;
            this.btnRebuild.Text = "再構築";
            this.btnRebuild.UseVisualStyleBackColor = true;
            this.btnRebuild.Click += new System.EventHandler(this.btnRebuild_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 406);
            this.Controls.Add(this.btnRebuild);
            this.Controls.Add(this.btnRebuildAll);
            this.Controls.Add(this.lblMgssage);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.lstSearchResult);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearchText);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "SearchMyIndex";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearchText;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListBox lstSearchResult;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Label lblMgssage;
        private System.Windows.Forms.Button btnRebuildAll;
        private System.Windows.Forms.Button btnRebuild;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}

