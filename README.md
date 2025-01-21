# Photo Editing Pipeline

## Overview

The **Photo Editing Pipeline** is a serverless, event-driven image processing system built using Azure Functions and Durable Functions. It supports dynamic tasks such as image resizing, watermarking, video transcoding, and metadata extraction, designed to handle large-scale, high-concurrency workloads with real-time progress tracking and advanced fault tolerance.

## Features

- **Multi-Stage Processing**: 
  - Validation and Metadata Extraction
  - Dynamic Image Resizing and Watermarking
  - Optimized File Compression and Packaging

- **Event-Driven Workflow**:
  - Triggered by Azure Blob Storage uploads using Azure Functions.
  - Orchestrated using Durable Functions for sequential and parallel task execution.

- **Scalability and Fault Tolerance**:
  - Supports fan-out/fan-in patterns for parallel processing.
  - Implements robust retry policies and error handling mechanisms.

- **Real-Time Updates**:
  - Integrated Azure SignalR for real-time progress notifications.
  - Live status updates for user transparency and engagement.

- **Security and Monitoring**:
  - Secured with Azure Active Directory and Azure Key Vault.
  - Performance monitoring using Azure Monitor and Application Insights.

## Architecture

The pipeline comprises:
1. **Azure Blob Storage**: For file upload triggers and output storage.
2. **Azure Functions**: Event-driven compute for processing tasks.
3. **Durable Functions**: Orchestrating multi-step workflows.
4. **Azure SignalR**: For real-time notifications.
5. **Azure Event Grid**: Handling events and triggering downstream processes.
6. **Azure Monitor**: Logging and performance tracking.
7. **Azure Key Vault**: Securing sensitive information.

## Getting Started

### Prerequisites
- Azure subscription.
- Azure CLI installed locally.
- Access to Azure Blob Storage and Function App resources.

### Setup
1. **Clone the repository**:
   ```bash
   git clone https://github.com/PranavPratapSingh/PhotoEditingPipeline.git
   cd PhotoEditingPipeline
   ```

2. **Deploy the Azure Resources**:
   ```bash
   az deployment group create --resource-group <RESOURCE_GROUP> --template-file template.json
   ```

3. **Configure Azure Functions**:
   - Update the `local.settings.json` with your Azure Storage connection string and SignalR credentials.
   ```json
   {
       "IsEncrypted": false,
       "Values": {
           "AzureWebJobsStorage": "<YOUR_STORAGE_CONNECTION_STRING>",
           "FUNCTIONS_WORKER_RUNTIME": "dotnet",
           "SignalRConnectionString": "<YOUR_SIGNALR_CONNECTION_STRING>"
       }
   }
   ```

4. **Run Locally**:
   ```bash
   func start
   ```

5. **Deploy to Azure**:
   ```bash
   func azure functionapp publish <FUNCTION_APP_NAME>
   ```

### Usage
1. **Upload Files**:
   Upload an image or video to the designated Azure Blob Storage container.

2. **Real-Time Updates**:
   Check processing status via the connected SignalR client.

3. **Retrieve Outputs**:
   Processed files will be stored in the output container of Azure Blob Storage.

## Contributing

Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/your-feature`.
3. Commit your changes: `git commit -m 'Add your feature'`.
4. Push to the branch: `git push origin feature/your-feature`.
5. Open a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

For queries or support, reach out to **Pranav Pratap Singh**:
- Email: [ppsingh@umass.edu](mailto:ppsingh@umass.edu)
- GitHub: [PranavPratapSingh](https://github.com/PranavPratapSingh)
