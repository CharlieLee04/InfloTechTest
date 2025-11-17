# User Management Technical Exercise

Project Summary: 

Throughout the project I completed a range of the tasks that were set out, improving the overall architecture and maintainability of the application.

Standard level tasks: 
I completed all standard level tasks, focusing on the core CRUD functionality and UI improvements: 
- Implemented User Filtering.
- Extended the user model by adding DateOfBirth Property, ensuring it was displayed throughout the application in relevant places.
- Developed all user management screens, including: 
    Add user
    View user
    Edit user
    Delete user

Advanced level tasks: 
I expanded the system by introducing a comprehensive logging system: 
- Added infrastructure to record all primary user actions 
- Displayed a full action history for each user in the user screen
- Implemented the dedicated Logs page to show all log entrys in the system 
- Added detailed log entry views, allowing users to inspect individual logs and get more information about them
- Made the logs page user friendly, including a sorting and search feature, allowing users to search by persons name. 

Expert level task: 

I implemented a significant architectural enhancement by converting the data access layer into fully asynchronous operations, improving the performance future maintainability of the application. 

Platform level tasks: 
I added CI pipelines that automatically build the application and run tests. 
In addition to this, I tried to set up CD pipelines to deploy through azure, as this is something I have experience doing from university,
but I ran into a technical issue in which I couldn't access azure on my microsoft account. 

Overall, I really enjoyed working on this project, and I think it has really helped to further my knowledge.
I think I have gained more confidence, and improved on my skills. 



The exercise is an ASP.NET Core web application backed by Entity Framework Core, which faciliates management of some fictional users.
We recommend that you use [Visual Studio (Community Edition)](https://visualstudio.microsoft.com/downloads) or [Visual Studio Code](https://code.visualstudio.com/Download) to run and modify the application. 

**The application uses an in-memory database, so changes will not be persisted between executions.**

## The Exercise
Complete as many of the tasks below as you feel comfortable with. These are split into 4 levels of difficulty 
* **Standard** - Functionality that is common when working as a web developer
* **Advanced** - Slightly more technical tasks and problem solving
* **Expert** - Tasks with a higher level of problem solving and architecture needed
* **Platform** - Tasks with a focus on infrastructure and scaleability, rather than application development.

### 1. Filters Section (Standard) ✔️

The users page contains 3 buttons below the user listing - **Show All**, **Active Only** and **Non Active**. Show All has already been implemented. Implement the remaining buttons using the following logic:
* Active Only – This should show only users where their `IsActive` property is set to `true`
* Non Active – This should show only users where their `IsActive` property is set to `false`

### 2. User Model Properties (Standard) ✔️

Add a new property to the `User` class in the system called `DateOfBirth` which is to be used and displayed in relevant sections of the app.

### 3. Actions Section (Standard) ✔️

Create the code and UI flows for the following actions
* **Add** – A screen that allows you to create a new user and return to the list ✔️
* **View** - A screen that displays the information about a user ✔️
* **Edit** – A screen that allows you to edit a selected user from the list ✔️
* **Delete** – A screen that allows you to delete a selected user from the list ✔️

Each of these screens should contain appropriate data validation, which is communicated to the end user.

### 4. Data Logging (Advanced) ✔️

Extend the system to capture log information regarding primary actions performed on each user in the app.
* In the **View** screen there should be a list of all actions that have been performed against that user. ✔️
* There should be a new **Logs** page, containing a list of log entries across the application. ✔️
* In the Logs page, the user should be able to click into each entry to see more detail about it. ✔️

* In the Logs page, think about how you can provide a good user experience - even when there are many log entries. 

### 5. Extend the Application (Expert) Complete (Make Async) . 

Make a significant architectural change that improves the application.
Structurally, the user management application is very simple, and there are many ways it can be made more maintainable, scalable or testable.
Some ideas are:
* Re-implement the UI using a client side framework connecting to an API. Use of Blazor is preferred, but if you are more familiar with other frameworks, feel free to use them.
* Update the data access layer to support asynchronous operations.✔️
* Implement authentication and login based on the users being stored.
* Implement bundling of static assets.
* Update the data access layer to use a real database, and implement database schema migrations.

    Data access layer has been updated to be fully asynchronous. 

### 6. Future-Proof the Application (Platform) (Added CI pipelines)

Add additional layers to the application that will ensure that it is scaleable with many users or developers. For example:
* Add CI pipelines to run tests and build the application. ✔️
* Add CD pipelines to deploy the application to cloud infrastructure.
* Add IaC to support easy deployment to new environments.
* Introduce a message bus and/or worker to handle long-running operations.

## Additional Notes

* Please feel free to change or refactor any code that has been supplied within the solution and think about clean maintainable code and architecture when extending the project.
* If any additional packages, tools or setup are required to run your completed version, please document these thoroughly.
