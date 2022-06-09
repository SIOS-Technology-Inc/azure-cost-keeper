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

// �C���t���X�g���N�`���w�ŗ��p����ݒ�t�@�C�����i�[����KeyValue���`����B
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

// DI����B
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

// ���[���Ń��|�[�g�𑗐M����ۂ̃G���x���[�v���(���M�҂⑗�M��Ȃ�)���`����B
MailFrom mailFrom = new MailFrom(Environment.GetEnvironmentVariable("SMTP_FROM"));
MailTo mailTo = new MailTo(Environment.GetEnvironmentVariable("SMTP_TO"));
MailEnvelope envelope = new MailEnvelope(mailFrom, mailTo);

// ���ϐ����痘�p���擾�Ώۂ̃T�u�X�N���v�V�������擾����B
string[] subscriptions = Environment.GetEnvironmentVariable("SUBSCRIPTION").Split(":");

List<SubscriptionId> subscriptionIds = new List<SubscriptionId>();

foreach (string subscrition in subscriptions) {
    subscriptionIds.Add(new SubscriptionId(subscrition));
}

var app = builder.Build();

// DI���ꂽ�N���X�̃C���X�^���X�����ꂽ�I�u�W�F�N�g���擾����B
UsageApplicationService usageApplicationService = app.Services.GetRequiredService<UsageApplicationService>();
IUsageQueryService usageQueryService = app.Services.GetRequiredService<IUsageQueryService>();
IUsageReportLayoutService usageReportLayoutService = app.Services.GetRequiredService<IUsageReportLayoutService>();

// ���|�[�g���쐬����B
LimitMonthlyUsage limitMonthlyUsage = new LimitMonthlyUsage(Convert.ToDecimal(Environment.GetEnvironmentVariable("LIMIT_MONTHLY_COST")));
ResourceGroupUsageDailyReport resourceGroupUsageReport = await usageQueryService.GetResourceGroupReport(subscriptionIds, limitMonthlyUsage);
UsageReportLayoutDto usageReportLayoutDto = usageReportLayoutService.CreateUsageReportLayout(resourceGroupUsageReport.OrderByDescending().Top(50));

// ���|�[�g�𑗐M����B
await usageApplicationService.SendUsageReport(envelope, usageReportLayoutDto);
