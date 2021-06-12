namespace Contador.Web.Shared.Files
{
	/// <summary>
	/// Contains information about file chunk to upload/download.
	/// </summary>
	public class FileChunk
	{

		/// <summary>
		/// Name of the file.
		/// </summary>
		public string FileNameNoPath { get; set; } = "";

		/// <summary>
		/// Offset of download/upload.
		/// </summary>
		public long Offset { get; set; }

		/// <summary>
		/// Data of the file.
		/// </summary>
		public byte[] Data { get; set; }

		/// <summary>
		/// Indicates if the chunk is the first one.
		/// </summary>
		public bool FirstChunk { get; set; }
	}
}
