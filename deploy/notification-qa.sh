#!/bin/bash
docker pull nazmialtun/testokur-notification:latest
docker stop testokur-notification-qa && docker rm testokur-notification-qa --force
docker run -d \
	--env-file  /home/env/notification-qa.env \
	--name testokur-notification-qa \
	--restart=always  \
	--network=testokur \
	--network-alias=notification-qa \
	-m=200M \
	nazmialtun/testokur-notification:latest