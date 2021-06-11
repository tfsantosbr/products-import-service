-- public.imports definition

CREATE TABLE "Imports" (
	"Id" uuid NOT NULL,
	"CreatedAt" timestamp(0) NOT NULL,
	"CompletedAt" timestamp(0) NULL,
	"SpreadsheetFileUrl" varchar(300) NOT NULL,
	CONSTRAINT imports_pk PRIMARY KEY ("Id")
);

-- public."imports-products" definition

CREATE TABLE "ImportProducts" (
	"ImportId" uuid NOT NULL,
	"ProductCode" varchar(20) NOT NULL,
	"IsProcessed" bool NOT NULL,
	"Observation" varchar(500) NULL,
	"ProcessedAt" information_schema."time_stamp" NULL,
	CONSTRAINT imports_products_pk PRIMARY KEY ("ImportId", "ProductCode")
);

-- public."imports-products" foreign keys

ALTER TABLE public."ImportProducts" ADD CONSTRAINT imports_products_fk FOREIGN KEY ("ImportId") REFERENCES "Imports"("Id");
