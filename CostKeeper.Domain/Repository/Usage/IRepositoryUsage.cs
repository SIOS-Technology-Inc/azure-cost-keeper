/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.Subscription;
using CostKeeper.Domain.Model.Usage;

namespace CostKeeper.Domain.Repository.Usage
{
    /// <summary>
    /// Azureの料金情報を取得するためのリポジトリインターフェース
    /// </summary>

    public interface IRepositoryUsage
    {
        public Task<QueryUsage> FindQueryUsageBySubscriptionId(SubscriptionId subscriptionId);
        public Task<ForeCastUsage> FindForeCastUsageBySubscriptionId(SubscriptionId subscriptionId);
    }
}
