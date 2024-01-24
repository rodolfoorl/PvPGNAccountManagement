using System;

namespace Launcher.Share.Commands
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
