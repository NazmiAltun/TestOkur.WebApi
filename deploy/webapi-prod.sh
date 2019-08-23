#!/bin/bash
docker stop testokur-webapi && docker rm testokur-webapi
docker run -d -e "ASPNETCORE_ENVIRONMENT=prod" \
	--env-file  /home/docker-images/webapi-prod.env \
	--name testokur-webapi \
	--restart=always  \
	--network=testokur \
	--network-alias=webapi \
	testokur-webapi:latest