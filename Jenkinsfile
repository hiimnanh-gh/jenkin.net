pipeline {
    agent any

    environment {
        SOLUTION_NAME = "WebApplication1.sln"
        PROJECT_PATH = "WebApplication1/WebApplication1.csproj"
        PUBLISH_DIR = "publish"
        DEPLOY_DIR = "C:\\wwwroot\\myproject"
        IIS_SITE_NAME = "MySite"
        IIS_SITE_PORT = 81
        IIS_APP_POOL = "DefaultAppPool"
        IIS_PHYSICAL_PATH = "C:\\wwwroot\\myproject"
    }

    stages {
        stage('Clone') {
            steps {
                echo '📥 Cloning source code from GitHub'
                git branch: 'main', url: 'https://github.com/hiimnanh-gh/jenkin.net'
            }
        }

        stage('Restore Package') {
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

        stage('Tests') {
            steps {
                echo '🧪 Running tests'
                bat "dotnet test ${env.SOLUTION_NAME} --no-build --verbosity normal"
            }
        }

        stage('Publish to Folder') {
            steps {
                echo '📤 Publishing project to temporary folder'
                bat "dotnet publish ${env.PROJECT_PATH} -c Release -o ${env.PUBLISH_DIR}"
            }
        }

        stage('Stop IIS App Pool') {
            steps {
                echo '🛑 Stopping IIS App Pool to prevent file lock'
                powershell """
                    Import-Module WebAdministration
                    if (Test-Path 'IIS:\\AppPools\\${env.IIS_APP_POOL}') {
                        Stop-WebAppPool -Name '${env.IIS_APP_POOL}'
                    }
                """
            }
        }

        stage('Copy to Deploy Folder') {
            steps {
                echo '📁 Copying published files to deployment folder'
                bat "xcopy \"%WORKSPACE%\\${env.PUBLISH_DIR}\" \"${env.DEPLOY_DIR}\" /E /Y /I /R"
            }
        }

        stage('Start IIS App Pool') {
            steps {
                echo '▶️ Starting IIS App Pool'
                powershell """
                    Import-Module WebAdministration
                    if (Test-Path 'IIS:\\AppPools\\${env.IIS_APP_POOL}') {
                        Start-WebAppPool -Name '${env.IIS_APP_POOL}'
                    }
                """
            }
        }

        stage('Deploy to IIS') {
            steps {
                echo '🌐 Creating IIS site if not exists'
                powershell """
                    Import-Module WebAdministration
                    if (-not (Test-Path 'IIS:\\Sites\\${env.IIS_SITE_NAME}')) {
                        New-Website -Name '${env.IIS_SITE_NAME}' -Port ${env.IIS_SITE_PORT} -PhysicalPath '${env.IIS_PHYSICAL_PATH}' -ApplicationPool '${env.IIS_APP_POOL}'
                    } else {
                        Write-Output '✅ Website đã tồn tại, không cần tạo lại.'
                    }
                """
            }
        }

        stage('Done') {
            steps {
                echo "✅ Triển khai thành công: http://localhost:${env.IIS_SITE_PORT}"
            }
        }
    }
}
