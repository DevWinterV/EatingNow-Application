import AsyncStorage from "@react-native-async-storage/async-storage";

export const getCache = async (key) => {
  let data = await AsyncStorage.getItem(key);
  return data ? JSON.parse(data) : null;
};
export const removeCache = async (key) => {
  return await AsyncStorage.removeItem(key);
};
export const setCache = async (key, data) => {
  return await AsyncStorage.setItem(key, JSON.stringify(data));
};
export const clearAllCache = async () => {
  return await AsyncStorage.clear();
};
const licenseKey = "519012d0-b03a-4eff-8926-6a51bd3474ab";
export const AppKey = {
  SERVERENDPOINT: "@SERVERENDPOINT" + licenseKey,
  AUTH: "@AUTH" + licenseKey,
  SETTINGS: "@SETTINGS" + licenseKey,
  REMEMBER: "@REMEMBER" + licenseKey,
  CITY: "@CITY" + licenseKey,
  DISTRICTS: "@DISTRICTS" + licenseKey,
  WARD: "@WARD" + licenseKey,
  CURRENTUSER: "@CURRENTUSER" + licenseKey,
};
