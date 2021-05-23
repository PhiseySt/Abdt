-- Table: public.TaskEntity

-- DROP TABLE public."TaskEntity";

CREATE TABLE public."TaskEntity"
(
    "Id" uuid NOT NULL,
    "TaskInterval" integer NOT NULL,
    "TaskStartTime" timestamp without time zone NOT NULL,
    "TaskEndTime" timestamp without time zone NOT NULL,
    "CreatedDate" timestamp without time zone NOT NULL,
    "LastSavedDate" timestamp without time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "LastSavedBy" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    CONSTRAINT "PK_TaskEntity" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE public."TaskEntity"
    OWNER to postgres;