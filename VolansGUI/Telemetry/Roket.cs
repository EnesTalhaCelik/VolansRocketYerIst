namespace VolansGUI.Telemetry
{
    /// <summary>
    /// Roket / görev yükü telemetri modeli. Simülasyon ve (ileride) canlı
    /// binary telemetri bu nesneyi doldurur.
    /// </summary>
    public class Roket
    {
        public byte TakimID { get; set; } = 7;
        public byte Durum { get; set; } = 1;
        public byte PaketSayac { get; set; }
        public byte PaketSayacAnaAv { get; set; }
        public byte PaketSayacYedek { get; set; }
        public byte GorevYukuPaketSayac { get; set; }

        // Ana aviyonik
        public float JiroskopX { get; set; }
        public float JiroskopY { get; set; }
        public float JiroskopZ { get; set; }
        public float IvmeX { get; set; }
        public float IvmeY { get; set; }
        public float IvmeZ { get; set; }
        public float Aci { get; set; }
        public double RoketEnlem { get; set; } = 40.716582514101354;
        public double RoketBoylam { get; set; } = 31.52471117735377;
        public float RoketGpsIrtifa { get; set; } = 3000;
        public float BasincIrtifa { get; set; } = 2999;
        public float RoketNem { get; set; }
        public float RoketSicaklik { get; set; }
        public float RoketBasinc { get; set; }

        // Görev yükü
        public double GorevYukuEnlem { get; set; } = 40.716131152;
        public double GorevYukuBoylam { get; set; } = 31.524978876;
        public float GorevYukuGpsIrtifa { get; set; } = 1000;
        public float GorevYukuNem { get; set; }
        public float GorevYukuSicaklik { get; set; }
        public float GorevYukuBasinc { get; set; }

        public Roket() { }

        public Roket(byte takimID)
        {
            TakimID = takimID;
        }
    }
}
