
# Photograph App

Photograph is a social media application developed using React, .NET, SQL, and SMTP. It allows users to create accounts, upload images, like and comment on other users' photos, creating a dynamic and engaging community.

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologiesused)
- [Usage](#usage)
## Features

- **User Authentication:** Secure user accounts with authentication features.
- **Image Upload:** Users can upload their images to share with the community.
- **Likes and Comments:** Users can express their appreciation by liking and commenting on images.
- **Notifications:** Receive email notifications for various activities.
- **Responsive Design:** A responsive design that ensures a seamless user experience across devices.


## Technologies Used

- **Frontend:** React
- **Backend:** .NET
- **Database:** SQL
- **Email Notifications:** SMTP


## Usage

1. **Clone this repository** to your local machine:

    ```bash
    git clone https://github.com/mrrfifa/photograph.git
    ```

2. **Navigate to the cloned directory**:

    ```bash
    cd photograph
    ```

3. **Install dependencies**:

    # Frontend 
    ```bash
    cd frontend
    ```
    ```bash
    npm install
    ```

    # Backend
    ```bash
    cd backend
    ```
    ```bash
    dotnet restore
    ```
    In the backend directory create a .env file
    ```hcl
    EMAIL_ADDRESS=TCMP_Email_address
    EMAIL_USERNAME=TCMP_Username
    EMAIL_PASSWORD=TCMP_Password
    EMAIL_HOST=TCMP_Host
    DB_CONNECTION_STRING="Your_connection_string"
    CORS="frontend_url"
    ```

4. **Run migrations and updates**:

    ```bash
    cd backend
    ```
    ```bash
    dotnet ef migrations add InitialCreate
    ```
    ```bash
    dotnet ef database update
    ```
    

5. **Initialize Terraform by running**:
    
    ```bash
    cd backend
    ```
    ```bash
    dotnet run
    ```
    
    ```bash
    cd frontend
    ```
    ```bash
    npm run dev
    ```

6. **Review the plan to ensure everything is set up correctly:**:
      ```bash
    terraform plan
    ```
## Screenshots


