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
		private string _openedArchivePath = null;
		private List<ListViewDirectoryEntry> _entries = new List<ListViewDirectoryEntry>();
		private const string FileFilter = "IMG Archives (*.img, *.dir)|*.img|All files (*.*)|*.*";

		/// <summary>
		/// Tells whether or not the application has an archive opened.
		/// </summary>
		public bool IsArchiveOpened
		{
			get
			{
				return _openedArchivePath != null;
			}
		}

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// format title
			Text = string.Format(Text, Application.ProductVersion);

			// set icon size
			fileListView.View = Properties.Settings.Default.fileListViewMode;

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
			// clear view if nothing is opened
			if (!IsArchiveOpened)
			{
				_entries.Clear();
				fileListView.VirtualListSize = 0;
				fileListView.Refresh();
			}

			using (IIMGArchive archive = GetArchive(_openedArchivePath))
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
			// create dialog
			using (OpenFileDialog dialog = new OpenFileDialog()
			{
				InitialDirectory = Properties.Settings.Default.openFileLastDirectory,
				CheckFileExists = true,
				CheckPathExists = true,
				Filter = FileFilter,
				FilterIndex = Properties.Settings.Default.openFileFilterIndex
			})
			{

				DialogResult result = dialog.ShowDialog();
				if (result != DialogResult.OK)
					return;

				// save last used settings
				Properties.Settings.Default.openFileLastDirectory = Path.GetDirectoryName(dialog.FileName);
				Properties.Settings.Default.openFileFilterIndex = dialog.FilterIndex;
				Properties.Settings.Default.Save();

				OpenArchive(dialog.FileName);
			}
		}

		/// <summary>
		/// Opens an archive
		/// </summary>
		private void OpenArchive(string filePath)
		{
			_openedArchivePath = filePath;
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
				return IMGUtility.GetArchive(filePath);
			}
			// file not found
			catch (FileNotFoundException)
			{
				MessageBox.Show($"The file \"{filePath}\" could not be found.", Program.PROGRAM_TITLE, MessageBoxButtons.OK);
				return null;
			}
			// invalid archive
			catch (InvalidArchiveException ex)
			{
				MessageBox.Show(ex.Message, Program.PROGRAM_TITLE, MessageBoxButtons.OK);
				return null;
			}
			// probably in use by something else
			catch (IOException ex)
			{
				if (MessageBox.Show(ex.Message, Program.PROGRAM_TITLE, MessageBoxButtons.RetryCancel) == DialogResult.Retry)
					return GetArchive(filePath);

				return null;
			}
			catch (Exception ex)
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
			Properties.Settings.Default.fileListViewMode = view;
			Properties.Settings.Default.Save();

			fileListView.View = view;
		}

		/// <summary>
		/// Sets the sorting for items
		/// </summary>
		/// <param name="method"></param>
		/// <param name="order"></param>
		private void SetListViewSorting(SortMethod? method = null, SortOrder? order = null)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Prompts the user to extract the files that are currently selected, or all of them
		/// </summary>
		private void ExtractFiles(bool extractAll)
		{
			if (!IsArchiveOpened)
				return;

			// are we extracting a single file?
			if (!extractAll && fileListView.SelectedIndices.Count == 1)
			{
				// get file entry
				var entry = _entries[fileListView.SelectedIndices[0]].Entry;

				// show save dialog
				using (var dialog = new SaveFileDialog()
				{
					Filter = "All files (*.*)|*.*",
					CheckPathExists = true,
					OverwritePrompt = true,
					InitialDirectory = Properties.Settings.Default.extractFileLastDirectory,
					FileName = entry.Name
				})
				{
					DialogResult result = dialog.ShowDialog();

					if (result != DialogResult.OK)
						return;

					// extract the file
					using (var archive = GetArchive(_openedArchivePath))
					{
						IMGUtility.ExtractFile(archive, Path.GetFileName(dialog.FileName), Path.GetDirectoryName(dialog.FileName), true);
					}

					// save the last directory
					Properties.Settings.Default.extractFileLastDirectory = Path.GetDirectoryName(dialog.FileName);
					Properties.Settings.Default.Save();
				}
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

		private void FileListView_DragDrop(object sender, DragEventArgs e)
		{
			// handle file dropping
			if (e.Data.GetFormats(false).Contains(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

				// if an archive is opened, copy the file(s) into the archive
				if (IsArchiveOpened)
				{
					// open the archive and start adding files
					using (IIMGArchive archive = GetArchive(_openedArchivePath))
					{
						// get list of names
						List<string> fileNames = new List<string>();
						var entries = archive.GetDirectoryEntries();
						foreach (var entry in entries)
							fileNames.Add(entry.Name);

						foreach (string filePath in files)
						{
							string fileName = Path.GetFileName(filePath);

							// check if file exists
							if (fileNames.Contains(fileName))
							{
								DialogResult result = MessageBox.Show($"Overwite file \"{fileName}\"?", Program.PROGRAM_TITLE, MessageBoxButtons.YesNoCancel, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
								if (result == DialogResult.Cancel)
									return;
								else if (result == DialogResult.No)
									continue;
							}

							// add the file
							IMGUtility.AddFile(archive, filePath, true);
						}
					}

					PopulateEntries();
				}
				// otherwise, check if we're dropping an archive file onto the program and open it
				else if (IMGUtility.IsValidArchive(files[0]))
				{
					OpenArchive(files[0]);
				}
			}
		}

		private void FileListView_DragEnter(object sender, DragEventArgs e)
		{
			// check if the data is a file
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				// if an archive is opened, drop the file in
				if (IsArchiveOpened)
				{
					e.Effect = DragDropEffects.Copy;
				}
				// otherwise, checl if it's an archive we can open
				else
				{
					string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);

					// if we're just dragging on archive file on top, allow drop
					if (fileNames.Length == 1 && IMGUtility.IsValidArchive(fileNames[0]))
						e.Effect = DragDropEffects.Copy;
				}
			}
		}

		private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PopulateEntries();
		}

		private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenArchive(null);
		}

		private void FileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			// close button
			closeToolStripMenuItem.Enabled = IsArchiveOpened;

			// extract and add buttons
			extractToolStripMenuItem.Enabled = (IsArchiveOpened && fileListView.SelectedIndices.Count > 0); // items must be selected for this one

			extractAlllToolStripMenuItem.Enabled =
				addFilesToolStripMenuItem.Enabled = IsArchiveOpened;
		}

		private void ToolToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			// archive info
			archiveInfoToolStripMenuItem.Enabled = IsArchiveOpened;
		}

		private void ExtractToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ExtractFiles(false);
		}

		private void ExtractAlllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ExtractFiles(true);
		}

		private void FileListView_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
		{
			int result = _entries.GetRange(e.StartIndex, (_entries.Count - e.StartIndex)).FindIndex((entry) =>
			{
				if (e.IsPrefixSearch)
					return entry.Name.StartsWith(e.Text);
				else return entry.Name.Equals(e.Text, StringComparison.OrdinalIgnoreCase);
			});

			if (result > -1)
				e.Index = result;
		}

		private void RebuildArchiveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!IsArchiveOpened)
				return;

			var result = MessageBox.Show("Rebuilding (or defragmenting) an archive can take a very long time. Once the process is started, it cannot be stopped.\n\n" +
				"Would you like to continue?", Program.PROGRAM_TITLE, MessageBoxButtons.YesNo);

			if (result != DialogResult.Yes)
				return;

			var rebuildForm = new RebuildForm();

			Task rebuildTask = new Task(() =>
			{
				var archive = GetArchive(_openedArchivePath);

				// defragment archive
				archive.Defragment(new Progress<ProgressUpdate>(progress =>
				{
				// update progress window
				rebuildForm.UpdateProgress(progress);
				}));

				// close modal dialog
				rebuildForm.Invoke(new Action(() => { rebuildForm.Close(); }));
			});

			rebuildTask.Start();
			rebuildForm.ShowDialog();
		}
	}
}
