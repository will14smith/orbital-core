docker run --name orbital-postgres -e POSTGRES_USER=orbital -e POSTGRES_PASSWORD=orbital -e POSTGRES_DB=orbital -d postgres

## Libraries "forked" for .NET standard compatibility
* [DbUp.Postgres](https://github.com/DbUp/DbUp/tree/57f648c216c9bd3089ecaf22d8e99df4f3dcc243/src/dbup-postgresql)
* [Respawn](https://github.com/jbogard/Respawn/commit/d98138d169002c1df105d7e436899364e2d62519/Respawn)
* [Dapper.Contrib](https://github.com/StackExchange/Dapper/tree/f0bbbab8e57701dd6562f5cfcdcc9d1f296f99e3/Dapper.Contrib) - needed some postgres fixes