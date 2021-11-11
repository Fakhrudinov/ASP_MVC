using LoggingLib;
using ScanerProcessor.Interfaces;
using ScanerProcessor.Models;

namespace ScanerProcessor.ScanHandlers
{
    abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;

        public IHandler SetNext(IHandler handler)
        {
            _nextHandler = handler;

            return handler;
        }

        public virtual object Handle(ScanModel scan, ILogging _log)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle(scan, _log);
            }
            else
            {
                return null;
            }
        }
    }
}
