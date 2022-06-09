/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Domain.Model.ResourceGroup;
using CostKeeper.Domain.Model.ResourceOwner;
using CostKeeper.Domain.Repository.ResourceGroup;
using CostKeeper.Infrastructure.Configuration;
using CostKeeper.Infrastructure.Utils;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace CostKeeper.Infrastructure.ResourceOwner
{
    /// <summary>
    /// リソースオーナーの情報を作成するためのファクトリ(実装)
    /// </summary>
    public class ResourceOwnerFactory : IResourceOwnerFactory
    {
        private readonly IRepositoryResourceGroup RepositoryResourceGroup;
        private readonly AzureManagementAPIUtil AzureManagementAPIUtil;
        private readonly GraphAPIUtil GraphAPIUtil;
        private readonly ConfigurationMap ConfigurationMap;

        public ResourceOwnerFactory(IRepositoryResourceGroup repositoryResourceGroup, AzureManagementAPIUtil azureManagementAPIUtil, GraphAPIUtil graphAPIUtil, ConfigurationMap configurationMap)
        {
            RepositoryResourceGroup = repositoryResourceGroup;
            AzureManagementAPIUtil = azureManagementAPIUtil;
            GraphAPIUtil = graphAPIUtil;
            ConfigurationMap = configurationMap;
        }

        public async Task<Domain.Model.ResourceOwner.ResourceOwner> createResourceOwner(Domain.Model.Resource.ResourceId resourceId)
        {
            // リソースIDからリソースグループの情報を取得する            
            ResourceGroup resourceGroup = await RepositoryResourceGroup.FindByResourceGroupId(new ResourceGroupId(resourceId.Value));

            // 取得しようとしたリソースグループがすでに削除されていた場合、nullを返す
            if (resourceGroup == null) return null;

            ResourceOwnerId resourceOwnerId = resourceGroup.ResourceOwnerId;

            // もしリソースオーナーの情報がなかったら取得処理を行う。
            if (resourceOwnerId == null)
            {
                // リソースIDからサブスクリプションIDを取得する。
                string subscriptionId = ResourceUtils.SubscriptionFromResourceId(resourceId.Value);

                // アクティビティログは90日前のログしか取れないので、現在日付から90日前の日付を生成する
                string startDate = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"); ;

                ActivityLogJsonReponse resourceGroupJsonReponse = await AzureManagementAPIUtil.Get<ActivityLogJsonReponse>("https://management.azure.com/subscriptions/" + subscriptionId + "/providers/Microsoft.Insights/eventtypes/management/values?api-version=2015-04-01&$filter=eventTimestamp ge '" + startDate + "' and resourceId eq '" + resourceId.Value + "'&$select=caller,operationName,subStatus,eventTimestamp");

                // 取得した結果をループで回して、OperationNameとSubStatusが以下に一致するもののcallerがリソースの作成者とする
                // OperationName:Microsoft.Resources/subscriptions/resourcegroups/write
                // SubStatus:Created
                foreach (ActivityLog value in resourceGroupJsonReponse.value)
                {
                    if (value.operationName.value == "Microsoft.Resources/subscriptions/resourceGroups/write"
                        && value.subStatus.value == "Created")
                    {
                        resourceOwnerId = new ResourceOwnerId(value.caller);

                        // アクティビティログから取得したリソースの作成者をリソースオーナーとして、
                        // リソースグループのタグに追加する。
                        string body = $@"
{{
  ""operation"": ""merge"", 
  ""properties"": {{
    ""tags"": {{
      ""ackowner"": ""{resourceOwnerId.Value}""
    }}
  }}
}}
";
                        await AzureManagementAPIUtil.Patch("https://management.azure.com" + resourceGroup.ResourceGroupId.Value + "/providers/Microsoft.Resources/tags/default?api-version=2021-04-01", body);

                    }
                }
            }

            // Azure ADからリソースオーナーの表示名を取得する。
            ResourceOwnerName resourceOwnerName = null;
            if (resourceOwnerId != null)
            {
                ResourceOwnerNameJsonReponse resourceOwnerNameJsonReponse = await GraphAPIUtil.Get<ResourceOwnerNameJsonReponse>("https://graph.microsoft.com/v1.0/users?$filter=" + ConfigurationMap["UID_ATTR"] + " eq '" + resourceOwnerId.Value + "'&$select=displayName");
                if (resourceOwnerNameJsonReponse.value.Length > 0)
                {
                    resourceOwnerName = new ResourceOwnerName(resourceOwnerNameJsonReponse.value[0].displayName);
                }
                return new Domain.Model.ResourceOwner.ResourceOwner(resourceOwnerId, resourceOwnerName);
            }
            else {
                return null;            
            }
        }

        public class ActivityLogJsonReponse
        {
            public ActivityLog[] value { get; set; }
        }

        public class ActivityLog
        {
            public string caller { get; set; }
            public string id { get; set; }
            public string resourceId { get; set; }
            public Operationname operationName { get; set; }
            public Substatus subStatus { get; set; }
            public DateTime eventTimestamp { get; set; }
        }

        public class Operationname
        {
            public string value { get; set; }
            public string localizedValue { get; set; }
        }

        public class Substatus
        {
            public string value { get; set; }
            public string localizedValue { get; set; }
        }


        public class ResourceOwnerNameJsonReponse
        {
            public string odatacontext { get; set; }
            public Value[] value { get; set; }
        }

        public class Value
        {
            public string displayName { get; set; }
        }


    }
}
