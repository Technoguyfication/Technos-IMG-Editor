using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.IMGEditor.Shared.FileStructure;

namespace Technoguyfication.IMGEditor.Shared
{
	interface IIMGArchive
	{
		void AddFile(string fileName, Stream dataStream, long length, long offset);
		Stream OpenFile(string fileName);
		Stream OpenFile(DirectoryEntry file);

		DirectoryEntry GetDirectoryEntry(ushort index);
		DirectoryEntry GetDirectoryEntry(string fileName);
		List<DirectoryEntry> GetDirectoryEntries();

		void Defragment(IProgress<ProgressUpdate> progress);
	}
}
