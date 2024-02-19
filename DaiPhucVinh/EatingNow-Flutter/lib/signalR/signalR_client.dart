import 'package:signalr_flutter/signalr_flutter.dart';

class SignalRClient {
  final String serverUrl = "http://localhost:3002/signalr/hubs";
  final String hubName = "OrderNotificationHub";
  final List<String> hubMethods = ["SendOrderNotification", "SetCustomerId", "SendOrderNotificationToUser", "RemoveUserConnection"];

  late SignalR signalR;

  SignalRClient() {
    if(signalR == null){
      signalR = SignalR(serverUrl, hubName, hubMethods: hubMethods);
    }
  }

  void connectToServer() {
    signalR.connect();
  }

  void SendOrderNotificationToUser(String userId) {
    signalR.invokeMethod(hubMethods[2], arguments: ["Thông báo mới", userId]);
  }
  void setCustomerId(String customerId) {
    signalR.invokeMethod(hubMethods[1], arguments: [customerId]);
  }
}
