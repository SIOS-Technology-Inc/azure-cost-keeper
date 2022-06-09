/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.Subscription;

namespace CostKeeper.Domain.Repository.Usage
{
    /// <summary>
    /// Azureのサブスクリプションの情報を取得するためのリポジトリインターフェース
    /// </summary>
    public interface IRepositorySubscription
    {
        public Task<Model.Subscription.Subscription> findBySubscriptionId(SubscriptionId subscriptionId);
    }
}
