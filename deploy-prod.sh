#!/bin/bash
docker stop testokur-webapi && docker rm testokur-webapi
docker run -d -e "ASPNETCORE_ENVIRONMENT=prod" \
	-e VIRTUAL_HOST=webapi.testokur.com \
	-e VIRTUAL_PORT=80 \
	-e LETSENCRYPT_HOST=webapi.testokur.com \
	-e LETSENCRYPT_EMAIL=bilgi@testokur.com \
	--name testokur-webapi \
	--restart=always  \
	--network=testokur \
	--network-alias=webapi \
	testokur-webapi:latest

docker stop testokur-notification && docker rm testokur-notification
docker run -d -e "ASPNETCORE_ENVIRONMENT=prod" \
	--name testokur-notification \
	--restart=always  \
	--network=testokur \
	--network-alias=notification \
	testokur-notification:latest

docker stop testokur-report && docker rm testokur-report
docker run -d -e "ASPNETCORE_ENVIRONMENT=prod" \
	-e VIRTUAL_HOST=report.testokur.com \
	-e VIRTUAL_PORT=80 \
	-e LETSENCRYPT_HOST=report.testokur.com \
	-e LETSENCRYPT_EMAIL=bilgi@testokur.com \
	--name testokur-report \
	--restart=always  \
	--network=testokur \
	--network-alias=report \
	testokur-report:latest