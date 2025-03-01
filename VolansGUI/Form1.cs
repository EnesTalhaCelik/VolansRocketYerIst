using MaterialSkin;
using MaterialSkin.Controls;
using System.Drawing;

namespace VolansGUI
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = false;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                ColorTranslator.FromHtml("#212121"), // action bar, deðiþken olmayan labellar.
                ColorTranslator.FromHtml("#171717"), // durum çubuðu.
                ColorTranslator.FromHtml("#0f0f0f"), // tabpage arkaplan rengi.
                ColorTranslator.FromHtml("#000000"), // tabpage yazýlarý, butonalar, deðiþken labellar.
                TextShade.WHITE
            );
            this.Size = new Size(1610, 970);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void materialButton4_Click(object sender, EventArgs e)
        {
            // Temayý deðiþtirme
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Renk þemasýný deðiþtirme
            materialSkinManager.ColorScheme = new ColorScheme(
                ColorTranslator.FromHtml("#212121"), // action bar, deðiþken olmayan labellar.
                ColorTranslator.FromHtml("#171717"), // durum çubuðu.
                ColorTranslator.FromHtml("#0f0f0f"), // tabpage arkaplan rengi.
                ColorTranslator.FromHtml("#000000"), // tabpage yazýlarý, butonalar, deðiþken labellar.
                TextShade.WHITE
            );
        }
        private void materialButton5_Click(object sender, EventArgs e)
        {
            // Temayý deðiþtirme
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;

            // Renk þemasýný deðiþtirme
            materialSkinManager.ColorScheme = new ColorScheme(
                ColorTranslator.FromHtml("#212121"), // action bar, deðiþken olmayan labellar.
                ColorTranslator.FromHtml("#171717"), // durum çubuðu.
                ColorTranslator.FromHtml("#0f0f0f"), // tabpage arkaplan rengi.
                ColorTranslator.FromHtml("#ffffff"), // tabpage yazýlarý, butonalar, deðiþken labellar.
                TextShade.WHITE
            );
        }

        private void materialLabel67_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel68_Click(object sender, EventArgs e)
        {
        }
    }
}
