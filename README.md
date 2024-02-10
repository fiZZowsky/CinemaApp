# CinemaApp

## Description

### General structure
CinemaApp is an IT system for the cinemas that uses hardware tickets in the form of RFID cards. This system consists of two parts:
    <br>
    - Web Application
    <br>
    - A physical device along with the software to operate it
    <br>

![General structure of IT system.](./GithubResources/ITSystemStructure.jpg)
<br>

### Web Application
#### Structure
Full-stack application built using .NET Core 7.0, Entity Framework Core, Identity and Microsoft SQL Server for the backend part. HTML/CSS, Bootstrap 5 and JavaScript were used to create the frontend part.
The application was created in accordance with Clean Architecture. Division of application layers:
<br>
![Division of architecture layers.](./GithubResources/CleanArchitecture.jpg)
<br>
![Division of application layers.](./GithubResources/AppStructure.png)
<br>

The following patterns were also used to build the application:
    <br>
    - CQRS
    <br>
    - Mediator
    <br>
    - MVC
    <br>
    - Dependency injection
    <br>
    - Builder
    <br>

The database was created using Microsoft SQL Server. Database schema:
![Database schema.](./GithubResources/DatabaseSchema.png)
<br>

### Physical device
The physical device used by the IT system is a ticket scanner in the form of RFID tags. The following were used for its construction:
    <br>
    - Arduino Uno R4 WiFi
    <br>
    - RFID-RC522
    <br>
    - 4-channel logic level converter
    <br>
    - 2 x LED
    <br>
    - 2 x 330R resistor
    <br>
The software code for the Arduino microcontroller was created using C++ and the Arduino IDE.
<br>
Diagram of the construction of a physical device:
<br>
![Physical Device Construction Diagram.](./GithubResources/ConstructionDiagram.png)
<br>

## Operation of the IT system
> [!IMPORTANT]
> For the system to function properly, it is required to create a publication of the MVC layer of the web application on the IIS server and to set the appropriate server data in the Arduino code configuration files. The server and the physical device should be connected to one network.

Presentation of the operation of the web application
<br>
https://github.com/fiZZowsky/CinemaApp/blob/main/GithubResources/Presentation.mp4
