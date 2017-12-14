using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.IMGEditor.Shared.Version2;

namespace Technoguyfication.IMGEditor.Shared
{
	public interface IIMGArchive
	{
		FileInfo FileInfo { get; }
		uint FileCount { get; }

		void AddFile(string fileName, Stream dataStream, uint length, uint offset = 0);
		Stream OpenFile(string fileName);
		void Defragment(IProgress<ProgressUpdate> progress);
		void Bump(int amount);

		IDirectoryEntry GetDirectoryEntry(string fileName);
		List<IDirectoryEntry> GetDirectoryEntries();
	}
}
