import 'dart:convert';
import 'package:fam/models/cuisine_model.dart';
import 'package:fam/models/stores_model.dart';
import 'package:http/http.dart' as http;

import '../../models/category_model.dart';

class  CategoryService{
  final String apiUrl;

  CategoryService({required this.apiUrl});

  Future<CategoryModel?> TakeCategoryByStoreId(int Id) async {
    final response = await http.get(
      Uri.parse(apiUrl+"?Id=${Id}"),
      headers: {
        'Content-Type': 'application/json', // Thiết lập kiểu dữ liệu của yêu cầu là JSON
      },
    );

    if (response.statusCode == 200) {
      final jsonData = json.decode(response.body);
      return CategoryModel.fromJson(jsonData);
    } else {
      throw Exception('Failed to load store data');
    }
  }

}
