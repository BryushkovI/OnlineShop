using HomeWork15.Command;
using OnlineShop.Command.Base;
using OnlineShop.Services;
using OnlineShop.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace OnlineShop.ViewModel
{
    internal class EnteranceVM : ViewModel
    {
        string _username;
        public string Username
        {
            get => _username;
            set => Set(ref  _username, value);
        }

        string _password;
        public string Password
        {
            private get => _password;
            set => Set(ref _password, value);
        }

        
        public ICommand Login { get; }
        void OnLoginExecuted(object p)
        {
            DataProvider dataProvider = new DataProvider(_username, _password);
            if (dataProvider.IsConnected()) OnEnter();

        }
        bool CanLoginExecute(object p) => !string.IsNullOrWhiteSpace(_username) && !string.IsNullOrWhiteSpace(_password);
        public EnteranceVM()
        {
            Login = new LambdaCommand(OnLoginExecuted, CanLoginExecute);
            
        }

        public delegate void EnterHandler();
        public event EnterHandler Enter;
        void OnEnter()
        {
            Enter?.Invoke();
        }
    }
}
