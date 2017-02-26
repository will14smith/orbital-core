CREATE TABLE badge (
    "Id" SERIAL PRIMARY KEY,
	
	"Name" TEXT NOT NULL,
	"Description" TEXT NULL,
	"Algorithm" TEXT NULL,
	"Category" TEXT NULL,
	"Multiple" BOOLEAN NOT NULL,
	"ImageUrl" TEXT NULL
);