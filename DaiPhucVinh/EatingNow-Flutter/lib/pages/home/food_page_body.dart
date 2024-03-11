import 'dart:math';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:geolocator/geolocator.dart';
import 'package:intl/intl.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Icon_and_Text_widget.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/Widget/app_column.dart';
import 'package:fam/data/Api/CuisineService.dart';
import 'package:fam/data/Api/ProductService.dart';
import 'package:fam/models/cuisine_model.dart';
import 'package:fam/models/product_recommended_model.dart';
import 'package:fam/models/storenearUser.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:dots_indicator/dots_indicator.dart';
import '../../data/Api/StoreService.dart';
import '../../util/Colors.dart';
import '../../util/app_constants.dart';
import '../../util/dimensions.dart';
import '../circularprogress/DottedCircularProgressIndicator.dart';
import '../../storage/cartstorage.dart';



class FoodPageBody extends StatefulWidget {
  const FoodPageBody({Key? key}): super(key: key);

  @override
  State<FoodPageBody> createState() => _FoodPageBodyState();


}

class _FoodPageBodyState extends State<FoodPageBody> {
  FirebaseAuth _auth = FirebaseAuth.instance;
  final storeService = StoreService(apiUrl: AppConstants.TakeStoreByCuisineId);//lấy cửa hàng gần nhất
  final productService = ProductService(apiUrl: AppConstants.TakeRecommendedFoodList);// lấy món ăn được gợi ý
  final cuisineService = CuiSineService(apiUrl: AppConstants.TakeAllCuisine);// lấy danh sách loại hình món ăn
  late  SharedPreferences prefs;// khai báo dữ liệu localstore

  // Tạo một số ngẫu nhiên từ 2 đến 5
  Random random = Random();
  ProductRecommended? products;
  CuisineModel? cuisineData;
  StoreNearUserModel? storeNearUserModel;
  late  bool isloading = true;
  Offset? cartPosition;
  List<CartItem> cartItems = [];
  Future<void> fetchData() async {
    try {
      final productsDataFuture = productService.fectProductRecommended(
          { "CustomerId": _auth.currentUser?.uid ?? null,
            "Latitude":  prefs.getDouble('latitude') ?? 10.3792302,
            "Longittude":  prefs.getDouble('longitude') ?? 105.3872573
          });
      final cuisineDataFuture = cuisineService.fetchCuisineData(
          {
            "ItemCategoryCode": 0
          });
      final storeNearUserDataFuture = storeService.fetchStoreDataNearUser(
          {
            "CuisineId": 0,
            "latitude": prefs.getDouble('latitude') ?? 0.0,
            "longitude": prefs.getDouble('longitude') ?? 0.0
          });
      final results = await Future.wait([ productsDataFuture, cuisineDataFuture, storeNearUserDataFuture]);
      setState(() {
        isloading = false;
        products = results[0] as ProductRecommended?;
        cuisineData = results[1] as CuisineModel?;
        storeNearUserModel = results[2] as StoreNearUserModel?;
      });

    } catch (e) {
        print(e);
    }
  }
  PageController pageController= PageController(viewportFraction: 0.85);
  var _currPageValue=0.0;
  double _scaleFactor=0.8;
  double _height = Dimensions.pageViewContainer;
  NextListFoodPopular(){
  }
  void setPrefs() async {
    prefs = await SharedPreferences.getInstance();
    fetchData(); // Lấy dữ liệu các cửa hàng từ API Store
  }

  Future<double> calculateDistanceToStore(double storeLatitude, double storeLongitude) async {
    double distanceInMeters = 0;
    try {
      distanceInMeters = await Geolocator.distanceBetween(
          prefs.getDouble('latitude') ?? 10.3792302, prefs.getDouble('longitude') ?? 105.3872573, storeLatitude, storeLongitude);
      print(prefs.getDouble('latitude'));
      print(prefs.getDouble('longitude'));

    } catch (e) {
      // Xử lý lỗi nếu có
      print("Lỗi khi tính toán khoảng cách: $e");
    }
    return distanceInMeters;
  }

