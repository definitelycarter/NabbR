
using System;
using NabbR.ViewModels;
namespace NabbR.Services
{
    public interface IDialogService
    {
        void Show<T>(String uri, Action<T,Boolean> callback) where T : ViewModelBase;
        void Show<T>(String uri, T viewModel, Action<T, Boolean> callback) where T : ViewModelBase;
    }
}
