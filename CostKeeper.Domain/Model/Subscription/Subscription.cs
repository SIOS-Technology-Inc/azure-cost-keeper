/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.Usage;

namespace CostKeeper.Domain.Model.Subscription
{
    /// <summary>
    /// Azureのサブスクリプションを表すエンティティ
    /// </summary>
    public class Subscription
    {
        public SubscriptionId SubscriptionId { get; private set; }
        public SubscriptionName SubscriptionName { get; private set; }

        public Subscription(
            SubscriptionId subscriptionId,
            SubscriptionName subscriptionName)
        {
            SubscriptionId = subscriptionId;
            SubscriptionName = subscriptionName;
        }

    }
}
