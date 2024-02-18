import 'dart:convert';

import 'package:shared_preferences/shared_preferences.dart';

class UserAccountStorage {
  Future<void> saveUserAccount(UserAccount user) async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    // Chuyển đối tượng UserAccount thành chuỗi JSON trước khi lưu
    final String userJson = user.toJson();
    await prefs.setString('useraccount', userJson);
    final userSaved = await getSavedUserAccount();
    print('userSaved $userSaved');
  }
  Future<UserAccount> getSavedUserAccount() async{
    SharedPreferences prefs = await SharedPreferences.getInstance();
    final String savedUserJson = prefs.getString('useraccount') ?? '';
    // Chuyển chuỗi JSON thành đối tượng UserAccount khi lấy ra
    final UserAccount savedUser = UserAccount.fromJson(savedUserJson);
    return savedUser;
  }
}

class UserAccount {
  String userId;
  String name;
  String phone;

  UserAccount({required this.userId, required this.name, required this.phone});

  // Chuyển đối tượng UserAccount thành chuỗi JSON
  String toJson() {
    return '{"userId": "$userId", "name": "$name", "phone": "$phone"}';
  }

  // Chuyển chuỗi JSON thành đối tượng UserAccount
  factory UserAccount.fromJson(String json) {
    final Map<String, dynamic> data = jsonDecode(json);
    return UserAccount(
      userId: data['userId'] ?? '',
      name: data['name'] ?? '',
      phone: data['phone'] ?? '',
    );
  }
}
