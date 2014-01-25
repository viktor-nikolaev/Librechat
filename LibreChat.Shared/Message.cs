namespace LibreChat.Shared
{
  public class Message
  {
    public Message(string sender, string content)
    {
      Content = content;
      Sender = sender;
    }

    public string Sender { get; private set; }
    public string Content { get; private set; }
  }
}