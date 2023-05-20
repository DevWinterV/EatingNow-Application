const CryptoJS = require("crypto-js");
export function encrypt(data, saltKey) {
  return CryptoJS.AES.encrypt(JSON.stringify(data), saltKey).toString();
}
export function decrypt(hashedData, saltKey) {
  const bytes = CryptoJS.AES.decrypt(hashedData, saltKey);
  return JSON.parse(bytes.toString(CryptoJS.enc.Utf8));
}
