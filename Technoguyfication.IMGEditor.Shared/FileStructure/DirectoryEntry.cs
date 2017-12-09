using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.IMGEditor.Shared.FileStructure
{
	public struct DirectoryEntry
	{
		public uint Offset { get; set; }
		public ushort StreamingSize { get; set; }
		public ushort Size { get; set; }
		public string Name { get; set; }
	}
}
