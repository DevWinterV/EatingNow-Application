import axios from "axios";
const ServiceEndPoint = "https://fcm.googleapis.com/fcm/send";
const ServerKey = "AAAA1Nb0sbs:APA91bF_jfSd__o995VjsoOUGqY5lBS626YpkId4wjSpKI4q0AssfH7RNXLnmSoRIBW8BwNTLFMyq3EO1OGU0hVE_iib7-U6b4GG-bwulhGayq6MuyzBc018HF1m6i9fXZzTOIL-EyDj";

const SendNotification = async (message) => {
  if(message.to != "")
  {
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
      return response.data;
    } catch (error) {
      console.error("Error sending notification:", error.message);
    }
  }
};


export {
    SendNotification,
  };