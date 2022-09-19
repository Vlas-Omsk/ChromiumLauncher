CREATE TABLE "cookies" (
	"creation_utc"	INTEGER NOT NULL,
	"host_key"	TEXT NOT NULL,
	"top_frame_site_key"	TEXT NOT NULL,
	"name"	TEXT NOT NULL,
	"value"	TEXT NOT NULL,
	"encrypted_value"	BLOB NOT NULL,
	"path"	TEXT NOT NULL,
	"expires_utc"	INTEGER NOT NULL,
	"is_secure"	INTEGER NOT NULL,
	"is_httponly"	INTEGER NOT NULL,
	"last_access_utc"	INTEGER NOT NULL,
	"has_expires"	INTEGER NOT NULL,
	"is_persistent"	INTEGER NOT NULL,
	"priority"	INTEGER NOT NULL,
	"samesite"	INTEGER NOT NULL,
	"source_scheme"	INTEGER NOT NULL,
	"source_port"	INTEGER NOT NULL,
	"is_same_party"	INTEGER NOT NULL,
	"last_update_utc"	INTEGER NOT NULL
);

CREATE TABLE "meta" (
	"key"	LONGVARCHAR NOT NULL UNIQUE,
	"value"	LONGVARCHAR,
	PRIMARY KEY("key")
);

CREATE UNIQUE INDEX "cookies_unique_index" ON "cookies" (
	"host_key",
	"top_frame_site_key",
	"name",
	"path"
);

INSERT INTO "main"."meta" ("key", "value") VALUES ('mmap_status', '-1');
INSERT INTO "main"."meta" ("key", "value") VALUES ('version', '18');
INSERT INTO "main"."meta" ("key", "value") VALUES ('last_compatible_version', '18');