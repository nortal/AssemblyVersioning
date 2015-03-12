@ECHO OFF
cd
%SYSTEMROOT%\Microsoft.NET\Framework64\v4.0.30319\Msbuild.exe /verbosity:m /nologo /p:Configuration=Release ..\..\AssemblyVersioning.csproj
pause
..\..\..\.nuget\nuget.exe pack -Outputdirectory ..\output -Properties Configuration=Release Nortal.Utilities.AssemblyVersioning.MsBuildTask.nuspec
pause
exit