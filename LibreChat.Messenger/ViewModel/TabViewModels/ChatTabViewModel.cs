using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LibreChat.Messenger.Services;

namespace LibreChat.Messenger.ViewModel.TabViewModels
{
  public abstract class ChatTabViewModel : ViewModelBase
  {
    private string _chatFlow;
    private ObservableCollection<DataGridItem<string>> _lobbyMembers;
    private string _message;
    private DataGridItem<string> _selectedMember;

    public ObservableCollection<DataGridItem<string>> LobbyMembers
    {
      get { return _lobbyMembers; }
      set
      {
        if (_lobbyMembers != value)
        {
          _lobbyMembers = value;
          RaisePropertyChanged(() => LobbyMembers);
        }
      }
    }

    public string Opposite { get; set; }

    public ICommand CloseCommand
    {
      get { return new RelayCommand(LeaveRoom); }
    }

    public string ChatFlow
    {
      get { return _chatFlow; }
      set
      {
        if (_chatFlow != value)
        {
          _chatFlow = value;
          RaisePropertyChanged(() => ChatFlow);
        }
      }
    }

    public string Message
    {
      get { return _message; }
      set
      {
        if (_message != value)
        {
          _message = value;
          RaisePropertyChanged(() => Message);
        }
      }
    }

    public ICommand SendCommand
    {
      get { return new RelayCommand(Send); }
    }

    public ICommand MemberClickCommand
    {
      get { return new RelayCommand(OnMemberLoginClicked); }
    }

    public DataGridItem<string> SelectedMember
    {
      get { return _selectedMember; }
      set
      {
        if (_selectedMember != value)
        {
          _selectedMember = value;
          RaisePropertyChanged(() => SelectedMember);
        }
      }
    }

    private void Send()
    {
      if (!Message.EndsWith(Environment.NewLine))
      {
        Message += Environment.NewLine;
      }

      SendMessage();
      Message = string.Empty;
    }

    public event EventHandler<string> PrivateChatStarted;

    private void OnMemberLoginClicked()
    {
      EventHandler<string> handler = PrivateChatStarted;
      if (handler != null)
      {
        handler(this, SelectedMember.Value);
      }
    }

    public event EventHandler LobbyLeaved;

    private void LeaveRoom()
    {
      ChatService.LeaveLobby(Opposite);
      RaiseLobbyLeaved();
    }

    private void RaiseLobbyLeaved()
    {
      EventHandler handler = LobbyLeaved;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }

    protected abstract void SendMessage();
  }
}