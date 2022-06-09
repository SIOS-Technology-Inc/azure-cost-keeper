/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.ResourceGroup;
using CostKeeper.Domain.Model.Subscription;

namespace CostKeeper.Domain.Repository.ResourceGroup
{
    /// <summary>
    /// Azureのリソースグループの情報を取得するためのリポジトリインターフェース
    /// </summary>
    public interface IRepositoryResourceGroup
    {
        public Task<List<Model.ResourceGroup.ResourceGroup>> FindBySubscriptionId(SubscriptionId subscriptionId);
        public Task<Model.ResourceGroup.ResourceGroup> FindByResourceGroupId(ResourceGroupId resourceGroupId);
        public Task save(Model.ResourceGroup.ResourceGroup resourceGroup);
    }
}
