# Background Order Processor
Demonstration of a background service processing when orders have a 'delivered' status.

I used Rider IDE: https://www.jetbrains.com/rider/

Service is built in .NET 9 so you'll need the latest .NET 9 SDK + Hosting Bundle: https://dotnet.microsoft.com/en-us/download/dotnet/9.0

App settings are environment variables - you can set launchSettings.json for local execution: 
![image](https://github.com/user-attachments/assets/b5e2deb2-50be-421d-9bbc-485a1e4ad75b)

You can use the docker-compose.yml to containerize and deploy the background service locally:
![image](https://github.com/user-attachments/assets/a4f8cfdf-0a1b-4d7a-8dd4-835d3df700d9)

 - Be sure to have Docker Desktop and Docker compose installed.
 - Navigate to the dir with docker-compose.yml and run `docker-compose up --build -d`
   
![image](https://github.com/user-attachments/assets/e2f40be6-f631-40c2-9db5-c7e65b4ba11f)

![image](https://github.com/user-attachments/assets/73be71b8-ac64-41fd-ba64-79c265fedd03)



There are basic happy path unit tests using xUnit and Moq:

![image](https://github.com/user-attachments/assets/1a4de62e-efc9-4ef8-ab7d-c0136c14ae8b)







