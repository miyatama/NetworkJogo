using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.AirPcap;
using SharpPcap.WinPcap;

namespace NetworkJogo
{
    public partial class fmPacketCapture : frmBase
    {
        #region member
        private IPAddress _captureAddr;
        private LibPcapLiveDevice _capturePcapLiveDevice;
        private PcapAddress _capturePcapAddr;
        private bool _captureEnable;
        private dsNetworkInfo.CaptureListDataTable _dtCaptureBuffer;
        private bool _captureRunning;
        #endregion

        #region コンストラクタ
        public fmPacketCapture(IPAddress captureAddr)
        {
            InitializeComponent();
            _captureAddr = captureAddr;

            _capturePcapAddr = null;
            _capturePcapLiveDevice = null;
            _dtCaptureBuffer = new dsNetworkInfo.CaptureListDataTable();
            foreach (LibPcapLiveDevice device in LibPcapLiveDeviceList.Instance)
            {
                foreach (PcapAddress addr in device.Interface.Addresses)
                {
                    if( !addr.Addr.ipAddress.ToString().Equals(_captureAddr.ToString()))
                    {
                        continue;
                    }
                    _capturePcapAddr = addr;
                    _capturePcapLiveDevice = device;
                    break;
                }
                if (_capturePcapAddr != null)
                {
                    break;
                }
            }

            if (_capturePcapAddr == null)
            {
                showMessage("dapture device not found!");
            }

            dgvList.AutoGenerateColumns = false;
            dsNetworkInfo.CaptureListDataTable dt = new dsNetworkInfo.CaptureListDataTable();
            dgvList.DataSource = dt;

        }
        #endregion

