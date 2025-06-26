pipeline {
    agent any

    stages {
        stage('Clone') {
            steps {
                git branch: 'main', url: 'https://github.com/hiimnanh-gh/jenkin.net'
            }
        }

        stage('Restore Packages') {
            steps {
                echo 'Restoring packages...'
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                echo 'Building project...'
                bat 'dotnet build --configuration Release'
            }
        }

        stage('Test') {
            steps {
                echo 'Running tests...'
                bat 'dotnet test --no-build --verbosity normal'
            }
        }

        stage('Publish to Temp Folder') {
            steps {
                echo 'Publishing project...'
                bat 'dotnet publish -c Release -o ./publish'
            }
        }

        stage('Copy to IIS Folder') {
            steps {
                echo 'Copying files to IIS folder...'
                bat '''
                    icacls "C:\\wwwroot\\myproject" /grant Everyone:F /T
                    xcopy "%WORKSPACE%\\publish" "C:\\wwwroot\\myproject" /E /Y /I /R
                '''
            }
        }

        stage('Deploy to IIS') {
            steps {
                powershell '''
                    Import-Module WebAdministration

                    if (-not (Test-Path IIS:\\Sites\\MySite)) {
                        New-Website -Name "MySite" -Port 81 -PhysicalPath "C:\\wwwroot\\myproject" -ApplicationPool "DefaultAppPool"
                    } else {
                        Restart-WebAppPool -Name "DefaultAppPool"
                    }
                '''
            }
        }
    }
}
