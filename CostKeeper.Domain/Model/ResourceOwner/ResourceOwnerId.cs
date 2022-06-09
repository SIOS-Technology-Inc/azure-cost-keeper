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
    /// Azureのリソース所有者のIDを表す値オブジェクト
    /// </summary>
    public class ResourceOwnerId : IValueObject
    {
        public string Value { get; private set; }

        public ResourceOwnerId(string value)
        {
            Value = value;
        }
    }
}
