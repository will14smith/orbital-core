# orbital-core

Orbital archery score management system

## Setup

Docker: 
```
docker run --name orbital-postgres -e POSTGRES_USER=orbital -e POSTGRES_PASSWORD=orbital -e POSTGRES_DB=orbital -d -p 5432:5432 postgres
```

Install dependencies:
```
make setup
```

## Build

```
make build
```

## Run (with watch)

```
make start
```


## Migrations

Add a migration:

```
make migrations-add NAME="migration name"
```

Run migrations:

```
make migrations-up
```