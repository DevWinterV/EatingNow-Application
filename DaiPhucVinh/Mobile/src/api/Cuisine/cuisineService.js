import { Proxy } from "../Proxy";
import { BaseResponse } from "../BaseResponse";
import { ServerEndPoint } from "../ServerEndPoint.js";
const ServiceEndPoint = ServerEndPoint + "/cuisine";
const API = {
  TakeAllCuisine: "/TakeAllCuisine",
  CreateNewCuisine: "/CreateNewCuisine",
  DeleteCuisine: "/DeleteCuisine",
  UpdateNewCuisine: "/UpdateNewCuisine",
  TakeStoreTypeById: "/TakeStoreTypeById",
};

const TakeAllCuisine = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy("post", ServiceEndPoint + API.TakeAllCuisine, request, true);
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
const CreateNewCuisine = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy("post", ServiceEndPoint + API.CreateNewCuisine, request, true);
    if (response.data.Success) {
      result.item = response.data.Item;
      result.success = true;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const DeleteCuisine = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.DeleteCuisine,
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
const UpdateNewCuisine = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy("post", ServiceEndPoint + API.UpdateNewCuisine, request, true);
    if (response.data.Success) {
      result.success = true;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const TakeStoreTypeById = async (Id) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeStoreTypeById,
      { params: { Id: Id } },
      true
    );
    result.Item = response;
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
export { TakeAllCuisine, CreateNewCuisine, DeleteCuisine, UpdateNewCuisine, TakeStoreTypeById };
