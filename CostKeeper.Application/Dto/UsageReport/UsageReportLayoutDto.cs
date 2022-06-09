/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
namespace CostKeeper.Application.Dto.UsageReport
{
    /// <summary>
    /// メール用やSlack用など各種クライアント向けに整形されたレポートを格納するDTO
    /// </summary>
    public class UsageReportLayoutDto
    {
        public string Title { get; private set; }
        public string Body { get; private set; }

        public UsageReportLayoutDto(string title, string body) { 
            Title = title;
            Body = body;
        }

    }
}
