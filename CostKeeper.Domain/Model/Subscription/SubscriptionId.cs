/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.Resource;
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.Subscription
{
    /// <summary>
    /// AzureのサブスクリプションのIDを表す値オブジェクト
    /// </summary>
    public class SubscriptionId : ResourceId
    {
        public SubscriptionId(string value) : base(value)
        {
        }
    }
}
