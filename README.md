docker build -t fuzzydice555/fountainpens-ng .
docker run -p 4200:4200 fuzzydice555/fountainpens-ng
docker push fuzzydice555/fountainpens-ng

docker compose:

services:
    perfume-tracker:
        image: fuzzydice555/fountainpens-ng
        ports:
          - 4200:4200
        restart: unless-stopped