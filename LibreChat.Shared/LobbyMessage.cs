namespace LibreChat.Shared
{
  public class LobbyMessage : Message
  {
    public LobbyMessage(string lobby, string sender, string content) : base(sender, content)
    {
      Lobby = lobby;
    }

    public string Lobby { get; private set; }
  }
}