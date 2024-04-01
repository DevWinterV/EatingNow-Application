import 'package:fam/util/Colors.dart';
import 'package:fam/util/app_constants.dart';
import 'package:fam/util/dimensions.dart';
import 'package:flutter/material.dart';
import 'package:geolocator/geolocator.dart';
import 'package:flutter_native_splash/flutter_native_splash.dart';
import 'package:permission_handler/permission_handler.dart';
import '../../data/Api/GoogleAPIService.dart';
import '../../models/LocationData.dart';
import '../../storage/cartstorage.dart';
import '../../storage/locationstorage.dart';
import 'main_food_page.dart';


class LocationPage extends StatefulWidget {
  final String? link;
  LocationPage({required this.link});
  @override
  _LocationPageState createState() => _LocationPageState(link: link);
}

class _LocationPageState extends State<LocationPage> {
  final String? link;
  _LocationPageState({required this.link});
  String? _locationMessage;
  final googleApiService = GoogleAPIService(
      'AIzaSyAG61NrUZkmMW8AS9F7B8mCdT9KQhgG95s');
  late final LocationStorage locationStorage;
  late bool isloadingdata = true;
  late AddressResult addressResult;
  late Position position;
  List<CartItem> cartItems = [];


  @override
  initState() {
    super.initState();
    initialization();
    requestLocationPermission();
    locationStorage = LocationStorage();
  }


  // Hàm xóa SplassCreen
  void initialization() async {
    FlutterNativeSplash.remove();
  }

  // yêu cầu quyền sử dụng vị trí của người dùng
  Future<void> requestLocationPermission() async {
    final status = await Permission.location.request();
    if (status.isGranted) {
      // Tự động lấy vị trí đã sử dụng gần nhất
      getCurrentLocation();
    } else if (status.isDenied) {

    } else if (status.isPermanentlyDenied) {
      openAppSettings();
    }
  }
  Future<void> getAddressdeOnMapCallBack(LocationData locationData) async {
    await locationStorage.saveLocation(
        locationData.name , locationData.latitude, locationData.longitude, locationData.address );
    if (link != "") {
      Navigator.pop(context, true);
    } else {
      // Otherwise, go to the MainFoodPage
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (context) => MainFoodPage()),
      );
    }
  }
  // Lưu vị trí khách hàng chọn giao hàng và chuyển san màn hình Home Page
  Future<void> getAddressdelivery() async {
    await locationStorage.saveLocation(
        addressResult.name_address ?? "", position.latitude, position.longitude, addressResult.formatted_address ?? "");
    if (link != "") {
      Navigator.pop(context, true);
    } else {
      // Otherwise, go to the MainFoodPage
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (context) => MainFoodPage()),
      );
      // Navigator.popAndPushNamed(context,'/', result: true);
    }
  }

  // Hàm lấy vị trí hiện tại của người dùng
  Future<void> getCurrentLocation() async {
    try {
      isloadingdata = true;
      position = await Geolocator.getCurrentPosition(
        desiredAccuracy: LocationAccuracy.high,
      );
      // Lấy địa chỉ từ location người dùng
      addressResult = await googleApiService.fetchPlacesFromLocation(
          position.latitude, position.longitude);
      if (addressResult != null) {
        setState(() {
          _locationMessage = addressResult.formatted_address;
          isloadingdata = false;
        });
      }

    } catch (e) {
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
        AppConstants.APP_NAME,
        overflow: TextOverflow.ellipsis,
        maxLines: 1, // Số dòng tối đa hiển thị (có thể điều chỉnh theo nhu cầu của bạn)
        ),
        centerTitle: true, // Để căn giữa tiêu đề trên thanh AppBar
        // Các thuộc tính khác của AppBar
        backgroundColor: AppColors.mainColor, // Màu nền cho AppBar
      ),
      body: Container(
        padding: const EdgeInsets.all(10.0),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: <Widget>[
            Text(
              "Cho XpressEat biết vị trí của bạn để tài xế có thể giao hàng nhanh nhất",
              style: TextStyle(
                fontSize: Dimensions.font20,
                fontWeight: FontWeight.normal,
                color: Colors.grey,
                fontStyle: FontStyle.italic, // Đặt fontStyle thành italic
              ),
              textAlign: TextAlign.justify,
            ),
            SizedBox(height: Dimensions.height20),
            isloadingdata == false
                ? Container(
              child: Column(
                children: <Widget>[
                  Text(
                    "Bạn muốn sử dụng vị trí hiện tại không ?",
                    style: TextStyle(fontSize: Dimensions.font13,
                        fontWeight: FontWeight.w400,
                        color: Colors.black),
                  ),
                  Row(children: [
                    Icon(Icons.location_on,
                      size: 30,
                      color: Colors.redAccent,),
                    Flexible(
                      child: Padding(
                        padding: const EdgeInsets.all(2.0),
                        child: Text(
                          _locationMessage ?? "",
                          style: TextStyle(fontSize: Dimensions.font16,
                              fontWeight: FontWeight.bold),
                        ),
                      ),)

                  ],)
                ],
              ),
            )
                : Container(
              child: Column(
                children: <Widget>[
                  CircularProgressIndicator(color: AppColors.mainColor,),
                  SizedBox(height: Dimensions.height10),
                  // Khoảng cách giữa vòng tròn tải và văn bản
                  Text("Đang lấy vị trí hiện tại", style: TextStyle(fontSize: 9),),
                ],
              ),
            ),

            Flexible(
                child: Container(
                  margin: EdgeInsets.only(top: 10),
                  child: Column(
                    children: [
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
                        onPressed: isloadingdata ? null : () {
                          getAddressdelivery();
                        },
                        child: Text('Sử dụng vị trí hiện tại', style: TextStyle(fontSize: Dimensions.font16),),
                      ),
                      SizedBox(height: Dimensions.height5),
                      ElevatedButton(
                        style: ElevatedButton.styleFrom(
                          // Customize the background color
                          foregroundColor: Colors.white,
                          backgroundColor: AppColors.mainColor,
                          // Add other customizations as needed
                          padding: EdgeInsets.only(right: 20.0, left: 20.0),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(10.0),
                          ),
                          minimumSize: Size(double.infinity, 50), // Đặt kích thước tối thiểu cho nút
                        ),
                        onPressed: () async {
                          LocationData result = await Navigator.of(context).pushNamed("/getlocationFromMap") as LocationData;
                          if(result != null){
                            getAddressdeOnMapCallBack(result);
                          }
                        },
                        child: Text('Chọn từ bản đồ', style: TextStyle(fontSize: Dimensions.font16),),
                      ),
                    ],
                  ),
                )
            ),
            // Khoảng cách giữa phần tử tiếp theo và nút
          ],
        ),
      ),
    );
  }
}

