@echo off
cls
echo ========================================
echo   Company.Template - Quick Demo
echo ========================================
echo.

echo Step 1: Building the solution...
dotnet build
if %ERRORLEVEL% neq 0 (
    echo ❌ Build failed!
    pause
    exit /b 1
)
echo ✅ Build successful!
echo.

echo Step 2: Running unit tests...
dotnet test tests/Company.Template.UnitTests --verbosity quiet
if %ERRORLEVEL% neq 0 (
    echo ❌ Tests failed!
    pause
    exit /b 1
)
echo ✅ All tests passed!
echo.

echo Step 3: Starting the API...
echo.
echo 🚀 API will start with in-memory database
echo 📖 Open https://localhost:5001/swagger in your browser
echo 🔍 Check https://localhost:5001/health for health status
echo.
echo Press Ctrl+C to stop the API when done testing
echo.
pause

cd src\Company.Template.Api
dotnet run