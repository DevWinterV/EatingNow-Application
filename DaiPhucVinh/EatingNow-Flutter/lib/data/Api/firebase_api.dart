import 'package:firebase_messaging/firebase_messaging.dart';

@pragma('vm:entry-point')
Future<void> _firebaseMessagingBackgroundHandler(RemoteMessage message) async {
  print("Handling a background message: ${message.messageId}");
}

class FirebaseApi {
  final _firebasemsg = FirebaseMessaging.instance;
  Future<void> initNotifications () async {
    final fmcToken = await _firebasemsg.getToken();
    print('Token ${fmcToken}');
    NotificationSettings settings = await _firebasemsg.requestPermission(
      alert: true,
      announcement: true,
      badge: true,
      carPlay: false,
      criticalAlert: false,
      provisional: false,
      sound: true,
    );
    print('User granted permission: ${settings.authorizationStatus}');
    FirebaseMessaging.onMessage.listen((RemoteMessage message) {
      print('Got a message whilst in the foreground!');
      print('Message data: ${message.data}');
      if (message.notification != null) {
        print('Message also contained a notification: ${message.notification}');
        print('Message title: ${message.notification?.title}');
        print('Message body: ${message.notification?.body}');
      }
    });
    FirebaseMessaging.onBackgroundMessage((msg) => _firebaseMessagingBackgroundHandler(msg));
  }
}