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
    public partial class frmResearchIPAddr : frmBase
    {
        #region member
        private LibPcapLiveDevice _capturePcapLiveDevice;
        private IPAddress _ipAddr;
        #endregion

        #region constructor
        public frmResearchIPAddr(LibPcapLiveDevice capturePcapLiveDevice, IPAddress ipAddr)
        {
            InitializeComponent();
            _capturePcapLiveDevice = capturePcapLiveDevice;
            _ipAddr = ipAddr;

        }
        #endregion
    }
}
