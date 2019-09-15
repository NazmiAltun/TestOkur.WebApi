#!/bin/bash
docker load -i /home/images/report.tar
docker stop testokur-report-qa && docker rm testokur-report-qa --force
docker run -d \
	--env-file  /home/env/report-qa.env \
	--name testokur-report-qa \
	--restart=always  \
	--network=testokur \
	--network-alias=report-qa \
	-m=200M \
	testokur-report:latest