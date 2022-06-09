/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Application.Service.UsageReport;
using CostKeeper.Domain.Model.Message;
using CostKeeper.Domain.Model.Resource;
using CostKeeper.Domain.Model.ResourceGroup;
using CostKeeper.Domain.Model.ResourceOwner;
using CostKeeper.Domain.Model.Subscription;
using CostKeeper.Domain.Model.Usage;
using CostKeeper.Domain.Repository.ResourceGroup;
using CostKeeper.Domain.Repository.Usage;
using CostKeeper.Infrastructure.Configuration;
using CostKeeper.Infrastructure.Utils;
using System.Globalization;

namespace CostKeeper.Infrastructure.Usage
{
    /// <summary>
    /// 複雑な条件で料金情報を取得するためのクエリサービスの実装
    /// </summary>
    public class UsageQueryService : IUsageQueryService
    {
        private readonly IRepositorySubscription RepositorySubscription;
        private readonly IRepositoryResourceGroup RepositoryResourceGroup;
        private readonly IRepositoryUsage RepositoryUsage;
        private readonly IResourceOwnerFactory ResourceOwnerFactory;
        private readonly AzureManagementAPIUtil AzureManagementAPIUtil;
        private readonly ConfigurationMap ConfigurationMap;

        public UsageQueryService(IRepositorySubscription repositorySubscription, IRepositoryResourceGroup repositoryResourceGroup, IRepositoryUsage repositoryUsage, IResourceOwnerFactory resourceOwnerFactory, AzureManagementAPIUtil azureManagementAPIUtil, ConfigurationMap configurationMap)
        {
            RepositorySubscription = repositorySubscription;
            RepositoryResourceGroup = repositoryResourceGroup;
            RepositoryUsage = repositoryUsage;
            ResourceOwnerFactory = resourceOwnerFactory;
            AzureManagementAPIUtil = azureManagementAPIUtil;
            ConfigurationMap = configurationMap;
        }

        public async Task<ResourceGroupUsageDailyReport> GetResourceGroupReport(List<SubscriptionId> subscriptionIds, LimitMonthlyUsage limitMonthlyUsage)
        {
            Domain.Model.Usage.Usage totalQueryUsage = new QueryUsage(0);
            Domain.Model.Usage.Usage totalForeCastUsage = new ForeCastUsage(0);
            List<ResourceGroupUsage> resourceGroupUsages = new List<ResourceGroupUsage>();

            // 料金情報を取得するための基準日を定義する。
            int usageBaseDateAdddays = Convert.ToInt16(ConfigurationMap["USAGE_BASEDATE_ADDDAYS"]);
            UsageBaseDate usageBaseDate = new UsageBaseDate(DateTime.Today.AddDays(usageBaseDateAdddays));

            // 指定したサブスクリプションに含まれるリソースグループの一覧を取得する。
            foreach (SubscriptionId subscriptionId in subscriptionIds)
            {
                Subscription subscription = await RepositorySubscription.findBySubscriptionId(subscriptionId);

                var usageBaseDateStr = usageBaseDate.Value.ToString("yyyy-MM-dd");

                var json = @"
{{
  ""type"": ""Usage"",
  ""timeframe"": ""Custom"",
  ""timePeriod"": {{
    ""from"": ""{0}T00:00:00.000Z"",
    ""to"": ""{0}T23:59:59.999Z""
      }},
  ""dataset"": {{
    ""granularity"": ""None"",
    ""aggregation"": {{
      ""totalCost"": {{
        ""name"": ""PreTaxCost"",
        ""function"": ""Sum""
      }}
    }},
    ""grouping"": [
      {{
        ""type"": ""Dimension"",
        ""name"": ""ResourceGroup""
      }}
    ]
  }}
}}
";

                var request = string.Format(json, usageBaseDateStr);

                // リソースグループごとの料金情報を取得する。
                Response response = await AzureManagementAPIUtil.Post<Response>("https://management.azure.com/subscriptions/" + subscriptionId.Value + "/providers/Microsoft.CostManagement/query?api-version=2021-10-01", request);

                // リソースグループの一覧を取得する。
                List<ResourceGroup> resourceGroups = await RepositoryResourceGroup.FindBySubscriptionId(subscriptionId);

                // 指定したサブスクリプションに含まれるリソースグループで回して、
                // リソースグループ単位の料金情報を取得する。
                foreach (Object[] values in response.properties.rows) {
                    // JSONをデシリアライズすると料金が浮動小数点形式になることがあるので、それを修正する。
                    if (!Decimal.TryParse(values[0].ToString(), out var value)) {
                        value = Decimal.Parse(values[0].ToString().ToLower(), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint);
                    }
                    
                    //decimal resourceGroupUsageValue = Convert.ToDecimal(value);
                    decimal resourceGroupUsageValue = value;
                    string resourceGroupNameValue = values[1].ToString();

                    // リソースグループの管理者を取得する。
                    string resourceGroupId = "/subscriptions/" + subscriptionId.Value + "/resourceGroups/" + resourceGroupNameValue;

                    // microsoft.defaultという謎の見えないリソースグループがあり、これを取得しようとすると
                    // 404エラーになるのでスキップする(要調査)。
                    Domain.Model.ResourceOwner.ResourceOwner resourceOwner = null;
                    if (resourceGroupNameValue != "microsoft.default") {
                        ResourceGroup resourceGroup = resourceGroups.Find(n => n.ResourceGroupId.Value.ToLower() == resourceGroupId.ToLower());
                        resourceOwner = await ResourceOwnerFactory.createResourceOwner(new ResourceId(resourceGroupId));
                    }

                    ResourceGroupUsage resourceGroupUsage = new ResourceGroupUsage(
                        subscription.SubscriptionName,
                        new Domain.Model.ResourceGroup.ReosurceGroupName(resourceGroupNameValue),
                        new QueryUsage(resourceGroupUsageValue),
                        resourceOwner?.ResourceOwnerName
                        );

                    resourceGroupUsages.Add(resourceGroupUsage);
                }

                // サブスクリプションごとの実際の利用料と予測利用料を取得する。
                QueryUsage subsctiprionQueryUsage = await RepositoryUsage.FindQueryUsageBySubscriptionId(subscriptionId);
                totalQueryUsage = totalQueryUsage.plus(subsctiprionQueryUsage);
                ForeCastUsage subsctiprionForeCastUsage = await RepositoryUsage.FindForeCastUsageBySubscriptionId(subscriptionId);
                totalForeCastUsage = totalForeCastUsage.plus(subsctiprionForeCastUsage);

            }

            // 今まで取得した情報をもとにリソースグループ単位のレポートを生成する。
            ResourceGroupUsageDailyReport resourceGroupUsageReport = new ResourceGroupUsageDailyReport(
                usageBaseDate,
                new QueryUsage(totalQueryUsage.Value),
                new ForeCastUsage(totalForeCastUsage.Value),
                new LimitMonthlyUsage(limitMonthlyUsage.Value),
                resourceGroupUsages
                );

            return resourceGroupUsageReport;

        }

        public class Response
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public object location { get; set; }
            public object sku { get; set; }
            public object eTag { get; set; }
            public Properties properties { get; set; }
        }

        public class Properties
        {
            public object nextLink { get; set; }
            public Column[] columns { get; set; }
            public object[][] rows { get; set; }
        }

        public class Column
        {
            public string name { get; set; }
            public string type { get; set; }
        }
    }
}
