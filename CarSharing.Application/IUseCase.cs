using System.Threading.Tasks;

namespace CarSharing.Application
{
    public interface IUseCase<TInput, TOutput> where TOutput : BaseUseCaseOutput
    {
        Task<TOutput> HandleAsync(TInput input);
    }
}
