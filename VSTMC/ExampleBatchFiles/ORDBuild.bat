echo off
call "%~fs2OpenRoadsDesignerDeveloperShell.bat" %~fs1 %~fs2
Set MS=%~fs1
set MSMDE=%~fs2
pushd %~4
Bmake %~5