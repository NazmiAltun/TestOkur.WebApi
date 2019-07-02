#!/bin/bash
docker load -i /home/docker-images/notification.tar
docker stop testokur-notification-qa && docker rm testokur-notification-qa
docker run -d \
	--env-file  /home/docker-images/qa.env \
	--name testokur-notification-qa \
	--restart=always  \
	--network=testokur \
	--network-alias=notification-qa \
	testokur-notification:latest