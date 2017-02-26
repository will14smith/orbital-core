CREATE TABLE round (
    "Id" SERIAL PRIMARY KEY,

    "VariantOfId" INT NULL REFERENCES round ("Id"),

    "Category" TEXT NOT NULL,
    "Name" TEXT NOT NULL UNIQUE,
    "Indoor" BOOLEAN NOT NULL
);

CREATE TABLE round_target (
    "Id" SERIAL PRIMARY KEY,

    "RoundId" INT NOT NULL REFERENCES round ("Id"),

    "ScoringType" INT NOT NULL,

    "DistanceValue" DOUBLE PRECISION NOT NULL,
    "DistanceUnit" INT NOT NULL,
    "FaceSizeValue" DOUBLE PRECISION NOT NULL,
    "FaceSizeUnit" INT NOT NULL,

    "ArrowCount" INT NOT NULL
)
