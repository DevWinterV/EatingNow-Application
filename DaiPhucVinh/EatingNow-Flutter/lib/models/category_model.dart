class CategoryModel {
  bool? success;
  Null? message;
  Null? customData;
  Null? item;
  List<Category>? data;
  int? dataCount;

  CategoryModel(
      {this.success,
        this.message,
        this.customData,
        this.item,
        this.data,
        this.dataCount});

  CategoryModel.fromJson(Map<String, dynamic> json) {
    success = json['Success'];
    message = json['Message'];
    customData = json['CustomData'];
    item = json['Item'];
    if (json['Data'] != null) {
      data = <Category>[];
      json['Data'].forEach((v) {
        data!.add(new Category.fromJson(v));
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

class Category {
  int? categoryId;
  String? categoryName;
  String? nameStore;
  String? descriptionStore;
  String? openTime;
  bool? status;
  String? address;
  String? phone;
  String? email;
  double? latitude;
  double? longitude;

  Category(
      {this.categoryId,
        this.categoryName,
        this.nameStore,
        this.descriptionStore,
        this.openTime,
        this.status,
        this.address,
        this.phone,
        this.email,
        this.latitude,
        this.longitude});

  Category.fromJson(Map<String, dynamic> json) {
    categoryId = json['CategoryId'];
    categoryName = json['CategoryName'];
    nameStore = json['NameStore'];
    descriptionStore = json['DescriptionStore'];
    openTime = json['OpenTime'];
    status = json['Status'];
    address = json['Address'];
    phone = json['Phone'];
    email = json['Email'];
    latitude = json['Latitude'];
    longitude = json['Longitude'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['CategoryId'] = this.categoryId;
    data['CategoryName'] = this.categoryName;
    data['NameStore'] = this.nameStore;
    data['DescriptionStore'] = this.descriptionStore;
    data['OpenTime'] = this.openTime;
    data['Status'] = this.status;
    data['Address'] = this.address;
    data['Phone'] = this.phone;
    data['Email'] = this.email;
    data['Latitude'] = this.latitude;
    data['Longitude'] = this.longitude;
    return data;
  }
}
