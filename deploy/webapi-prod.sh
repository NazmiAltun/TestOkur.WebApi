#!/bin/bash
docker pull nazmialtun/testokur-webapi:latest
docker stop testokur-webapi && docker rm testokur-webapi --force
docker run --cap-add=SYS_PTRACE --security-opt seccomp=unconfined  -d \
	--env-file /home/env/webapi-prod1.env \
	-v /home/uploads:/app/wwwroot/uploads \
	-v /home/DataProtection-Keys:/app/DataProtection-Keys \
	--name testokur-webapi \
	--restart=always \
	--network=testokur \
	--network-alias=webapi \
	nazmialtun/testokur-webapi:latest