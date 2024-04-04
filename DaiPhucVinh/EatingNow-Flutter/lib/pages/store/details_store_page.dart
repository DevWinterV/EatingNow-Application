import 'dart:async';
import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/data/Api/CategoryService.dart';
import 'package:fam/data/Api/StoreService.dart';
import 'package:fam/models/category_model.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/dimensions.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:geolocator/geolocator.dart';
import 'package:intl/intl.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:tap_debouncer/tap_debouncer.dart';
import 'package:url_launcher/url_launcher.dart';
import '../../Widget/Icon_and_Text_widget.dart';
import '../../Widget/RatingStars.dart';
import '../../data/Api/GoogleAPIService.dart';
import '../../models/product_recommended_model.dart';
import '../../models/storenearUser.dart';
import '../../util/app_constants.dart';

class StoreDetailPage extends StatefulWidget {
  const StoreDetailPage({Key? key}) : super(key: key);

  @override
  _StoreDetailState createState() => _StoreDetailState();
}

class _StoreDetailState extends State<StoreDetailPage> {
  StreamController<DataStoreNearUserModel?> _streamDataStore = StreamController<DataStoreNearUserModel?>.broadcast();
  StreamController<int?> _selectedCategoryController = StreamController<int?>.broadcast();
  final streamListProduct = StreamController<List<DataProduct>?>();
  late DataStoreNearUserModel? storeData;
  final categoryService = CategoryService(apiUrl: AppConstants.TakeCategoryByStoreId);//lấy cửa hàng gần nhất
  final storeServiceFoodListAll = StoreService(apiUrl: AppConstants.TakeAllFoodListByStoreId);// lấy danh sách tất cả món ăn
  final storeServiceFoodList = StoreService(apiUrl: AppConstants.TakeFoodListByStoreId);// lấy danh sách tất cả món ăn
  CategoryModel? categoryModel;
  ProductRecommended? productRecommended;
  int categoryId = 0; // Mặc định là tất cả

  Future<void> fetchData(int Id) async {
    try {
      final results = await Future.wait([
        Id == 0
            ? storeServiceFoodListAll.TakeAllFoodListByStoreId({
          "ExpiryDate": 2,
          "Id": storeData?.userId ?? 0,
          "Qtycontrolled": 2,
          "QuantitySupplied": 2,
          "TimeExpiryDate": 2,
          "keyWord": ""
        })
            : storeServiceFoodList.TakeFoodListByStoreId(Id)
      ]);
      streamListProduct.add(results[0]!.data ?? []);
    } catch (e) {
    }
  }

  Future<DistanceAndTime?> calculateDistanceAndTime(String end) async {
    try {
      final prefss = await SharedPreferences.getInstance();
      String start = prefss.getDouble('latitude').toString()+','+ prefss.getDouble('longitude').toString();
      final results = await GoogleAPIService('AIzaSyDeFN4A3eenCTIUYvCI7dViF-N-V5X8RgA').calculateDistanceAndTime(start, end);
      if(results != null){
        return results;
      }
    } catch (e) {
      // Xử lý lỗi nếu có
      print("Lỗi khi tính toán khoảng cách: $e");
    }
    return null;
  }

  Future<CategoryModel?> TakeListCategory(int? userId) async {
    try {
      final results = await categoryService.TakeCategoryByStoreId(userId ?? 0);
      if(results!.success == true){
        return results;
      }
      return null;
    } catch (e) {
      print(e);
    }
  }

  void _launchPhoneCall(String phoneNumber) async {
    String url = 'tel:$phoneNumber';
    if (await canLaunch(url)) {
      await launch(url);
    } else {
      // Handle error, e.g., show an error message
      print('Could not launch $url');
    }
  }

  @override
  void initState() {
    super.initState();
    // Defer the execution using Future.delayed
    Future.delayed(Duration.zero, () {
      fetchData(0); // mặc định là tất cả
    });
  }

