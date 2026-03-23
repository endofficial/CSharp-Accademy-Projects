# CODING TRACKER

Coding Tracker is my C# application. It is used to record working sessions, using SQlite.

This application is CRUD (Create, Read, Update e Delete).

## Key Features

- **Session recording**: The program records new work sessions, recording date, a start time, a end time and storing a duration.
- **CRUD**: The program allows to add, read, update and delete the sessions.
- **Code details**: The code is written using Spectre.Console.
- **Store date**: The program communicate with a database, using SQlite with Dapper.
- **Database initialization**: If database don't exists, it is auto-generated automatically.
- **Self-genereting data**: If database is empty, 100 records are generated.
- **Error handling**: The code provides a robust error handling user generated and possible exceptions.
- **Unit test**: The code provides the unit tests to test the correct function of the methods.
- **Console UI**: Open the program, after loading the database, is shown a menu. The user can move the directional arrows to navigate in the menu.

	- <img src="Resources\doc\images\Menu.png" alt="menu" width="500"> 

## Functionality & Usage

- **Live session**
	- ***Date recording***: Record date of the session.
		- <img src="Resources\doc\images\Live-session-date.png" alt="date" width="500">
	
	- ***User choice***: Type 'P' or 'p' to continue. Type '0' to return to main menu. 
		- <img src="Resources\doc\images\play-session.png" alt="play session" width="500">
	
	- ***Description***: Add a description for the session.
		- <img src="Resources\doc\images\description.png" alt="description session" width="500">
	
	- ***Start session***: The stopwatch is start and it is stop when the user presses any key.
		- <img src="Resources\doc\images\live-session.png" alt="start session" width="500">

- **Register a new session**
	- ***Date recording***: Record date of the session.
	- ***Description***: Add a description for the session.
	- ***Start time***: Enter the start time.
		- <img src="Resources\doc\images\startTime.png" alt="Start Time" width="500">
	
	- ***End time***: Enter the end time.
		- <img src="Resources\doc\images\endTime.png" alt="End Time" width="500">
	
- **View sessions**
	- ***View of the sessions***: It's shown a table with all the sessions.
		- <img src="Resources\doc\images\all-records.png" alt="All records" width="500">
	
	- ***Fiters***: A list of filters is displayed
		- <img src="Resources\doc\images\choice-list.png" alt="view filters" width="500">
	
		- ***Year filter***: The user can choose the year that he wants to view is displayed.
			- <img src="Resources\doc\images\orderToYear.png" alt="filter to year" width="500">
		
		- ***Month filter***: The user can choose the month that he wants to view is displayed.
			- <img src="Resources\doc\images\orderToMonth.png" alt="filter to month" width="500">
		
		- ***Day filter***: The user can choose the day that he wants to view is displayed.
			- <img src="Resources\doc\images\orderToDay.png" alt="filter to day" width="500">
		
		- ***Ascending order filter***: The user can choose to view sessions by ascending order.
		- ***Descending order filter***: The user can choose to view sessions by descending order.
		
- **Update session**
	- ***View of the sessions***: It's shown a table with all the sessions.
	- ***What you want update?***: It's shown a list and the user can navigate to choose what to update.
		
		- <img src="Resources\doc\images\all-sessions-update.png" alt="view all sessions" width="500">

- **Delete session**
	- ***View of the sessions***: It's shown a table with all the sessions.
		- <img src="Resources\doc\images\allsessiontodelete.png" alt="view all sessions to delete" width="500">

	- ***Delete all database***: Type '1' to delete all database.
	- ***Delete one session***: If the user press '2', he will have to indicate the Id session he wants to delete.
		- <img src="Resources\doc\images\allsessiontodelete.png" alt="option to delete one session" width="500">

## What I learned

- Database integration: I learned how to create, connect and communicate with a database using Dapper.
- Stopwatch integration: I learned how to implement the stopwatch method to live session.
- Problem solving: I had difficulties to implement the filters. I solved it with a serch on the web. I understand the 'strftime' function. It is used to format date and time and convert in text.
- Unit testing: I implement of the unit tests to verify the correct function of the methods.