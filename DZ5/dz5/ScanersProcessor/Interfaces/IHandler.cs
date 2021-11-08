using ScanerProcessor.Models;

namespace ScanerProcessor.Interfaces
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        object Handle(ScanModel scan, LoggingLib.ILogging _log);
    }
}
