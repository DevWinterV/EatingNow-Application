import 'dart:async';

import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/data/Api/GoogleAPIService.dart';
import 'package:fam/data/Api/ProductService.dart';
import 'package:fam/models/cuisine_model.dart';
import 'package:fam/models/storenearUser.dart';
import 'package:fam/storage/locationstorage.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/app_constants.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:geolocator/geolocator.dart';
import 'package:intl/intl.dart';

import '../../Widget/Icon_and_Text_widget.dart';
import '../../Widget/customSearch.dart';
import '../../data/Api/CuisineService.dart';
import '../../models/LocationData.dart';
import '../../models/foodlistSearchResponse.dart';
import '../../util/dimensions.dart';

class SearchFoodPage extends StatefulWidget {
  const SearchFoodPage({super.key});

  @override
  State<SearchFoodPage> createState() => _SearchFoodPageState();
}

class _SearchFoodPageState extends State<SearchFoodPage> {
  final cuisineService = CuiSineService(apiUrl: AppConstants.TakeAllCuisine);// lấy danh sách loại hình món ăn
  StreamController<FoodListSearchResponse?> _streamController = StreamController<FoodListSearchResponse?>.broadcast();
  Stream<FoodListSearchResponse?> get getstreamController => _streamController.stream;
  StreamController<CuisineModel?> _streamCuisineController = StreamController<CuisineModel?>.broadcast();
  StreamController<SearchItem?> _streamCuisineIdController = StreamController<SearchItem?>.broadcast();
  Stream<SearchItem?> get getstreamCuisineIDController => _streamCuisineIdController.stream;
  Stream<CuisineModel?> get getstreamCuisineController => _streamCuisineController.stream;

  late CuisineModel cuisineModel;
  late int?  cuisineId;
  late LocationStorage prefs;
  late LocationData locationData;
  double latitude = 0.0;
  double longitude = 0.0;

  void fetchData() async {
    try {

      final cuisineDataFuture = cuisineService.fetchCuisineData(
          {
            "ItemCategoryCode": 0
          });
      final results = await Future.wait([ cuisineDataFuture]);
        cuisineModel = (results[0] as CuisineModel?)!;
        print(cuisineModel.data);
        _streamCuisineController.sink.add(cuisineModel);
        _streamCuisineIdController.sink.add(SearchItem(cuisineId: 0, keyword: ""));
    } catch (e) {
      print(e);
    }
  }

  void initStream(arguments){
    cuisineModel = arguments['data'] as CuisineModel;
    cuisineId = arguments['IdSelected'] as int;
    _streamCuisineIdController.sink.add(SearchItem(cuisineId: cuisineId, keyword: ""));
    _streamCuisineController.sink.add(cuisineModel);
  }
  @override
  void initState() {
    super.initState();
      initLocationData();
      initStreamController();
      // After the async operation is completed, access inherited widgets or elements
      WidgetsBinding.instance.addPostFrameCallback((_) {
        final arguments = ModalRoute.of(context)!.settings.arguments;
        if (arguments != null) {
          initStream(arguments);
        } else {
          fetchData();
        }
      });
  }
  @override
  void dispose() {
    // TODO: implement dispose
    super.dispose();
    _streamController.close();_streamCuisineIdController.close();_streamCuisineController.close();
  }

  void initLocationData() async{
    prefs = await LocationStorage();
    final results = await prefs.getSavedLocation();
    locationData = results;
    if(cuisineId != null){
      SearchFoodListByUser("", cuisineId ?? 0);
    }
  }
  void initStreamController()
  {
    _streamController.sink.add(null);
  }

  void SearchFoodListByUser(String keyword, int? cuisineId) async {
    _streamController.sink.add(null);
    if(locationData == null){
      initLocationData();
    }
    final response = await ProductService(apiUrl: AppConstants.SearchFoodListByUser).SearchFoodListByUser(keyword, locationData.latitude ?? 0.0, locationData.longitude ?? 0.0, cuisineId ?? 0);
    if(response.success == true){
      _streamController.sink.add(response);
    }
  }


