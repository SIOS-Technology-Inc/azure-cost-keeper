/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.UsageReport.Transport;

namespace CostKeeper.Domain.Repository.UsageReport
{
    /// <summary>
    ///レポートをメールがSlackで送信するためのリポジトリインターフェース
    /// </summary>

    public interface IRepositorySendUsageReport
    {
        public void Send(Envelope envelop, Subject subject, Body body);
    }
}
