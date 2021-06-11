-- public.imports definition

CREATE TABLE imports (
	id uuid NOT NULL,
	"created-at" timestamp(0) NOT NULL,
	"completed-at" timestamp(0) NULL,
	"spreadsheet-file-url" varchar(300) NOT NULL,
	CONSTRAINT imports_pk PRIMARY KEY (id)
);
-- public."imports-products" definition

CREATE TABLE "imports-products" (
	"import-id" uuid NOT NULL,
	"product-code" varchar(20) NOT NULL,
	"is-processed" bool NOT NULL,
	observation varchar(500) NULL,
	"processed-at" information_schema."time_stamp" NULL,
	CONSTRAINT imports_products_pk PRIMARY KEY ("import-id", "product-code")
);

-- public."imports-products" foreign keys

ALTER TABLE public."imports-products" ADD CONSTRAINT imports_products_fk FOREIGN KEY ("import-id") REFERENCES imports(id);
