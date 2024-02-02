// class ProductRecommended {
//   bool? success;
//   String? message;
//   Object? customData;
//   Null? item;
//   List<DataProduct>? data;
//   int? dataCount;
//
//
//   ProductRecommended(
//       {this.success,
//         this.message,
//         this.customData,
//         this.item,
//         this.data,
//         this.dataCount});
//
//   ProductRecommended.fromJson(Map<String, dynamic> json) {
//     success = json['Success'];
//     message = json['Message'];
//     customData = json['CustomData'];
//     item = json['Item'];
//     if (json['Data'] != null) {
//       data = <DataProduct>[];
//       json['Data'].forEach((v) {
//         data!.add(new DataProduct.fromJson(v));
//       });
//     }
//     dataCount = json['DataCount'];
//   }
//
//
//   Map<String, dynamic> toJson() {
//     final Map<String, dynamic> data = new Map<String, dynamic>();
//     data['Success'] = this.success;
//     data['Message'] = this.message;
//     data['CustomData'] = this.customData;
//     data['Item'] = this.item;
//     if (this.data != null) {
//       data['Data'] = this.data!.map((v) => v.toJson()).toList();
//     }
//     data['DataCount'] = this.dataCount;
//     return data;
//   }
// }
//
// class DataProduct {
//   int? foodListId;
//   String? category;
//   int? categoryId;
//   String? foodName;
//   int? price;
//   int? qty;
//   String? uploadImage;
//   String? description;
//   int? userId;
//   bool? status;
//
//   DataProduct(
//       {this.foodListId,
//         this.category,
//         this.categoryId,
//         this.foodName,
//         this.price,
//         this.qty,
//         this.uploadImage,
//         this.description,
//         this.userId,
//         this.status});
//
//   DataProduct.fromJson(Map<String, dynamic> json) {
//     foodListId = json['FoodListId'];
//     category = json['Category'];
//     categoryId = json['CategoryId'];
//     foodName = json['FoodName'];
//     price = json['Price'];
//     qty = json['qty'];
//     uploadImage = json['UploadImage'];
//     description = json['Description'];
//     userId = json['UserId'];
//     status = json['Status'];
//   }
//
//   Map<String, dynamic> toJson() {
//     final Map<String, dynamic> data = new Map<String, dynamic>();
//     data['FoodListId'] = this.foodListId;
//     data['Category'] = this.category;
//     data['CategoryId'] = this.categoryId;
//     data['FoodName'] = this.foodName;
//     data['Price'] = this.price;
//     data['qty'] = this.qty;
//     data['UploadImage'] = this.uploadImage;
//     data['Description'] = this.description;
//     data['UserId'] = this.userId;
//     data['Status'] = this.status;
//     return data;
//   }
// }
//
//


class ProductRecommended {
  bool? success;
  Null? message;
  Null? customData;
  Null? item;
  List<DataProduct>? data;
  int? dataCount;

  ProductRecommended(
      {this.success,
        this.message,
        this.customData,
        this.item,
        this.data,
        this.dataCount});

  ProductRecommended.fromJson(Map<String, dynamic> json) {
    success = json['Success'];
    message = json['Message'];
    customData = json['CustomData'];
    item = json['Item'];
    if (json['Data'] != null) {
      data = <DataProduct>[];
      json['Data'].forEach((v) {
        data!.add(new DataProduct.fromJson(v));
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

class DataProduct {
  int? foodListId;
  String? category;
  int? categoryId;
  String? foodName;
  int? price;
  int? qty;
  String? uploadImage;
  String? description;
  String? storeName;
  int? userId;
  bool? status;
  int? hint;
  bool? isNew;
  bool? isNoiBat;
  int? quantitySupplied;
  String? expiryDate;
  bool? qtycontrolled;
  bool? qtySuppliedcontrolled;
  double? latitude;
  double? longitude;

  DataProduct(
      {this.foodListId,
        this.category,
        this.categoryId,
        this.foodName,
        this.price,
        this.qty,
        this.uploadImage,
        this.description,
        this.userId,
        this.status,
        this.hint,
        this.isNew,
        this.isNoiBat,
        this.quantitySupplied,
        this.expiryDate,
        this.qtycontrolled,
        this.qtySuppliedcontrolled,
        this.latitude,
        this.longitude});

  DataProduct.fromJson(Map<String, dynamic> json) {
    foodListId = json['FoodListId'];
    category = json['Category'];
    categoryId = json['CategoryId'];
    foodName = json['FoodName'];
    storeName = json['storeName'];
    price = json['Price'];
    qty = json['qty'];
    uploadImage = json['UploadImage'];
    description = json['Description'];
    userId = json['UserId'];
    status = json['Status'];
    hint = json['Hint'];
    isNew = json['IsNew'];
    isNoiBat = json['IsNoiBat'];
    quantitySupplied = json['QuantitySupplied'];
    expiryDate = json['ExpiryDate'];
    qtycontrolled = json['Qtycontrolled'];
    qtySuppliedcontrolled = json['QtySuppliedcontrolled'];
    latitude = json['Latitude'];
    longitude = json['Longitude'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['FoodListId'] = this.foodListId;
    data['Category'] = this.category;
    data['CategoryId'] = this.categoryId;
    data['FoodName'] = this.foodName;
    data['Price'] = this.price;
    data['qty'] = this.qty;
    data['UploadImage'] = this.uploadImage;
    data['Description'] = this.description;
    data['UserId'] = this.userId;
    data['Status'] = this.status;
    data['Hint'] = this.hint;
    data['IsNew'] = this.isNew;
    data['IsNoiBat'] = this.isNoiBat;
    data['QuantitySupplied'] = this.quantitySupplied;
    data['ExpiryDate'] = this.expiryDate;
    data['Qtycontrolled'] = this.qtycontrolled;
    data['QtySuppliedcontrolled'] = this.qtySuppliedcontrolled;
    data['Latitude'] = this.latitude;
    data['Longitude'] = this.longitude;
    data['storeName'] = this.storeName;
    return data;
  }
}
