# products-import-service
Serviço de importação de produtos


## Tutoriais

- https://medium.com/azure-na-pratica/apache-kafka-kafdrop-docker-compose-montando-rapidamente-um-ambiente-para-testes-606cc76aa66

## Provide Infra

```bash
bash items/scripts/provide-infra.sh
```

## Docker

```bash
# Products API
docker run -it -p 6000:80 -e CONNECTIONSTRINGS__DEFAULT='Server=host.docker.internal;Port=5432;Database=Products;User Id=postgres;Password=postgres;' tfsantosbr/products-api

# Imports API
docker run -it -p 5000:80 -e CONNECTIONSTRINGS__DEFAULT='Server=host.docker.internal;Port=5432;Database=Imports;User Id=postgres;Password=postgres;' tfsantosbr/imports-api

# Imports Consumer
docker run -it \
-e CONNECTIONSTRINGS__DEFAULT='Server=host.docker.internal;Port=5432;Database=Imports;User Id=postgres;Password=postgres;' \
-e KAFKA__BOOTSTRAPSERVERS='host.docker.internal:9092' \
tfsantosbr/imports-consumer
```
