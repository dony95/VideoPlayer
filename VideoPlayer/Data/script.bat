cd c:\Documents and settings\%username%\Documents\
#_TITLOVI
if exist vlc.loc goto start
where /R C: vlc.exe > vlc.loc
for /f %%i in ("vlc.loc") do set size=%%~zi
if %size% gtr 0 GOTO start
where /R D: vlc.exe > vlc.loc
:start
set /p texte=< vlc.loc
start ""  "%texte%" -vvv #_LINK #_SUB