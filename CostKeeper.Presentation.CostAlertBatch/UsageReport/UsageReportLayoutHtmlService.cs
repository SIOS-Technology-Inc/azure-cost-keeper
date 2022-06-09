/*
 * SPDX-License-Identifier: Apache-2.0
 *
 * Copyright (c) 2022 SIOS Technology, Inc.
 */
using CostKeeper.Application.Dto.UsageReport;
using CostKeeper.Application.Service.UsageReport;
using CostKeeper.Domain.Model.Message;
using System.Globalization;

namespace CostKeeper.Presentation.CostAlertBatch.UsageReport
{

    /// <summary>
    /// レポートをHTML形式に整形するためのサービスクラス
    /// </summary>
    public class UsageReportLayoutHtmlService : IUsageReportLayoutService
    {
        public UsageReportLayoutDto CreateUsageReportLayout(ResourceGroupUsageDailyReport resourceGroupUsageReport)
        {
            DateTime dt = resourceGroupUsageReport.UsageBaseDate.Value;
            string today = dt.ToString("yyyy年M月d日");
            string week_name = dt.ToString("ddd", new CultureInfo("ja-JP"));
            string limitMonthly = FormatUsageForReport(resourceGroupUsageReport.LimitMonthlyUsage.Value);
            string totalQueryUsage = FormatUsageForReport(resourceGroupUsageReport.TotalQueryUsage.Value);
            string unexpectedUsage = FormatUsageForReport(resourceGroupUsageReport.UnexpectedUsage.Value);
            string targetUsageSavingPerDay = FormatUsageForReport(resourceGroupUsageReport.TargetUsageSavingPerDay.Value);

            string header = "";
            string subject = "";
            if (resourceGroupUsageReport.IsOverLimitMonthlytUsage()) {
                subject = "【!!注意!!コスト超過の危険あり!!】Azure利用状況のお知らせ(" + today + "(" + week_name + ")時点)";
                header = $@"
<p> Azure利用者各位 </p>
<p>Azureの利用状況({today}({week_name})時点)をお知らせします。</p>
<p><b><font color=""#F00"">現在のペースで使っていくと、Azureの月額最大想定使用料({limitMonthly} 円)を超えてしまいます！！(T_T)</font></b></p>
<p>現在、今月分は{totalQueryUsage}円利用しており、月末には{unexpectedUsage}円超過する予定です。</p>
<p>以下に、{today} ({ week_name})の<b><font color=""#F00"">1日間</font></b>で消費された、リソースグループごとの使用料をお知らせいたします。
<br>※使用料が高い順に上位50位までを表示しています。</p>
<p><b><font color=""#F00"">このリストを参考にして、不要なリソースを削除する等、リソースの見直しを至急行ってください。</font></b></p>
<p>明日からの一日あたりの使用料を<b><font color=""#F00\"">{targetUsageSavingPerDay}円</font></b>程度削減して頂けますと、月額最大想定使用料内に<br>
収まる想定ですので、何卒ご協力の程よろしくお願い致します。</p>
";
            } else {
                subject = "Azure利用状況のお知らせ(" + today + "(" + week_name + ")時点)";
                header = $@"
<p>Azure利用者各位</p>
<p>Azureの利用状況({today}({week_name})時点)をお知らせします。</p>
<p><b><font color=""#00F"">現在のペースで使っていけば、Azureの月額最大想定使用料({limitMonthly}円)を超えることはありません(^o^)</font></b></p>
ただ、なるべく普段から不要なリソースを作成しないように心がけて下さい。</p>
<p>参考までに、{today}({week_name})の<b><font color=""#00F"">1日間</font></b>で消費された、リソースグループごとの使用料をお知らせいたします。
<br>※使用料が高い順に上位50位までを表示しています。</p>
";
            }

            string html = $@"
<html><body style=""background - color: #fff;"">
{header}
<body style=""background - color: #fff;"">
<table cellspacing =""0"" style=""margin: 10px 30px 0px 0px; padding: 0px; border: solid 1px #666;"">
<tr>
<td style=""background: #3d63c6;  color: #fff;padding: 3px 5px; border-bottom: dotted 1px #fff; border-right: solid 1px #fff;"">No</td>
<td style=""background: #3d63c6;  color: #fff;padding: 3px 5px; border-bottom: dotted 1px #fff; border-right: solid 1px #fff;"">サブスクリプション名</td>
<td style=""background: #3d63c6;  color: #fff;padding: 3px 5px; border-bottom: dotted 1px #fff; border-right: solid 1px #fff;"">リソースグループ名</td>
<td style=""background: #3d63c6;  color: #fff;padding: 3px 5px; border-bottom: dotted 1px #fff; border-right: solid 1px #fff;"">管理者</td>
<td style=""background: #3d63c6;  color: #fff;padding: 3px 5px; border-bottom: dotted 1px #fff; border-right: solid 1px #666;"">使用料</td>
</tr>
";

            int rank = 1;
            foreach (var item in resourceGroupUsageReport.ResourceGroupUsages) {
                html += $@"
<tr>
<td style=""padding: 3px 5px;border-bottom: dotted 1px #666;text-align: right;border-right: solid 1px #000;"">{rank}</td>
<td style=""padding: 3px 5px;border-bottom: dotted 1px #666;text-align: left;border-right: solid 1px #000;"">{item.SubscriptionName.Value}</td>
<td style=""padding: 3px 5px;border-bottom: dotted 1px #666;text-align: left;border-right: solid 1px #000;"">{item.ReosurceGroupName.Value}</td>
<td style=""padding: 3px 5px;border-bottom: dotted 1px #666;text-align: left;border-right: solid 1px #000;"">{item.ResourceOwnerName?.Value}</td>
<td style=""padding: 3px 5px;border-bottom: dotted 1px #666;text-align: right;"">{FormatUsageForReport(item.QueryUsage.Value)} 円</td>
</tr>
";
                rank++;
            }

            html += $@"
</table>
</body></html>
";



            UsageReportLayoutDto usageReportLayoutDto = new UsageReportLayoutDto(subject, html);

            return usageReportLayoutDto;
        }

        private string FormatUsageForReport(decimal value) { 
            return String.Format("{0:#,0}", Math.Floor(value));
        }
    }
}
