﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShrinkageExplorer.Wpf.Views
{
  /// <summary>
  /// Логика взаимодействия для LoginView.xaml
  /// </summary>
  public partial class LoginView
  {
    public LoginView()
    {
      InitializeComponent();
    }

    private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    private void LoginView_OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      Close();
    }
  }
}
