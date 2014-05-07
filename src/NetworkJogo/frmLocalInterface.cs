using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Text;

namespace NetworkJogo
{
    partial class frmLocalInterface : frmBase
    {
        #region コンストラクタ
        public frmLocalInterface()
        {
            InitializeComponent();
        }
        #endregion

        #region OKボタン
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                this.Close();
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region 再取得
        private void btnReflesh_Click(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                Utility.gatherLocalNetworkInfo();
                showLocalNetworkInfo();
            }
            finally
            {
                quitEvent();
            }
        }
        #endregion

        #region ネットワーク情報表示
        private void showLocalNetworkInfo()
        {
            StringBuilder sbInfo = new StringBuilder();
            sbInfo.AppendLine("ネットワーク情報");
            string sepalator = "--------------------------------------------";
            LocalNetworkInfo info = Utility.LocalNetworkInfo;
            if (info.IPADDRS != null && info.IPADDRS.Length > 0)
            {
                sbInfo.AppendLine("IP");
                for (int i = 0; i < info.IPADDRS.Length; i++)
                {
                    sbInfo.AppendLine(string.Format("{0} : {1} , {2}", (i+1),info.IPADDRS[i].ToString(), info.NETMASKS[i].ToString()));
                }
            }

            if (info.DNS != null && info.DNS.Length > 0)
            {
                sbInfo.AppendLine(sepalator);
                sbInfo.AppendLine("DNS");
                for (int i = 0; i < info.DNS.Length; i++)
                {
                    sbInfo.AppendLine(string.Format("{0} : {1}", (i+1), info.DNS[i].ToString()));
                }
            }

            if (info.DHCP != null && info.DHCP.Length > 0)
            {
                sbInfo.AppendLine(sepalator);
                sbInfo.AppendLine("DHCP");
                for (int i = 0; i < info.DHCP.Length; i++)
                {
                    sbInfo.AppendLine(string.Format("{0} : {1}", (i + 1), info.DHCP[i].ToString()));
                }
            }

            if (info.GATEWAY != null && info.GATEWAY.Length > 0)
            {
                sbInfo.AppendLine(sepalator);
                sbInfo.AppendLine("GATEWAY");
                for (int i = 0; i < info.GATEWAY.Length; i++)
                {
                    sbInfo.AppendLine(string.Format("{0} : {1}", (i + 1), info.GATEWAY[i].ToString()));
                }
            }
            if (info.WINS != null && info.WINS.Length > 0)
            {
                sbInfo.AppendLine(sepalator);
                sbInfo.AppendLine("WINS");
                for (int i = 0; i < info.WINS.Length; i++)
                {
                    sbInfo.AppendLine(string.Format("{0} : {1}", (i + 1), info.WINS[i].ToString()));
                }
            }

            textBoxDescription.Text = sbInfo.ToString();
        }
        #endregion

        #region load
        private void frmLocalInterface_Load(object sender, EventArgs e)
        {
            if (isRunningEvent()) return;
            try
            {
                showLocalNetworkInfo();
            }
            finally
            {
                quitEvent();
            }
            
        }
        #endregion
    }
}
