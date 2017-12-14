using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Technoguyfication.IMGEditor.Shared.Version2;

namespace Technoguyfication.IMGEditor.Shared
{
	/// <summary>
	/// Represents a Version 2 IMG archive
	/// https://www.gtamodding.com/wiki/IMG_archive#Version_2_-_GTA_SA
	/// </summary>
	public class Ver2IMGArchive : IDisposable, IIMGArchive
	{
		#region Constants

		/// <summary>
		/// Size of an archive sector, in bytes
		/// </summary>
		public const int SECTOR_SIZE = 2048;

		/// <summary>
		/// Size of a directory entry, in bytes
		/// </summary>
		public const int DIRECTORY_ITEM_SIZE = 32;

		/// <summary>
		/// The maximum size of a directory file name, in bytes
		/// </summary>
		public const int MAX_DIRECTORY_FILE_NAME = 23;

		/// <summary>
		/// The header at the beginning 4 bytes of all VER2 archives
		/// </summary>
		public static readonly byte[] HEADER = new byte[] { 0x56, 0x45, 0x52, 0x32 };

		#endregion

		private FileStream _fileStream;
		private string _filePath;

		/// <summary>
		/// Gets the FileInfo for the Archive
		/// </summary>
		public FileInfo FileInfo { get; private set; }

		/// <summary>
		/// Number of entries in the directory
		/// </summary>
		public uint FileCount
		{
			get
			{
				byte[] length = new byte[2];

				// read from the file stream and reset
				// seek position to original location (incase this is used while writing data elsewhere)
				lock (_fileStream)
				{
					long originalPos = _fileStream.Position;

					_fileStream.Seek(4, SeekOrigin.Begin);
					_fileStream.Read(length, 0, 2);
					_fileStream.Seek(originalPos, SeekOrigin.Begin);
				}

				return BitConverter.ToUInt16(length.ReverseIfBigEndian(), 0);
			}
			set
			{
				// get new length in bytes
				byte[] length = new byte[2];
				length = BitConverter.GetBytes(value).ReverseIfBigEndian();

				lock (_fileStream)
				{
					long originalPos = _fileStream.Position;

					// write to file and seek to original location
					_fileStream.Seek(4, SeekOrigin.Begin);
					_fileStream.Write(length, 0, 2);
					_fileStream.Seek(originalPos, SeekOrigin.Begin);
				}
			}
		}

		/// <summary>
		/// Creates a new IMG File instance
		/// </summary>
		/// <param name="filePath"></param>
		public Ver2IMGArchive(string filePath)
		{
			if (!IsValidArchive(filePath))
				throw new InvalidArchiveFormatException();

			SetupStream(filePath);
			FileInfo = new FileInfo(_filePath);
		}

		~Ver2IMGArchive()
		{
			Dispose();
		}

		/// <summary>
		/// Disposes the class and releases all resources
		/// </summary>
		public void Dispose()
		{
			_fileStream?.Dispose();
		}

		/// <summary>
		/// Enumerates all the directory entries in the file and returns them in a List
		/// </summary>
		/// <returns></returns>
		public List<IDirectoryEntry> GetDirectoryEntries()
		{
			lock (_fileStream)
			{
				var entries = new List<IDirectoryEntry>();
				_fileStream.Seek(8, SeekOrigin.Begin);
				uint numEntries = FileCount;

				for (int i = 0; i < numEntries; i++)
				{
					entries.Add(GetDirectoryEntry((ushort)i));
				}

				return entries;
			}
		}

		/// <summary>
		/// Gets the entry at the specified index
		/// </summary>
		/// <param name="index">Index of the entry</param>
		/// <returns></returns>
		/// <exception cref="InvalidDirectoryEntryException"></exception>
		private Ver2DirectoryEntry GetDirectoryEntry(ushort index)
		{
			if (index >= FileCount)
				throw new InvalidDirectoryEntryException();

			byte[] entryBytes = new byte[DIRECTORY_ITEM_SIZE];

			// read from file
			lock (_fileStream)
			{
				_fileStream.Position = 8 + (DIRECTORY_ITEM_SIZE * index);
				_fileStream.Read(entryBytes, 0, DIRECTORY_ITEM_SIZE);
			}

			return Ver2DirectoryEntry.FromBytes(entryBytes);
		}

		/// <summary>
		/// Gets the entry with the specified file name
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public IDirectoryEntry GetDirectoryEntry(string fileName)
		{
			var entry = GetDirectoryEntries().Find((e) => { return e.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase); });

			if (entry == null)
				throw new InvalidDirectoryEntryException("Directory entry does not exist");
			else
				return entry;
		}

