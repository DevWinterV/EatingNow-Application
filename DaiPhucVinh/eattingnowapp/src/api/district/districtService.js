import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/district";
const API = {
  TakeAllDistrict: "/TakeAllDistrict",
  CreateNewDistrict: "/CreateNewDistrict",
  UpdateNewDistrict: "/UpdateNewDistrict",
  DeleteDistrict: "/DeleteDistrict",
  TakeDistrictById: "/TakeDistrictById",
};

const TakeAllDistrict = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllDistrict,
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

const CreateNewDistrict = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CreateNewDistrict,
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
const DeleteDistrict = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.DeleteDistrict,
      {
        DistrictId: e,
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
const UpdateNewDistrict = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.UpdateNewDistrict,
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
const TakeDistrictById = async (ProvinceId) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeDistrictById,
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
  TakeAllDistrict,
  CreateNewDistrict,
  UpdateNewDistrict,
  DeleteDistrict,
  TakeDistrictById,
};
