using System;
using System.Collections.Generic;
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

			SubItems.Add(new ListViewSubItem(this, entry.Size.ToString()));	// size
		}
	}
}
