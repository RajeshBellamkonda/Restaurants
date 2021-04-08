## Answers to technical questions

**1. How long did you spend on the coding test? What would you add to your solution if you had more time? If you didn't spend much time on the coding test then use this as an opportunity to explain what you would add.**
 * Around 10 hours

**2. What was the most useful feature that was added to the latest version of your chosen language? Please include a snippet of code that shows how you've used it.**

* I found the Switch Expressions in c#8 to be much useful, it reduces the boilerplate code while making the code more expressive. 

* Also in c#8, inline 'Using' declaration for IDisposable implemented objects is much more simpler.

Listed out some latest language features used in my excercise.
 * c#7:
	- out variables inline declaration.  
	Example: `_cache.TryGetValue(cacheKey, out RestaurantsSearchResults restaurantsSearchResults);`
	- Non-trailing named arguments  
	Example:  HomeController Index method -  
	`public async Task<IActionResult> Index(string postcode = null, string latitude = null, string longitude = null, int page = 1)`
	using Named Optional Argument parameters to skip postcode as below.  
	`var result = await _homeController.Index(latitude: latitude, longitude: longitude);`
	
 * c#6:
	- $ - string interpolation  
	Example: `ErrorMessage = $"No resturants found for postcode: {postcode}";`
	- Auto-property initializers  
	Example: `public int Page { get; set; } = 1;`
	- Null-conditional operators ?.  
	Example: `response.RestaurantsSearchResults?.Restaurants == null`

**3. How would you track down a performance issue in production? Have you ever had to do this?**
 - Yes, I have followed the below steps:
	* Check heartbeat endpoints monitor if there are any flags / unavailability
	* Check metrics for the services and pinpoint on the service where the performance bottleneck is. Where the requests are taking longer, is it expected etc.
	* After pinpointing the service that is causing performance issue, check if it is all the requests that are causing the performance lag or just some type of requests.
	* If all the requests have performance issues, check the memory / cpu allocation is as expected. Are the external components (outside APIs / Cloud Services / DBs etc.) functioning as expected.
	* If only certain kinds of requests are causing the issue, check the logs to see which component (ie. validation layer / Service layer / Repository layer etc.) is causing the issue.
	* Once the area in the code has been identified, check the logs in that area to see the inputs are as expected. 
	* Run tests with similar mock data on a Dev /Test / Staging environment to see if the problem occurs there too. If yes, then replicate the issue locally and raise a bug.

**4. How would you improve the Just Eat APIs that you just used?**  
I have used the below API for searching restaurants by postcode  
https://uk.api.just-eat.io/restaurants/bypostcode/ec4m  

My observations:
- Restful & Single responsibility: I would keep the endpoint simple with specific information - different endpoints for metadata, offers, costs etc.
- Paging may be better instead of giving back all the results within the same call.
- Incomplete postcode validation - Implement not found for invalid postcodes and return appropriate API response codes as the geolocation endpoint is returing.  
  For example an invalid postcode search(https://uk.api.just-eat.io/restaurants/bypostcode/abc123) returns 200 response, where as https://uk.api.just-eat.io/restaurants/bypostcode/{postcode} return 400 Bad Request.
- Using this request I got the ASP.net yellow screen error message.  
  https://uk.api.just-eat.io/restaurants/bypostcode/<postcode>  
  It should have been a 404 / 400 Response.
- Some data may not be in correct area like 'Low delivery fee' in Cusine Types.