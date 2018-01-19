cd c:\Documents and settings\%username%\Documents\
#_TITLOVI
FOR /F "usebackq tokens=3*" %%A IN (`REG QUERY "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\VLC media player" /v DisplayIcon`) DO (
    set appdir=%%A %%B
    )
%appdir% -vvv #_LINK #_SUB