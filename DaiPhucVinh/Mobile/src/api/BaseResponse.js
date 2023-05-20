class BaseResponse {
  constructor(success, message, data, customData) {
    this.success = success;
    this.message = message;
    this.data = data;
    this.customData = customData;
  }
}
export { BaseResponse };
