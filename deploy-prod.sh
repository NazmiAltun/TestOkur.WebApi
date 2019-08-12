#!/bin/bash
docker stop testokur-webapi && docker rm testokur-webapi
docker run -d -e "ASPNETCORE_ENVIRONMENT=prod" \
	--env-file  /home/docker-images/webapi-prod.env \
	--name testokur-webapi \
	--restart=always  \
	--network=testokur \
	--network-alias=webapi \
	testokur-webapi:latest

docker stop testokur-notification && docker rm testokur-notification
docker run -d -e "ASPNETCORE_ENVIRONMENT=prod" \
	--env-file  /home/docker-images/notification-prod.env \
	--name testokur-notification \
	--restart=always  \
	--network=testokur \
	--network-alias=notification \
	testokur-notification:latest

docker stop testokur-report && docker rm testokur-report
docker run -d -e "ASPNETCORE_ENVIRONMENT=prod" \
	--env-file  /home/docker-images/report-prod.env \
	--name testokur-report \
	--restart=always  \
	--network=testokur \
	--network-alias=report \
	testokur-report:latest