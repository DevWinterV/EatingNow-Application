// Scripts for firebase and firebase messaging
importScripts("https://www.gstatic.com/firebasejs/8.2.0/firebase-app.js");
importScripts("https://www.gstatic.com/firebasejs/8.2.0/firebase-messaging.js");

// Initialize the Firebase app in the service worker by passing the generated config
const firebaseConfig = {
  apiKey: "AIzaSyBerAmPafM3WcAH_lTtM3af7u5xDWSjvEI",
  authDomain: "eating-now.firebaseapp.com",
  projectId: "eating-now",
  storageBucket: "eating-now.appspot.com",
  messagingSenderId: "334588390943",
  appId: "1:334588390943:web:4404c6edf638bae7f6ef2a",
  measurementId: "G-0TQ4V1JGW7"
};

firebase.initializeApp(firebaseConfig);

// Retrieve firebase messaging

const messaging = firebase.messaging();

// Đăng ký sự kiện notificationclick ở cấp độ cao hơn
self.addEventListener('notificationclick', function(event) {
  const payload = event.notification.data;
  event.waitUntil(
    clients.openWindow(payload.action_link)
  );
});

// Register a listener for background messages
messaging.onBackgroundMessage(function(payload) {
  const notificationTitle = payload.notification.title;
  const notificationOptions = {
    body: payload.notification.body,
    actions: [
      {
        action: 'open_link',
        title: 'Đi đên liên kết'
      }
    ],
    data: {
      action_link: payload.data.action_link
    }
  };
  self.registration.showNotification(notificationTitle, notificationOptions);
});


  
  