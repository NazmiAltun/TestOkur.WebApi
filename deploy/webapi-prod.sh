#!/bin/bash
docker stop testokur-webapi && docker rm testokur-webapi
docker run -d \
	--env-file /home/docker-images/webapi-prod.env \
	--name testokur-webapi \
	--restart=unless-stopped \
	--network=testokur \
	--network-alias=webapi \
	testokur-webapi:latest