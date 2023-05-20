import { Proxy } from "../Proxy";
import { ServerEndPoint } from "../ServerEndPoint";
import { BaseResponse } from "../BaseResponse";
const ServiceEndPoint = ServerEndPoint + "/product";
const API = {
  TakeAlls: "/TakeAlls",
  TakeAllItemGroup: "/TakeAllItemGroup",
  TakeAllImageByItemCode: "/TakeAllImageByItemCode",
  UploadImages: "/UploadImages",
  DeleteImages: "/DeleteImages",
  ImagesSync: "/ImagesSync",
  CheckMainImage: "/CheckMainImage",
  TakeProductById: "/TakeProductById",
  TakeItemCategory: "/TakeItemCategory",
  UpdateItem: "/UpdateItem",
  HidenImage: "/HidenImage",
};

const TakeAllsProduct = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAlls,
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
const TakeAllItemGroup = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllItemGroup,
      request,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.data = response.data.Data;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const TakeAllImageByItemCode = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.TakeAllImageByItemCode,
      {
        ItemCode: request.Code,
      },
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.data = response.data.Data;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const UploadImages = async (request, itemCode) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.UploadImages + "?ItemCode=" + itemCode,
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
const TakeItemCategory = async (Code) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeItemCategory,
      { params: { Code: Code } },
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
const TakeProductById = async (Code) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "get",
      ServiceEndPoint + API.TakeProductById,
      { params: { Code: Code } },
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
const DeleteImages = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.DeleteImages,
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
const CheckMainImage = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.CheckMainImage,
      e,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.data = response.data.Item;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const HidenImage = async (e) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.HidenImage,
      e,
      true
    );
    if (response.data.Success) {
      result.success = true;
      result.data = response.data.Item;
    }
  } catch (e) {
    result.message = e.toString();
  }
  return result;
};
const UpdateItem = async (request) => {
  let result = new BaseResponse(false, "", null);
  try {
    let response = await Proxy(
      "post",
      ServiceEndPoint + API.UpdateItem,
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
export {
  HidenImage,
  TakeAllsProduct,
  TakeAllItemGroup,
  TakeAllImageByItemCode,
  TakeItemCategory,
  UploadImages,
  DeleteImages,
  CheckMainImage,
  TakeProductById,
  UpdateItem,
};
