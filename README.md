# DnDBeyond Backend Challenge - API

## Introduction

This is the backend API for the DnDBeyondChallenge application, which allows a client to manage a character's health by adding and removing HP. The API is built using ASP.NET Core and uses Redis for caching character data.

## Prerequisites

To run the project locally, make sure you have the following installed on your machine:

- **.NET SDK 8.0** (or later)  
  You can install it from [here](https://dotnet.microsoft.com/download).
- **Redis**  
  This project uses Redis for caching character data. You can either install Redis locally or use a Redis cloud service like [Redis Labs](https://redislabs.com/).
- **Docker(If using Docker Compose)**  
  If you want to run the project in a Docker container, you need to have Docker installed on your machine. You can install Docker from [here](https://www.docker.com/get-started).

## Getting Started

## Running Without Docker

1. **Clone the repository:**

   Open your terminal and run the following command:

   ```bash
   git clone https://github.com/yourusername/DnDBeyondChallenge.git
   cd DnDBeyondChallenge
   ```
   
2. **Install dependencies:**
	In your terminal run:
	```bash
   dotnet restore
   ```
3. **Set up Redis:**

	1. If you have redis installed locally, you can start it by using your preferred method. Note that the application is bound to redis:6379
	2. If you don't have redis installed locally, you can use Redis from Redis Labs or any other Redis cloud provider. Make sure to get the connection string.
		1. If necessary configure your redis connection by setting environment variable:
		```bash
			export REDIS_CONNECTION=localhost:6379
		```
4. **Run the application:**
   In your terminal run:
   ```bash
   dotnet run
   ```

   The API will be running at `http://localhost:8080`


## Running With Docker Compose

To run the application using **Docker Compose**, follow these steps:

1. Make sure Docker is installed and running on your system.

2. In the root of the project folder (where your `docker-compose.yml` file is located), build and run the services:
    ```bash
    docker-compose up --build
    ```

3. This will build the application and start both:
    - **The API** service (running on port 8080 by default).
    - **Redis** service for caching.

4. After a successful build, the API should be accessible at `http://localhost:8080` (or the port you’ve configured). Redis will be accessible from within the container as `redis:6379`.

## Environment Variables

- **REDIS_CONNECTION**: The connection string for Redis. In Docker Compose, it is set to `redis:6379` because the Redis container is accessible from the API container under the name `redis`. For local runs, set this variable to `localhost:6379` if you're running Redis locally.

Example `.env` file for local development:
```env
REDIS_CONNECTION=localhost:6379
```
---

## API Endpoints

The API exposes several endpoints. Below are examples of how to use each endpoint with their corresponding request and response bodies.

**GET /api/character/[name]**

Retrieve the character with the specified name.

#### Request:
No request body is needed.

#### Response 200:
```json
{
  "name": "Briv",
  "level": 5,
  "hitPoints": 25,
  "classes": [
    {
    "name":"fighter",
    "hitDiceValue":10,
    "classLevel":5
    }
  ],
  "stats":{
    "strength":15,
    "dexterity":12,
    "constitution":14,
    "intelligence":13,
    "wisdom":10,
    "charisma":8
  },
  "items":[
    {
      "name":"Ioun Stone of Fortitude",
      "modifier":{
        "affectedObject":"stats",
        "affectedValue":"constitution",
        "value":2
      }
    }
  ],
  "defenses":[
    {
      "type":"fire",
      "defense":"immunity"
    },
    {
      "type":"slashing",
      "defense":"resistance"
    }
  ]
}
```

Error code 404
```
"Character [name] not found."
```


**POST /api/character/[name]/damage**

Deal damage to the specified character. You cannot deal damage to a character with 0 HP.
Request Body
```json
{
  "damageType": "cold",
  "amount": 5
}
```

Response 200:
```json
{
    "damageTaken": 5,
    "name": "Briv",
    "hitPoints": 20,
    "temporaryHitPoints": 0
}
```

Error code 400
```
"[name] HP is at 0. Cannot take any more damage."
```

Error code 404
```
"Character [name] not found."
```

**POST /api/character/[name]/heal**

Heal the specified character. You cannot heal beyond max HP but heal action still takes place.

Request Body
```json
{
  "amount": 20
}
```
Response 200:
```json
{
    "maxHitPoints": 25,
    "name": "Briv",
    "hitPoints": 25,
    "temporaryHitPoints": 0
}
```

Error code 404
```
"Character [name] not found."
```

**POST /api/character/[name]/addTempHP**

Add temporary hit points to the character. Temporary hit points are used before actual hit points.

Cannot add hit points to a character with 0 health.

When adding temporary hit points, the character's current temporary hit points will be overwritten by the request value if the requet value is higher. They are not additive.

Request Body
```json
{
  "amount": 10
}
```

Response 200
```json
{
    "name": "Briv",
    "hitPoints": 25,
    "temporaryHitPoints": 10
}
```
Error code 400
```
"[name] HP is at 0. Cannot add temporary health.""
```

Error code 404
```
"Character [name] not found."
```