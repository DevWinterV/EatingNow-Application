import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/userguides";
const API = {
  TakeAllUserGuides: "/TakeAllUserGuides",
  TakeUserGuidesById: "/TakeUserGuidesById",
  CreateUserGuides: "/CreateUserGuides",
  UpdateUserGuides: "/UpdateUserGuides",
  RemoveUserGuides: "/RemoveUserGuides",
};

const TakeAllUserGuides = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllUserGuides,
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
const TakeUserGuidesById = async (Id) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeUserGuidesById,
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

const CreateUserGuides = async (data) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CreateUserGuides,
      data,
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
const UpdateUserGuides = async (data) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.UpdateUserGuides,
      data,
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
const RemoveUserGuides = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.RemoveUserGuides,
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
export {
  TakeAllUserGuides,
  TakeUserGuidesById,
  CreateUserGuides,
  UpdateUserGuides,
  RemoveUserGuides,
};
