using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technoguyfication.IMGEditor;

namespace Technoguyfication.IMGEditor.GUI
{
	class ListViewDirectoryEntry : ListViewItem
	{
		public ListViewDirectoryEntry(IDirectoryEntry entry) : base()
		{
			Text = entry.Name;

			// set icon
			string exension = Path.GetExtension(entry.Name);
			switch (exension)
			{
				case ".txd":
					ImageIndex = 0;
					break;
				case ".dff":
					ImageIndex = 1;
					break;
				default:
					ImageIndex = 2;
					break;
			}

			SubItems.Add(new ListViewSubItem(this, entry.Size.ToString()));	// size
		}
	}
}
