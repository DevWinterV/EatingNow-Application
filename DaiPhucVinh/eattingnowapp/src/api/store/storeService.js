import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/store";
const API = {
  TakeAllStore: "/TakeAllStore",
  CreateNewStore: "/CreateNewStore",
  DeleteStore: "/DeleteStore",
  UpdateNewStore: "/UpdateNewStore",
  TakeStoreByCuisineId: "/TakeStoreByCuisineId",
  TakeCategoryByStoreId: "/TakeCategoryByStoreId",
  TakeFoodListByStoreId: "/TakeFoodListByStoreId",
  TakeAllFoodListByStoreId: "/TakeAllFoodListByStoreId",
  TakeOrderHeaderByStoreId: "/TakeOrderHeaderByStoreId",
  GetListOrderLineDetails: "/GetListOrderLineDetails",
  TakeStatisticalByStoreId: "/TakeStatisticalByStoreId",
  ApproveOrder: "/ApproveOrder",
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
// const TakeStoreByCuisineId = async (Id) => {
//   let result = new BaseResponse(false, "", null);
//   try {
//     let response = await Proxy(
//       "get",
//       ServiceEndPoint + API.TakeStoreByCuisineId,
//       { params: { Id: Id } },
//       true
//     );
//     result.data = response.Data;
//   } catch (e) {
//     result.message = e.toString();
//   }
//   return result;
// };
const TakeStoreByCuisineId = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeStoreByCuisineId,
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
const TakeCategoryByStoreId = async (Id) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeCategoryByStoreId,
      { params: { Id: Id } },
      true
    );
    result.data = response.Data;
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
    result.data = response.Data;
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const TakeAllFoodListByStoreId = async (Id) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeAllFoodListByStoreId,
      { params: { Id: Id } },
      true
    );
    result.data = response.Data;
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const TakeOrderHeaderByStoreId = async (Id) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeOrderHeaderByStoreId,
      { params: { Id: Id } },
      true
    );
    result.data = response.Data;
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const ApproveOrder = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.ApproveOrder,
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
const GetListOrderLineDetails = async (Id) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.GetListOrderLineDetails,
      { params: { Id: Id } },
      true
    );
    result.data = response.Data;
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
// const TakeStatisticalByStoreId = async (request) => {
//   let result = new BaseResponse(false, "", null);
//   try {
//     let response = await Proxy(
//       "post",
//       ServiceEndPoint + API.TakeStatisticalByStoreId,
//       request,
//       true
//     );
//     console.log('response', response);
//     if (response.data.Success) {
//       result.item = response.Item;
//     }
//   } catch (e) {
//     result.message = e.toString();
//   }
//   return result;
// };
const TakeStatisticalByStoreId = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeStatisticalByStoreId,
      request,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.item = response.data.Item;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};

export {
  TakeAllStore,
  CreateNewStore,
  DeleteStore,
  UpdateNewStore,
  TakeStoreByCuisineId,
  TakeCategoryByStoreId,
  TakeFoodListByStoreId,
  TakeAllFoodListByStoreId,
  TakeOrderHeaderByStoreId,
  GetListOrderLineDetails,
  TakeStatisticalByStoreId,
  ApproveOrder,
};
