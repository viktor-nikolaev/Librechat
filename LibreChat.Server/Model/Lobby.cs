using System.ComponentModel;
using LibreChat.Server.Properties;

namespace LibreChat.Server.Model
{
  public class Lobby : INotifyPropertyChanged
  {
    private string _name;

    public string Name
    {
      get { return _name; }
      set
      {
        if (_name != value)
        {
          var oldValue = _name;
          _name = value;
          OnPropertyChanged("Name", oldValue);
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName, string oldValue)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedCustomEventArgs(propertyName, oldValue));
      }
    }
  }
}