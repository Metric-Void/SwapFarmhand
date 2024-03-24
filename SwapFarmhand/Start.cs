namespace SwapFarmhand
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
        }

        private void btnExport_onclick(object sender, EventArgs e)
        {
            Export frmExport = new Export();
            frmExport.Show();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            Import frmImport = new Import();
            frmImport.Show();
        }
    }
}