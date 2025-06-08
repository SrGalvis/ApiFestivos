pipeline{
	agent any
	
	environment{
		DOCKER_IMAGEN='apiFestivos'
		CONTAINER_NAME='dockerbdfestivos'
		APP_PORT='5235'
		HOST_PORT='7080'
		DOCKER_NETWORK='festivos_red'
	}

	stages{
			stage('Clonar'){
				steps{
					gir url: 'https://github.com/SrGalvis/ApiFestivos.git', branch: 'main' 
			}		
		}

		stage('Construir la imagen de Docker'){
			steps{
				script{
				bat " docker build -t %DOCKER_IMAGEN% ."
				}
			}
		}

	stage('Desplegar contenedor'){
		steps{
			script{
				bat "docker run -d --name %CONTAINER_NAME% -- network %DOCKER_NETWORK% -p %HOST_PORT%:%APP_PORT% %DOCKER_IMAGEN%"
				}
			}
		}
	}

	post {
		success{
			echo 'Despliegue exitoso.'
		}
		failure{
			echo 'Falto el despliegue.'
		}
	}
}


