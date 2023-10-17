import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/customer";
const API = {
  CheckCustomer: "/CheckCustomer",
  CheckCustomerAddress: "/CheckCustomerAddress",
  CheckCustomerAddress: "/CheckCustomerAddress",
  CreateOrderCustomer: "/CreateOrderCustomer",
  UpdateToken: "/UpdateToken",
  UpdateInfoCustomer : "/UpdateInfoCustomer",
  CheckCustomerEmail : "/CheckCustomerEmail",
  TakeAllCustomerAddressById: "/TakeAllCustomerAddressById",
  CreateCustomerAddress : "/CreateCustomerAddress",
  DeleteAddress :"/DeleteAddress",
  TakeOrderByCustomer: "/TakeOrderByCustomer"
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
const CheckCustomerAddress = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CheckCustomerAddress,
      request,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.data = response.data.Data;
      result.dataCount = response.data.DataCount;
      result.message = response.data.Message;
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

const TakeAllCustomerAddressById = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllCustomerAddressById,
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

const TakeOrderByCustomer = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeOrderByCustomer,
      request,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.data = response.data.Data;
      result.dataCount = response.data.DataCount;
      result.message = response.data.Message;
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

const CreateCustomerAddress = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CreateCustomerAddress,
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

const DeleteAddress = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.DeleteAddress,
      request,
      true
    );
    if (response.data.Success) {
      result.success = true;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};

export { CheckCustomer, CheckCustomerEmail,CreateOrderCustomer, UpdateToken, UpdateInfoCustomer, CheckCustomerAddress , TakeAllCustomerAddressById, CreateCustomerAddress,DeleteAddress, TakeOrderByCustomer};
