# Background Order Processor
Demonstration of a background service processing when orders have a 'delivered' status.

I used Rider IDE: https://www.jetbrains.com/rider/

Service is built in .net so you'll need the latest .net 6 hosting bundle: [https://dotnet.microsoft.com/en-us/download/dotnet/6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

You can use the docker-compose.yml to containerize and deploy the background service locally:
 - Be sure to have Docker Desktop and Docker compose installed.
 - Navigate to the dir with docker-compose.yml and run `docker-compose up --build -d`
   
![image](https://github.com/user-attachments/assets/c5a5f9f2-e6c0-470b-a458-d782714001ff)

![image](https://github.com/user-attachments/assets/1f8f2537-65ef-4fd8-9608-7466d99d664e)


There are basic happy path unit tests:

![image](https://github.com/user-attachments/assets/1a4de62e-efc9-4ef8-ab7d-c0136c14ae8b)







