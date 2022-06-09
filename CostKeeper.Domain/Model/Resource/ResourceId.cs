/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.Resource
{
    /// <summary>
    /// AzureのリソースのリソースIDを表す値オブジェクト
    /// </summary>
    public class ResourceId : IValueObject
    {
        public string Value { get; private set; }

        public ResourceId(string value)
        {
            Value = value;
        }
    }
}
