using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LibreChat.Messenger.Annotations;
using LibreChat.Messenger.Properties;
using LibreChat.Messenger.Services;

namespace LibreChat.Messenger.ViewModel
{
  [UsedImplicitly]
  public class LobbyListViewModel : ViewModelBase
  {
    private ObservableCollection<DataGridItem<string>> _lobbies;
    private DataGridItem<string> _selectedLobby;

    public LobbyListViewModel()
    {
      _lobbies = new ObservableCollection<DataGridItem<string>>();
      ChatService.Receiver.LobbyListChanged += LobbyListChanged;

      LogService.Log("Lobby list is opened");
    }

    public ObservableCollection<DataGridItem<string>> Lobbies
    {
      get { return _lobbies; }
      set
      {
        if (_lobbies != value)
        {
          _lobbies = value;
          RaisePropertyChanged(() => Lobbies);
        }
      }
    }

    public ICommand OpenLobbyTabCommand
    {
      get { return new RelayCommand(OpenLobbyTab); }
    }

    public DataGridItem<string> SelectedLobby
    {
      get { return _selectedLobby; }
      set
      {
        if (_selectedLobby != value)
        {
          _selectedLobby = value;
          RaisePropertyChanged(() => SelectedLobby);
        }
      }
    }

    public string Title
    {
      get
      {
        return ChatService.Receiver.Login + " | Librechat";
      }
    }

    private void LobbyListChanged(object sender, string[] lobbies)
    {
      var list = lobbies.Select(s => new DataGridItem<string> {Value = s});
      Lobbies = new ObservableCollection<DataGridItem<string>>(list);
    }

    private void OpenLobbyTab()
    {
      DialogService.OpenMessengerWindow();
      MessengerInstance.Send(SelectedLobby.Value);
    }
  }
}