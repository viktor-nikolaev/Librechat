using System.ServiceModel;

namespace LibreChat.Service
{
  [ServiceContract(CallbackContract = typeof (IChatCallback))]
  public interface IChatService
  {
    [OperationContract(IsOneWay = false)]
    string Login(string login);

    [OperationContract(IsOneWay = true)]
    void Logout(string token);

    [OperationContract(IsOneWay = true)]
    void CreateLobby(string token, string lobbyName);

    [OperationContract(IsOneWay = true)]
    void RemoveLobby(string token, string lobbyName);

    [OperationContract(IsOneWay = true)]
    void JoinLobby(string token, string lobby);

    [OperationContract(IsOneWay = true)]
    void LeaveLobby(string token, string lobby);

    [OperationContract(IsOneWay = true)]
    void Say(string token, string lobby, string message);

    [OperationContract(IsOneWay = true)]
    void Whisper(string token, string receiver, string message);
  }
}