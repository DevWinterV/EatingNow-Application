import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/foodlist";
const API = {
  CreateFoodItem: "/CreateFoodItem",
  DeleteFoodList: "/DeleteFoodList",
  UpdateFoodList: "/UpdateFoodList",
  TakeFoodListById: "/TakeFoodListById",
  TakeFoodListByHint: "/TakeFoodListByHint",
  TakeBestSeller: "/TakeBestSeller",
  TakeNewFood: "/TakeNewFood",
};

const CreateFoodItem = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy("post", ServiceEndPoint + API.CreateFoodItem, request, true);
    if (response.data.Success) {
      result.item = response.data.Item;
      result.success = true;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const DeleteFoodList = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.DeleteFoodList,
      {
        FoodListId: e,
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
const UpdateFoodList = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy("post", ServiceEndPoint + API.UpdateFoodList, request, true);
    if (response.data.Success) {
      result.success = true;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const TakeFoodListById = async (FoodListId) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeFoodListById,
      { params: { FoodListId: FoodListId } },
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

const TakeFoodListByHint = async () => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy("post", ServiceEndPoint + API.TakeFoodListByHint, {}, true);
    if (response.data.Success) {
      result.success = response.data.Success;
      result.data = response.data.Data;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};

const TakeBestSeller = async () => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy("post", ServiceEndPoint + API.TakeBestSeller, {}, true);
    if (response.data.Success) {
      result.success = response.data.Success;
      result.data = response.data.Data;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};

const TakeNewFood = async () => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy("post", ServiceEndPoint + API.TakeNewFood, {}, true);
    if (response.data.Success) {
      result.success = response.data.Success;
      result.data = response.data.Data;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};

export {
  CreateFoodItem,
  DeleteFoodList,
  UpdateFoodList,
  TakeFoodListById,
  TakeFoodListByHint,
  TakeBestSeller,
  TakeNewFood,
};
