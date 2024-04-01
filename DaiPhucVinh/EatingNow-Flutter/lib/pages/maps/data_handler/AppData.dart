import 'package:flutter/material.dart';

import '../../../models/LocationData.dart';

class AppData with ChangeNotifier{
  LocationData? locationData;
  void UpdatePickUpLocationData(LocationData locationDataPickup){
    print("Cập nhật lại tọa độ vị trí $locationDataPickup");
    locationData = locationDataPickup;
    notifyListeners();
  }
}