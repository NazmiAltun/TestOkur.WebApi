#!/bin/bash
docker pull nazmialtun/testokur-webapi:latest
docker stop testokur-webapi && docker rm testokur-webapi --force
docker run -d \
	--env-file /home/env/webapi-prod1.env \
	-v /home/uploads:/app/wwwroot/uploads \
	-v /home/DataProtection-Keys:/app/DataProtection-Keys \
	--name testokur-webapi \
	--restart=unless-stopped \
	--network=testokur \
	--network-alias=webapi \
	-m=256M \
	nazmialtun/testokur-webapi:latest
docker stop testokur-webapi_2 && docker rm testokur-webapi_2 --force
docker run -d \
	--env-file /home/env/webapi-prod2.env \
	-v /home/uploads:/app/wwwroot/uploads \
	-v /home/DataProtection-Keys:/app/DataProtection-Keys \
	--name testokur-webapi_2 \
	--restart=unless-stopped \
	--network=testokur \
	--network-alias=webapi_2 \
	-m=256M \
	nazmialtun/testokur-webapi:latest