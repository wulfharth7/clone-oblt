# Overview

Obilet Clone has been coded by me for Obilet Firm as an take-home assignment. The project is structured around a modular architecture, using controllers, services, and utility classes to ensure separation of concerns and maintainability. External APIs are used in this project.
## How to Run the Program?
- Install Dependencies for ASP.NET. 
- Change the INPUT_API_KEY_HERE in appsettings.json. 
- Run

# Project Structure
## Bus Location Search (Main Page)
![image](https://github.com/user-attachments/assets/0c34bf86-2254-418f-bc55-6d1070521a67)
- [x] Whenever a new user is using the system, the location text boxes are pre-populated from the external api by default.
- [x] If a user has used the system before and closed the page, when they come back they will see their last choices of destination and origin cities that is stored in the local storage.
  - [x] They also will see the departure date, but if its older than today, the default departure date will be set to tomorrow.
- [x] Swapping button works. Quick selection of today and tomorrow date buttons do work.
- [x] Users can't choose the same cities for Arrival and Departure cities. 
## Journey Section (Ticket Page)
![image](https://github.com/user-attachments/assets/dda4c698-5168-442a-9f92-da530b9809a7)
- [x] The journeys are listed on this page. They are sorted on their departure dates from the earliest to latest.
- [x] Users can see the logos, bus stop names, departure and arrival time.
- [x] When users click the back button, they will be redirected to index page.
### Mobile Compability
![image](https://github.com/user-attachments/assets/1440a820-f059-46e4-8740-a3d6f8f6f90b)
- [x] Media queries have been written for the mobile version.
### Journey Url System
![image](https://github.com/user-attachments/assets/9a3221e6-a918-41c3-851d-3231a3e7d776)
- [x] When a user searches for journeys, they are being redirected to `/seferler/{originid}-{destinationid}/{departure-date}`
## Functional Requirements
- [x] A new session gets created for every user using `GetSession` external api. All requests that are called from the client goes to my MVC Backend system instead of direct external api calls.
- [x] Each user uses his/her own session information in the subsequent API requests made by the application on behalf of that user.
- [x] Default value for the `Departure Date` field is `tomorrow`
- [x] Users are able to perform text-based search on origin and destination fields. The search keyword user enters is used in order to fetch related bus locations from the obilet.com business API.

## Extra Notes

1. **Builders**:
   - When I was developing the project, there was a constant need to create new request objects for payloads in every apiservice that was running.
   - This was making the code a mess, less readable and non-modular.
   - Hence I've created builder classes for these request DTOs that exist in the `Builders` folder. It could've also been developed as Factory Method also.
   - But after inspecting and thinking of the codebase, Builder was the way to go.
    
## Key Features

- **API Integration**: Modular services to handle API requests for bus locations, journeys, and session management.
- **Modular Architecture**: Uses interfaces and builders for scalability and maintainability.
- **Utilities**: Helper classes to simplify repetitive tasks such as setting headers and managing API responses.

## Front-End
- React is being used for this project. There are 2 pages as Index and Journey pages.
- Both of them are available as screenshots in this readme as you can see.
- Frontend starts running when the ASP.NET App starts running automatically. The production code is built with `npm run build` and exists under wwwroot.
- However, if you want to look for the client-side code, its also available in this repository under the `oblt-client` folder as a standalone react app.

