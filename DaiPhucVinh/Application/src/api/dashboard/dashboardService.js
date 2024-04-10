import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/dashboard";
const API = {
  TotalRevenueStatistics: "/TotalRevenueStatistics",
  TakeProductStatistics: "/TakeProductStatistics"
};

const TotalRevenueStatistics = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TotalRevenueStatistics,
      request,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.item = response.data.Item;
      result.message = response.data.Message;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};

const TakeProductStatistics = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeProductStatistics,
      request,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.item = response.data.Item;
      result.message = response.data.Message;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
export {  TotalRevenueStatistics, TakeProductStatistics };
