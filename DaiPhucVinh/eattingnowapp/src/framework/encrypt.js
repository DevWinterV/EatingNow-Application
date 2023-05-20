import CryptoJS from "crypto-js";
const premiumKey = "EdwWIKAMBUHw5cIq9WlnH29UcN30eSXy7M";
export function encrypt(data) {
  return CryptoJS.AES.encrypt(JSON.stringify(data), premiumKey).toString();
}
export function decrypt(hashData) {
  const bytes = CryptoJS.AES.decrypt(hashData, premiumKey);
  return JSON.parse(bytes.toString(CryptoJS.enc.Utf8));
}
