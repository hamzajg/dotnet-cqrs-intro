using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.ReadModels;

namespace SeparateModels.Queries
{
    public class GetPolicyVersionsListHandler : IRequestHandler<GetPolicyVersionsListQuery, GetPolicyVersionsListResult>
    {
        private readonly IPolicyVersionDtoFinder policyVersionDtoFinder;

        public GetPolicyVersionsListHandler(IPolicyVersionDtoFinder policyVersionDtoFinder)
        {
            this.policyVersionDtoFinder = policyVersionDtoFinder;
        }

        public Task<GetPolicyVersionsListResult> Handle(GetPolicyVersionsListQuery query, CancellationToken cancellationToken)
        {
            var versions = policyVersionDtoFinder.FindVersionsListByPolicyNumber(query.PolicyNumber);
            
            return Task.FromResult
            (
                new GetPolicyVersionsListResult
                {
                    Versions = versions
                }
            );
        }
    }
}