using System;

namespace FilesManager
{
    public class FileModel
    {
        public FileModel(string encoder, byte[] scannedDocument, string fileName)
        {
            ScannedDocument = scannedDocument;
            FileName = fileName;
            Encoder = encoder;
        }

        public string FileName { get; set; }
        public string Encoder { get; set; }
        public Byte[] ScannedDocument { get; set; }
    }
}
