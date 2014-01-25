using System;
using System.ComponentModel;
using System.Windows;
using LibreChat.Messenger.Services;
using LibreChat.Messenger.ViewModel;

namespace LibreChat.Messenger.Views
{
  /// <summary>
  ///   Interaction logic for MessengerWindow.xaml
  /// </summary>
  public partial class MessengerWindow : Window
  {
    /// <summary>
    ///   Initializes a new instance of the MessengerWindow class.
    /// </summary>
    public MessengerWindow()
    {
      InitializeComponent();
      Closing += (s, e) => ViewModelLocator.Cleanup();
    }

    private void MessengerWindow_OnClosing(object sender, CancelEventArgs e)
    {
      e.Cancel = true;
      DialogService.HideMessengerWindow();
    }
  }
}