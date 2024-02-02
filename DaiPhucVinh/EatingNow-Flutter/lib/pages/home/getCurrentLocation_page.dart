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
import 'package:fluttertoast/fluttertoast.dart';
import '../circularprogress/DottedCircularProgressIndicator.dart';
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


  // Phương thức để load danh sách món ăn từ SharedPreferences
  void _loadCartItems() async {
    List<CartItem> loadedItems = await CartStorage.getCartItems();
    setState(() {
      cartItems = loadedItems;
    });
  }


  @override
  initState() {
    super.initState();
    initialization();
    requestLocationPermission();
    // Tự động lấy vị trí đã sử dụng gần nhất
    getCurrentLocation();
    locationStorage = LocationStorage();
    _loadCartItems();
  }

  // Danh sách các địa điểm đã lưu
  List<LocationData> savedLocations = [
    LocationData(
      name: 'Nhà',
      latitude: 10.3753666,
      longitude: 105.4378349,
      address: '9CGQ+45P, Phường Mỹ Xuyên, Thành phố Long Xuyên, An Giang, Việt Nam',
    ),
    LocationData(
      latitude: 10.3753666,
      longitude: 105.4378349,
      address: '418 Ung Văn Khiêm, Phường Đông Xuyên, Thành phố Long Xuyên, An Giang, Việt Nam',
      name: 'Trường',
    ),
    LocationData(
      latitude: 10.767714,
      longitude: 105.6547152,
      address: '30C Đ. Nguyễn Trường Tộ, p. Bình Khánh, Thành phố Long Xuyên, An Giang 08408, Việt Nam',
      name: 'Cf ngọc trai núi',
    )
  ];


  // Hàm xóa SplassCreen
  void initialization() async {
    FlutterNativeSplash.remove();
  }


  // yêu cầu quyền sử dụng vị trí của người dùng
  Future<void> requestLocationPermission() async {
    final status = await Permission.location.request();
    if (status.isGranted) {
      // Quyền truy cập vị trí đã được cấp.
    } else if (status.isDenied) {
      // Người dùng đã từ chối quyền truy cập vị trí, bạn có thể cung cấp hướng dẫn về việc cấp quyền trong ứng dụng của bạn.
    } else if (status.isPermanentlyDenied) {
      // Người dùng đã từ chối cấp quyền vĩnh viễn, bạn có thể chuyển hướng họ đến cài đặt ứng dụng để cấp quyền.
      openAppSettings();
    }
  }

  Future<void> getAddressdeliveryOnTap(LocationData locationData) async {
    await locationStorage.saveLocation(
        locationData.name, locationData.latitude, locationData.longitude, locationData.address);

    if (link != "") {
      // Chuyển đổi route tới link và truyền dữ liệu caritems
      Navigator.pushReplacement(
        context,
          Navigator.pushNamed(context, link!, arguments: {'data': cartItems }) as Route<Object?>
      );
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
      // Chuyển đổi route tới link và truyền dữ liệu caritems
      Navigator.pushReplacement(
          context,
          Navigator.pushNamed(context, link!, arguments: {'data': cartItems }) as Route<Object?>
      );
    } else {
      // Otherwise, go to the MainFoodPage
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (context) => MainFoodPage()),
      );
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
          isloadingdata = false;
        });
      }

    } catch (e) {
      // Fluttertoast.showToast(
      //     msg: "Đã có lỗi khi lấy vị trí của bạn. Vui lòng load lại ứng dụng.",
      //     toastLength: Toast.LENGTH_LONG,
      //     gravity: ToastGravity.TOP,
      //     backgroundColor: AppColors.toastSuccess,
      //     textColor: Colors.black54,
      //     timeInSecForIosWeb: 1,
      //     fontSize: 15);
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
          mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            Text(
              "Chọn vị trí chính xác nhất để tài xế có thể giao hàng nhanh và chính xác nhất",
              style: TextStyle(
                fontSize: Dimensions.font20,
                fontWeight: FontWeight.bold,
                color: Colors.grey,
                fontStyle: FontStyle.normal, // Đặt fontStyle thành italic
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
                      size: 33,
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
                  // CircularProgress đã chỉnh sửa theo ý muốn
                  DottedCircularProgressIndicator(
                    radius: 18.0,
                    color: Colors.orange,
                    dotRadius: 2.0,
                    numberOfDots: 9, // Số lượng dấu chấm
                  ),
                  SizedBox(height: Dimensions.height10),
                  // Khoảng cách giữa vòng tròn tải và văn bản
                  Text("Đang lấy vị trí hiện tại ...", style: TextStyle(fontSize: 9),),
                ],
              ),
            ),
            SizedBox(height: Dimensions.height45,),
            Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                // Để tiêu đề căn trái
                children: [
                  Card(
                    color: Colors.grey, // Màu nền của khung
                    elevation: 10, // Độ nâng của khung
                    child: Padding(
                      padding: EdgeInsets.fromLTRB(20.0, 0.0, 10.0, 2.0),
                      child: Text(
                        "DANH SÁCH VỊ TRÍ ĐÃ SỬ DỤNG",
                        style: TextStyle(fontSize: Dimensions.font16,
                            fontWeight: FontWeight.bold),
                      ),
                    ),
                  ),
                ]
            ),
            // LISTVIEW CÁC ĐIỂM ĐÃ LƯU
            Flexible(
                child: Container(
                  height: 170,
                  child: ListView.builder(
                    itemCount: savedLocations.length,
                    itemBuilder: (BuildContext context, int index) {
                      final locationData = savedLocations[index];
                      return Container(
                        child: ListTile(
                          leading: Icon(Icons.location_on,
                            size: 33,
                            color: Colors.redAccent,),
                          title: Text(
                            locationData.name + " | " + locationData.address,
                            style: TextStyle(fontSize: Dimensions.font16,
                                fontWeight: FontWeight.bold),
                            softWrap: true,
                            // Cho phép xuống dòng nếu văn bản quá dài
                            maxLines: 3, // Số dòng tối đa hiển thị (có thể điều chỉnh theo nhu cầu của bạn)
                          ),
                          onTap: ()  {
                            getAddressdeliveryOnTap(locationData);
                             },
                        ),
                      );
                    },
                  ),
                )
            )
            ,
            // SizedBox(height: Dimensions.height),
            Flexible(
                child: Container(
                  margin: EdgeInsets.only(top: Dimensions.bottomHeightBar),
                  child: Column(
                    children: [

                      ElevatedButton(
                        onPressed: isloadingdata ? null : () {
                          getAddressdelivery();
                        },
                        child: Text(
                          "Sử dụng vị trí hiện tại",
                          style: TextStyle(
                            color: Colors.black,
                            fontSize: Dimensions.font16,
                          ),
                        ),
                        style: ElevatedButton.styleFrom(
                          primary: AppColors.mainColor,
                          minimumSize: Size(double.infinity, 48),
                        ),
                      ),
                      SizedBox(height: Dimensions.height5),


                      // ElevatedButton(
                      //   onPressed: isloadingdata ? null : () {
                      //     changeOrAddNewAddressDelivery();
                      //   },
                      //   child: Text(
                      //     "Thêm hoặc thay đổi vị trí",
                      //     style: TextStyle(
                      //       color: Colors.black,
                      //       fontSize: Dimensions.font16,
                      //     ),
                      //   ),
                      //   style: ElevatedButton.styleFrom(
                      //     primary: AppColors.mainColor,
                      //     minimumSize: Size(double.infinity, 48),
                      //   ),
                      // )

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

