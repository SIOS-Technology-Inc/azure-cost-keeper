/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.UsageReport.Transport;
using CostKeeper.Domain.Model.UsageReport.Transport.Mail;
using CostKeeper.Domain.Repository.UsageReport;
using CostKeeper.Infrastructure.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace CostKeeper.Infrastructure.UsageReport
{

    /// <summary>
    /// レポートをHTMLメールで送信するためのリポジトリクラス
    /// </summary>
    public class RepositorySendUsageReportByHtmlMail : IRepositorySendUsageReport
    {
        private readonly ConfigurationMap ConfigurationMap;

        public RepositorySendUsageReportByHtmlMail(ConfigurationMap configurationMap) { 
            ConfigurationMap = configurationMap;
        }

        public void Send(Envelope envelop, Subject subject, Body body)
        {
            MailEnvelope mailEnvelope = (MailEnvelope)envelop;

            using (var smtp = new SmtpClient(ConfigurationMap["SMTP_HOST"], Convert.ToInt16(ConfigurationMap["SMTP_PORT"])))
            {
                smtp.Timeout = 10000;
                smtp.Credentials = new NetworkCredential(ConfigurationMap["SMTP_USER"], ConfigurationMap["SMTP_PASSWORD"]);
                smtp.EnableSsl = true;

                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(mailEnvelope.MailFrom.Value);
                    mail.To.Add(mailEnvelope.MailTo.Value);
                    mail.Subject = subject.Value;
                    mail.SubjectEncoding = Encoding.UTF8;
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body.Value, null, Text.Html);
                    htmlView.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                    mail.AlternateViews.Add(htmlView);

                    smtp.Send(mail);
                }
            }


        }
    }
}
