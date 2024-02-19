import 'package:signalr_flutter/signalr_flutter.dart';

class SignalRClient {
  final String serverUrl = "https://ef7e-2402-800-63a4-c5fe-682f-cfdb-b515-4824.ngrok-free.app/signalr/hubs";
  final String hubName = "OrderNotificationHub";
  final List<String> hubMethods = ["SendOrderNotification", "SetCustomerId", "SendOrderNotificationToUser", "RemoveUserConnection"];

  late SignalR signalR;

  SignalRClient() {
      signalR = SignalR(serverUrl, hubName, hubMethods: hubMethods);
  }

  Future<void> connectToServer() async {
    await signalR.connect();
  }

  Future<void> SendOrderNotificationToUser(String userId) async {
    await signalR.invokeMethod(hubMethods[2], arguments: ["Thông báo đơn hàng mới", userId]);
  }
  Future<void> setCustomerId(String customerId) async {
    await signalR.invokeMethod(hubMethods[1], arguments: [customerId]);
  }
}
