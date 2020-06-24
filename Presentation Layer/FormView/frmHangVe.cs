﻿using Business_Logic_Layer;
using Data_Transfer_Objects;
using Guna.UI2.WinForms.Helpers;
using Presentation_Layer.FormDigital;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation_Layer.FormView
{
    public partial class frmHangVe : CustomForm
    {

        private List<HangVe> hangVes;
        private SortableBindingList<HangVe> bl;
        private Color lastColor;
        private Actions action;
        public frmHangVe()
        {
            InitializeComponent();
        }
        #region Other functions
        private void UpdateHeightDgv()
        {
            pnContain.Height = Math.Min(40 + 24 * dgvHangVe.RowCount, Height - 38 - 50);
        }
        private DataGridViewButtonColumn ButtonColumn(string text)
        {
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.Name = text;
            btn.Text = text;
            btn.HeaderText = "";
            btn.UseColumnTextForButtonValue = true;
            btn.FlatStyle = FlatStyle.Flat;
            btn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            btn.Width = 50;
            return btn;
        }
        private void CustomDgv()
        {
            dgvHangVe.Columns["MaHV"].Visible = false;
            dgvHangVe.Columns["strMaHV"].HeaderText = "Mã";
            dgvHangVe.Columns["strMaHV"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHangVe.Columns["strMaHV"].Width = 100;

            dgvHangVe.Columns["TenHV"].HeaderText = "Tên Sân Bay";
            dgvHangVe.Columns["TiLe"].HeaderText = "Tỉ Lệ";
        }

        private List<CBSource> sources()
        {
            var res = new List<CBSource>();
            CBSource i = new CBSource("MaHV", "Mã");
            res.Add(i);
            i = new CBSource("TenHV", "Tên Hạng Vé");
            res.Add(i);
            return res;
        }
        #endregion
        #region Chỉnh sửa hiển thị của dgvHangVe
        private void dgvHangVe_DataSourceChanged(object sender, EventArgs e)
        {
            UpdateHeightDgv();
        }

        private void dgvHangVe_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0) return;
            lastColor = dgvHangVe.Rows[e.RowIndex].DefaultCellStyle.BackColor;
            dgvHangVe.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(99, 191, 173);
        }

        private void dgvHangVe_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) dgvHangVe.Rows[e.RowIndex].DefaultCellStyle.BackColor = lastColor;
        }

        private void dgvHangVe_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) dgvHangVe.Rows[e.RowIndex].Selected = true;
        }

        private void frmHangVe_SizeChanged(object sender, EventArgs e)
        {
            UpdateHeightDgv();
        }
        private void dgvHangVe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHangVe.SelectedRows.Count > 0)
            {
                if (action != Actions.NOTHING)
                {
                    var dialog = new frmWarning("Cảnh Báo", "Bạn có muốn hủy bỏ chỉnh sửa?");
                    DialogResult res = dialog.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        DisablePanelEdit();
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                tbMaHV.Text = dgvHangVe.SelectedRows[0].Cells["strMaHV"].Value.ToString();
                tbTenHV.Text = dgvHangVe.SelectedRows[0].Cells["TenHV"].Value.ToString();
                tbTiLe.Text = dgvHangVe.SelectedRows[0].Cells["TiLe"].Value.ToString();
            }
        }
        #endregion
        private void frmHangVe_Load(object sender, EventArgs e)
        {
            cbSearch.DataSource = sources();
            cbSearch.DisplayMember = "Name";
            cbSearch.ValueMember = "ID";

            hangVes = BLL_HangVe.GetHangVes();
            bl = new SortableBindingList<HangVe>(hangVes);
            dgvHangVe.DataSource = bl;
            CustomDgv();

            action = Actions.NOTHING;
        }

        public override void RefreshData()
        {
            hangVes = BLL_HangVe.GetHangVes();
            bl = new SortableBindingList<HangVe>(hangVes);
            dgvHangVe.DataSource = bl;
            Notification.Show("Làm mới danh sách Hạng vé");
        }
        public override void SizeChange()
        {
            UpdateHeightDgv();
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cbSearch.SelectedValue.ToString() == "MaHV") hangVes = BLL_HangVe.SearchMaHV(tbSearch.Text);
            else if (cbSearch.SelectedValue.ToString() == "TenHV") hangVes = BLL_HangVe.SearchTenHV(tbSearch.Text);
            bl = new SortableBindingList<HangVe>(hangVes);
            dgvHangVe.DataSource = bl;
        }
        #region panel Edit
        private void ActivePanelEdit(Actions x)
        {
            action = x;
            tbTenHV.Enabled = true;
            btnAdd.Text = "OK";
            btnEdit.Text = "Cancel";
        }
        private void DisablePanelEdit()
        {
            action = Actions.NOTHING;
            tbTenHV.Enabled = false;
            btnAdd.Text = "Add";
            btnEdit.Text = "Edit";
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (action == Actions.NOTHING)
            {
                ActivePanelEdit(Actions.ADD);
            }
            else if (action == Actions.ADD)
            {
                Notification.Show("Add");
                DisablePanelEdit();
            }
            else if (action == Actions.EDIT)
            {
                Notification.Show("Edit");
                DisablePanelEdit();
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (action == Actions.NOTHING)
            {
                ActivePanelEdit(Actions.EDIT);
            }
            else
            {
                DisablePanelEdit();
            }
        }
        #endregion
    }
}
