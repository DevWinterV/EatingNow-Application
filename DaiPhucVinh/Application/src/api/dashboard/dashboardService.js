import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/dashboard";
const API = {
  TotalRevenue_Chart: "/TotalRevenue_Chart",
  TotalPriceQuote: "/TotalPriceQuote",
};

const TotalRevenue_Chart = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TotalRevenue_Chart,
      request,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.data = response.data.Data;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const TotalPriceQuote = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TotalPriceQuote,
      request,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.data = response.data.Data;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};

export { TotalRevenue_Chart, TotalPriceQuote };
