/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.ResourceGroup
{
    /// <summary>
    /// Azureのリソースグループの名前を表す値オブジェクト
    /// </summary>
    public class ReosurceGroupName : IValueObject
    {
        public string Value { get; private set; }

        public ReosurceGroupName(string value)
        {
            Value = value;
        }
    }
}
