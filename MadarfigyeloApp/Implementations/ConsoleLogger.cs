using MadarfigyeloApp.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadarfigyeloApp.Implementations
{
    public class ConsoleLogger : ILoggerService
    {
        void ILoggerService.LogError(string message)
        {
            Console.WriteLine("##### ERROR: {0}", message);
        }

        void ILoggerService.LogInfo(string message)
        {
            Console.WriteLine("##### INFO: {0}", message);
        }

        void ILoggerService.LogWarning(string message)
        {
            Console.WriteLine("##### WARNING: {0}", message);
        }
    }
}
