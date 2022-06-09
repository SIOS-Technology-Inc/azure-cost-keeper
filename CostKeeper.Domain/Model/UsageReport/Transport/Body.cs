/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.UsageReport.Transport
{
    /// <summary>
    /// レポートをメールやSlackなどで通知する際の本文を表す値オブジェクト
    /// </summary>
    public class Body : IValueObject
    {
        public string Value { get; private set; } = "";

        public Body(string value)
        {
            Value = value;
        }
    }
}
