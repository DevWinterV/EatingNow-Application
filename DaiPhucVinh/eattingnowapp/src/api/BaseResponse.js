class BaseResponse {
  constructor(success, message, data, item, customdata) {
    this.success = success;
    this.message = message;
    this.data = data;
    this.item = item;
    this.customdata = customdata;
  }
}
export { BaseResponse };
