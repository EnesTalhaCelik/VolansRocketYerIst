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
        // ÖNEMLİ: Bu UserControl alanları artık burada DEĞİL constructor içinde
        // oluşturuluyor. Çünkü uc_VeriTablo constructor'ı UcusVeri klasörünün
        // varlığını gerektiriyor. Bu yüzden önce BaslangicKontrolu() çağrılmalı,
        // SONRA UserControl'ler oluşturulmalı.
        private UserControls.uc_AnaSayfa anaSayfaEkrani;
        public static UserControls.uc_PortAyarlari portAyarEkrani;
        private UserControls.uc_VerilerinGrafikleri veriGrafikEkrani;
        private UserControls.uc_AnalizTablo veriAnalizTabloEkrani;
        private UserControls.uc_VeriTablo veriVeriTabloEkrani;

        public UygulamaForm()
        {
            InitializeComponent();

            // ----- ÖNCELİKLE veri klasörünü hazırla -----
            // Bu çağrı, herhangi bir UserControl oluşturulmadan ÖNCE yapılmalı.
            // Aksi takdirde uc_VeriTablo constructor'ı klasör yoksa hata verir.
            UcusVeriYoneticisi.BaslangicKontrolu();

            // ----- Şimdi UserControl'leri oluştur -----
            anaSayfaEkrani = new UserControls.uc_AnaSayfa();
            portAyarEkrani = new UserControls.uc_PortAyarlari();
            veriGrafikEkrani = new UserControls.uc_VerilerinGrafikleri();
            veriAnalizTabloEkrani = new UserControls.uc_AnalizTablo();
            veriVeriTabloEkrani = new UserControls.uc_VeriTablo();

            pnl_AnaSayfa.Controls.Add(veriVeriTabloEkrani);
            pnl_AnaSayfa.Controls.Add(veriAnalizTabloEkrani);
            pnl_AnaSayfa.Controls.Add(anaSayfaEkrani);
            pnl_AnaSayfa.Controls.Add(portAyarEkrani);
            pnl_AnaSayfa.Controls.Add(veriGrafikEkrani);

            anaSayfaEkrani.Dock = DockStyle.Fill;
            anaSayfaEkrani.BringToFront();

            TeknofestVeriler.Veriler.GorevYukuBoylam = 31.524978876f;
            TeknofestVeriler.Veriler.GorevYukuEnlem = 40.716131152f;
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

        private void btn_AnalizTablo_Click(object sender, EventArgs e)
        {
            veriVeriTabloEkrani.BringToFront();
        }
    }
}
