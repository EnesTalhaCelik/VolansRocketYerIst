namespace VolansCommunication
{
    internal class DataPacket
    {
        public byte[] StartCode { get; set; } // Başlangıç Kodu (2 byte)
        public byte[] IdentifierCode { get; set; } // Tanımlayıcı Kod (1 byte)
        public byte[] Data { get; set; } // Veri Bölümü
        public byte PacketCounter { get; set; } // Paket Sayacı
        public byte CRC { get; set; } // CRC Kontrolü
        public byte[] EndCode { get; set; } // Bitiş Kodu (2 byte)
    }
}
