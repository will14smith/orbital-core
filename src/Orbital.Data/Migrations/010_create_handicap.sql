CREATE TABLE handicap (
    "Id" SERIAL PRIMARY KEY,

    "PersonId" INT NOT NULL REFERENCES "person" ("Id"),
    "ScoreId" INT NULL REFERENCES "score" ("Id"),

	"Type" INT NOT NULL,
	"Date" TIMESTAMP NOT NULL,
	"Value" INT NOT NULL,

	"Indoor" BOOLEAN NOT NULL,
	"Bowstyle" INT NOT NULL
);