		/// <summary>
		/// Appends a file to the archive
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="dataStream">A stream containing the file data</param>
		public void AddFile(string fileName, Stream dataStream, uint length, uint offset = 0)
		{
			if (Encoding.ASCII.GetByteCount(fileName) > MAX_DIRECTORY_FILE_NAME)
				throw new ArgumentException($"Name cannot be longer than {MAX_DIRECTORY_FILE_NAME} bytes");

			if (length + offset > dataStream.Length)
				throw new ArgumentException("Data to read exceeds file size");

			lock (_fileStream)
			{
				// find first sector containing data
				var entries = GetDirectoryEntries();

				// sort by data start ascending
				entries.Sort((a, b) => { return (int)(a.Offset - b.Offset); });

				uint firstSector;

				// if it's an empty archive, start at the second sector
				if (entries.Count > 0)
					firstSector = entries[0].Offset;
				else
					firstSector = 1;    // zero-indexed

				long dataStart = firstSector * SECTOR_SIZE;

				// bump the first file if we don't have enough space to fit the entry
				if (dataStart <= ((FileCount + 1) * DIRECTORY_ITEM_SIZE) + 8)
					Bump(1);

				// find the amount of new sectors we need
				int newSectorCount = (int)(length / SECTOR_SIZE);
				if (length % SECTOR_SIZE != 0)
					newSectorCount++;

				// get the position of the end of the first free sector
				uint endDataFirstSector;
				if (entries.Count > 0)  // if there aren't any entries, the first sector is the beginning of free space
					endDataFirstSector = entries.Last().Offset + entries.Last().Size;
				else
					endDataFirstSector = firstSector;

				// create a buffer to copy one sector of data at a time
				byte[] buffer = new byte[SECTOR_SIZE];

				// seek to offset
				dataStream.Seek(offset, SeekOrigin.Begin);

				// copy each sector of data
				for (int i = 0; i < newSectorCount; i++)
				{
					dataStream.Read(buffer, 0, SECTOR_SIZE);
					_fileStream.Seek((endDataFirstSector + i) * SECTOR_SIZE, SeekOrigin.Begin);
					_fileStream.Write(buffer, 0, SECTOR_SIZE);

					_fileStream.Flush();
				}

				// create a new directory entry
				Ver2DirectoryEntry entry = new Ver2DirectoryEntry()
				{
					Name = fileName,
					StreamingSize = (ushort)newSectorCount,
					Offset = endDataFirstSector
				};

				// write the directory entry
				uint directoryEntryStart = (uint)((FileCount) * DIRECTORY_ITEM_SIZE) + 8;

				_fileStream.Seek(directoryEntryStart, SeekOrigin.Begin);
				_fileStream.Write(entry.GetBytes(), 0, DIRECTORY_ITEM_SIZE);

				FileCount = (ushort)(FileCount + 1);

				// done!
				_fileStream.Flush();
			}
		}

		/// <summary>
		/// Opens a file from the archive
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public Stream OpenFile(IDirectoryEntry file)
		{
			lock (_fileStream)
			{
				if (!GetDirectoryEntries().Contains(file))
					throw new ArgumentException("File is not contained inside the archive.");

				// create output stream
				MemoryStream outStream = new MemoryStream();

				byte[] buffer = new byte[SECTOR_SIZE];
				_fileStream.Seek(file.Offset * SECTOR_SIZE, SeekOrigin.Begin);

				// read from file stream to out stream
				for (int i = 0; i < file.Size; i++)
				{
					_fileStream.Read(buffer, 0, SECTOR_SIZE);
					outStream.Write(buffer, 0, SECTOR_SIZE);
				}

				// flush stream to make sure it's full
				outStream.Flush();

				return outStream;
			}
		}

		/// <summary>
		/// Opens a file from the archive
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public Stream OpenFile(string fileName)
		{
			return OpenFile(GetDirectoryEntry(fileName));
		}

		/// <summary>
		/// Defragment the IMG archive, this may take a very long time
		/// </summary>
		/// <param name="progress">A <see cref="Progress{T}"/> to track defrag progress</param>
		public void Defragment(IProgress<ProgressUpdate> progress = null)
		{
			lock (_fileStream)
			{
				// create another img file from scratch
				string newArchivePath = Path.GetTempFileName();
				var newArchive = Create(newArchivePath);

				// sort file entries by name
				var entries = GetDirectoryEntries();
				entries.Sort((a, b) => string.Compare(a.Name, b.Name));

				// add each entry into the file
				for (int i = 0; i < entries.Count; i++)
				{
					newArchive.AddFile(entries[i].Name, _fileStream, (uint)entries[i].Size * SECTOR_SIZE, entries[i].Offset * SECTOR_SIZE);

					// report progress
					progress?.Report(new ProgressUpdate()
					{
						MaxValue = entries.Count - 1,
						Value = i
					});
				}

				// check integrity
				if (newArchive.FileCount != FileCount)
					throw new Exception("Defragmented file count did not match original file count");

				_fileStream.Dispose();
				newArchive.Dispose();

				// move file to original location
				File.Copy(newArchivePath, _filePath, true);
				File.Delete(newArchivePath);

				// reset stream
				SetupStream(_filePath);
			}
		}


