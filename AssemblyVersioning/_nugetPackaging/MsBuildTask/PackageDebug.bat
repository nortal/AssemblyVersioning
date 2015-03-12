@ECHO OFF
%SYSTEMROOT%\Microsoft.NET\Framework64\v4.0.30319\Msbuild.exe /verbosity:m /nologo /p:Configuration=Debug ..\..\AssemblyVersioning.csproj
pause
..\..\..\.nuget\nuget.exe pack -Outputdirectory ..\output -Properties Configuration=Debug Nortal.Utilities.AssemblyVersioning.MsBuildTask.nuspec
pause
exit