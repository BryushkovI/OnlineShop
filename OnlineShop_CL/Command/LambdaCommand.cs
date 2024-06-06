using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop_CL.Command
{
    public class LambdaCommand : OnlineShop_CL.Command.Base.Command
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;
        public LambdaCommand(Action<object> Execute, Func<object,bool> CanExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;
        }
        public override bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;

        public override void Execute(object parameter) => _Execute(parameter);
    }
    internal class LambdaCommandAsync : OnlineShop_CL.Command.Base.AsyncCommand
    {
        private readonly Func<object,Task> _Execute;
        private readonly Func<Task, bool> _CanExecute;
        public LambdaCommandAsync(Func<object,Task> Execute, Func<Task, bool> CanExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;
        }
        public override bool CanExecute(object parameter) => _CanExecute?.Invoke((Task)parameter) ?? true;

        public override Task ExecuteAsync(object parameter) => _Execute(parameter);
    }
}
