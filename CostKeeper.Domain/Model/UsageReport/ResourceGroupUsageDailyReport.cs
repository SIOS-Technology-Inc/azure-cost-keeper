/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.Usage;
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.Message
{
    public class ResourceGroupUsageDailyReport : IValueObject
    {
        /// <summary>
        /// リソースグループごとのレポートを表す値オブジェクト
        /// </summary>
        public UsageBaseDate UsageBaseDate { get; private set; }
        public QueryUsage TotalQueryUsage { get; private set; }
        public ForeCastUsage TotalForeCastUsage { get; private set; }
        public LimitMonthlyUsage LimitMonthlyUsage { get; private set; }
        public List<ResourceGroupUsage> ResourceGroupUsages { get; private set; }

        public ResourceGroupUsageDailyReport(
            UsageBaseDate usageBaseDate,
            QueryUsage totalQueryUsage,
            ForeCastUsage totalForeCastUsage,
            LimitMonthlyUsage limitMonthlyUsage,
            List<ResourceGroupUsage> resourceGroupUsages)
        {
            UsageBaseDate = usageBaseDate;
            TotalQueryUsage = totalQueryUsage;
            TotalForeCastUsage = totalForeCastUsage;
            LimitMonthlyUsage = limitMonthlyUsage;
            ResourceGroupUsages = resourceGroupUsages;
        }

        public Usage.Usage TargetUsageSavingPerDay {
            get
            {
                DateTime usageBaseDate = UsageBaseDate.Value;
                var lastDayOfMonth = new DateTime(usageBaseDate.Year, usageBaseDate.Month, 1).AddMonths(1).AddDays(-1).Day;

                var restDaysToLastDayOfMonth = lastDayOfMonth - usageBaseDate.Day;

                var targetUsageSavingPerDay = (TotalForeCastUsage.Value - LimitMonthlyUsage.Value) / restDaysToLastDayOfMonth;

                if (targetUsageSavingPerDay < 0) targetUsageSavingPerDay = 0;

                return new Usage.Usage(targetUsageSavingPerDay);

            }
            private set { }

        }

        public Usage.Usage UnexpectedUsage
        {
            get
            {
                if (TotalForeCastUsage.Value - LimitMonthlyUsage.Value < 0)
                {
                    return new Usage.Usage(0);
                }
                else { 
                    return new Usage.Usage(TotalForeCastUsage.Value - LimitMonthlyUsage.Value);                
                }

            }
            private set { }
        }

        public ResourceGroupUsageDailyReport OrderByDescending() {
            var orderdItems = ResourceGroupUsages.OrderByDescending(a => a.QueryUsage.Value);
            List<ResourceGroupUsage> orderdResourceGroupDayBeforeUsages = new List<ResourceGroupUsage>();

            foreach (ResourceGroupUsage item in orderdItems) {
                orderdResourceGroupDayBeforeUsages.Add(item);
            }

            this.ResourceGroupUsages = orderdResourceGroupDayBeforeUsages;
            return this;
        }

        public ResourceGroupUsageDailyReport Top(int count) {
            var orderdItems = ResourceGroupUsages.Take(count);
            List<ResourceGroupUsage> orderdResourceGroupDayBeforeUsages = new List<ResourceGroupUsage>();

            foreach (ResourceGroupUsage item in orderdItems)
            {
                orderdResourceGroupDayBeforeUsages.Add(item);
            }

            this.ResourceGroupUsages = orderdResourceGroupDayBeforeUsages;
            return this;

        }

        public Boolean IsOverLimitMonthlytUsage() {
            return UnexpectedUsage.Value > 0;
        }
    }
}
