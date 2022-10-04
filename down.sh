#!/bin/sh

echo "Stopping EnsimagCafet apps..."
docker compose -f docker-compose.generated.yml down
echo "Stopping reverse proxy..."
cd ReverseProxy
docker compose -f docker-compose.generated.yml down
echo "Finished!"
