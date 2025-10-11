using System;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace HollywoodEditor
{
    public partial class App : Application
    {
        public static string PathToExe = AppDomain.CurrentDomain.BaseDirectory;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            EnsureResourcesUnpacked();
        }

        private void EnsureResourcesUnpacked()
        {
            //string resDir = Path.Combine(PathToExe, "Resources");
            //string profilesDir = Path.Combine(resDir, "Profiles");
            //string yzFile = Path.Combine(resDir, "Profiles.yz");

       //     if (Directory.Exists(profilesDir))
                return;

            // если архива нет — предупредим, чтобы пользователь понимал
            //if (!File.Exists(yzFile))
            //{
            //    MessageBox.Show(
            //        "Resource archive not found (Profiles.yz).\n" +
            //        "Character icons will not be displayed..",
            //        "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}
            try
            {
          
               // ZipFile.ExtractToDirectory(yzFile, resDir);
            }
            catch (Exception ex)
            {
         //       MessageBox.Show(
         //         $"Error unpacking resources:\n{ex.Message}",
         //           "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
