#!/bin/bash
docker stop testokur-report && docker rm testokur-report
docker run -d -e "ASPNETCORE_ENVIRONMENT=prod" \
	--env-file  /home/docker-images/report-prod.env \
	--name testokur-report \
	--restart=always  \
	--network=testokur \
	--network-alias=report \
	testokur-report:latest