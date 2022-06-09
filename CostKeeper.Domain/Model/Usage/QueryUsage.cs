/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
namespace CostKeeper.Domain.Model.Usage
{
    /// <summary>
    /// Azureの予測料金を表す値オブジェクト
    /// </summary>
    public class QueryUsage : Usage
    {
        public QueryUsage(decimal value) : base(value)
        {
        }
    }
}
