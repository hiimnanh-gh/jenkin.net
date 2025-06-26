pipeline {
    agent any

    environment {
        SOLUTION_NAME = "WebApplication1.sln"
        PROJECT_PATH = "WebApplication1/WebApplication1.csproj"
        PUBLISH_DIR = "publish"
        DEPLOY_DIR = "C:\\wwwroot\\myproject"
        IIS_SITE_NAME = "MySite"
        IIS_SITE_PORT = 81
        IIS_PHYSICAL_PATH = "C:\\test1-netcore"
    }

    stages {
        stage('Clone') {
            steps {
                echo ' Cloning source code'
                git branch: 'main', url: 'https://github.com/hiimnanh-gh/jenkin.net'
            }
        }

        stage('Restore Package') {
            steps {
                echo ' Restoring packages'
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                echo ' Building project'
                bat "dotnet build ${env.SOLUTION_NAME} --configuration Release"
            }
        }

        stage('Tests') {
            steps {
                echo ' Running tests'
                bat "dotnet test ${env.SOLUTION_NAME} --no-build --verbosity normal"
            }
        }

        stage('Publish to Folder') {
            steps {
                echo ' Publishing project'
                bat "dotnet publish ${env.PROJECT_PATH} -c Release -o ${env.PUBLISH_DIR}"
            }
        }

        stage('Copy to Deploy Folder') {
            steps {
                echo ' Copy publish to deployment folder'
                bat "xcopy \"%WORKSPACE%\\${env.PUBLISH_DIR}\" \"${env.DEPLOY_DIR}\" /E /Y /I /R"
            }
        }

        stage('Deploy to IIS') {
            steps {
                echo ' Deploying to IIS'
                powershell """
                    Import-Module WebAdministration

                    if (-not (Test-Path 'IIS:\\Sites\\${env.IIS_SITE_NAME}')) {
                        New-Website -Name '${env.IIS_SITE_NAME}' -Port ${env.IIS_SITE_PORT} -PhysicalPath '${env.IIS_PHYSICAL_PATH}' -ApplicationPool 'DefaultAppPool'
                    } else {
                        Write-Output ' Restarting site...'
                        Restart-WebAppPool -Name 'DefaultAppPool'
                    }
                """
            }
        }

        stage(' Done') {
            steps {
                echo " Application deployed at: http://localhost:${env.IIS_SITE_PORT}"
            }
        }
    }
}
