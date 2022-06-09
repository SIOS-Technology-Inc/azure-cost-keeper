/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Infrastructure.Configuration;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CostKeeper.Infrastructure.Utils
{

    /// <summary>
    /// Rest APIを発行するためのユーティリティクラス
    /// </summary>
    public abstract class RestAPIUtil
    {
        private readonly ConfigurationMap ConfigurationMap;
        private HttpClient client;
        private IConfidentialClientApplication app;
        //private readonly string[] SCOPES = new string[] {
        //    "https://management.azure.com/.default"
        //};

        protected abstract string[] SCOPES { get; }

        public RestAPIUtil(ConfigurationMap configurationMap) {
            ConfigurationMap = configurationMap;

            app = ConfidentialClientApplicationBuilder
                .Create(configurationMap["CLIENT_ID"])
                .WithTenantId(configurationMap["TENANT_ID"])
                .WithClientSecret(configurationMap["CLIENT_SECRET"])
                .Build();

            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<T> Get<T>(String uri) {
            HttpClient client = await AcquireHttpClientWithToken();

            var hoge = await client.GetStringAsync(uri);
            var streamTask = client.GetStreamAsync(uri);

            var repositories = await JsonSerializer.DeserializeAsync<T>(await streamTask);

            return repositories;
        }

        public async Task<T> Post<T>(string uri, string body)
        {
            HttpClient client = await AcquireHttpClientWithToken();

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(uri, content);

            var result = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals
            };

            var repositories = JsonSerializer.Deserialize<T>(result, options);

            return repositories;
        }

        public async Task<string> Get(String uri)
        {
            HttpClient client = await AcquireHttpClientWithToken();

            var result = await client.GetStringAsync(uri);

            return result;
        }

        public async Task<string> Post(string uri, string body)
        {
            HttpClient client = await AcquireHttpClientWithToken();

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(uri, content);

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        public async Task Patch(string uri, string body)
        {
            HttpClient client = await AcquireHttpClientWithToken();

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync(uri, content);

            Console.WriteLine(response.Content);
        }

        private async Task<HttpClient> AcquireHttpClientWithToken() {
            // 後ほどリトライ処理を実装する必要あり
            var token = await app.AcquireTokenForClient(SCOPES)
                .WithForceRefresh(true)
                .ExecuteAsync();

            if (!string.IsNullOrEmpty(token?.AccessToken)) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            }

            return client;

        }
    }

}
