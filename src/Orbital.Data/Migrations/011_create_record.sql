CREATE TABLE record (
    "Id" SERIAL PRIMARY KEY,

	"TeamSize" INT NOT NULL
);

CREATE TABLE record_club (
    "Id" SERIAL PRIMARY KEY,

    "RecordId" INT NOT NULL REFERENCES "record" ("Id"),
    "ClubId" INT NOT NULL REFERENCES "club" ("Id"),

	"ActiveFrom" TIMESTAMP NOT NULL,
	"ActiveTo" TIMESTAMP NOT NULL
);

CREATE TABLE record_round (
    "Id" SERIAL PRIMARY KEY,

    "RecordId" INT NOT NULL REFERENCES "record" ("Id"),
    "RoundId" INT NOT NULL REFERENCES "round" ("Id"),

	"Count" INT NOT NULL,
	"Skill" INT NULL,
	"Bowstyle" INT NULL,
	"Gender" INT NULL
);
