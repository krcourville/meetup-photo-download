using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace MPDL.UI {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App()
            : base() {
            this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
            MessageBox.Show(
                    string.Format("Error: {0}", e.Exception.Message),
                    "Application Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            Application.Current.Shutdown();
        }

        static App() {
            DispatcherHelper.Initialize();

        }


    }
}
