namespace ImageGet
{
    partial class MainWin2
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("item1");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("item2");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin2));
            this.TaskListView = new System.Windows.Forms.ListView();
            this.SourceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.CloseCheckBox = new System.Windows.Forms.CheckBox();
            this.ShowButton = new System.Windows.Forms.Button();
            this.ActionButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DestinationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TaskListView
            // 
            this.TaskListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SourceColumnHeader,
            this.StateColumnHeader});
            this.TaskListView.FullRowSelect = true;
            this.TaskListView.GridLines = true;
            this.TaskListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            this.TaskListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.TaskListView.Location = new System.Drawing.Point(12, 31);
            this.TaskListView.MultiSelect = false;
            this.TaskListView.Name = "TaskListView";
            this.TaskListView.Size = new System.Drawing.Size(477, 200);
            this.TaskListView.TabIndex = 0;
            this.TaskListView.UseCompatibleStateImageBehavior = false;
            this.TaskListView.View = System.Windows.Forms.View.Details;
            // 
            // SourceColumnHeader
            // 
            this.SourceColumnHeader.Text = "Source";
            this.SourceColumnHeader.Width = 360;
            // 
            // StateColumnHeader
            // 
            this.StateColumnHeader.Text = "State";
            this.StateColumnHeader.Width = 85;
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(12, 240);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(477, 25);
            this.ProgressBar.Step = 1;
            this.ProgressBar.TabIndex = 1;
            // 
            // CloseCheckBox
            // 
            this.CloseCheckBox.AutoSize = true;
            this.CloseCheckBox.Checked = true;
            this.CloseCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CloseCheckBox.Location = new System.Drawing.Point(12, 279);
            this.CloseCheckBox.Name = "CloseCheckBox";
            this.CloseCheckBox.Size = new System.Drawing.Size(142, 19);
            this.CloseCheckBox.TabIndex = 2;
            this.CloseCheckBox.Text = "Close after completed";
            this.CloseCheckBox.UseVisualStyleBackColor = true;
            // 
            // ShowButton
            // 
            this.ShowButton.Location = new System.Drawing.Point(296, 273);
            this.ShowButton.Name = "ShowButton";
            this.ShowButton.Size = new System.Drawing.Size(112, 25);
            this.ShowButton.TabIndex = 3;
            this.ShowButton.Text = "Show Directory";
            this.ShowButton.UseVisualStyleBackColor = true;
            this.ShowButton.Click += new System.EventHandler(this.OnShowButtonClicked);
            // 
            // ActionButton
            // 
            this.ActionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionButton.Location = new System.Drawing.Point(414, 273);
            this.ActionButton.Name = "ActionButton";
            this.ActionButton.Size = new System.Drawing.Size(75, 25);
            this.ActionButton.TabIndex = 4;
            this.ActionButton.Text = "Cancel";
            this.ActionButton.UseVisualStyleBackColor = true;
            this.ActionButton.Click += new System.EventHandler(this.OnActionButtonClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Destination:";
            // 
            // DestinationLabel
            // 
            this.DestinationLabel.AutoSize = true;
            this.DestinationLabel.Location = new System.Drawing.Point(85, 9);
            this.DestinationLabel.Name = "DestinationLabel";
            this.DestinationLabel.Size = new System.Drawing.Size(38, 15);
            this.DestinationLabel.TabIndex = 6;
            this.DestinationLabel.Text = "label2";
            // 
            // MainWin2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 310);
            this.Controls.Add(this.DestinationLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ActionButton);
            this.Controls.Add(this.ShowButton);
            this.Controls.Add(this.CloseCheckBox);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.TaskListView);
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainWin2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImageGet";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView TaskListView;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.CheckBox CloseCheckBox;
        private System.Windows.Forms.Button ShowButton;
        private System.Windows.Forms.Button ActionButton;
        private System.Windows.Forms.ColumnHeader SourceColumnHeader;
        private System.Windows.Forms.ColumnHeader StateColumnHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label DestinationLabel;
    }
}