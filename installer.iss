[Setup]
AppName=ChurchSermonsMetaData
AppVersion=1.0
DefaultDirName={autopf}\ChurchSermonsMetaData
DefaultGroupName=ChurchSermonsMetaData
UninstallDisplayIcon={app}\ChurchSermonsMetaData.exe
Compression=lzma2
SolidCompression=yes
OutputDir=.
OutputBaseFilename=ChurchSermonsMetaData-Setup
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

[Files]
; Source points to the publish directory created by the GitHub workflow
Source: "publish_output\*"; DestDir: "{app}"; Flags: recursesubdirs createallsubdirs

[Icons]
; Creates shortcuts for your users automatically
Name: "{group}\ChurchSermonsMetaData"; Filename: "{app}\ChurchSermonsMetaData.exe"
Name: "{autodesktop}\ChurchSermonsMetaData"; Filename: "{app}\ChurchSermonsMetaData.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Run]
; Offers to run the app right after installation completes
Filename: "{app}\ChurchSermonsMetaData.exe"; Description: "{cm:LaunchProgram,ChurchSermonsMetaData}"; Flags: nowait postinstall skipifsilent