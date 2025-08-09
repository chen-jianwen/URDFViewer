using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using WindowsAPICodePack.Dialogs;

namespace URDFViewer
{
    public class FileSystemItem
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsDirectory { get; set; }
        public ObservableCollection<FileSystemItem> Children { get; set; }
    }

    /// <summary>
    /// TestOpenFolder.xaml 的交互逻辑
    /// </summary>
    public partial class TestOpenFolder : Window
    {

        public ObservableCollection<FileSystemItem> RootItems { get; set; } = new();

        public TestOpenFolder()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OpenURDFFolder(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "请选择一个文件夹"
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                RootItems.Clear();
                RootItems.Add(LoadDirectory(dialog.FileName));
                // 通知UI刷新（如用INotifyPropertyChanged）
            }
        }

        public FileSystemItem LoadDirectory(string path)
        {
            var item = new FileSystemItem
            {
                Name = System.IO.Path.GetFileName(path),
                FullPath = path,
                IsDirectory = true,
                Children = new ObservableCollection<FileSystemItem>()
            };

            // 加载子文件夹
            foreach (var dir in Directory.GetDirectories(path))
            {
                item.Children.Add(LoadDirectory(dir));
            }
            // 加载文件
            foreach (var file in Directory.GetFiles(path))
            {
                item.Children.Add(new FileSystemItem
                {
                    Name = System.IO.Path.GetFileName(file),
                    FullPath = file,
                    IsDirectory = false
                });
            }
            return item;
        }
    }
}
