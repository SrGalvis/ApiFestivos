pipeline {
	agent any

	environment {
		DOCKER_IMAGEN = 'apifestivos'
		CONTAINER_NAME = 'apifestivos-container'
		APP_PORT = '5289'
		HOST_PORT = '7080'
		DOCKER_NETWORK = 'festivos_red'
	}

	stages {
		stage('Clonar') {
			steps {
				git url: 'https://github.com/SrGalvis/ApiFestivos.git', branch: 'main'
			}
		}

		stage('Verificar Docker') {
			steps {
				bat 'docker --version'
			}
		}

		stage('Construir la imagen de Docker') {
			steps {
				script {
					bat "docker build -t %DOCKER_IMAGEN%:latest ."
				}
			}
		}

		stage('Limpiar contenedor existente') {
			steps {
				script {
					catchError(buildResult: 'SUCCESS', stageResult: 'UNSTABLE') {
						bat """
						docker container inspect %CONTAINER_NAME% >nul 2>&1 && (
							docker container stop %CONTAINER_NAME%
							docker container rm %CONTAINER_NAME%
						) || echo El contenedor '%CONTAINER_NAME%' no existe o ya fue eliminado.
						"""
					}
				}
			}
		}

		stage('Verificar red Docker') {
			steps {
				script {
					bat """
					docker network inspect %DOCKER_NETWORK% >nul 2>&1 || docker network create %DOCKER_NETWORK%
					"""
				}
			}
		}

		stage('Desplegar contenedor') {
			steps {
				script {
					bat "docker run -d --name %CONTAINER_NAME% --network %DOCKER_NETWORK% -p %HOST_PORT%:%APP_PORT% %DOCKER_IMAGEN%:latest"
				}
			}
		}
	}

	post {
		success {
			echo 'Despliegue exitoso.'
		}
		failure {
			echo 'Falló el despliegue.'
		}
	}
}
