using System;

namespace ScanerProcessor.Models
{
    public class ScanModel
    {
        public Byte[] ScannedDocument { get; set; }
        public int ScanFormat { get; set; }
    }
}
