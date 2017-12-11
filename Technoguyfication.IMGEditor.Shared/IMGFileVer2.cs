using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Technoguyfication.IMGEditor.Shared.FileStructure;

namespace Technoguyfication.IMGEditor.Shared
{
	/// <summary>
	/// Represents a Version 2 IMG archive
	/// https://www.gtamodding.com/wiki/IMG_archive#Version_2_-_GTA_SA
	/// </summary>
	public class IMGFileVer2
	{
		private FileStream _fileStream;
		private string _filePath;

		/// <summary>
		/// Gets the FileInfo for the IMG Archive
		/// </summary>
		public FileInfo FileInfo { get; private set; }

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
		/// Number of entries in the directory
		/// </summary>
		public ushort FileCount
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
		public IMGFileVer2(string filePath)
		{
			_filePath = filePath;
			_fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
			FileInfo = new FileInfo(_filePath);
		}

		/// <summary>
		/// Enumerates all the directory entries in the file and returns them in a List
		/// </summary>
		/// <returns></returns>
		public List<DirectoryEntry> GetDirectoryEntries()
		{
			var entries = new List<DirectoryEntry>();

			lock (_fileStream)
			{
				_fileStream.Seek(8, SeekOrigin.Begin);
				int numEntries = FileCount;

				for (int i = 0; i < numEntries; i++)
				{
					entries.Add(GetDirectoryEntry((ushort)i));
				}
			}

			return entries;
		}

		/// <summary>
		/// Gets the entry at the specified index
		/// </summary>
		/// <param name="index">Index of the entry</param>
		/// <returns></returns>
		/// <exception cref="InvalidDirectoryEntryException"></exception>
		public DirectoryEntry GetDirectoryEntry(ushort index)
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

			return DirectoryEntry.FromBytes(entryBytes);
		}

		/// <summary>
		/// Appends a file to the archive
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fileContents"></param>
		public void AddFile(string fileName, byte[] fileContents)
		{
			if (Encoding.ASCII.GetByteCount(fileName) > MAX_DIRECTORY_FILE_NAME)
				throw new ArgumentException($"Name cannot be longer than {MAX_DIRECTORY_FILE_NAME} bytes");

			lock (_fileStream)
			{
				// find first sector containing data
				var entries = GetDirectoryEntries();

				// sort by data start ascending
				entries.Sort((a, b) => { return (int)(a.Offset - b.Offset); });

				uint firstSector = entries[0].Offset;
				long dataStart = firstSector * SECTOR_SIZE;

				// bump the first file if we don't have enough space to fit the entry
				if (dataStart <= ((FileCount + 1) * DIRECTORY_ITEM_SIZE) + 8)
					Bump(1);

				// find the amount of new sectors we need
				int newSectorCount = fileContents.Length / SECTOR_SIZE;
				if (fileContents.Length % SECTOR_SIZE != 0)
					newSectorCount++;

				// TODO: do this in steps of one sector each to decrease memory usage

				// create a new buffer for data which contains the full sector(s) size and copy our data into it
				byte[] buffer = new byte[newSectorCount * SECTOR_SIZE];
				Array.Copy(fileContents, buffer, fileContents.Length);

				// get the position of the end of the first free sector
				uint endDataFirstSector = entries.Last().Offset + entries.Last().GetSize();
				long endDataStart = endDataFirstSector * SECTOR_SIZE;

				// write the data buffer into the file
				_fileStream.Seek(endDataStart, SeekOrigin.Begin);
				_fileStream.Write(buffer, 0, buffer.Length);
				_fileStream.Flush();

				// create a new directory entry
				DirectoryEntry entry = new DirectoryEntry()
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
		/// Gets a file from the archive
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public byte[] GetFile(DirectoryEntry file)
		{
			lock (_fileStream)
			{
				if (!GetDirectoryEntries().Contains(file))
					throw new ArgumentException("File is not contained inside the archive.");

				byte[] buffer = new byte[file.GetSize() * SECTOR_SIZE];

				_fileStream.Seek(file.Offset * SECTOR_SIZE, SeekOrigin.Begin);

				int bytesRead = 0;
				while (bytesRead < buffer.Length)
				{
					bytesRead += _fileStream.Read(buffer, bytesRead, buffer.Length - bytesRead);
				}

				return buffer;
			}
		}

		/// <summary>
		/// Sends directory entries from the beginning of the directory list to the end, moving their data. Useful for making more space for appending directory entries.
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
					ushort entryCount = FileCount;

					// get position of first file data
					var firstEntry = GetDirectoryEntry(0);
					byte[] buffer = new byte[SECTOR_SIZE]; // shift one sector at a time

					// get position of last sector
					var lastEntry = GetDirectoryEntry((ushort)(entryCount - 1));
					uint lastSectorEnd = lastEntry.Offset + lastEntry.GetSize();

					// shift one sector at a time
					for (int j = 0; j < firstEntry.GetSize(); j++)
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
	}
}
