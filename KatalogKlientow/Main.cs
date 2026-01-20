using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using KatalogKlientow.Models;
using KatalogKlientow.Services;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace KatalogKlientow
{
    public partial class ClientCatalogForm : Form
    {
        public readonly IClientService _clientService;
        public readonly IModelValidator _modelValidator;

        public ClientCatalogForm(IClientService clientService, IModelValidator modelValidator)
        {
            InitializeComponent();

            _modelValidator = modelValidator;
            _clientService = clientService;

            ConfigureGridView();
            clientGrid.DataSource = new BindingList<Client>(_clientService.GetAllClients());

            clientGridView.RowDeleted += KlientGridView_RowDeleted;
            clientGridView.KeyDown += KlientGridView_KeyDown;
            clientGridView.ValidateRow += KlientGridView_ValidateRow;
            clientGridView.InvalidRowException += KlientGridView_InvalidRowException;
            clientGridView.RowUpdated += KlientGridView_RowUpdated;
        }

        private void KlientGridView_RowUpdated(object sender, RowObjectEventArgs e)
        {
            var client = (Client)e.Row;
            var refreshData = _clientService.AddOrUpdateClient(client);
            clientGrid.DataSource = new BindingList<Client>(refreshData);
        }

        private void KlientGridView_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }

        private void KlientGridView_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            if (e.Row is Client klient)
            {
                var errors = _modelValidator.Validate(klient);

                e.Valid = errors.Count == 0;

                clientGridView.ClearColumnErrors();

                foreach (var err in errors)
                {
                    foreach (var member in err.MemberNames)
                    {
                        var col = clientGridView.Columns[member];
                        if (col != null)
                            clientGridView.SetColumnError(col, err.ErrorMessage);
                    }
                }
            }
        }

        private void KlientGridView_RowDeleted(object sender, RowDeletedEventArgs e)
        {
            var id = ((Client)e.Row).Id;
            var refreshedData = _clientService.DeleteClient(id);
            clientGrid.DataSource = new BindingList<Client>(refreshedData);
        }

        private void KlientGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Czy na pewno chesz usunąć wiersz?", "Usuń wiersz",
                    MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }

                clientGridView.DeleteRow(clientGridView.FocusedRowHandle);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConfigureGridView()
        {
            clientGridView.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditForm;
            clientGridView.OptionsBehavior.Editable = true;
            clientGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            clientGridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            clientGridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            clientGridView.OptionsView.ShowAutoFilterRow = true;
            clientGridView.OptionsView.ShowGroupPanel = false;
            clientGridView.OptionsCustomization.AllowGroup = false;
            clientGridView.OptionsFilter.ColumnFilterPopupMode = DevExpress.XtraGrid.Columns.ColumnFilterPopupMode.Excel;
        }
    }
}
