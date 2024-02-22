class OrderCustomerRequest {
  String? customerId;
  int? orderType;
  String? status;

  OrderCustomerRequest({this.customerId, this.orderType, this.status});

  OrderCustomerRequest.fromJson(Map<String, dynamic> json) {
    customerId = json['CustomerId'];
    orderType = json['OrderType'];
    status = json['Status'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['CustomerId'] = this.customerId;
    data['OrderType'] = this.orderType;
    data['Status'] = this.status;
    return data;
  }
}
