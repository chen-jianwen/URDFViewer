using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Microsoft.Win32;
using System.IO;
using HelixToolkit.Wpf;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Numerics;
using URDFImporter;
using System.Xml;


namespace URDFImporter
{
    /// <summary>
    /// MainWindow 交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // ===================== 字段 =====================
        private string currentUrdfPath = "";
        private string? lastPackagePath = null;
        private Robot? robot = null;

        private Dictionary<string, ModelVisual3D> linkVisuals = new();
        private Dictionary<string, CoordinateSystemVisual3D> jointCoordinateVisuals = new();
        private bool showLinks = true;
        private bool showJointCoordinates = true;


        // ===================== 构造函数 =====================
        public MainWindow()
        {
            InitializeComponent();
            MenuCloseUrdf.IsEnabled = false;
            UpdateStatusBarNotification("就绪");
            UpdateStatusBarNotification("欢迎使用 URDF Importer！");
            if (StatusExpander != null)
            {
                StatusExpander.Expanded += (s, e) =>
                {
                    if (StatusText != null) StatusText.Text = string.Empty;
                };
                StatusExpander.Collapsed += (s, e) =>
                {
                    if (StatusHistoryBox != null && StatusText != null)
                    {
                        // 取第一行（最新一条）
                        var lines = StatusHistoryBox.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        StatusText.Text = lines.Length > 0 ? lines[0] : "";
                    }
                };
            }
        }

