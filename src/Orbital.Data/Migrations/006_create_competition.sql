CREATE TABLE competition (
    "Id" SERIAL PRIMARY KEY,

    "Name" TEXT,

	"Start" TIMESTAMP,
	"End" TIMESTAMP
);

CREATE TABLE competition_round (
	"CompetitionId" INT REFERENCES competition ("Id"),
	"RoundId" INT REFERENCES round ("Id"),

	CONSTRAINT competition_round_pkey PRIMARY KEY ("CompetitionId", "RoundId")
)