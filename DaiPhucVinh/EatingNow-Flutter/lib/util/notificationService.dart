import 'package:flutter_local_notifications/flutter_local_notifications.dart';
import 'package:rxdart/rxdart.dart';

class NotificationService{
  static final FlutterLocalNotificationsPlugin flutterLocalNotificationsPlugin =  FlutterLocalNotificationsPlugin();
  static final onclickNotification = BehaviorSubject<String>.seeded("null");
  static final onclickNotificationBack = BehaviorSubject<String>.seeded("null");

  static void onTapNotification (NotificationResponse response){
    onclickNotification.add(response.payload!);
  }

  // Init
  static Future init() async{
    // initialise the plugin. app_icon needs to be a added as a drawable resource to the Android head project
    const AndroidInitializationSettings initializationSettingsAndroid =
    AndroidInitializationSettings('@mipmap/ic_launcher');
    final DarwinInitializationSettings initializationSettingsDarwin =
    DarwinInitializationSettings(onDidReceiveLocalNotification: (id, title, body, payload) => null);
    final LinuxInitializationSettings initializationSettingsLinux =
    LinuxInitializationSettings(defaultActionName: 'Xem thông báo');

    final InitializationSettings initializationSettings = InitializationSettings(
        android: initializationSettingsAndroid,
        iOS: initializationSettingsDarwin,
        linux: initializationSettingsLinux);
    await flutterLocalNotificationsPlugin.initialize(
        initializationSettings,
        onDidReceiveNotificationResponse: onTapNotification,
    );
  }

  //Show notification
  static Future showNotification(
  {
    required String title,
    required String body,
    String? payload
  }) async{
    const AndroidNotificationDetails androidNotificationDetails =
    AndroidNotificationDetails(
        'your channel id',
        'your channel name',
        icon: "@mipmap/ic_launcher",
        channelDescription: 'Thông báo',
        importance: Importance.max,
        priority: Priority.high,
        ticker: 'ticker');
    const NotificationDetails notificationDetails =
    NotificationDetails(
        android: androidNotificationDetails,
        iOS:  DarwinNotificationDetails()
    );
    await flutterLocalNotificationsPlugin.show(
        0,
        title,
        body,
        notificationDetails,
        payload: payload
    );
  }
}