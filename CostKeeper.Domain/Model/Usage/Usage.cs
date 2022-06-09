/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.Usage
{
    /// <summary>
    /// Azureの料金を表す値オブジェクト
    /// </summary>
    public class Usage : IValueObject
    {
        public decimal Value { get; private set; } = 0;

        public Usage(decimal value)
        {
            if (value < 0) throw new ArgumentException();
            Value = value;
        }

        public Usage plus(Usage otherValue)
        {
            return new Usage(otherValue.Value + Value);
        }

    }

}
