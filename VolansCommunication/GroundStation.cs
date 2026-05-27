using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace VolansCommunication
{
    internal class GroundStation
    {
        private LoraE32 RocketLora;
        private LoraE32 PayloadLora;
        private HakemStation HakemGroundStation;

        private MissionTimer dataReceiveTimer;
        private MissionTimer dataSendTimer;

        // Callback fonksiyonları tanımlayalım
        public Action<string> LogMessageCallback;

        public GroundStation(Action<string> logCallback)
        {
            LogMessageCallback = logCallback;  // Callback fonksiyonunu alıyoruz

            // İlk Timer: Veriyi Al
            dataReceiveTimer = new MissionTimer(200, ReceiveRocketData); // 200 ms aralıkla veri al

            // İkinci Timer: Veriyi Gönder
            dataSendTimer = new MissionTimer(200, SendDataToPort); // 200 ms aralıkla veriyi gönder
        }

        // Veriyi almak için görev fonksiyonu
        private void ReceiveRocketData()
        {
            if (RocketLora != null && RocketLora.LoraPort.IsOpen)
            {
                // RocketLora üzerinden veri alımı işlemi (örnek)
                byte[] receivedData = RocketLora.ReceiveData();

                if (receivedData != null && receivedData.Length > 0)
                {
                    LogMessageCallback($"Veri alındı: {BitConverter.ToString(receivedData)}");
                    // İşlemler sonrası veri işlenebilir.
                }
                else
                {
                    LogMessageCallback("Veri alınamadı.");
                }
            }
            else
            {
                LogMessageCallback("Roket Lora portu açık değil.");
            }
        }

        // Alınan veriyi bir seri porta gönderen görev fonksiyonu
        private void SendDataToPort()
        {
            if (HakemGroundStation.HakemPort != null && HakemGroundStation.HakemPort.IsOpen)
            {
                // Veri göndermek için örnek işlem
                byte[] dataToSend = new byte[] { 0x01, 0x02, 0x03 };
                HakemGroundStation.HakemPort.Write(dataToSend, 0, dataToSend.Length);
                LogMessageCallback("Veri Hakem Portuna gönderildi.");
            }
            else
            {
                LogMessageCallback("Hakem portu açılmadı veya hatalı.");
            }
        }
    }
}