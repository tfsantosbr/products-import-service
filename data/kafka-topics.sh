kafka-topics --create --replication-factor 3 --partitions 3 --topic products-import-created --bootstrap-server kafka1:19091 
kafka-topics --create --replication-factor 3 --partitions 3 --topic process-product-requested --bootstrap-server kafka1:19091 
kafka-topics --create --replication-factor 3 --partitions 3 --topic products-import-completed --bootstrap-server kafka1:19091 