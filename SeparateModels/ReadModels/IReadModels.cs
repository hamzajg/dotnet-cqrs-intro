using System.Collections.Generic;
using SeparateModels.Domain;

namespace SeparateModels.ReadModels
{
    public interface IPolicyInfoDtoProjection
    {
        void CreatePolicyInfoDto(Policy policy);
        void UpdatePolicyInfoDto(Policy policy, PolicyVersion currentVersion);
    }

    public interface IPolicyVersionDtoProjection
    {
        void CreatePolicyVersionDtoProjection(Policy policy, PolicyVersion policyVersion);
        void UpdatePolicyVersionDtoProjection(PolicyVersion policyVersion);
    }

    public interface IPolicyInfoDtoFinder
    {
        IList<PolicyInfoDto> FindByFilter(PolicyFilter filter);
    }

    public interface IPolicyVersionDtoFinder
    {
        PolicyVersionDto FindByPolicyNumberAndVersionNumber(string policyNumber, int versionNumber);
        PolicyVersionsListDto FindVersionsListByPolicyNumber(string policyNumber);
    }
}
