using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Technoguyfication.IMGEditor.Shared.FileStructure;

namespace Technoguyfication.IMGEditor.Shared
{
	public class IMGFileVer2
	{
		private FileStream _fileStream;

		/// <summary>
		/// Size of an archive sector, in bytes
		/// </summary>
		public const int SECTOR_SIZE = 2048;

		/// <summary>
		/// Size of a directory entry, in bytes
		/// </summary>
		public const int DIR_ENTRY_SIZE = 32;

		/// <summary>
		/// Number of entries in the directory
		/// </summary>
		public ushort EntryCount
		{
			get
			{
				byte[] len = new byte[2];

				// read from the file stream and reset
				// seek position to original location (incase this is used while writing data elsewhere)
				lock (_fileStream)
				{
					long originalPos = _fileStream.Position;

					_fileStream.Seek(4, SeekOrigin.Begin);
					_fileStream.Read(len, 0, 2);
					_fileStream.Seek(originalPos, SeekOrigin.Begin);
				}

				return BitConverter.ToUInt16(len.ReverseIfBigEndian(), 0);
			}
			set
			{
				byte[] len = new byte[2];
				len = BitConverter.GetBytes(value).ReverseIfBigEndian();

				lock (_fileStream)
				{
					long originalPos = _fileStream.Position;

					_fileStream.Seek(0, SeekOrigin.Begin);
					_fileStream.Write(len, 0, 2);
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
			_fileStream = new FileStream(filePath, FileMode.Open);
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
				int numEntries = EntryCount;

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
			if (index >= EntryCount)
				throw new InvalidDirectoryEntryException();

			byte[] entryBytes = new byte[DIR_ENTRY_SIZE];

			// read from file
			lock (_fileStream)
			{
				_fileStream.Position = 8 + (DIR_ENTRY_SIZE * index);
				_fileStream.Read(entryBytes, 0, DIR_ENTRY_SIZE);
			}

			// convert bytes to actual values
			byte[] offsetBytes = entryBytes.SubArray(0, 4);
			byte[] streamingSizeBytes = entryBytes.SubArray(4, 2);
			byte[] sizeBytes = entryBytes.SubArray(6, 2);
			byte[] nameBytes = entryBytes.SubArray(8, 24);

			DirectoryEntry entry = new DirectoryEntry
			{
				Offset = BitConverter.ToUInt32(offsetBytes.ReverseIfBigEndian(), 0),
				StreamingSize = BitConverter.ToUInt16(streamingSizeBytes.ReverseIfBigEndian(), 0),
				Size = BitConverter.ToUInt16(sizeBytes.ReverseIfBigEndian(), 0),
				Name = nameBytes.ToNullTerminatedString()
			};

			return entry;
		}

		/// <summary>
		/// Appends a file to the archive
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fileContents"></param>
		public void AddFile(string fileName, byte[] fileContents)
		{
			lock (_fileStream)
			{
				// find first sector containing data
				var entries = GetDirectoryEntries();
				entries.Sort((a, b) => { return (int)(a.Offset - b.Offset); });

				uint firstSector = entries[0].Offset;
				ulong dataStart = firstSector * SECTOR_SIZE;

				if (dataStart <= (ulong)((EntryCount + 1) * DIR_ENTRY_SIZE) + 8)
				{
					// need to move the first sector to the back
				}
			}
		}

		/// <summary>
		/// Sends x amount of file entries to the back of the archive, for expanding the directory
		/// </summary>
		public void SendFilesToBack(int amount)
		{
			if (EntryCount < 1)	// what's the point of shifting nothing back?
				return;

			if (EntryCount < amount)
				throw new InvalidDirectoryEntryException("Cannot shift more than the amount of existing files.");

			lock (_fileStream)
			{
				for (int i = 0; i < amount; i++)
				{
					ushort entryCount = EntryCount;

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
						_fileStream.Seek(firstEntry.Offset * SECTOR_SIZE, SeekOrigin.Begin);
						_fileStream.Read(buffer, 0, SECTOR_SIZE);

						// write sector to end of file
						_fileStream.Seek((lastSectorEnd) * SECTOR_SIZE, SeekOrigin.Begin);
						_fileStream.Write(buffer, 0, SECTOR_SIZE);

						_fileStream.Flush();

						// overwrite old sector location with 0
						_fileStream.Seek(firstEntry.Offset * SECTOR_SIZE, SeekOrigin.Begin);
						_fileStream.Write(new byte[SECTOR_SIZE], 0, SECTOR_SIZE);

						_fileStream.Flush();
					}

					// change original file offset
					firstEntry.Offset = lastSectorEnd / SECTOR_SIZE;

					// shift the entire directoy index up and rewrite the file to the end of it
					// yeah we don't need to do this, but it could cause issues with other
					// applications that aren't written as well

					buffer = new byte[DIR_ENTRY_SIZE];

					// start at second entry (the first got shifted)
					for (int j = 1; j < entryCount; j++)
					{
						// read entry into buffer
						_fileStream.Seek((j * DIR_ENTRY_SIZE) + 8, SeekOrigin.Begin);
						_fileStream.Read(buffer, 0, DIR_ENTRY_SIZE);

						// write entry to space before it
						_fileStream.Seek(((j - 1) * DIR_ENTRY_SIZE) + 8, SeekOrigin.Begin);
						_fileStream.Write(buffer, 0, DIR_ENTRY_SIZE);
					}

					// write moved entry to end of directory
					_fileStream.Seek(((entryCount - 1) * DIR_ENTRY_SIZE) + 8, SeekOrigin.Begin);
					_fileStream.Write(firstEntry.GetBytes(), 0, DIR_ENTRY_SIZE);
					_fileStream.Flush();
				}
			}
		}
	}
}
