#!/bin/bash
docker load -i /home/docker-images/webapi.tar
docker stop testokur-webapi-qa && docker rm testokur-webapi-qa
docker run -d \
	--env-file  /home/docker-images/webapi-qa.env \
	-v /home/data-qa:/app/Data \
	-v /home/uploads-qa:/app/wwwroot/uploads \
	--name testokur-webapi-qa \
	--restart=always  \
	--network=testokur \
	--network-alias=webapi-qa \
	testokur-webapi:latest