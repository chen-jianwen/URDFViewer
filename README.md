

# URDF Viewer

URDF Viewer 是一款用于加载、可视化和验证 URDF（统一机器人描述格式，Unified Robot Description Format）文件的 Windows 桌面工具，基于 WPF 开发。

> **URDF 相关资料：**
> - [ROS Wiki: URDF](http://wiki.ros.org/urdf)
> - [ROS 2 Documentation: URDF](https://docs.ros.org/en/rolling/Concepts/Description/URDF.html)
> - [URDF 教程（英文）](http://wiki.ros.org/urdf/Tutorials)

---

## ✨ 功能特性

- 🚀 拖拽式加载 URDF 文件夹
- 🦾 3D 可视化机器人模型
- 🖱️ 交互式 3D 视图操作（旋转、缩放、平移）
- 🔄 关节坐标系显示/隐藏
- 🧩 机器人连杆显示/隐藏
- 🌳 结构树浏览，支持关节/连杆属性查看
- ⌨️ 丰富快捷键操作

---

## 🛠️ 操作指南

1. **加载模型**：拖拽包含 URDF 文件的机器人模型文件夹至 3D 视图，或点击菜单栏“文件”→“加载URDF文件夹”进行加载。
2. **结构浏览**：在左侧“机器人URDF”树中浏览机器人结构，点击不同连杆或关节可查看详细属性。
3. **3D 交互**：右侧 3D 视图支持鼠标拖拽旋转、滚轮缩放、右键平移等操作，可自由调整观察角度。
4. **关节控制**：通过 3D 视图上方的滑块面板调整关节角度，模型会实时响应变化。
5. **视图切换**：通过菜单栏“视图”可切换连杆模型和关节坐标系的显示与隐藏。
6. **重置视角**：如需重置视角或缩放到适合，可点击右上角悬浮按钮或使用快捷键。

---

## ❓ 常见问题

- **无法加载模型？**
	- 请确认 URDF 文件路径正确，且 package 目录下包含 meshes 文件夹。
- **模型显示异常？**
	- 请检查网格文件格式是否支持（推荐 STL/DAE），并确保文件未损坏。
- **如何重置视图？**
	- 点击“视图”菜单中的“重置视角”即可。

---

## ⌨️ 快捷键一览

| 快捷键   | 功能           |
| :------- | :------------- |
| Ctrl+O   | 加载URDF文件夹 |
| Ctrl+W   | 关闭URDF       |
| Ctrl+Q   | 退出程序       |
| Ctrl+R   | 重置视图       |
| Ctrl+F   | 缩放到适合     |
| F1       | 帮助中心       |
| Alt+A    | 关于           |

---

## 🏗️ 构建与发布

1. 使用 Visual Studio 2022 或更高版本打开 `URDFViewer.sln` 解决方案。
2. 还原 NuGet 包并编译项目。
3. 生成的安装包在 `Output/` 目录下，如 `URDFViewerSetup.exe`。

---


## 📦 依赖环境

- Windows 10/11
- .NET 8
- 需通过 NuGet 自动还原以下依赖包：
	- [AssimpNet](https://www.nuget.org/packages/AssimpNet)（模型格式解析）
	- [HandyControl](https://www.nuget.org/packages/HandyControl)（现代化 WPF 控件库）
	- [HelixToolkit.Core.Wpf](https://www.nuget.org/packages/HelixToolkit.Core.Wpf)（3D 可视化）
	- [WindowsAPICodePack.Shell.CommonFileDialogs.Wpf](https://www.nuget.org/packages/WindowsAPICodePack.Shell.CommonFileDialogs.Wpf)（Windows 文件对话框支持）
	- 其他依赖包会在首次构建时自动还原

---

## ℹ️ 关于

- 版本：1.0
- 作者：JavenChan
- 版权所有 © JavenChan 2025

---

## 📬 联系方式

- 邮箱：javenchan1994@foxmail.com
- 如有问题或建议，欢迎通过邮箱或 GitHub Issues 反馈。

---

## ☕ 支持作者

如果本工具对你有帮助，欢迎扫码赞赏支持，感谢你的支持！

<div align="center">
	<img src="Assets/appreciate.jpg" alt="appreciate" width="180" />
</div>
