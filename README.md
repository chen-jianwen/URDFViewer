
# URDF Viewer

URDF Viewer 是一款用于加载、可视化和验证 URDF（统一机器人描述格式，Unified Robot Description Format）文件的 Windows 桌面工具，基于 WPF 开发。

URDF Viewer is a Windows desktop tool for loading, visualizing, and validating URDF (Unified Robot Description Format) files, developed with WPF.

> **URDF 相关资料 / URDF Resources：**
> - [ROS Wiki: URDF](http://wiki.ros.org/urdf)
> - [ROS 2 Documentation: URDF](https://docs.ros.org/en/rolling/Concepts/Description/URDF.html)
> - [URDF 教程（英文）](http://wiki.ros.org/urdf/Tutorials)

---


## ✨ 功能特性 / Features

- 🚀 拖拽式加载 URDF 文件夹  
	Drag-and-drop loading of URDF folders
- 🦾 3D 可视化机器人模型  
	3D visualization of robot models
- 🖱️ 交互式 3D 视图操作（旋转、缩放、平移）  
	Interactive 3D view (rotate, zoom, pan)
- 🔄 关节坐标系显示/隐藏  
	Show/hide joint coordinate systems
- 🧩 机器人连杆显示/隐藏  
	Show/hide robot links
- 🌳 结构树浏览，支持关节/连杆属性查看  
	Structure tree browsing, view joint/link properties
- ⌨️ 丰富快捷键操作  
	Rich keyboard shortcuts

---


## 🛠️ 操作指南 / User Guide

1. **加载模型**：拖拽包含 URDF 文件的机器人模型文件夹至 3D 视图，或点击菜单栏“文件”→“加载URDF文件夹”进行加载。  
	**Load Model:** Drag and drop a folder containing URDF files into the 3D view, or use the menu “File” → “Load URDF Folder”.
2. **结构浏览**：在左侧“机器人URDF”树中浏览机器人结构，点击不同连杆或关节可查看详细属性。  
	**Structure Browsing:** Browse the robot structure in the left “Robot URDF” tree, click links or joints to view details.
3. **3D 交互**：右侧 3D 视图支持鼠标拖拽旋转、滚轮缩放、右键平移等操作，可自由调整观察角度。  
	**3D Interaction:** The right 3D view supports mouse drag to rotate, scroll to zoom, right-click to pan.
4. **关节控制**：通过 3D 视图上方的滑块面板调整关节角度，模型会实时响应变化。  
	**Joint Control:** Use the slider panel above the 3D view to adjust joint angles in real time.
5. **视图切换**：通过菜单栏“视图”可切换连杆模型和关节坐标系的显示与隐藏。  
	**View Switch:** Use the “View” menu to show/hide links and joint coordinate systems.
6. **重置视角**：如需重置视角或缩放到适合，可点击右上角悬浮按钮或使用快捷键。  
	**Reset View:** Click the floating button at the top right or use shortcuts to reset/fit the view.

---


## ❓ 常见问题 / FAQ

- **无法加载模型？**  
	**Model not loading?**  
	请确认 URDF 文件路径正确，且 package 目录下包含 meshes 文件夹。  
	Please check the URDF file path and ensure the package folder contains a meshes directory.
- **模型显示异常？**  
	**Model display abnormal?**  
	请检查网格文件格式是否支持（推荐 STL/DAE），并确保文件未损坏。  
	Please check if the mesh file format is supported (recommended: STL/DAE) and not corrupted.
- **如何重置视图？**  
	**How to reset the view?**  
	点击“视图”菜单中的“重置视角”即可。  
	Click “Reset View” in the “View” menu.

---


## ⌨️ 快捷键一览 / Keyboard Shortcuts

| 快捷键         | 功能           | Shortcut         | Description           |
| :------------- | :------------- | :--------------- | :-------------------- |
| Ctrl+O         | 加载URDF文件夹 | Ctrl+O           | Load URDF folder      |
| Ctrl+W         | 关闭URDF       | Ctrl+W           | Close URDF            |
| Ctrl+Q         | 退出程序       | Ctrl+Q           | Exit                  |
| Ctrl+R         | 重置视图       | Ctrl+R           | Reset view            |
| Ctrl+F         | 缩放到适合     | Ctrl+F           | Fit to view           |
| F1             | 帮助中心       | F1               | Help                  |
| Alt+A          | 关于           | Alt+A            | About                 |

---



## 🏗️ 构建与发布 / Build & Release

1. 使用 Visual Studio 2022 或更高版本打开 `URDFViewer.sln` 解决方案。  
	 Open `URDFViewer.sln` with Visual Studio 2022 or later.
2. 还原 NuGet 包并编译项目。  
	 Restore NuGet packages and build the project.
3. 编译完成后，使用 [Inno Setup](https://jrsoftware.org/isinfo.php) 打包生成安装程序：  
	 After build, use [Inno Setup](https://jrsoftware.org/isinfo.php) to package the installer:
	 - 脚本文件为 `URDFViewerPackage.iss`，可直接用 Inno Setup 编辑器打开并生成安装包。  
		 Script file: `URDFViewerPackage.iss`, open with Inno Setup to generate installer.
	 - 生成的安装包在 `SetupPackage/` 目录下，如 `URDFViewerSetup.exe`。  
		 The installer will be in the `SetupPackage/` directory, e.g. `URDFViewerSetup.exe`.
4. 用户可直接运行安装包进行软件安装：  
	 Users can install via the installer:
	 - 双击 `URDFViewerSetup.exe`，按照安装向导提示完成安装。  
		 Double-click `URDFViewerSetup.exe` and follow the wizard.
	 - 安装完成后可在开始菜单或桌面快捷方式启动 URDF Viewer。  
		 After installation, launch from Start Menu or desktop shortcut.

---



## 📦 依赖环境 / Dependencies

- Windows 10/11
- .NET 8
- 需通过 NuGet 自动还原以下依赖包：  
	The following NuGet packages are required:
	- [AssimpNet](https://www.nuget.org/packages/AssimpNet)（模型格式解析 / Model format parsing）
	- [HandyControl](https://www.nuget.org/packages/HandyControl)（现代化 WPF 控件库 / Modern WPF controls）
	- [HelixToolkit.Core.Wpf](https://www.nuget.org/packages/HelixToolkit.Core.Wpf)（3D 可视化 / 3D visualization）
	- [WindowsAPICodePack.Shell.CommonFileDialogs.Wpf](https://www.nuget.org/packages/WindowsAPICodePack.Shell.CommonFileDialogs.Wpf)（Windows 文件对话框支持 / Windows file dialogs）
	- 其他依赖包会在首次构建时自动还原  
		Other dependencies will be restored on first build.

---


## ℹ️ 关于 / About

- 版本：1.0  
	Version: 1.0
- 作者：JavenChan  
	Author: JavenChan
- 版权所有 © JavenChan 2025  
	Copyright © JavenChan 2025

---


## 📬 联系方式 / Contact

- 邮箱：javenchan1994@foxmail.com  
	Email: javenchan1994@foxmail.com
- 如有问题或建议，欢迎通过邮箱或 GitHub Issues 反馈。  
	For questions or suggestions, contact via email or GitHub Issues.

---


## ☕ 支持作者 / Support

如果本工具对你有帮助，欢迎扫码赞赏支持，感谢你的支持！  
If you find this tool helpful, feel free to support the author:

<div align="center">
	<img src="Assets/appreciate.jpg" alt="appreciate" width="180" />
</div>
