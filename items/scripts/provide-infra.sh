#!/bin/sh

cd /d/Github/infra-kafka-cluster
docker-compose up -d

cd /d/Github/infra-postgres-database
docker-compose up -d

cd /d/Github/infra-elastic-stack
docker-compose up -d

cd /d/Github/infra-kafka-cluster
docker-compose exec kafka-1 kafka-topics --create --replication-factor 3 --partitions 3 --topic products-import-created --bootstrap-server kafka1:19091
docker-compose exec kafka-1 kafka-topics --create --replication-factor 3 --partitions 3 --topic process-product-requested --bootstrap-server kafka1:19091
docker-compose exec kafka-1 kafka-topics --create --replication-factor 3 --partitions 3 --topic products-import-completed --bootstrap-server kafka1:19091
