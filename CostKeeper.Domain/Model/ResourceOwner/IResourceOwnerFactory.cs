/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.Resource;

namespace CostKeeper.Domain.Model.ResourceOwner
{
    /// <summary>
    /// リソースオーナーの情報を作成するためのファクトリ(インターフェース)
    /// </summary>
    public interface IResourceOwnerFactory
    {
        public Task<ResourceOwner> createResourceOwner(ResourceId resourceId);
    }
}
