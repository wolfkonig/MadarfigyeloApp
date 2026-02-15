using MadarfigyeloApp.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadarfigyeloApp.Implementations
{
    public class ConsoleLogger : ILogger
    {
        void ILogger.LogError(string message)
        {
            Console.WriteLine("##### ERROR: {0}", message);
        }

        void ILogger.LogInfo(string message)
        {
            Console.WriteLine("##### INFO: {0}", message);
        }

        void ILogger.LogWarning(string message)
        {
            Console.WriteLine("##### WARNING: {0}", message);
        }
    }
}
