class UpdateInfoCustomerResonseModel {
  bool? success;
  Null? message;
  Null? customData;
  bool? item;
  Null? data;
  int? dataCount;

  UpdateInfoCustomerResonseModel(
      {this.success,
        this.message,
        this.customData,
        this.item,
        this.data,
        this.dataCount});

  UpdateInfoCustomerResonseModel.fromJson(Map<String, dynamic> json) {
    success = json['Success'];
    message = json['Message'];
    customData = json['CustomData'];
    item = json['Item'];
    data = json['Data'];
    dataCount = json['DataCount'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['Success'] = this.success;
    data['Message'] = this.message;
    data['CustomData'] = this.customData;
    data['Item'] = this.item;
    data['Data'] = this.data;
    data['DataCount'] = this.dataCount;
    return data;
  }
}