		/// <summary>
		/// Sends directory entries from the beginning of the directory list to the end, moving their data.
		/// Useful for making more space for appending directory entries.
		/// </summary>
		public void Bump(int amount)
		{
			if (FileCount < 1) // what's the point of shifting nothing back?
				return;

			if (FileCount < amount)
				throw new InvalidDirectoryEntryException("Cannot shift more than the amount of existing files.");

			lock (_fileStream)
			{
				for (int i = 0; i < amount; i++)
				{
					uint entryCount = FileCount;

					// get position of first file data
					var firstEntry = GetDirectoryEntry(0);
					byte[] buffer = new byte[SECTOR_SIZE]; // shift one sector at a time

					// get position of last sector
					var lastEntry = GetDirectoryEntry((ushort)(entryCount - 1));
					uint lastSectorEnd = lastEntry.Offset + lastEntry.Size;

					// shift one sector at a time
					for (int j = 0; j < firstEntry.Size; j++)
					{
						// read sector into buffer
						_fileStream.Seek((firstEntry.Offset + j) * SECTOR_SIZE, SeekOrigin.Begin);
						_fileStream.Read(buffer, 0, SECTOR_SIZE);

						// write sector to end of file
						_fileStream.Seek((lastSectorEnd + j) * SECTOR_SIZE, SeekOrigin.Begin);
						_fileStream.Write(buffer, 0, SECTOR_SIZE);

						_fileStream.Flush();

						// overwrite old sector location with 0
						_fileStream.Seek((firstEntry.Offset + j) * SECTOR_SIZE, SeekOrigin.Begin);
						_fileStream.Write(new byte[SECTOR_SIZE], 0, SECTOR_SIZE);

						_fileStream.Flush();
					}

					// change original file offset
					firstEntry.Offset = lastSectorEnd;

					// shift the entire directoy index up and rewrite the file to the end of it
					// yeah we don't need to do this, but it could cause issues with other
					// applications that aren't written as well

					buffer = new byte[DIRECTORY_ITEM_SIZE];

					// start at second entry (the first got shifted)
					for (int j = 1; j < entryCount; j++)
					{
						// read entry into buffer
						_fileStream.Seek((j * DIRECTORY_ITEM_SIZE) + 8, SeekOrigin.Begin);
						_fileStream.Read(buffer, 0, DIRECTORY_ITEM_SIZE);

						// write entry to space before it
						_fileStream.Seek(((j - 1) * DIRECTORY_ITEM_SIZE) + 8, SeekOrigin.Begin);
						_fileStream.Write(buffer, 0, DIRECTORY_ITEM_SIZE);
					}

					// write moved entry to end of directory
					_fileStream.Seek(((entryCount - 1) * DIRECTORY_ITEM_SIZE) + 8, SeekOrigin.Begin);
					_fileStream.Write(firstEntry.GetBytes(), 0, DIRECTORY_ITEM_SIZE);
					_fileStream.Flush();
				}
			}
		}

		/// <summary>
		/// Create a new blank archive
		/// </summary>
		/// <param name="filePath"></param>
		public static Ver2IMGArchive Create(string filePath)
		{
			FileStream _newArchiveStream = File.Create(filePath);
			_newArchiveStream.Write(HEADER, 0, HEADER.Length);

			// write length
			_newArchiveStream.Write(new byte[] { 0x00, 0x00 }, 0, 2);

			// make the file one sector long
			_newArchiveStream.SetLength(SECTOR_SIZE);

			// close stream
			_newArchiveStream.Flush();
			_newArchiveStream.Dispose();

			return new Ver2IMGArchive(filePath);
		}

		/// <summary>
		/// Set up the file stream for this instance
		/// </summary>
		/// <param name="archivePath"></param>
		private void SetupStream(string archivePath)
		{
			_filePath = archivePath;
			_fileStream = new FileStream(archivePath, FileMode.Open, FileAccess.ReadWrite);
		}

		/// <summary>
		/// Checks if an IMG file is valid for this format
		/// </summary>
		/// <param name="filePath"></param>
		public static bool IsValidArchive(string filePath)
		{
			Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

			stream.Seek(0, SeekOrigin.Begin);
			byte[] buffer = new byte[HEADER.Length];

			stream.Read(buffer, 0, buffer.Length);

			for (int i = 0; i < buffer.Length; i++)
			{
				if (buffer[i] != HEADER[i])
					return false;
			}

			stream.Dispose();
			return true;
		}
	}
}
