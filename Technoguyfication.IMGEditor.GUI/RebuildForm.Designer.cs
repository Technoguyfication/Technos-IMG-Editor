namespace Technoguyfication.IMGEditor.GUI
{
	partial class RebuildForm
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
			this.rebuildProgressBar = new System.Windows.Forms.ProgressBar();
			this.rebuildingInfoLabel = new System.Windows.Forms.Label();
			this.rebuildingProgressLabel = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// rebuildProgressBar
			// 
			this.rebuildProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rebuildProgressBar.Location = new System.Drawing.Point(12, 70);
			this.rebuildProgressBar.Name = "rebuildProgressBar";
			this.rebuildProgressBar.Size = new System.Drawing.Size(307, 23);
			this.rebuildProgressBar.TabIndex = 0;
			// 
			// rebuildingInfoLabel
			// 
			this.rebuildingInfoLabel.AutoSize = true;
			this.rebuildingInfoLabel.Location = new System.Drawing.Point(9, 9);
			this.rebuildingInfoLabel.Name = "rebuildingInfoLabel";
			this.rebuildingInfoLabel.Size = new System.Drawing.Size(174, 26);
			this.rebuildingInfoLabel.TabIndex = 1;
			this.rebuildingInfoLabel.Text = "Rebuilding archive...\r\nThis process cannot be interrupted.";
			// 
			// rebuildingProgressLabel
			// 
			this.rebuildingProgressLabel.AutoSize = true;
			this.rebuildingProgressLabel.Location = new System.Drawing.Point(9, 44);
			this.rebuildingProgressLabel.Name = "rebuildingProgressLabel";
			this.rebuildingProgressLabel.Size = new System.Drawing.Size(128, 13);
			this.rebuildingProgressLabel.TabIndex = 1;
			this.rebuildingProgressLabel.Text = "Rebuilding files: %s of %s.";
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(244, 39);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// RebuildForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(331, 105);
			this.ControlBox = false;
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.rebuildingProgressLabel);
			this.Controls.Add(this.rebuildingInfoLabel);
			this.Controls.Add(this.rebuildProgressBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RebuildForm";
			this.ShowInTaskbar = false;
			this.Text = "Rebuilding...";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar rebuildProgressBar;
		private System.Windows.Forms.Label rebuildingInfoLabel;
		private System.Windows.Forms.Label rebuildingProgressLabel;
		private System.Windows.Forms.Button cancelButton;
	}
}