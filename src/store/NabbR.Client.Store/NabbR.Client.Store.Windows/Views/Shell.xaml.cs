using NabbR.ViewModels;
using Windows.UI.Xaml.Controls;

namespace NabbR.Views
{
    [View("/")]
    [ViewModel(typeof(ShellViewModel))]
    public sealed partial class Shell : Page
    {
        public Shell()
        {
            this.InitializeComponent();
        }
    }
}
