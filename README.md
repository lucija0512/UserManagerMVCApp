# User Manager MVC App

A .NET MVC web application for managing user forms with spam prevention, external API integration and email notifications.

## Installation

1. Clone the repository
2. Run the database setup script found in `Database/script.sql`
3. Update connection string and email settings in `appsettings.json`
4. Start the application

## Application Routes

### Home
- `GET /` - Home page
- `GET /Home/Error` - Error page
### User
- `GET /User/Create` - Displays user form
- `POST /User/Create` - Processes user form data - checks for spam, calls external API for additional data, stores the user in the database and sends the email
