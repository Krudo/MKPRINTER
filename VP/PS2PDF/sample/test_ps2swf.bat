for %%F in (*.ps) do ..\bin\ps2pdf.exe "%%F" "%%~dpnF.swf"

pause
