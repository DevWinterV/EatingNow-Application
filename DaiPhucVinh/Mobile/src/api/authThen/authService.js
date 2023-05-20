import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/auth";
const API = {
  LoginInFront: "/LoginInFront",
};

const LoginInFront = async (request) => {
  console.log("request", request);
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy("post", ServiceEndPoint + API.LoginInFront, request, true);
    console.log("response", response);
    if (response.data.Success) {
      result.success = true;
      result.data = response.data.Data;
      result.dataCount = response.data.DataCount;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
export { LoginInFront };
