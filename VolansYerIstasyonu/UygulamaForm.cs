using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VolansYerIstasyonu
{
    public partial class UygulamaForm : Form
    {

        private UserControls.uc_AnaSayfa anaSayfaEkrani = new UserControls.uc_AnaSayfa();


        public UygulamaForm()
        {
            InitializeComponent();

            pnl_AnaSayfa.Controls.Add(anaSayfaEkrani);
            anaSayfaEkrani.Dock = DockStyle.Fill;
        }

        private void UygulamaForm_Load(object sender, EventArgs e)
        {

        }
    }
}
