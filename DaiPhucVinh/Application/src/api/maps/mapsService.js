import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPointDelivery = ServerEndPoint + "/store";

const API = {
  TakeAllDelivery: "/TakeAllDeliveryDriver",
};

const TakeAllDelivery = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPointDelivery + API.TakeAllDelivery,
      request,
      true
    );
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

export {
  TakeAllDelivery,
};
