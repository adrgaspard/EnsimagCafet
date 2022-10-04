#!/bin/sh

cd Compose \
&& echo "Stopping EnsimagCafet apps..." \
&& cd EnsimagCafet \
&& docker compose -f docker-compose.generated.yml down \
&& echo "Stopping reverse proxy..." \
&& cd ../ReverseProxy \
&& docker compose -f docker-compose.generated.yml down \
&& echo "Deleting generated files..." \
&& cd .. \
&& rm ./*/docker-compose.generated.yml \
&& echo "Finished!"
