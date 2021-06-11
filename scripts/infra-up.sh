#!/bin/bash

cd /d/Github/infra-kafka-cluster
docker-compose up -d

cd /d/Github/infra-postgres-database
docker-compose up -d

cd /d/Github/infra-elastic-stack
bash run.sh

cd /d/Github/infra-kafka-cluster
docker-compose exec kafka kafka-topics --create --replication-factor 1 --partitions 1 --topic products-import-created --bootstrap-server kafka:9092
docker-compose exec kafka kafka-topics --create --replication-factor 1 --partitions 1 --topic process-product-requested --bootstrap-server kafka:9092
docker-compose exec kafka kafka-topics --create --replication-factor 1 --partitions 1 --topic products-import-completed --bootstrap-server kafka:9092
