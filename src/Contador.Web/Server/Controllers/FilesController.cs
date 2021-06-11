
using System;
using System.IO;

using Contador.Web.Shared.Files;

using Microsoft.AspNetCore.Mvc;

namespace Contador.Web.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FilesController : ControllerBase
	{

		private readonly string _receiptsPath = Path.Combine(Environment.CurrentDirectory, "Files", "Receipts");

		[HttpGet("{fileName}")]
		public ActionResult<string> GetReceiptFullPath(string fileName)
		{
			return Ok(Path.Combine(_receiptsPath, fileName));
		}

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
