import { Proxy } from "../Proxy";
const API = {
  TakeAllBranch: "/branch/TakeAllBranch",
};

export const TakeAllBranchs = async (request) => await Proxy("post", API.TakeAllBranch, request);
