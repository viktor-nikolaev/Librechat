using LibreChat.Messenger.Services;

namespace LibreChat.Messenger.ViewModel.TabViewModels
{
  internal class WhisperTabViewModel : ChatTabViewModel
  {
    protected override void SendMessage()
    {
      ChatService.Whisper(Opposite, Message);
      ChatFlow += string.Format("{0} >> {1}", ChatService.Receiver.Login, Message);
    }
  }
}