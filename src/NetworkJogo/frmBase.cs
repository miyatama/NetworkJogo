using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetworkJogo
{
    public partial class frmBase : Form
    {
        #region member
        protected bool _eventRunning;
        #endregion


        #region コンストラクタ
        public frmBase()
        {
        }
        #endregion

        #region イベント制御
        protected bool isRunningEvent()
        {
            if (_eventRunning) return true;
            _eventRunning = true;
            this.Cursor = Cursors.WaitCursor;
            return false;
        }

        protected void quitEvent()
        {
            _eventRunning = false;
            this.Cursor = Cursors.Default;
        }
        #endregion

        #region mostrar mensagem
        protected DialogResult showMessage(string text)
        {
            return this.showMessage(text, MessageBoxButtons.OK);
        }
        protected DialogResult showMessage(string text, MessageBoxButtons buttons)
        {
            return MessageBox.Show(text, "Network jogo", buttons);
        }
        #endregion
    }
}
