using FirstFloor.ModernUI.Windows;
using System;
using System.Threading;
using System.Windows;

namespace NabbR.Client
{
    partial class App
    {
        public App()
        {
            this.Startup += OnAppStartup;
        }

        private async void OnAppStartup(object sender, StartupEventArgs e)
        {
            // create the kernel
            this.Initialize();

            IContentLoader contentLoader = resolver.Get<IContentLoader>();
            this.Resources.Add("ContentLoader", contentLoader);

            this.MainWindow = (Window)await contentLoader.LoadContentAsync(new Uri("/", UriKind.Relative), CancellationToken.None);
            this.MainWindow.Show();
        }
    }
}
