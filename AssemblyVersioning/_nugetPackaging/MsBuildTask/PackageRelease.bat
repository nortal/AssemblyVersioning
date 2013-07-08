@ECHO OFF
..\..\..\.nuget\nuget.exe pack -Outputdirectory ..\output -Properties Configuration=Release Nortal.Utilities.AssemblyVersioning.MsBuildTask.nuspec
pause
exit