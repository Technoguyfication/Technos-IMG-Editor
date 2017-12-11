using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.IMGEditor.Shared.FileStructure
{
	/// <summary>
	/// Represents a file directory entry
	/// https://www.gtamodding.com/wiki/IMG_archive#Directory_Entry_2
	/// </summary>
	public class DirectoryEntry
	{
		/// <summary>
		/// The offset of the data (in sectors)
		/// </summary>
		public uint Offset { get; set; }

		/// <summary>
		/// Streaming Size (in sectors)
		/// </summary>
		public ushort StreamingSize { get; set; }

		/// <summary>
		/// Size of the file (in sectors)
		/// </summary>
		public ushort Size { get; set; }

		/// <summary>
		/// Name of the file the data represents, cannot be longer than 23 bytes
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				// name takes at max. 24 bytes, with last one reserved for null terminator
				if (Encoding.ASCII.GetByteCount(value) > IMGFileVer2.MAX_DIRECTORY_FILE_NAME)
					throw new Exception($"Name cannot be longer than {IMGFileVer2.MAX_DIRECTORY_FILE_NAME} bytes.");

				_name = value;
			}
		}
		private string _name;

		/// <summary>
		/// Serialize the directory entry to an array of bytes
		/// </summary>
		/// <returns></returns>
		public byte[] GetBytes()
		{
			List<byte> builder = new List<byte>();

			// serialize each value
			builder.AddRange(BitConverter.GetBytes(Offset).ReverseIfBigEndian());
			builder.AddRange(BitConverter.GetBytes(StreamingSize).ReverseIfBigEndian());
			builder.AddRange(BitConverter.GetBytes(Size).ReverseIfBigEndian());
			builder.AddRange(Encoding.ASCII.GetBytes(Name));

			// fill the rest of the string with null bytes
			builder.AddRange(new byte[IMGFileVer2.DIRECTORY_ITEM_SIZE - builder.Count]);

			return builder.ToArray();
		}

		/// <summary>
		/// Get the size of the directory entry, in sectors
		/// </summary>
		/// <returns></returns>
		public ushort GetSize()
		{
			if (StreamingSize == 0)
				return Size;
			else
				return StreamingSize;
		}

		public override string ToString()
		{
			return $"Offset: {Offset} Sectors ({Offset * 2048} Bytes)\n" +
				$"Streaming Size: {StreamingSize} Sectors ({StreamingSize * 2048} Bytes)\n" +
				$"Size: {Size} Sectors ({Size * 2048} Bytes)\n" +
				$"Name: {Name}";
		}

		/// <summary>
		/// Create a directory entry from an array of 32 bytes
		/// </summary>
		/// <param name="bytes">An array containing exactly 32 bytes</param>
		/// <returns></returns>
		public static DirectoryEntry FromBytes(byte[] bytes)
		{
			if (bytes.Length != 32)
			{
				throw new ArgumentException("Invalid input buffer size. Must be 32 bytes.");
			}

			// convert bytes to actual values
			byte[] offsetBytes = bytes.SubArray(0, 4);
			byte[] streamingSizeBytes = bytes.SubArray(4, 2);
			byte[] sizeBytes = bytes.SubArray(6, 2);
			byte[] nameBytes = bytes.SubArray(8, 24);

			// create a new entry
			DirectoryEntry entry = new DirectoryEntry
			{
				Offset = BitConverter.ToUInt32(offsetBytes.ReverseIfBigEndian(), 0),
				StreamingSize = BitConverter.ToUInt16(streamingSizeBytes.ReverseIfBigEndian(), 0),
				Size = BitConverter.ToUInt16(sizeBytes.ReverseIfBigEndian(), 0),
				Name = nameBytes.ToNullTerminatedString()
			};

			return entry;
		}
	}
}
