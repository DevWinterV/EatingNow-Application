import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/unitofmeasure";
const API = {
  TakeAllUnitOfMeasure: "/TakeAllUnitOfMeasure",
  TakeUnitOfMeasure: "/TakeUnitOfMeasure",
};

const TakeAllUnitOfMeasure = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllUnitOfMeasure,
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
const TakeUnitOfMeasure = async (Code) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeUnitOfMeasure,
      { params: { Code: Code } },
      true
    );
    if (response.Success) {
      result.success = true;
      result.Item = response.Item;
      result.dataCount = response.data.DataCount;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
export { TakeAllUnitOfMeasure, TakeUnitOfMeasure };
