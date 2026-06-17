# Ticket / Issue Management System (API)

A backend service for tracking issues through their lifecycle — like a lightweight Jira.
Projects contain tickets; tickets move through a validated **status workflow**, carry
**priority/type**, support **comments**, and are listable with **filtering and pagination**.

Built with **ASP.NET Core 8 (Web API)**, **Entity Framework Core (SQLite)**, and **xUnit**.
A **Next.js** front end will consume these APIs (CORS is already enabled for it).

---

## Features
- **Projects**: create, fetch, list (with ticket counts).
- **Tickets**: create, fetch (with comments), update, list with filters + pagination.
- **Status workflow**: status changes are validated against allowed transitions
  (e.g. Open → InProgress → Resolved → Closed, with reopen) — illegal moves return a clear 400.
- **Comments**: add and retrieve activity on a ticket.
- Enums stored as readable strings; indexes on the columns we filter by (Status, ProjectId).

---

## Architecture

```
Domain/          Entities (Project, Ticket, TicketComment) + enums
Application/      DTOs, the status Workflow, and orchestration services
Infrastructure/   EF Core DbContext + repositories
Api/Controllers/  Thin HTTP layer
```

- Controllers stay thin; business rules and validation live in the Application layer.
- The status workflow is isolated in one class, so the transition rules can't be bypassed
  and are unit-tested directly with no database or web host.
- Data access sits behind repository interfaces for testability and separation.

---

## Run it

Requires the **.NET 8 SDK**.

```bash
cd TicketManagement
dotnet restore
dotnet run        # opens http://localhost:5080/swagger
```
The SQLite database `ticketmanagement.db` is created automatically on first run.

### Tests
```bash
cd TicketManagement.Tests
dotnet test
```

---

## API

| Method | Route | Purpose |
|--------|-------|---------|
| POST   | `/api/projects` | Create a project |
| GET    | `/api/projects` | List projects |
| GET    | `/api/projects/{id}` | Get a project |
| POST   | `/api/tickets` | Create a ticket |
| GET    | `/api/tickets` | List tickets (filters: projectId, status, priority, assignee, search; paged) |
| GET    | `/api/tickets/{id}` | Get a ticket with comments |
| PUT    | `/api/tickets/{id}` | Update ticket fields |
| PATCH  | `/api/tickets/{id}/status` | Change status (workflow-validated) |
| POST   | `/api/tickets/{id}/comments` | Add a comment |

Ready-to-run requests are in `TicketManagement.http`.

---

## Scoped out (and how I'd add it next)
- A **Next.js** front end (board + ticket views) — next step.
- Authentication & roles (reporter vs assignee vs admin).
- Attachments, labels, and full-text search.
- EF Core migrations for versioned schema changes.
