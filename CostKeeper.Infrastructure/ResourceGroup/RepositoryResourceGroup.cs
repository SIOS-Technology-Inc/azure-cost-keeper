/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.ResourceGroup;
using CostKeeper.Domain.Model.ResourceOwner;
using CostKeeper.Domain.Model.Subscription;
using CostKeeper.Domain.Repository.ResourceGroup;
using CostKeeper.Infrastructure.Utils;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace CostKeeper.Infrastructure.Usage
{
    /// <summary>
    /// Azureのリソースグループの情報を取得するためのリポジトリ(実装)
    /// </summary>
    public class RepositoryResourceGroup : IRepositoryResourceGroup
    {
        private readonly AzureManagementAPIUtil AzureManagementAPIUtil;

        public RepositoryResourceGroup(AzureManagementAPIUtil azureManagementAPIUtil)
        {
            AzureManagementAPIUtil = azureManagementAPIUtil;
        }

        public async Task<ResourceGroup> FindByResourceGroupId(ResourceGroupId resourceGroupId)
        {
            // リソースIDからサブスクリプションIDを取得する。
            string subscriptionId = ResourceUtils.SubscriptionFromResourceId(resourceGroupId.Value);
            Value value;

            try {
                 value = await AzureManagementAPIUtil.Get<Value>("https://management.azure.com" + resourceGroupId.Value + "?api-version=2021-04-01");

                ResourceOwnerId resourceOwnerId = null;
                if (value.tags != null && value.tags.ackowner != null)
                {
                    resourceOwnerId = new ResourceOwnerId(value.tags.ackowner);
                }

                ResourceGroup resourceGroup = new ResourceGroup(
                    new SubscriptionId(subscriptionId),
                    new ResourceGroupId(value.id),
                    new ReosurceGroupName(value.name),
                    resourceOwnerId
                    );

                return resourceGroup;
            } catch(HttpRequestException e) {
                // リソースグループがない場合、つまり404のえらーが返ってきた場合はnullを返す。
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

                throw e;
            }

        }

        public async Task<List<Domain.Model.ResourceGroup.ResourceGroup>> FindBySubscriptionId(SubscriptionId subscriptionId)
        {
            Response result = await AzureManagementAPIUtil.Get<Response>("https://management.azure.com/subscriptions/" + subscriptionId.Value + "/resourcegroups?api-version=2021-04-01");

            List<ResourceGroup> resourceGroups = new List<ResourceGroup>();

            foreach (Value value in result.value)
            {
                ResourceOwnerId resourceOwnerId = null;
                if (value.tags != null && value.tags.ackowner != null) {
                    resourceOwnerId = new ResourceOwnerId(value.tags.ackowner);
                }

                ResourceGroup resourceGroup = new ResourceGroup(
                    subscriptionId,
                    new ResourceGroupId(value.id),
                    new ReosurceGroupName(value.name),
                    resourceOwnerId
                );
                resourceGroups.Add(resourceGroup);

            }

            return resourceGroups;
        }

        public async Task save(ResourceGroup resourceGroup)
        {
            // 現状のところではタグ情報の更新のみとしておく
            string body = $@"
{{
  ""operation"": ""merge"", 
  ""properties"": {{
    ""tags"": {{
      ""ackowner"": ""{resourceGroup.ResourceOwnerId.Value}""
    }}
  }}
}}
";
            await AzureManagementAPIUtil.Patch("https://management.azure.com" + resourceGroup.ResourceGroupId.Value + "/providers/Microsoft.Resources/tags/default?api-version=2021-04-01", body);
        }

        public class Response
        {
            public Value[] value { get; set; }
        }

        public class Value
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string location { get; set; }
            public Properties properties { get; set; }
            public Tags tags { get; set; }
            public string managedBy { get; set; }
        }

        public class Properties
        {
            public string provisioningState { get; set; }
        }

        public class Tags
        {
            public string ackowner { get; set; }
        }


    }
}
