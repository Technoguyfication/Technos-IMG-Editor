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
		/// <summary>
		/// Gets the <see cref="FileInfo"/> for the underlying archive file
		/// </summary>
		FileInfo FileInfo { get; }

		/// <summary>
		/// Number of entries in the directory
		/// </summary>
		uint FileCount { get; }

		/// <summary>
		/// Gets the entry with the specified file name
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Thrown when the specified file is invalid</exception>
		IDirectoryEntry GetDirectoryEntry(string fileName);

		/// <summary>
		/// Gets the entry at the specified index
		/// </summary>
		/// <param name="index">Index of the entry</param>
		/// <returns></returns>
		/// <exception cref="InvalidDirectoryEntryException">Thrown when the specified file is invalid</exception>
		IDirectoryEntry GetDirectoryEntry(int index);

		/// <summary>
		/// Enumerates all the directory entries in the file and returns them in a List
		/// </summary>
		/// <returns></returns>
		List<IDirectoryEntry> GetDirectoryEntries();

		/// <summary>
		/// Adds a file to the archive
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="dataStream">A stream containing the file data</param>
		/// <param name="length">How many bytes to read from the dataStream</param>
		/// <param name="offset">The offset to read from</param>
		/// <exception cref="ArgumentException">Thrown when invalid parameters are passed</exception>
		IDirectoryEntry AddFile(string fileName, Stream dataStream, uint length, uint offset);

		/// <summary>
		/// Opens a file from the archive
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Thrown when the file does not exist</exception>
		Stream OpenFile(IDirectoryEntry file);

		/// <summary>
		/// Opens a file from the archive
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Thrown when the file does not exist</exception>
		Stream OpenFile(int index);

		/// <summary>
		/// Opens a file from the archive
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Thrown when the file does not exist</exception>
		Stream OpenFile(string fileName);

		/// <summary>
		/// Deletes a file from the archive
		/// </summary>
		/// <exception cref="ArgumentException">Thrown when the specified file does not exist</exception>
		void DeleteFile(IDirectoryEntry file);

		/// <summary>
		/// Deletes a file from the archive
		/// </summary>
		/// <param name="fileName">Name of the file to delete</param>
		/// <exception cref="ArgumentException">Thrown when the specified file does not exist</exception>
		void DeleteFile(string fileName);

		/// <summary>
		/// Deletes a file from the archive
		/// </summary>
		/// <exception cref="ArgumentException">Thrown when the specified file does not exist</exception>
		void DeleteFile(int index);

		/// <summary>
		/// Defragment or rebuild the archive.
		/// May take a very long time.
		/// </summary>
		/// <param name="progress">Used for tracking the progress of the operation.</param>
		void Defragment(IProgress<ProgressUpdate> progress);

		/// <summary>
		/// "Bump" entries from the beginning of the archive to the end.
		/// Used to create space for more directory entries.
		/// </summary>
		/// <param name="amount"></param>
		/// <exception cref="ArgumentException">Thrown when an invalid amount of directory entries are shifted</exception>
		void Bump(int amount);
	}
}
