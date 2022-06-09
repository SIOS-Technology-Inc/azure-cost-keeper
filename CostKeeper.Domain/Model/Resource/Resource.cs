/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.ResourceGroup;
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.Resource
{
    /// <summary>
    /// Azureのリソースを表すエンティティ
    /// </summary>
    public class Resource : IEntity
    {
        public ResourceGroupId ResourceGroupId { get; private set; }
        public ResourceId ResourceId { get; private set; }
        public ResourceName ResourceName { get; private set; }

        public Resource(ResourceGroupId resourceGroupId, ResourceId resourceId, ResourceName resourceName)
        {
            ResourceGroupId = resourceGroupId;
            ResourceId = resourceId;
            ResourceName = resourceName;
        }


    }
}
