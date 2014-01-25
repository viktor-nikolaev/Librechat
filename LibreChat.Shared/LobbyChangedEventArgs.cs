using System;

namespace LibreChat.Shared
{
  public class LobbyChangedEventArgs : EventArgs
  {
    public LobbyChangedEventArgs(string lobbyName, string[] lobbyMembers)
    {
      LobbyMembers = lobbyMembers;
      LobbyName = lobbyName;
    }

    public string LobbyName { get; private set; }
    public string[] LobbyMembers { get; private set; }
  }
}