import 'dart:async';
import 'package:fam/data/Api/CustomerService.dart';
import 'package:fam/util/notificationService.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:firebase_messaging/firebase_messaging.dart';

import '../../util/app_constants.dart';

@pragma('vm:entry-point')
Future<void> _firebaseMessagingBackgroundHandler(RemoteMessage message) async {
  print("Nhận thông báo dưới nền");
}

class FirebaseApi {
  CustomerService customerService =
      CustomerService(apiUrl: AppConstants.UpdateToken);

  final _firebasemsg = FirebaseMessaging.instance;

  Future<void> initNotifications() async {
    final fmcToken = await _firebasemsg.getToken();

    // Update Token App to send notification
    if (FirebaseAuth.instance.currentUser?.uid != null) {
      final reponse = await customerService.updateToken({
        "TokenApp": fmcToken,
        "CustomerId": FirebaseAuth.instance.currentUser?.uid,
      });
      if (reponse.success != true) {
        print("Xảy ra lỗi khi cập nhật Token App ... ");
      }
    }

    await _firebasemsg.requestPermission(
      alert: true,
      announcement: true,
      badge: true,
      carPlay: false,
      criticalAlert: false,
      provisional: false,
      sound: true,
    );
    //Nhận thông báo trong khi mở ứng dụng
    FirebaseMessaging.onMessage.listen((RemoteMessage message) {
      String title = message.notification?.title ?? "";
      String body = message.notification?.body ?? "";
      String? payload = message.data['body']; // Accessing the payload as String?
      NotificationService.showNotification(title: title, body: body, payload: payload);
    });

    //Nhận thông báo dưới nền
    FirebaseMessaging.onBackgroundMessage(
        (msg) => _firebaseMessagingBackgroundHandler(msg));

    FirebaseMessaging.onMessageOpenedApp.listen((RemoteMessage message) {
      NotificationService.onclickNotificationBack.add(message.data["body"]);
    });


    //Làm mới token và cập nhật lại CSDL
    _firebasemsg.onTokenRefresh.listen((fcmToken) async {
      if (FirebaseAuth.instance.currentUser?.uid != null) {
        final reponse = await customerService.updateToken({
          "TokenApp": fmcToken,
          "CustomerId": FirebaseAuth.instance.currentUser?.uid,
        });
        if (reponse.success != true) {
          print("Xảy ra lỗi khi cập nhật Token App ... ");
        }
      }
    })
        .onError((err) {
    });
  }


}
