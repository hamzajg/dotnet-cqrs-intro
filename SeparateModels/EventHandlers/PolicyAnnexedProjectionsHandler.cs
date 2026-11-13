using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;
using SeparateModels.ReadModels;

namespace SeparateModels.EventHandlers
{
    public class PolicyAnnexedProjectionsHandler :
        INotificationHandler<PolicyAnnexed>
    {
        private readonly IPolicyInfoDtoProjection policyInfoDtoProjection;
        private readonly IPolicyVersionDtoProjection policyVersionDtoProjection;

        public PolicyAnnexedProjectionsHandler(IPolicyInfoDtoProjection policyInfoDtoProjection, IPolicyVersionDtoProjection policyVersionDtoProjection)
        {
            this.policyInfoDtoProjection = policyInfoDtoProjection;
            this.policyVersionDtoProjection = policyVersionDtoProjection;
        }

        public Task Handle(PolicyAnnexed @event, CancellationToken cancellationToken)
        {
            policyInfoDtoProjection.UpdatePolicyInfoDto(@event.AnnexedPolicy, @event.AnnexVersion);
            
            policyVersionDtoProjection.CreatePolicyVersionDtoProjection(@event.AnnexedPolicy, @event.AnnexVersion);
            
            return Task.CompletedTask;
        }
    }
}