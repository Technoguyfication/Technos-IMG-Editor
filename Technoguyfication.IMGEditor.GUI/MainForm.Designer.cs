namespace Technoguyfication.IMGEditor.GUI
{
	partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fileToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.extractAlllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fileToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.itemViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.largeIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.smallIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sortingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.defaultSortingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.offsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.ascendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.descendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fileListView = new System.Windows.Forms.ListView();
			this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.sizeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.offsetHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.largeFileIcons = new System.Windows.Forms.ImageList(this.components);
			this.smallFileIcons = new System.Windows.Forms.ImageList(this.components);
			this.mainMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenuStrip
			// 
			this.mainMenuStrip.BackColor = System.Drawing.SystemColors.Window;
			this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			this.mainMenuStrip.Name = "mainMenuStrip";
			this.mainMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.mainMenuStrip.Size = new System.Drawing.Size(348, 24);
			this.mainMenuStrip.TabIndex = 0;
			this.mainMenuStrip.Text = "mainMenuStrip";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.fileToolStripSeparator,
            this.extractToolStripMenuItem,
            this.extractAlllToolStripMenuItem,
            this.addFilesToolStripMenuItem,
            this.fileToolStripSeparator3,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Image = global::Technoguyfication.IMGEditor.GUI.Properties.Resources.database_add;
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.newToolStripMenuItem.Text = "&New...";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.openToolStripMenuItem.Text = "&Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
			// 
			// fileToolStripSeparator
			// 
			this.fileToolStripSeparator.Name = "fileToolStripSeparator";
			this.fileToolStripSeparator.Size = new System.Drawing.Size(132, 6);
			// 
			// extractToolStripMenuItem
			// 
			this.extractToolStripMenuItem.Image = global::Technoguyfication.IMGEditor.GUI.Properties.Resources.page_white_get;
			this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
			this.extractToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.extractToolStripMenuItem.Text = "&Extract...";
			// 
			// extractAlllToolStripMenuItem
			// 
			this.extractAlllToolStripMenuItem.Image = global::Technoguyfication.IMGEditor.GUI.Properties.Resources.folder_go;
			this.extractAlllToolStripMenuItem.Name = "extractAlllToolStripMenuItem";
			this.extractAlllToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.extractAlllToolStripMenuItem.Text = "Extract All...";
			// 
			// addFilesToolStripMenuItem
			// 
			this.addFilesToolStripMenuItem.Image = global::Technoguyfication.IMGEditor.GUI.Properties.Resources.page_add;
			this.addFilesToolStripMenuItem.Name = "addFilesToolStripMenuItem";
			this.addFilesToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.addFilesToolStripMenuItem.Text = "&Add Files...";
			// 
			// fileToolStripSeparator3
			// 
			this.fileToolStripSeparator3.Name = "fileToolStripSeparator3";
			this.fileToolStripSeparator3.Size = new System.Drawing.Size(132, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemViewToolStripMenuItem,
            this.sortingToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "&View";
			// 
			// itemViewToolStripMenuItem
			// 
			this.itemViewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.detailsToolStripMenuItem,
            this.largeIconToolStripMenuItem,
            this.smallIconToolStripMenuItem});
			this.itemViewToolStripMenuItem.Name = "itemViewToolStripMenuItem";
			this.itemViewToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
			this.itemViewToolStripMenuItem.Text = "Item View";
			// 
			// detailsToolStripMenuItem
			// 
			this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
			this.detailsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.detailsToolStripMenuItem.Text = "Details";
			this.detailsToolStripMenuItem.Click += new System.EventHandler(this.DetailsToolStripMenuItem_Click);
			// 
			// largeIconToolStripMenuItem
			// 
			this.largeIconToolStripMenuItem.Name = "largeIconToolStripMenuItem";
			this.largeIconToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.largeIconToolStripMenuItem.Text = "Large Icons";
			this.largeIconToolStripMenuItem.Click += new System.EventHandler(this.LargeIconToolStripMenuItem_Click);
			// 
			// smallIconToolStripMenuItem
			// 
			this.smallIconToolStripMenuItem.Name = "smallIconToolStripMenuItem";
			this.smallIconToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.smallIconToolStripMenuItem.Text = "Small Icons";
			this.smallIconToolStripMenuItem.Click += new System.EventHandler(this.SmallIconToolStripMenuItem_Click);
			// 
			// sortingToolStripMenuItem
			// 
			this.sortingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultSortingToolStripMenuItem,
            this.nameToolStripMenuItem,
            this.sizeToolStripMenuItem,
            this.offsetToolStripMenuItem,
            this.toolStripMenuItem1,
            this.ascendingToolStripMenuItem,
            this.descendingToolStripMenuItem});
			this.sortingToolStripMenuItem.Name = "sortingToolStripMenuItem";
			this.sortingToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
			this.sortingToolStripMenuItem.Text = "Sorting";
			// 
			// defaultSortingToolStripMenuItem
			// 
			this.defaultSortingToolStripMenuItem.Name = "defaultSortingToolStripMenuItem";
			this.defaultSortingToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
			this.defaultSortingToolStripMenuItem.Text = "Default Sorting";
			// 
			// nameToolStripMenuItem
			// 
			this.nameToolStripMenuItem.Name = "nameToolStripMenuItem";
			this.nameToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
			this.nameToolStripMenuItem.Text = "Name";
			// 
			// sizeToolStripMenuItem
			// 
			this.sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
			this.sizeToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
			this.sizeToolStripMenuItem.Text = "Size";
			// 
			// offsetToolStripMenuItem
			// 
			this.offsetToolStripMenuItem.Name = "offsetToolStripMenuItem";
			this.offsetToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
			this.offsetToolStripMenuItem.Text = "Offset";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(150, 6);
			// 
			// ascendingToolStripMenuItem
			// 
			this.ascendingToolStripMenuItem.Name = "ascendingToolStripMenuItem";
			this.ascendingToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
			this.ascendingToolStripMenuItem.Text = "Ascending";
			// 
			// descendingToolStripMenuItem
			// 
			this.descendingToolStripMenuItem.Name = "descendingToolStripMenuItem";
			this.descendingToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
			this.descendingToolStripMenuItem.Text = "Descending";
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewHelpToolStripMenuItem,
            this.helpToolStripMenuSeparator,
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// viewHelpToolStripMenuItem
			// 
			this.viewHelpToolStripMenuItem.Image = global::Technoguyfication.IMGEditor.GUI.Properties.Resources.help;
			this.viewHelpToolStripMenuItem.Name = "viewHelpToolStripMenuItem";
			this.viewHelpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.viewHelpToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.viewHelpToolStripMenuItem.Text = "View Help";
			// 
			// helpToolStripMenuSeparator
			// 
			this.helpToolStripMenuSeparator.Name = "helpToolStripMenuSeparator";
			this.helpToolStripMenuSeparator.Size = new System.Drawing.Size(143, 6);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.aboutToolStripMenuItem.Text = "About...";
			// 
			// fileListView
			// 
			this.fileListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.sizeColumnHeader,
            this.offsetHeader});
			this.fileListView.FullRowSelect = true;
			this.fileListView.LargeImageList = this.largeFileIcons;
			this.fileListView.Location = new System.Drawing.Point(12, 27);
			this.fileListView.Name = "fileListView";
			this.fileListView.Size = new System.Drawing.Size(324, 259);
			this.fileListView.SmallImageList = this.smallFileIcons;
			this.fileListView.TabIndex = 1;
			this.fileListView.UseCompatibleStateImageBehavior = false;
			this.fileListView.View = System.Windows.Forms.View.Details;
			this.fileListView.VirtualMode = true;
			this.fileListView.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.FileListView_RetreiveVirtualItem);
			// 
			// nameColumnHeader
			// 
			this.nameColumnHeader.Text = "File Name";
			this.nameColumnHeader.Width = 140;
			// 
			// sizeColumnHeader
			// 
			this.sizeColumnHeader.Text = "Size";
			this.sizeColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// offsetHeader
			// 
			this.offsetHeader.Text = "Offset";
			this.offsetHeader.Width = 90;
			// 
			// largeFileIcons
			// 
			this.largeFileIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("largeFileIcons.ImageStream")));
			this.largeFileIcons.TransparentColor = System.Drawing.Color.Transparent;
			this.largeFileIcons.Images.SetKeyName(0, "image.png");
			this.largeFileIcons.Images.SetKeyName(1, "model.png");
			this.largeFileIcons.Images.SetKeyName(2, "unknown.png");
			this.largeFileIcons.Images.SetKeyName(3, "animation.png");
			// 
			// smallFileIcons
			// 
			this.smallFileIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smallFileIcons.ImageStream")));
			this.smallFileIcons.TransparentColor = System.Drawing.Color.Transparent;
			this.smallFileIcons.Images.SetKeyName(0, "image.png");
			this.smallFileIcons.Images.SetKeyName(1, "model.png");
			this.smallFileIcons.Images.SetKeyName(2, "unknown.png");
			this.smallFileIcons.Images.SetKeyName(3, "animation.png");
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(348, 298);
			this.Controls.Add(this.fileListView);
			this.Controls.Add(this.mainMenuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.mainMenuStrip;
			this.Name = "MainForm";
			this.Text = "Techno\'s IMG Editor - v{0}";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.mainMenuStrip.ResumeLayout(false);
			this.mainMenuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewHelpToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator helpToolStripMenuSeparator;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator fileToolStripSeparator;
		private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem extractAlllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addFilesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator fileToolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem itemViewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem largeIconToolStripMenuItem;
		private System.Windows.Forms.ListView fileListView;
		private System.Windows.Forms.ToolStripMenuItem smallIconToolStripMenuItem;
		private System.Windows.Forms.ColumnHeader nameColumnHeader;
		private System.Windows.Forms.ColumnHeader sizeColumnHeader;
		private System.Windows.Forms.ImageList smallFileIcons;
		private System.Windows.Forms.ImageList largeFileIcons;
		private System.Windows.Forms.ColumnHeader offsetHeader;
		private System.Windows.Forms.ToolStripMenuItem sortingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem defaultSortingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nameToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem offsetToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem ascendingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem descendingToolStripMenuItem;
	}
}

