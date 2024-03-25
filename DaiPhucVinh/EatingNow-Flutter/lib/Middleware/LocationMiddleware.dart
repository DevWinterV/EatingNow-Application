import 'package:fam/storage/locationstorage.dart';
import 'package:flutter/cupertino.dart';
import 'package:get/get.dart';

class LocationMiddleware extends GetMiddleware {
  @override
  RouteSettings? redirect(String? route) {
    print('LocationStorage().getSavedLocation()  ${LocationStorage().getSavedLocation() }');
    if (LocationStorage().getSavedLocation() == null && route != '/getlocation') {
      return RouteSettings(name: '/getlocation');
    }
    return null;
  }
}
