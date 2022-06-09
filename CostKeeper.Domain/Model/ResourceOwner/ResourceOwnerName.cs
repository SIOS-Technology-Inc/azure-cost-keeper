/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Framework.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostKeeper.Domain.Model.ResourceOwner
{
    /// <summary>
    /// Azureのリソース所有者の名前を表す値オブジェクト
    /// </summary>
    public class ResourceOwnerName : IValueObject
    {
        public string Value { get; private set; }

        public ResourceOwnerName(string value)
        {
            Value = value;
        }
    }
}
