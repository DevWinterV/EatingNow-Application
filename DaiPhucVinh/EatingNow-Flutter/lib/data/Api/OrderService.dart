import 'package:dio/dio.dart';
import 'package:fam/models/ordercustomerRequest_model.dart';
import 'package:fam/models/ordercustomerResponse_model.dart';
import 'package:fam/util/app_constants.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

import '../../models/OrderRequest.dart';
import '../../models/order_details_model.dart';
import '../../models/paymentconfirm_request.dart';
class ApiResult {
  final bool success;
  final Map<String, dynamic>? data;
  final String? Message;
  final Object? CustomData;
  ApiResult({
    required this.success,
    this.data,
    this.Message,
    this.CustomData
  });
}
class OrderService {
  static const String apiUrl = AppConstants.CreateOreder;
  static const String apiUrlTakeOrderByCustomer = AppConstants.TakeOrderByCustomer;
  static const String apiUrlPaymentConfirm = AppConstants.PaymentConfirm;
  static const String apiUrlGetListOrderLineDetails = AppConstants.GetListOrderLineDetails;
  Future<ApiResult> postOrder(OrderRequest order) async {
    try {
      print(order.toJson());
      final response = await http.post(
        Uri.parse(apiUrl),
        headers: <String, String>{
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: jsonEncode(order.toJson()),
      );

      if (response.statusCode == 200) {
        final Map<String, dynamic> responseBody = jsonDecode(response.body);
        if (responseBody["Success"] == true) {
          return ApiResult(success: true, data: responseBody["Data"], Message: responseBody["Message"], CustomData: responseBody["CustomData"]);
        } else {
          return ApiResult(success: false, Message: responseBody["Message"], CustomData: responseBody["CustomData"]);
        }
      } else {
        return ApiResult(success: false, Message: 'Failed to post order');
      }
    } catch (e) {
      // Xử lý khi có lỗi kết nối
      print('Error posting order: $e');
      throw e; // Ném exception để báo cáo lỗi ra khỏi phương thức
    }
  }
  Future<ApiResult> PaymentConfirm(PaymentTransaction paymentTransaction) async {
    try {
      print(paymentTransaction.toJson());
      final response = await http.post(
        Uri.parse(apiUrlPaymentConfirm),
        headers: <String, String>{
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: jsonEncode(paymentTransaction.toJson()),
      );

      if (response.statusCode == 200) {
        final Map<String, dynamic> responseBody = jsonDecode(response.body);
        if (responseBody["Success"] == true) {
          return ApiResult(success: true, data: responseBody["Data"], Message: responseBody["Message"]);
        } else {
          return ApiResult(success: false, Message: responseBody["Message"]);
        }
      } else {
        return ApiResult(success: false, Message: 'Failed to post order');
      }
    } catch (e) {
      // Xử lý khi có lỗi kết nối
      print('Error posting order: $e');
      throw e; // Ném exception để báo cáo lỗi ra khỏi phương thức
    }
  }
  Future<OrderCustomerResponse?> TakeOrderByCustomer(OrderCustomerRequest request) async {
    try {
      final response = await http.post(
        Uri.parse(apiUrlTakeOrderByCustomer),
        headers: <String, String>{
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: jsonEncode(request.toJson()),
      );

      if (response.statusCode == 200) {
        final Map<String, dynamic> responseBody = jsonDecode(response.body);
        if (responseBody["Success"] == true) {
          return OrderCustomerResponse.fromJson(responseBody);
        } else {
          return OrderCustomerResponse.fromJson(responseBody);
        }
      } else {
        return null;
      }
    } catch (e) {
      // Xử lý khi có lỗi kết nối
      print('Error posting order: $e');
      throw e; // Ném exception để báo cáo lỗi ra khỏi phương thức
    }
  }
  Future<OrderDetailsResponse?> GetListOrderLineDetails(String orderId) async {
    try {
      final response = await Dio().get(
          apiUrlGetListOrderLineDetails,
          queryParameters: {"Id" :orderId }
      );
      // final response = await http.get(
      //   Uri.parse(apiUrlTakeOrderByCustomer),
      //   headers: <String, String>{
      //     'Content-Type': 'application/json; charset=UTF-8',
      //   },
      //
      // );

      if (response.statusCode == 200) {
        final Map<String, dynamic> responseBody = jsonDecode(response.data);
        if (responseBody["Success"] == true) {
          return OrderDetailsResponse.fromJson(responseBody);
        } else {
          return OrderDetailsResponse.fromJson(responseBody);
        }
      } else {
        return null;
      }
    } catch (e) {
      // Xử lý khi có lỗi kết nối
      print('Error posting order: $e');
      throw e; // Ném exception để báo cáo lỗi ra khỏi phương thức
    }
  }

}


