set path=%~dp0%
set fa=%path%startup.htm
set fb=%path%startup2.htm
reg add "HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\MenuExt\����ͼƬ" /t REG_SZ /d "%fa%"
reg add "HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\MenuExt\����ͼƬ" /v Contexts /t REG_DWORD /d "2"
reg add "HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\MenuExt\��������ͼƬ" /t REG_SZ /d "%fb%"
reg add "HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\MenuExt\��������ͼƬ" /v Contexts /t REG_DWORD /d "1"
pause