  @override
  initState(){
    super.initState();
    setPrefs();
    pageController.addListener(() {
      setState(() {
        _currPageValue = pageController.page!;
      });
    });
    _loadCartItems();
  }
  void _loadCartItems() async {
    List<CartItem> loadedItems = await CartStorage.getCartItems();
    setState(() {
      cartItems = loadedItems;
    });
  }
  @override
  void dispose(){
    pageController.dispose();
  }
  @override
  Widget build(BuildContext context) {
    return
        RefreshIndicator(
          color: AppColors.iconColor1,
          onRefresh: () async {
            fetchData();
          },
          child:
          SingleChildScrollView(
            child:
              isloading || products == null || products!.data == null || products!.data!.length == 0
                  ? Container(
                height: MediaQuery.of(context).size.height, // Đặt chiều cao bằng chiều cao của thiết bị
                width: MediaQuery.of(context).size.width, // Đặt chiều rộng bằng chiều rộng của thiết bị
                alignment: Alignment.center,
                child: Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      DottedCircularProgressIndicator(
                        radius: 28.0,
                        color: Colors.orange,
                        dotRadius: 3.0,
                        numberOfDots: 10,
                      ),
                    ],
                  ),
                ),
              ) :
              Column(
                children: [
                  _headerContainer(AppConstants.APP_NAME, "Loại món ăn 🍔"),
                  buldCatagoryItem(),
                  _line(),
                  // Kiểm tra isLoading để hiển thị "Loading" hoặc nội dung của PageView.
                  _headerContainer("Các cửa hàng gần nhất", "⚡"),
                  Container(
                    height: Dimensions.pageView,
                    child: PageView.builder(
                      controller: pageController,
                      itemCount: storeNearUserModel?.data!.take(5).length ?? 0,
                      itemBuilder: (context, position) {
                        final item = storeNearUserModel?.data?[position];
                        return _buildPageItem(position, item);
                      },
                    ),
                  ),
                  DotsIndicator(
                    dotsCount: storeNearUserModel?.data?.take(5).length ?? 1,//độ dài cửa hàng đề cử
                    position: _currPageValue,
                    decorator: DotsDecorator(
                      activeColor: AppColors.mainColor,
                      size: const Size.square(9.0),
                      activeSize: const Size(18.0, 9.0),
                      activeShape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(5.0)),
                    ),
                  ),
                  _line(),
                  _headerContainer("Gợi ý", "Món ngon cho bạn 🧡"),
                  // Danh sách các món ăn yêu thích của khách hàng
                  SingleChildScrollView(
                    scrollDirection: Axis.horizontal,
                    child: Row(
                      children: products!.data!.take(10).map((data) {
                        return GestureDetector(
                          onTap: () {
                            if(data.qtycontrolled == true && data.qty! > 0 ||  data.qtycontrolled == false )
                            {
                              // Điều hướng đến trang chi tiết tại đây
                              // Chuyển đổi route tới link và truyền dữ liệu caritems
                              Navigator.pushReplacement(
                                  context,
                                  Navigator.pushNamed(context, "/productdetail", arguments: {'data': data }) as Route<Object?>
                              );
                            }
                          },
                          child:
                          Container(
                            margin: EdgeInsets.only(left: Dimensions.width5, right: Dimensions.width5, bottom: Dimensions.height10),
                            width: 180, // Đặt chiều rộng của mỗi phần tử
                            height: 250,
                            decoration: BoxDecoration(
                              borderRadius: BorderRadius.circular(Dimensions.radius20),
                              color: Colors.white, // Màu nền của phần tử
                              boxShadow: [
                                BoxShadow(
                                  color: Colors.grey.withOpacity(0.2), // Màu bóng đổ
                                  spreadRadius: 1, // Bán kính bóng đổ
                                  blurRadius: 2, // Độ mờ của bóng đổ
                                  offset: Offset(0, 3), // Độ dịch chuyển của bóng đổ
                                ),
                              ],
                            ),
                            child: Stack(
                              children: [
                                Column(
                                  crossAxisAlignment: CrossAxisAlignment.center,
                                  children: [
                                    // Hình ảnh
                                    Container(
                                      height: 140,
                                      decoration: BoxDecoration(
                                        borderRadius: BorderRadius.only(
                                          topLeft: Radius.circular(Dimensions.radius20),
                                          topRight: Radius.circular(Dimensions.radius20),
                                          bottomRight: Radius.circular(Dimensions.radius20),
                                          bottomLeft: Radius.circular(Dimensions.radius20),
                                        ),
                                        image: DecorationImage(
                                            fit: BoxFit.fitWidth,
                                            image:
                                            NetworkImage(data?.uploadImage ?? "https://cdn-icons-png.flaticon.com/128/2276/2276931.png")
                                        ),
                                      ),
                                    ),
                                    Padding(
                                      padding: EdgeInsets.all(0),
                                      child: Column(
                                        crossAxisAlignment: CrossAxisAlignment.center,
                                        children: [
                                          BigText(text: (data?.foodName ?? ""), size: Dimensions.font16, color: AppColors.signColor,maxlines: 1,),
                                          SmallText(text: (data?.storeName ?? ""), size: Dimensions.font13, color: AppColors.paraColor,),
                                          Container(
                                            margin: EdgeInsets.only(left: Dimensions.width10),
                                            child: Text(
                                              NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(data?.price ?? 0),
                                              style: TextStyle(
                                                color: Colors.black,
                                                fontSize: Dimensions.font13,
                                                fontFamily: 'Roboto',
                                                fontWeight: FontWeight.w400,
                                              ),
                                            ),
                                          ),
                                          FutureBuilder<double>(
                                            future: calculateDistanceToStore(data.latitude ?? 10.3792302, data.longitude ?? 105.3872573),
                                            builder: (context, snapshot) {
                                              if (snapshot.connectionState == ConnectionState.waiting) {
                                                return SizedBox(); // Show a loading indicator while waiting for the result
                                              } else if (snapshot.hasError) {
                                                return Text("Error: ${snapshot.error}");
                                              } else {
                                                final km = (snapshot.data! / 1000).toStringAsFixed(1); // Convert meters to kilometers
                                                final minite = (double.parse(km) * 60)/ 35;
                                                return Row(
                                                  mainAxisAlignment: MainAxisAlignment.spaceAround ,
                                                  children: [
                                                    IconAndTextWidget(
                                                      icon: Icons.location_on,
                                                      text: km+ " km",
                                                      iconColor: AppColors.mainColor,
                                                    ),
                                                    IconAndTextWidget(
                                                      icon: Icons.access_time_rounded,
                                                      text: minite.toStringAsFixed(1) + " phút",
                                                      iconColor: AppColors.iconColor2,
                                                    ),
                                                  ],
                                                );
                                              }
                                            },
                                          ),
                                        ],
                                      ),
                                    ),
                                  ],
                                ),
                                Positioned(
                                  top: 0,
                                  left: 0,
                                  child:  Visibility(
                                    visible: data?.qty == 0 && data?.qtycontrolled == true,
                                      child: Container(
                                        width: 180,
                                        height: 140,
                                        decoration: BoxDecoration(
                                          borderRadius: BorderRadius.circular(Dimensions.radius20),
                                          color: Colors.grey.withOpacity(0.5), // Mờ đi một chút
                                        ),
                                        child: Center(
                                          child: Text(
                                            "Hết số lượng",
                                            style: TextStyle(
                                              color: Colors.white,
                                              fontSize: 16.0,
                                              fontWeight: FontWeight.bold,
                                            ),
                                          ),
                                        ),
                                      ),
                                  ),
                                )
                              ],
                            ),
                          ),
                        );
                      }).toList(),
                    ),

                  ),
                  // Đường kẻ ngang
                  _line(),
                  // SizedBox(height: Dimensions.height30,),
                  _headerContainer("Phổ biến", "Các món ăn đang HOT 🔥"),
                  //Danh sách các món ăn đang phổ biến
                  ListView.builder(
                      physics: NeverScrollableScrollPhysics(),
                      shrinkWrap: true,
                      itemCount: products!.data!.length,
                      itemBuilder: (context, index) {
                        final product = products?.data![index];
                        return
                          GestureDetector(
                              onTap: () {
                                if(product.qtycontrolled == true && product.qty! > 0 ||  product.qtycontrolled == false )
                                 {
                                   Navigator.pushReplacement(
                                       context,
                                       Navigator.pushNamed(context, "/productdetail", arguments: {'data': product }) as Route<Object?>
                                   );
                                 }
                              },
                              child:
                              Container(
                                margin: EdgeInsets.only(left: Dimensions.width20, right: Dimensions.width20, bottom: Dimensions.height10),
                                child:  Stack(
                                  children: [
                                    Row(
                                      children: [
                                        Container(
                                          width: Dimensions.listViewImgSize,
                                          height: Dimensions.listViewImgSize,
                                          decoration: BoxDecoration(
                                            borderRadius: BorderRadius.circular(Dimensions.radius20),
                                            color:  (index % 2 == 0) ? Colors.orange[50] : Colors.amber[100], // Bạn có thể thay thế bằng widget Image.network
                                            image: DecorationImage(
                                              fit: BoxFit.cover,
                                              image: NetworkImage(product?.uploadImage ?? ""),
                                            ),
                                          ),
                                        ),
                                        // Phần chứa văn bản
                                        Expanded(
                                          child: Container(
                                            height: Dimensions.listViewTextContSize,
                                            width: Dimensions.listViewTextContSize,
                                            decoration: BoxDecoration(
                                              borderRadius: BorderRadius.only(
                                                topRight: Radius.circular(Dimensions.radius20),
                                                bottomRight: Radius.circular(Dimensions.radius20),
                                              ),
                                              color: Colors.white,
                                            ),
                                            child: Padding(
                                              padding: EdgeInsets.only(left: Dimensions.width10, right: Dimensions.width10),
                                              child: Column(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                mainAxisAlignment: MainAxisAlignment.center,
                                                children: [
                                                  BigText(text: (product?.foodName ?? ""), size: Dimensions.font16, color: AppColors.signColor,),
                                                  SmallText(text: (product?.storeName ?? ""), size: Dimensions.font13, color: AppColors.paraColor,),
                                                  Row(
                                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                                    children: [
                                                      SmallText(text: NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(product?.price ?? 0),), // Thay thế bằng thuộc tính tương ứng
                                                      FutureBuilder<double>(
                                                        future: calculateDistanceToStore(product!.latitude ?? 10.323233, product!.longitude ?? 105.1727172),
                                                        builder: (context, snapshot) {
                                                          if (snapshot.connectionState == ConnectionState.waiting) {
                                                            return CircularProgressIndicator(); // Show a loading indicator while waiting for the result
                                                          } else if (snapshot.hasError) {
                                                            return Text("Error: ${snapshot.error}");
                                                          } else {
                                                            final km = (snapshot.data! / 1000).toStringAsFixed(1); // Convert meters to kilometers
                                                            final minite = (double.parse(km) * 60) / 35;
                                                            return Row(
                                                              mainAxisAlignment: MainAxisAlignment.end,
                                                              children: [
                                                                IconAndTextWidget(
                                                                  icon: Icons.access_time_rounded,
                                                                  text: minite.toStringAsFixed(1) + " phút",
                                                                  iconColor: AppColors.mainColor,),
                                                                IconAndTextWidget(
                                                                  icon: Icons.location_on,
                                                                  text: km + " km",
                                                                  iconColor: AppColors.iconColor2,
                                                                ),
                                                              ],
                                                            );
                                                          }
                                                        },
                                                      )
                                                    ],
                                                  )
                                                ],
                                              ),
                                            ),
                                          ),
                                        ),
                                        // Hiển thị thông báo "Hết số lượng" khi qty == 0 và qtycontroled == true
                                      ],
                                    ),
                                    Positioned(
                                      top: 0,
                                      left: 0,
                                      child:  Visibility(
                                        visible: product?.qty == 0 && product?.qtycontrolled == true,
                                          child: Container(
                                            width: Dimensions.listViewImgSize,
                                            height: Dimensions.listViewImgSize,
                                            decoration: BoxDecoration(
                                              borderRadius: BorderRadius.circular(Dimensions.radius20),
                                              color: Colors.grey.withOpacity(0.5), // Mờ đi một chút
                                            ),
                                            child: Center(
                                              child: Text(
                                                "Hết số lượng",
                                                style: TextStyle(
                                                  color: Colors.white,
                                                  fontSize: 16.0,
                                                  fontWeight: FontWeight.bold,
                                                ),
                                              ),
                                            ),
                                          ),
                                        ),
                                      ),
                                  ],
                                ),
                              ),
                          );


                      }),
                ],
              )
          ),
        );
  }
  Container _line(){
    return
    Container(
      margin: EdgeInsets.only(top: Dimensions.height5, bottom:  Dimensions.height5),
      width: MediaQuery.of(context).size.width, // Đặt chiều rộng bằng chiều rộng của thiết bị
      height: 5, // Đặt chiều cao của đường line
      color: Colors.white, // Màu xám
    );
  }
  Container _headerContainer(String text, String text2){
    return
      Container(
        margin: EdgeInsets.only(left: Dimensions.width30, bottom:  Dimensions.height5),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.end,
          children: [
            BigText(text: text),
            Container(
              margin: const EdgeInsets.only(bottom: 3),
              child: BigText(text: ".", color: Colors.black26,),
            ),
            SizedBox(width: Dimensions.width10,),
            Container(
              margin: const EdgeInsets.only(bottom: 2),
              child: SmallText(text: text2,),
            ),
            Expanded( // Sử dụng Expanded để Icon luôn nằm ở cuối
              child: SizedBox(),
            ),
            Icon(Icons.navigate_next,
              size: 35,
              color: Colors.black54,),
          ],
        ),
      );

  }

  Container buldCatagoryItem(){
    return !isloading ?
      Container(
        margin: EdgeInsets.only(left: Dimensions.width10, right: Dimensions.width10 ),
      height: 100,
      child: ListView.separated(
        itemCount: cuisineData!.data!.length ,
        scrollDirection: Axis.horizontal,
        separatorBuilder: (context, index)=>
          SizedBox(width: 5,),
        itemBuilder: (context, position){
          return GestureDetector(
              onTap: (){
                Navigator.of(context).pushNamed("/searchpage", arguments: {"data": cuisineData , "IdSelected": cuisineData!.data![position].cuisineId ?? 0 });
              },
              child: Container(
            margin: EdgeInsets.only(left: 2),
            height: 50,
            width: 100,
            decoration: BoxDecoration(
              color: (position % 2 == 0) ? Colors.orange[50] : Colors.amber[100], // Chọn màu sắc dựa trên điều kiện
              borderRadius: BorderRadius.circular(15),
            ),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                Container(
                  width: 80,
                  height: 80,
                  decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(Dimensions.radius20),
                      color: Colors.transparent,
                      image: DecorationImage(
                        fit: BoxFit.fitWidth,
                        image: //AssetImage("assets/image/logoEN.png"),
                        NetworkImage(cuisineData!.data![position].absoluteImage ?? ""),
                      )
                  ),

                ),

                (cuisineData != null && cuisineData!.data != null && position >= 0 && position < cuisineData!.data!.length) ?


                Text(cuisineData!.data![position].name ?? " ",
                  style: TextStyle(fontSize: 12, color: AppColors.paraColor),
                  overflow: TextOverflow.ellipsis,
                  // Sẽ hiển thị dấu ba chấm (...) nếu văn bản quá dài
                  maxLines: 1, // Số dòng tối đa hiển thị (có thể điều chỉnh theo nhu cầu của bạn)
                )
                    :
                Text("Invalid Data or Position",
                  overflow: TextOverflow.ellipsis,
                  // Sẽ hiển thị dấu ba chấm (...) nếu văn bản quá dài
                  maxLines: 1, // Số dòng tối đa hiển thị (có thể điều chỉnh theo nhu cầu của bạn)
                ),
              ],
            ),
          ));
        }
      ),
    ):(
      Container(child:
          Column(
            children: [
            DottedCircularProgressIndicator(
            radius: 30.0,
            color: Colors.orange,
            dotRadius: 3.0,
            numberOfDots: 10,
          ),
          Text("Đang tải dữ liệu..."),
            ],
          )
      )
    );
  }
  //Các cửa hàng gần nhất
  Widget _buildPageItem(int index,DataStoreNearUserModel? popularProduct){
    Matrix4 matrix = new Matrix4.identity();
    if(index==_currPageValue.floor()){
      var currScale = 1-(_currPageValue- index)*(1-_scaleFactor);
      var currTrans = _height*(1-currScale)/2;
      matrix = Matrix4.diagonal3Values(1, currScale, 1)..setTranslationRaw(0,currTrans,0);

    }
    else if(index == _currPageValue.floor()+1){
      var currScale = _scaleFactor+(_currPageValue-index+1)*(1-_scaleFactor);
      var currTrans = _height*(1-currScale)/2;
      matrix = Matrix4.diagonal3Values(1, currScale, 1);
      matrix = Matrix4.diagonal3Values(1, currScale, 1)..setTranslationRaw(0,currTrans,0);

    }
    else if(index == _currPageValue.floor()-1) {
      var currScale = 1-(_currPageValue- index)*(1-_scaleFactor);
      var currTrans = _height * (1 - currScale) / 2;
      matrix = Matrix4.diagonal3Values(1, currScale, 1);
      matrix = Matrix4.diagonal3Values(1, currScale, 1)..setTranslationRaw(0, currTrans, 0);
    }
    else {
      var currScale = 0.8;
      matrix = Matrix4.diagonal3Values(1, currScale, 1)..setTranslationRaw(0, _height*(1-_scaleFactor)/2, 1);

    }

    return GestureDetector(
      onTap: (){
        Navigator.of(context).pushNamed("/storedetail", arguments: {'data': popularProduct });
      },
      child: Transform(
        transform: matrix,
        child: Stack(
          children: [
            Container(
              height: Dimensions.pageViewContainer,
              margin: EdgeInsets.only(left: Dimensions.width10, right: Dimensions.width10),
              decoration: BoxDecoration(
                  borderRadius: BorderRadius.circular(Dimensions.radius30),
                  color:index.isEven?Color(0xFF69c5df):Color(0xFF9294cc),
                  image: DecorationImage(
                      fit: BoxFit.cover,
                      image: NetworkImage(
                          popularProduct?.absoluteImage ?? "https://cdn-icons-png.flaticon.com/128/869/869636.png"
                      )
                  )
              ),
            ),
            Align(
              alignment: Alignment.bottomCenter,
              child: Container(
                  height: Dimensions.pageViewTextContainer,
                  margin: EdgeInsets.only(left: Dimensions.width30, right: Dimensions.width30,bottom: Dimensions.height30),
                  decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(Dimensions.radius15),
                      color: Colors.white,
                      boxShadow: [
                        BoxShadow(
                          color: Color(0xFFe8e8e8),
                          blurRadius: 5.0,
                          offset: Offset(0,5),
                        ),
                        BoxShadow(
                          color: Colors.white,
                          offset: Offset(-5,0),
                        ),
                        BoxShadow(
                          color: Colors.white,
                          offset: Offset(5,0),
                        )
                      ]
                  ),
                  // Tên cửa hàng
                  child: Container(
                    padding: EdgeInsets.only(top: Dimensions.height5,left: Dimensions.height5, right: Dimensions.height5),
                    child:
                    AppColumn(text:popularProduct?.fullName ?? "", rating: random.nextInt(4) + 2, latitude: popularProduct?.latitude?? 0.0, longtitude: popularProduct?.longitude?? 0.0,time: popularProduct?.time?? 0.0, prefs: prefs,),
                    // Sử dụng widget RatingStars để hiển thị số sao dựa trên tỉ lệ đánh giá
                  )
              ),

            ),
          ],
        ),
      )
    );
  }
}












