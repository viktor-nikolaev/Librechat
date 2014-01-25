using System.Collections.Generic;
using System.ServiceModel;

namespace LibreChat.Service
{
  public interface IChatCallback
  {
    [OperationContract(IsOneWay = true)]
    void RefreshLobbyList(IEnumerable<string> lobbies);

    [OperationContract(IsOneWay = true)]
    void RefreshLobbyMembers(string lobby, IEnumerable<string> users);

    [OperationContract(IsOneWay = true)]
    void ReceiveMessage(string lobby, string sender, string message);

    [OperationContract(IsOneWay = true)]
    void ReceiveWhisper(string sender, string message);
  }
}