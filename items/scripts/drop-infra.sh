#!/bin/sh

cd /d/Github/infra-kafka-cluster
docker-compose exec kafka-1 kafka-topics --bootstrap-server kafka1:19091 --delete --topic products-import-created
docker-compose exec kafka-1 kafka-topics --bootstrap-server kafka1:19091 --delete --topic process-product-requested
docker-compose exec kafka-1 kafka-topics --bootstrap-server kafka1:19091 --delete --topic products-import-completed

cd /d/Github/infra-kafka-cluster
docker-compose down -v

cd /d/Github/infra-postgres-database
docker-compose down -v

cd /d/Github/infra-elastic-stack
docker-compose down -v