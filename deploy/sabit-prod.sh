#!/bin/bash
docker pull nazmialtun/testokur-sabit:latest
docker stop testokur-sabit && docker rm testokur-sabit --force
docker run --cap-add=SYS_PTRACE --security-opt seccomp=unconfined  -d \
	--env-file /home/env/sabit-prod.env \
	-v /home/uploads:/app/wwwroot/uploads \
	--name testokur-sabit \
	--restart=always \
	--network=testokur \
	--network-alias=sabit \
	nazmialtun/testokur-sabit:latest