import 'package:flutter_local_notifications/flutter_local_notifications.dart';

class NotificationService {
  static final FlutterLocalNotificationsPlugin flutterLocalNotificationsPlugin =
  FlutterLocalNotificationsPlugin();

  static Future<void> initialize() async {
    AndroidInitializationSettings initializationSettings =
    const AndroidInitializationSettings('flutter_logo');
    var initializationSettingIOS = DarwinInitializationSettings(
      requestAlertPermission: true,
      requestBadgePermission: true,
      requestSoundPermission: true,
    );
    var initialalizationSettings = InitializationSettings(
      android: initializationSettings,
      iOS: initializationSettingIOS,
    );
    await flutterLocalNotificationsPlugin.initialize(initialalizationSettings, onDidReceiveNotificationResponse: (NotificationResponse response) async {});
  }

  // Create notification details once
  static final NotificationDetails _notificationDetails = const NotificationDetails(
    android: AndroidNotificationDetails(
      'channelId',
      'channelName',
      importance: Importance.max,
    ),
    iOS: DarwinNotificationDetails(),
  );

  Future<void> showNotification({
    required int id,
    required String? title,
    required String? body,
    String? payload,
  }) async {
    await flutterLocalNotificationsPlugin.show(
      id,
      title,
      body,
      _notificationDetails,
      payload: payload,
    );
  }
}
