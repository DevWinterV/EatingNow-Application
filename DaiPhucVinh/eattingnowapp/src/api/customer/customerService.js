import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/customer";
const API = {
  CheckCustomer: "/CheckCustomer",
  CreateOrderCustomer: "/CreateOrderCustomer",
  UpdateToken: "/UpdateToken",
  UpdateInfoCustomer : "/UpdateInfoCustomer",
  CheckCustomerEmail : "/CheckCustomerEmail"
};

const CheckCustomer = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CheckCustomer,
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
const CheckCustomerEmail = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CheckCustomerEmail,
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


const CreateOrderCustomer = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CreateOrderCustomer,
      request,
      true
    );
    if (response.data.Success) {
      result.message = response.data.Message;
      result.success = true;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const UpdateToken = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.UpdateToken,
      request,
      true
    );
    if (response.data.Success) {
      result.message = response.data.Message;
      result.success = true;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const UpdateInfoCustomer = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.UpdateInfoCustomer,
      request,
      true
    );
    if (response.data.Success) {
      result.message = response.data.Message;
      result.success = true;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};

export { CheckCustomer, CheckCustomerEmail,CreateOrderCustomer, UpdateToken, UpdateInfoCustomer };
