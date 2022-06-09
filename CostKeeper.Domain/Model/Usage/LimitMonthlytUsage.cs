/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
namespace CostKeeper.Domain.Model.Usage
{
    public class LimitMonthlyUsage : Usage
    {
        /// <summary>
        /// ひと月あたり使ってもよい料金の上限を表す値オブジェクト
        /// </summary>
        public LimitMonthlyUsage(decimal value) : base(value)
        {
        }
    }
}
