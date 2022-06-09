/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
namespace CostKeeper.Domain.Model.UsageReport.Transport.Mail
{
    /// <summary>
    /// レポートをメールで送る際のエンベロープ情報を表す値オブジェクト
    /// </summary>
    public class MailEnvelope : Envelope
    {
        public MailFrom MailFrom { get; private set; }
        public MailTo MailTo { get; private set; }

        public MailEnvelope(MailFrom mailFrom, MailTo mailTo) {
            MailFrom = mailFrom;
            MailTo = mailTo;
        }
    }
}
