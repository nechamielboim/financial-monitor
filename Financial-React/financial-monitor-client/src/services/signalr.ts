import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

const connection = new HubConnectionBuilder()
  .withUrl("http://localhost:5177/hubs/transactions")
  .configureLogging(LogLevel.Information)
  .withAutomaticReconnect()
  .build();

export default connection;