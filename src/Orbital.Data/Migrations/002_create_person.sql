CREATE TABLE person (
    "Id" SERIAL PRIMARY KEY,

    "ClubId" INT REFERENCES club ("Id"),

    "Name" TEXT NOT NULL,

    "Gender" INT NOT NULL,
    "Bowstyle" INT NULL,
	"ArcheryGBNumber" TEXT NULL UNIQUE,
    "DateOfBirth" DATE NULL,
    "DateStartedArchery" DATE NULL
)