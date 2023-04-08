using System;
using System.Windows;
using System.Windows.Input;
using Diploma.Client.MVVM.Model;
using Diploma.Client.MVVM.View.Authorization.Pages;

namespace Diploma.Client.MVVM.View.Authorization
{
    public partial class AuthorizationWindow : Window
    {
        private AuthEventType _eventType;
        
        public AuthorizationWindow()
        {
            InitializeComponent();

            _eventType = AuthEventType.LOGIN;
            AuthFrame.Content = new LoginPage();
            EventComment.Text = _eventType.GetComment();
            EventAction.Content = _eventType.GetAction();
        }
        
        private void WindowBorderOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MinimizeButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void MaximizeButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void CloseButtonOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EventActionOnClick(object sender, RoutedEventArgs e)
        {
            switch (_eventType)
            {
                case AuthEventType.LOGIN:
                    _eventType = AuthEventType.REGISTRATION;
                    AuthFrame.Content = new RegistrationPage();
                    break;
                case AuthEventType.REGISTRATION:
                    _eventType = AuthEventType.LOGIN;
                    AuthFrame.Content = new LoginPage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}