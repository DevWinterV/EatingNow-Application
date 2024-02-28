import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/customer";
const API = {
  TakeAlls: "/TakeAllCustomer",
  TakeCustomerById: "/TakeCustomerById",
  CreateCustomer: "/CreateCustomer",
  UpdateCustomer: "/UpdateInfoCustomer",
  RemoveCustomer: "/RemoveCustomer",
  SearchCustomer: "/SearchCustomer",
  TakeAllCustomerByProvinceId: "/TakeAllCustomerByProvinceId",
  TakeOrderByCustomer: "/TakeOrderByCustomer",
  RemoveOrderLine: "/RemoveOrderLine",
  RemoveOrderHeader: "/RemoveOrderHeader"
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

const TakeAllCustomer = async (request) => {
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
const TakeAllCustomerByProvinceId = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllCustomerByProvinceId,
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
const RemoveCustomer = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.RemoveCustomer,
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
const RemoveOrderLine = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.RemoveOrderLine,
      e,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.message = response.data.Message;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const RemoveOrderHeader = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.RemoveOrderHeader,
      e,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.message = response.data.Message;
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
  TakeAllCustomer,
  TakeCustomerById,
  CreateCustomer,
  RemoveCustomer,
  UpdateCustomer,
  SearchCustomer,
  TakeAllCustomerByProvinceId,
  TakeOrderByCustomer,
  RemoveOrderLine,
  RemoveOrderHeader
};
