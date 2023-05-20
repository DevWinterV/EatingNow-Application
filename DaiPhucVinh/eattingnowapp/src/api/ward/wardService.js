import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/ward";
const API = {
  TakeAllWard: "/TakeAllWard",
  CreateNewWard: "/CreateNewWard",
  UpdateNewWard: "/UpdateNewWard",
  DeleteWard: "/DeleteWard",
  TakeWardById: "/TakeWardById",
};

const TakeAllWard = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllWard,
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

const CreateNewWard = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CreateNewWard,
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
const DeleteWard = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.DeleteWard,
      {
        WardId: e,
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
const UpdateNewWard = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.UpdateNewWard,
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
const TakeWardById = async (ProvinceId) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeWardById,
      { params: { Id: ProvinceId } },
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

export { TakeAllWard, CreateNewWard, UpdateNewWard, DeleteWard, TakeWardById };
