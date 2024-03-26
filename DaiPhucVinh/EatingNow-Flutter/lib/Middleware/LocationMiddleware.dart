import 'package:fam/locationState.dart';
import 'package:flutter/cupertino.dart';
import 'package:get/get.dart';
class LocationMiddleware extends GetMiddleware {
  @override
  RouteSettings? redirect(String? route) {
    print(locationState.currentLocation.peek());
    if (locationState.currentLocation.peek() == null && route != '/getlocation') {
      return RouteSettings(name: '/getlocation');
    }
    return null;
  }
}
