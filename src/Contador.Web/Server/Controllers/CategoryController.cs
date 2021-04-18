
using System.Threading.Tasks;

using Contador.Core.Models;

using Microsoft.AspNetCore.Mvc;

namespace Contador.Web.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : Controller
	{
		[HttpGet]
		public async Task<ActionResult<ExpenseCategory>> GetAll()
		{
			return Ok();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ExpenseCategory>> GetById([FromRoute] int id)
		{
			return Ok();
		}

		[HttpGet("{name}")]
		public async Task<ActionResult<ExpenseCategory>>GetByName([FromRoute] string name)
		{
			return Ok();
		}

		[HttpPost]
		public async Task<ActionResult> Add([FromBody] ExpenseCategory category)
		{
			return Ok();
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> Update([FromBody] int id, [FromBody] ExpenseCategory category)
		{
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Remove([FromBody] int id)
		{
			return Ok(); 
		}
	}
}
