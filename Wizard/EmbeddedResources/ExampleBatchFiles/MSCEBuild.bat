echo off
call "%~fs2MicroStationDeveloperShell.bat" %~fs1 %~fs2 %~fs3
pushd %~4
Bmake %~5