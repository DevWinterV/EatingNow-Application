import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/models/LocationData.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import '../../storage/locationstorage.dart';
import '../../util/Colors.dart';
import '../../util/dimensions.dart';
import '../Cart/cartPage.dart';
import 'food_page_body.dart';
import 'package:flutter_native_splash/flutter_native_splash.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'getCurrentLocation_page.dart';


class MainFoodPage extends StatefulWidget {
  const MainFoodPage({super.key});
  @override
  State<MainFoodPage> createState() => _MainFoodPageState();
}

class _MainFoodPageState extends State<MainFoodPage> {
  bool isLoading = true; // Biến để kiểm tra xem đang lấy dữ liệu hay chưa
  String addressdelivery = '';
  String nameAddressdelivery ='';
  late  SharedPreferences prefs;
  @override
  initState() {
    super.initState();
    initialization();
    getAddressDelivery();
  }
  void getAddressDelivery() async {
    prefs = await SharedPreferences.getInstance();
    setState(() {
      addressdelivery = prefs.getString('address') ?? '';
      nameAddressdelivery =prefs.getString('name') ?? '';

    });
  }

  void initialization() async {
    FlutterNativeSplash.remove();
  }
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
         children: [

           Column(
             children: [
               // Phần Header
               Container(
                   child: Container(
                       margin: EdgeInsets.only(top: Dimensions.height45, bottom: Dimensions.height5),
                       padding: EdgeInsets.only(left: Dimensions.width20, right: Dimensions.width20),
                       child: Row(
                         mainAxisAlignment: MainAxisAlignment.spaceBetween,
                         children: [
                           Column(
                             crossAxisAlignment: CrossAxisAlignment.start,
                             children: [
                               BigText(text: "Giao tới: "+nameAddressdelivery.toUpperCase(), color: AppColors.mainColor),
                               GestureDetector(
                                 onTap: () {
                                   Navigator.push(
                                     context,
                                     MaterialPageRoute(builder: (context) => LocationPage(link: "",)), // Thay thế 'LocationPage' bằng tên trang thực tế của bạn
                                   );
                                 },
                                 child: Row(
                                   children: [
                                     Container(
                                       height: Dimensions.height45,
                                       width: Dimensions.screenWidth - 110,
                                       child: BigText(text: addressdelivery, size: Dimensions.font13,),
                                     ),
                                     Icon(Icons.arrow_drop_down_rounded)
                                   ],
                                 ),
                               )
                             ],
                           ),
                           Container(
                               width:Dimensions.height40,
                               height:Dimensions.height40,
                               child:Icon(Icons.search, color:Colors.white, size: Dimensions.iconSize16,),
                               decoration: BoxDecoration(
                                 borderRadius: BorderRadius.circular(Dimensions.radius15),
                                 color: AppColors.mainColor,
                               )
                           )
                         ],
                       )
                   )
               ),

               //showing the body
               Expanded(child: SingleChildScrollView(
                 child:  FoodPageBody(),
               )),

             ],
           ),
           Positioned(
             bottom: 50,
             right: 20,
             child: GestureDetector(
               onTap: () {
                 Navigator.push(
                   context,
                   MaterialPageRoute(builder: (context) => CartPage()),
                 );
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
      )


    );
  }
}
