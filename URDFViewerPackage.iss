; -- Inno Setup 脚本模板 --
[Setup]
AppName=URDFViewer
AppVersion=1.0.0
DefaultDirName={autopf}\URDFViewer
DefaultGroupName=URDFViewer
OutputDir=Output
OutputBaseFilename=URDFViewerSetup
Compression=lzma
SolidCompression=yes
SetupIconFile=Assets\Icon.ico

[Files]
Source: "bin\Release\net8.0-windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "Assets\Icon.ico"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\URDFViewer"; Filename: "{app}\URDFViewer.exe"; 
Name: "{commondesktop}\URDFViewer"; Filename: "{app}\URDFViewer.exe"; Tasks: desktopicon; 

[Tasks]
Name: "desktopicon"; Description: "创建桌面快捷方式"; GroupDescription: "附加任务："

[Run]
Filename: "{app}\URDFViewer.exe"; Description: "运行 URDFViewer"; Flags: nowait postinstall skipifsilent