  Future<DistanceAndTime?> calculateDistanceAndTime(String end) async {
    try {
      if(locationData == null){
        initLocationData();
      }
      String start = locationData.latitude.toString()+','+locationData.longitude.toString();
      final results = await GoogleAPIService('AIzaSyAG61NrUZkmMW8AS9F7B8mCdT9KQhgG95s').calculateDistanceAndTime(start, end);
      if(results != null){
        return results;
      }
    } catch (e) {
      // Xử lý lỗi nếu có
      print("Lỗi khi tính toán khoảng cách: $e");
    }
    return null;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
        children: [
          StreamBuilder<SearchItem?>(
              stream: getstreamCuisineIDController,
              builder: (builder, snapshot){
              return Column(
                mainAxisAlignment: MainAxisAlignment.start,
                children: [
                  Padding(
                    padding: EdgeInsets.only(top: 30, left: 10, right: 10, bottom: 5),
                    child: CustomSearchBar(
                      onSubmitted: (value) {
                        _streamCuisineIdController.sink.add(SearchItem(cuisineId: snapshot.data?.cuisineId ?? cuisineId, keyword: value));
                        SearchFoodListByUser(value.trim().toLowerCase(), snapshot.data?.cuisineId ?? 0 );
                      },
                    ),
                  ),
                  buldCatagoryItem(snapshot?.data?.cuisineId ?? 0, snapshot?.data?.keyword ?? ""),
                  Expanded(
                    flex: 2,
                    child:
                    StreamBuilder<FoodListSearchResponse?>(
                      initialData: null,
                      stream: getstreamController,
                      builder: (context, snapshot) {
                        if(snapshot.data == null){
                          return Center(
                            child: SmallText(text: "Bạn đang muốn tìm gì nào ?",color: Colors.black, size: Dimensions.font14,),
                          );
                        }
                        if(snapshot.data!.data!.length! > 0 && snapshot.hasData){
                          return   ListView.builder(
                              physics: AlwaysScrollableScrollPhysics(),
                              shrinkWrap: true,
                              itemCount: snapshot!.data!.data?.length ?? 0,
                              itemBuilder: (context, index) {
                                final item = snapshot!.data?.data![index];
                                final storeConvert = DataStoreNearUserModel(
                                  userId: item?.storeinFo?.userId ?? 0,
                                  fullName: item?.storeinFo?.fullName ?? "",
                                  openTime: item?.storeinFo?.openTime ?? "",
                                  ownerName: item?.storeinFo?.ownerName ?? "",
                                  latitude: item?.storeinFo?.latitude ?? 0.0,
                                  longitude: item?.storeinFo?.longitude ?? 0.0,
                                  phone: item?.storeinFo?.phone ?? "",
                                  description: item?.storeinFo?.description ?? "",
                                  absoluteImage: item?.storeinFo?.absoluteImage ?? "",
                                  address: item?.storeinFo?.address ?? "",
                                );
                                return
                                  Column(
                                    children: [
                                      GestureDetector(
                                          onTap: () {
                                            // Điều hướng đến trang chi tiết tại đây
                                            // Chuyển đổi route tới link và truyền dữ liệu caritems
                                            Navigator.pushReplacement(
                                                context,
                                                Navigator.pushNamed(context, "/storedetail", arguments: {'data': storeConvert }) as Route<Object?>
                                            );
                                          },
                                          child:
                                          Container(
                                            margin: EdgeInsets.only(left: Dimensions.width20, right: Dimensions.width20, bottom: Dimensions.height10),
                                            child:
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
                                                      image: NetworkImage(item?.storeinFo?.absoluteImage ?? "" ),
                                                    ),
                                                  ),
                                                ),
                                                //text container
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
                                                          mainAxisAlignment: MainAxisAlignment.spaceAround,
                                                          children: [
                                                            BigText(text: (item?.storeinFo?.fullName ?? ""), size: Dimensions.font16, color: AppColors.signColor, ),
                                                            // SmallText(text: (product?.storeName ?? ""), size: Dimensions.font13, color: AppColors.paraColor,),
                                                            FutureBuilder<DistanceAndTime?>(
                                                              future: calculateDistanceAndTime(item!.storeinFo!.latitude.toString()  +","+item!.storeinFo!.longitude.toString()),
                                                              builder: (context, snapshot) {
                                                                if (snapshot.connectionState == ConnectionState.waiting) {
                                                                  return SizedBox(); // Show a loading indicator while waiting for the result
                                                                } else if (snapshot.hasError) {
                                                                  return SizedBox();
                                                                } else {
                                                                  return
                                                                    Row(
                                                                      mainAxisAlignment: MainAxisAlignment.spaceAround,
                                                                      children: [
                                                                        IconAndTextWidget(
                                                                          icon: Icons.access_time_rounded,
                                                                          text: snapshot.data?.time ?? "",
                                                                          iconColor: AppColors.mainColor,),
                                                                        IconAndTextWidget(
                                                                          icon: Icons.location_on,
                                                                          text: snapshot.data?.distance ?? "",
                                                                          iconColor: AppColors.iconColor2,
                                                                        ),
                                                                      ],
                                                                    );
                                                                }
                                                              },
                                                            )
                                                          ],
                                                        ),
                                                      ),
                                                    )
                                                )
                                              ],
                                            ),
                                          )
                                      ),
                                      item!.foodList!.length > 0 ?
                                      Column(
                                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                        crossAxisAlignment: CrossAxisAlignment.center,
                                        children: [
                                          Container(
                                            height: item!.foodList!.length * 110,
                                            child:ListView.builder(
                                                physics: NeverScrollableScrollPhysics(),
                                                shrinkWrap: true,
                                                itemCount: item!.foodList!.length,
                                                //itemCount: item!.foodList!.length > 2 ? 2 : item!.foodList!.length,
                                                itemBuilder: (context, index) {
                                                  final product = item!.foodList![index];
                                                  product.storeName = item?.storeinFo?.fullName?? "";
                                                  return
                                                    GestureDetector(
                                                        onTap: () {
                                                          // Điều hướng đến trang chi tiết tại đây
                                                          // Chuyển đổi route tới link và truyền dữ liệu caritems
                                                          Navigator.pushReplacement(
                                                              context,
                                                              Navigator.pushNamed(context, "/productdetail", arguments: {'data': product }) as Route<Object?>
                                                          );
                                                        },
                                                        child:
                                                        Container(
                                                          margin: EdgeInsets.only(left: Dimensions.width20, right: Dimensions.width20, bottom: Dimensions.height10),
                                                          child: Row(
                                                            children: [
                                                              //text container
                                                              // Phần chứa văn bản
                                                              Expanded(
                                                                  child: Container(
                                                                    height: Dimensions.listViewTextContSize,
                                                                    width: Dimensions.listViewTextContSize,
                                                                    decoration: BoxDecoration(
                                                                      borderRadius: BorderRadius.only(
                                                                        topLeft: Radius.circular(Dimensions.radius20),
                                                                        bottomLeft: Radius.circular(Dimensions.radius20),
                                                                        topRight: Radius.circular(Dimensions.radius20),
                                                                        bottomRight: Radius.circular(Dimensions.radius20),
                                                                      ),
                                                                      color: Colors.white,
                                                                    ),
                                                                    child: Padding(
                                                                        padding: EdgeInsets.only(left: Dimensions.width10, right: Dimensions.width10),
                                                                        child:
                                                                        Row(
                                                                          children: [
                                                                            Padding(padding: EdgeInsets.only(right: 14), child: Container(
                                                                              width: Dimensions.listViewFoodImgSize,
                                                                              height: Dimensions.listViewFoodImgSize,
                                                                              decoration: BoxDecoration(
                                                                                borderRadius: BorderRadius.circular(Dimensions.radius20),
                                                                                color:  (index % 2 == 0) ? Colors.orange[50] : Colors.amber[100], // Bạn có thể thay thế bằng widget Image.network
                                                                                image: DecorationImage(
                                                                                  fit: BoxFit.cover,
                                                                                  image: NetworkImage(product?.uploadImage ?? "" ),
                                                                                ),
                                                                              ),
                                                                            ),),
                                                                            Column(
                                                                              crossAxisAlignment: CrossAxisAlignment.start,
                                                                              mainAxisAlignment: MainAxisAlignment.center,
                                                                              children: [
                                                                                Container(
                                                                                  height: 40,
                                                                                  child: Text(
                                                                                    product?.foodName ?? "",
                                                                                    style: TextStyle(
                                                                                      fontSize:  Dimensions.font16,
                                                                                      color: AppColors.signColor,
                                                                                    ),
                                                                                    maxLines: 2,
                                                                                    overflow: TextOverflow.ellipsis,
                                                                                  ),
                                                                                ),
                                                                                Row(
                                                                                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                                                                  children: [
                                                                                    SmallText(text: NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(product?.price ?? 0),), // Thay thế bằng thuộc tính tương ứng
                                                                                  ],
                                                                                )
                                                                              ],
                                                                            ),
                                                                          ],
                                                                        )
                                                                    ),
                                                                  )
                                                              )
                                                            ],
                                                          ),
                                                        )
                                                    );


                                                }),
                                          ),
                                        ],
                                      ):
                                      SizedBox(),
                                      Divider(thickness: 5,color: Colors.white,),
                                    ],
                                  );
                              });
                        }
                        if(snapshot.data!.data!.length! == 0 && snapshot.hasData){
                          return Center(
                            child: SmallText(text: "Không tìm thấy kết quả phù hợp",color: Colors.black, size: Dimensions.font14,),
                          );
                        }
                        return SizedBox();
                      },

                    ),
                  ),
                ],
              );
              }),
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
      ),
    );

  }
  StreamBuilder buldCatagoryItem(int cuisineId, String keyword){
    return StreamBuilder<CuisineModel?>(
          stream: getstreamCuisineController,
          builder: (builder, snapshot){
            if(snapshot.connectionState == ConnectionState.waiting){
              return Center(
                child: SizedBox(),
              );
            }
            if(snapshot.data!.data!.length > 0 && snapshot.hasData) {
              return   Container(
                margin: EdgeInsets.only(
                    left: Dimensions.width10, right: Dimensions.width10),
                height: 110,
                child: ListView.separated(
                    itemCount: snapshot!.data!.data?.length ?? 0,
                    scrollDirection: Axis.horizontal,
                    separatorBuilder: (context, index) =>
                        SizedBox(width: 5,),
                    itemBuilder: (context, position) {
                      final cuisineItem = snapshot!.data!.data?[position];
                      return
                        Stack(
                          children: [
                            GestureDetector(
                                onTap: (){
                                  print(cuisineItem!.cuisineId);
                                  print(snapshot!.data!.data?[position].cuisineId);
                                  _streamCuisineIdController.sink.add(SearchItem(cuisineId: cuisineItem!.cuisineId ?? 0 ,keyword: keyword ));
                                  SearchFoodListByUser(keyword, cuisineItem!.cuisineId ?? 0);
                                },
                                child: Container(
                                  margin: EdgeInsets.only(left: 2),
                                  height: 100,
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
                                              NetworkImage(cuisineItem?.absoluteImage ?? ""),
                                            )
                                        ),

                                      ),
                                      Text(cuisineItem?.name ?? " ",
                                        style: TextStyle(fontSize: 12, color: AppColors.paraColor),
                                        overflow: TextOverflow.ellipsis,
                                        // Sẽ hiển thị dấu ba chấm (...) nếu văn bản quá dài
                                        maxLines: 1, // Số dòng tối đa hiển thị (có thể điều chỉnh theo nhu cầu của bạn)
                                      )
                                    ],
                                  ),
                                )),
                            cuisineItem?.cuisineId == cuisineId ?
                            Positioned(
                              top: 2,
                              right: 2,
                              child: GestureDetector(
                                onTap: (){
                                  _streamCuisineIdController.sink.add(SearchItem(cuisineId: 0 ,keyword: keyword ));
                                  SearchFoodListByUser(keyword, 0);
                                    },
                                  child: Icon(Icons.check_circle, color: Colors.green,),
                                ),
                            ) : SizedBox()
                          ],
                        );
                    }
                ),
              );
            }
            return SizedBox();
          });
  }
}
class SearchItem{
  int? cuisineId;
  String? keyword;
  SearchItem({this.cuisineId, this.keyword});
}


