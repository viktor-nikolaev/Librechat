using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace LibreChat.Messenger.ViewModel
{
  public class ViewModelLocator
  {
    static ViewModelLocator()
    {
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

      SimpleIoc.Default.Register<MessengerViewModel>();
      SimpleIoc.Default.Register<LoginViewModel>();
      SimpleIoc.Default.Register<LobbyListViewModel>();
    }

    /// <summary>
    ///   Gets the Messenger property.
    /// </summary>
    [SuppressMessage("Microsoft.Performance",
      "CA1822:MarkMembersAsStatic",
      Justification = "This non-static member is needed for data binding purposes.")]
    public MessengerViewModel Messenger
    {
      get { return ServiceLocator.Current.GetInstance<MessengerViewModel>(); }
    }

    [SuppressMessage("Microsoft.Performance",
      "CA1822:MarkMembersAsStatic",
      Justification = "This non-static member is needed for data binding purposes.")]
    public LoginViewModel Login
    {
      get { return ServiceLocator.Current.GetInstance<LoginViewModel>(); }
    }

    [SuppressMessage("Microsoft.Performance",
      "CA1822:MarkMembersAsStatic",
      Justification = "This non-static member is needed for data binding purposes.")]
    public LobbyListViewModel LobbyList
    {
      get { return ServiceLocator.Current.GetInstance<LobbyListViewModel>(); }
    }

    /// <summary>
    ///   Cleans up all the resources.
    /// </summary>
    public static void Cleanup()
    {
    }
  }
}