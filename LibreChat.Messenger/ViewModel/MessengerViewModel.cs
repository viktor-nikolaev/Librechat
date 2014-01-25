using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using GalaSoft.MvvmLight;
using LibreChat.Messenger.Annotations;
using LibreChat.Messenger.Services;
using LibreChat.Messenger.ViewModel.TabViewModels;
using LibreChat.Shared;

namespace LibreChat.Messenger.ViewModel
{
  [UsedImplicitly]
  public class MessengerViewModel : ViewModelBase
  {
    private ChatTabViewModel _selectedTab;
    private ObservableCollection<ChatTabViewModel> _tabs;

    public MessengerViewModel()
    {
      _tabs = new ObservableCollection<ChatTabViewModel>();
      _tabs.CollectionChanged += TabsCollectionChanged;

      MessengerInstance.Register<string>(this, OpenLobbyTab);
      InitChatService();
    }

    public string Title
    {
      get { return ChatService.Receiver.Login + " | Librechat"; }
    }

    public ObservableCollection<ChatTabViewModel> Tabs
    {
      get { return _tabs; }
      set
      {
        if (_tabs != value)
        {
          _tabs = value;
          RaisePropertyChanged(() => Tabs);
        }
      }
    }

    public ChatTabViewModel SelectedTab
    {
      get { return _selectedTab; }
      set
      {
        if (_selectedTab != value)
        {
          _selectedTab = value;
          RaisePropertyChanged(() => SelectedTab);
        }
      }
    }

    private void InitChatService()
    {
      ChatService.Receiver.LobbyListChanged += LobbyListChanged;
      ChatService.Receiver.LobbyMembersChanged += LobbyMembersChanged;
      ChatService.Receiver.MessageReceived += MessageReceived;
      ChatService.Receiver.WhisperReceived += WhisperReceived;
    }

    private void WhisperReceived(object sender, Message message)
    {
      var tab = Tabs.FirstOrDefault(x => x.Opposite == message.Sender);

      if (tab == null)
      {
        tab = new WhisperTabViewModel {Opposite = message.Sender};
        Tabs.Add(tab);
        SelectedTab = tab;
      }

      tab.ChatFlow += string.Format("{0} >> {1}", message.Sender, message.Content);
    }

    private void MessageReceived(object sender, LobbyMessage lobbyMessage)
    {
      var tab = Tabs.FirstOrDefault(x => x.Opposite == lobbyMessage.Lobby);
      if (tab != null)
      {
        tab.ChatFlow += string.Format("{0} >> {1}", lobbyMessage.Sender, lobbyMessage.Content);
      }
    }

    private void LobbyMembersChanged(object sender, LobbyChangedEventArgs args)
    {
      ChatTabViewModel tab = Tabs.FirstOrDefault(x => x.Opposite == args.LobbyName);

      if (tab != null)
      {
        var members = args.LobbyMembers.Select(x => new DataGridItem<string> {Value = x});
        tab.LobbyMembers = new ObservableCollection<DataGridItem<string>>(members);
      }
    }

    private void LobbyListChanged(object sender, string[] strings)
    {
      List<LobbyTabViewModel> tabs = Tabs.OfType<LobbyTabViewModel>()
                                         .Where(x => strings.All(s => s != x.Opposite))
                                         .ToList();

      foreach (LobbyTabViewModel tab in tabs)
      {
        Tabs.Remove(tab);
      }
    }

    private void OpenLobbyTab(string lobby)
    {
      var tab = Tabs.FirstOrDefault(x => x.Opposite == lobby);

      if (tab == null)
      {
        tab = new LobbyTabViewModel {Opposite = lobby};
        tab.PrivateChatStarted += OnPrivateChatStarted;

        ChatService.JoinLobby(lobby);
        Tabs.Add(tab);
      }

      SelectedTab = tab;
    }

    private void OnPrivateChatStarted(object sender, string oposite)
    {
      if (sender is LobbyTabViewModel)
      {
        if (ChatService.Receiver.Login == oposite)
        {
          return;
        }

        var tab = Tabs.FirstOrDefault(x => x.Opposite == oposite);

        if (tab == null)
        {
          tab = new WhisperTabViewModel {Opposite = oposite};
          Tabs.Add(tab);
        }

        SelectedTab = tab;
      }
    }

    private void TabsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      if (args.NewItems != null)
      {
        foreach (ChatTabViewModel tab in args.NewItems)
        {
          tab.LobbyLeaved += TabOnLobbyLeaved;
        }
      }

      if (args.OldItems != null && args.OldItems.Count != 0)
      {
        foreach (ChatTabViewModel tab in args.OldItems)
        {
          tab.LobbyLeaved -= TabOnLobbyLeaved;
        }
      }

      if (Tabs.Count == 0)
      {
        DialogService.HideMessengerWindow();
      }
    }

    private void TabOnLobbyLeaved(object s, EventArgs e)
    {
      var tab = s as ChatTabViewModel;
      if (tab != null)
      {
        if (SelectedTab == tab)
        {
          SelectedTab = Tabs.First();
        }

        Tabs.Remove(tab);
      }
    }
  }
}