#!/bin/bash
docker pull nazmialtun/testokur-webapi:latest
docker stop testokur-webapi && docker rm testokur-webapi --force
docker run -d \
	--env-file /home/env/webapi-prod.env \
	-v /home/data:/app/Data \
	-v /home/uploads:/app/wwwroot/uploads \
	--name testokur-webapi \
	--restart=unless-stopped \
	--network=testokur \
	--network-alias=webapi \
	-m=350M \
	nazmialtun/testokur-webapi:latest