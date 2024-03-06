import 'dart:convert';
import 'dart:ffi';
import 'package:fam/models/foodlistSearchResponse.dart';
import 'package:fam/models/product_recommended_model.dart';
import 'package:fam/models/stores_model.dart';
import 'package:http/http.dart' as http;

import '../../util/app_constants.dart';

class ApiResult {
  final bool success;
  final Map<String, dynamic>? data;
  final String? errorMessage;

  ApiResult({
    required this.success,
    this.data,
    this.errorMessage,
  });
}
class ProductService {
  final String apiUrl;
  final String apiUrlSearchFoodListByUser  = AppConstants.SearchFoodListByUser;
  ProductService({required this.apiUrl});

  Future<ProductRecommended> fectProductRecommended(Map<String, dynamic> requestData) async {
    final response = await http.post(
      Uri.parse(apiUrl),
      headers: {
        'Content-Type': 'application/json', // Thiết lập kiểu dữ liệu của yêu cầu là JSON
      },
      body: json.encode(requestData), // Chuyển đổi dữ liệu thành chuỗi JSON và gửi đi
    );
    if (response.statusCode == 200) {
      final jsonData = json.decode(response.body);
      print('response $jsonData');

      return ProductRecommended.fromJson(jsonData);
    } else {
      throw Exception('Failed to load store data');
    }
  }

  Future<FoodListSearchResponse> SearchFoodListByUser(String keyword, Float latitude, Float longitude) async {
    final response = await http.get(
      Uri.parse('${apiUrlSearchFoodListByUser}?keyword=${keyword}&latitude=${latitude}&longitude=${longitude}'),
      headers: {
        'Content-Type': 'application/json', // Thiết lập kiểu dữ liệu của yêu cầu là JSON
      },
    );
    if (response.statusCode == 200) {
      final jsonData = json.decode(response.body);
      return FoodListSearchResponse.fromJson(jsonData);
    } else {
      throw Exception('Failed to load FoodListSearchResponse');
    }
  }
}