  void didChangeDependencies() async {
    super.didChangeDependencies();
    final arguments = ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
    if(arguments['data'] is DataStoreNearUserModel){
      storeData = arguments['data'] as DataStoreNearUserModel; // Nhận dữ liệu store
      Future.delayed(Duration.zero, () {
        _streamDataStore.sink.add(storeData);
      });
    }
    else
    {
      final response = await StoreService(apiUrl: AppConstants.TakeStoreById).TakeStoreById(arguments['data'] as int);
      if(response != null && response.success == true || response?.item != null){
        storeData = response?.item?.toDataStoreNearUserModel(); // Nhận dữ liệu store
        _streamDataStore.sink.add(storeData);
        fetchData(0); // mặc định là tất cả
      }
      else{
        storeData = null;
        _streamDataStore.sink.add(null);
      }
    }
  }


  @override
  void dispose(){
    super.dispose();
  }
  @override
  Widget build(BuildContext context) {
    return
      Stack(
        children: [
              Scaffold(
              backgroundColor: Colors.white,
              body:
               StreamBuilder(
                  // initialData: null,
                  stream: _streamDataStore.stream,
                  builder: (builder,snapshot){
                    if(snapshot.connectionState == ConnectionState.waiting){
                      return Center(
                        child: CircularProgressIndicator(color: AppColors.mainColor,),
                      );
                    }
                    if(snapshot.data == null){
                      return Center(
                        child: SmallText(text: 'Không có dữ liệu',),
                      );
                    }

                    if(snapshot.data != null && snapshot.hasData ){
                      return CustomScrollView(
                        slivers: [
                          SliverAppBar(
                            centerTitle: true,
                            title:Center(
                              child: Padding(
                                  padding: EdgeInsets.all(0),
                                  child: Text(
                                    snapshot.data?.fullName ?? "",
                                    maxLines: 1,
                                    style:
                                    TextStyle(
                                        color: Colors.white,
                                        fontSize: 20,
                                        fontWeight: FontWeight.bold
                                    ),
                                  )
                              ),
                            ),
                            toolbarHeight: 70,
                            actions: [
                              Container(
                                  width:Dimensions.height40,
                                  height:Dimensions.height40,
                                  child:Icon(
                                    Icons.search,
                                    color:Colors.white,
                                    size: Dimensions.iconSize16,),
                                  decoration: BoxDecoration(
                                    borderRadius: BorderRadius.circular(Dimensions.radius15),
                                    color: AppColors.mainColor,
                                  )
                              ),
                              // Container(
                              //   decoration: BoxDecoration(
                              //     shape: BoxShape.circle,
                              //     color: AppColors.mainColor,
                              //   ),
                              //   child:  Icon(
                              //       Icons.search_outlined,
                              //       color: Colors.white,
                              //       size: 22,
                              //   )),
                              SizedBox(width: 13,)
                            ],
                            bottom: PreferredSize(
                              preferredSize: Size.fromHeight(100),
                              // THONG TIN CỬA HÀNG
                              child: Container(
                                child: Center(
                                    child: Column(
                                      mainAxisAlignment: MainAxisAlignment.center,
                                      children: [
                                        SizedBox(height: 5,),
                                        // TEN CUA HANG
                                        // Padding(padding: EdgeInsets.all(15),child: BigText(
                                        //     size: Dimensions.font23, text: storeData!.fullName ?? ""),
                                        // ),
                                        Padding(
                                          padding: EdgeInsets.only(right: 20, left: 20, top: 5, bottom: 10),
                                          child: Column(
                                            mainAxisAlignment: MainAxisAlignment.start,
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: [
                                              // DANH GIA, KHOANG CÁCH, THOI GIAN
                                              FutureBuilder<DistanceAndTime?>(
                                                  future: calculateDistanceAndTime(snapshot.data!.latitude.toString() +","+snapshot.data!.longitude.toString()),
                                                  builder: (context, snapshot) {
                                                    if (snapshot.connectionState == ConnectionState.waiting) {
                                                      return SizedBox(); // Show a loading indicator while waiting for the result
                                                    }
                                                    else if (snapshot.hasError) {
                                                      return SizedBox();
                                                    }
                                                    else
                                                    {
                                                      return Row(
                                                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                                        children: [
                                                          // Đánh giá sao cửa hàng
                                                          RatingStars(
                                                            rating: 4.5,
                                                            starSize: 15,
                                                            starColor: AppColors.yellowColor,
                                                            emptyStarColor: Colors.grey,
                                                          ),
                                                          // Khoảng cách
                                                          IconAndTextWidget(
                                                            icon: Icons.location_on,
                                                            text: snapshot.data?.distance ?? " ",
                                                            iconColor: AppColors.mainColor,),
                                                          // Thời gian
                                                          IconAndTextWidget(
                                                            icon: Icons.access_time_rounded,
                                                            text: snapshot.data?.time ?? " ",
                                                            iconColor: AppColors.mainColor,),
                                                        ],
                                                      );
                                                    }
                                                    return SizedBox();
                                                  }
                                              ),
                                              // THOG TIN DIA CHI - SO DIEN THOAI
                                              BigText(text: "Địa chỉ: ${snapshot.data!.address}", maxlines: 2, size: 14, color: AppColors.paraColor,),
                                              InkWell(
                                                  onTap: () {
                                                    _launchPhoneCall(snapshot.data!.phone ?? "");
                                                  },
                                                  child: Row(
                                                    children: [
                                                      BigText(
                                                        text: "Điện thoại: ${snapshot.data!.phone}",
                                                        maxlines: 2,
                                                        size: 14,
                                                        color: AppColors.paraColor,
                                                      ),
                                                      SizedBox(width: 2,),
                                                      Icon(
                                                        Icons.phone,
                                                        size: 15,
                                                      )
                                                    ],
                                                  )
                                              ),
                                              Row(
                                                children: [

                                                ],
                                              )
                                            ],),
                                        )
                                      ],
                                    )
                                ),
                                width: double.maxFinite,
                                padding: EdgeInsets.only(top: 5, bottom: 10),
                                decoration: BoxDecoration(
                                    color: Colors.white,
                                    borderRadius: BorderRadius.only(
                                      topLeft: Radius.circular(Dimensions.radius15),
                                      topRight: Radius.circular(Dimensions.radius15),
                                    )),
                              ),
                            ),
                            pinned: true,
                            backgroundColor: Colors.white,
                            expandedHeight: 300,
                            //hÌNH ẢNH CỬA HÀNG
                            flexibleSpace: FlexibleSpaceBar(
                              background: snapshot.data!.absoluteImage != null
                                  ? Image.network(
                                snapshot.data!.absoluteImage ?? "",
                                width: double.maxFinite,
                                fit: BoxFit.cover,
                              )
                                  : Image.asset(
                                "assets/image/logoEN.png",
                                width: double.maxFinite,
                                fit: BoxFit.cover,
                              ),
                            ),
                          ),
                          // MENU CUA HANG
                          SliverToBoxAdapter(
                              child: Padding(
                                padding: EdgeInsets.only(top: 0, left: 10, right: 10, bottom: 0),
                                child: Column(
                                  mainAxisAlignment: MainAxisAlignment.center,
                                  children: [
                                    // BigText(text: "Menu của quán", color: AppColors.mainColor, size: Dimensions.font23,),
                                    Container(
                                      decoration: BoxDecoration(
                                        color: AppColors.mainColor,
                                        borderRadius: BorderRadius.circular(10), // Adjust the radius as needed
                                      ),
                                      height: 35, // Set a fixed height for horizontal ListView
                                      child:
                                      FutureBuilder<CategoryModel?>(
                                        future: TakeListCategory(snapshot.data!.userId ?? 0),
                                        builder: (BuildContext context, AsyncSnapshot<CategoryModel?> snapshot) {
                                          if(snapshot.connectionState == ConnectionState.waiting){
                                            return SizedBox();
                                          }
                                          if(snapshot.data!.data!.length > 0 || snapshot.hasData){
                                            return StreamBuilder<int?>(
                                              stream: _selectedCategoryController.stream,
                                              initialData: null,
                                              builder: (context, selectedSnapshot) {
                                                // Danh loại món ăn
                                                return ListView.separated(
                                                  itemCount: snapshot.data!.data!.length + 1, // Add 1 for the "All" item
                                                  scrollDirection: Axis.horizontal,
                                                  separatorBuilder: (context, index) => SizedBox(width: 0),
                                                  itemBuilder: (context, index) {
                                                    if (index == 0) {
                                                      // Handle the "All" item separately
                                                      return buildCategoryItem(0, "Tất cả", selectedSnapshot.data == 0);
                                                    } else {
                                                      // Handle other items in your data
                                                      final item = snapshot.data!.data![index - 1]; // Adjust index for data
                                                      return buildCategoryItem(item.categoryId ?? 0, item.categoryName ?? "", selectedSnapshot.data == item.categoryId);
                                                    }
                                                  },
                                                );
                                              },
                                            );
                                          }
                                          return Padding(
                                            padding: EdgeInsets.only(left: 10, right: 10, top: 3, bottom: 3),
                                            child: Container(
                                                decoration: BoxDecoration(
                                                  color: AppColors.mainColor,
                                                  borderRadius: BorderRadius.circular(5), // Adjust the radius as needed
                                                ),
                                                child: Text("Cửa hàng hiện chưa có nhóm sản phẩm ...", )
                                            ),);
                                        },
                                      ),
                                    ),
                                    Container(
                                      height: MediaQuery.of(context).size.height - 400,
                                      child:
                                      StreamBuilder<List<DataProduct>?>(
                                        stream: streamListProduct.stream,
                                        builder: (context, snapshot) {
                                          if(snapshot.connectionState == ConnectionState.waiting){
                                            return Center(child: CircularProgressIndicator(color: AppColors.mainColor,),);
                                          }
                                          if(!snapshot.hasData || snapshot.data?.length == 0 ){
                                            return Center(
                                                child:
                                                Column(
                                                  mainAxisAlignment: MainAxisAlignment.center,
                                                  children: [
                                                    Image.asset(
                                                      "assets/image/empty-box.png",
                                                      height: 100,
                                                      width: 100,),
                                                    BigText(text: "Cửa hàng hiện chưa có sản phẩm kinh doanh", color: Colors.grey,),
                                                  ],
                                                )
                                            );
                                          }
                                          if (snapshot.hasData) {
                                            return ListView.builder(
                                              itemCount: snapshot.data?.length ?? 0,
                                              scrollDirection: Axis.vertical,
                                              itemBuilder: (BuildContext context, int index) {
                                                final item = snapshot.data![index];
                                                return GestureDetector(
                                                    onTap: (){
                                                      if(item.qtycontrolled == true && item.qty! > 0 ||  item.qtycontrolled == false )
                                                      {
                                                        Navigator.pushReplacement(
                                                            context,
                                                            Navigator.pushNamed(context, "/productdetail", arguments: {'data': item }) as Route<Object?>
                                                        );
                                                      }
                                                    },
                                                    child:
                                                    Stack(
                                                      children: [
                                                        Column(
                                                          children: [
                                                            ListTile(
                                                                leading: Image.network(item.uploadImage ?? "", height: 80, width: 80,),
                                                                title: BigText(text: item.foodName ?? "", size: 15,),
                                                                subtitle: SmallText(text: "Đơn giá: " +NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(item?.price ?? 0),
                                                                  size: 13,
                                                                )
                                                            ),
                                                            Divider(
                                                              thickness: 1, // Adjust the thickness of the divider as needed
                                                              color: Colors.black26, // Set the color of the divider
                                                            ),
                                                          ],
                                                        ),
                                                        Positioned(
                                                          top: 0,
                                                          left: 0,
                                                          child:  Visibility(
                                                            visible: item?.qty == 0 && item?.qtycontrolled == true,
                                                            child: Container(
                                                              width: 110,
                                                              height: 76,
                                                              decoration: BoxDecoration(
                                                                color: Colors.grey.withOpacity(0.5), // Mờ đi một chút
                                                              ),
                                                              child: Center(
                                                                child: Text(
                                                                  "Hết số lượng",
                                                                  style: TextStyle(
                                                                    color: Colors.white,
                                                                    fontSize: 12.0,
                                                                    fontWeight: FontWeight.bold,
                                                                  ),
                                                                ),
                                                              ),
                                                            ),
                                                          ),
                                                        )
                                                      ],
                                                    )
                                                );
                                              },
                                            );
                                          }
                                          else if (snapshot.hasError) {
                                            return Center(
                                              child: Text("Đã xảy ra lỗi ... Vui lòng tải lại "),
                                            ); // Add your error widget here
                                          } else {
                                            return Center(child: CircularProgressIndicator(),); // Add your loading widget here
                                          }
                                        },
                                      ),
                                    )
                                    // StreamBuilder widget
                                  ],
                                ),
                              )
                          )
                        ],
                      );
                    }
                    return Center(
                      child: SmallText(text: 'Tải lại trang',),
                    );
                  }),
              // bottomNavigationBar: Column(
              //   mainAxisSize: MainAxisSize.min,
              //   children: [
              //     Container(
              //       padding: EdgeInsets.only(
              //         left: Dimensions.width20 * 1.8,
              //         right: Dimensions.width20 * 1.8,
              //         top: Dimensions.height30,
              //         bottom: Dimensions.height30,
              //       ),
              //       child: StreamBuilder<int?>(
              //           stream: streamCount.stream,
              //           initialData: countAvailable,
              //           builder: (context, snapshot) {
              //             _quantityController.text = snapshot.data.toString();
              //             if (snapshot.hasData) {
              //               var count = snapshot.data!;
              //               return Row(
              //                 mainAxisAlignment: MainAxisAlignment.spaceBetween,
              //                 children: [
              //                   snapshot.data! != 0 ?
              //                   GestureDetector(
              //                     onTap: () {
              //                       if ((count! + countAvailable) > 0) {
              //                         count = count! - 1;
              //                         streamCount.sink.add(count!);
              //                         ProductStreamSum data = ProductStreamSum();
              //                         data.qty = count;
              //                         data.sum = (count * dataProduct.price!.toDouble()) as double?;
              //                         streamSum.sink.add(data);
              //                       }
              //                     },
              //                     child: AppIcon(
              //                       iconsize: Dimensions.iconSize24,
              //                       iconColor: Colors.white,
              //                       backgroundColor: AppColors.mainColor,
              //                       icon: Icons.remove,
              //                     ),
              //                   ): SizedBox(width: 30,),
              //                   Column(
              //                     children: [
              //                       BigText( text:"Đơn giá: " + NumberFormat.currency(
              //                           locale: 'vi_VN', symbol: '₫')
              //                           .format(dataProduct?.price ?? 0),
              //                         color: AppColors.mainColor,
              //                         size: Dimensions.font16,
              //                       ),
              //
              //                       SmallText( text: "Số lượng: ${(snapshot.data).toString()}",
              //                         color: AppColors.mainColor,
              //                         size: Dimensions.font16,
              //                       ),
              //                     ],
              //                   ),
              //                   GestureDetector(
              //                     onTap: () {
              //                       count = count! + 1;
              //                       streamCount.sink.add(count!);
              //                       ProductStreamSum data = ProductStreamSum();
              //                       data.qty = count;
              //                       data.sum = (count * dataProduct.price!.toDouble()) as double?;
              //                       streamSum.sink.add(data);
              //                     },
              //                     child: AppIcon(
              //                       iconsize: Dimensions.iconSize24,
              //                       iconColor: Colors.white,
              //                       backgroundColor: AppColors.mainColor,
              //                       icon: Icons.add,
              //                     ),
              //                   ),
              //                 ],
              //               );
              //             }
              //             return SizedBox();
              //           }),
              //     ),
              //
              //     StreamBuilder<ProductStreamSum?>(
              //         stream: streamSum.stream,
              //         initialData: productStreamSum,
              //         builder: (context, snapshot) {
              //           var productSum = snapshot.data!;
              //           return Container(
              //             height: Dimensions.bottomHeightBar,
              //             padding: EdgeInsets.only(
              //               top: Dimensions.height30,
              //               bottom: Dimensions.height20,
              //               left: Dimensions.width20,
              //               right: Dimensions.width20,
              //             ),
              //             decoration: BoxDecoration(
              //               color: AppColors.buttonBackqroundColor,
              //               borderRadius: BorderRadius.only(
              //                 topLeft: Radius.circular(Dimensions.radius20 * 2),
              //                 topRight: Radius.circular(Dimensions.radius20 * 2),
              //               ),
              //             ),
              //             child: Row(
              //               mainAxisAlignment: MainAxisAlignment.spaceBetween,
              //               children: [
              //                 Container(
              //                   padding: EdgeInsets.all(Dimensions.width20),
              //                   decoration: BoxDecoration(
              //                     borderRadius:
              //                     BorderRadius.circular(Dimensions.radius20),
              //                     color: Colors.white,
              //                   ),
              //                   child: Icon(
              //                     Icons.favorite,
              //                     color: AppColors.mainColor,
              //                   ),
              //                 ),
              //                 Container(
              //                   padding: EdgeInsets.all(Dimensions.width20),
              //                   child: GestureDetector(
              //                     onTap: () {
              //                       if (productSum.qty! > 0 && dataProduct != null) {
              //                         double price =  dataProduct!.price!.toDouble();
              //                         CartItem newItem = CartItem(
              //                           foodName: dataProduct.foodName ?? "",
              //                           categoryId: dataProduct.categoryId ?? 0,
              //                           price: price,
              //                           qty: productSum.qty!,
              //                           foodListId: dataProduct.foodListId ?? 0,
              //                           uploadImage:
              //                           dataProduct.uploadImage ?? "",
              //                           description: dataProduct.description,
              //                           descriptionBuy: noteController.text,
              //                         );
              //                         _addToCart(newItem);
              //                       }
              //                     },
              //                     child: BigText(
              //                       text: NumberFormat.currency(
              //                           locale: 'vi_VN', symbol: '₫')
              //                           .format(productSum.sum ??    0) +
              //                           " | $updateOrAdd",
              //                       color: Colors.white,
              //                       size: Dimensions.font13,
              //                     ),
              //                   ),
              //                   decoration: BoxDecoration(
              //                     borderRadius:
              //                     BorderRadius.circular(Dimensions.radius20),
              //                     color: (productSum.qty!  +  countAvailable) > 0
              //                         ? AppColors.mainColor
              //                         : Colors.grey,
              //                   ),
              //                 ),
              //               ],
              //             ),
              //           );
              //         }),
              //   ],
              // ),
            ),
          Positioned(
            bottom: 50,
            right: 20,
            child: GestureDetector(
              onTap: () {
                Navigator.pushNamed(context, "/cartdetails");
              },
              child: Container(
                padding: EdgeInsets.all(10),
                decoration: BoxDecoration(
                  shape: BoxShape.circle,
                  color: AppColors.mainColor,
                  boxShadow: [
                    BoxShadow(
                      color: Colors.grey.withOpacity(0.5),
                      spreadRadius: 2,
                      blurRadius: 3,
                      offset: Offset(0, 2),
                    ),
                  ],
                ),
                child: Icon(
                  Icons.shopping_bag,
                  size: 30,
                  color: Colors.white,
                ),
              ),
            ),
          )
        ],
      );
  }
  Widget buildCategoryItem(int categoryId, String categoryName, bool isSelected) {
    return Padding(
      padding: EdgeInsets.only(left: 10, right: 10, top: 5, bottom: 5),
      child:
        TapDebouncer(
          cooldown: const Duration(milliseconds: 2500), // Thời gian chờ giữa các lần nhấn
          onTap: () async {
            _selectedCategoryController.add(categoryId);
            fetchData(categoryId);
          },
          builder: (BuildContext context, TapDebouncerFunc? onTap) {
            return
              GestureDetector(
                onTap: onTap,
                child: Container(
                  decoration: BoxDecoration(
                    color: isSelected ? Colors.orange : AppColors.mainColor,
                    borderRadius: BorderRadius.circular(8),
                  ),
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      BigText(text: categoryName, size: 15, color: isSelected ? Colors.white : Colors.white, maxlines: 2),
                    ],
                  ),
                ),
              );
          }
      ),
    );
  }

}

