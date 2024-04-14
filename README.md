# PixelChopper

PixelChopper is an application designed to resizes images into 100x100px
thumbnails.

## How to Use

### **Prerequisites**

Docker, Docker compose, Git

### Getting Started

Follow these steps to set up the application:

- clone the Repository:

- cd PixelChopper

- Create a .env file in the root directory of your project.

- Add the necessary environment variables for your Azure Blob Storage connection strings:
 AzureBlobStorage__ConnectionString=your_connection_string_here

## How it works

 The PixelChopper application exposes two endpoints:

POST /api/images: This endpoint accepts an image file in the request body and resizes it to a 100x100px thumbnail. The endpoint responds with a URL that can be used to retrieve the resized image, with the blob name as a parameter.
To use this endpoint, you would send a POST request with an image file in the request body. The Content-Type header should be multipart/form-data.

GET /api/images/{blobName}: This endpoint retrieves an image from Azure Blob Storage. The blobName parameter in the URL is the blob name of the image you want to retrieve.
To use this endpoint, you would send a GET request with the blob name of the image you want to retrieve in the URL.


## Architecture

The PixelChopper application consists of two main services:

Web API: This service acts as the entry point for the application. It accepts an image, stores it in Azure Blob Storage, and publishes a notification to a RabbitMQ queue. The notification contains the ID of the original blob and the ID that should be used to save the resized image. The ID for the resized image is sent back to the caller.

Background Worker Service: This service listens to the RabbitMQ queue and consumes the messages. When it receives a message, it retrieves the original blob from Azure Blob Storage, resizes the image using ImageSharp library , and uploads it back to Azure Blob Storage with the provided ID for the resized image.
The caller can retrieve the resized image by making a GET request to the /api/images/{blobName} endpoint, replacing {blobName} with the ID of the resized image.

Docker: The application is containerized using Docker, which makes it easy to deploy and scale. Each component runs in its own Docker container, and Docker Compose is used to manage the containers.

The xUnit framework is used for testing.
This project uses GitHub Actions for Continuous Integration.

## Production Considerations

To use this application in a production environment, consider the following:

- **Secure Entrypoint:** Implement a secure entrypoint such as an API Gateway. This can handle tasks like API authentication, rate limiting, and routing requests to the appropriate services.

- **Sensitive Configuration:** Store sensitive configuration data in a secure storage service like Azure Key Vault. This includes connection strings, API keys, and other secrets.

## Scalability Considerations

To scale this application to handle more load, consider the following:

- **Web API:** The Web API can be scaled by deploying more container instances. Consider using a load balancer or API Gateway to distribute the load among the instances. Cloud services like Azure App Services can simplify the deployment and scaling process.

- **Background Worker Service:** The Background Worker Service can be scaled by deploying more container instances. This service can leverage the RabbitMQ queue to handle increased load. As messages can be consumed by any worker, scaling can be as simple as adding more workers.


Before making scalability decisions, it's important to have a clear understanding of the application's behavior under different circumstances. This can be achieved through observability using tools like Application Insights or Datadog to collect logs, metrics and traces.
