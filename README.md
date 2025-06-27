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
- CDN/Storage: Cloudflare R2


## Quick Start
Example docker-compose:

```
services:
    fountainpens-ng-cl:
        image: adamsarok/fountainpens-ng-cl
        ports:
          - 4200:80
        environment:
          - apiUrl=http://fountainpens-api:8080
        restart: unless-stopped

    fountainpens-api:
        image: adamsarok/fountainpens-api
        ports:
          - 4080:8080
        restart: unless-stopped
        environment:
          - ConnectionStrings__FountainPens=Server=db; Port=5432; User Id=postgres; Password=postgres; Database=fountainpens
          - R2__AccessKey=${R2_ACCESS_KEY}
          - R2__SecretKey=${R2_SECRET_KEY}
          - R2__AccountId=${R2_ACCOUNT_ID}
          - R2__BucketName=${R2_BUCKET_NAME}
          - R2__ExpirationHours=24
          - R2__MaxFileSizeKb=256
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
        POSTGRES_DB: fountainpens 
      ports:
        - 5432:5432

```

## Badges

![Docker Image CI](https://github.com/adamsarok/FountainPensNg/actions/workflows/docker-image.yml/badge.svg)
[![codecov](https://codecov.io/github/adamsarok/FountainPensNg/graph/badge.svg?token=4HATTWKM9V)](https://codecov.io/github/adamsarok/FountainPensNg)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=adamsarok_FountainPensNg&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=adamsarok_FountainPensNg)

Client: [![Docker Hub](https://img.shields.io/docker/pulls/adamsarok/fountainpens-ng-cl.svg)](https://hub.docker.com/r/adamsarok/fountainpens-ng-cl)

API: [![Docker Hub](https://img.shields.io/docker/pulls/adamsarok/fountainpens-api.svg)](https://hub.docker.com/r/adamsarok/fountainpens-api)