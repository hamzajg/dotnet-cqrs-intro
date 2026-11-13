using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;
using SeparateModels.ReadModels;

namespace SeparateModels.EventHandlers
{
    public class PolicyTerminatedProjectionsHandler : INotificationHandler<PolicyTerminated>
    {
        private readonly IPolicyInfoDtoProjection policyInfoDtoProjection;
        private readonly IPolicyVersionDtoProjection policyVersionDtoProjection;

        public PolicyTerminatedProjectionsHandler(IPolicyInfoDtoProjection policyInfoDtoProjection, IPolicyVersionDtoProjection policyVersionDtoProjection)
        {
            this.policyInfoDtoProjection = policyInfoDtoProjection;
            this.policyVersionDtoProjection = policyVersionDtoProjection;
        }

        public Task Handle(PolicyTerminated @event, CancellationToken cancellationToken)
        {
            policyInfoDtoProjection.UpdatePolicyInfoDto(@event.TerminatedPolicy, @event.TerminatedVersion);
            
            policyVersionDtoProjection.CreatePolicyVersionDtoProjection(@event.TerminatedPolicy, @event.TerminatedVersion);
            
            return Task.CompletedTask;
        }
    }
}