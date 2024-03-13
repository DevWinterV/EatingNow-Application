import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/foodlist";
const API = {
  CreateFoodItem: "/CreateFoodItem",
  DeleteFoodList: "/DeleteFoodList",
  UpdateFoodList: "/UpdateFoodList",
  TakeFoodListById: "/TakeFoodListById",
  ChangeIsNoiBatFoodList: "/ChangeIsNoiBatFoodList",
  ChangeIsNewFoodList: "/ChangeIsNewFoodList",
  TakeRecommendedFoodList: "/TakeRecommendedFoodList",
  SearchFoodListByUser: "/SearchFoodListByUser"
};

const TakeRecommendedFoodList = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeRecommendedFoodList,
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
const CreateFoodItem = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CreateFoodItem,
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
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.UpdateFoodList,
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

const SearchFoodListByUser = async (keyword, latitude, longitude, cuisineId) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.SearchFoodListByUser,
      { params: { keyword: keyword, latitude: latitude, longitude: longitude, cuisineId: cuisineId } },
      true
    );
    if (response.Success) {
      result.data = response.Data
      result.success = response.Success;
      result.item = response.Item;
      result.dataCount = response.DataCount;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};

const ChangeIsNoiBatFoodList = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.ChangeIsNoiBatFoodList,
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

const ChangeIsNewFoodList = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.ChangeIsNewFoodList,
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
export { CreateFoodItem, DeleteFoodList, UpdateFoodList, TakeFoodListById, ChangeIsNoiBatFoodList, ChangeIsNewFoodList,TakeRecommendedFoodList, SearchFoodListByUser };
