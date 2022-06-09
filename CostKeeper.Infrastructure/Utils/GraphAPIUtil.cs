/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostKeeper.Infrastructure.Utils
{
    public class GraphAPIUtil : RestAPIUtil
    {
        public GraphAPIUtil(ConfigurationMap configurationMap) : base(configurationMap)
        {
        }

        protected override string[] SCOPES => new string[] { "https://graph.microsoft.com/.default" };
    }
}
