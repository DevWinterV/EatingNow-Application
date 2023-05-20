import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/accounttype";
const API = {
  TakeAllAccountType: "/TakeAllAccountType",
  CreateNewAccountType: "/CreateNewAccountType",
  UpdateNewAccountType: "/UpdateNewAccountType",
  DeleteAccountType: "/DeleteAccountType",
  TakeAccountTypeById: "/TakeAccountTypeById",
};

const TakeAllAccountType = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllAccountType,
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

const CreateNewAccountType = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CreateNewAccountType,
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
const DeleteAccountType = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.DeleteAccountType,
      {
        Id: e,
      },
      true
    );
    if (response.data.Success) {
      result.success = true;
    }
    console.log(response);
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const UpdateNewAccountType = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.UpdateNewAccountType,
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
const TakeAccountTypeById = async (Id) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeAccountTypeById,
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

export {
  TakeAllAccountType,
  CreateNewAccountType,
  UpdateNewAccountType,
  DeleteAccountType,
  TakeAccountTypeById,
};
