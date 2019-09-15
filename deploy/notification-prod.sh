#!/bin/bash
docker load -i /home/images/notification.tar
docker stop testokur-notification && docker rm testokur-notification --force
docker run -d \
	--env-file /home/env/notification-prod.env \
	--name testokur-notification \
	--restart=unless-stopped  \
	--network=testokur \
	--network-alias=notification \
	testokur-notification:latest