using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkJogo
{
    public class LocalNetworkInfo
    {
        private System.Net.IPAddress[] _ipaddrs;
        private System.Net.IPAddress[] _netmasks;
        private System.Net.IPAddress[] _dnsaddrs;
        private System.Net.IPAddress[] _dhcpaddrs;
        private System.Net.IPAddress[] _gatewayaddrs;
        private System.Net.IPAddress[] _winsaddrs;

        #region コンストラクタ
        public LocalNetworkInfo(
            System.Net.IPAddress[] ipaddrs
            , System.Net.IPAddress[] subnetmasks
            , System.Net.IPAddress[] dnsaddrs
            , System.Net.IPAddress[] dhcpaddrs
            , System.Net.IPAddress[] gatewayaddrs
            , System.Net.IPAddress[] winsaddrs)
        {
            _ipaddrs = ipaddrs;
            _netmasks = subnetmasks;
            _dnsaddrs = dnsaddrs;
            _dhcpaddrs = dhcpaddrs;
            _gatewayaddrs = gatewayaddrs;
            _winsaddrs = winsaddrs;
        }
        #endregion

        #region アクセッサ
        public System.Net.IPAddress[] IPADDRS{
            get
            {
                return _ipaddrs;
            }
        }
        public System.Net.IPAddress[] NETMASKS
        {
            get
            {
                return _netmasks;
            }
        }
        public System.Net.IPAddress[] DNS
        {
            get
            {
                return _dnsaddrs;
            }
        }
        public System.Net.IPAddress[] DHCP
        {
            get
            {
                return _dhcpaddrs;
            }
        }
        public System.Net.IPAddress[] GATEWAY{
            get
            {
                return _gatewayaddrs;
            }
        }
        public System.Net.IPAddress[] WINS
        {
            get
            {
                return _winsaddrs;
            }
        }
        #endregion
    }
}