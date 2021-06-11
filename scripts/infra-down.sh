#!/bin/sh

cd /d/Github/infra-kafka-cluster
docker-compose exec kafka kafka-topics --bootstrap-server kafka:9092 --delete --topic products-import-created
docker-compose exec kafka kafka-topics --bootstrap-server kafka:9092 --delete --topic process-product-requested
docker-compose exec kafka kafka-topics --bootstrap-server kafka:9092 --delete --topic products-import-completed

cd /d/Github/infra-kafka-cluster
docker-compose down -v

cd /d/Github/infra-postgres-database
docker-compose down

cd /d/Github/infra-elastic-stack
docker-compose down -v