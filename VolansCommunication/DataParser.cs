namespace VolansCommunication
{
    internal class DataParser
    {
        private byte[] expectedStartCode = { 0x15, 0x98 };  // Başlangıç Kodu (2 byte)
        private byte[] expectedEndCode = { 0x10, 0x10 };    // Bitiş Kodu (2 byte)
        private byte expectedIdentifierCode = 0x98; // Tanımlayıcı Kod (1 byte)
        private byte lastReceivedPacketCounter = 0;

        private List<byte> buffer = new List<byte>(); // Gelen veriyi tutacak buffer
        private Action<string> _logCallback; // Hata ve işlem mesajları için callback

        // Callback fonksiyonu verilecek şekilde constructor
        public DataParser(Action<string> logCallback)
        {
            _logCallback = logCallback;
        }

        public bool ProcessIncomingData(byte[] incomingData)
        {
            // Gelen veriyi buffer'a ekle
            buffer.AddRange(incomingData);

            // Tam bir paket olup olmadığını kontrol et
            while (TryGetCompletePacket(out DataPacket packet))
            {
                // Paket geçerliyse işle
                if (IsPacketValid(packet))
                {
                    LogMessage("Paket Geçerli, işlem yapılabilir.");
                }
                else
                {
                    LogMessage("Geçersiz paket tespit edildi.");
                }

                // Paket alındıktan sonra buffer'ı temizle
                buffer.Clear();
            }

            return true;
        }

        private void LogMessage(string message)
        {
            _logCallback?.Invoke(message); // Callback fonksiyonunu çağırarak mesajı ilet
        }

        private bool TryGetCompletePacket(out DataPacket packet)
        {
            packet = null;

            // Buffer'dan bir paket almak için minimum uzunluk (başlangıç kodu + tanımlayıcı + veri + bitiş kodu) gerekir
            if (buffer.Count < expectedStartCode.Length + 1 + 1 + expectedEndCode.Length)
            {
                return false; // Henüz tam paket yok
            }

            // Başlangıç ve bitiş kodlarını kontrol et
            if (buffer[0] == expectedStartCode[0] && buffer[1] == expectedStartCode[1] &&
                buffer[buffer.Count - 2] == expectedEndCode[0] && buffer[buffer.Count - 1] == expectedEndCode[1])
            {
                // Paket tespit edildi, veri kısmı alınacak
                packet = new DataPacket
                {
                    StartCode = new byte[] { buffer[0], buffer[1] },
                    IdentifierCode = new byte[] { buffer[2] },
                    Data = buffer.GetRange(3, buffer.Count - 5).ToArray(), // Veri kısmı
                    PacketCounter = buffer[buffer.Count - 4],
                    CRC = buffer[buffer.Count - 3],
                    EndCode = new byte[] { buffer[buffer.Count - 2], buffer[buffer.Count - 1] }
                };

                return true; // Paket başarılı bir şekilde alındı
            }

            return false; // Tam paket yok
        }

        public bool IsPacketValid(DataPacket packet)
        {
            // Başlangıç kodunu kontrol et (iki baytlık karşılaştırma)
            if (!StartCodeMatches(packet.StartCode))
            {
                LogMessage("Geçersiz Başlangıç Kodu!");
                return false;
            }

            // Tanımlayıcı kodu kontrol et
            if (packet.IdentifierCode[0] != expectedIdentifierCode)
            {
                LogMessage("Geçersiz Tanımlayıcı Kod!");
                return false;
            }

            // CRC'yi kontrol et
            if (!VerifyCRC(packet))
            {
                LogMessage("CRC Doğrulaması Başarısız!");
                return false;
            }

            // Bitiş kodunu kontrol et (iki baytlık karşılaştırma)
            if (!EndCodeMatches(packet.EndCode))
            {
                LogMessage("Geçersiz Bitiş Kodu!");
                return false;
            }

            // Paket sayacını kontrol et (kayıp paket kontrolü)
            if (packet.PacketCounter != lastReceivedPacketCounter + 1)
            {
                LogMessage($"Paket kaybı tespit edildi! Beklenen paket: {lastReceivedPacketCounter + 1}, Alınan paket: {packet.PacketCounter}");
                return false;
            }

            // Paketi geçerli kabul et
            lastReceivedPacketCounter = packet.PacketCounter;
            LogMessage("Paket Geçerli.");
            return true;
        }

        private bool StartCodeMatches(byte[] startCode)
        {
            return startCode.Length == expectedStartCode.Length &&
                   startCode[0] == expectedStartCode[0] &&
                   startCode[1] == expectedStartCode[1];
        }

        private bool EndCodeMatches(byte[] endCode)
        {
            return endCode.Length == expectedEndCode.Length &&
                   endCode[0] == expectedEndCode[0] &&
                   endCode[1] == expectedEndCode[1];
        }

        private bool VerifyCRC(DataPacket packet)
        {
            // CRC hesaplama ve doğrulama işlemi (örneğin, basit bir toplama bazlı CRC kontrolü)
            byte calculatedCRC = CalculateCRC(packet);
            return calculatedCRC == packet.CRC;
        }

        private byte CalculateCRC(DataPacket packet)
        {
            // Basit bir CRC hesaplama algoritması (örneğin toplam CRC)
            byte crc = 0;
            crc ^= packet.StartCode[0];
            crc ^= packet.StartCode[1];
            crc ^= packet.IdentifierCode[0];

            foreach (byte b in packet.Data)
            {
                crc ^= b;
            }

            crc ^= packet.PacketCounter;
            crc ^= packet.EndCode[0];
            crc ^= packet.EndCode[1];

            return crc;
        }
    }
}
