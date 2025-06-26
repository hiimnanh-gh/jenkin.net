pipeline {
    agent any

    environment {
        BASE_PORT = 5000
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
                script {
                    env.RUN_ID = "site_${BUILD_NUMBER}"
                    env.RUN_FOLDER = "C:\\deploy\\${env.RUN_ID}"
                }

                bat 'dotnet publish -c Release -o "%RUN_FOLDER%"'
            }
        }

        stage('Run Web App') {
            steps {
                script {
                    env.PORT = (env.BASE_PORT.toInteger() + env.BUILD_NUMBER.toInteger()).toString()
                }

                powershell """
                Start-Process -FilePath 'dotnet' `
                    -ArgumentList "'C:\\deploy\\site_${BUILD_NUMBER}\\WebApplication1.dll --urls=http://localhost:${PORT}'" `
                    -RedirectStandardOutput 'C:\\deploy\\site_${BUILD_NUMBER}\\log.txt' `
                    -RedirectStandardError 'C:\\deploy\\site_${BUILD_NUMBER}\\err.txt' `
                    -WindowStyle Hidden
                """

                echo " App is now running at: http://localhost:${PORT}"

                script {
                    currentBuild.description = "<a href='http://localhost:${PORT}'>🌐 App on :${PORT}</a>"
                }
            }
        }
    }
}
