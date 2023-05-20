import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/customer";
const API = {
  TakeAlls: "/TakeAlls",
  TakeAllCustomerType: "/TakeAllCustomerType",
  TakeCustomerById: "/TakeCustomerById",
  TakeContractByCustomerCode: "/TakeContractByCustomerCode",
  TakeContractByContractLineCode: "/TakeContractByContractLineCode",
  CreateCustomer: "/CreateCustomer",
  UpdateCustomer: "/UpdateCustomer",
  RemoveCustomer: "/RemoveCustomer",
  SearchCustomer: "/SearchCustomer",
};

const TakeAllsCustomer = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAlls,
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
const TakeAllCustomerType = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllCustomerType,
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

const TakeCustomerById = async (Id) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeCustomerById,
      { params: { Id: Id } },
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
const TakeContractByCustomerCode = async (request, code) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeContractByCustomerCode + "?code=" + code,
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
const TakeContractByContractLineCode = async (request, code) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeContractByContractLineCode + "?code=" + code,
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
const CreateCustomer = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CreateCustomer,
      request,
      true
    );
    if (response.data.Success) {
      result.success = true;
    } else {
      result.success = false;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const UpdateCustomer = async (formModalDetail) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.UpdateCustomer,
      formModalDetail,
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
const RemoveCustomer = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.RemoveCustomer,
      {
        Id: e,
      },
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
const SearchCustomer = async (request, CustomerCode) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.SearchCustomer + "?CustomerCode=" + CustomerCode,
      request,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.data = response.data.Item;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
export {
  TakeAllsCustomer,
  TakeAllCustomerType,
  TakeCustomerById,
  TakeContractByCustomerCode,
  TakeContractByContractLineCode,
  CreateCustomer,
  RemoveCustomer,
  UpdateCustomer,
  SearchCustomer,
};
