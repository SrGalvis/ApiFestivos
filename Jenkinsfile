pipeline {
	agent any
	
	environment {
		DOCKER_IMAGEN = 'apiFestivos'
		CONTAINER_NAME = 'dockerbdfestivos'
		APP_PORT = '5235'
		HOST_PORT = '7080'
		DOCKER_NETWORK = 'festivos_red'
	}

	stages {
		stage('Clonar') {
			steps {
				git url: 'https://github.com/SrGalvis/ApiFestivos.git', branch: 'main'
			}
		}

	stage('Construir la imagen de Docker') {
	    steps {
	        script {
	            bat 'docker build -t apifestivos:latest .'
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

		stage('Desplegar contenedor'){
			steps{
				script{
					bat "docker run -d --name %CONTAINER_NAME% --network %DOCKER_NETWORK% -p %HOST_PORT%:%APP_PORT% %DOCKER_IMAGE%"
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
