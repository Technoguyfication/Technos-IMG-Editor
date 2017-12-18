using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Technoguyfication.IMGEditor.GUI
{
	public partial class MainForm : Form
	{
		IIMGArchive currentArchive = null;

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// format title
			Text = string.Format(Text, Application.ProductVersion);

			// set view mode
			fileListView.View = Properties.Settings.Default.viewMode;
		}

		private void FileListView_RetreiveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			e.Item = new ListViewDirectoryEntry(currentArchive.GetDirectoryEntry(e.ItemIndex));
		}

		/// <summary>
		/// Prompts the user to open an archive
		/// </summary>
		private void PromptOpenArchive()
		{
			OpenFileDialog dialog = new OpenFileDialog()
			{
				InitialDirectory = (string)Properties.Settings.Default.lastDirectory,
				CheckFileExists = true,
				CheckPathExists = true
			};

			DialogResult result = dialog.ShowDialog();
			if (result != DialogResult.OK)
				return;

			Properties.Settings.Default.lastDirectory = Path.GetDirectoryName(dialog.FileName);

			OpenArchive(dialog.FileName);
		}

		/// <summary>
		/// Opens an archive
		/// </summary>
		private void OpenArchive(string filePath)
		{
			try
			{
				currentArchive = IMGOpener.GetArchive(filePath);
			}
			catch (FileNotFoundException)
			{
				MessageBox.Show("File not found.", "IMG Editor", MessageBoxButtons.OK);
				return;
			}
			catch (InvalidArchiveFormatException)
			{
				MessageBox.Show("Unrecognized IMG file format. It may be corrupted.", "IMG Editor", MessageBoxButtons.OK);
				return;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Unhandled error opening file. Please send the following to the app developer:\n\n{ex}");
				return;
			}

			fileListView.VirtualListSize = (int)currentArchive.FileCount;
			fileListView.Refresh();
		}

		private void SetListViewMode(View view)
		{
			Properties.Settings.Default.viewMode = view;
			fileListView.View = view;
		}

		private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PromptOpenArchive();
		}

		private void DetailsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetListViewMode(View.Details);
		}

		private void LargeIconToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetListViewMode(View.LargeIcon);
		}

		private void SmallIconToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetListViewMode(View.SmallIcon);
		}
	}
}
