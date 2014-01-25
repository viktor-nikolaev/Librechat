using System;
using System.ServiceModel;
using LibreChat.Shared.ChatReference;

namespace LibreChat.Shared
{
  public class ChatReceiver : IChatServiceCallback
  {
    private ChatServiceClient _server;
    private string _token;

    #region Implementation of IChatServiceCallback

    void IChatServiceCallback.RefreshLobbyList(string[] lobbies)
    {
      EventHandler<string[]> handler = LobbyListChanged;
      if (handler != null)
      {
        handler(this, lobbies);
      }
    }

    void IChatServiceCallback.RefreshLobbyMembers(string lobby, string[] users)
    {
      EventHandler<LobbyChangedEventArgs> handler = LobbyMembersChanged;
      if (handler != null)
      {
        var args = new LobbyChangedEventArgs(lobby, users);
        handler(this, args);
      }
    }

    void IChatServiceCallback.ReceiveMessage(string lobby, string sender, string message)
    {
      EventHandler<LobbyMessage> handler = MessageReceived;
      if (handler != null)
      {
        var mes = new LobbyMessage(lobby, sender, message);
        handler(this, mes);
      }
    }

    void IChatServiceCallback.ReceiveWhisper(string sender, string message)
    {
      EventHandler<Message> handler = WhisperReceived;
      if (handler != null)
      {
        var mes = new Message(sender, message);
        handler(this, mes);
      }
    }

    #endregion

    public ChatReceiver(string login)
    {
      Login = login;
    }

    public string Login { get; private set; }

    public event EventHandler<string[]> LobbyListChanged;
    public event EventHandler<LobbyChangedEventArgs> LobbyMembersChanged;
    public event EventHandler<LobbyMessage> MessageReceived;
    public event EventHandler<Message> WhisperReceived;

    #region Server opeations

    public async void Connect()
    {
      var server = new ChatServiceClient(new InstanceContext(this));
      string token = await server.LoginAsync(Login);

      if (string.IsNullOrEmpty(token))
      {
        throw new InvalidOperationException("Can't login into server");
      }

      _server = server;
      _token = token;
    }

    public void Disconect()
    {
      _server.Logout(_token);
    }

    public void CreateLobby(string lobbyName)
    {
      _server.CreateLobby(_token, lobbyName);
    }

    public void RemoveLobby(string lobbyName)
    {
      _server.RemoveLobby(_token, lobbyName);
    }

    public void JoinLobby(string lobby)
    {
      _server.JoinLobby(_token, lobby);
    }

    public void LeaveLobby(string lobby)
    {
      _server.LeaveLobby(_token, lobby);
    }

    public void Say(string lobby, string message)
    {
      _server.Say(_token, lobby, message);
    }

    public void Whisper(string recipient, string message)
    {
      _server.Whisper(_token, recipient, message);
    }

    #endregion
  }
}