using System;
using System.IO;
using System.Windows.Forms;

namespace VolansGUI.Telemetry
{
    /// <summary>
    /// Uçuş verilerinin saklandığı yerel klasörü yönetir. Klasör, çalıştırılan
    /// .exe dosyasının yanında "UcusVeri" adıyla bulunur; böylece program farklı
    /// bir bilgisayara/sürücüye taşındığında yine çalışır.
    /// </summary>
    public static class UcusVeriYoneticisi
    {
        public const string KlasorAdi = "UcusVeri";

        /// <summary>Uçuş verisi klasörünün tam yolu (exe'nin yanında).</summary>
        public static string KlasorYolu =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, KlasorAdi);

        /// <summary>Program açılışında çağrılmalı. Klasör yoksa oluşturur, kullanıcıyı uyarır.</summary>
        public static void BaslangicKontrolu()
        {
            try
            {
                if (!Directory.Exists(KlasorYolu))
                {
                    Directory.CreateDirectory(KlasorYolu);
                    MessageBox.Show(
                        "Uçuş verisi klasörü bulunamadı, yeni bir tane oluşturuldu:" +
                        Environment.NewLine + Environment.NewLine + KlasorYolu + Environment.NewLine + Environment.NewLine +
                        "Önceki uçuş verilerinizi bu klasöre kopyalamanız gerekiyor.",
                        "Uçuş Verisi Klasörü Oluşturuldu",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(
                    "Uçuş verisi klasörü oluşturulamadı: Yazma izni reddedildi." + Environment.NewLine +
                    "Hedef klasör: " + KlasorYolu + Environment.NewLine +
                    "Programı Masaüstü veya Belgeler altından çalıştırın.",
                    "Yazma İzni Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Uçuş verisi klasörü oluşturulurken beklenmedik bir hata oluştu:" +
                    Environment.NewLine + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>UcusVeri klasörü altında verilen dosya adına tam yol döner.</summary>
        public static string DosyaYolu(string dosyaAdi)
        {
            EnsureKlasorVar();
            return Path.Combine(KlasorYolu, dosyaAdi);
        }

        /// <summary>UcusVeri klasöründeki dosyaları pattern ile listeler.</summary>
        public static string[] DosyalariListele(string searchPattern)
        {
            EnsureKlasorVar();
            return Directory.GetFiles(KlasorYolu, searchPattern);
        }

        private static void EnsureKlasorVar()
        {
            if (!Directory.Exists(KlasorYolu))
                Directory.CreateDirectory(KlasorYolu);
        }
    }
}
