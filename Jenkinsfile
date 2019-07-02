pipeline {
	agent { label 'docker-slave' }	
	options {
		disableConcurrentBuilds()
		timeout(time: 120, unit: 'MINUTES')
		buildDiscarder(logRotator(numToKeepStr: '20'))
	}
	parameters {
        choice(
            choices: ['QA' , 'PROD'],
            description: 'Environment?',
            name: 'Environment')
    }
	stages {
		stage('Clean') {
		  steps {
			cleanWs()
			sh "echo y | docker volume prune"
			checkout scm
			sh "docker-compose -f docker-compose.tests.yml down"
		  }
		}
		stage('Run Tests'){
			when {
                expression { params.Environment != 'PROD' }
            }
			steps{
				sh "docker-compose -f docker-compose.tests.yml up --build --abort-on-container-exit domain-unit-test"
				sh "docker-compose -f docker-compose.tests.yml up --build --abort-on-container-exit notification-unit-test"
				sh "docker-compose -f docker-compose.tests.yml up --build --abort-on-container-exit webapi-unit-test"
				sh "docker-compose -f docker-compose.tests.yml up --build --abort-on-container-exit notification-integration-test"
				sh "docker-compose -f docker-compose.tests.yml up --build --abort-on-container-exit webapi-integration-test"
				sh "docker-compose -f docker-compose.tests.yml up --build --abort-on-container-exit report-integration-test"
				sh "docker-compose -f docker-compose.tests.yml down"
			}
		}
		stage('Build') {
			when {
                expression { params.Environment != 'PROD' }
            }
			steps{
				sh "docker-compose build webapi notification report"
				sh "docker save -o webapi.tar testokur-webapi:latest"
				sh "docker save -o report.tar testokur-report:latest"
				sh "docker save -o notification.tar testokur-notification:latest"
			}
		}
		stage('Deploy Web-Api QA') {
			when {
                expression { params.Environment != 'PROD' }
            }
			steps{
				echo "Deploying to qa environment..."
				echo "Clearing the cache...."
				sh "curl -X 'DELETE' https://webapi-qa.testokur.com/api/cache"
				sh "scp webapi.tar root@185.141.33.33:/home/docker-images/"
				sh "ssh root@185.141.33.33 'bash -s' < deploy-webapi-qa.sh"
			}
		}
		stage('Deploy Report-Api QA') {
			when {
                expression { params.Environment != 'PROD' }
            }
			steps{
				sh "scp report.tar root@185.141.33.33:/home/docker-images/"
				sh "ssh root@185.141.33.33 'bash -s' < deploy-report-qa.sh"
			}
		}
		stage('Deploy Notification QA') {
			when {
                expression { params.Environment != 'PROD' }
            }
			steps{
				sh "scp notification.tar root@185.141.33.33:/home/docker-images/"
				sh "ssh root@185.141.33.33 'bash -s' < deploy-notification-qa.sh"
			}
		}
		stage('Deploy to PROD') {
			when {
                expression { params.Environment == 'PROD' }
            }
            steps {
                echo "Deploying to prod environment..."
				echo "Cleaning the cache...."
				sh "curl -X 'DELETE' https://webapi.testokur.com/api/cache"
				sh "ssh root@185.141.33.33 'bash -s' < deploy-prod.sh"
            }
		}
	}
	post {
        always {
            echo 'CI job execution finished'
        }
        success {
            echo 'CI job is successful'
        }
        failure {
            echo 'CI job has failed.'
		    emailext body: 'testokur-webapi CI job failed',
                     subject: "testokur-webapi CI job failed",
                     to: "nazmialtun@windowslive.com"
        }
        changed {
            echo 'CI job is changed.'
		    emailext body: 'testokur-webapi CI job changed',
                     subject: "testokur-webapi CI job changed",
                     to: "nazmialtun@windowslive.com"
        }
    }
}