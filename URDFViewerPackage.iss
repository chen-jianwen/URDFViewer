; -- Inno Setup 脚本模板 --
[Setup]
AppName=URDFImporter
AppVersion=1.0.0
DefaultDirName={autopf}\URDFImporter
DefaultGroupName=URDFImporter
OutputDir=Output
OutputBaseFilename=URDFImporterSetup
Compression=lzma
SolidCompression=yes
SetupIconFile=Assets\Icon.ico

[Files]
Source: "bin\Release\net8.0-windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "Assets\Icon.ico"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\URDFImporter"; Filename: "{app}\URDFImporter.exe"; 
Name: "{commondesktop}\URDFImporter"; Filename: "{app}\URDFImporter.exe"; Tasks: desktopicon; 

[Tasks]
Name: "desktopicon"; Description: "创建桌面快捷方式"; GroupDescription: "附加任务："

[Run]
Filename: "{app}\URDFImporter.exe"; Description: "运行 URDFImporter"; Flags: nowait postinstall skipifsilent