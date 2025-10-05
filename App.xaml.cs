using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Hollywood_Editor
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создаем и показываем существующий MainWindow
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}