@ECHO OFF
 
set OLDDIR=%~dp0
echo %OLDDIR%

REM The following directory is for .NET 4.5
set DOTNETFX4=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319
set PATH=%PATH%;%DOTNETFX4%
 
echo Uninstalling Sample Window Service...
echo ---------------------------------------------------
InstallUtil /u %OLDDIR%\SampleWindowService.exe
echo ---------------------------------------------------
echo Done.
pause