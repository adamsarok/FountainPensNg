## What is FountainPensNg?

FountainPensNg is an Angular app for managing your fountain pen, ink, and paper collection. Organize inks by color, save swatches, and keep track of which pens are currently inked.

## Features

✅ Track Your Collection → Track and review your pens, inks and papers.

✅ Store swatches and organize by color.

✅ Usage Logging → "Inked Up" function lets you track usage.

✅ Full-Text Search → Find perfumes to review or check perfumes with zero stock.

## Tech Stack
- Frontend: Angular 19
- Backend: ASP.NET Core 9
- Database: PostgreSQL
- CDN/Storage: Go & Cloudflare R2


## Quick Start
Example docker-compose:

```
services:
    fountainpens-ng-cl:
        image: adamsarok/fountainpens-ng-cl
        ports:
          - 4200:80
        environment:
          - apiUrl=http://fountainpens-api:8080/api
          - r2ApiUrl=http://r2-api-go:8080
        restart: unless-stopped

    fountainpens-api:
        image: adamsarok/fountainpens-api
        ports:
          - 4080:8080
        restart: unless-stopped
        environment:
          - ConnectionStrings__DefaultConnection=Server=db; Port=5432; User Id=postgres; Password=postgres; Database=fountainpens
        restart: unless-stopped

    db:
      image: postgres:16.5
      restart: always
      shm_size: 128mb
      volumes:
      - ./postgres:/var/lib/postgresql/data
      environment:
        POSTGRES_PASSWORD: postgres
        PGDATA: /var/lib/postgresql/data/pgdata
        POSTGRES_DB: metabaseappdb 
      ports:
        - 5432:5432

    r2-api-go:
        image: adamsarok/r2-api-go
        ports:
          - 9088:8080
        environment:
          - R2_ENDPOINT=${R2_ENDPOINT}
          - R2_BUCKET=test
          - R2_REGION=auto
          - R2_ACCESS_KEY=${R2_ACCESS_KEY}
          - R2_SECRET_KEY=${R2_SECRET_KEY}
          - R2_UPLOAD_EXPIRY_MINUTES=30m
          - R2_DOWNLOAD_EXPIRY_MINUTES=30m
          - CACHE_DIR=/config
        volumes:
          - ./r2-api-go/cache:/config
        restart: unless-stopped
```

## Badges

![Docker Image CI](https://github.com/adamsarok/FountainPensNg/actions/workflows/docker-image.yml/badge.svg)
[![codecov](https://codecov.io/github/adamsarok/FountainPensNg/graph/badge.svg?token=4HATTWKM9V)](https://codecov.io/github/adamsarok/FountainPensNg)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=adamsarok_FountainPensNg&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=adamsarok_FountainPensNg)

Client: [![Docker Hub](https://img.shields.io/docker/pulls/adamsarok/fountainpens-ng-cl.svg)](https://hub.docker.com/r/adamsarok/fountainpens-ng-cl)

API: [![Docker Hub](https://img.shields.io/docker/pulls/adamsarok/fountainpens-api.svg)](https://hub.docker.com/r/adamsarok/fountainpens-api)

CDN: [![Docker Hub](https://img.shields.io/docker/pulls/adamsarok/r2-api-go.svg)](https://hub.docker.com/r/adamsarok/r2-api-go)
