Unicode True

#####################################################################
!include LogicLib.nsh
!ifndef IPersistFile
!define IPersistFile {0000010b-0000-0000-c000-000000000046}
!endif
!ifndef CLSID_ShellLink
!define CLSID_ShellLink {00021401-0000-0000-C000-000000000046}
!define IID_IShellLinkA {000214EE-0000-0000-C000-000000000046}
!define IID_IShellLinkW {000214F9-0000-0000-C000-000000000046}
!define IShellLinkDataList {45e2b4ae-b1c3-11d0-b92f-00a0c90312e1}
	!ifdef NSIS_UNICODE
	!define IID_IShellLink ${IID_IShellLinkW}
	!else
	!define IID_IShellLink ${IID_IShellLinkA}
	!endif
!endif
 
 
 
Function ShellLinkSetRunAs
System::Store S
pop $9
System::Call "ole32::CoCreateInstance(g'${CLSID_ShellLink}',i0,i1,g'${IID_IShellLink}',*i.r1)i.r0"
${If} $0 = 0
	System::Call "$1->0(g'${IPersistFile}',*i.r2)i.r0" ;QI
	${If} $0 = 0
		System::Call "$2->5(w '$9',i 0)i.r0" ;Load
		${If} $0 = 0
			System::Call "$1->0(g'${IShellLinkDataList}',*i.r3)i.r0" ;QI
			${If} $0 = 0
				System::Call "$3->6(*i.r4)i.r0" ;GetFlags
				${If} $0 = 0
					System::Call "$3->7(i $4|0x2000)i.r0" ;SetFlags ;SLDF_RUNAS_USER
					${If} $0 = 0
						System::Call "$2->6(w '$9',i1)i.r0" ;Save
					${EndIf}
				${EndIf}
				System::Call "$3->2()" ;Release
			${EndIf}
		System::Call "$2->2()" ;Release
		${EndIf}
	${EndIf}
	System::Call "$1->2()" ;Release
${EndIf}
push $0
System::Store L
FunctionEnd
#####################################################################

Function LaunchConfig
    ExecShell "" "$INSTDIR\Configure.exe"
FunctionEnd

# 日本語UI
LoadLanguageFile "${NSISDIR}\Contrib\Language files\Japanese.nlf"
# アプリケーション名
Name "MailToStarPRNT"
# 作成されるインストーラ
OutFile "SetupMailToStarPRNT.exe"
# インストールされるディレクトリ
InstallDir "$PROGRAMFILES64\MailToStarPRNT"

# ページ
Page directory
Page instfiles "" "" LaunchConfig

# デフォルト セクション
Section
  # 出力先を指定します。
  SetOutPath "$INSTDIR"

  # インストールされるファイル
  File /x *.ini /x *.pdb "bin\Release\*"
  File "Configure\bin\Release\Configure.exe*"
  File "start-service.bat"
  File "stop-service.bat"
  File "MailToStarPRNT_EventLog.xml"

  ExecWait '"$INSTDIR\MailToStarPRNT.exe" install'

  # アンインストーラを出力
  WriteUninstaller "$INSTDIR\Uninstall.exe"

  # スタート メニューにショートカットを登録
  CreateDirectory "$SMPROGRAMS\MailToStarPRNT"
  SetOutPath "$INSTDIR"

  CreateShortcut "$SMPROGRAMS\MailToStarPRNT\印刷アプリケーションの開始.lnk" "$INSTDIR\start-service.bat" ""
  Push "$SMPROGRAMS\MailToStarPRNT\印刷アプリケーションの開始.lnk"
  Call ShellLinkSetRunAs
  Pop $0

  CreateShortcut "$SMPROGRAMS\MailToStarPRNT\印刷アプリケーションの停止.lnk" "$INSTDIR\stop-service.bat" ""
  Push "$SMPROGRAMS\MailToStarPRNT\印刷アプリケーションの停止.lnk"
  Call ShellLinkSetRunAs
  Pop $0

  CreateShortcut "$SMPROGRAMS\MailToStarPRNT\設定.lnk" "$INSTDIR\Configure.exe" ""
  Push "$SMPROGRAMS\MailToStarPRNT\設定.lnk"
  Call ShellLinkSetRunAs
  Pop $0

  CreateShortcut "$SMPROGRAMS\MailToStarPRNT\ログ表示.lnk" "$SYSDIR\eventvwr.exe" '/v:"$INSTDIR\MailToStarPRNT_EventLog.xml"'
  CreateShortcut "$SMPROGRAMS\MailToStarPRNT\アンインストール.lnk" "$INSTDIR\Uninstall.exe" ""

  # レジストリに登録
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MailToStarPRNT" "DisplayName" "MailToStarPRNT"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MailToStarPRNT" "UninstallString" '"$INSTDIR\Uninstall.exe"'
SectionEnd

# アンインストーラ
Section "Uninstall"

  ExecWait '"$INSTDIR\MailToStarPRNT.exe" uninstall'

  # アンインストーラを削除
  Delete "$INSTDIR\Uninstall.exe"
  # ファイルを削除
  Delete "$INSTDIR\*"
  # ディレクトリを削除
  RMDir "$INSTDIR"

  Delete "$SMPROGRAMS\MailToStarPRNT\印刷アプリケーションの開始.lnk"
  Delete "$SMPROGRAMS\MailToStarPRNT\印刷アプリケーションの停止.lnk"
  Delete "$SMPROGRAMS\MailToStarPRNT\設定.lnk"
  Delete "$SMPROGRAMS\MailToStarPRNT\ログ表示.lnk"
  Delete "$SMPROGRAMS\MailToStarPRNT\アンインストール.lnk"
  RMDir "$SMPROGRAMS\MailToStarPRNT"

  # レジストリ キーを削除
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MailToStarPRNT"
SectionEnd
