# Winning project at 2016 Hackathon BauerNZ

### Screenshot of the game

![Trump Cars - Game Screenshot](https://raw.githubusercontent.com/sridharrreddy/TrumpCars/master/trump-car-preview.png)

### Tech Stack 
- ASP.Net MVC
- ReactJs.Net
- Signal R
- SQL Server

### How the stack works
- ASP.Net and SQL Server manage data and business logic of the game
- SignalR allows the clients to communicate with each other immediately (with server only having to worry about game state)
- ReactJs.Net refreshes the page as and when the game state changes
