@echo off
rmdir /s /q dotnet
mkdir dotnet
pushd dotnet
copy ..\..\NUnit\*.* . > nul
echo Building...
csc /nologo /noconfig /target:library /out:Edulinq.dll /r:System.dll /r:System.Core.dll ..\..\src\Edulinq\*.cs
csc /nologo /noconfig /target:library /out:Edulinq.TestSupport.dll /r:nunit.framework.dll /r:System.dll /r:System.Core.dll ..\..\src\Edulinq.TestSupport\*.cs
csc /nologo /noconfig /target:library /out:Edulinq.BuiltIn_Tests.dll /d:ALL_TESTS /r:System.dll /r:System.Core.dll /r:nunit.framework.dll /r:Edulinq.TestSupport.dll ..\..\src\Edulinq.Tests\*.cs
csc /nologo /noconfig /target:library /out:Edulinq.Edulinq_Tests.dll /d:ALL_TESTS /r:System.dll /r:Edulinq.dll /r:nunit.framework.dll /r:Edulinq.TestSupport.dll ..\..\src\Edulinq.Tests\*.cs

REM Hack to force it to run in .NET 4
copy ..\nunit-console.exe.config . > nul

echo Running tests against built-in implementation...
nunit-console.exe Edulinq.BuiltIn_Tests.dll > builtin.txt
echo Running tests against Edulinq implementation...
nunit-console.exe Edulinq.Edulinq_Tests.dll > edulinq.txt
popd
