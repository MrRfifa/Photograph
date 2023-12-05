
# Photograph App

Photograph is a social media application developed using React, .NET, SQL, and SMTP. It allows users to create accounts, upload images, like and comment on other users' photos, creating a dynamic and engaging community.

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
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

## Demo
This video shows the home page of the app.
https://github.com/MrRfifa/Photograph/assets/101003527/1ced6853-71ac-4a70-b4b2-ead52d28d2a6

This video shows the process of registration and login.
https://github.com/MrRfifa/Photograph/assets/101003527/69a34c5b-d9bd-45b7-a71c-27fbf979917b

The video shows how to change the profile image, how to comment and delete the comment and finally the liking system.
https://github.com/MrRfifa/Photograph/assets/101003527/6c64bc35-0483-410b-99f6-ce61f374e991

The video shows how to upload an image.
https://github.com/MrRfifa/Photograph/assets/101003527/d50d5b8e-72d4-431c-99e4-f497a9bfdb64

The video shows the images in another account, commenting images, liking images ...
https://github.com/MrRfifa/Photograph/assets/101003527/23601fe2-873e-4953-b538-8c4c7010029a

This video shows how to delete an account (with account deletion everything related to that user is deleted.).
https://github.com/MrRfifa/Photograph/assets/101003527/8100ca93-9077-46f6-af57-0ea8b0f02f38


