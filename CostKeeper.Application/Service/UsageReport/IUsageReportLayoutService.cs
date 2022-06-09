/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Application.Dto.UsageReport;
using CostKeeper.Domain.Model.Message;

namespace CostKeeper.Application.Service.UsageReport
{
    /// <summary>
    /// レポートをテキスト形式やHTML形式に整形するためのサービスのインターフェース
    /// </summary>
    public interface IUsageReportLayoutService
    {
        public UsageReportLayoutDto CreateUsageReportLayout(ResourceGroupUsageDailyReport resourceGroupUsageReport);
    }
}
