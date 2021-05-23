-- Table: public.TaskSearchWordsEntity

-- DROP TABLE public."TaskSearchWordsEntity";

CREATE TABLE public."TaskSearchWordsEntity"
(
    "Id" uuid NOT NULL,
    "FindWord" text COLLATE pg_catalog."default",
    "TaskEntityId" uuid,
    CONSTRAINT "PK_TaskSearchWordsEntity" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_TaskSearchWordsEntity_TaskEntity_TaskEntityId" FOREIGN KEY ("TaskEntityId")
        REFERENCES public."TaskEntity" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE RESTRICT
)

TABLESPACE pg_default;

ALTER TABLE public."TaskSearchWordsEntity"
    OWNER to postgres;
-- Index: IX_TaskSearchWordsEntity_TaskEntityId

-- DROP INDEX public."IX_TaskSearchWordsEntity_TaskEntityId";

CREATE INDEX "IX_TaskSearchWordsEntity_TaskEntityId"
    ON public."TaskSearchWordsEntity" USING btree
    ("TaskEntityId" ASC NULLS LAST)
    TABLESPACE pg_default;