class CuisineModel {
  bool? success;
  Null? message;
  Null? customData;
  Null? item;
  List<DataCuiSine>? data;
  int? dataCount;

  CuisineModel(
      {this.success,
        this.message,
        this.customData,
        this.item,
        this.data,
        this.dataCount});

  CuisineModel.fromJson(Map<String, dynamic> json) {
    success = json['Success'];
    message = json['Message'];
    customData = json['CustomData'];
    item = json['Item'];
    if (json['Data'] != null) {
      data = <DataCuiSine>[];
      json['Data'].forEach((v) {
        data!.add(new DataCuiSine.fromJson(v));
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

class DataCuiSine {
  int? cuisineId;
  String? absoluteImage;
  String? name;

  DataCuiSine({this.cuisineId, this.absoluteImage, this.name});

  DataCuiSine.fromJson(Map<String, dynamic> json) {
    cuisineId = json['CuisineId'];
    absoluteImage = json['AbsoluteImage'];
    name = json['Name'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['CuisineId'] = this.cuisineId;
    data['AbsoluteImage'] = this.absoluteImage;
    data['Name'] = this.name;
    return data;
  }
}