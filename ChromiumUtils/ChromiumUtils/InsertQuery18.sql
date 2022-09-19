﻿INSERT INTO "cookies" (
	"creation_utc",
	"host_key",
	"top_frame_site_key",
	"name",
	"value",
	"encrypted_value",
	"path",
	"expires_utc",
	"is_secure",
	"is_httponly",
	"last_access_utc",
	"has_expires",
	"is_persistent",
	"priority",
	"samesite",
	"source_scheme",
	"source_port",
	"is_same_party",
	"last_update_utc"
)
VALUES (
	@creationUtc,
	@hostKey,
	"",
	@name,
	@value,
	"",
	@path,
	@expiresUtc,
	@isSecure,
	0,
	@creationUtc,
	1,
	1,
	1,
	-1,
	2,
	@sourcePort,
	0,
	@creationUtc
);