        #region load
        private void fmPacketCapture_Load(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                _captureEnable = false;
                showCaptureEnable();

            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region Start Capture
        private void startCapture()
        {
            _captureRunning = false;
            _dtCaptureBuffer.Clear();
            _capturePcapLiveDevice.OnPacketArrival += OnPacketArrival;
            int readTimeoutMilliseconds = 1000;
            _capturePcapLiveDevice.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
            _capturePcapLiveDevice.StartCapture();
            tmrCaptureBuffer.Enabled = true;
        }
        #endregion

        #region Stop Capture
        private void stopCapture()
        {
            tmrCaptureBuffer.Enabled = false;
            _capturePcapLiveDevice.StopCapture();
            _capturePcapLiveDevice.Close();
        }
        #endregion

        #region キャプチャイベント
        private void OnPacketArrival(object sender, CaptureEventArgs e)
        {
            _captureRunning = true;
            DateTime now = DateTime.Now;
            int len = e.Packet.Data.Length;
            switch (e.Packet.LinkLayerType)
            {
                case PacketDotNet.LinkLayers.Ethernet:
                    captureEthernetPacket(now,e);
                    break;

                default:
                    dsNetworkInfo.CaptureListRow row = _dtCaptureBuffer.NewCaptureListRow();
                    row.CAPTURE_DATE = now;
                    row.PACKET_LENGTH = len;
                    row.LINKLAYERTYPE = e.Packet.LinkLayerType.ToString();
                    row.DATA = System.Text.Encoding.UTF8.GetString(e.Packet.Data);
                    row.IS_ANALYSIS_ERROR = false;
                    _dtCaptureBuffer.AddCaptureListRow(row);
                    break;
            }
            _captureRunning = false;
        }
        #endregion

        #region Ethernetキャプチャ
        private void captureEthernetPacket(DateTime now , CaptureEventArgs e)
        {
            PacketDotNet.Packet packet = null;
            PacketDotNet.EthernetPacket ethPacket = null;
            try
            {
                packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
                ethPacket = (PacketDotNet.EthernetPacket)packet;
                
                switch(ethPacket.Type){
                    case PacketDotNet.EthernetPacketType.IpV4:
                        captureEthernetIPv4Packet(
                            now
                            , (PacketDotNet.IPv4Packet)ethPacket.PayloadPacket
                            , e.Packet.Data.Length
                            , e.Packet.LinkLayerType.ToString()
                            );
                        break;

                    case PacketDotNet.EthernetPacketType.IpV6:
                        {
                            dsNetworkInfo.CaptureListRow row = _dtCaptureBuffer.NewCaptureListRow();
                            row.CAPTURE_DATE = now;
                            row.PACKET_LENGTH = e.Packet.Data.Length;
                            row.LINKLAYERTYPE = "ethernet.ipv6";
                            row.DATA = System.Text.Encoding.UTF8.GetString(e.Packet.Data);
                            row.IS_ANALYSIS_ERROR = true;
                            row.ANALYSIS_ERROR_MSG = "IPv6 Packet";
                            _dtCaptureBuffer.AddCaptureListRow(row);
                        }
                        break;

                    case PacketDotNet.EthernetPacketType.Arp:
                        captureArpPacket(
                            now
                            , (PacketDotNet.ARPPacket)ethPacket.PayloadPacket
                            , e.Packet.Data.Length
                            , e.Packet.LinkLayerType.ToString()
                            );
                        break;


                    default:
                        {
                            dsNetworkInfo.CaptureListRow row = _dtCaptureBuffer.NewCaptureListRow();
                            row.CAPTURE_DATE = now;
                            row.PACKET_LENGTH = e.Packet.Data.Length;
                            row.LINKLAYERTYPE = e.Packet.LinkLayerType.ToString();
                            row.DATA = System.Text.Encoding.UTF8.GetString(e.Packet.Data);
                            row.IS_ANALYSIS_ERROR = true;
                            row.ANALYSIS_ERROR_MSG = string.Format("Unknown ethernet type : {0}", ethPacket.Type.ToString());
                            _dtCaptureBuffer.AddCaptureListRow(row);
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                {
                    dsNetworkInfo.CaptureListRow row = _dtCaptureBuffer.NewCaptureListRow();
                    row.CAPTURE_DATE = now;
                    row.PACKET_LENGTH = e.Packet.Data.Length;
                    row.LINKLAYERTYPE = e.Packet.LinkLayerType.ToString();
                    row.DATA = System.Text.Encoding.UTF8.GetString(e.Packet.Data);
                    row.IS_ANALYSIS_ERROR = true;
                    row.ANALYSIS_ERROR_MSG = ex.ToString();
                    _dtCaptureBuffer.AddCaptureListRow(row);
                }
                return;
            }
        }
        #endregion

        #region IPv4キャプチャ
        private void captureEthernetIPv4Packet(DateTime now, PacketDotNet.IPv4Packet packet, int len , string linkLayerType)
        {
            if (packet.PayloadPacket.GetType().Equals(typeof(PacketDotNet.TcpPacket)))
            {
                captureEthernetIPv4TcpPacket(now, packet, (PacketDotNet.TcpPacket)packet.PayloadPacket, len, linkLayerType);
            }
            else if (packet.PayloadPacket.GetType().Equals(typeof(PacketDotNet.UdpPacket)))
            {
                captureEthernetIPv4UdpPacket(now, packet, (PacketDotNet.UdpPacket)packet.PayloadPacket, len, linkLayerType);
            }
            else
            {
                dsNetworkInfo.CaptureListRow row = _dtCaptureBuffer.NewCaptureListRow();
                row.CAPTURE_DATE = now;
                row.PACKET_LENGTH = len;
                row.LINKLAYERTYPE = linkLayerType;
                row.DATA = System.Text.Encoding.UTF8.GetString(packet.PayloadData);
                row.IS_ANALYSIS_ERROR = true;
                row.ANALYSIS_ERROR_MSG = string.Format("Unknown IP payload type : {0}",packet.PayloadPacket.GetType().ToString());
                _dtCaptureBuffer.AddCaptureListRow(row);
            }            
        }
        #endregion

        #region IPv4TCPキャプチャ
        private void captureEthernetIPv4TcpPacket(DateTime now, PacketDotNet.IPv4Packet v4packet , PacketDotNet.TcpPacket packet, int len, string linkLayerType)
        {
            int sourcePort = packet.SourcePort;
            int destPort = packet.DestinationPort;
            string sourceIP = v4packet.SourceAddress.ToString();
            string destIP = v4packet.DestinationAddress.ToString();
            int id = v4packet.Id;
            int fragment = v4packet.FragmentFlags;
            byte[] data = packet.PayloadData;
            string msg = string.Format("source : {0}:{1} , destination : {2}:{3}",sourceIP,sourcePort,destIP,destPort);
            dsNetworkInfo.CaptureListRow row = _dtCaptureBuffer.NewCaptureListRow();
            row.CAPTURE_DATE = now;
            row.PACKET_LENGTH = len;
            row.LINKLAYERTYPE = "ethernet.ipv4.tcp";
            row.DATA = msg;
            row.IS_ANALYSIS_ERROR = false;
            row.ANALYSIS_ERROR_MSG = string.Empty;
            row.SOURCE_ADDR = sourceIP;
            row.SOURCE_PORT = sourcePort;
            row.DESTINATION_ADDR = destIP;
            row.DESTINATION_PORT = destPort;
            row.ID = id;
            row.FRAGMENT_FLG = fragment;
            row.PAYLOAD_DATA = data;
            _dtCaptureBuffer.AddCaptureListRow(row);
        }
        #endregion

        #region IPv4UDPキャプチャ
        private void captureEthernetIPv4UdpPacket(DateTime now, PacketDotNet.IPv4Packet v4packet, PacketDotNet.UdpPacket packet, int len, string linkLayerType)
        {
            int sourcePort = packet.SourcePort;
            int destPort = packet.DestinationPort;
            string sourceIP = v4packet.SourceAddress.ToString();
            string destIP = v4packet.DestinationAddress.ToString();
            int id = v4packet.Id;
            int fragment = v4packet.FragmentFlags;
            byte[] data = packet.PayloadData;
            string msg = string.Format("source : {0}:{1} , destination : {2}:{3}", sourceIP, sourcePort, destIP, destPort);
            dsNetworkInfo.CaptureListRow row = _dtCaptureBuffer.NewCaptureListRow();
            row.CAPTURE_DATE = now;
            row.PACKET_LENGTH = len;
            row.LINKLAYERTYPE = "ethernet.ipv4.udp";
            row.DATA = msg;
            row.IS_ANALYSIS_ERROR = false;
            row.ANALYSIS_ERROR_MSG = string.Empty;
            row.SOURCE_ADDR = sourceIP;
            row.SOURCE_PORT = sourcePort;
            row.DESTINATION_ADDR = destIP;
            row.DESTINATION_PORT = destPort;
            row.ID = id;
            row.FRAGMENT_FLG = fragment;
            row.PAYLOAD_DATA = data;
            _dtCaptureBuffer.AddCaptureListRow(row);
        }
        #endregion

        #region ARPキャプチャ
        private void captureArpPacket(DateTime now, PacketDotNet.ARPPacket packet, int len, string linkLayerType)
        {
            dsNetworkInfo.CaptureListRow row = _dtCaptureBuffer.NewCaptureListRow();
            row.CAPTURE_DATE = now;
            row.PACKET_LENGTH = len;
            row.LINKLAYERTYPE = "ethernet.arp";
            row.DATA = string.Format(
                "sender : {0}({1}) , target : {2}({3})"
                , packet.SenderProtocolAddress.ToString()
                ,packet.SenderHardwareAddress.ToString()
                ,packet.TargetProtocolAddress.ToString()
                ,packet.TargetHardwareAddress.ToString());
            row.IS_ANALYSIS_ERROR = false;
            _dtCaptureBuffer.AddCaptureListRow(row);
        }
        #endregion

        #region キャプチャ開始
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                _captureEnable = true;
                showCaptureEnable();
                startCapture();
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region キャプチャ開始設定
        private void showCaptureEnable()
        {
            if (_captureEnable)
            {
                btnStop.Enabled = true;
                btnStart.Enabled = false;
            }
            else
            {
                btnStop.Enabled = false;
                btnStart.Enabled = true;
            }
        }
        #endregion

        #region キャプチャ終了
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                _captureEnable = false;
                showCaptureEnable();
                stopCapture();
                dsNetworkInfo.CaptureListDataTable dt = (dsNetworkInfo.CaptureListDataTable)dgvList.DataSource;
                dt.Merge(_dtCaptureBuffer);
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region キャプチャバッファセット
        private void tmrCaptureBuffer_Tick(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                if (_captureRunning) return;
                try
                {
                    dsNetworkInfo.CaptureListDataTable dt = (dsNetworkInfo.CaptureListDataTable)dgvList.DataSource;
                    dt.Merge(_dtCaptureBuffer);
                    _dtCaptureBuffer.Clear();
                }
                catch (Exception ex)
                {
                }
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region フォーム終了時
        private void fmPacketCapture_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                if (_captureEnable)
                {
                    stopCapture();
                }
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion
    }
}
