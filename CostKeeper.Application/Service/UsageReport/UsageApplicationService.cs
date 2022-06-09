/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Application.Dto.UsageReport;
using CostKeeper.Domain.Model.UsageReport.Transport;
using CostKeeper.Domain.Repository.UsageReport;

namespace CostKeeper.Application.Service.UsageReport
{
    /// <summary>
    /// アプリケーションサービスクラス
    /// </summary>
    public class UsageApplicationService
    {
        private readonly IRepositorySendUsageReport UsageReportSendRepository;

        public UsageApplicationService(IRepositorySendUsageReport usageReportSendRepository)
        {
            UsageReportSendRepository = usageReportSendRepository;
        }
        
        /// <summary>
        /// レポートを送信する。
        /// </summary>
        public async Task SendUsageReport(Envelope envelope, UsageReportLayoutDto usageReportLayoutDto)
        {
            UsageReportSendRepository.Send(envelope, new Subject(usageReportLayoutDto.Title), new Body(usageReportLayoutDto.Body)) ;
        }

    }
}
