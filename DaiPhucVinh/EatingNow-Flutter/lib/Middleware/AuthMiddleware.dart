import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/cupertino.dart';
import 'package:get/get.dart';

class AuthMiddleware extends GetMiddleware {
  FirebaseAuth _auth = FirebaseAuth.instance;
  @override
  RouteSettings? redirect(String? route) {
    if (_auth.currentUser == null && route != '/login') {
      return RouteSettings(name: '/login');
    }
    return null;
  }
}
