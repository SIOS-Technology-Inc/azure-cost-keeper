/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.Resource;
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.ResourceGroup
{
    /// <summary>
    /// AzureのリソースグループのIDを表す値オブジェクト
    /// </summary>
    public class ResourceGroupId : ResourceId
    {
        public ResourceGroupId(string value) : base(value)
        {
        }
    }
}
