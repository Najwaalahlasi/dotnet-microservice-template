@echo off
echo Starting Company.Template API locally...
echo.

echo Step 1: Building the solution...
dotnet build
if %ERRORLEVEL% neq 0 (
    echo Build failed!
    pause
    exit /b 1
)

echo.
echo Step 2: Starting the API (without database dependencies)...
echo Note: Database and RabbitMQ features will be disabled for this demo
echo.

cd src\Company.Template.Api
dotnet run --environment Development