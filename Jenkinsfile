pipeline {
    agent any

    environment {
        DEPLOY_PATH = "C:\\inetpub\\MySite"
        SITE_NAME = "MySite"
        SITE_PORT = "81"
    }

    stages {
        stage('Clone') {
            steps {
                git branch: 'main', url: 'https://github.com/hiimnanh-gh/jenkin.net'
            }
        }

        stage('Restore & Build') {
            steps {
                bat 'dotnet restore'
                bat 'dotnet build --configuration Release'
            }
        }

        stage('Test') {
            steps {
                bat 'dotnet test --no-build --configuration Release --verbosity normal'
            }
        }

        stage('Publish') {
            steps {
                bat "dotnet publish WebApplication1/WebApplication1.csproj -c Release -o \"${env.DEPLOY_PATH}\""
            }
        }

        stage('Deploy to IIS') {
            steps {
                powershell """
                    Import-Module WebAdministration

                    \$siteName = '${env.SITE_NAME}'
                    \$sitePath = '${env.DEPLOY_PATH}'
                    \$port = ${env.SITE_PORT}

                    if (-not (Test-Path "IIS:\\Sites\\\$siteName")) {
                        Write-Output "🔧 Creating new IIS site..."
                        New-Website -Name \$siteName -Port \$port -PhysicalPath \$sitePath -ApplicationPool "DefaultAppPool"
                    } else {
                        Write-Output "🔄 Restarting AppPool..."
                        Restart-WebAppPool -Name "DefaultAppPool"
                    }
                """
            }
        }

        stage('Done') {
            steps {
                echo "✅ Site deployed at: http://localhost:${env.SITE_PORT}"
            }
        }
    }
}
