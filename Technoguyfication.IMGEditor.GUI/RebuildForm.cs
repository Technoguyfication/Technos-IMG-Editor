using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Technoguyfication.IMGEditor.GUI
{
	public partial class RebuildForm : Form
	{
		public delegate void CancelledEventHandler();
		public event CancelledEventHandler Cancelled;

		public RebuildForm()
		{
			InitializeComponent();
		}

		public void UpdateProgress(ProgressUpdate progress)
		{
			if (InvokeRequired)
			{
				Invoke(new Action(() => { UpdateProgress(progress); }));
				return;
			}

			rebuildingProgressLabel.Text = $"Rebuilt {progress.Value} of {progress.MaxValue} files in archive.";
			rebuildProgressBar.Maximum = progress.MaxValue;
			rebuildProgressBar.Value = progress.Value;
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Cancelled.Invoke();
			Close();
		}
	}
}
