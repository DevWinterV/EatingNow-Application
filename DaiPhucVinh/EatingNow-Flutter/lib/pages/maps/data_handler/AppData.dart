import 'package:flutter/material.dart';

import '../../../models/LocationData.dart';

class AppData with ChangeNotifier{
  LocationData? locationData;
  void UpdatePickUpLocationData(LocationData locationDataPickup){
    locationData = locationDataPickup;
    notifyListeners();
  }
}