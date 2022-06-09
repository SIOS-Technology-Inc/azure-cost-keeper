/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.Subscription;
using CostKeeper.Domain.Repository.Usage;
using CostKeeper.Infrastructure.Utils;

namespace CostKeeper.Infrastructure.Usage
{
    /// <summary>
    /// Azureのサブスクリプションの情報を取得するためのリポジトリ(実装)
    /// </summary>
    public class RepositorySubscription : IRepositorySubscription
    {
        private readonly AzureManagementAPIUtil AzureManagementAPIUtil;

        public RepositorySubscription(AzureManagementAPIUtil azureManagementAPIUtil)
        {
            AzureManagementAPIUtil = azureManagementAPIUtil;
        }

        public async Task<Domain.Model.Subscription.Subscription> findBySubscriptionId(SubscriptionId subscriptionId)
        {
            Response result = await AzureManagementAPIUtil.Get<Response>("https://management.azure.com/subscriptions/" + subscriptionId.Value + "?api-version=2020-01-01");

            SubscriptionName subscriptionName = new SubscriptionName(result.displayName);
            Subscription subscription = new Subscription(subscriptionId, subscriptionName);

            return subscription;
        }

        class Response
        {
            public string displayName { get; set; }
        }
    }

}



