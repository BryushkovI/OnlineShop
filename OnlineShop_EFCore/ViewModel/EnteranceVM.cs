﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using OnlineShop_CL.Services;
using OnlineShop_CL.Command;

namespace OnlineShop_EFCore.ViewModel
{
    internal class EnteranceVM : ViewModel
    {
        Context context;
        public DataProvider dataProvider; 
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
            if (dataProvider.IsConnected())
            {
                OnEnter();
            }
        }
        bool CanLoginExecute(object p) => !string.IsNullOrWhiteSpace(_username) && !string.IsNullOrWhiteSpace(_password);
        public EnteranceVM()
        {
            Login = new LambdaCommand(OnLoginExecuted, CanLoginExecute);
            context = new();
            dataProvider = new(context);
        }

        public delegate void EnterHandler(Context context);
        public event EnterHandler Enter;
        void OnEnter()
        {
            Enter?.Invoke(context);
        }
    }
}
