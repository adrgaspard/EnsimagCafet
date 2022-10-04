#!/bin/sh

echo "Generating docker-compose.override files..." \
&& cd Compose \
&& python3 generate-overrides.py \
&& echo "Starting reverse proxy..." \
&& cd ReverseProxy \
&& docker compose -f docker-compose.generated.yml -p reverseproxy up --build -d \
&& echo "Starting EnsimagCafet apps..." \
&& cd ../EnsimagCafet \
&& docker compose -f docker-compose.generated.yml -p ensimagcafet up --build -d \
&& echo "Finished!" \
&& cd ..
