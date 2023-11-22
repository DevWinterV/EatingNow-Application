import 'dart:convert';
import 'package:fam/models/storenearUser.dart';
import 'package:fam/models/stores_model.dart';
import 'package:fam/util/app_constants.dart';
import 'package:http/http.dart' as http;

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

  Future<StoreNearUserModel?> fetchStoreDataNearUser(Map<String, dynamic> requestData) async {
    print(apiUrl);
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


  Future<void> postData(StoreModel store) async {
    final Map<String, dynamic> data = store.toJson();

    final response = await http.post(
      Uri.parse(apiUrl),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
      },
      body: jsonEncode(data),
    );

    if (response.statusCode == 200) {
      print('Data posted successfully');
    } else {
      throw Exception('Failed to post data');
    }
  }
}
