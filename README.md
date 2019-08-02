Demo for autoscaler. This is composed of a message generator and a message consumer.  The generator is generating messages into a queue, with a "busy time" which simulates processing. The consumer is deployed in Kubernetes, consumes messages in a mono-threaded manner, simulates working by pausing the thread for a configured period of time, and gets autoscaled.

# Running the generator

You need to export the following environment variables:

```sh
export MESSAGE_BUS_TYPE="StorageQueue"
export CONNECTION_STRING="" # connection string to the storage account containing the queue where to send the messages.
export QUEUE_NAME="messages"
```

Then run the generator using:

```sh
dotnet run [--busy-time BUSY_TIME_IN_SECONDS] [--interval INTERVAL_IN_MILLISECONDS]
```

`Busy-time` is the time in seconds that the thread will stay paused for.

`interval` is the time in milliseconds that will be waited between sending two messages to the bus.

# Running the consumer

There is a helm template to deploy the consumer in the `chart` directory. You can deploy it by running the following command:

```sh
helm template chart --set connectionString="$CONNECTION_STRING" | kubectl apply -f -
```

Validate everything works by running `kubectl get all` - should produce an output similar to this:

```
NAME                                     READY   STATUS    RESTARTS   AGE
pod/messages-consumer-866594dc4b-lbtr9   1/1     Running   0          10s

NAME                                READY   UP-TO-DATE   AVAILABLE   AGE
deployment.apps/messages-consumer   1/1     1            1           11s

NAME                                           DESIRED   CURRENT   READY   AGE
replicaset.apps/messages-consumer-866594dc4b   1         1         1       11s
```

With the generator running, looking at the pod's log shoud produce the following:

```
kubectl logs messages-consumer-866594dc4b-lbtr9

#1: This is the message. Staying busy 1s
#2: This is the message. Staying busy 1s
#3: This is the message. Staying busy 1s
```