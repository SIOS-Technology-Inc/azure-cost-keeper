/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostKeeper.Infrastructure.Configuration
{
    /// <summary>
    /// インフラストラクチャ層で利用する各種設定値(APIへの接続情報など)を格納したKeyValueオブジェクト
    /// </summary>

    public class ConfigurationMap : Dictionary<String, String>
    {
        public ConfigurationMap() { 
        }

    }

}
