/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.UsageReport.Transport
{
    /// <summary>
    /// レポートのエンベロープ情報(送信元や受信者など)を表す値オブジェクト
    /// </summary>
    public abstract class Envelope : IValueObject
    {
    }
}
