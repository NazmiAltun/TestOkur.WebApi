#!/bin/bash
docker stop testokur-notification && docker rm testokur-notification
docker run -d -e "ASPNETCORE_ENVIRONMENT=prod" \
	--env-file  /home/docker-images/notification-prod.env \
	--name testokur-notification \
	--restart=always  \
	--network=testokur \
	--network-alias=notification \
	testokur-notification:latest