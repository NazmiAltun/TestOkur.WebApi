#!/bin/bash
docker pull nazmialtun/testokur-report:latest
docker stop testokur-report && docker rm testokur-report --force
docker run --cap-add=SYS_PTRACE --security-opt seccomp=unconfined  -d \
	--env-file  /home/env/report-prod.env \
	--name testokur-report \
	--restart=unless-stopped  \
	--network=testokur \
	--network-alias=report \
	-m=256M \
	nazmialtun/testokur-report:latest