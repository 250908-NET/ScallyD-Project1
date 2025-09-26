# Go Tournament Planner

The Go Tournament Planner helps to organize go tournaments by storing info about
tournaments and their participants.

## Endpoints

- `/tournaments`
  - GET: List all tournaments
  - POST: Create a new tournament
- ~~`/tournaments?startDate=YYYY-MM-DD&endDate=YYYY-MM-DD`~~
  - ~~GET: List all tournaments in given date range~~
- `/tournaments/:id`
  - GET: Get info (name, location, dates, ruleset, organizer) on a given
    tournament
  - ~~PATCH: Change a tournament's name, location, start or end dates, ruleset,
    or organizer~~
  - DELETE: Remove a tournament
- `/tournaments/:id/participants`
  - GET: List all participants in a given tournament
  - POST: Add a participant to a tournament
- `/tournaments/:id/participants/:id`
  - DELETE: Remove a participant from a tournament
- `/players`
  - GET: List all players
  - POST: Create a new player/organizer
- `/players/:id`
  - GET: Get info (name, email, rank) on a given player
  - ~~PATCH: Change a player's name, email, or rank~~
  - DELETE: Remove a player
- `/players/:id/tournaments`
  - GET: List all tournaments a given player has participated in (or will
    participate in)
