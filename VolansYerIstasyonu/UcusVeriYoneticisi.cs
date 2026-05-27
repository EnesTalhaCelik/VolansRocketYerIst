using System;
using System.IO;
using System.Windows.Forms;

namespace VolansYerIstasyonu
{
    /// <summary>
    /// Uçuş verilerinin saklandığı yerel klasörü yönetir.
    /// Klasör, çalıştırılan .exe dosyasının HEMEN YANINDA "UcusVeri" adıyla
    /// bulunur. Bu sayede program farklı bir bilgisayara veya sürücüye
    /// (C:, D:, USB, masaüstü vb.) taşındığında yine çalışır.
    ///
    /// NOT: Program "C:\Program Files\" altına kurulursa Windows yazma izni
    /// vermez. Programı kullanıcıya açık bir klasörden çalıştırın
    /// (örn. Masaüstü, Belgeler, USB).
    /// </summary>
    public static class UcusVeriYoneticisi
    {
        public const string KlasorAdi = "UcusVeri";

        /// <summary>
        /// Uçuş verisi klasörünün tam yolu. exe'nin bulunduğu klasörün altında.
        /// </summary>
        public static string KlasorYolu
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, KlasorAdi);
            }
        }

        /// <summary>
        /// Program açılışında ÇAĞRILMASI ZORUNLU.
        /// Klasör yoksa oluşturur ve kullanıcıyı uyarır.
        /// </summary>
        public static void BaslangicKontrolu()
        {
            try
            {
                if (!Directory.Exists(KlasorYolu))
                {
                    Directory.CreateDirectory(KlasorYolu);

                    MessageBox.Show(
                        "Uçuş verisi klasörü bulunamadı, yeni bir tane oluşturuldu:" +
                        Environment.NewLine + Environment.NewLine +
                        KlasorYolu + Environment.NewLine + Environment.NewLine +
                        "Önceki uçuş verilerinizi bu klasöre kopyalamanız gerekiyor. " +
                        "Aksi takdirde 'Veritabanı Seçiniz' listesinde eski dosyalar görünmeyecektir.",
                        "Uçuş Verisi Klasörü Oluşturuldu",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(
                    "Uçuş verisi klasörü oluşturulamadı: Yazma izni reddedildi." +
                    Environment.NewLine + Environment.NewLine +
                    "Hedef klasör: " + KlasorYolu + Environment.NewLine + Environment.NewLine +
                    "Program büyük olasılıkla 'Program Files' gibi izinli bir klasörden " +
                    "çalışıyor. Lütfen exe dosyasını Masaüstü veya Belgeler altına taşıyıp " +
                    "tekrar çalıştırın.",
                    "Yazma İzni Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Uçuş verisi klasörü oluşturulurken beklenmedik bir hata oluştu:" +
                    Environment.NewLine + ex.Message + Environment.NewLine + Environment.NewLine +
                    "Veri kayıt işlemleri çalışmayabilir.",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// UcusVeri klasörü altında verilen dosya adına tam yol döner.
        /// Klasör yoksa otomatik oluşturur (kullanıcı silmiş olabilir).
        /// </summary>
        public static string DosyaYolu(string dosyaAdi)
        {
            EnsureKlasorVar();
            return Path.Combine(KlasorYolu, dosyaAdi);
        }

        /// <summary>
        /// UcusVeri klasörü altındaki dosyaları belirli bir pattern ile listeler.
        /// </summary>
        public static string[] DosyalariListele(string searchPattern)
        {
            EnsureKlasorVar();
            return Directory.GetFiles(KlasorYolu, searchPattern);
        }

        /// <summary>
        /// Klasör var olduğundan emin olur. Silinmişse tekrar oluşturur.
        /// Sessiz çalışır - uyarı vermez (uyarı sadece program başlangıcında verilir).
        /// </summary>
        private static void EnsureKlasorVar()
        {
            if (!Directory.Exists(KlasorYolu))
            {
                Directory.CreateDirectory(KlasorYolu);
            }
        }
    }
}
