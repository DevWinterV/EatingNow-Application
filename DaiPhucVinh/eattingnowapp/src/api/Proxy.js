import axios from "axios";

export const Proxy = async (method, api, request) => {
  try {
    if (method.toLowerCase() === "get") {
      let requestMethod = {
        ...request,
      };
      const reponse = await axios.get(api, requestMethod || {});
      return reponse.data;
    } else if (method.toLowerCase() === "post") {
      const reponse = await axios.post(api, request || {});
      return reponse;
    }
    else{
      const reponse = await axios.put(api, request || {});
      return reponse;
    }
  } catch (err) {
    return null;
  }
};
