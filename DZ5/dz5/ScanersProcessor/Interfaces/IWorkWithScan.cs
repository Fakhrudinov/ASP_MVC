using ScanerProcessor.Models;

namespace ScanerProcessor.Interfaces
{
    public interface IWorkWithScan
    {
        void AnalizeIncomingScan(ScanModel newScan);
    }
}
