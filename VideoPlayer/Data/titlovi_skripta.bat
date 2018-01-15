if not exist Titlovi mkdir Titlovi
cd titlovi
set "URL=#_URL"
set "SaveAs=#_FILENAME"
powershell "Import-Module BitsTransfer; Start-BitsTransfer '%URL%' '%SaveAs%'"
cd ..