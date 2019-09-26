#!/bin/bash
docker pull nazmialtun/testokur-notification:latest
docker stop testokur-notification && docker rm testokur-notification --force
docker run -d \
	--env-file /home/env/notification-prod.env \
	--name testokur-notification \
	--restart=unless-stopped  \
	--network=testokur \
	--network-alias=notification \
	-m=350M \
	nazmialtun/testokur-notification:latest