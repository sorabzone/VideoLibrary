# VideoLibrary

# Requirement
Build a web app to allow customers to get the cheapest price for movies from these two providers in a timely manner

This is a sample API provides access to a mocked movies database.
There are 2 API operations available for 2 popular movie databases, cinemaworld and filmworld

/api/{cinemaworld or filmworld}/movies : This returns the movies that are available
/api/{cinemaworld or filmworld}/movie/{ID}: This returns the details of a single movie
To access this API you'll require an API token.

Also just like any realworld API these may be flakey at times

# How to Run and Test
1. Clone Repository
2. Open in Visual Studio 2017 and execute VideoLibrary. It is .Net Core MVC + React/Redux project created on top of Visual Studio boiler plate template.
3. UnitTest in project "VideoLibrary.UnitTest" can be run from Visual Studio,  
4. To execute UnitTest from command prompt, go to "\VideoLibrary\VideoLibrary.UnitTest" and execute following command

			dotnet test
			

# Solution Structure

VideoLibrary

It is .Net Core MVC + React/Redux project created on top of Visual Studio boiler plate template. Not much changed in the defaukt template except the page to display movies list.

App will always receive response in an standard format from API. If Code is 200 then the result is expected otherwise failed or exception.

			public class CustomResponse<TData>
			{
				public StatusCode Code { get; set; }
				public string Message { get; set; }
				public TData Data { get; set; }
			}


		CI/CD pipeline is also created and integrated to AppVeyor(OpenSource), to build and execute unit tests.


VideoLibrary.Logger

It is a common logger created using NLog. By default application is configured to write logs in text file.


VideoLibrary.Engine

All business logic is written in this project.

1. MoviesService class calls Webjet API to get movies data.
2. Key for API is read from environment variable.
3. ServiceAPIClient is used to maintain HttpClients, so that application doesn't create it for each request.
4. All services are injected as singleton at startup.
5. If there is an invalid response from webjetAPI, then the response is null and is handled accordingly
6. API are called in parallel to fetch and combine results.

# Note: HttpClient is configured to timeout in 3 seconds for this test application. If there is no response from WebjetAPI, then request will timeout after 3 seconds


VideoLibrary.UnitTest

Unit tests are created using NUnit and executed in CI pipeline.


There are several interface, extension, constant and helper classes are created to follow SOLID principles. It also helps in scaling and expanding the application in future.

# Assumptions & Limitations
1. Application display the combined result from bith movie sources. Movies are fetched from both cinemaworld and filmworld, the results are then combined to only return the movie with cheaper price.
2. ID is displayed on screen, so that you can know the source of library
3. Suppose movies list API returns 10 movies, but there is error in fetching movie detail, then that movie is not displayed on UI.
4. There is Reload buttonin UI to fetch results again.
