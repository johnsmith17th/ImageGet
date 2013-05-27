set path=%~dp0%
set fa=%path%startup.htm
set fb=%path%startup2.htm
reg add "HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\MenuExt\下载图片" /t REG_SZ /d "%fa%"
reg add "HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\MenuExt\下载图片" /v Contexts /t REG_DWORD /d "2"
reg add "HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\MenuExt\下载所有图片" /t REG_SZ /d "%fb%"
reg add "HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\MenuExt\下载所有图片" /v Contexts /t REG_DWORD /d "1"
pause