pipeline {
    agent any

    environment {
        SOLUTION_NAME = "WebApplication1.sln"
        PROJECT_PATH = "WebApplication1/WebApplication1.csproj"
        PUBLISH_DIR = "publish"
        DEPLOY_DIR = "C:\\wwwroot\\myproject"
        IIS_SITE_NAME = "MySite"
        IIS_SITE_PORT = 80
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
                echo '🛑 Stopping IIS App Pool'
                powershell """
                    Import-Module WebAdministration
                    \$pool = Get-WebAppPoolState -Name '${env.APP_POOL_NAME}'
                    if (\$pool.Value -eq 'Started') {
                        Stop-WebAppPool -Name '${env.APP_POOL_NAME}'
                        Start-Sleep -Seconds 2
                    }
                """
            }
        }

        stage('Clean & Copy Deploy Folder') {
            steps {
                echo '📁 Cleaning and copying files to deployment folder'
                bat "rmdir /S /Q \"${env.DEPLOY_DIR}\" || echo Folder does not exist"
                bat "mkdir \"${env.DEPLOY_DIR}\""
                bat "xcopy \"%WORKSPACE%\\${env.PUBLISH_DIR}\" \"${env.DEPLOY_DIR}\" /E /Y /I /R"
            }
        }

        stage('Start IIS') {
            steps {
                echo '🚀 Starting IIS App Pool'
                powershell """
                    Import-Module WebAdministration
                    Start-WebAppPool -Name '${env.APP_POOL_NAME}'
                """
            }
        }

        stage('Ensure IIS Site Exists') {
            steps {
                echo '🌐 Ensuring IIS site exists'
                powershell """
                    Import-Module WebAdministration
                    if (-not (Test-Path "IIS:\\Sites\\${env.IIS_SITE_NAME}")) {
                        New-Website -Name '${env.IIS_SITE_NAME}' -Port ${env.IIS_SITE_PORT} -PhysicalPath '${env.IIS_PHYSICAL_PATH}' -ApplicationPool '${env.APP_POOL_NAME}'
                    } else {
                        Write-Host 'IIS site already exists.'
                    }
                """
            }
        }

        stage('Done') {
            steps {
                echo "✅ Deployment completed: http://localhost:${env.IIS_SITE_PORT}"
            }
        }
    }
}
