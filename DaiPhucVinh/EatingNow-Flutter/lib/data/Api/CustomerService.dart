import 'dart:convert';
import 'dart:io';
import 'package:fam/models/cuisine_model.dart';
import 'package:fam/models/stores_model.dart';
import 'package:fam/models/updateinfoCustomerResponse.dart';
import 'package:fam/models/updatetoken_model.dart';
import 'package:fam/models/user_account_model.dart';
import 'package:get/get_connect/http/src/multipart/form_data.dart';
import 'package:get/get_connect/http/src/multipart/multipart_file.dart';
import 'package:http/http.dart' as http;

import '../../models/customerReqeust.dart';

class CustomerService {
  final String apiUrl ;
  CustomerService({required this.apiUrl});
  Future<UserAccountModel> fecthUserData(Map<String, dynamic> requestData) async {
    final response = await http.post(
      Uri.parse(apiUrl),
      headers: {
        'Content-Type': 'application/json', // Thiết lập kiểu dữ liệu của yêu cầu là JSON
      },
      body: json.encode(requestData), // Chuyển đổi dữ liệu thành chuỗi JSON và gửi đi
    );

    if (response.statusCode == 200) {
      final jsonData = json.decode(response.body);
      print(jsonData);
      return UserAccountModel.fromJson(jsonData);
    } else {
      throw Exception('Failed to load store data');
    }
  }

  Future<UpdateTokenModel> updateToken(Map<String, dynamic> requestData) async {
    final response = await http.post(
      Uri.parse(apiUrl),
      headers: {
        'Content-Type': 'application/json', // Thiết lập kiểu dữ liệu của yêu cầu là JSON
      },
      body: json.encode(requestData), // Chuyển đổi dữ liệu thành chuỗi JSON và gửi đi
    );

    if (response.statusCode == 200) {
      final jsonData = json.decode(response.body);
      return UpdateTokenModel.fromJson(jsonData);
    } else {
      throw Exception('Failed to updateTokenApp');
    }
  }
  Future<UpdateInfoCustomerResonseModel> updateInfoCustomer(EN_CustomerRequest request, {File? imageFile}) async {
    try {
      final Uri uri = Uri.parse(apiUrl);
      FormData formData = FormData({
        'CustomerId': request.customerId,
        'CompleteName': request.completeName,
        'Email': request.email,
        'Phone': request.phone,
        'file': imageFile
      });
      final http.Response response = await http.post(
        uri,
        body: formData,
      );

      if (response.statusCode == 200) {
        final jsonResponse = jsonDecode(response.body);
        return UpdateInfoCustomerResonseModel.fromJson(jsonResponse);
      } else {
        throw Exception('Failed to update customer info: ${response.reasonPhrase}');
      }
    } catch (e) {
      throw Exception('Failed to update customer info: $e');
    }
  }
}
