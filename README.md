FountainPensNG: A simple app to track my fountain pens & inks

docker build -t fuzzydice555/fountainpens-ng .
docker run -p 4200:4200 fuzzydice555/fountainpens-ng
docker push fuzzydice555/fountainpens-ng

docker compose:

services:
    fountainpens-ng-cl:
        image: fuzzydice555/fountainpens-ng-cl
        ports:
          - 4200:80
        restart: unless-stopped

services:
    fountainpens-api:
        image: fuzzydice555/fountainpens-api
        ports:
          - 4080:8080
        restart: unless-stopped

#client:
docker build -t fuzzydice555/fountainpens-ng-cl .      
docker run -p 4200:80 fuzzydice555/fountainpens-ng-cl
docker push fuzzydice555/fountainpens-ng-cl

#api:
docker build -t fuzzydice555/fountainpens-api .      
docker run -p 8080:8080 fuzzydice555/fountainpens-api
docker push fuzzydice555/fountainpens-api