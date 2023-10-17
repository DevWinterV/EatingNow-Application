import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/store";
const API = {
  TakeAllOrder: "/TakeAllOrder",
  ApproveOrder: "/ApproveOrder",
  GetListOrderLineDetails: "/GetListOrderLineDetails",
};

const TakeAllOrder = async (request) => {
    let result = new BaseResponse(false, "", null);
    try {
      let response = await Proxy(
        "post",
        ServiceEndPoint + API.TakeAllOrder,
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
  const ApproveOrder = async (request) => {
    let result = new BaseResponse(false, "", null);
    try {
      let response = await Proxy(
        "post",
        ServiceEndPoint + API.ApproveOrder,
        request,
        true
      );
      if (response.data.Success) {
        result.item = response.data.Item;
        result.success = true;
      }
    } catch (e) {
      result.message = e.toString();
    }
    return result;
  };
  const GetListOrderLineDetails = async (Id) => {
    let result = new BaseResponse(false, "", null);
    try {
      let response = await Proxy(
        "get",
        ServiceEndPoint + API.GetListOrderLineDetails,
        { params: { Id: Id } },
        true
      );
      result.data = response.Data;
    } catch (e) {
      result.message = e.toString();
    }
    return result;
  };

export {
  TakeAllOrder,
  ApproveOrder,
  GetListOrderLineDetails,
};
