using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.IMGEditor
{
	public static class Shared
	{
		/// <summary>
		/// Size of a data sector, in bytes
		/// </summary>
		public const int SECTOR_SIZE = 2048;

		/// <summary>
		/// Reverse order of bytes if the <see cref="BitConverter"/> is big-endian.
		/// </summary>
		/// <returns>Reversed bytes</returns>
		public static byte[] ReverseIfBigEndian(this byte[] bytes)
		{
			if (!BitConverter.IsLittleEndian)
				Array.Reverse(bytes);

			return bytes;
		}

		/// <summary>
		/// Reverse order of bytes if the <see cref="BitConverter"/> is little-endian
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static byte[] ReverseIfLittleEndian(this byte[] bytes)
		{
			if (BitConverter.IsLittleEndian)
				Array.Reverse(bytes);

			return bytes;
		}

		/// <summary>
		/// Copies a subset of elements from one array to another
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static T[] SubArray<T>(this T[] source, int offset, int length)
		{
			T[] result = new T[length];

			Array.Copy(source, offset, result, 0, length);
			return result;
		}

		/// <summary>
		/// Gets a null-terminated string from an array of bytes
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string ToNullTerminatedString(this byte[] bytes)
		{
			List<byte> finalBytes = new List<byte>();

			int i = 0;
			while (bytes[i] != 0)
			{
				finalBytes.Add(bytes[i]);
				i++;
			}

			return Encoding.ASCII.GetString(finalBytes.ToArray());
		}
	}

	/// <summary>
	/// Thrown when an archive has an invalid or corrupted format
	/// </summary>
	[Serializable]
	public class InvalidArchiveException : Exception
	{
		public InvalidArchiveException() { }
		public InvalidArchiveException(string message) : base(message) { }
		public InvalidArchiveException(string message, Exception inner) : base(message, inner) { }
		protected InvalidArchiveException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
