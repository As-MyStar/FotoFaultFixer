using System;

namespace FotoFaultFixerUI.Services.Commands.Base
{
    public interface ICommand<T>
    {
        T Execute(T obj, IProgress<int> progressReporter);
    }
}
