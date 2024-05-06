﻿using System;
using System.Configuration;
using System.Data;
using System.Windows;

namespace shop_desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        MainWindow mainWindow = new MainWindow(); 
        mainWindow.Show();
    }

    [STAThread]
    public static void Main()
    {
        App app = new App();
        app.InitializeComponent();
        app.Run();  
    }
}
}
