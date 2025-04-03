# Azure Functions Blob Processing and CSV EDA Project

## Overview

This project demonstrates an Azure Function App that processes blob files from an Azure Storage Account. The function performs the following tasks:
- **Blob Trigger:** Automatically activates when a file is uploaded to the `input` container.
- **Data Processing:** Reads the blob content, performs basic CSV Exploratory Data Analysis (EDA) and cleaning (if applicable), and appends `" hello"` to the content.
- **Output Binding:** Writes the processed content to the `output` container.
- **Logging and Monitoring:** Integrated with Application Insights and Azure Monitor to capture logs and telemetry for real-time monitoring and troubleshooting.

## Features

- **Automatic Blob Processing:** Trigger function execution upon blob upload.
- **CSV EDA and Cleaning:** Splits CSV content, trims fields, removes empty rows, and logs missing value counts.
- **Output to Blob Container:** Correctly writes modified content to an output container by specifying write access.
- **Robust Logging:** Utilizes the ILogger interface for detailed logging, which is forwarded to Application Insights.
- **Deployment Flexibility:** Deployable via Visual Studio Code 2022 using the Azure Functions extension.

## Prerequisites

- **Azure Subscription:** Ensure you have an active subscription and access to allowed regions (e.g., `italynorth`, `francecentral`, `switzerlandnorth`, `uaenorth`, `polandcentral`).
- **Azure Storage Account:** Set up with `input` and `output` blob containers.
- **Azure Functions Core Tools:** For local testing and command-line deployment.
- **Visual Studio Code 2022:** With the [Azure Functions extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions) installed.
- **.NET Runtime:** Ensure the appropriate .NET runtime version is installed.

## Getting Started

### Local Setup

1. **Clone the Repository:**  
   ```bash
   git clone https://github.com/yourusername/yourrepository.git
   cd yourrepository
