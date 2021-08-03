# 369 Serverless OpenHack

This repo contains all challenges for the serverless openhack done by MTTs.

# Run locally

Make sure you have Azure function tools installed, then move into the project directory and run:

```bash
func start
```

# Deploy manually to Azure

If you wanted to deploy to Azure manually, use Azure CLI to login to Azure and then run:

```bash
func azure functoionapp publish <FunctionAppName>
```

> 💡 Replace the `<FunctionAppName>` with the name of your function app in the above command.