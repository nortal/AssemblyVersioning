@ECHO OFF
..\..\..\.nuget\nuget.exe pack -Outputdirectory ..\output -Properties Configuration=Debug Nortal.Utilities.AssemblyVersioning.MsBuildTask.nuspec
pause
exit