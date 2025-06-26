pipeline {
    agent any

    environment {
        SOLUTION_NAME = "WebApplication1.sln"
        PROJECT_PATH = "WebApplication1/WebApplication1.csproj"
        PUBLISH_DIR = "publish"
        DEPLOY_DIR = "C:\\wwwroot\\myproject"
        IIS_SITE_NAME = "MySite"
        IIS_SITE_PORT = 81
        IIS_PHYSICAL_PATH = "${DEPLOY_DIR}"
        APP_POOL_NAME = "DefaultAppPool"
    }

    stages {

        stage('clone') {
            steps {
                echo '📥 Cloning source code from GitHub'
                git branch: 'main', url: 'https://github.com/hiimnanh-gh/jenkin.net'
            }
        }

        stage('restore package') {
            steps {
                echo '📦 Restoring NuGet packages'
                bat 'dotnet restore'
            }
        }

        stage('build') {
            steps {
                echo '🔧 Building project'
                bat "dotnet build ${env.SOLUTION_NAME} --configuration Release"
            }
        }

        stage('tests') {
            steps {
                echo '🧪 Running tests'
                bat "dotnet test ${env.SOLUTION_NAME} --no-build --verbosity normal"
            }
        }

        stage('public den t thu muc') {
            steps {
                echo '📤 Publishing project to temporary folder'
                bat "dotnet publish ${env.PROJECT_PATH} -c Release -o ${env.PUBLISH_DIR}"
            }
        }

        stage('Stop IIS App Pool') {
            steps {
                echo '🛑 Stopping IIS App Pool to prevent file lock'
                powershell "Stop-WebAppPool -Name '${env.APP_POOL_NAME}'"
            }
        }

        stage('Publish') {
            steps {
                echo '📁 Copying published files to deployment folder'
                bat "xcopy \"%WORKSPACE%\\${env.PUBLISH_DIR}\" \"${env.DEPLOY_DIR}\" /E /Y /I /R"
            }
        }

        stage('Start IIS App Pool') {
            steps {
                echo '🚀 Starting IIS App Pool'
                powershell "Start-WebAppPool -Name '${env.APP_POOL_NAME}'"
            }
        }

        stage('Deploy to IIS') {
            steps {
                echo '🌐 Deploying to IIS'
                powershell """
                    Import-Module WebAdministration
                    if (-not (Test-Path IIS:\\Sites\\${env.IIS_SITE_NAME})) {
                        New-Website -Name '${env.IIS_SITE_NAME}' -Port ${env.IIS_SITE_PORT} -PhysicalPath '${env.IIS_PHYSICAL_PATH}' -ApplicationPool '${env.APP_POOL_NAME}'
                    }
                """
            }
        }

        stage('Done') {
            steps {
                echo "✅ Application deployed at: http://localhost:${env.IIS_SITE_PORT}"
            }
        }
    }
}
