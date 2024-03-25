import 'dart:async';

import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/models/user_account_model.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/dimensions.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';
import '../../data/Api/CustomerService.dart';
import '../../util/app_constants.dart';

class ProfilePage extends StatefulWidget {
  const ProfilePage({super.key});
  @override
  _ProfilePageState createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage> {
  late StreamController<Data?> _userDataStreamController;
  late CustomerService customerService;

  void _fetchDataUser() async {
    final result = await customerService.fecthUserData({
      "CustomerId": FirebaseAuth.instance.currentUser?.uid ?? "",
      "Phone": FirebaseAuth.instance.currentUser?.phoneNumber ?? ""
    });

    if (result.success == true) {
      _userDataStreamController.add(result.data?[0] ?? null);
    }
  }

  void _onRefresh() {
    _fetchDataUser();
  }

  void navigateToHome() {
    if (FirebaseAuth.instance.currentUser?.uid == null) {
      Navigator.pop(context);
    }
  }

  void _launchPhoneCall(String phoneNumber) async {
    String url = 'tel:$phoneNumber';
    if (await canLaunch(url)) {
      await launch(url);
    } else {
      // Handle error, e.g., show an error message
      print('Không thể chuyển đến URL $url');
    }
  }

  @override
  void initState() {
    super.initState();
    _userDataStreamController = StreamController<Data?>();
    customerService = CustomerService(apiUrl: AppConstants.CheckCustomer);
    _fetchDataUser();
  }

  @override
  void dispose() {
    _userDataStreamController.close();
    super.dispose();
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
            ), // Số dòng tối đa hiển thị (có thể điều chỉnh theo nhu cầu của bạn)
          ),
          centerTitle: true, // Để căn giữa tiêu đề trên thanh AppBar
          // Các thuộc tính khác của AppBar
          backgroundColor: AppColors.mainColor, // Màu nền cho AppBar
        ),
        body: Column(
          children: [
            Expanded(
              child: StreamBuilder<Data?>(
                stream: _userDataStreamController.stream,
                builder: (BuildContext context, AsyncSnapshot<Data?> snapshot) {
                  if (snapshot.connectionState == ConnectionState.waiting) {
                    return Center(
                      child: CircularProgressIndicator(
                        color: AppColors.mainColor,
                      ),
                    );
                  }
                  if (snapshot.hasData) {
                    final userdata = snapshot.data;
                    return Column(
                      children: [
                        Expanded(
                            child: RefreshIndicator(
                                color: AppColors.iconColor1,
                                onRefresh: () async {
                                  _onRefresh();
                                },
                                child: SingleChildScrollView(
                                  child: Column(
                                    mainAxisAlignment: MainAxisAlignment.start,
                                    children: [
                                      Padding(
                                        padding: EdgeInsets.all(15),
                                        child: Row(
                                          mainAxisAlignment:
                                          MainAxisAlignment.spaceBetween,
                                          children: [
                                            Row(
                                              children: [
                                                userdata?.imageProfile == null || userdata?.imageProfile == ""
                                                    ? ClipOval(
                                                  child: Image.asset(
                                                    "assets/image/logo_xpressEat.png",
                                                    height: 60,
                                                    width: 60,
                                                  ),
                                                )
                                                    : ClipOval(
                                                  child: Image.network(
                                                    userdata!.imageProfile!,
                                                    height: 60,
                                                    width: 60,
                                                  ),
                                                ),
                                                SizedBox(
                                                  width: 8,
                                                ),
                                                BigText(
                                                  text: userdata!.completeName! ??
                                                      "",
                                                  size: 18,
                                                )
                                              ],
                                            ),
                                            IconButton(
                                                onPressed: () async {
                                                  final result = await Navigator.of(context).pushNamed('viewprofiledetail', arguments: {"data": userdata });
                                                  if(result == true){
                                                    _onRefresh();
                                                  }
                                                },
                                                icon: Icon(Icons.chevron_right))
                                          ],
                                        ),
                                      ),
                                      Divider(
                                        thickness: 4,
                                        // Adjust the thickness of the divider as needed
                                        color: Colors.white,
                                      ),
                                      Padding(
                                        padding: EdgeInsets.all(12),
                                        child: Row(
                                          mainAxisAlignment:
                                          MainAxisAlignment.spaceBetween,
                                          crossAxisAlignment:
                                          CrossAxisAlignment.center,
                                          children: [
                                            GestureDetector(
                                              onTap: (){
                                                Navigator.of(context).pushNamed("/orderlist");
                                              },
                                              child: Column(
                                                children: [
                                                  Image.asset(
                                                    "assets/image/listorder.png",
                                                    height: 100,
                                                    width: 100,
                                                  ),
                                                  BigText(
                                                    text: 'Đơn hàng',
                                                    size: 13,
                                                  )
                                                ],
                                              ),
                                            ),
                                            Column(
                                              children: [
                                                Image.asset(
                                                  "assets/image/favoritefood.png",
                                                  height: 100,
                                                  width: 100,
                                                ),
                                                BigText(
                                                  text: 'Món yêu thích',
                                                  size: 13,
                                                )
                                              ],
                                            ),
                                            Column(
                                              children: [
                                                Image.asset(
                                                  "assets/image/favoritestore.png",
                                                  height: 100,
                                                  width: 100,
                                                ),
                                                BigText(
                                                  text: 'Quán yêu thích',
                                                  size: 13,
                                                )
                                              ],
                                            ),
                                          ],
                                        ),
                                      ),
                                      Divider(
                                        thickness: 4,
                                        // Adjust the thickness of the divider as needed
                                        color: Colors.white,
                                      ),
                                      // đánh giá
                                      Padding(
                                        padding: EdgeInsets.all(6),
                                        child: Row(
                                          mainAxisAlignment:
                                          MainAxisAlignment.spaceBetween,
                                          children: [
                                            Row(
                                              children: [
                                                Icon(
                                                  Icons.star,
                                                  size: 20,
                                                  color: Colors.yellow[700],
                                                ),
                                                SizedBox(
                                                  width: 5,
                                                ),
                                                SmallText(
                                                  text: 'Đánh giá XpressEat.',
                                                  size: 15,
                                                  color: Colors.black,
                                                )
                                              ],
                                            ),
                                            IconButton(
                                                onPressed: () {},
                                                icon: Icon(Icons.chevron_right))
                                          ],
                                        ),
                                      ),
                                      Divider(
                                        thickness: 4,
                                        // Adjust the thickness of the divider as needed
                                        color: Colors.white,
                                      ),
                                      // hỗ trợ
                                      Padding(
                                          padding: EdgeInsets.all(6),
                                          child: GestureDetector(
                                            onTap: () {},
                                            child: Row(
                                              mainAxisAlignment:
                                              MainAxisAlignment.spaceBetween,
                                              children: [
                                                Row(
                                                  children: [
                                                    Icon(
                                                      Icons.headset_mic_sharp,
                                                      size: 20,
                                                      color: Colors.black,
                                                    ),
                                                    SizedBox(
                                                      width: 5,
                                                    ),
                                                    SmallText(
                                                      text: 'Trung tâm hỗ trợ',
                                                      size: 15,
                                                      color: Colors.black,
                                                    )
                                                  ],
                                                ),
                                                IconButton(
                                                    onPressed: () {
                                                      _launchPhoneCall(
                                                          "0766837068");
                                                    },
                                                    icon:
                                                    Icon(Icons.chevron_right))
                                              ],
                                            ),
                                          )),
                                      Divider(
                                        thickness: 4,
                                        // Adjust the thickness of the divider as needed
                                        color: Colors.white,
                                      ),
                                      // phiên bản app
                                      Padding(
                                        padding: EdgeInsets.all(6),
                                        child: Row(
                                          mainAxisAlignment:
                                          MainAxisAlignment.spaceBetween,
                                          children: [
                                            Row(
                                              children: [
                                                SmallText(
                                                  text:
                                                  "Phiên bản hiện tại: v${AppConstants.APP_VERSION}",
                                                  size: 15,
                                                  color: Colors.black,
                                                )
                                              ],
                                            ),
                                            IconButton(
                                                onPressed: () {},
                                                icon: Icon(Icons.chevron_right))
                                          ],
                                        ),
                                      ),
                                      Divider(
                                        thickness: 4,
                                        // Adjust the thickness of the divider as needed
                                        color: Colors.white,
                                      ),
                                      // Đăng xuất
                                      Padding(
                                        padding: EdgeInsets.all(6),
                                        child: Row(
                                          mainAxisAlignment:
                                          MainAxisAlignment.center,
                                          children: [
                                            Row(
                                              children: [
                                                GestureDetector(
                                                  onTap: () async {
                                                    await FirebaseAuth.instance
                                                        .signOut();
                                                    navigateToHome();
                                                  },
                                                  child: Text("Đăng xuất",
                                                    style: TextStyle(
                                                      fontSize: Dimensions.font16,
                                                      fontWeight: FontWeight.bold
                                                  ),),
                                                  // child: SmallText(
                                                  //   text: "Đăng xuất",
                                                  //   size: 15,
                                                  //   color: Colors.black,
                                                  //
                                                  // ),
                                                )
                                              ],
                                            ),
                                          ],
                                        ),
                                      ),
                                      Divider(
                                        thickness: 4,
                                        // Adjust the thickness of the divider as needed
                                        color: Colors.white,
                                      ),
                                      Container(
                                        height:
                                        MediaQuery.of(context).size.height -
                                            600,
                                        child: Center(
                                          child: BigText(
                                            text: AppConstants.APP_NAME,
                                            size: 30,
                                            color: AppColors.mainColor,
                                          ),
                                        ),
                                      )
                                    ],
                                  ),
                                ))),
                      ],
                    );
                  }
                  return Center(
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          ElevatedButton(
                            style: ElevatedButton.styleFrom(
                              backgroundColor: AppColors.mainColor,
                            ),
                            onPressed: () {
                              Navigator.of(context).pushNamed('/login');
                            },
                            child: Text(
                              'Đăng nhập',
                              style: TextStyle(color: Colors.white),
                            ),
                          ),
                        ],
                      ));
                },
              ) ,
              // child: FutureBuilder<Data?>(
              //   future: fetchDataUser(),
              //   builder: (BuildContext context, AsyncSnapshot<Data?> snapshot) {
              //     print(snapshot);
              //     if (snapshot.connectionState == ConnectionState.waiting) {
              //       return Center(
              //         child: CircularProgressIndicator(
              //           color: AppColors.mainColor,
              //         ),
              //       );
              //     }
              //     if (snapshot.hasData) {
              //       final userdata = snapshot.data;
              //       return Column(
              //         children: [
              //           Expanded(
              //               child: RefreshIndicator(
              //                   color: AppColors.mainColor,
              //                   onRefresh: () async {
              //                     _onRefresh();
              //                   },
              //                   child: SingleChildScrollView(
              //                     child: Column(
              //                       mainAxisAlignment: MainAxisAlignment.start,
              //                       children: [
              //                         Padding(
              //                           padding: EdgeInsets.all(15),
              //                           child: Row(
              //                             mainAxisAlignment:
              //                                 MainAxisAlignment.spaceBetween,
              //                             children: [
              //                               Row(
              //                                 children: [
              //                                   userdata?.imageProfile == null
              //                                       ? ClipOval(
              //                                           child: Image.asset(
              //                                             "assets/image/logo_xpressEat.png",
              //                                             height: 60,
              //                                             width: 60,
              //                                           ),
              //                                         )
              //                                       : ClipOval(
              //                                           child: Image.network(
              //                                             userdata!
              //                                                 .imageProfile!,
              //                                             height: 60,
              //                                             width: 60,
              //                                           ),
              //                                         ),
              //                                   SizedBox(
              //                                     width: 8,
              //                                   ),
              //                                   BigText(
              //                                     text:
              //                                         userdata!.completeName! ??
              //                                             "",
              //                                     size: 18,
              //                                   )
              //                                 ],
              //                               ),
              //                               IconButton(
              //                                   onPressed: () {},
              //                                   icon: Icon(Icons.chevron_right))
              //                             ],
              //                           ),
              //                         ),
              //                         Divider(
              //                           thickness: 4,
              //                           // Adjust the thickness of the divider as needed
              //                           color: Colors.white,
              //                         ),
              //                         Padding(
              //                           padding: EdgeInsets.all(12),
              //                           child: Row(
              //                             mainAxisAlignment:
              //                                 MainAxisAlignment.spaceBetween,
              //                             crossAxisAlignment:
              //                                 CrossAxisAlignment.center,
              //                             children: [
              //                               Column(
              //                                 children: [
              //                                   Image.asset(
              //                                     "assets/image/listorder.png",
              //                                     height: 100,
              //                                     width: 100,
              //                                   ),
              //                                   BigText(
              //                                     text: 'Đơn hàng',
              //                                     size: 13,
              //                                   )
              //                                 ],
              //                               ),
              //                               Column(
              //                                 children: [
              //                                   Image.asset(
              //                                     "assets/image/favoritefood.png",
              //                                     height: 100,
              //                                     width: 100,
              //                                   ),
              //                                   BigText(
              //                                     text: 'Món yêu thích',
              //                                     size: 13,
              //                                   )
              //                                 ],
              //                               ),
              //                               Column(
              //                                 children: [
              //                                   Image.asset(
              //                                     "assets/image/favoritestore.png",
              //                                     height: 100,
              //                                     width: 100,
              //                                   ),
              //                                   BigText(
              //                                     text: 'Quán yêu thích',
              //                                     size: 13,
              //                                   )
              //                                 ],
              //                               ),
              //                             ],
              //                           ),
              //                         ),
              //                         Divider(
              //                           thickness: 4,
              //                           // Adjust the thickness of the divider as needed
              //                           color: Colors.white,
              //                         ),
              //                         // đánh giá
              //                         Padding(
              //                           padding: EdgeInsets.all(6),
              //                           child: Row(
              //                             mainAxisAlignment:
              //                                 MainAxisAlignment.spaceBetween,
              //                             children: [
              //                               Row(
              //                                 children: [
              //                                   Icon(
              //                                     Icons.star,
              //                                     size: 20,
              //                                     color: Colors.yellow[700],
              //                                   ),
              //                                   SizedBox(
              //                                     width: 5,
              //                                   ),
              //                                   SmallText(
              //                                     text: 'Đánh giá XpressEat.',
              //                                     size: 15,
              //                                     color: Colors.black,
              //                                   )
              //                                 ],
              //                               ),
              //                               IconButton(
              //                                   onPressed: () {},
              //                                   icon: Icon(Icons.chevron_right))
              //                             ],
              //                           ),
              //                         ),
              //                         Divider(
              //                           thickness: 4,
              //                           // Adjust the thickness of the divider as needed
              //                           color: Colors.white,
              //                         ),
              //                         // hỗ trợ
              //                         Padding(
              //                             padding: EdgeInsets.all(6),
              //                             child: GestureDetector(
              //                               onTap: () {},
              //                               child: Row(
              //                                 mainAxisAlignment:
              //                                     MainAxisAlignment
              //                                         .spaceBetween,
              //                                 children: [
              //                                   Row(
              //                                     children: [
              //                                       Icon(
              //                                         Icons.headset_mic_sharp,
              //                                         size: 20,
              //                                         color: Colors.black,
              //                                       ),
              //                                       SizedBox(
              //                                         width: 5,
              //                                       ),
              //                                       SmallText(
              //                                         text: 'Trung tâm hỗ trợ',
              //                                         size: 15,
              //                                         color: Colors.black,
              //                                       )
              //                                     ],
              //                                   ),
              //                                   IconButton(
              //                                       onPressed: () {
              //                                         _launchPhoneCall(
              //                                             "0766837068");
              //                                       },
              //                                       icon: Icon(
              //                                           Icons.chevron_right))
              //                                 ],
              //                               ),
              //                             )),
              //                         Divider(
              //                           thickness: 4,
              //                           // Adjust the thickness of the divider as needed
              //                           color: Colors.white,
              //                         ),
              //                         // phiên bản app
              //                         Padding(
              //                           padding: EdgeInsets.all(6),
              //                           child: Row(
              //                             mainAxisAlignment:
              //                                 MainAxisAlignment.spaceBetween,
              //                             children: [
              //                               Row(
              //                                 children: [
              //                                   SmallText(
              //                                     text:
              //                                         "Phiên bản hiện tại: v${AppConstants.APP_VERSION}",
              //                                     size: 15,
              //                                     color: Colors.black,
              //                                   )
              //                                 ],
              //                               ),
              //                               IconButton(
              //                                   onPressed: () {},
              //                                   icon: Icon(Icons.chevron_right))
              //                             ],
              //                           ),
              //                         ),
              //                         Divider(
              //                           thickness: 4,
              //                           // Adjust the thickness of the divider as needed
              //                           color: Colors.white,
              //                         ),
              //                         // Đăng xuất
              //                         Padding(
              //                           padding: EdgeInsets.all(6),
              //                           child: Row(
              //                             mainAxisAlignment:
              //                                 MainAxisAlignment.center,
              //                             children: [
              //                               Row(
              //                                 children: [
              //                                   GestureDetector(
              //                                     onTap: () async {
              //                                       await FirebaseAuth.instance
              //                                           .signOut();
              //                                       navigateToHome();
              //                                     },
              //                                     child: SmallText(
              //                                       text: "Đăng xuất",
              //                                       size: 15,
              //                                       color: Colors.black,
              //                                     ),
              //                                   )
              //                                 ],
              //                               ),
              //                             ],
              //                           ),
              //                         ),
              //                         Divider(
              //                           thickness: 4,
              //                           // Adjust the thickness of the divider as needed
              //                           color: Colors.white,
              //                         ),
              //                         Container(
              //                           height:
              //                               MediaQuery.of(context).size.height -
              //                                   600,
              //                           child: Center(
              //                             child: BigText(
              //                               text: AppConstants.APP_NAME,
              //                               size: 30,
              //                               color: AppColors.mainColor,
              //                             ),
              //                           ),
              //                         )
              //                       ],
              //                     ),
              //                   ))),
              //         ],
              //       );
              //     }
              //     return Center(
              //         child: Column(
              //       mainAxisAlignment: MainAxisAlignment.center,
              //       children: [
              //         ElevatedButton(
              //           style: ElevatedButton.styleFrom(
              //             backgroundColor: AppColors.mainColor,
              //           ),
              //           onPressed: () {
              //             Navigator.of(context).pushNamed('/login');
              //           },
              //           child: Text(
              //             'Đăng nhập',
              //             style: TextStyle(color: Colors.white),
              //           ),
              //         ),
              //       ],
              //     ));
              //   },
              // ),
            ),
          ],
        ));
  }
}
