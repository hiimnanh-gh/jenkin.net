pipeline {
    agent any

    environment {
        SOLUTION_NAME = "WebApplication1.sln"
        PROJECT_PATH = "WebApplication1/WebApplication1.csproj"
        PUBLISH_DIR = "publish"
        DEPLOY_DIR = "C:\\wwwroot\\myproject"
        IIS_SITE_NAME = "MySite"
        IIS_SITE_PORT = 81
        IIS_PHYSICAL_PATH = "C:\\wwwroot\\myproject"
        APP_POOL_NAME = "DefaultAppPool"
    }

    stages {
        stage('Clone') {
            steps {
                echo '📥 Cloning source code from GitHub'
                git branch: 'main', url: 'https://github.com/hiimnanh-gh/jenkin.net'
            }
        }

        stage('Restore Packages') {
            steps {
                echo '📦 Restoring NuGet packages'
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                echo '🔧 Building project'
                bat "dotnet build ${env.SOLUTION_NAME} --configuration Release"
            }
        }

        stage('Run Tests') {
            steps {
                echo '🧪 Running unit tests'
                bat "dotnet test ${env.SOLUTION_NAME} --no-build --verbosity normal"
            }
        }

        stage('Publish') {
            steps {
                echo '📤 Publishing to temporary folder'
                bat "dotnet publish ${env.PROJECT_PATH} -c Release -o ${env.PUBLISH_DIR}"
            }
        }

        stage('Stop IIS') {
            steps {
                echo '🛑 Stopping IIS to unlock files'
                powershell '''
                    Import-Module WebAdministration
                    if ((Get-WebAppPoolState -Name "${env.APP_POOL_NAME}").Value -eq "Started") {
                        Stop-WebAppPool -Name "${env.APP_POOL_NAME}"
                        Start-Sleep -Seconds 3
                    }
                '''
            }
        }

        stage('Clean & Copy Deploy Folder') {
            steps {
                echo '📁 Cleaning deploy folder and copying files'
                bat "rmdir /S /Q \"${env.DEPLOY_DIR}\" || echo Folder not found"
                bat "mkdir \"${env.DEPLOY_DIR}\""
                bat "xcopy \"%WORKSPACE%\\${env.PUBLISH_DIR}\" \"${env.DEPLOY_DIR}\" /E /Y /I /R"
            }
        }

        stage('Start IIS') {
            steps {
                echo '🚀 Starting IIS App Pool'
                powershell '''
                    Import-Module WebAdministration
                    Start-WebAppPool -Name "${env.APP_POOL_NAME}"
                '''
            }
        }

        stage('Ensure IIS Site Exists') {
            steps {
                echo '🌐 Deploying to IIS'
                powershell '''
                    Import-Module WebAdministration
                    if (-not (Test-Path "IIS:\\Sites\\${env.IIS_SITE_NAME}")) {
                        New-Website -Name "${env.IIS_SITE_NAME}" -Port ${env.IIS_SITE_PORT} -PhysicalPath "${env.IIS_PHYSICAL_PATH}" -ApplicationPool "${env.APP_POOL_NAME}"
                    } else {
                        Write-Host "IIS site already exists"
                    }
                '''
            }
        }

        stage('Done') {
            steps {
                echo "✅ Deployed successfully: http://localhost:${env.IIS_SITE_PORT}"
            }
        }
    }
}
