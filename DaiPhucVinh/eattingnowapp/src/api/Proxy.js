import axios from "axios";
import { decrypt } from "../framework/encrypt";

export const Proxy = async (method, api, request) => {
  try {
    if (method.toLowerCase() === "get") {
      let requestMethod = {
        ...request,
      };
      const reponse = await axios.get(api, requestMethod || {});
      return reponse.data;
    } else {
      const reponse = await axios.post(api, request || {});
      return reponse;
    }
  } catch (err) {
    return null;
  }
};
