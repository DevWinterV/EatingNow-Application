import 'package:fam/util/app_constants.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

import '../../models/OrderRequest.dart';
import 'package:url_launcher/url_launcher.dart';
class ApiResult {
  final bool success;
  final Map<String, dynamic>? data;
  final String? Message;

  ApiResult({
    required this.success,
    this.data,
    this.Message,
  });
}
class OrderService {
  static const String apiUrl = AppConstants.CreateOreder;

  Future<void> _launchInWebView(Uri url) async {
    if (!await launchUrl(url, mode: LaunchMode.inAppBrowserView)) {
      throw Exception('Could not launch $url');
    }
  }
  Future<ApiResult> postOrder(OrderRequest order) async {
    try {
      final response = await http.post(
        Uri.parse(apiUrl),
        headers: <String, String>{
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: jsonEncode(order.toJson()),
      );

      if (response.statusCode == 200) {
        final Map<String, dynamic> responseBody = jsonDecode(response.body);
        print(responseBody);
        if (responseBody["Success"] == true) {
          return ApiResult(success: true, data: responseBody["Data"], Message: responseBody["Message"]);
        } else {
          print('Failed to post order. Server reported failure.');
          // Xử lý mã lỗi từ máy chủ tại đây, ví dụ: throw Exception('Server reported failure');
          return ApiResult(success: false, Message: 'Server reported failure');
        }
      } else {
        print('Failed to post order. Status code: ${response.statusCode}');
        print('Response body: ${response.body}');
        return ApiResult(success: false, Message: 'Failed to post order');
      }
    } catch (e) {
      // Xử lý khi có lỗi kết nối
      print('Error posting order: $e');
      throw e; // Ném exception để báo cáo lỗi ra khỏi phương thức
    }
  }}