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
    public partial class frmPwInput : Form
    {
        string _title;
        string _prompt;
        DialogResult _dialogResult;

        #region DialogResult
        internal DialogResult Result
        {
            get
            {
                return _dialogResult;
            }
        }
        #endregion

        #region Password
        internal string Password
        {
            get
            {
                return txtPw.Text;
            }
        }
        #endregion

        public frmPwInput(string title , string prompt)
        {
            InitializeComponent();
            _title = title;
            _prompt = prompt;
        }

        #region load
        private void frmPwInput_Load(object sender, EventArgs e)
        {
            this.Text = _title;
            lblPrompt.Text = _prompt;
            txtPw.Text = "yes";
            _dialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        #endregion

        #region OK
        private void btnOK_Click(object sender, EventArgs e)
        {
            this._dialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        #endregion

        #region Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._dialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        #endregion



    }
}
