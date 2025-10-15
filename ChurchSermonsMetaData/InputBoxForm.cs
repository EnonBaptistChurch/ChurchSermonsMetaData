using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChurchSermonsMetaData
{
    public partial class InputBoxForm : Form
    {
        public string InputText => txtInput.Text;

        public InputBoxForm(string title, string prompt)
        {
            InitializeComponent(); // calls the setup method below
            this.Text = title;
            lblPrompt!.Text = prompt;
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

    // 👇 This is the designer code (Step 2)
    public partial class InputBoxForm
    {
        private readonly Label lblPrompt = new ();
        private readonly TextBox txtInput = new ();
        private readonly Button btnOK = new ();
        private readonly Button btnCancel = new ();

        private void InitializeComponent()
        {
            

            this.SuspendLayout();

            // lblPrompt
            lblPrompt.AutoSize = true;
            lblPrompt.Location = new System.Drawing.Point(12, 9);
            lblPrompt.Text = "Prompt text";

            // txtInput
            txtInput.Location = new System.Drawing.Point(15, 35);
            txtInput.Width = 260;

            // btnOK
            btnOK.Text = "OK";
            btnOK.Location = new System.Drawing.Point(120, 70);
            btnOK.Click += BtnOK_Click;

            // btnCancel
            btnCancel.Text = "Cancel";
            btnCancel.Location = new System.Drawing.Point(200, 70);
            btnCancel.Click += BtnCancel_Click;

            // Form
            this.ClientSize = new System.Drawing.Size(300, 110);
            this.Controls.Add(lblPrompt);
            this.Controls.Add(txtInput);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
