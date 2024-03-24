import 'dart:async';

import 'package:fam/models/LocationData.dart';
import 'package:fam/storage/locationstorage.dart';
import 'package:signals/signals_flutter.dart';

class LocationState{
  late LocationData locationData;

  void getLocationData() async {
    locationData = await LocationStorage().getSavedLocation();
    currentLocation = signal<LocationData?>(locationData);
  }

  final _controller = StreamController<LocationData?>();

  late Signal<LocationData?> currentLocation;

  late final isCheckIn = computed(() => currentLocation != null);

  late final currentLocationAuth = computed(() => currentLocation() ?? null);

  late Connect<LocationData?> _locationListener;

  LocationState() {
    getLocationData();
    _locationListener = connect(currentLocation) << _controller.stream;
  }
  dispose(){
    _locationListener.dispose();
    _controller.close();
  }

  save(LocationData locationData){
    _controller.add(locationData);
  }

}
final locationState = LocationState();