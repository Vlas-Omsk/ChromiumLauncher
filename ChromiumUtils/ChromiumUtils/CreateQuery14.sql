CREATE TABLE "cookies" (
	"creation_utc"	INTEGER NOT NULL,
	"host_key"	TEXT NOT NULL,
	"name"	TEXT NOT NULL,
	"value"	TEXT NOT NULL,
	"path"	TEXT NOT NULL,
	"expires_utc"	INTEGER NOT NULL,
	"is_secure"	INTEGER NOT NULL,
	"is_httponly"	INTEGER NOT NULL,
	"last_access_utc"	INTEGER NOT NULL,
	"has_expires"	INTEGER NOT NULL DEFAULT 1,
	"is_persistent"	INTEGER NOT NULL DEFAULT 1,
	"priority"	INTEGER NOT NULL DEFAULT 1,
	"encrypted_value"	BLOB DEFAULT '',
	"samesite"	INTEGER NOT NULL DEFAULT -1,
	"source_scheme"	INTEGER NOT NULL DEFAULT 0,
	"source_port"	INTEGER NOT NULL DEFAULT -1,
	"is_same_party"	INTEGER NOT NULL DEFAULT 0,
	UNIQUE("host_key","name","path")
);

CREATE TABLE "meta" (
	"key"	LONGVARCHAR NOT NULL UNIQUE,
	"value"	LONGVARCHAR,
	PRIMARY KEY("key")
);

INSERT INTO "main"."meta" ("key", "value") VALUES ('mmap_status', '-1');
INSERT INTO "main"."meta" ("key", "value") VALUES ('version', '14');
INSERT INTO "main"."meta" ("key", "value") VALUES ('last_compatible_version', '14');