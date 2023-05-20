import { Proxy } from "../Proxy";
import { BaseResponse } from "../BaseResponse";
import { AppKey, getCache, setCache } from "../../framework/cache";
const API = {
  TakeAllCitys: "/common/TakeAllCitys",
  TakeAllDistricts: "/common/TakeAllDistricts",
  TakeAllWards: "/common/TakeAllWards",
  TakeAllEducationLevel: "/common/TakeAllEducationLevel",
  TakeAllEducationPlace: "/common/TakeAllEducationPlace",
  TakeAllDocumentType: "/common/TakeAllDocumentType",
};

export const TakeAllCities = async () =>
  await Proxy(
    "post",
    API.TakeAllCitys,
    {
      page: 0,
      pageSize: 100,
    },
    false
  );

export const TakeAllDistricts = async (cityId) =>
  await Proxy(
    "post",
    API.TakeAllDistricts,
    {
      page: 0,
      pageSize: 100,
      cityId: cityId,
    },
    false
  );

export const TakeAllWards = async (districtId) =>
  await Proxy(
    "post",
    API.TakeAllWards,
    {
      page: 0,
      pageSize: 100,
      districtId: districtId,
    },
    false
  );

export const TakeAllEducationLevel = async () => await Proxy("post", API.TakeAllEducationLevel);
export const TakeAllEducationPlace = async () => await Proxy("post", API.TakeAllEducationPlace);
export const TakeAllDocumentType = async () => await Proxy("post", API.TakeAllDocumentType);
