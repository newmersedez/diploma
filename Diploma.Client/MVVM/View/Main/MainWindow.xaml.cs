using System.Windows;
using System.Windows.Input;
using Diploma.Client.MVVM.View.Main.Pages;

namespace Diploma.Client.MVVM.View.Main
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowFrame.Content = new ChatPage();
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
    }
}