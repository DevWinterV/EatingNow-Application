import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/province";
const API = {
  TakeAllProvince: "/TakeAllProvince",
  CreateNewProvince: "/CreateNewProvince",
  UpdateNewProvince: "/UpdateNewProvince",
  DeleteProvince: "/DeleteProvince",
  TakeProvinceById: "/TakeProvinceById",
};

const TakeAllProvince = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllProvince,
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

const CreateNewProvince = async (request) => {
  let result = new BaseResponse(false, "", null);
  console.log("haha");
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CreateNewProvince,
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
const DeleteProvince = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.DeleteProvince,
      {
        ProvinceId: e,
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
const UpdateNewProvince = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.UpdateNewProvince,
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
const TakeProvinceById = async (ProvinceId) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeProvinceById,
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

export {
  TakeAllProvince,
  CreateNewProvince,
  UpdateNewProvince,
  DeleteProvince,
  TakeProvinceById,
};
