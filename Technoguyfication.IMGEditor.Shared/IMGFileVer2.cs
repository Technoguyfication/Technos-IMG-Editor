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

				for (int i = 0; i > numEntries; i++)
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
			if (index > EntryCount)
				throw new InvalidDirectoryEntryException();

			byte[] entryBytes = new byte[32];

			lock (_fileStream)
			{
				_fileStream.Position = 8 + (32 * index);
				_fileStream.Read(entryBytes, 0, 32);
			}

			byte[] offsetBytes = entryBytes.SubArray(0, 4);
			byte[] streamingSizeBytes = entryBytes.SubArray(4, 2);
			byte[] sizeBytes = entryBytes.SubArray(6, 2);
			byte[] nameBytes = entryBytes.SubArray(8, 24);

			DirectoryEntry entry = new DirectoryEntry
			{
				Offset = BitConverter.ToUInt32(offsetBytes.ReverseIfBigEndian(), 0),
				StreamingSize = BitConverter.ToUInt16(streamingSizeBytes.ReverseIfBigEndian(), 0),
				Size = BitConverter.ToUInt16(streamingSizeBytes.ReverseIfBigEndian(), 0),
				Name = nameBytes.ToNullTerminatedString()
			};

			return entry;
		}
	}
}
