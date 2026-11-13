using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;
using SeparateModels.ReadModels;
using SeparateModels.Services;

namespace SeparateModels.Queries
{
    public class GetPolicyVersionDetailsHandler : IRequestHandler<GetPolicyVersionDetailsQuery, PolicyVersionDto>
    {
        private readonly IPolicyVersionDtoFinder policyVersionDtoFinder;

        public GetPolicyVersionDetailsHandler(IPolicyVersionDtoFinder policyVersionDtoFinder)
        {
            this.policyVersionDtoFinder = policyVersionDtoFinder;
        }

        public Task<PolicyVersionDto> Handle(GetPolicyVersionDetailsQuery query, CancellationToken cancellationToken)
        {
            var policyVersion = policyVersionDtoFinder.FindByPolicyNumberAndVersionNumber
            (
                query.PolicyNumber,
                query.VersionNumber
            );
            return Task.FromResult(policyVersion);
        }
    }
}