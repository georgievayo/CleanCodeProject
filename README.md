# FindAndBook API

## Project Summary
The project represents simple ASP.NET Web API for booking tables in restaurants. It provides endpoints for register, login, create, edit and delete restaurants. It also provides search restaurants by name, address and average bill, book a table, get all bookings of restaurant and cancel booking. Only authenticated users can use all functionality of API.

## Entities
* User - has unique username, password, e-mail, first name, last name, phone number, role and collection of bookings.
* Manger - extends User model and adds collection of restaurants. 
* Restaurant - has name, manager, address, photo, contact, weekend working hours, weekday working hours, details, average bill, max count of people and collection of bookings. 
* Booking - has references to restaurant and user, time and number of people. 

## Project Structure
* Models - contains all models that are used for database.
* Factories - abstractions for creating models.
* Data Layer - contains database related classes and their abstractions (DbContext, Generic Repository and UnitOfWork).
* Service Layer - contains business logic of the project. It calls Data Layer to get needed data from database. It contains RestaurantsService, UsersService and BookingsService and their abstractions.
* Providers - contains authentication provider, datetime provider and http context provider.
* API - contains all controllers, mapper, configurations for Autofac, exception filter and token validation handler. It communicates with services and providers.

## Endpoints
All endpoints accept and return JSON objects.

### 1. Routes without authentication
* **POST** /api/users - creates new user. The body should contains username, password, email, firstName, lastName and phoneNumber.
* **POST** /api/login - generates token for user. The body should contains username and password. Returns generated token.
### 2. Authenticated routes
* **GET** /api/users/{userId} - returns profile of user with userId only if current user has userId.
* **DELETE** /api/users/{userId} - deletes profile of user with userId only if current user has userId.
* **POST** /api/restaurants - creates new restaurant only if current user is Manager. The body should contains address, name, contact, weekendHours, weekdayHours and maxPeopleCount. You can also specify photoUrl, details and averageBill. Returns all information about created restaurant.
* **GET** /api/restaurants/{id} - returns all information about restaurant.
* **GET** /api/restaurants - if there are no query parameters it will return all restaurants in the system. You can use searchBy parameter with value of "name", "address" or "averageBill" to search by property and pattern parameter to specify the word that you want to search.
* **GET** /api/users/{userId}/restaurants - returns manager's restaurants only if current user wants to see his restaurants.
* **PUT** /api/restaurants/{id} - updates restaurant with given id. The body of the request can contain address, contact, weekendHours, weekdayHours, maxPeopleCount, photoUrl, details and averageBill.
* **DELETE** /api/restaurants/{id} - deletes a restaurant with given id only if current user is the manager of this restaurant.
* **POST** /api/restaurants/{id}/bookings - books a table in restaurant with given id. The body of the request should contain time and peopleCount. Returns created booking.
* **GET** /api/restaurants/{id}/bookings - if there are no query parameters it returns all bookings of restaurant with given id. If there is time query parameter it returns all bookings of restaurant on given time.
* **DELETE** /api/bookings/{id} - deletes a booking with given id only if current user wants to delete his booking.


 

