CREATE TABLE badge_holder (
    "Id" SERIAL PRIMARY KEY,

    "BadgeId" INT NOT NULL REFERENCES badge ("Id"),
    "PersonId" INT NOT NULL REFERENCES person ("Id"),

    "AwardedOn" TIMESTAMP NOT NULL,
    "ConfirmedOn" TIMESTAMP NULL,
    "MadeOn" TIMESTAMP NULL,
    "DeliveredOn" TIMESTAMP NULL
)
