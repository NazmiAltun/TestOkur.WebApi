#!/bin/bash
docker load -i /home/images/report.tar
docker stop testokur-report && docker rm testokur-report --force
docker run -d \
	--env-file  /home/env/report-prod.env \
	--name testokur-report \
	--restart=unless-stopped  \
	--network=testokur \
	--network-alias=report \
	testokur-report:latest