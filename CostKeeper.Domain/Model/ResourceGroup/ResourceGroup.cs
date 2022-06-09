/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.ResourceOwner;
using CostKeeper.Domain.Model.Subscription;
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.ResourceGroup
{
    /// <summary>
    /// Azureのリソースグループを表すエンティティ
    /// </summary>
    public class ResourceGroup : IEntity
    {
        public SubscriptionId SubscriptionId { get; private set; }
        public ResourceGroupId ResourceGroupId { get; private set; }
        public ReosurceGroupName ResourceGroupName { get; private set; }
        public ResourceOwnerId ResourceOwnerId { get; private set; }

        public ResourceGroup(
            SubscriptionId subscriptionId,
            ResourceGroupId resourceGroupId,
            ReosurceGroupName resourceGroupName,
            ResourceOwnerId resourceOwnerId
            )
        {
            SubscriptionId = subscriptionId;
            ResourceGroupId = resourceGroupId;
            ResourceGroupName = resourceGroupName;
            ResourceOwnerId = resourceOwnerId;
        }

        public void changeResourceOwner(ResourceOwnerId resourceOwnerId) {
            ResourceOwnerId = resourceOwnerId;
        }


    }
}
