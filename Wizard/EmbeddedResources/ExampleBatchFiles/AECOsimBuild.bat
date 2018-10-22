echo off
call "%~fs2ABDDeveloperShell.bat" %~fs1 %~fs2
Set MS=%~fs1
set MSMDE=%~fs2
set ABDSDK=%MSMDE%ABDSDK\
pushd %~4
Bmake %~5