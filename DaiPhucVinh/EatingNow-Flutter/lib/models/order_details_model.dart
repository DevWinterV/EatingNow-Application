class OrderDetailsResponse {
  bool? success;
  Null? message;
  Null? customData;
  Null? item;
  List<Data>? data;
  int? dataCount;

  OrderDetailsResponse(
      {this.success,
        this.message,
        this.customData,
        this.item,
        this.data,
        this.dataCount});

  OrderDetailsResponse.fromJson(Map<String, dynamic> json) {
    success = json['Success'];
    message = json['Message'];
    customData = json['CustomData'];
    item = json['Item'];
    if (json['Data'] != null) {
      data = <Data>[];
      json['Data'].forEach((v) {
        data!.add(new Data.fromJson(v));
      });
    }
    dataCount = json['DataCount'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['Success'] = this.success;
    data['Message'] = this.message;
    data['CustomData'] = this.customData;
    data['Item'] = this.item;
    if (this.data != null) {
      data['Data'] = this.data!.map((v) => v.toJson()).toList();
    }
    data['DataCount'] = this.dataCount;
    return data;
  }
}

class Data {
  int? orderLineId;
  String? orderHeaderId;
  int? foodListId;
  Null? categoryName;
  String? foodName;
  int? price;
  int? qty;
  String? uploadImage;
  String? description;
  int? totalPrice;

  Data(
      {this.orderLineId,
        this.orderHeaderId,
        this.foodListId,
        this.categoryName,
        this.foodName,
        this.price,
        this.qty,
        this.uploadImage,
        this.description,
        this.totalPrice});

  Data.fromJson(Map<String, dynamic> json) {
    orderLineId = json['OrderLineId'];
    orderHeaderId = json['OrderHeaderId'];
    foodListId = json['FoodListId'];
    categoryName = json['CategoryName'];
    foodName = json['FoodName'];
    price = json['Price'];
    qty = json['qty'];
    uploadImage = json['UploadImage'];
    description = json['Description'];
    totalPrice = json['TotalPrice'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['OrderLineId'] = this.orderLineId;
    data['OrderHeaderId'] = this.orderHeaderId;
    data['FoodListId'] = this.foodListId;
    data['CategoryName'] = this.categoryName;
    data['FoodName'] = this.foodName;
    data['Price'] = this.price;
    data['qty'] = this.qty;
    data['UploadImage'] = this.uploadImage;
    data['Description'] = this.description;
    data['TotalPrice'] = this.totalPrice;
    return data;
  }
}
