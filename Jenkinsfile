pipeline {
    agent any
    environment {
        DOCKER_IMAGEN = 'apifestivos'
        CONTAINER_NAME = 'apifestivos-container'
        APP_PORT = '80'
        HOST_PORT = '7080'
        DOCKER_NETWORK = 'red'
    }
    stages {
        stage('Clonar') {
            steps {
                git url: 'https://github.com/SrGalvis/ApiFestivos.git', branch: 'main'
            }
        }
        stage('Restaurar dependencias') {
            steps {
                bat 'dotnet restore'
            }
        }
        stage('Ejecutar pruebas') {
            steps {
                bat 'dotnet test --configuration Release --logger trx'
            }
            post {
                always {
                    publishTestResults testResultsPattern: '**/*.trx'
                }
            }
        }
        stage('Análisis de código') {
            steps {
                bat 'dotnet build --configuration Release --no-restore'
            }
        }
        stage('Construir imagen Docker') {
            steps {
                bat "docker build -t %DOCKER_IMAGEN%:latest ."
            }
        }
        stage('Desplegar con Docker Compose') {
            steps {
                bat "docker-compose down"
                bat "docker-compose up -d"
            }
        }
        stage('Verificar despliegue') {
            steps {
                script {
                    sleep(time: 30, unit: 'SECONDS')
                    bat "curl -f http://localhost:%HOST_PORT%/health || exit 1"
                }
            }
        }
    }
    post {
        success {
            echo 'Despliegue exitoso - API disponible en http://localhost:7080'
        }
        failure {
            echo 'Falló el despliegue - Revisar logs'
            bat 'docker-compose logs'
        }
        always {
            bat 'docker system prune -f'
        }
    }
}
