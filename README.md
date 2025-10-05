# 🪐 Rockets Service 🚀

This application was made with **C# .NET Core 8** and **MongoDB**. 

# Setup and run the service using docker

 1. Clone the last version of the app.
 2. You need to install **[Docker](https://docker.com/),** and **[Compose](https://docs.docker.com/compose/)**.

 ### Building the image
In the root folder of the app the first time run:
```sh
docker compose up --build
```
This will build two containers (mongodb and rocketsapi), then will download and install all the dependencies in each container.

The next time you want to use it run:
```sh
docker compose up
```

To close down the docker containers run:
```sh
docker compose down
```

## API Reference

The **API endpoints** can be called from **[http://localhost:8088](http://localhost:8088)**\
The **Swagger** can be show in **[http://localhost:8088/swagger](http://localhost:8088/swagger)**\
You can use **Swagger** to test the endpoints or use **Postman** or another.

## MongoDB Reference


The **MongoDB* can be access with any MongoDB GUI tool like **MongoDB Compass** **http://localhost:27017**
> **UserName:** admin \
> **Password:** password

### Rockets Endpoints
#### Get all rockets
```http
  GET http://localhost:8088/rockets_list
```
| Parameter | Type     | Description                         |
| :-------- | :------- | :---------------------------------- |
| `sortBy`  | `string` | Sort all the rocket list posible values **(speed, mission or type)**, **type** by default |


#### Get rocket by channel
```http
  GET http://localhost:8088/rocket
```
| Parameter | Type     | Description                         |
| :-------- | :------- | :---------------------------------- |
| `channel`  | `string` | Channel of the rocket to fetch |

#### Launched, SpeedIncreased, SpeedDecreased, Exploded or MissionChanged in a rocket
```http
  POST http://localhost:8088/messages
  Content-Type: application/json
```
```json
  {
    "metadata": {
        "channel": "193270a9-c9cf-404a-8f83-838e71d9ae67",
        "messageNumber": 1,    
        "messageTime": "2022-02-02T19:39:05.86337+01:00",                                          
        "messageType": "RocketLaunched"                             
    },
    "message": {                                                    
        "type": "Falcon-9",
        "launchSpeed": 500,
        "mission": "ARTEMIS"  
    }
  }
```
| Parameter | Type     | Description                         |
| :-------- | :------- | :---------------------------------- |
| `metadata`      | `json` | **Required**. Contains the Channel(ID), and the message information like Number, Time and Type   |
| `message`      | `json` | **Required**. Contains the rocket information like Type, LaunchSpeed and Mission |

The messageType in the metadata object can be set with this values **(RocketLaunched, RocketSpeedIncreased, RocketSpeedDecreased, RocketExploded or RocketMissionChanged)**.

The message object change depending of the messageType like this:
| metadata.messageType | message     |
| :-------- | :------- |
| `RocketLaunched`      | { "type": "Falcon-9", "launchSpeed": 500, "mission": "ARTEMIS" } | 
| `RocketSpeedIncreased`      | { "by": 3000 } |
| `RocketSpeedDecreased`      | { "by": 2500 } |
| `RocketExploded`      | { "reason": "PRESSURE_VESSEL_FAILURE" } |
| `RocketMissionChanged`      | { "newMission":"SHUTTLE_MIR" } |


### Unit Tests
>In the same directory of the solution exists another project called **RocketsApi.Tests**\
![App Screenshot](https://github.com/mlacava/SharedImages/blob/main/ProjectPath.png?raw=true)

In this project there are some unit tests to the endpoints and all MessageTypes
>You can run it with **Visual Studio 2022** or **Visual Studio Code**\
![App Screenshot](https://github.com/mlacava/SharedImages/blob/main/dotnetTestsVS.png?raw=true)

>or running in the console in the project path **dotnet test**
![App Screenshot](https://github.com/mlacava/SharedImages/blob/main/dotnetTestConsole.png?raw=true)

Enjoy the solution 👋