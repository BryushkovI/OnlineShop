using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop_CL.Services
{
    public class Logger : ILoggable
    {
        public event Action<string, string[]> Log;

        public void OnLog(string message, string[] args)
        {
            Log?.Invoke(message, args);
        }
    }
    public interface ILoggable
    {
        event Action<string, string[]> Log;
        void OnLog(string message, string[] args);
    }
}
