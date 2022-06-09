/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.ResourceGroup;
using CostKeeper.Domain.Model.ResourceOwner;
using CostKeeper.Domain.Model.Subscription;
using CostKeeper.Domain.Model.Usage;
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.Message
{

    /// <summary>
    /// リソースグループごとのレポートにおけるリソースグループ単位の利用料を表す値オブジェクト
    /// </summary>
    public class ResourceGroupUsage : IValueObject
    {
        public SubscriptionName SubscriptionName { get; private set; }
        public ReosurceGroupName ReosurceGroupName { get; private set; }
        public QueryUsage QueryUsage { get; private set; }
        public ResourceOwnerName ResourceOwnerName { get; private set; }

        public ResourceGroupUsage(
            SubscriptionName subscriptionName,
            ReosurceGroupName reosurceGroupName,
            QueryUsage queryUsage,
            ResourceOwnerName resourceOwnerName
            )
        {
            SubscriptionName = subscriptionName;
            ReosurceGroupName = reosurceGroupName;
            QueryUsage = queryUsage;
            ResourceOwnerName = resourceOwnerName;
        }

    }
}
