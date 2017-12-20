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

			// add size
			string size;

			if (entry.GetType() == typeof(Version2.Ver2DirectoryEntry))
				size = $"{entry.Size} KiB";
			else
				size = entry.Size.ToString();
			SubItems.Add(new ListViewSubItem(this, size));

			// add ofset
			SubItems.Add(new ListViewSubItem(this, $"0x{entry.Offset.ToString("X8")}"));
		}
	}
}
