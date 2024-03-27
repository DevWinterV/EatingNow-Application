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
  Future<void> saveLocation(String name, double latitude, double longitude, String address) async {
    await prefs.setDouble('latitude', latitude);
    await prefs.setDouble('longitude', longitude);
    await prefs.setString('address', address);
    await prefs.setString('name', name);

  }

   Future<LocationData?> getSavedLocation() async {
     try{
       prefs = await SharedPreferences.getInstance();

       // Check if prefs is null, and initialize if needed
       if (prefs == null) {
         await initPrefs();
       }

       if (prefs != null) {
         final String name = prefs!.getString('name') ?? '';
         final double latitude = prefs!.getDouble('latitude') ?? 0.0;
         final double longitude = prefs!.getDouble('longitude') ?? 0.0;
         final String address = prefs!.getString('address') ?? '';
         print('Lấy được vị trí');
         return LocationData(
           name: name,
           latitude: latitude,
           longitude: longitude,
           address: address,
         );
       } else {
         print('Không lấy được vị trí');
         return null;
       }
     }catch(e){
       throw Exception("SharedPreferences initialization failed");
     }
   }
  double getSavedLatitude() {
    return prefs.getDouble('latitude') ?? 0.0;
  }

  double getSavedLongitude() {
    return prefs.getDouble('longitude') ?? 0.0;
  }

  String getSavedAddress() {
    return prefs.getString('address') ?? '';
  }
  String getSavedName() {
    return prefs.getString('name') ?? '';
  }
}
