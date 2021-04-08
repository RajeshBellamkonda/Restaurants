## Restaurants Search

##### This web application can be used to search restaurants by postcode or by using the GeoLocation*  
*browser needs location access for GeoLocation search to work.

#### Application
* **Pre-requisite:** dotnet core 3.1 to build and run.
	* To run the application, please make sure dotnet runtime / hosting bundle 3.1.x is installed. Download at : https://dotnet.microsoft.com/download/dotnet/3.1
* **Run the Application:**  dotnet CLI can be used to run the application. Using powershell / command prompt / bash and navigate to folder {SolutionRootFolder}\Restaurants and use `dotnet run` command to run the application. Once the application starts, go to the browser https://localhost:5001 address to use the application.
* **External Dependency:** The application has dependency on external API (https://uk.api.just-eat.io/restaurants/) to search and get the results.
* **Logging:** Once the application is started, logging is captured in the console at Info level by default.
* **HealthCheck endpoint:** https://localhost:5001

#### Tests
* There are unit & integration level tests covering the acceptance criteria, controller and Service layers.  
* Tests can be run by using the `dotnet test` command from the {SolutionRootFolder}.
