@echo off
rmdir /s /q mono40
mkdir mono40
pushd mono40
copy ..\..\NUnit\*.* . > nul
echo Building...
call dmcs -target:library -out:Edulinq.dll ../Edulinq/*.cs
call dmcs -target:library -out:Edulinq.TestSupport.dll -r:./nunit.framework.dll ../../src/Edulinq.TestSupport/*.cs
call dmcs -target:library -out:Edulinq.BuiltIn_Tests.dll -d:ALL_TESTS -r:./nunit.framework.dll,Edulinq.TestSupport.dll ../../src/Edulinq.Tests/*.cs
call dmcs -target:library -out:Edulinq.Edulinq_Tests.dll -d:ALL_TESTS -r:./nunit.framework.dll,Edulinq.TestSupport.dll,Edulinq.dll ../../src/Edulinq.Tests/*.cs
echo Running tests against built-in implementation...
mono --runtime=v4.0.30319 nunit-console.exe Edulinq.BuiltIn_Tests.dll > builtin.txt
echo Running tests against Edulinq implementation...
mono --runtime=v4.0.30319 nunit-console.exe Edulinq.Edulinq_Tests.dll > edulinq.txt
popd
