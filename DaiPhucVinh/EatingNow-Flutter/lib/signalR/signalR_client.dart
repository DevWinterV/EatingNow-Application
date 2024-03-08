import 'package:fam/util/app_constants.dart';
import 'package:signalr_flutter/signalr_flutter.dart';

class SignalRClient {
  final String serverUrl = "${AppConstants.BASE_URL}/signalr/hubs";
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
    await signalR.invokeMethod(hubMethods[2], arguments: ["Thông báo đơn đặt hàng mới", userId]);
  }
  Future<void> setCustomerId(String customerId) async {
    await signalR.invokeMethod(hubMethods[1], arguments: [customerId]);
  }
}
