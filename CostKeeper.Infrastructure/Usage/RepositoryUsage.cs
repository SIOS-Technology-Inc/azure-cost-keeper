/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.Subscription;
using CostKeeper.Domain.Model.Usage;
using CostKeeper.Domain.Repository.Usage;
using CostKeeper.Infrastructure.Utils;
using System.Text.Json;

namespace CostKeeper.Infrastructure.Usage
{
    /// <summary>
    /// Azureの料金情報を取得するためのリポジトリ(実装)
    /// </summary>
    public class RepositoryUsage : IRepositoryUsage
    {

        private readonly AzureManagementAPIUtil AzureManagementAPIUtil;

        public RepositoryUsage(AzureManagementAPIUtil azureManagementAPIUtil) { 
            AzureManagementAPIUtil = azureManagementAPIUtil;
        }

        public async Task<ForeCastUsage> FindForeCastUsageBySubscriptionId(SubscriptionId subscriptionId)
        {
            var json = @"
{
  ""type"": ""Usage"",
  ""timeframe"": ""MonthToDate"",
  ""dataset"": {
    ""granularity"": ""None"",
    ""aggregation"": {
      ""totalCost"": {
        ""name"": ""PreTaxCost"",
        ""function"": ""Sum""
      }
    }
  },
  ""includeActualCost"": ""true"",
  ""includeFreshPartialCost"": ""true""
}
";

            Response response = await AzureManagementAPIUtil.Post<Response>("https://management.azure.com/subscriptions/" + subscriptionId.Value + "/providers/Microsoft.CostManagement/forecast?api-version=2021-10-01", json);

            var actualUsageData = response.properties.rows[0][0];
            var actualUsage = Convert.ToDecimal(actualUsageData.ToString());

            var foreCastUsageData = response.properties.rows[1][0];
            var foreCastUsage = Convert.ToDecimal(foreCastUsageData.ToString());

            return new ForeCastUsage(actualUsage + foreCastUsage);
        }

        public async Task<QueryUsage> FindQueryUsageBySubscriptionId(SubscriptionId subscriptionId)
        {
            var json = @"
{
  ""type"": ""Usage"",
  ""timeframe"": ""MonthToDate"",
  ""dataset"": {
    ""granularity"": ""None"",
    ""aggregation"": {
      ""totalCost"": {
        ""name"": ""PreTaxCost"",
        ""function"": ""Sum""
      }
    }
  }
}
";

            Response response = await AzureManagementAPIUtil.Post<Response>("https://management.azure.com/subscriptions/" + subscriptionId.Value + "/providers/Microsoft.CostManagement/query?api-version=2021-10-01", json);

            var result = response.properties.rows[0][0];

            var usage = Convert.ToDecimal(result.ToString());

            QueryUsage queryUsage = new QueryUsage(usage);

            return queryUsage;
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
