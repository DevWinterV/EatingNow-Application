// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAnalytics } from "firebase/analytics";
import { getFirestore } from "firebase/firestore";
import { getDatabase } from "firebase/database";
import { getAuth,  GoogleAuthProvider } from "firebase/auth";
import { getMessaging, getToken ,onMessage} from "firebase/messaging";
import  Notification from "react-push-notification";
import { TbRulerMeasure } from "react-icons/tb";

// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
  apiKey: "AIzaSyBerAmPafM3WcAH_lTtM3af7u5xDWSjvEI",
  authDomain: "eating-now.firebaseapp.com",
  projectId: "eating-now",
  storageBucket: "eating-now.appspot.com",
  messagingSenderId: "334588390943",
  appId: "1:334588390943:web:4404c6edf638bae7f6ef2a",
  measurementId: "G-0TQ4V1JGW7"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);
const firestore = getFirestore(app);
// Initialize Firebase Cloud Messaging and get a reference to the service
export const auth = getAuth(app);
auth.languageCode = 'it';
export const provider = new GoogleAuthProvider();
provider.addScope('https://www.googleapis.com/auth/contacts.readonly');

// Get registration token. Initially this makes a network call, once retrieved
// subsequent calls to getToken will return from cache.


// Get registration token. Initially this makes a network call, once retrieved
// subsequent calls to getToken will return from cache.
export const messaging = getMessaging();
onMessage(messaging, (payload) => {
  console.log('Message received. ', payload);
  //Customize notification form payload
  Notification(
    {
      title: payload.data.title,
      message: payload.data.body,
      icon: payload.data.icon,
      native: true,
      duration: 4000,
      onClick: () => window.location = payload.data.action_link
    }
  )
});
// Initialize Realtime Database and get a reference to the service
export const  realtime_database = getDatabase(app);

