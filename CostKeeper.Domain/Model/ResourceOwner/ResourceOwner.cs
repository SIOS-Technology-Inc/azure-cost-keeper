/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.ResourceOwner
{
    /// <summary>
    /// Azureのリソース所有者を表すエンティティ
    /// </summary>
    public class ResourceOwner : IEntity
    {
        public ResourceOwnerId ResourceOwnerId { get; private set; }
        public ResourceOwnerName ResourceOwnerName { get; private set; }

        public ResourceOwner(ResourceOwnerId resourceOwnerId, ResourceOwnerName resourceOwnerName) { 
            ResourceOwnerId = resourceOwnerId;
            ResourceOwnerName = resourceOwnerName;
        }
    }
}
