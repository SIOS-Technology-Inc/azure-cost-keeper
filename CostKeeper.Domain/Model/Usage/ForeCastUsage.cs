/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
namespace CostKeeper.Domain.Model.Usage
{
    public class ForeCastUsage : Usage
    {
        /// <summary>
        /// Azureの使用料金を表す値オブジェクト
        /// </summary>
        public ForeCastUsage(decimal value) : base(value)
        {
        }
    }
}
