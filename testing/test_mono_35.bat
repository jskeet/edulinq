@echo off
rmdir /s /q mono35
mkdir mono35
pushd mono35
copy ..\..\NUnit\*.* . > nul
copy ..\..\src\LinqBridge.dll . > nul
echo Building...
call gmcs -target:library -out:Edulinq.dll -d:DOTNET35_ONLY ../Edulinq/*.cs
call gmcs -target:library -out:Edulinq.TestSupport.dll -d:DOTNET35_ONLY -r:./nunit.framework.dll ../../src/Edulinq.TestSupport/*.cs
call gmcs -target:library -out:Edulinq.BuiltIn_Tests.dll -d:ALL_TESTS -d:DOTNET35_ONLY -r:./nunit.framework.dll,Edulinq.TestSupport.dll ../../src/Edulinq.Tests/*.cs
call gmcs -target:library -out:Edulinq.Edulinq_Tests.dll -d:ALL_TESTS -d:DOTNET35_ONLY -r:./nunit.framework.dll,Edulinq.TestSupport.dll,Edulinq.dll ../../src/Edulinq.Tests/*.cs
call gmcs -noconfig -target:library -out:Edulinq.LinqBridge_Tests.dll -d:ALL_TESTS -d:DOTNET35_ONLY -d:LINQBRIDGE -r:./nunit.framework.dll,Edulinq.TestSupport.dll,LinqBridge.dll,System.dll ../../src/Edulinq.Tests/*.cs
echo Running tests against built-in implementation...
mono nunit-console.exe Edulinq.BuiltIn_Tests.dll > builtin.txt
echo Running tests against Edulinq implementation...
mono nunit-console.exe Edulinq.Edulinq_Tests.dll > edulinq.txt
echo Running tests against LinqBridge implementation...
mono nunit-console.exe Edulinq.LinqBridge_Tests.dll > linqbridge.txt
popd
