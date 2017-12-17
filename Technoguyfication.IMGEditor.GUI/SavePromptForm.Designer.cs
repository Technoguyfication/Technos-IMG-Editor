namespace Technoguyfication.IMGEditor.GUI
{
	partial class SavePromptForm
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
			this.savePromptLabel = new System.Windows.Forms.Label();
			this.bottomPanel = new System.Windows.Forms.Panel();
			this.cancelButton = new System.Windows.Forms.Button();
			this.dontSaveButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.bottomPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// savePromptLabel
			// 
			this.savePromptLabel.AutoSize = true;
			this.savePromptLabel.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.savePromptLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(82)))), ((int)(((byte)(155)))));
			this.savePromptLabel.Location = new System.Drawing.Point(12, 18);
			this.savePromptLabel.Name = "savePromptLabel";
			this.savePromptLabel.Size = new System.Drawing.Size(249, 20);
			this.savePromptLabel.TabIndex = 0;
			this.savePromptLabel.Text = "Do you want to save changes to {0}?";
			// 
			// bottomPanel
			// 
			this.bottomPanel.BackColor = System.Drawing.SystemColors.Control;
			this.bottomPanel.Controls.Add(this.saveButton);
			this.bottomPanel.Controls.Add(this.dontSaveButton);
			this.bottomPanel.Controls.Add(this.cancelButton);
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Location = new System.Drawing.Point(0, 69);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size(402, 42);
			this.bottomPanel.TabIndex = 1;
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(315, 9);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// dontSaveButton
			// 
			this.dontSaveButton.Location = new System.Drawing.Point(212, 9);
			this.dontSaveButton.Name = "dontSaveButton";
			this.dontSaveButton.Size = new System.Drawing.Size(97, 23);
			this.dontSaveButton.TabIndex = 2;
			this.dontSaveButton.Text = "Do&n\'t Save";
			this.dontSaveButton.UseVisualStyleBackColor = true;
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(149, 9);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(57, 23);
			this.saveButton.TabIndex = 2;
			this.saveButton.Text = "&Save";
			this.saveButton.UseVisualStyleBackColor = true;
			// 
			// SavePromptForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(402, 111);
			this.Controls.Add(this.bottomPanel);
			this.Controls.Add(this.savePromptLabel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SavePromptForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Techno\'s IMG Editor";
			this.bottomPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label savePromptLabel;
		private System.Windows.Forms.Panel bottomPanel;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button dontSaveButton;
		private System.Windows.Forms.Button cancelButton;
	}
}