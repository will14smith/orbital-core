CREATE TABLE score (
    "Id" SERIAL PRIMARY KEY,

    "PersonId" INT NOT NULL REFERENCES "person" ("Id"),
    "ClubId" INT NOT NULL REFERENCES "club"("Id"),
    "RoundId" INT NOT NULL REFERENCES "round" ("Id"),
    "CompetitionId" INT NULL REFERENCES "competition" ("Id"),

	"Bowstyle" INT NOT NULL,

	"TotalScore" DOUBLE PRECISION NOT NULL,
	"TotalGolds" DOUBLE PRECISION NOT NULL,
	"TotalHits" DOUBLE PRECISION NOT NULL,

    "ShotAt" TIMESTAMP NOT NULL,
    "EnteredAt" TIMESTAMP NOT NULL
);

CREATE TABLE score_target (
    "Id" SERIAL PRIMARY KEY,

    "ScoreId" INT NOT NULL REFERENCES "score" ("Id"),

	"Score" DOUBLE PRECISION NOT NULL,
	"Golds" DOUBLE PRECISION NOT NULL,
	"Hits" DOUBLE PRECISION NOT NULL
);

