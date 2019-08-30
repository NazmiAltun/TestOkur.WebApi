#!/bin/bash
docker load -i /home/docker-images/report.tar
docker stop testokur-report-qa && docker rm testokur-report-qa --force
docker run -d \
	--env-file  /home/docker-images/report-qa.env \
	--name testokur-report-qa \
	--restart=always  \
	--network=testokur \
	--network-alias=report-qa \
	testokur-report:latest