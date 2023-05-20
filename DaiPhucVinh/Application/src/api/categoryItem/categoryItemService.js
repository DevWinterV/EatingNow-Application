import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/cuisine";
const API = {
  TakeAllCuisine: "/TakeAllCuisine",
  CreateNewCuisine: "/CreateNewCuisine",
  DeleteCuisine: "/DeleteCuisine",
  UpdateNewCuisine: "/UpdateNewCuisine",
  TakeCuisineById: "/TakeCuisineById",
};

const TakeAllCuisine = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllCuisine,
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
const CreateNewCuisine = async (request) => {
  console.log(request);
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CreateNewCuisine,
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
const DeleteCuisine = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.DeleteCuisine,
      {
        CuisineId: e,
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
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.UpdateNewCuisine,
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

const TakeCuisineById = async (Id) => {
  console.log(Id);
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeCuisineById,
      { params: { Id: Id } },
      true
    );
    if (response.Success) {
      result.success = response.Success;
      result.item = response.Item;
      result.dataCount = response.DataCount;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
export {
  TakeAllCuisine,
  CreateNewCuisine,
  DeleteCuisine,
  UpdateNewCuisine,
  TakeCuisineById,
};
