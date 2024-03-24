import 'dart:convert';
import 'package:fam/models/foodRatingResponse.dart';
import 'package:fam/models/storenearUser.dart';
import 'package:fam/models/stores_model.dart';
import 'package:fam/util/app_constants.dart';
import 'package:http/http.dart' as http;

import '../../models/FoodRatingRequest.dart';
import '../../models/product_recommended_model.dart';
import 'OrderService.dart';

class FoodRatingService {

  Future<FoodRatingResponse?> TakeFoodsRatingByOrderHeaderId(String? orderheaderId) async {
    final response = await http.get(
      Uri.parse(AppConstants.TakeFoodsRatingByOrderHeaderId+"?orderHeaderId=${orderheaderId ?? ""}"),
      headers: {
        'Content-Type': 'application/json', // Thiết lập kiểu dữ liệu của yêu cầu là JSON
      },
    );

    if (response.statusCode == 200) {
      final jsonData = json.decode(response.body);
      return FoodRatingResponse.fromJson(jsonData);
    } else {
      throw Exception('Failed to load store data');
    }
  }

  Future<ApiResult> CreateFoodRating(FoodRatingRequest request) async {
    try {
      print(request.toJson());
      final response = await http.post(
        Uri.parse(AppConstants.CreateFoodRating),
        headers: <String, String>{
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: jsonEncode(request.toJson()),
      );

      if (response.statusCode == 200) {
        final Map<String, dynamic> responseBody = jsonDecode(response.body);
        if (responseBody["Success"] == true) {
          return ApiResult(success: true, data: responseBody["Data"], Message: responseBody["Message"]);
        } else {
          return ApiResult(success: false, data: responseBody["Data"], Message: responseBody["Message"]);
        }
      } else {
        return ApiResult(success: false, Message: 'Failed to Create Rating');
      }
    } catch (e) {
      // Xử lý khi có lỗi kết nối
      print('Error posting order: $e');
      throw e; // Ném exception để báo cáo lỗi ra khỏi phương thức
    }
  }

  Future<ApiResult> UpdateFoodRating(FoodRatingRequest request) async {
    try {
      print(request.toJson());
      final response = await http.post(
        Uri.parse(AppConstants.UpdateFoodRating),
        headers: <String, String>{
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: jsonEncode(request.toJson()),
      );

      if (response.statusCode == 200) {
        final Map<String, dynamic> responseBody = jsonDecode(response.body);
        if (responseBody["Success"] == true) {
          return ApiResult(success: true, data: responseBody["Data"], Message: responseBody["Message"]);
        } else {
          return ApiResult(success: false, data: responseBody["Data"], Message: responseBody["Message"]);
        }
      } else {
        return ApiResult(success: false, Message: 'Failed to Update Rating');
      }
    } catch (e) {
      // Xử lý khi có lỗi kết nối
      print('Error posting order: $e');
      throw e; // Ném exception để báo cáo lỗi ra khỏi phương thức
    }
  }

  Future<ApiResult> DeleteFoodRating(FoodRatingRequest request) async {
    try {
      print(request.toJson());
      final response = await http.post(
        Uri.parse(AppConstants.DeleteFoodRating),
        headers: <String, String>{
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: jsonEncode(request.toJson()),
      );

      if (response.statusCode == 200) {
        final Map<String, dynamic> responseBody = jsonDecode(response.body);
        if (responseBody["Success"] == true) {
          return ApiResult(success: true, data: responseBody["Data"], Message: responseBody["Message"]);
        } else {
          return ApiResult(success: false, data: responseBody["Data"], Message: responseBody["Message"]);
        }
      } else {
        return ApiResult(success: false, Message: 'Failed to Delete Rating');
      }
    } catch (e) {
      // Xử lý khi có lỗi kết nối
      print('Error posting order: $e');
      throw e; // Ném exception để báo cáo lỗi ra khỏi phương thức
    }
  }
}
