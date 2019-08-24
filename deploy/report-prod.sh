#!/bin/bash
docker stop testokur-report && docker rm testokur-report
docker run -d \
	--env-file  /home/docker-images/report-prod.env \
	--name testokur-report \
	--restart=unless-stopped  \
	--network=testokur \
	--network-alias=report \
	testokur-report:latest