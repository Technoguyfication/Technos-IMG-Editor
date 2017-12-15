using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.IMGEditor.Version2
{
	/// <summary>
	/// Represents a file directory entry
	/// https://www.gtamodding.com/wiki/IMG_archive#Directory_Entry_2
	/// </summary>
	public class Ver2DirectoryEntry : IDirectoryEntry
	{
		/// <summary>
		/// The offset of the data (in sectors)
		/// </summary>
		public uint Offset { get; set; }

		/// <summary>
		/// Streaming Size (in sectors)
		/// </summary>
		public uint StreamingSize { get; set; }

		/// <summary>
		/// Size of the file (in sectors)
		/// </summary>
		public uint InternalSize { get; set; }

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
				if (Encoding.ASCII.GetByteCount(value) > Ver2IMGArchive.MAX_DIRECTORY_FILE_NAME)
					throw new Exception($"Name cannot be longer than {Ver2IMGArchive.MAX_DIRECTORY_FILE_NAME} bytes.");

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
			builder.AddRange(BitConverter.GetBytes((uint)Offset).ReverseIfBigEndian());
			builder.AddRange(BitConverter.GetBytes((ushort)StreamingSize).ReverseIfBigEndian());
			builder.AddRange(BitConverter.GetBytes((ushort)InternalSize).ReverseIfBigEndian());
			builder.AddRange(Encoding.ASCII.GetBytes(Name));

			// fill the rest of the string with null bytes
			builder.AddRange(new byte[Ver2IMGArchive.DIRECTORY_ITEM_SIZE - builder.Count]);

			return builder.ToArray();
		}

		/// <summary>
		/// The size of the directory entry, in sectors
		/// </summary>
		/// <returns></returns>
		public uint Size
		{
			get
			{
				if (StreamingSize == 0)
					return InternalSize;
				else
					return StreamingSize;
			}
			set
			{
				StreamingSize = (ushort)value;
			}
		}

		/// <summary>
		/// Returns a string containing information about the directory entry
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"Offset: {Offset} Sectors ({Offset * 2048} Bytes)\n" +
				$"Streaming Size: {StreamingSize} Sectors ({StreamingSize * 2048} Bytes)\n" +
				$"Size: {InternalSize} Sectors ({InternalSize * 2048} Bytes)\n" +
				$"Name: {Name}";
		}

		/// <summary>
		/// Create a directory entry from an array of 32 bytes
		/// </summary>
		/// <param name="bytes">An array containing exactly 32 bytes</param>
		/// <returns></returns>
		public static Ver2DirectoryEntry FromBytes(byte[] bytes)
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
			Ver2DirectoryEntry entry = new Ver2DirectoryEntry
			{
				Offset = BitConverter.ToUInt32(offsetBytes.ReverseIfBigEndian(), 0),
				StreamingSize = BitConverter.ToUInt16(streamingSizeBytes.ReverseIfBigEndian(), 0),
				InternalSize = BitConverter.ToUInt16(sizeBytes.ReverseIfBigEndian(), 0),
				Name = nameBytes.ToNullTerminatedString()
			};

			return entry;
		}

		public override bool Equals(object obj)
		{
			return GetHashCode() == obj.GetHashCode();
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
	}
}
