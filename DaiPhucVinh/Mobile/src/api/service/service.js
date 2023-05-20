import { Proxy } from "../Proxy";
const API = {
  TakeAllsService: "/common/TakeAllsService",
  TakeAllsPost: "/common/TakeAllsPost",
  TakeServiceById: "/common/TakeServiceById",
  TakePostById: "/common/TakePostById",
};

export const TakeAllsService = async (request) =>
  await Proxy("post", API.TakeAllsService, request, false);
export const TakeAllsPost = async (request) =>
  await Proxy("post", API.TakeAllsPost, request, false);
export const TakeServiceById = async () => await Proxy("get", API.TakeServiceById, null);
export const TakePostById = async (request) =>
  await Proxy("post", API.TakePostById, request, false);
