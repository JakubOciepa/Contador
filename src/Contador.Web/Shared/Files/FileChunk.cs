namespace Contador.Web.Shared.Files
{
	public class FileChunk
	{
		public string FileNameNoPath { get; set; } = "";

		public long Offset { get; set; }

		public byte[] Data { get; set; }

		public bool FirstChunk { get; set; }
	}
}
