using System.Threading.Tasks;

namespace Contador.Abstractions
{
	public interface IExpenseManager : IExpenseNotifier, IExpenseService
	{
	}
}
