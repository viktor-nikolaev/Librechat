using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using GalaSoft.MvvmLight;
using LibreChat.Server.Model;
using LibreChat.Server.Properties;
using LibreChat.Service;
using LibreChat.Shared;

namespace LibreChat.Server.ViewModel
{
  [UsedImplicitly]
  public class MainViewModel : ViewModelBase
  {
    private readonly ChatReceiver _receiver;
    private ObservableCollection<Lobby> _lobbies;

    public MainViewModel()
    {
      var host = new ServiceHost(typeof(ChatService));
      host.Open();

      _receiver = new ChatReceiver("admin");
      _receiver.Connect();

      _lobbies = new ObservableCollection<Lobby>();
      _lobbies.CollectionChanged += LobbiesListChanged;
    }

    public ObservableCollection<Lobby> Lobbies
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

    private void LobbiesListChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      if (args.Action == NotifyCollectionChangedAction.Add)
      {
        Lobby newLobby = args.NewItems.Cast<Lobby>().First();
        newLobby.PropertyChanged += LobbyNameChanged;
      }
      else if (args.Action == NotifyCollectionChangedAction.Remove)
      {
        foreach (Lobby lobby in args.OldItems)
        {
          _receiver.RemoveLobby(lobby.Name);
        }
      }
    }

    private void LobbyNameChanged(object s, PropertyChangedEventArgs e)
    {
      var lobby = s as Lobby;
      if (lobby != null && e.PropertyName == "Name")
      {
        var args = e as PropertyChangedCustomEventArgs;

        if (args != null && !string.IsNullOrEmpty(args.OldValue))
        {
          _receiver.RemoveLobby(args.OldValue);
        }

        _receiver.CreateLobby(lobby.Name);
      }
    }
  }
}