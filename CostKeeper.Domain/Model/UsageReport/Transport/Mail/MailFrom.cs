/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.UsageReport.Transport.Mail
{
    /// <summary>
    /// レポートをメールで送るときの送信元を表す値オブジェクト
    /// </summary>
    public class MailFrom : IValueObject
    {
        public string Value { get; private set; } = "";

        public MailFrom(string value)
        {
            Value = value;
        }
    }
}
