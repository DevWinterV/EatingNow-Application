import 'dart:async';
import 'dart:typed_data';
import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/models/LocationData.dart';
import 'package:fam/util/Colors.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:geolocator/geolocator.dart';
import 'package:permission_handler/permission_handler.dart';
import 'package:provider/provider.dart';
import '../../data/Api/GoogleAPIService.dart';
import '../../util/dimensions.dart';
import 'data_handler/AppData.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
class GoogleMapsPage extends StatefulWidget {
  const GoogleMapsPage({super.key});

  @override
  State<GoogleMapsPage> createState() => _GoogleMapsPageState();
}

class _GoogleMapsPageState extends State<GoogleMapsPage> {
  final Completer<GoogleMapController> _CompletergooglemapController = Completer();
  GoogleMapController? _newGoogleAMapsContrller;
  LocationPermission? _locationPermission;
  static CameraPosition? _cameraPosition;
  static LatLng? _inittalLatlng;
  Position? currentPosition;
  Set<Circle> circleSet ={};
  LatLng? oncameramoveLatlng;
  bool isPinMarkerVisible = true;
  late Uint8List pickupMarker = Uint8List.fromList([]);
  final googleApiService = GoogleAPIService();

  // yêu cầu quyền sử dụng vị trí của người dùng
  Future<void> requestLocationPermission() async {
    final status = await Permission.location.request();
    if (status.isGranted) {
      // Tự động lấy vị trí đã sử dụng gần nhất
      _getUserCurrentLocation();
    } else if (status.isDenied) {
      _locationPermission = await Geolocator.requestPermission();
    } else if (status.isPermanentlyDenied) {
      openAppSettings();
    }
  }

  void _getUserCurrentLocation() async{
    try {
      Position position = await Geolocator.getCurrentPosition(
        desiredAccuracy: LocationAccuracy.bestForNavigation,
      );
      // Lấy địa chỉ từ location người dùng
      currentPosition = position;
      setState(() {
        _inittalLatlng = LatLng(currentPosition?.latitude ?? 0.0, currentPosition?.longitude ?? 0.0);
        _cameraPosition = CameraPosition(target: _inittalLatlng as LatLng, zoom: 14.9);
        _newGoogleAMapsContrller?.animateCamera(CameraUpdate.newCameraPosition(_cameraPosition!));
      });

    } catch (e) {
      print(e.toString());
    }
  }

  @override
  void dispose() {
    // TODO: implement initState
    super.dispose();
    _newGoogleAMapsContrller!.dispose();
  }

  @override
  void initState() {
    requestLocationPermission();
    // TODO: implement initState
    super.initState();
    InitMarker();
  }


  void InitMarker () async {
    pickupMarker = await getMarker("marker");
  }

  // Hàm lấy vị trí hiện tại của người dùng
  Future<AddressResult?> getCurrentLocation() async {
    try {
      // Lấy địa chỉ từ location người dùng
      AddressResult addressResult = await googleApiService.fetchPlacesFromLocation(
          oncameramoveLatlng?.latitude ?? 0.0, oncameramoveLatlng?.longitude ?? 0.0);
      if (addressResult != null) {
          return addressResult;
      }
      return null;

    } catch (e) {
      print(e.toString());
    }
  }

  Future<Uint8List> getMarker (String fileName) async {
    ByteData byteData = await DefaultAssetBundle.of(context).load("assets/image/$fileName.png");
    return byteData.buffer.asUint8List();
  }

  void pickOriginPositionOnMap() async  {
    final addressResult = await getCurrentLocation();
    LocationData? locationData = new LocationData(name: addressResult?.name_address ??"", latitude: oncameramoveLatlng?.latitude ?? 0.0, longitude: oncameramoveLatlng?.longitude ?? 0.0, address: addressResult?.formatted_address ?? "");
    Provider.of<AppData>(context, listen: false).UpdatePickUpLocationData(locationData);
  }

  void _getPinAddress(){
    pickOriginPositionOnMap();
  }

  Future<LatLng> pickLocationOnMap(CameraPosition cameraPosition) async{
    LatLng onCameraMoveLatLng = cameraPosition.target;
    Circle piccircle = Circle(
        circleId: const CircleId("0"),
        radius: 1,
        zIndex: 1,
        strokeColor: Colors.black54,
        center: cameraPosition.target,
        fillColor:  Colors.blue.withAlpha(70),
    );
    circleSet.add(piccircle);
    return onCameraMoveLatLng;
  }


