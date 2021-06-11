-- public.products definition

CREATE TABLE products (
	id uuid NOT NULL,
	"store-id" numeric NOT NULL,
	code varchar(20) NOT NULL,
	name varchar(500) NOT NULL,
	price money NOT NULL,
	stock numeric NOT NULL,
	"processed-at" timestamp(0) NULL,
	CONSTRAINT products_pk PRIMARY KEY (id)
);
CREATE INDEX products_store_id_idx ON public.products USING btree ("store-id");