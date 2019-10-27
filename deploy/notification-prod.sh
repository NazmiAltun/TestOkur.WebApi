#!/bin/bash
docker pull nazmialtun/testokur-notification:latest
docker stop testokur-notification && docker rm testokur-notification --force
docker run --cap-add=SYS_PTRACE --security-opt seccomp=unconfined  -d \
	--env-file /home/env/notification-prod.env \
	--name testokur-notification \
	--restart=unless-stopped  \
	--network=testokur \
	--network-alias=notification \
	-m=256M \
	nazmialtun/testokur-notification:latest