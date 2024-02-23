import UIKit
import Flutter
import flutter_local_notifications
@UIApplicationMain
@objc class AppDelegate: FlutterAppDelegate {
  override func application(
    _ application: UIApplication,
    didFinishLaunchingWithOptions launchOptions: [UIApplication.LaunchOptionsKey: Any]?
  ) -> Bool {
    FlutterLocalNotificationsPlugin.setPluginRegistrantCallback{(registry) in
        GeneratedPluginRegistrant.register(with: registry)
    }
    GeneratedPluginRegistrant.register(with: self)

    if #available(iOS 10.0, *) {
        // Set the delegate for UNUserNotificationCenter
        UNUserNotificationCenter.current().delegate = self
    }

    return super.application(application, didFinishLaunchingWithOptions: launchOptions)
  }
     // Handle notification taps when the app is in the foreground.
      // Implement this method if you want to perform custom actions when a notification is tapped.
      func userNotificationCenter(_ center: UNUserNotificationCenter, didReceive response: UNNotificationResponse, withCompletionHandler completionHandler: @escaping () -> Void) {
          // Handle notification tap here
          print("Received notification response")

          // Perform any custom actions

          // Call the completion handler when done
          completionHandler()
      }
}
