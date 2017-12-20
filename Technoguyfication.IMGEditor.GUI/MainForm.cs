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
		private string _currentOpenedFilePath = null;
		private List<ListViewDirectoryEntry> _entries = new List<ListViewDirectoryEntry>();

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
			if (_entries == null)
				PopulateEntries();

			e.Item = _entries[e.ItemIndex];
		}

		/// <summary>
		/// Populates <see cref="_entries"/> with file entries from the archive
		/// </summary>
		private void PopulateEntries()
		{
			using (IIMGArchive archive = GetArchive(_currentOpenedFilePath))
			{
				if (archive == null)
					return;

				// populate entries list
				_entries.Clear();
				archive.GetDirectoryEntries().ForEach((entry) =>
				{
					_entries.Add(new ListViewDirectoryEntry(entry));
				});

				// set up listview
				fileListView.VirtualListSize = _entries.Count;
				fileListView.Refresh();
			}
		}

		/// <summary>
		/// Prompts the user to open an archive
		/// </summary>
		private void PromptOpenArchive()
		{
			OpenFileDialog dialog = new OpenFileDialog()
			{
				InitialDirectory = Properties.Settings.Default.lastDirectory,
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
		/// Opens an archive in the program, closing any existing ones
		/// </summary>
		private void OpenArchive(string filePath)
		{
			_currentOpenedFilePath = filePath;
			PopulateEntries();
		}

		/// <summary>
		/// Returns an <see cref="IIMGArchive"/> for use
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns>An archive, or null if it failed</returns>
		private IIMGArchive GetArchive(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
				return null;

			try
			{
				return IMGOpener.GetArchive(filePath);
			}
			catch (FileNotFoundException)			// file not found
			{
				MessageBox.Show($"The file \"{filePath}\" could not be found.", "IMG Editor", MessageBoxButtons.OK);
				return null;
			}
			catch (InvalidArchiveFormatException)	// invalid archive
			{
				MessageBox.Show("Unrecognized IMG file format. It may be corrupted.", "IMG Editor", MessageBoxButtons.OK);
				return null;
			}
			catch (IOException ex)					// probably in use by something else
			{
				if (MessageBox.Show($"Error opening\"{filePath}\": {ex.Message}", "IMG Editor", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
					return GetArchive(filePath);

				return null;
			}
			catch (Exception ex)					// idfk
			{
				MessageBox.Show($"Unhandled error opening file:\n\n{ex}");
				return null;
			}
		}

		/// <summary>
		/// Sets the view mode of the item list
		/// </summary>
		/// <param name="view"></param>
		private void SetListViewMode(View view)
		{
			Properties.Settings.Default.viewMode = view;
			fileListView.View = view;
		}

		/// <summary>
		/// Sets the sorting for items
		/// </summary>
		/// <param name="method"></param>
		/// <param name="order"></param>
		private void SetListViewSorting(SortMethod? method = null, SortOrder? order = null)
		{
			if (method != null)
			{

			}
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
