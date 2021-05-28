-- Table: public.TextTaskEntity

-- DROP TABLE public."TextTaskEntity";

CREATE TABLE public."TextTaskEntity"
(
    "Id" uuid NOT NULL,
    "TaskId" uuid NOT NULL,
    "TextId" uuid NOT NULL,
    "FindindWordsCount" integer NOT NULL,
    "CreatedDate" timestamp without time zone NOT NULL,
    "LastSavedDate" timestamp without time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "LastSavedBy" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    CONSTRAINT "PK_TextTaskEntity" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE public."TextTaskEntity"
    OWNER to postgres;