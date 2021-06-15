#!/bin/bash

echo 'PUBLISH PRODUCTS.API...'
docker build -f src/Products.Api/Dockerfile -t tfsantosbr/products-api src/Products.Api
docker push tfsantosbr/products-api

echo 'PUBLISH IMPORTS.API...'
docker build -f src/ProductsImport.Api/Dockerfile -t tfsantosbr/imports-api src/ProductsImport.Api
docker push tfsantosbr/imports-api

echo 'PUBLISH IMPORTS.CONSUMER...'
docker build -f src/ProductsImport.Consumer/Dockerfile -t tfsantosbr/imports-consumer src/ProductsImport.Consumer
docker push tfsantosbr/imports-api