import axios from "axios";
import { decrypt } from "../framework/encrypt";

export const Proxy = async (method, api, request, isUseToken = false) => {
  try {
    let authCache = await localStorage.getItem("@auth");
    const auth = decrypt(authCache);
    if (method.toLowerCase() === "get") {
      let requestMethod = {
        ...request,
        headers: { Authorization: `Bearer ${auth.AccessToken}` },
      };
      const reponse = await axios.get(api, requestMethod || {});
      return reponse.data;
    } else {
      const config = {
        headers: { Authorization: `Bearer ${auth.AccessToken}` },
      };
      const reponse = await axios.post(
        api,
        request || {},
        isUseToken ? config : {}
      );
      return reponse;
    }
  } catch {
    return null;
  }
};
