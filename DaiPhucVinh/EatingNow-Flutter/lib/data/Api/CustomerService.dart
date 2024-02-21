import 'dart:convert';
import 'dart:io';
import 'package:dio/dio.dart';
import 'package:fam/models/updateinfoCustomerResponse.dart';
import 'package:fam/models/updatetoken_model.dart';
import 'package:fam/models/user_account_model.dart';
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
      FormData formData = FormData();
      // Thêm dữ liệu từ request
      formData.fields.add(
          MapEntry('form', jsonEncode(request)),
      );
      // Nếu có file, thêm file vào formData
      if (imageFile != null) {
        formData.files.add(MapEntry(
          'file[]',
          await MultipartFile.fromFile(
            imageFile.path,
            filename: imageFile.path.split('/').last, // Trích xuất tên tệp từ đường dẫn
          ),
        ));
      }


      Options options = Options(
        contentType: 'multipart/form-data',
      );

      final response = await Dio().post(apiUrl, data: formData, options: options);
      print(response.data);
      if (response.statusCode == 200) {
        return UpdateInfoCustomerResonseModel.fromJson(response.data);
      } else {
        throw Exception('Failed to update customer info: ${response.data['message']}');
      }
    } catch (e) {
      throw Exception('Failed to update customer info: $e');
    }
  }
}
