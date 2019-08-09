#!/bin/bash
docker load -i /home/docker-images/webapi.tar
docker stop testokur-webapi-qa && docker rm testokur-webapi-qa
docker run -d \
	--env-file  /home/docker-images/webapi-qa.env \
	-e VIRTUAL_HOST=webapi-qa.testokur.com \
	-e VIRTUAL_PORT=80 \
	-e LETSENCRYPT_HOST=webapi-qa.testokur.com \
	-e LETSENCRYPT_EMAIL=bilgi@testokur.com \
	-v /home/data-qa:/app/Data \
	-v /home/uploads-qa:/app/wwwroot/uploads \
	--name testokur-webapi-qa \
	--restart=always  \
	--network=testokur \
	--network-alias=webapi-qa \
	testokur-webapi:latest