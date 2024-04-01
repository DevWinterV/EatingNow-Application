import 'package:fam/storage/locationstorage.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:get/get.dart';

import '../util/Colors.dart';
import '../util/dimensions.dart';

class LocationMiddleware extends GetMiddleware {
  @override
  RouteSettings? redirect(String? route) {
    LocationStorage().getSavedLocation().then((savedLocation) {
      if (savedLocation != null &&
          savedLocation.longitude == 0.0 &&
          savedLocation.latitude == 0.0 &&
          savedLocation.name == '' &&
          savedLocation.address == '' &&
          route != '/getlocation') {
          Get.offNamed('/getlocation');
          Fluttertoast.showToast(msg: "Vui lòng chọn vị trí của bạn",
              toastLength: Toast.LENGTH_LONG,
              gravity: ToastGravity.BOTTOM_LEFT,
              backgroundColor: AppColors.toastSuccess,
              textColor: Colors.black54,
              timeInSecForIosWeb: 1,
              fontSize: Dimensions.font13);
      }
    });
    // Returning null by default
    return null;
  }
}
