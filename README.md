FountainPensNG: a web app for tracking fountain pens & inks

![Docker Image CI](https://github.com/adamsarok/FountainPensNg/actions/workflows/docker-image.yml/badge.svg)

Client:

[![Docker Hub](https://img.shields.io/docker/pulls/adamsarok/fountainpens-ng-cl.svg)](https://hub.docker.com/r/adamsarok/fountainpens-ng-cl)

API:

[![Docker Hub](https://img.shields.io/docker/pulls/adamsarok/fountainpens-api.svg)](https://hub.docker.com/r/adamsarok/fountainpens-api)

CDN:

[![Docker Hub](https://img.shields.io/docker/pulls/adamsarok/r2-api-go.svg)](https://hub.docker.com/r/adamsarok/r2-api-go)

Sample docker compose connecting to a separate Postgres server:
```
services:
    fountainpens-ng-cl:
        image: adamsarok/fountainpens-ng-cl
        ports:
          - 4200:80
        restart: unless-stopped
        environment:
          - apiUrl=http://DOCKER_HOST_IP:4080/api/
          - r2ApiUrl=http://DOCKER_HOST_IP:9088
    fountainpens-api:
        image: adamsarok/fountainpens-api
        ports:
          - 4080:8080
        restart: unless-stopped
        environment:
          - CONNECTION_STRING=Server=POSTGRES_IP; Port=5432; User Id=postgres; Password=POSTGRESPASS; Database=fountainpens
    r2-api-go:
        image: adamsarok/r2-api-go
        ports:
          - 9088:8080
        environment:
          - R2_ENDPOINT=https://YOUR_ENDPOINT.r2.cloudflarestorage.com
          - R2_BUCKET=test
          - R2_REGION=auto
          - R2_ACCESS_KEY=YOUR_ACCESS_KEY
          - R2_SECRET_KEY=YOUR_SECRET_KEY
          - R2_UPLOAD_EXPIRY_MINUTES=30
          - R2_DOWNLOAD_EXPIRY_MINUTES=30
          - CACHE_DIR=/config
        volumes:
          - /srv/YOUR_VOLUME/r2-api-go/cache:/config
        restart: unless-stopped
```
