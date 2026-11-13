using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SeparateModels.Domain;

namespace SeparateModels.ReadModels.InMemory
{
    /// <summary>
    /// Read-model tables the Postgres profile keeps in policy_info_view, policy_version_view
    /// and policy_version_cover, held in memory instead. Registered as a singleton so the
    /// projections and the finders see the same rows.
    /// </summary>
    public class InMemoryReadModelStore
    {
        public ConcurrentDictionary<Guid, PolicyInfoDto> PolicyInfos { get; } = new();
        public ConcurrentDictionary<Guid, PolicyVersionDto> PolicyVersions { get; } = new();
    }

    public class InMemoryPolicyInfoDtoProjection : IPolicyInfoDtoProjection
    {
        private readonly InMemoryReadModelStore store;

        public InMemoryPolicyInfoDtoProjection(InMemoryReadModelStore store)
        {
            this.store = store;
        }

        public void CreatePolicyInfoDto(Policy policy)
        {
            var version = policy.Versions.WithNumber(1);

            store.PolicyInfos[policy.Id] = new PolicyInfoDto
            {
                PolicyId = policy.Id,
                PolicyNumber = policy.Number,
                CoverFrom = version.CoverPeriod.ValidFrom,
                CoverTo = version.CoverPeriod.ValidTo,
                PolicyHolder = $"{version.PolicyHolder.LastName} {version.PolicyHolder.FirstName}",
                Vehicle = $"{version.Car.PlateNumber} {version.Car.Make}",
                TotalPremiumAmount = version.TotalPremium.Amount
            };
        }

        public void UpdatePolicyInfoDto(Policy policy, PolicyVersion currentVersion)
        {
            store.PolicyInfos[policy.Id] = new PolicyInfoDto
            {
                PolicyId = policy.Id,
                PolicyNumber = policy.Number,
                CoverFrom = currentVersion.CoverPeriod.ValidFrom,
                CoverTo = currentVersion.CoverPeriod.ValidTo,
                PolicyHolder = $"{currentVersion.PolicyHolder.LastName} {currentVersion.PolicyHolder.FirstName}",
                Vehicle = $"{currentVersion.Car.PlateNumber} {currentVersion.Car.Make}",
                TotalPremiumAmount = currentVersion.TotalPremium.Amount
            };
        }
    }

    public class InMemoryPolicyVersionDtoProjection : IPolicyVersionDtoProjection
    {
        private readonly InMemoryReadModelStore store;

        public InMemoryPolicyVersionDtoProjection(InMemoryReadModelStore store)
        {
            this.store = store;
        }

        public void CreatePolicyVersionDtoProjection(Policy policy, PolicyVersion policyVersion)
        {
            store.PolicyVersions[policyVersion.Id] = new PolicyVersionDto
            {
                Id = policyVersion.Id,
                PolicyId = policy.Id,
                PolicyNumber = policy.Number,
                ProductCode = policy.ProductCode,
                VersionNumber = policyVersion.VersionNumber,
                VersionStatus = policyVersion.VersionStatus.ToString(),
                PolicyStatus = policyVersion.PolicyStatus.ToString(),
                PolicyHolder =
                    $"{policyVersion.PolicyHolder.LastName} {policyVersion.PolicyHolder.FirstName} {policyVersion.PolicyHolder.TaxId}",
                Insured = "",
                Car = $"{policyVersion.Car.PlateNumber} {policyVersion.Car.Make} {policyVersion.Car.ProductionYear}",
                CoverFrom = policyVersion.CoverPeriod.ValidFrom,
                CoverTo = policyVersion.CoverPeriod.ValidTo,
                VersionFrom = policyVersion.VersionValidityPeriod.ValidFrom,
                VersionTo = policyVersion.VersionValidityPeriod.ValidTo,
                TotalPremium = policyVersion.TotalPremium.Amount,
                Covers = policyVersion.Covers.Select(cover => new CoverDto
                {
                    Id = Guid.NewGuid(),
                    Code = cover.CoverCode,
                    CoverFrom = cover.CoverPeriod.ValidFrom,
                    CoverTo = cover.CoverPeriod.ValidTo,
                    PremiumAmount = cover.Amount.Amount
                }).ToList()
            };
        }

        public void UpdatePolicyVersionDtoProjection(PolicyVersion policyVersion)
        {
            if (store.PolicyVersions.TryGetValue(policyVersion.Id, out var dto))
                dto.VersionStatus = policyVersion.VersionStatus.ToString();
        }
    }

    public class InMemoryPolicyInfoDtoFinder : IPolicyInfoDtoFinder
    {
        private readonly InMemoryReadModelStore store;

        public InMemoryPolicyInfoDtoFinder(InMemoryReadModelStore store)
        {
            this.store = store;
        }

        public IList<PolicyInfoDto> FindByFilter(PolicyFilter filter)
        {
            var query = store.PolicyInfos.Values.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(filter.PolicyNumber))
                query = query.Where(p => p.PolicyNumber == filter.PolicyNumber);

            if (filter.PolicyStartDateFrom.HasValue)
                query = query.Where(p => p.CoverFrom >= filter.PolicyStartDateFrom.Value);

            if (filter.PolicyStartDateTo.HasValue)
                query = query.Where(p => p.CoverTo <= filter.PolicyStartDateTo.Value);

            if (!string.IsNullOrWhiteSpace(filter.CarPlateNumber))
                query = query.Where(p => p.Vehicle != null && p.Vehicle.StartsWith(filter.CarPlateNumber + " "));

            if (!string.IsNullOrWhiteSpace(filter.PolicyHolder))
                query = query.Where(p => p.PolicyHolder == filter.PolicyHolder);

            return query.ToList();
        }
    }

    public class InMemoryPolicyVersionDtoFinder : IPolicyVersionDtoFinder
    {
        private readonly InMemoryReadModelStore store;

        public InMemoryPolicyVersionDtoFinder(InMemoryReadModelStore store)
        {
            this.store = store;
        }

        public PolicyVersionDto FindByPolicyNumberAndVersionNumber(string policyNumber, int versionNumber) =>
            store.PolicyVersions.Values.FirstOrDefault(v =>
                v.PolicyNumber == policyNumber && v.VersionNumber == versionNumber);

        public PolicyVersionsListDto FindVersionsListByPolicyNumber(string policyNumber) =>
            new PolicyVersionsListDto
            {
                PolicyNumber = policyNumber,
                VersionsInfo = store.PolicyVersions.Values
                    .Where(v => v.PolicyNumber == policyNumber)
                    .OrderBy(v => v.VersionNumber)
                    .Select(v => new PolicyVersionInfoDto
                    {
                        Number = v.VersionNumber,
                        VersionFrom = v.VersionFrom,
                        VersionTo = v.VersionTo,
                        VersionStatus = v.VersionStatus
                    })
                    .ToList()
            };
    }
}
