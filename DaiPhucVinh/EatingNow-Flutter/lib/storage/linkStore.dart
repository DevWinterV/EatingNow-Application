import 'package:shared_preferences/shared_preferences.dart';

import '../models/LocationData.dart';

// Sử dụng cơ sở dữ liệu local
class LocationStorage {
  late SharedPreferences prefs; // Sử dụng "late" để trì hoãn khởi tạo prefs

  // Hàm khởi tạo
  LocationStorage() {
    initPrefs(); // Gọi hàm khởi tạo prefs trong constructor
  }


  // Khởi tạo prefs bằng cách sử dụng async trong constructor
  Future<void> initPrefs() async {
    prefs = await SharedPreferences.getInstance();
  }
  // Lưu dữ liệu vị trí của người dùng vào Local
  Future<void> saveLocation(String link) async {
    await prefs.setString('link', link);
  }

  String getSavedLink() {
    return prefs.getString('link') ?? '';
  }
}
