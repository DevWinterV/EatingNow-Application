import axios from "axios";
import { AppKey, getCache, setCache } from "../../framework/cache";
const bundleId = "vn.phoenixcompany.homecare247";
const phoenixEndpoint = "http://tech.phoenixcompany.vn/api/tracking/WhoAreYouApplication";
import { ServerEndPoint } from "../ServerEndPoint";
import { Proxy } from "../Proxy";

export async function AppInit(internetConnected) {
  if (internetConnected) {
    let trackingResponse = await axios.post(phoenixEndpoint, {
      packageId: bundleId,
    });
    try {
      if (trackingResponse.status === 200) {
        if (trackingResponse.data?.Success) {
          let data = trackingResponse.data.Data;
          setCache(
            AppKey.SERVERENDPOINT,
            data.UsingDomain ? data.ServerEndPoint : data.ServerEndPointIP
          );
        }
      }
    } catch {
      setCache(AppKey.SERVERENDPOINT, ServerEndPoint);
    }
  } else {
    setCache(AppKey.SERVERENDPOINT, ServerEndPoint);
  }
}

export async function Login(request) {
  try {
    var endpoint = await getCache(AppKey.SERVERENDPOINT);
    var response = await axios.post(endpoint + "/auth/token", request);
    if (response.status == 200) {
      return response.data;
    }
  } catch {
    return null;
  }
}
export async function Signup(request) {
  return await Proxy("post", "/auth/signup", request, false);
}
export async function ChangePassword(request) {
  return await Proxy("post", "/user/changePassword", request);
}
export async function GetCurrentUser() {
  return await Proxy("get", "/user/getInfo", null);
}

export async function UpdateUserProfile(request) {
  return await Proxy("post", "/user/updateUserProfile", request);
}
export async function DeleteAccount() {
  return await Proxy("post", "/user/deleteAccount", null);
}
