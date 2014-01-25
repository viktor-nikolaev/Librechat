using System.Collections.Generic;

namespace LibreChat.Service
{
  public class Client
  {
    private readonly List<string> _joinedLobbies;

    public Client(string login, IChatCallback callback)
    {
      Login = login;
      Callback = callback;
      _joinedLobbies = new List<string>();
    }

    public IEnumerable<string> JoinedLobbies
    {
      get { return _joinedLobbies.AsReadOnly(); }
    }

    public string Login { get; private set; }
    public IChatCallback Callback { get; private set; }

    public void JoinLobby(string lobby)
    {
      if (!_joinedLobbies.Contains(lobby))
      {
        _joinedLobbies.Add(lobby);
      }
    }

    public bool IsInLobby(string lobby)
    {
      return _joinedLobbies.Contains(lobby);
    }

    public void LeaveLobby(string lobby)
    {
      _joinedLobbies.Remove(lobby);
    }
  }
}