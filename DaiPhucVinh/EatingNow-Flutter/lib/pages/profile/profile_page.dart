import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/storage/UserAccountstorage.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/dimensions.dart';
import 'package:flutter/material.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter_native_splash/flutter_native_splash.dart';
import 'package:intl_phone_field/intl_phone_field.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../data/Api/CustomerService.dart';
import '../../util/app_constants.dart';

class ProfilePage extends StatefulWidget {
  @override
  _ProfilePageState createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          'Thông tin tài khoản',
          overflow: TextOverflow.ellipsis,
          maxLines: 1,
          style: TextStyle(
            fontSize: Dimensions.font20,
          ),// Số dòng tối đa hiển thị (có thể điều chỉnh theo nhu cầu của bạn)
        ),
        centerTitle: true, // Để căn giữa tiêu đề trên thanh AppBar
        // Các thuộc tính khác của AppBar
        backgroundColor: AppColors.mainColor, // Màu nền cho AppBar
      ),
      backgroundColor: Colors.white,
      body: Container(
        height: MediaQuery.of(context).size.height,
        width: MediaQuery.of(context).size.width,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: [
            Padding(padding: EdgeInsets.all(5),child:Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Row(
                  children: [
                    ClipOval(
                      child:    Image.asset(
                        "assets/image/logo_xpressEat.png",
                        height: 80,
                        width: 80,),
                    ),
                    SmallText(text: "Đông Châu",)
                  ],
                ),
                IconButton(onPressed: (){}, icon: Icon(Icons.chevron_right))
              ],
            ),),
            Divider(
              thickness: 2, // Adjust the thickness of the divider as needed
              color: Colors.white,
            ),
            Padding(padding: EdgeInsets.all(5),child:Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                Column(
                  children: [
                    Icon(Icons.featured_play_list_sharp),
                    SmallText(text: 'Đơn hàng',)
                  ],
                ),
                Column(
                  children: [
                    Icon(Icons.featured_play_list_sharp),
                    SmallText(text: 'Món ăn yêu thích',)
                  ],
                ),
                Column(
                  children: [
                    Icon(Icons.featured_play_list_sharp),
                    SmallText(text: 'Cửa hàng yêu thích',)
                  ],
                ),
              ],
            ),),
            Padding(padding: EdgeInsets.all(5),child:Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Row(
                  children: [
                    SmallText(text: "Phiên bản hiện tại: v${AppConstants.APP_VERSION}",)
                  ],
                ),
                IconButton(onPressed: (){}, icon: Icon(Icons.chevron_right))
              ],
            ),),
            Center(
              child: Container(
                child: BigText(text: AppConstants.APP_NAME, size: 30, color: AppColors.mainColor,),
              )
            )
          ],
        ),
      ),
    );
  }
}
