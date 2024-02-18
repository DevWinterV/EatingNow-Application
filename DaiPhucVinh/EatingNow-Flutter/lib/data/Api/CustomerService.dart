import 'dart:convert';
import 'package:fam/models/cuisine_model.dart';
import 'package:fam/models/stores_model.dart';
import 'package:fam/models/user_account_model.dart';
import 'package:http/http.dart' as http;

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
      print('Fetch data user response: $response');
      final jsonData = json.decode(response.body);
      return UserAccountModel.fromJson(jsonData);
    } else {
      throw Exception('Failed to load store data');
    }
  }
}