  @override
  Widget build(BuildContext context) {
    final appdata = Provider.of<AppData>(context);
    String? placeAddress;
    if(appdata.locationData != null){
      placeAddress = appdata.locationData?.address ?? " ";
    }
    else{
      placeAddress = " ";
    }
    return ChangeNotifierProvider(
        create: (context) => AppData(),
        child: Scaffold(
              appBar: AppBar(
                centerTitle: true,
                toolbarHeight: 50,
                title: Text(
                  "Bản đồ",
                  maxLines: 1,
                  style:
                  TextStyle(
                      color: Colors.black,
                      fontSize: Dimensions.font20,
                      fontWeight: FontWeight.normal
                  ),
                ),
              ),
              body:
              _inittalLatlng == null ?
              Center(child: SmallText(text: "Đang tải dữ liệu bản đồ ...",))
                  :
              Stack(
                alignment: Alignment.center,
                children: [
                  Container(
                    height: Dimensions.screenHeight,
                    child:  GoogleMap(
                      mapType: MapType.normal,
                      myLocationEnabled: true,
                      myLocationButtonEnabled: true,
                      zoomControlsEnabled: true,
                      zoomGesturesEnabled: true,
                      initialCameraPosition: _cameraPosition!,
                      onMapCreated: (GoogleMapController mapContrller){
                        _CompletergooglemapController.complete(mapContrller);
                        _newGoogleAMapsContrller = mapContrller;
                      },
                      onCameraMove: (position) async{
                        if(isPinMarkerVisible){
                          oncameramoveLatlng = await pickLocationOnMap(position);
                          //pickOriginPositionOnMap();
                        }
                      },
                      onCameraIdle: _getPinAddress,
                    ),
                  ),
                  Visibility(
                      visible: isPinMarkerVisible,
                      child:
                      pickupMarker.length > 0 ?
                        Image.memory(pickupMarker,
                          height: 40,
                          width: 40,
                          alignment: Alignment.center,
                          frameBuilder: (context,child, frame, wasSynchronouslyLoaded) {
                            return Transform.translate(offset: const Offset(0, -20), child: child,);
                          }
                        )
                          :
                        SizedBox()
                  ),
                  Positioned(
                      bottom: 0.0,
                      left: 0.0,
                      right:  0.0,
                      child: Container(
                        decoration: BoxDecoration(
                          borderRadius:BorderRadius.only(
                            topLeft: Radius.circular(Dimensions.radius15 * 1),
                            topRight: Radius.circular(Dimensions.radius15 * 1),
                          ),
                          color: Colors.white
                        ),
                        height: 130,
                        child: Padding(
                          padding: const EdgeInsets.symmetric(horizontal: 16.0, vertical: 10.0),
                          child: Column(
                            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                            children: [
                              Container(
                                height: 50.0,
                                decoration: BoxDecoration(
                                  borderRadius: BorderRadius.circular(10.0),
                                  border: Border.all(color: AppColors.mainColor, width: 1.0, style: BorderStyle.solid)
                                ),
                                child: Row(
                                  mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                                  children: [
                                    SizedBox(width: 5,),
                                    Icon(Icons.my_location_sharp, color: AppColors.mainColor, size: 30),
                                    SizedBox(width: 5,),
                                    Expanded(
                                        child: BigText(
                                          text: placeAddress,
                                          maxlines: 2,
                                        )
                                    )
                                  ],
                                ),
                              ),
                              ElevatedButton(
                                style: ElevatedButton.styleFrom(
                                  // Customize the background color
                                  foregroundColor: Colors.white,
                                  backgroundColor: AppColors.mainColor,                          // Add other customizations as needed
                                  padding: EdgeInsets.only(right: 20.0, left: 20.0),
                                  shape: RoundedRectangleBorder(
                                    borderRadius: BorderRadius.circular(10.0),
                                  ),
                                  minimumSize: Size(double.infinity, 50), // Đặt kích thước tối thiểu cho nút
                                ),
                                onPressed: () async {
                                  Navigator.of(context).pop(appdata.locationData);
                                },
                                child: Text('Sử dụng vị trí này', style: TextStyle(fontSize: Dimensions.font16),),
                              ),
                            ],
                          ),
                        ),
                      ))
                ],
              ),
      ),
    );
  }
}

