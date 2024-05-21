using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services
{
    internal class Logger : ILoggable
    {
        public event Action<string, string[]> Log;

        public void OnLog(string message, string[] args)
        {
            Log?.Invoke(message, args);
        }
    }
    interface ILoggable
    {
        event Action<string, string[]> Log;
        void OnLog(string message, string[] args);
    }
}
