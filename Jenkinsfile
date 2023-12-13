pipeline {
  agent any
  tools {
    dockerTool 'docker'
  }

  stages {
    stage('Checkout') {
      steps {
        checkout scm
      }
    }

    stage('Dotnet Unit tests') {
      steps {
        dir('Backend.Tests') {
              sh 'dotnet test'
            }
      }
    }

    stage('Analyses SonarQube') {
      parallel {
        stage('SonarQube analysis - Back') {
          steps {
            dir('Backend') {
              script {
                def scannerHome = tool 'sonar'
                def scannerHomeBuild = tool 'sonarBuild'
                withSonarQubeEnv('SonarQube') {
                  sh "sudo ${scannerHomeBuild} begin /k:${SONAR_PROJECT_KEY} /d:sonar.host.url=${SONAR_SERVER_URL} /d:sonar.login=${SONAR_TOKEN_BACK}"
                  sh 'dotnet build'
                  sh "sudo ${scannerHomeBuild} end /d:sonar.login=${SONAR_TOKEN_BACK}"
                }
              }
            }
          }
        }

        stage('SonarQube analysis - Front') {
          steps {
            dir('client') {
              script {
                def scannerHome = tool 'sonar'
                withSonarQubeEnv('SonarQube') {
                  sh ""
                  "${scannerHome}/bin/sonar-scanner \
                    -Dsonar.projectKey=${PROJECT_KEY_IN_SONAR_CLIENT} \
                    -Dsonar.sources=./ \
                    -Dsonar.host.url=${SONAR_SERVER_URL} \
                    -Dsonar.login=${SONAR_TOKEN_FRONT}"
                  ""
                }
              }
            }
          }
        }
      }
    }

    stage('Construction des images') {
      parallel {

        stage('Build backend image') {
          steps {
            dir('Backend') {
              sh 'docker build -t $DOCKER_USERNAME/backend-photograph:1.0 .'
            }
          }
        }

        stage('Build front-office image') {
          steps {
            dir('Frontend') {
              sh 'docker build -t $DOCKER_USERNAME/frontend-photograph:1.0 .'
            }
          }
        }
      }
    }

    stage('Push images to Docker Hub') {
      steps {
        sh 'docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD'
        sh 'docker push $DOCKER_USERNAME/backend-photograph:1.0'
        sh 'docker push $DOCKER_USERNAME/frontend-photograph:1.0'
      }
    }
    stage('Set Azure Subscription') {
      steps {
        withCredentials([string(credentialsId: 'azure_subscription', variable: 'AZURE_SUBSCRIPTION_ID')]) {
          script {
            sh "az cloud set --name AzureCloud"
            sh "az login"
            sh "az account set --subscription \${AZURE_SUBSCRIPTION_ID}"
          }
        }
      }
    }



  }
}