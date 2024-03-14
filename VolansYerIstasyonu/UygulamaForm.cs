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
        private UserControls.uc_PortAyarlari portAyarEkrani = new UserControls.uc_PortAyarlari();
        private UserControls.uc_VerilerinGrafikleri veriGrafikEkrani = new UserControls.uc_VerilerinGrafikleri();
        public UygulamaForm()
        {
            InitializeComponent();

            pnl_AnaSayfa.Controls.Add(anaSayfaEkrani);
            pnl_AnaSayfa.Controls.Add(portAyarEkrani);
            pnl_AnaSayfa.Controls.Add(veriGrafikEkrani);
            anaSayfaEkrani.Dock = DockStyle.Fill;


        }

        

        private void UygulamaForm_Load(object sender, EventArgs e)
        {

        }

        private void btn_PrtAyar_Click(object sender, EventArgs e)
        {
            portAyarEkrani.BringToFront();
        }

        private void btn_yerIst_Click(object sender, EventArgs e)
        {
            anaSayfaEkrani.BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            veriGrafikEkrani.BringToFront();
        }
    }
}
