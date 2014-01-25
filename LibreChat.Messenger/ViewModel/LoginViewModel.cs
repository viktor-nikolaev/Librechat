using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LibreChat.Messenger.Annotations;
using LibreChat.Messenger.Services;

namespace LibreChat.Messenger.ViewModel
{
  [UsedImplicitly]
  public class LoginViewModel : ViewModelBase
  {
    public string Login { get; set; }

    public ICommand LoginCommand
    {
      get { return new RelayCommand(Connect); }
    }

    private void Connect()
    {
      try
      {
        LogService.Log("Connecting");
        ChatService.Connect(Login);
        LogService.Log("Connected");
      }
      catch (Exception)
      {
        MessageBox.Show("Login is already registered");
        return;
      }

      DialogService.OpenLobbyListWindow();
      DialogService.CloseLoginWindow();
    }
  }
}