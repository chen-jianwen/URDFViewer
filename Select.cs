using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace URDFViewer
{
    public class SelectUrdfWindow : Window
    {
        public string? SelectedUrdfFile { get; private set; }

        public SelectUrdfWindow(List<string> urdfFiles)
        {
            Title = "选择 URDF 文件";
            Width = 400;
            Height = 300;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            // 创建主容器
            var stackPanel = new StackPanel
            {
                Margin = new Thickness(10)
            };

            // 添加提示文本
            var instructionText = new TextBlock
            {
                Text = "请选择一个 URDF 文件：",
                Margin = new Thickness(0, 0, 0, 10),
                FontWeight = FontWeights.Bold
            };
            stackPanel.Children.Add(instructionText);

            // 创建列表框
            var listBox = new ListBox
            {
                ItemsSource = urdfFiles,
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(listBox);

            // 创建按钮容器
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            // 确定按钮
            var okButton = new Button
            {
                Content = "确定",
                Width = 75,
                Margin = new Thickness(5, 0, 0, 0)
            };
            okButton.Click += (sender, e) =>
            {
                if (listBox.SelectedItem != null)
                {
                    SelectedUrdfFile = listBox.SelectedItem.ToString();
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("请先选择一个 URDF 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };
            buttonPanel.Children.Add(okButton);

            // 取消按钮
            var cancelButton = new Button
            {
                Content = "取消",
                Width = 75,
                Margin = new Thickness(5, 0, 0, 0)
            };
            cancelButton.Click += (sender, e) =>
            {
                DialogResult = false;
            };
            buttonPanel.Children.Add(cancelButton);

            stackPanel.Children.Add(buttonPanel);

            // 设置窗口内容
            Content = stackPanel;
        }
    }
}
