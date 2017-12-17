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
	public partial class SavePromptForm : Form
	{
		public SavePromptForm()
		{
			InitializeComponent();
		}

		public void ShowDialog(string fileName)
		{
			//if (fileName.Length > 

			// write filename to label
			savePromptLabel.Text = string.Format(savePromptLabel.Text, fileName);
		}
	}
}
