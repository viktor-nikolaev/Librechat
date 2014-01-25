using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace LibreChat.Service
{
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
  public class ChatService : IChatService
  {
    private readonly Dictionary<string, Client> _clients;
    private readonly List<string> _lobbies;

    public ChatService()
    {
      _clients = new Dictionary<string, Client>();
      _lobbies = new List<string>();
    }

    public string Login(string login)
    {
      if (_clients.Any(x => x.Value.Login == login))
      {
        return string.Empty;
      }

      var callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();
      var client = new Client(login, callback);

      string token = Guid.NewGuid().ToString("N");

      _clients[token] = client;
      client.Callback.RefreshLobbyList(_lobbies);

      return token;
    }

    public void Logout(string token)
    {
      if (!CheckTokenValidity(token))
      {
        return;
      }

      IEnumerable<string> joinedLobbies = _clients[token].JoinedLobbies;
      _clients.Remove(token);

      foreach (string lobby in joinedLobbies)
      {
        RefreshLobbyMembers(lobby);
      }
    }

    public void CreateLobby(string token, string lobbyName)
    {
      if (!CheckTokenValidity(token))
      {
        return;
      }

      if (_clients[token].Login == "admin" && !_lobbies.Contains(lobbyName))
      {
        _lobbies.Add(lobbyName);
        RefreshLobbyList();
      }
    }

    public void RemoveLobby(string token, string lobbyName)
    {
      if (CheckTokenValidity(token) && _clients[token].Login == "admin")
      {
        if (_lobbies.Remove(lobbyName))
        {
          RefreshLobbyList();
        }
      }
    }

    public void JoinLobby(string token, string lobby)
    {
      if (!CheckTokenValidity(token) || _clients[token].IsInLobby(lobby))
      {
        return;
      }

      _clients[token].JoinLobby(lobby);
      RefreshLobbyMembers(lobby);
    }

    public void LeaveLobby(string token, string lobby)
    {
      if (!CheckTokenValidity(token) || !_clients[token].IsInLobby(lobby))
      {
        return;
      }

      _clients[token].LeaveLobby(lobby);
      RefreshLobbyMembers(lobby);
    }

    public void Say(string token, string lobby, string message)
    {
      if (!CheckTokenValidity(token) || !_clients[token].IsInLobby(lobby))
      {
        return;
      }

      IEnumerable<Client> lobbyMembers = _clients.Values.Where(x => x.IsInLobby(lobby));
      foreach (Client client in lobbyMembers)
      {
        client.Callback.ReceiveMessage(lobby, _clients[token].Login, message);
      }
    }

    public void Whisper(string token, string receiver, string message)
    {
      if (!CheckTokenValidity(token))
      {
        return;
      }

      Client clientReceiver = _clients.Values.FirstOrDefault(x => x.Login == receiver);
      if (clientReceiver != null)
      {
        Client sender = _clients[token];
        clientReceiver.Callback.ReceiveWhisper(sender.Login, message);
      }
    }

    private void RefreshLobbyList()
    {
      foreach (Client client in _clients.Values)
      {
        client.Callback.RefreshLobbyList(_lobbies);
      }
    }

    private bool CheckTokenValidity(string token)
    {
      return _clients.ContainsKey(token);
    }

    private void RefreshLobbyMembers(string lobby)
    {
      List<Client> lobbyMembers = _clients.Values.Where(x => x.IsInLobby(lobby)).ToList();
      List<string> logins = lobbyMembers.Select(x => x.Login).ToList();

      foreach (Client client in lobbyMembers)
      {
        client.Callback.RefreshLobbyMembers(lobby, logins);
      }
    }
  }
}