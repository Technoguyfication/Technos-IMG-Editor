using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.IMGEditor.Version2;

namespace Technoguyfication.IMGEditor
{
	public interface IIMGArchive : IDisposable
	{
		FileInfo FileInfo { get; }
		uint FileCount { get; }

		void AddFile(string fileName, Stream dataStream, uint length, uint offset);
		Stream OpenFile(string fileName);
		Stream OpenFile(int index);
		void DeleteFile(string fileName);
		void DeleteFile(int index);
		void Defragment(IProgress<ProgressUpdate> progress);
		void Bump(int amount);

		IDirectoryEntry GetDirectoryEntry(string fileName);
		IDirectoryEntry GetDirectoryEntry(int index);
		List<IDirectoryEntry> GetDirectoryEntries();
	}
}
