using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace NetworkJogo
{
    public static class Utility
    {
        #region 自端末情報参照
        private static LocalNetworkInfo _localNetworkInfo;
        public static LocalNetworkInfo LocalNetworkInfo{
            get{
                return _localNetworkInfo;
            }
        }
        #endregion

        #region 自端末情報収集
        public static void gatherLocalNetworkInfo()
        {
            NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in nis)
            {
                if (ni.OperationalStatus != OperationalStatus.Up) continue;
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel) continue;

                IPInterfaceProperties ipips = ni.GetIPProperties();
                if (ipips == null) continue;

                System.Net.IPAddress[] ipaddrs = null;
                System.Net.IPAddress[] subnetmasks = null;
                System.Net.IPAddress[] dnsaddrs = null;
                System.Net.IPAddress[] dhcpaddrs = null;
                System.Net.IPAddress[] gatewayaddrs = null;
                System.Net.IPAddress[] winsaddrs = null;

                if (ipips.UnicastAddresses.Count > 0)
                {
                    ipaddrs = new System.Net.IPAddress[ipips.UnicastAddresses.Count];
                    subnetmasks = new System.Net.IPAddress[ipips.UnicastAddresses.Count];
                    int i = 0;
                    foreach (UnicastIPAddressInformation ips in ipips.UnicastAddresses)
                    {
                        ipaddrs[i] = ips.Address;
                        subnetmasks[i] = ips.IPv4Mask;
                        i++;
                    }
                }
                if (ipips.DnsAddresses.Count > 0)
                {
                    dnsaddrs = new System.Net.IPAddress[ipips.DnsAddresses.Count];
                    int i = 0;
                    foreach (System.Net.IPAddress ip in ipips.DnsAddresses)
                    {
                        dnsaddrs[i] = ip;
                        i++;
                    }
                }
                if (ipips.DhcpServerAddresses.Count > 0)
                {
                    dhcpaddrs = new System.Net.IPAddress[ipips.DhcpServerAddresses.Count];
                    int i = 0;
                    foreach (System.Net.IPAddress ip in ipips.DhcpServerAddresses)
                    {
                        dhcpaddrs[i] = ip;
                        i++;
                    }
                }
                if (ipips.GatewayAddresses.Count > 0)
                {
                    gatewayaddrs = new System.Net.IPAddress[ipips.GatewayAddresses.Count];
                    int i = 0;
                    foreach (GatewayIPAddressInformation ip in ipips.GatewayAddresses)
                    {
                        gatewayaddrs[i] = ip.Address;
                        i++;
                    }
                }
                if (ipips.WinsServersAddresses.Count > 0)
                {
                    winsaddrs = new System.Net.IPAddress[ipips.WinsServersAddresses.Count];
                    int i = 0;
                    foreach (System.Net.IPAddress ip in ipips.WinsServersAddresses)
                    {
                        winsaddrs[i] = ip;
                        i++;
                    }
                }

                _localNetworkInfo = new LocalNetworkInfo(ipaddrs, subnetmasks, dnsaddrs, dhcpaddrs, gatewayaddrs, winsaddrs);
                return;
            }
            _localNetworkInfo = null;
        }
        #endregion

        #region DataTableのCSV変換
        /// <summary>
        /// DataTableの内容をCSVファイルに保存する
        /// </summary>
        /// <param name="dt">CSVに変換するDataTable</param>
        /// <param name="csvPath">保存先のCSVファイルのパス</param>
        /// <param name="writeHeader">ヘッダを書き込む時はtrue。</param>
        public static void  ConvertDataTableToCsv(DataTable dt, string csvPath, bool writeHeader)
        {
            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc =
                System.Text.Encoding.GetEncoding("Shift_JIS");

            //書き込むファイルを開く
            System.IO.StreamWriter sr =
                new System.IO.StreamWriter(csvPath, false, enc);

            int colCount = dt.Columns.Count;
            int lastColIndex = colCount - 1;

            //ヘッダを書き込む
            if (writeHeader)
            {
                for (int i = 0; i < colCount; i++)
                {
                    //ヘッダの取得
                    string field = dt.Columns[i].Caption;
                    //"で囲む
                    field = EncloseDoubleQuotesIfNeed(field);
                    //フィールドを書き込む
                    sr.Write(field);
                    //カンマを書き込む
                    if (lastColIndex > i)
                    {
                        sr.Write(',');
                    }
                }
                //改行する
                sr.Write("\r\n");
            }

            //レコードを書き込む
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < colCount; i++)
                {
                    //フィールドの取得
                    string field = row[i].ToString();
                    //"で囲む
                    field = EncloseDoubleQuotesIfNeed(field);
                    //フィールドを書き込む
                    sr.Write(field);
                    //カンマを書き込む
                    if (lastColIndex > i)
                    {
                        sr.Write(',');
                    }
                }
                //改行する
                sr.Write("\r\n");
            }

            //閉じる
            sr.Close();
        }

        /// <summary>
        /// 必要ならば、文字列をダブルクォートで囲む
        /// </summary>
        private static string EncloseDoubleQuotesIfNeed(string field)
        {
            if (NeedEncloseDoubleQuotes(field))
            {
                return EncloseDoubleQuotes(field);
            }
            return field;
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む
        /// </summary>
        private static string EncloseDoubleQuotes(string field)
        {
            if (field.IndexOf('"') > -1)
            {
                //"を""とする
                field = field.Replace("\"", "\"\"");
            }
            return "\"" + field + "\"";
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む必要があるか調べる
        /// </summary>
        private static bool NeedEncloseDoubleQuotes(string field)
        {
            return field.IndexOf('"') > -1 ||
                field.IndexOf(',') > -1 ||
                field.IndexOf('\r') > -1 ||
                field.IndexOf('\n') > -1 ||
                field.StartsWith(" ") ||
                field.StartsWith("\t") ||
                field.EndsWith(" ") ||
                field.EndsWith("\t");
        }
        #endregion

        #region CSVのDataTable変換
        public static System.Collections.ArrayList convertCsv2DataTable(string csvFileName)
        {
            System.Collections.ArrayList csvRecords = new System.Collections.ArrayList();

            Microsoft.VisualBasic.FileIO.TextFieldParser tfp =
                new Microsoft.VisualBasic.FileIO.TextFieldParser(
                    csvFileName,
                    System.Text.Encoding.UTF8);
            tfp.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
            tfp.Delimiters = new string[] { "," };
            tfp.HasFieldsEnclosedInQuotes = true;
            tfp.TrimWhiteSpace = true;

            while (!tfp.EndOfData)
            {
                string[] fields = tfp.ReadFields();
                csvRecords.Add(fields);
            }
            tfp.Close();

            return csvRecords;
        }
        #endregion
    }
}
