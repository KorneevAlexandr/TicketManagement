# Solution task 5. React application.

As a result of task 5, some of the functionality of the previously developed application has been ported to the client React.js application. User Profile page and EventManagement functionality moved to JavaScript files. The old implementation of the specified functionality has been preserved. The underlying ASP.NET Core application is used as the host for the client application.

## Description React application.

Added a new ASP.NET Core project called ReactUI that hosts the client React.js application. This implementation allows you not to violate CORS rules. Also, the ASP.NET Core application calls other API methods required for the client application, it also has the functionality of authentication and authorization based on JWT tokens, logging using Serilog.  

The client application has the same functionality as the old application. Several components have been developed for User Profile page and EventManagement, which are located in different folders. The app has links to various pages, including links to the pages of the old app where appropriate. For links, react-router-dom is used. The styling has changed a bit, modals and pop-ups are used. Added application localization for 3 languages (English, Russian and Belarusian) using react-localization. The information about the selected language is stored in a cookie. To retrieve data, the React application calls the methods of the ASP.NET Core application that is being hosted, which calls the methods of the APIs.

### Interaction of client applications using feature flags.

To interact with the old application, the feature flag is used. Attributes have been applied to the corresponding controllers. To redirect requests to a new application, ‘RedirectDisabledFeatureHandler’ is used, which checks the value of the flag in the ‘appsettings.json’ file and redirects the request. To use only the old application, you need to set the ‘UseOnlyMvc’ element of the ‘FeatureManagement’ section to ‘true’ or ‘false’ if you want to use the new React application. If set to ‘false’, it will redirect to the User Profile page and EventManagement pages, and return to the old application when exiting those pages.

## Description of Web-projects.

The application has 6 launch projects. 4 of them are different APIs (EventAPI, UserAPI, VenueAPI, PurchaseAPI) with Swagger documentation, and 2 projects for display - the MVC PresentationLayer project and - ReactUI, which access these APIs and have different views for the user. 
All web projects are runnable. All projects use one database as a data source, the connection string to which is specified for each project API. ASP.NET Core JWT-authentification is used to authenticate the user. UserAPI generates a JWT token for the authorized user, which is passed in every request in the header. For saving and adding a token, the corresponding middleware is implemented.

All web projects are logged with Serilog. All incoming requests are processed using a special middleware that writes all data about the request to the console and .log file. All .log files are created and used locally and are not passed to, for example, the GitHub repository. To call the client (MVC, React) to the required API, the RestEasy library is used. Using it, an interface and extension methods for it are written, which call methods from the API.

For models that are manipulated by both the API and the client, the TicketManagement.ClientModels class library has been developed, which has a set of classes that simply describe models without business logic.

The APIs themselves don't know anything about the client and just have the appropriate controllers and methods. They use the Json format as responses to which objects of types from TicketManagement.ClientModels are serialized.

## Developed by Web APIs.

UserAPI. This API is intended for authentication, authorization and user management in the system. JWT-Authentification or JWT token-based authentication is used for authentication. It is this API that generates and transfers tokens to the client.

EventAPI. This API is for managing events. All of its controllers and their methods responsible for modifying events are accessible only to a user logged in with the ModeratorEvent role.

VenueAPI. This API is intended for managing venues and everything related to them - layouts, areas, seats. Its controllers and methods are available only to a user with the ModeratorVenue role.

PurchaseAPI. This API allows you to purchase tickets for events. Its controllers and methods are available to all users except the administrator.

## Application launch.

To run the application, you need to deploy the database. Download the app from GitHub and unzip the downloaded archive. Then open the project solution and follow these steps:
- you need to open the TicketManagement.Database project -> Publish and in the window that opens, select the server that suits you;
- specify the desired database name and create a database deployment script;
- the created SQL script must be run, as a result of which a new database will be created, ready for testing and containing the necessary data in advance;
- find and open files 'appsettings.json' in all Web-API projects;
- inside the file, for the 'ConnectionStrings' section in the 'DefaultConnection' variable in the 'Initial Catalog' attribute, specify the name of the created database.

It is recommended to deploy 2 databases at once with different names, one for using the application and one for the integration tests.

After specifying and configuring the database connection, you can open the project root folder and simply run the 'AppStart.bat' file. This file contains a set of commands that build and run the application. After executing the commands in this file, you will automatically open a browser at the desired address.

If you have done everything correctly, the browser will display the start page of the application and you can use it. If you want to see the functionality of any Web API, you can open them at the addresses specified in the 'appsettings.json' file of the 'TicketManagement.WebApplication' project.

To use the system by different users, the system has several accounts that are created automatically when the database is deployed. Their logins and passwords are presented below.

### Start login details ThirdPartyEventEditor:
ModeratorEvent: Login - '1111', Password - '1111'.  
ModeratorExport: Login - '2222', Password - '2222'.  

### Start login details for TicketManagement system:
Admin: Login - 'Admin', Password - 'Admin'.  
ModeratorEvent: Login - 'Alex', Password - 'Event'.  
ModeratorVenue: Login - 'Maks', Password - 'Venue'.  
User 1: Login - 'Igor', Password - 'User'.  
User 2: Login - 'Irina', Password - 'UserX'.  

## Testing the application.
The app is covered with unit and integration tests. For old classes, previously written tests are used, new classes of web projects are covered with unit tests using the Moq 
library.

### Unit tests.

Unit tests only test specific parts of the system and do not require an actual data source or connection to a data source. You don't need to prepare to run them, unless you are missing a library from NuGet. If so, download the required packages. Tests should be repeatable and quick.

### Integration tests.

To run the integration tests, you need to deploy the test database. This process is described above in the 'Launching the application' section. To connect to the created database, you need to specify the connection string in the 'appSettings.json' file of the 'TicketManagement.IntegrationTests' integration tests project, changing the 'Initial Catalog' value (specifying the name of the test database) and for the 'TestConnectionString' variable.

If you did everything correctly, the tests are relatively fast to run as soon as you connect to the database. Do not modify, delete, or add data to the test database, as integration tests use the data that was originally there and generate additional data themselves. Changing the data integrity in the test database can lead to errors and failing tests.
