// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAuth } from "firebase/auth";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
  apiKey: "AIzaSyCOoS_wgIPkSIExHRzJoij7I2n1QXhsEZ8",
  authDomain: "otp-project-59628.firebaseapp.com",
  projectId: "otp-project-59628",
  storageBucket: "otp-project-59628.appspot.com",
  messagingSenderId: "712142032339",
  appId: "1:712142032339:web:882b41d5cbcc7a5f407326",
  measurementId: "G-P4ZGJSTJY5",
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

export const auth = getAuth(app);
