using BUSLayer;
using Model;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QuanLyContact
{
    public partial class FormAddEditContact : Form
    { public contact EditedContact { get; private set; }
        bool IsNew;
        private BindingSource contactBindingSource;
        public FormAddEditContact(contact obj)
        {
            contactBindingSource = new BindingSource();

            InitializeComponent();
            if (obj == null)
            {
                contactBindingSource.DataSource = new contact();
                IsNew = true;
            }
            else
            {
                contactBindingSource.DataSource = obj;
                IsNew = false;
            }
        }

        private void FormAddEditContact_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (DialogResult == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Hãy nhập vào tên!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    e.Cancel = true;
                    return;
                }

                contact ct = contactBindingSource.Current as contact;
                if (ct != null)
                {
                    ct.ContactName = txtName.Text;
                    ct.PhoneNumber = txtPhone.Text;
                    ct.Email = txtEmail.Text;
                    ct.Address = txtAddress.Text;

                    if (IsNew)
                    {
                        ContactServices.Insert(ct);
                    }
                    else
                    {
                        ContactServices.Update(ct);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            contact ct = contactBindingSource.Current as contact;
            if (ct != null)
            {
                ct.ContactName = txtName.Text;
                ct.PhoneNumber = txtPhone.Text;
                ct.Email = txtEmail.Text;
                ct.Address = txtAddress.Text;

                if (IsNew)
                {
                    ContactServices.Insert(ct);
                }
                else
                {
                    ContactServices.Update(ct);
                }
            }
            contactBindingSource.RemoveCurrent();
            DialogResult = DialogResult.OK;
        }
        public void LoadContactData(contact contact)
        {
            txtName.Text = contact.ContactName;
            txtPhone.Text = contact.PhoneNumber;
            txtEmail.Text = contact.Email;
            txtAddress.Text = contact.Address;
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)  && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
            if (txtPhone.Text.Length > 13)
            {
                MessageBox.Show("Chỉ được 13 số thôi!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPhone.Text = string.Empty;
            }
        }

        private void txtEmail_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string email = txtEmail.Text;

            if (!IsValidEmail(email))
            {

                errorProvider.SetError(txtEmail, "Địa chỉ email không hợp lệ. Vui lòng nhập theo mẫu: ten1@gmail.com");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(txtEmail, "");
            }
        }
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9]+[a-zA-Z0-9._]*[0-9]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)+$";
            return Regex.IsMatch(email, pattern);
        }

    }
}
