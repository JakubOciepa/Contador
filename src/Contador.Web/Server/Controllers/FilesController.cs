
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



		[HttpPost]
		public ActionResult<bool> UploadReceiptChunk([FromBody] FileChunk fileChunk)
		{
			try
			{
				string filePath = Path.Combine(Environment.CurrentDirectory, "Files", "Receipts");
				string fileName = Path.Combine(filePath, fileChunk.FileNameNoPath);

				if (Directory.Exists(filePath) is not true)
				{
					Directory.CreateDirectory(filePath);
				}

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