        // ===================== 文件菜单事件 =====================
        #region 文件菜单事件
        private void MenuLoadUrdf_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "请选择URDF文件",
                Filter = "URDF文件 (*.urdf)|*.urdf|所有文件 (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = false
            };
            if (dialog.ShowDialog() != true) return;
            string urdfFile = dialog.FileName;
            string? packagePath = LocatePackageRoot(urdfFile);
            if (string.IsNullOrEmpty(packagePath))
            {
                UpdateStatusBarNotification("未能自动定位到package根目录（需包含 urdf 和 meshes 文件夹）！");
                return;
            }
            currentUrdfPath = urdfFile;
            lastPackagePath = packagePath;
            LoadUrdfFile(currentUrdfPath, lastPackagePath);
            MenuCloseUrdf.IsEnabled = true;
            Title = $"URDF Importer - {Path.GetFileName(currentUrdfPath)}";
            UpdateStatusBarNotification($"已加载URDF文件：{Path.GetFileName(currentUrdfPath)}");
        }

        private void MenuCloseUrdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearViewport();
                if (JointSlidersPanel != null)
                    JointSlidersPanel.Children.Clear(); // 清空所有滑块
                currentUrdfPath = "";
                MenuCloseUrdf.IsEnabled = false;
                Title = "URDF Importer";
                UpdateStatusBarNotification("URDF文件已关闭");
            }
            catch (Exception ex)
            {
                UpdateStatusBarNotification($"关闭URDF文件时出错：{ex.Message}");
            }
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("确定要退出应用程序吗？", "确认退出", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        #endregion

        // ===================== 视图菜单事件 =====================
        #region 视图菜单事件
        private void MenuResetView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetCameraToDefault();
                UpdateStatusBarNotification("视图已重置");
            }
            catch (Exception ex)
            {
                UpdateStatusBarNotification($"重置视图时出错：{ex.Message}");
            }
        }

        private void MenuZoomToFit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainViewport.ZoomExtents();
            }
            catch (Exception ex)
            {
                UpdateStatusBarNotification($"缩放到适合时出错：{ex.Message}");
            }
        }

        private void MenuShowLinks_Click(object sender, RoutedEventArgs e)
        {
            if (MenuShowLinks.IsChecked)
                ShowLinks();
            else
                HideLinks();
        }

        private void MenuShowJointCoordinates_Click(object sender, RoutedEventArgs e)
        {
            if (MenuShowJointCoordinates.IsChecked)
                ShowJointCoordinates();
            else
                HideJointCoordinates();
        }

        #endregion

        // ===================== 帮助菜单事件 =====================
        #region 帮助菜单事件
        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            string aboutMessage = @"URDF Importer v1.0\n\n这是一个用于加载和显示URDF文件的工具。\n\n功能特性：\n• 加载URDF文件\n• 3D可视化机器人模型\n• 交互式3D视图操作\n• 坐标系显示\n\n开发环境：\n• .NET 8.0\n• WPF\n• HelixToolkit\n\n版权所有 ©JavenChan 2025";
            UpdateStatusBarNotification(aboutMessage);
        }
        #endregion

       

        // ===================== 主要业务逻辑 =====================
        #region 主要业务逻辑

        



        /// <summary>
        /// 加载URDF文件并初始化界面
        /// </summary>
        private void LoadUrdfFile(string filePath, string packagePath = "")
        {
            try
            {
                UpdateStatusBarNotification("正在加载URDF文件...");
                robot = UrdfParser.Parse(filePath);
                if (robot == null)
                {
                UpdateStatusBarNotification("解析URDF失败");
                UpdateStatusBarNotification($"URDF文件解析失败！\n文件路径：{filePath}");
                return;
                }
                int jointCount = robot.Joints?.Count ?? 0;
                int linkCount = robot.Links?.Count ?? 0;
                int meshCount = robot.Links?.Count(l => l.Visual != null && l.Visual.Geometry != null && l.Visual.Geometry.Mesh != null) ?? 0;


                UpdateStatusBarNotification($"已加载：{Path.GetFileName(filePath)}");

                CreateJointSliders();

                CreateLinkVisuals();

                UpdateLinkVisuals();

                CreateJointCoordinateSystems();
                UpdateJointCoordinateSystems();

                ShowRobotTopologyTree();
                //ShowLinkVisuals();
                string packageInfo = string.IsNullOrEmpty(packagePath) ? "" : $"\nPackage路径：{packagePath}";
                UpdateStatusBarNotification($"URDF文件已成功加载");
            }
            catch (Exception ex)
            {
                UpdateStatusBarNotification("加载失败");
                UpdateStatusBarNotification($"加载URDF文件时发生异常：{ex.Message}");
            }
        }



        /// <summary>
        /// 展示robot的树状拓扑结构到UrdfTreeView
        /// </summary>
        private void ShowRobotTopologyTree()
        {
            if (UrdfTreeView == null || robot == null || robot.Links == null || robot.Joints == null)
                return;
            UrdfTreeView.Items.Clear();

            // 构建link->children joints映射
            var linkToJoints = new Dictionary<string, List<Joint>>();
            foreach (var joint in robot.Joints)
            {
                if (joint.Parent?.Link == null) continue;
                if (!linkToJoints.ContainsKey(joint.Parent.Link))
                    linkToJoints[joint.Parent.Link] = new List<Joint>();
                linkToJoints[joint.Parent.Link].Add(joint);
            }

            // 找到根link（未被任何joint作为child的link）
            var allLinkNames = robot.Links.Select(l => l.Name).ToHashSet();
            var childLinks = robot.Joints.Select(j => j.Child?.Link).Where(n => !string.IsNullOrEmpty(n)).ToHashSet();
            var rootLinks = allLinkNames.Except(childLinks).ToList();
            if (rootLinks.Count == 0 && robot.Links.Count > 0)
                rootLinks.Add(robot.Links[0].Name); // fallback

            foreach (var root in rootLinks)
            {
                var rootItem = BuildLinkTreeItem(root, linkToJoints);
                UrdfTreeView.Items.Add(rootItem);
            }
        }

        private TreeViewItem BuildLinkTreeItem(string linkName, Dictionary<string, List<Joint>> linkToJoints)
        {
            var linkItem = new TreeViewItem { Header = $"{linkName}" };
            if (linkToJoints.TryGetValue(linkName, out var joints))
            {
                foreach (var joint in joints)
                {
                    var jointItem = new TreeViewItem { Header = $"{joint.Name} ({joint.Type})" };
                    // 递归添加子link
                    if (joint.Child?.Link != null)
                    {
                        var childLinkItem = BuildLinkTreeItem(joint.Child.Link, linkToJoints);
                        jointItem.Items.Add(childLinkItem);
                    }
                    linkItem.Items.Add(jointItem);
                }
            }
            return linkItem;
        }

        private void CreateLinkVisuals()
        {
            if (robot?.Links == null) return;
            // 清理旧的
            foreach (var v in linkVisuals.Values)
                MainViewport.Children.Remove(v);
            linkVisuals.Clear();

            var importer = new ModelImporter();

            foreach (var link in robot.Links)
            {
                Model3D? model = null;
                if (link.Visual?.Geometry?.Mesh != null && !string.IsNullOrEmpty(link.Visual.Geometry.Mesh.Filename))
                {
                    string meshPath = link.Visual.Geometry.Mesh.Filename;
                    if (meshPath.StartsWith("package://") && lastPackagePath != null)
                    {
                        var relPath = meshPath.Substring("package://".Length);
                        int slashIndex = relPath.IndexOf('/');
                        if (slashIndex >= 0)
                            relPath = relPath.Substring(slashIndex + 1);
                        meshPath = System.IO.Path.Combine(lastPackagePath, relPath.Replace('/', System.IO.Path.DirectorySeparatorChar));
                    }
                    if (File.Exists(meshPath) && string.Equals(Path.GetExtension(meshPath), ".STL", StringComparison.OrdinalIgnoreCase))
                    {
                        try { model = importer.Load(meshPath); } catch { }
                    }
                }
                if (model == null)
                {
                    var box = new HelixToolkit.Wpf.BoxVisual3D
                    {
                        Width = 0.05,
                        Height = 0.05,
                        Length = 0.05,
                        Fill = Brushes.SkyBlue
                    };
                    model = box.Content;
                }
                var visual = new ModelVisual3D
                {
                    Content = model,
                    Transform = Transform3D.Identity
                };
                visual.SetValue(FrameworkElement.TagProperty, "LinkVisual");
                linkVisuals[link.Name] = visual;
                if (showLinks)
                    MainViewport.Children.Add(visual);
            }
        }

        private void CreateJointCoordinateSystems()
        {
            // 移除旧的
            foreach (var v in jointCoordinateVisuals.Values)
                MainViewport.Children.Remove(v);
            jointCoordinateVisuals.Clear();

            if (robot?.Joints == null || robot.Links == null) return;

            foreach (var joint in robot.Joints)
            {
                if (joint.Child?.Link == null) continue;
                var cs = new CoordinateSystemVisual3D
                {
                    ArrowLengths = 0.08,
                    Transform = Transform3D.Identity
                };
                jointCoordinateVisuals[joint.Child.Link] = cs;
                if (showJointCoordinates)
                    MainViewport.Children.Add(cs);
            }
        }

        
        /// <summary>
        /// 创建关节滑块控件
        /// </summary>
        private void CreateJointSliders()
        {
            if (JointSlidersPanel == null || robot?.Joints == null)
                return;
            JointSlidersPanel.Children.Clear();
            foreach (var joint in robot.Joints)
            {
                if (joint.Type == JointType.Fixed)
                    continue;
                var panel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 2, 0, 2) };
                var label = new TextBlock
                {
                    Text = $"{joint.Name} ({joint.Type})",
                    Width = 120,
                    Foreground = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Center
                };
                var slider = new Slider
                {
                    Width = 120,
                    Minimum = joint.Limit?.Lower ?? -3.14,
                    Maximum = joint.Limit?.Upper ?? 3.14,
                    Value = 0,
                    Margin = new Thickness(5, 0, 5, 0),
                    Tag = joint.Name
                };
                var valueText = new TextBlock
                {
                    Width = 50,
                    Foreground = Brushes.LightGreen,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = slider.Value.ToString("F2")
                };
                slider.ValueChanged += (s, e) =>
                {
                    valueText.Text = slider.Value.ToString("F2");
                    if (slider.Tag is string jointName && robot?.Joints != null)
                    {
                        var targetJoint = robot.Joints.FirstOrDefault(j => j.Name == jointName);
                        if (targetJoint != null)
                        {
                            targetJoint.JointValue = slider.Value;
                        }
                    }
                    UpdateJointCoordinateSystems();
                    UpdateLinkVisuals();
                };
                panel.Children.Add(label);
                panel.Children.Add(slider);
                panel.Children.Add(valueText);
                JointSlidersPanel.Children.Add(panel);
            }
        }

        private void UpdateLinkVisuals()
        {
            if (robot?.Links == null) return;
            var linkTransforms = TransformUtils.ComputeLinkTransforms(robot);
            foreach (var link in robot.Links)
            {
                if (linkVisuals.TryGetValue(link.Name, out var visual) && linkTransforms.TryGetValue(link.Name, out var m))
                {
                    var matrix = new Matrix3D(
                        m.M11, m.M12, m.M13, 0,
                        m.M21, m.M22, m.M23, 0,
                        m.M31, m.M32, m.M33, 0,
                        m.M41, m.M42, m.M43, 1);
                    visual.Transform = new MatrixTransform3D(matrix);
                }
            }
        }

        /// <summary>
        /// 显示所有关节的坐标系
        /// </summary>
        private void UpdateJointCoordinateSystems()
        {
            if (robot?.Joints == null || robot.Links == null) return;
            var linkTransforms = TransformUtils.ComputeLinkTransforms(robot);
            foreach (var joint in robot.Joints)
            {
                if (joint.Child?.Link == null) continue;
                if (!linkTransforms.TryGetValue(joint.Child.Link, out var m)) continue;
                if (jointCoordinateVisuals.TryGetValue(joint.Child.Link, out var cs))
                {
                    var matrix = new Matrix3D(
                        m.M11, m.M12, m.M13, 0,
                        m.M21, m.M22, m.M23, 0,
                        m.M31, m.M32, m.M33, 0,
                        m.M41, m.M42, m.M43, 1);
                    cs.Transform = new MatrixTransform3D(matrix);
                }
            }
        }



        private void ShowLinks()
        {
            showLinks = true;
            foreach (var v in linkVisuals.Values)
            {
                if (!MainViewport.Children.Contains(v))
                    MainViewport.Children.Add(v);
            }
        }

        private void HideLinks()
        {
            showLinks = false;
            foreach (var v in linkVisuals.Values)
            {
                if (MainViewport.Children.Contains(v))
                    MainViewport.Children.Remove(v);
            }
        }

        private void ShowJointCoordinates()
        {
            showJointCoordinates = true;
            foreach (var v in jointCoordinateVisuals.Values)
            {
                if (!MainViewport.Children.Contains(v))
                    MainViewport.Children.Add(v);
            }
        }

        private void HideJointCoordinates()
        {
            showJointCoordinates = false;
            foreach (var v in jointCoordinateVisuals.Values)
            {
                if (MainViewport.Children.Contains(v))
                    MainViewport.Children.Remove(v);
            }
        }

        /// <summary>
        /// 清空视口内容
        /// </summary>
        private void ClearViewport()
        {
            try
            {
                // 只保留默认光照和网格，移除所有其他元素（包括所有CoordinateSystemVisual3D）
                var itemsToRemove = MainViewport.Children
                    .OfType<Visual3D>()
                    .Where(visual => !(visual is DefaultLights || visual is GridLinesVisual3D))
                    .ToList();
                foreach (var item in itemsToRemove)
                {
                    MainViewport.Children.Remove(item);
                }
                UpdateStatusBarNotification("就绪");
            }
            catch (Exception ex)
            {
                UpdateStatusBarNotification($"清除视图时出错：{ex.Message}");
                throw;
            }
        }
        #endregion

        // ===================== 辅助方法 =====================
        #region 辅助方法
        /// <summary>
        /// 自动定位package根目录（需包含 urdf 和 meshes 文件夹）
        /// </summary>
        private string? LocatePackageRoot(string urdfFile)
        {
            var dir = Path.GetDirectoryName(urdfFile);
            while (!string.IsNullOrEmpty(dir))
            {
                if (Directory.Exists(Path.Combine(dir, "urdf")) &&
                    Directory.Exists(Path.Combine(dir, "meshes")))
                {
                    return dir;
                }
                var parent = Path.GetDirectoryName(dir);
                if (parent == dir) break;
                dir = parent;
            }
            return null;
        }

        /// <summary>
        /// 重置相机到默认视角
        /// </summary>
        private void ResetCameraToDefault()
        {
            if (MainViewport?.Camera is PerspectiveCamera camera)
            {
                camera.Position = new Point3D(1.5, 1.5, 1.5);
                camera.LookDirection = new Vector3D(-1, -1, -1);
                camera.UpDirection = new Vector3D(0, 0, 1);
                camera.FieldOfView = 45;
            }
            else
            {
                MainViewport?.ResetCamera();
            }
        }

        /// <summary>
        /// 更新状态栏信息
        /// </summary>
        private void UpdateStatusBarNotification(string message)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            string fullMsg = $"[{time}] {message}";
            if (StatusText != null)
                StatusText.Text = fullMsg;
            if (StatusHistoryBox != null)
            {
                // 新消息插入到最上面
                if (string.IsNullOrWhiteSpace(StatusHistoryBox.Text))
                    StatusHistoryBox.Text = fullMsg;
                else
                    StatusHistoryBox.Text = fullMsg + "\r\n" + StatusHistoryBox.Text;
            }
        }

        
        /// <summary>
        /// 递归获取目录大小
        /// </summary>
        private long GetDirectorySize(DirectoryInfo dir)
        {
            long size = 0;
            try
            {
                foreach (var file in dir.GetFiles())
                    size += file.Length;
                foreach (var sub in dir.GetDirectories())
                    size += GetDirectorySize(sub);
            }
            catch { }
            return size;
        }

        /// <summary>
        /// 更新模型统计信息
        /// </summary>
        private void UpdateModelStats(int jointCount = 0, int linkCount = 0, int meshCount = 0)
        {
            
        }
        #endregion
    }

    // ===================== 扩展方法 =====================
    /// <summary>
    /// 扩展方法用于简化TextBlock赋值
    /// </summary>
    public static class TextBlockExtensions
    {
        public static void SetText(this TextBlock? textBlock, string text)
        {
            if (textBlock != null)
                textBlock.Text = text;
        }
    }
}
