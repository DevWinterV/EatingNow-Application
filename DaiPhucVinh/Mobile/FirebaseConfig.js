import firebase from "@react-native-firebase/app";

const firebaseConfig = {
  apiKey: "AIzaSyCOoS_wgIPkSIExHRzJoij7I2n1QXhsEZ8",
  authDomain: "otp-project-59628.firebaseapp.com",
  projectId: "otp-project-59628",
  storageBucket: "otp-project-59628.appspot.com",
  messagingSenderId: "712142032339",
  appId: "1:712142032339:web:882b41d5cbcc7a5f407326",
  measurementId: "G-P4ZGJSTJY5",
};

if (!firebase.apps.length) {
  firebase.initializeApp(firebaseConfig);
}

export default firebase;
