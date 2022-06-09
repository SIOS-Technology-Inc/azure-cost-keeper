/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.Usage
{
    /// <summary>
    /// レポートの集計対象の日を表す値オブジェクト
    /// </summary>
    public class UsageBaseDate : IValueObject
    {
        public DateTime Value { get; private set; }

        public UsageBaseDate(DateTime value)
        {
            Value = value;
        }
    }
}
