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
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// format title
			Text = string.Format(Text, Application.ProductVersion);
		}

		private void FileListView_RetreiveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Opens an archive, prompting the user to save if required
		/// </summary>
		private void OpenArchive()
		{

		}


	}
}
