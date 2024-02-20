import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/storage/UserAccountstorage.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/dimensions.dart';
import 'package:flutter/material.dart';
import '../../util/app_constants.dart';

class ProfilePage extends StatefulWidget {
  @override
  _ProfilePageState createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage> {
  void onRefresh(){
    print("Demoo");
  }
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
      body:
          Column(
            children: [
              Expanded(
                  child:  RefreshIndicator(
                      color: AppColors.mainColor,
                      onRefresh: () async {
                        onRefresh();
                      },
                      child: SingleChildScrollView(
                        child: Column(
                          mainAxisAlignment: MainAxisAlignment.start,
                          children: [
                            Padding(
                              padding: EdgeInsets.all(12),
                              child:Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: [
                                  Row(
                                    children: [
                                      ClipOval(
                                        child:    Image.asset(
                                          "assets/image/logo_xpressEat.png",
                                          height: 60,
                                          width: 60,),
                                      ),
                                      SizedBox(width: 5,),
                                      BigText(text: "Đông Châu",size: 18,)
                                    ],
                                  ),
                                  IconButton(onPressed: (){}, icon: Icon(Icons.chevron_right))
                                ],
                              ),),
                            Divider(
                              thickness: 4, // Adjust the thickness of the divider as needed
                              color: Colors.white,
                            ),
                            Padding(padding: EdgeInsets.all(12),child:Row(
                              mainAxisAlignment: MainAxisAlignment.spaceAround,
                              crossAxisAlignment: CrossAxisAlignment.center,
                              children: [
                                Column(
                                  children: [
                                    Icon(Icons.featured_play_list_sharp),
                                    SmallText(text: 'Đơn hàng', size: 12,)
                                  ],
                                ),
                                Column(
                                  children: [
                                    Icon(Icons.featured_play_list_sharp),
                                    SmallText(text: 'Món ăn yêu thích',size: 12,)
                                  ],
                                ),
                                Column(
                                  children: [
                                    Icon(Icons.featured_play_list_sharp),
                                    SmallText(text: 'Cửa hàng yêu thích',size: 12,)
                                  ],
                                ),
                              ],
                            ),),
                            Divider(
                              thickness: 4, // Adjust the thickness of the divider as needed
                              color: Colors.white,
                            ),
                            Padding(
                              padding: EdgeInsets.all(8),
                              child:Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: [
                                Row(
                                  children: [
                                    SmallText(text: "Phiên bản hiện tại: v${AppConstants.APP_VERSION}",size: 15,)
                                  ],
                                ),
                                IconButton(onPressed: (){}, icon: Icon(Icons.chevron_right))
                              ],
                            ),),
                            Divider(
                              thickness: 4, // Adjust the thickness of the divider as needed
                              color: Colors.white,
                            ),
                            Center(
                                child: Container(
                                  child: BigText(text: AppConstants.APP_NAME, size: 40, color: AppColors.mainColor,),
                                )
                            ),
                          ],
                        ),
                      )
                  )
              ),
            ],
          )
    );
  }
}
