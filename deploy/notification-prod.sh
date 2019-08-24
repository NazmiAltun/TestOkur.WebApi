#!/bin/bash
docker stop testokur-notification && docker rm testokur-notification
docker run -d \
	--env-file  /home/docker-images/notification-prod.env \
	--name testokur-notification \
	--restart=unless-stopped  \
	--network=testokur \
	--network-alias=notification \
	testokur-notification:latest