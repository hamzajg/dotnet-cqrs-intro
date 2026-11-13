using System;
using System.Collections.Concurrent;

namespace CqrsWithEs.ReadModel
{
    public interface IPolicyInfoDao
    {
        void CreatePolicyInfo(
            Guid id,
            string policyNumber,
            DateTime coverFrom,
            DateTime coverTo,
            string policyHolderFirstName, string policyHolderLastName,
            string carPlateNumber, string carMake,
            decimal totalPremium);

        void UpdatePolicyInfo(
            Guid id,
            string policyNumber,
            DateTime coverFrom,
            DateTime coverTo,
            string policyHolderFirstName, string policyHolderLastName,
            string carPlateNumber, string carMake,
            decimal totalPremium);
    }

    /// <summary>
    /// Read-model store for the InMemory storage profile: the same rows the Postgres
    /// policy_info_view holds, kept in process memory so no server is needed.
    /// </summary>
    public class InMemoryPolicyInfoDao : IPolicyInfoDao
    {
        public ConcurrentDictionary<Guid, PolicyInfo> Rows { get; } = new();

        public void CreatePolicyInfo(
            Guid id,
            string policyNumber,
            DateTime coverFrom,
            DateTime coverTo,
            string policyHolderFirstName, string policyHolderLastName,
            string carPlateNumber, string carMake,
            decimal totalPremium)
        {
            Rows[id] = new PolicyInfo
            {
                PolicyId = id,
                PolicyNumber = policyNumber,
                CoverFrom = coverFrom,
                CoverTo = coverTo,
                PolicyHolder = $"{policyHolderLastName} {policyHolderFirstName}",
                Vehicle = $"{carPlateNumber} {carMake}",
                TotalPremiumAmount = totalPremium
            };
        }

        public void UpdatePolicyInfo(
            Guid id,
            string policyNumber,
            DateTime coverFrom,
            DateTime coverTo,
            string policyHolderFirstName, string policyHolderLastName,
            string carPlateNumber, string carMake,
            decimal totalPremium)
        {
            Rows[id] = new PolicyInfo
            {
                PolicyId = id,
                PolicyNumber = policyNumber,
                CoverFrom = coverFrom,
                CoverTo = coverTo,
                PolicyHolder = $"{policyHolderLastName} {policyHolderFirstName}",
                Vehicle = $"{carPlateNumber} {carMake}",
                TotalPremiumAmount = totalPremium
            };
        }
    }
}
