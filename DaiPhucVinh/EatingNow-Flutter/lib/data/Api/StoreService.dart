import 'dart:convert';
import 'package:fam/models/storenearUser.dart';
import 'package:fam/models/stores_model.dart';
import 'package:fam/util/app_constants.dart';
import 'package:http/http.dart' as http;

import '../../models/product_recommended_model.dart';

class StoreService {
  final String apiUrl;

  StoreService({required this.apiUrl});

  Future<StoreModel?> fetchStoreData(Map<String, dynamic> requestData) async {
    final response = await http.post(
      Uri.parse(apiUrl),
      headers: {
        'Content-Type': 'application/json', // Thiết lập kiểu dữ liệu của yêu cầu là JSON
      },
      body: json.encode(requestData), // Chuyển đổi dữ liệu thành chuỗi JSON và gửi đi
    );

    if (response.statusCode == 200) {
      final jsonData = json.decode(response.body);
      return StoreModel.fromJson(jsonData);
    } else {
      throw Exception('Failed to load store data');
    }
  }

  Future<ProductRecommended?> TakeFoodListByStoreId(Map<String, dynamic> requestData) async {
    final response = await http.post(
      Uri.parse(apiUrl),
      headers: {
        'Content-Type': 'application/json', // Thiết lập kiểu dữ liệu của yêu cầu là JSON
      },
      body: json.encode(requestData), // Chuyển đổi dữ liệu thành chuỗi JSON và gửi đi
    );

    if (response.statusCode == 200) {
      final jsonData = json.decode(response.body);
      return ProductRecommended.fromJson(jsonData);
    } else {
      throw Exception('Failed to load store data');
    }
  }

  Future<ProductRecommended?> TakeAllFoodListByStoreId(Map<String, dynamic> requestData) async {
    final response = await http.post(
      Uri.parse(apiUrl),
      headers: {
        'Content-Type': 'application/json', // Thiết lập kiểu dữ liệu của yêu cầu là JSON
      },
      body: json.encode(requestData), // Chuyển đổi dữ liệu thành chuỗi JSON và gửi đi
    );

    if (response.statusCode == 200) {
      final jsonData = json.decode(response.body);
      return ProductRecommended.fromJson(jsonData);
    } else {
      throw Exception('Failed to load store data');
    }
  }



  Future<StoreNearUserModel?> fetchStoreDataNearUser(Map<String, dynamic> requestData) async {
    final response = await http.post(
      Uri.parse(apiUrl),
      headers: {
        'Content-Type': 'application/json', // Thiết lập kiểu dữ liệu của yêu cầu là JSON
      },
      body: json.encode(requestData), // Chuyển đổi dữ liệu thành chuỗi JSON và gửi đi
    );
    print(response);
    if (response.statusCode == 200) {
      final jsonData = json.decode(response.body);
      return StoreNearUserModel.fromJson(jsonData);
    } else {
      throw Exception('Failed to load store data');
    }
  }



}
