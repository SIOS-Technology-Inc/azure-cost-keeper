/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.Message;
using CostKeeper.Domain.Model.Subscription;
using CostKeeper.Domain.Model.Usage;

namespace CostKeeper.Application.Service.UsageReport
{
    /// <summary>
    /// 複雑な条件で料金情報を取得するためのクエリサービスのインターフェース
    /// </summary>
    public interface IUsageQueryService
    {
        public Task<ResourceGroupUsageDailyReport> GetResourceGroupReport(List<SubscriptionId> subscriptionIds, LimitMonthlyUsage limitMonthlyUsage);
    }
}
