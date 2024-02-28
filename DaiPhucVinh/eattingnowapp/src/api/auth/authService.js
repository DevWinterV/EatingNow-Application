import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/auth";
const API = {
  LoginInFront: "/LoginInFront",
  CheckStatusAccout: "/CheckStatusAccout",
};

const LoginInFront = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.LoginInFront,
      request,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.data = response.data.Data;
      result.dataCount = response.data.DataCount;
    }
    else{
      result.success = false;
      result.message = response.data.Message;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};


const CheckStatusAccout = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CheckStatusAccout,
      request,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.data = response.data.Data;
      result.dataCount = response.data.DataCount;
    }
    else{
      result.success = false;
      result.message = response.data.Message;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
export { LoginInFront, CheckStatusAccout };
