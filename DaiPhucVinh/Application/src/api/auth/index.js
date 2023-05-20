import { ServerEndPoint } from "../ServerEndPoint";
import axios from "axios";
import { encrypt } from "../../framework/encrypt";
const endpoint = {
  login: ServerEndPoint + "/auth/token",
};
export const Login = async (username, password) => {
  const response = await axios.post(endpoint.login, {
    username,
    password,
  });
  if (response.data.AccessToken != null) {
    let responsePermission = await axios.post(
      "/api/role/GetPermissons",
      {},
      { headers: { Authorization: `Bearer ${response.data.AccessToken}` } }
    );
    const dataPermissions = responsePermission.data.Data;
    localStorage.setItem("@permissions", encrypt(dataPermissions));
  }
  return response.data;
};
