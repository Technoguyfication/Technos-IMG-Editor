using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.IMGEditor.Shared.FileStructure
{
	/// <summary>
	/// Represents a file directory entry - https://www.gtamodding.com/wiki/IMG_archive#Directory_Entry_2
	/// </summary>
	public struct DirectoryEntry
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
		/// Name of the file the data represents
		/// </summary>
		public string Name { get; set; }

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
			builder.AddRange(new byte[IMGFileVer2.DIR_ENTRY_SIZE - builder.Count]);

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
	}
}
