
using System;
using System.IO;

using Contador.Web.Shared.Files;

using Microsoft.AspNetCore.Mvc;

namespace Contador.Web.Server.Controllers
{
	/// <summary>
	/// Provides methods to get or add receipts files.
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class FilesController : ControllerBase
	{

		private readonly string _receiptsPath = Path.Combine(Environment.CurrentDirectory, "Files", "Receipts");

		/// <summary>
		/// Gets the receipt file path by the name of the file.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>Correct code for the action.</returns>
		[HttpGet("{fileName}")]
		public ActionResult<string> GetReceiptFullPath(string fileName)
		{
			return Ok(Path.Combine(_receiptsPath, fileName));
		}


		/// <summary>
		/// Posts the receipt image.
		/// </summary>
		/// <param name="fileChunk">Chunk of the receipt image file.</param>
		/// <returns>Correct code for the action and boolean indicating if file has been uploaded.</returns>
		[HttpPost]
		public ActionResult<bool> UploadReceiptChunk([FromBody] FileChunk fileChunk)
		{
			try
			{
				string fileName = Path.Combine(_receiptsPath, fileChunk.FileNameNoPath);

				if (fileChunk.FirstChunk && System.IO.File.Exists(fileName))
				{
					return BadRequest(false);
				}

				using (var stream = System.IO.File.OpenWrite(fileName))
				{
					stream.Seek(fileChunk.Offset, SeekOrigin.Begin);
					stream.Write(fileChunk.Data, 0, fileChunk.Data.Length);
				}

				return Ok(true);
			}
			catch (Exception ex)
			{
				var msg = ex.Message;

				return BadRequest(msg);
			}
		}
	}
}
