import axios from "axios";
const ServiceEndPoint = "https://fcm.googleapis.com/fcm/send";
const ServerKey = "AAAATecFrh8:APA91bEap0oFWBC1O1xcIKKtRQTz_sMMs0P9buwQl9bX6nDbmamrFyR7HxuViEvWMpBW16c5nDiMLoTHaw22RYP5IJNd1t90bs4Q0EkYaRsbDqeAAEgTaLmPh5O57uS0DV-bVrKSiuii";

const SendNotification = async (message) => {
  const headers = {
    Authorization: `key=${ServerKey}`,
    "Content-Type": "application/json",
  };

  try {
    const response = await axios.post(
      ServiceEndPoint, 
      message, {
      headers: headers,
    });
    if (response.data.success) {
      console.log("Notification sent successfully!");
    } else {
      console.error("Error sending notification:", response.data.message);
    }
  } catch (error) {
    console.error("Error sending notification:", error.message);
  }
};


export {
    SendNotification,
  };