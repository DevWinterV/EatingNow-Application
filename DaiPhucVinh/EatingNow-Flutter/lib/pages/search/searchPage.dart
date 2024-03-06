import 'dart:async';

import 'package:fam/Widget/Small_text.dart';
import 'package:fam/data/Api/ProductService.dart';
import 'package:fam/storage/locationstorage.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/app_constants.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import '../../models/LocationData.dart';
import '../../models/foodlistSearchResponse.dart';
import '../../util/dimensions.dart';

class SearchFoodPage extends StatefulWidget {
  const SearchFoodPage({super.key});

  @override
  State<SearchFoodPage> createState() => _SearchFoodPageState();
}

class _SearchFoodPageState extends State<SearchFoodPage> {
  StreamController<FoodListSearchResponse?> _streamController = StreamController<FoodListSearchResponse?>.broadcast();
  Stream<FoodListSearchResponse?> get getstreamController => _streamController.stream;

  late LocationStorage prefs;
  late LocationData locationData;
  double latitude = 0.0;
  double longitude = 0.0;
  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    initStreamController();
    initLocationData();
  }
  @override
  void dispose() {
    // TODO: implement dispose
    super.dispose();
    _streamController.close();
  }
  void initLocationData() async{
    prefs = await LocationStorage();
    final results = await prefs.getSavedLocation();
    locationData = results;
  }
  void initStreamController()
  {
    _streamController.sink.add(null);
  }

  void SearchFoodListByUser(String keyword) async {
    if(locationData == null){
      initLocationData();
    }
    final response = await ProductService(apiUrl: AppConstants.SearchFoodListByUser).SearchFoodListByUser(keyword, locationData.latitude ?? 0.0, locationData.longitude ?? 0.0);
    if(response.success == true){
      _streamController.sink.add(response);
    }
    else{

    }
  }
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
        children: [
          Column(
            children: [
              // Phần Header tìm kiếm
              Container(
                      margin: EdgeInsets.only(top: Dimensions.height45, bottom: Dimensions.height5),
                      padding: EdgeInsets.only(left: Dimensions.width20, right: Dimensions.width20),
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Column(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                                IconButton(
                                    onPressed: (){
                                        Navigator.of(context).pop();
                                }, icon: Icon(Icons.arrow_circle_left, size: 20,)),
                                TextField(
                                  onChanged: (value) {
                                    SearchFoodListByUser(value.trim().toLowerCase());
                                  },
                                  style: TextStyle(color: Colors.black),
                                  decoration: InputDecoration(
                                    filled: true,
                                    fillColor: Colors.white,
                                    border: OutlineInputBorder(
                                      borderRadius: BorderRadius.circular(8.0),
                                      borderSide: BorderSide.none,
                                    ),
                                    hintText: "Nhập món ăn hoặc cửa hàng bạn muốn tìm ...",
                                    prefixIcon: Icon(Icons.search_outlined),
                                    prefixIconColor: AppColors.mainColor
                                  ),
                                )
                            ],
                          ),
                        ],
                      )
                  ),
              //showing the body
              Expanded(
                child: SingleChildScrollView(
                  child: StreamBuilder<FoodListSearchResponse?>(
                    stream: getstreamController,
                    builder: (context, snapshot) {
                      if(snapshot.connectionState == ConnectionState.waiting){
                        return Center(
                          child: CircularProgressIndicator(color: AppColors.mainColor,),
                        );
                      }
                      if(snapshot.data!.data!.length! > 0 && snapshot.hasData){
                        return ListView.builder(
                            itemCount: snapshot.data!.data!.length,
                            itemBuilder: (itemBuilder, index){
                              final item = snapshot.data!.data?[index];
                              return ListTile(
                                title: Text(item?.storeinFo?.fullName ?? ""),
                              );
                            });
                      }
                      if(snapshot.data!.dataCount! == 0 && snapshot.hasData){
                        return Center(
                          child: SmallText(text: "Không tìm thấy kết quả phù hợp",color: Colors.black, size: Dimensions.font14,),
                        );
                      }

                      return SizedBox();
                    },

                  ),
                ),
              ),
            ],
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
      ),
    );
  }
}

