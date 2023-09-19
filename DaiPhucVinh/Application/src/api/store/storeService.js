import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/store";
const API = {
  TakeAllStore: "/TakeAllStore",
  TakeAllDeliveryDriver: "/TakeAllDeliveryDriver",
  TakeAllOrder: "/TakeAllOrder",
  CreateNewStore: "/CreateNewStore",
  UpdateNewStore: "/UpdateNewStore",
  DeleteStore: "/DeleteStore",
  TakeStoreByCuisineId: "/TakeStoreByCuisineId",
  TakeFoodListByStoreId: "/TakeFoodListByStoreId",
  ApproveStore: "/ApproveStore",
};

const TakeAllStore = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllStore,
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


const TakeAllDeliveryDriver = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllDeliveryDriver,
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

const CreateNewStore = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CreateNewStore,
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
const DeleteStore = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.DeleteStore,
      {
        UsersId: e,
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
const UpdateNewStore = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.UpdateNewStore,
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
const TakeStoreByCuisineId = async (Id) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeStoreByCuisineId,
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
const TakeFoodListByStoreId = async (Id) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeFoodListByStoreId,
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
const ApproveStore = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.ApproveStore,
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

export {
  TakeAllStore,
  TakeAllDeliveryDriver,
  CreateNewStore,
  UpdateNewStore,
  DeleteStore,
  TakeStoreByCuisineId,
  TakeFoodListByStoreId,
  ApproveStore,
};
