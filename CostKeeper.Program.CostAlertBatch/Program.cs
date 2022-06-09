/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Application.Dto.UsageReport;
using CostKeeper.Application.Service.UsageReport;
using CostKeeper.Domain.Model.Message;
using CostKeeper.Domain.Model.ResourceOwner;
using CostKeeper.Domain.Model.Subscription;
using CostKeeper.Domain.Model.Usage;
using CostKeeper.Domain.Model.UsageReport.Transport.Mail;
using CostKeeper.Domain.Repository.ResourceGroup;
using CostKeeper.Domain.Repository.Usage;
using CostKeeper.Domain.Repository.UsageReport;
using CostKeeper.Infrastructure.Configuration;
using CostKeeper.Infrastructure.ResourceOwner;
using CostKeeper.Infrastructure.Usage;
using CostKeeper.Infrastructure.UsageReport;
using CostKeeper.Infrastructure.Utils;
using CostKeeper.Presentation.CostAlertBatch.UsageReport;

var builder = WebApplication.CreateBuilder(args);

// インフラストラクチャ層で利用する設定ファイルを格納するKeyValueを定義する。
ConfigurationMap configurationMap = new ConfigurationMap();
configurationMap.Add("TENANT_ID", Environment.GetEnvironmentVariable("TENANT_ID")); ;
configurationMap.Add("CLIENT_ID", Environment.GetEnvironmentVariable("CLIENT_ID"));
configurationMap.Add("CLIENT_SECRET", Environment.GetEnvironmentVariable("CLIENT_SECRET"));
configurationMap.Add("SMTP_HOST", Environment.GetEnvironmentVariable("SMTP_HOST"));
configurationMap.Add("SMTP_USER", Environment.GetEnvironmentVariable("SMTP_USER"));
configurationMap.Add("SMTP_PASSWORD", Environment.GetEnvironmentVariable("SMTP_PASSWORD"));
configurationMap.Add("SMTP_PORT", Environment.GetEnvironmentVariable("SMTP_PORT"));
configurationMap.Add("USAGE_BASEDATE_ADDDAYS", Environment.GetEnvironmentVariable("USAGE_BASEDATE_ADDDAYS") ?? "-2");
configurationMap.Add("UID_ATTR", Environment.GetEnvironmentVariable("UID_ATTR") ?? "UserPrincipalName");

// DIする。
builder.Services.AddSingleton<IRepositoryResourceGroup, RepositoryResourceGroup>();
builder.Services.AddSingleton<IRepositorySubscription, RepositorySubscription>();
builder.Services.AddSingleton<IRepositoryUsage, RepositoryUsage>();
builder.Services.AddSingleton<IUsageQueryService, UsageQueryService>();
builder.Services.AddSingleton<UsageApplicationService>();
builder.Services.AddSingleton<IRepositorySendUsageReport, RepositorySendUsageReportByHtmlMail>();
builder.Services.AddSingleton<IUsageReportLayoutService, UsageReportLayoutHtmlService>();
builder.Services.AddSingleton<IResourceOwnerFactory, ResourceOwnerFactory>();
builder.Services.AddSingleton<GraphAPIUtil>();
builder.Services.AddSingleton<AzureManagementAPIUtil>();
builder.Services.AddSingleton<ConfigurationMap>(configurationMap);

// メールでレポートを送信する際のエンベロープ情報(送信者や送信先など)を定義する。
MailFrom mailFrom = new MailFrom(Environment.GetEnvironmentVariable("SMTP_FROM"));
MailTo mailTo = new MailTo(Environment.GetEnvironmentVariable("SMTP_TO"));
MailEnvelope envelope = new MailEnvelope(mailFrom, mailTo);

// 環境変数から利用料取得対象のサブスクリプションを取得する。
string[] subscriptions = Environment.GetEnvironmentVariable("SUBSCRIPTION").Split(":");

List<SubscriptionId> subscriptionIds = new List<SubscriptionId>();

foreach (string subscrition in subscriptions) {
    subscriptionIds.Add(new SubscriptionId(subscrition));
}

var app = builder.Build();

// DIされたクラスのインスタンス化されたオブジェクトを取得する。
UsageApplicationService usageApplicationService = app.Services.GetRequiredService<UsageApplicationService>();
IUsageQueryService usageQueryService = app.Services.GetRequiredService<IUsageQueryService>();
IUsageReportLayoutService usageReportLayoutService = app.Services.GetRequiredService<IUsageReportLayoutService>();

// レポートを作成する。
LimitMonthlyUsage limitMonthlyUsage = new LimitMonthlyUsage(Convert.ToDecimal(Environment.GetEnvironmentVariable("LIMIT_MONTHLY_COST")));
ResourceGroupUsageDailyReport resourceGroupUsageReport = await usageQueryService.GetResourceGroupReport(subscriptionIds, limitMonthlyUsage);
UsageReportLayoutDto usageReportLayoutDto = usageReportLayoutService.CreateUsageReportLayout(resourceGroupUsageReport.OrderByDescending().Top(50));

// レポートを送信する。
await usageApplicationService.SendUsageReport(envelope, usageReportLayoutDto);
