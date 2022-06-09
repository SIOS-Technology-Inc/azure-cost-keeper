/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.Subscription
{
    /// <summary>
    /// Azureのサブスクリプションの名前を表す値オブジェクト
    /// </summary>
    public class SubscriptionName : IValueObject
    {
        public string Value { get; private set; }

        public SubscriptionName(string value)
        {
            Value = value;
        }
    }
}
