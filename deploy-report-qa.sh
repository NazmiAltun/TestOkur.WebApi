#!/bin/bash
docker load -i /home/docker-images/report.tar
docker stop testokur-report-qa && docker rm testokur-report-qa
docker run -d \
	--env-file  /home/docker-images/report-qa.env \
	-e VIRTUAL_HOST=report-qa.testokur.com \
	-e VIRTUAL_PORT=80 \
	-e LETSENCRYPT_HOST=report-qa.testokur.com \
	-e LETSENCRYPT_EMAIL=bilgi@testokur.com \
	--name testokur-report-qa \
	--restart=always  \
	--network=testokur \
	--network-alias=report-qa \
	testokur-report:latest