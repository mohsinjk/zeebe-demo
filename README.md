# Overview

This repository contains instructions on how to install and run Zeebe with a set of demo workers.

For a more in-depth understanding of Zeebe, please consult the [product documentation](https://docs.camunda.io/docs/components/zeebe/zeebe-overview/).

# Getting started

## Prerequisites

1. [Git](https://git-scm.com/downloads)
1. [Docker for Desktop](https://www.docker.com/products/docker-desktop)
1. [.NET Core SDK](https://dotnet.microsoft.com/download)

## Usage

Clone this repository to your local machine:

## Start Zeebe

The `docker-compose.yml` file in the `zeebe` folder can be used to start a single Zeebe brokers; optionally with Simple Monitor, and/or with Operate, along with the Elasticsearch and Kibana containers.

Start the containers in the background:

```
# cd to zeebe
> docker-compose up
```

The containers expose the following services:

- Zeebe broker - port 26500
- Operate - web interface http://localhost:8080 (login: demo/demo)
- ElasticSearch - port http://localhost:9200
- Kibana - port http://localhost:5601
- Simple Monitor - web interface http://localhost:8082

Install Zeebe cli
```
npm i -g zbctl
```

Check that your Zeebe is up and running:

```
zbctl --insecure status
```

This is what you should expect as a result when running a single node:

```
Cluster size: 1
Partitions count: 1
Replication factor: 1
Gateway version: 0.23.1
Brokers:
  Broker 0 - 192.168.144.3:26501
    Version: 0.23.1
    Partition 1 : Leader
```

If it's not yet started, this is what to expect:

```
Error: rpc error: code = Unavailable desc = connection closed
```

For more information on using Zeebe and Operate, consult the Quickstart Guide in the [Zeebe docs](https://docs.camunda.io/docs/components/zeebe/zeebe-overview/).

## Deploy a workflow

```
# cd to zeebe folder
> zbctl --insecure deploy ..\resources\create-privateloan-process.bpmn
```

This is what you should expect as a result:

```
{
  "key": 2251799813685250,
  "workflows": [
    {
      "bpmnProcessId": "create-privateloan-process",
      "version": 1,
      "workflowKey": 2251799813685249,
      "resourceName": "../zeebe-demo/resources/create-privateloan-process.bpmn"
    }
  ]
}
```

## Run a workflow

The `dotnet run 1 35000` command will create an instance where 1, 35000 is used as the customer id and amount, customer id is also used as correlation key of the instance.

Run this command:

```
# cd to client1 folder
> dotnet run 1 35000
```

## Operate a workflow

1. Open the web interface http://localhost:8080 (login: demo/demo)
2. Click on running instances tab and then click on the running instance from the list.

![bild](/img/Operate.png)

## Run the workers

Start the workers one by one and monitor the workflow progress in Operate.

Worker 1 - Create application:

```
# cd to worker1 folder
> dotnet run normal
```

Worker 2 - Create decision:

```
# cd to worker2 folder
> dotnet run normal
```

Worker 3:

```
# cd to worker3 folder
> dotnet run normal
```

Worker 4:

```
# cd to worker4 folder
> dotnet run normal
```

Worker 5:

```
# cd to worker5 folder
> dotnet run normal
```

Worker 6:

```
# cd to worker6 folder
> dotnet run normal
```
Worker 7:

```
# cd to worker7 folder
> dotnet run normal
```
Worker 8:

```
# cd to worker8 folder
> dotnet run normal
```

## Recieve message with correlation key
Background Service:

```
# cd to system1 folder
> dotnet run <correlation-key>
```

API:

```
# cd to api1 folder
> dotnet run
http post: http://localhost:5000/zeebe
request: { "correlationKey": <correlation-key> }
```
```
curl -X POST -H "Content-Type: application/json" \
    -d '{"correlationKey": "p-123" }' \
    http://localhost:5000/zeebe
```

## Stop Zeebe

To stop the containers and clean the persistent data:

```
> docker-compose down
```

