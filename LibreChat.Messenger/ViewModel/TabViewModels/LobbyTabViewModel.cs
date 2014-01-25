using LibreChat.Messenger.Services;

namespace LibreChat.Messenger.ViewModel.TabViewModels
{
  internal class LobbyTabViewModel : ChatTabViewModel
  {
    protected override void SendMessage()
    {
      ChatService.Send(Opposite, Message);
    }
  }
}