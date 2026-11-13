using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;
using SeparateModels.ReadModels;

namespace SeparateModels.EventHandlers
{
    public class PolicyChangesCancelledProjectionsHandler :
        INotificationHandler<PolicyAnnexCancelled>
    {
        private readonly IPolicyInfoDtoProjection policyInfoDtoProjection;
        private readonly IPolicyVersionDtoProjection policyVersionDtoProjection;

        public PolicyChangesCancelledProjectionsHandler(IPolicyInfoDtoProjection policyInfoDtoProjection, IPolicyVersionDtoProjection policyVersionDtoProjection)
        {
            this.policyInfoDtoProjection = policyInfoDtoProjection;
            this.policyVersionDtoProjection = policyVersionDtoProjection;
        }

        public Task Handle(PolicyAnnexCancelled @event, CancellationToken cancellationToken)
        {
            policyInfoDtoProjection.UpdatePolicyInfoDto(@event.Policy, @event.CurrentVersionAfterAnnexCancellation);
            
            policyVersionDtoProjection.UpdatePolicyVersionDtoProjection(@event.CancelledAnnexVersion);
            
            return Task.CompletedTask;
        }
    }
}