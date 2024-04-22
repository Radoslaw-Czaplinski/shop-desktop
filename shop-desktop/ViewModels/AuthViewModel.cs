using System;
using System.ComponentModel;
using System.Windows.Input;
using shop_desktop.Services;

namespace shop_desktop.Models
{
    public class AuthViewModel : BaseViewModel
    {
        private string _email;
        private string _password;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }

        private readonly AuthService _authService;

        public AuthViewModel(AuthService authService)
        {
            _authService = authService;
            RegisterCommand = new RelayCommand(Register);
            LoginCommand = new RelayCommand(Login);
        }

        private async void Register(object parameter)
        {
            bool success = await _authService.RegisterAsync(Email, Password);
            if (success)
            {
                // Obsłuż sukces rejestracji
            }
            else
            {
                // Obsłuż błąd rejestracji
            }
        }

        private async void Login(object parameter)
        {
            bool success = await _authService.LoginAsync(Email, Password);
            if (success)
            {
                // Obsłuż sukces logowania
            }
            else
            {
                // Obsłuż błąd logowania
            }
        }
    }
}
