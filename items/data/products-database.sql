-- public.products definition

CREATE TABLE "Products" (
	"Id"  uuid NOT NULL,
	"StoreId" numeric NOT NULL,
	"Code" varchar(20) NOT NULL,
	"Name" varchar(500) NOT NULL,
	"Price" money NOT NULL,
	"Stock" numeric NOT NULL,
	"ProcessedAt" timestamp(0) NULL,
	CONSTRAINT products_pk PRIMARY KEY ("Id")
);

CREATE INDEX products_store_id_idx ON public."Products" USING btree ("StoreId");