# Trump Cars

A trump card game based on cars for 2 players. 

### Winning project at 2016 Hackathon BauerNZ

### Screenshot of the game

![Trump Cars - Game Screenshot](https://raw.githubusercontent.com/sridharrreddy/TrumpCars/master/trump-car-preview.png)

### Tech Stack 
- ASP.Net MVC
- ReactJs.Net
- SignalR
- SQL Server

### How the stack works
- ASP.Net and SQL Server manage business logic and data of the game
- SignalR allows the clients to communicate with each other realtime, via sockets, while server only has to manage game state.
- ReactJs.Net refreshes the page as & when the game state changes
