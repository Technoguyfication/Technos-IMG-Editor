using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.IMGEditor
{
	/// <summary>
	/// Represents a file directory entry in an archive
	/// </summary>
	public interface IDirectoryEntry
	{
		string Name { get; set; }

		uint Offset { get; set; }
		uint Size { get; set; }

		byte[] GetBytes();
	}
}
