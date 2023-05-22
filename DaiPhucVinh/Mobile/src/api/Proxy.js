import axios from "axios";
import { BaseResponse } from "./BaseResponse";
import { AppKey, getCache } from "../framework/cache";

// export const Proxy = async (method, api, request, isUseToken = true) => {
//   let result = new BaseResponse(false, "", null, null);
//   try {
//     let auth = await getCache(AppKey.AUTH);
//     let SERVERENDPOINT = await getCache(AppKey.SERVERENDPOINT);
//     if (method.toLowerCase() === "get") {
//       let requestMethod = {
//         ...request,
//         headers: { Authorization: `Bearer ${auth.AccessToken}` },
//       };
//       const response = await axios.get(SERVERENDPOINT + api, requestMethod || {});
//       result.success = response?.data?.Success;
//       result.message = response?.data?.Message;
//       result.data = response?.data?.Data;
//     } else {
//       const config = {
//         headers: { Authorization: `Bearer ${auth?.AccessToken}` },
//       };
//       const response = await axios.post(
//         SERVERENDPOINT + api,
//         request || {},
//         isUseToken ? config : {}
//       );
//       result.success = response?.data?.Success;
//       result.message = response?.data?.Message;
//       result.data = response?.data?.Data;
//     }
//   } catch (err) {
//     result.message = err;
//   }
//   return result;
// };

export const Proxy = async (method, api, request) => {
  console.log(method, api, request);
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
    console.log(err);
    return null;
  }
};

export const ProxyWithFiles = async (api, request) => {
  let result = new BaseResponse(false, "", null, null);
  try {
    let auth = await getCache(AppKey.AUTH);
    let SERVERENDPOINT = await getCache(AppKey.SERVERENDPOINT);
    const config = {
      headers: {
        Authorization: `Bearer ${auth.AccessToken}`,
        Accept: "application/json",
        "Content-Type": "multipart/form-data",
      },
    };
    const response = await axios.post(SERVERENDPOINT + api, request, config);
    result.success = response?.data?.Success;
    result.message = response?.data?.Message;
    result.data = response.data;
  } catch (err) {
    result.message = err;
  }
  return result;
};
