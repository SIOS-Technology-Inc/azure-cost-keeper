/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.UsageReport.Transport
{
    /// <summary>
    /// レポートをメールやSlackなどで通知する際のタイトルを表す値オブジェクト
    /// </summary>
    public class Subject : IValueObject
    {
        public string Value { get; private set; } = "";

        public Subject(string value)
        {
            Value = value;
        }
    }
}
