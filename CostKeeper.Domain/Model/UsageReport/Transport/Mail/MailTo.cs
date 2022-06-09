/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Framework.Domain.Model;

namespace CostKeeper.Domain.Model.UsageReport.Transport.Mail
{
    /// <summary>
    /// レポートをメールで送るときの送信先を表す値オブジェクト
    /// </summary>
    public class MailTo : IValueObject
    {
        public string Value { get; private set; } = "";

        public MailTo(string value)
        {
            Value = value;
        }
    }
}
