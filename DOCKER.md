# Docker Compose Setup

This docker-compose setup spins up both FinanceAPI and FinanceMcpServerWebApi with proper networking.

## Services

- **financeapi**: The Finance API service (exposed on port 5000)
- **financemcpserverwebapi**: The MCP Server Web API that connects to FinanceAPI (exposed on port 5001)

## Architecture

```
┌─────────────────────────┐
│  FinanceMcpServerWebApi │
│     (port 5001)         │
└───────────┬─────────────┘
            │
            │ http://financeapi:8080
            ▼
    ┌───────────────┐
    │  FinanceAPI   │
    │  (port 5000)  │
    └───────────────┘
```

## Usage

### Build and Start Services

```bash
# Build and start all services
docker-compose up --build

# Start in detached mode (background)
docker-compose up -d --build
```

### Stop Services

```bash
# Stop services
docker-compose down

# Stop and remove volumes
docker-compose down -v
```

### View Logs

```bash
# View all logs
docker-compose logs

# View specific service logs
docker-compose logs financeapi
docker-compose logs financemcpserverwebapi

# Follow logs in real-time
docker-compose logs -f
```

### Rebuild Single Service

```bash
# Rebuild only FinanceAPI
docker-compose up -d --build financeapi

# Rebuild only FinanceMcpServerWebApi
docker-compose up -d --build financemcpserverwebapi
```

## Endpoints

- **FinanceAPI**: http://localhost:5000
- **FinanceMcpServerWebApi**: http://localhost:5001
- **MCP Endpoint**: http://localhost:5001/mcp

## Configuration

The FinanceMcpServerWebApi connects to FinanceAPI using:
- **In Docker**: `http://financeapi:8080` (uses Docker service name)
- **Local Development**: `http://localhost:5000` (from appsettings.json)

The configuration is set via environment variable in docker-compose.yml:
```yaml
environment:
  - FinanceApi__BaseUrl=http://financeapi:8080
```

## Troubleshooting

### Check if services are running
```bash
docker-compose ps
```

### Check container logs for errors
```bash
docker-compose logs financemcpserverwebapi
```

### Access container shell
```bash
docker exec -it financemcpserverwebapi /bin/bash
```

### Test connectivity between containers
```bash
# From financemcpserverwebapi container, ping financeapi
docker exec -it financemcpserverwebapi ping financeapi
```

### Full Reset (Nuclear Option)
```bash
# Stop all containers
docker-compose down

# Remove all volumes, networks, and images
docker system prune -a --volumes -f

# Rebuild from scratch
docker-compose up --build
```
