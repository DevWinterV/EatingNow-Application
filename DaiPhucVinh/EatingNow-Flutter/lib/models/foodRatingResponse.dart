class FoodRatingResponse {
  bool? success;
  String? message;
  Null? customData;
  Null? item;
  List<Data>? data;
  int? dataCount;

  FoodRatingResponse(
      {this.success,
        this.message,
        this.customData,
        this.item,
        this.data,
        this.dataCount});

  FoodRatingResponse.fromJson(Map<String, dynamic> json) {
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
  int? foodRatingId;
  int? foodId;
  String? foodName;
  String? phone;
  String? email;
  int? price;
  String? uploadImage;
  bool? status;
  double? rating;
  String? comment;
  String? addedAt;
  String? customerId;
  String? completeName;
  int? userId;
  String? fullName;
  String? orderHeaderId;
  bool? reviewed;

  Data(
      {this.foodRatingId,
        this.foodId,
        this.foodName,
        this.phone,
        this.email,
        this.price,
        this.uploadImage,
        this.status,
        this.rating,
        this.comment,
        this.addedAt,
        this.customerId,
        this.completeName,
        this.userId,
        this.fullName,
        this.orderHeaderId,
        this.reviewed});

  Data.fromJson(Map<String, dynamic> json) {
    foodRatingId = json['FoodRatingId'];
    foodId = json['FoodId'];
    foodName = json['FoodName'];
    phone = json['Phone'];
    email = json['Email'];
    price = json['Price'];
    uploadImage = json['UploadImage'];
    status = json['Status'];
    rating = json['Rating'];
    comment = json['Comment'];
    addedAt = json['AddedAt'];
    customerId = json['CustomerId'];
    completeName = json['CompleteName'];
    userId = json['UserId'];
    fullName = json['FullName'];
    orderHeaderId = json['OrderHeaderId'];
    reviewed = json['reviewed'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['FoodRatingId'] = this.foodRatingId;
    data['FoodId'] = this.foodId;
    data['FoodName'] = this.foodName;
    data['Phone'] = this.phone;
    data['Email'] = this.email;
    data['Price'] = this.price;
    data['UploadImage'] = this.uploadImage;
    data['Status'] = this.status;
    data['Rating'] = this.rating;
    data['Comment'] = this.comment;
    data['AddedAt'] = this.addedAt;
    data['CustomerId'] = this.customerId;
    data['CompleteName'] = this.completeName;
    data['UserId'] = this.userId;
    data['FullName'] = this.fullName;
    data['OrderHeaderId'] = this.orderHeaderId;
    data['reviewed'] = this.reviewed;
    return data;
  }
}
