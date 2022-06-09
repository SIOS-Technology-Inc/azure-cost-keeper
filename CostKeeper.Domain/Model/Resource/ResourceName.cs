/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.Resource
{
    /// <summary>
    /// Azureのリソースの名前を表す値オブジェクト
    /// </summary>
    public class ResourceName : IValueObject
    {
        public string Value { get; private set; }

        public ResourceName(string value)
        {
            Value = value;
        }
    }
}
