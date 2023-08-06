using System;
using System.Windows.Forms;
using BUSLayer;
using Model;

namespace QuanLyContact
{
    public partial class FormManagement : Form
    {
        private BindingSource contactBindingSource;
        public FormManagement()
        {
            InitializeComponent();
            contactBindingSource = new BindingSource();
            dgvContact.DataSource = contactBindingSource;
        }
        private void FormManagement_Load(object sender, EventArgs e)
        {
            contactBindingSource.DataSource = ContactServices.GetAll();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (FormAddEditContact frm = new FormAddEditContact(null))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    contactBindingSource.DataSource = ContactServices.GetAll();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (contactBindingSource.Current == null)
                return;

            contact selectedContact = contactBindingSource.Current as contact;

            using (FormAddEditContact frm = new FormAddEditContact(selectedContact))
            {
                frm.LoadContactData(selectedContact); // Tải dữ liệu vào các textbox

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    contactBindingSource.DataSource = ContactServices.GetAll();
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (contactBindingSource.Current == null)
                return;
            if (MessageBox.Show("Bạn thực sự có muốn xóa contact này?", "Thông báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ContactServices.Delete(contactBindingSource.Current as contact);
                contactBindingSource.RemoveCurrent();
            }
        }
    }
}
