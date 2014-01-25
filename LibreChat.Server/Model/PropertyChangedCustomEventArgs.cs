using System.ComponentModel;

namespace LibreChat.Server.Model
{
  public class PropertyChangedCustomEventArgs : PropertyChangedEventArgs
  {
    public PropertyChangedCustomEventArgs(string propertyName, string oldValue) : base(propertyName)
    {
      OldValue = oldValue;
    }

    public string OldValue { get; private set; }
  }
}