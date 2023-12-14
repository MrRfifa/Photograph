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
    stage('Build') {
            steps {
                dir("Frontend"){
                  script {
                    // Install dependencies and build the React app
                    sh 'npm install'
                    sh 'npm run build'
                }
                }
            }
        }


    stage('Analyses SonarQube') {
      parallel {
        // stage('SonarQube analysis - Back') {
        //   steps {
        //     dir('Backend') {
        //       script {
        //         def scannerHome = tool 'sonar'
        //         def scannerHomeBuild = tool 'sonarBuild'
        //         withSonarQubeEnv('SonarQube') {
        //           sh "./var/jenkins_home/tools/hudson.plugins.sonar.MsBuildSQRunnerInstallation/sonarBuild/SonarScanner.MSBuild.exe begin /k:${SONAR_PROJECT_KEY} /d:sonar.host.url=${SONAR_SERVER_URL} /d:sonar.login=${SONAR_TOKEN_BACK}"
        //           sh 'dotnet build'
        //           sh "./var/jenkins_home/tools/hudson.plugins.sonar.MsBuildSQRunnerInstallation/sonarBuild/SonarScanner.MSBuild.exe end /d:sonar.login=${SONAR_TOKEN_BACK}"
        //         }
        //       }
        //     }
        //   }
        // }

        stage('SonarQube analysis - Front') {
          steps {
            dir('Frontend') {
              script {
                def scannerHome = tool 'sonarqube'
                
                withSonarQubeEnv('sonarqube') {
                  sh ""
                  "${scannerHome}/bin/sonar-scanner \
                    -Dsonar.projectKey=${PROJECT_KEY_IN_SONAR_CLIENT} \
                    -Dsonar.sources=./src \
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
        withCredentials([string(credentialsId: 'docker_credentials', variable: 'DOCKER_PASSWORD')]) {
          
        sh 'docker login -u $DOCKER_USERNAME -p \$DOCKER_PASSWORD'
        sh 'docker push $DOCKER_USERNAME/backend-photograph:1.0'
        sh 'docker push $DOCKER_USERNAME/frontend-photograph:1.0'
        }
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

    stage('Provision server') {
            // environment {
            //     TF_VAR_env_prefix = "test"
            //     }
                steps {
                    script {
                        dir("Terraform") {
                            // Initialize and apply Terraform configuration
                            sh "terraform init"
                            sh "terraform apply --auto-approve"
                            // Capture the VM's public IP
                            VM_PUBLIC_IP = sh(
                                script: "terraform output vm_ip",
                                returnStdout: true
                            ).trim()
                        }
                    }
                }
        }


  
  stage('Deploy to Azure VM') {
    steps {
        script {
            // Define Azure VM SSH credentials from Jenkins Credential store
            def azureVmCredentials = credentials('server-ssh-key')
            echo "VM_PUBLIC_IP: ${VM_PUBLIC_IP}"
            if (azureVmCredentials) {
                echo "Azure VM SSH credentials found"
                // Azure VM SSH connection details
                def azureVmHostname = "${VM_PUBLIC_IP}"
                def azureVmPort = 22 // Default SSH port
                def azureVmUsername = 'azureuser'
                def virtual_machine = "${azureVmUsername}@${azureVmHostname}"
                def shellCmd = "bash ./server-cmds.sh"
                // Define the application deployment commands
                def deployCommands = """
                    scp -o StrictHostKeyChecking=no server-cmds.sh docker-compose.yaml ${virtual_machine}:/home/azureuser/
                    ssh -o StrictHostKeyChecking=no -p 22 ${azureVmUsername}@${azureVmHostname} ${shellCmd}
                """
                // Execute SSH commands to deploy the application
                sshagent( ['server-ssh-key']) {
                    sh """
                        ${deployCommands}
                    """
                }
            } else {
                error("Azure VM SSH credentials not found")
            }
        }
    }
}

  }
}