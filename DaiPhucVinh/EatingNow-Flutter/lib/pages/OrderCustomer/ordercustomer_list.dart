import 'dart:async';
import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/data/Api/OrderService.dart';
import 'package:fam/models/ordercustomerRequest_model.dart';
import 'package:fam/util/Colors.dart';
import 'package:intl/intl.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import '../../models/orderTypeSelectTed.dart';
import '../../models/ordercustomerResponse_model.dart';
import '../../util/dimensions.dart';
import '../../util/notificationService.dart';

class OrderCustomerPage extends StatefulWidget {
  const OrderCustomerPage({super.key});
  @override
  State<OrderCustomerPage> createState() => _OrderCustomerPageState();
}

class _OrderCustomerPageState extends State<OrderCustomerPage> {
  late StreamController<OrderCustomerResponse?> _OrderListStreamController;
  late StreamController<
      List<OrdertypeSelected>?> _OrderTypeSelectListStreamController;
  int? selectedIndex = 0;
  final List<OrdertypeSelected> listOrderTypeSelect = [];
  final orderservice = OrderService();

  @override
  void initState() {
    super.initState();
    _OrderListStreamController = StreamController<OrderCustomerResponse?>();
    _OrderTypeSelectListStreamController =
        StreamController<List<OrdertypeSelected>?>();
    NotificationService.onclickNotification.add("null");
    NotificationService.onclickNotificationBack.add("null");
    _initStreamOrderList(0, "");
    _initListOrdertype();
  }

  void _initListOrdertype() {
    listOrderTypeSelect.add(OrdertypeSelected("Tất cả", "", 0));
    listOrderTypeSelect.add(OrdertypeSelected("Chờ xác nhận", "false", 1));
    listOrderTypeSelect.add(OrdertypeSelected("Đã xác nhận", "true", 1));
    listOrderTypeSelect.add(OrdertypeSelected("Đang giao", "true", 1));
    listOrderTypeSelect.add(OrdertypeSelected("Đã hoàn thành", "true", 1));
    _OrderTypeSelectListStreamController.sink.add(listOrderTypeSelect);
  }

  void _initStreamOrderList(int? orderType, String? status) async {
    final orderlist = await _takeorderBycustomer(orderType, status);
    _OrderListStreamController.sink.add(orderlist);

  }

  Future<OrderCustomerResponse?> _takeorderBycustomer(int? ordertype,
      String? status) async {
    final request = OrderCustomerRequest(
        customerId: FirebaseAuth.instance.currentUser?.uid ?? "",
        orderType: ordertype ?? 0,
        status: status ?? ""
    );
    final response = await orderservice.TakeOrderByCustomer(request);
    if (response != null) {
      return response;
    }
    return null;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          'Đơn hàng của bạn',
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
      body: RefreshIndicator(
          color: AppColors.iconColor1,
          onRefresh: () async {
            _initStreamOrderList(0, "");
          },
          child: Column(
            children: [
              SingleChildScrollView(
                child:
                Column(
                  children: [
                    StreamBuilder<List<OrdertypeSelected>?>(
                        initialData: null,
                        stream: _OrderTypeSelectListStreamController.stream,
                        builder: (builder, snapshot) {
                          return Container(
                              height: 40,
                              child: ListView.separated(
                                  itemCount: 5, // Add 1 for the "All" item
                                  scrollDirection: Axis.horizontal,
                                  separatorBuilder: (context, index) =>
                                      SizedBox(width: 0),
                                  itemBuilder: (context, index) {
                                    final selectTab = snapshot.data![index];
                                    return buildCategoryItem(
                                        index,
                                        selectTab.orderType ?? 0,
                                        selectTab.status ?? "",
                                        selectTab.nameTab ?? "");
                                  }
                              )
                          );
                        }),
                    StreamBuilder<OrderCustomerResponse?>(
                        initialData: null,
                        stream: _OrderListStreamController.stream,
                        builder: (builder, snapshot) {
                          if (snapshot.connectionState ==
                              ConnectionState.waiting) {
                            return Center(
                              child: CircularProgressIndicator(
                                color: AppColors.mainColor,
                              ),
                            );
                          }
                          if (!snapshot.hasData ||
                              snapshot.data!.data!.length! == 0) {
                            return Center(
                                child:
                                Column(
                                  mainAxisAlignment: MainAxisAlignment.center,
                                  crossAxisAlignment: CrossAxisAlignment.center,
                                  children: [
                                    // Image.asset(
                                    //   "assets/image/empty-box.png",
                                    //   height: 100,
                                    //   width: 100,),
                                    BigText(
                                      text: "Không có dữ liệu",
                                      color: Colors.grey,),
                                  ],
                                )
                            );
                          }
                          if (snapshot.hasData &&
                              snapshot.data!.data!.length! > 0) {
                            return Container(
                              height: Dimensions.screenHeight -136 ,
                              child: ListView.builder(
                                  physics: AlwaysScrollableScrollPhysics(),
                                  itemCount: snapshot.data?.data?.length ?? 0,
                                  itemBuilder: (itemBuilder, index) {
                                    final order = snapshot.data?.data?[index];
                                    return Padding(padding: EdgeInsets.only(
                                        top: 5, bottom: 5),
                                        child: Column(
                                          children: [
                                            // ListTile(
                                            //   title: BigText(text: order?.orderHeaderId ?? ""),
                                            //   subtitle:
                                            //   Column(
                                            //     mainAxisAlignment: MainAxisAlignment.start,
                                            //     crossAxisAlignment: CrossAxisAlignment.start,
                                            //     children: [
                                            //       SmallText(
                                            //         text: "Đặt lúc: ${DateFormat('dd/MM/yyyy HH:mm:ss').format(DateTime.parse(order?.creationDate ?? "").toLocal())}" ,
                                            //         color: Colors.black,
                                            //       ),
                                            //       Row(
                                            //         children: [
                                            //           SmallText(text: "Trạng thái: ",color: Colors.black,),
                                            //           SmallText(text: order?.status == true ? "Đã xét duyệt" : "Chờ xét duyệt" , color: order?.status == true ? Colors.green : Colors.red,),
                                            //         ],
                                            //       ),
                                            //       Row(
                                            //         mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            //         children: [
                                            //           BigText(text:
                                            //           'Tổng tiền: ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(order?.intoMoney ?? 0)}',
                                            //               size: Dimensions.font13,
                                            //               color: Colors.red[400]
                                            //           ),
                                            //         ],
                                            //       ),
                                            //       BigText(
                                            //           text:
                                            //           order?.paymentStatusID == 2 ? 'Đã thanh toán Online' : "Thanh toán khi nhận hàng",
                                            //           size: Dimensions.font13,
                                            //           color:  order?.paymentStatusID == 2 ? Colors.green : Colors.red
                                            //       ),
                                            //     ],
                                            //   ),
                                            //   selectedColor: Colors.white60,
                                            //   iconColor: AppColors.mainColor,
                                            //   trailing: Column(
                                            //     mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            //     crossAxisAlignment: CrossAxisAlignment.center,
                                            //     children: [
                                            //       Container(
                                            //         height: 20,
                                            //         child:  ElevatedButton(
                                            //           onPressed: (){
                                            //             Navigator.of(context).pushNamed("/ordedetails", arguments: {"data": order});
                                            //           },
                                            //           child: BigText(text:
                                            //           'Xem chi tiết ',
                                            //               size: Dimensions.font13,
                                            //               color: Colors.white
                                            //           ),
                                            //           style: ElevatedButton.styleFrom(
                                            //             backgroundColor: Colors.blue,
                                            //             textStyle: TextStyle(
                                            //                 color: Colors.white
                                            //             ),
                                            //           ),
                                            //         ),
                                            //       ),
                                            //       order?.status != true ?
                                            //       Container(
                                            //         height: 20,
                                            //         child:  ElevatedButton(
                                            //           onPressed: (){
                                            //             Navigator.of(context).pushNamed("/ordedetails", arguments: {"data": order});
                                            //           },
                                            //           child:  BigText(text:
                                            //           'Hủy đơn',
                                            //               size: Dimensions.font13,
                                            //               color: Colors.white,
                                            //           ),
                                            //           style: ElevatedButton.styleFrom(
                                            //             backgroundColor: Colors.red[400],
                                            //           ),
                                            //         ),
                                            //       )
                                            //           :
                                            //       SizedBox(),
                                            //     ],
                                            //   ),
                                            // ),
                                            Padding(
                                              padding: EdgeInsets.only(
                                                  left: 8, right: 8),
                                              child: Row(
                                                mainAxisAlignment: MainAxisAlignment
                                                    .spaceBetween,
                                                crossAxisAlignment: CrossAxisAlignment
                                                    .center,
                                                children: [
                                                  Column(
                                                    mainAxisAlignment: MainAxisAlignment
                                                        .spaceBetween,
                                                    crossAxisAlignment: CrossAxisAlignment
                                                        .start,
                                                    children: [
                                                      Container(
                                                        width: 200,
                                                        child: BigText(
                                                          text: order
                                                              ?.orderHeaderId ??
                                                              "",
                                                          size: Dimensions
                                                              .font16,),
                                                      ),
                                                      SmallText(
                                                        text: "Cửa hàng: ${order?.storeName ?? ""}",
                                                        color: Colors.black,
                                                      ),
                                                      SmallText(
                                                        text: "Đặt lúc: ${DateFormat(
                                                            'dd/MM/yyyy HH:mm:ss')
                                                            .format(
                                                            DateTime.parse(order
                                                                ?.creationDate ??
                                                                "")
                                                                .toLocal())}",
                                                        color: Colors.black,
                                                      ),
                                                      BigText(
                                                          text:
                                                          order
                                                              ?.paymentStatusID ==
                                                              2
                                                              ? 'Đã thanh toán Online'
                                                              : "Thanh toán khi nhận hàng",
                                                          size: Dimensions
                                                              .font13,
                                                          color: order
                                                              ?.paymentStatusID ==
                                                              2
                                                              ? Colors.green
                                                              : Colors.black
                                                      ),
                                                      Row(
                                                        mainAxisAlignment: MainAxisAlignment
                                                            .spaceBetween,
                                                        children: [
                                                          BigText(text:
                                                          'Tổng tiền: ${NumberFormat
                                                              .currency(
                                                              locale: 'vi_VN',
                                                              symbol: '₫')
                                                              .format(order
                                                              ?.intoMoney ??
                                                              0)}',
                                                              size: Dimensions
                                                                  .font13,
                                                              color: Colors
                                                                  .red[400]
                                                          ),
                                                        ],
                                                      ),
                                                    ],
                                                  ),
                                                  Column(
                                                    mainAxisAlignment: MainAxisAlignment.center,
                                                    crossAxisAlignment: CrossAxisAlignment
                                                        .center,
                                                    children: [
                                                      SmallText(
                                                      text: order?.status == true && order?.shippingstatus == 3
                                                      ? "Giao thành công"
                                                            : order?.status == true && order?.shippingstatus == 2
                                                      ? "Đang giao hàng"
                                                          : order?.status == true && order?.shippingstatus == 1
                                                      ? "Đang chuẩn bị"
                                                          : order?.status == true && order?.shippingstatus == 0
                                                      ? "Đã xác nhận"
                                                        :
                                                        "Chưa xác nhận ",
                                                        color: order?.status == true ? Colors.green : Colors.red,
                                                        size: Dimensions.font13,
                                                      ),
                                                      SizedBox(height: 6,),
                                                      Container(
                                                        height: 20,
                                                        width: 100,
                                                        child: ElevatedButton(
                                                          onPressed: () {
                                                            Navigator.of(
                                                                context)
                                                                .pushNamed(
                                                                "/ordedetails",
                                                                arguments: {
                                                                  "data": order
                                                                });
                                                          },
                                                          child: BigText(text:
                                                          'Chi tiết ',
                                                              size: Dimensions
                                                                  .font13,
                                                              color: Colors
                                                                  .white
                                                          ),
                                                          style: ElevatedButton
                                                              .styleFrom(
                                                            backgroundColor: Colors
                                                                .blue,
                                                            textStyle: TextStyle(
                                                                color: Colors
                                                                    .white
                                                            ),
                                                          ),
                                                        ),
                                                      ),
                                                      SizedBox(height: 6,),
                                                      order?.status != true && order?.shippingstatus != 3
                                                          ?
                                                      Container(
                                                        width: 100,
                                                        height: 20,
                                                        child: ElevatedButton(
                                                          onPressed: () {
                                                            Navigator.of(
                                                                context)
                                                                .pushNamed(
                                                                "/ordedetails",
                                                                arguments: {
                                                                  "data": order
                                                                });
                                                          },
                                                          child: BigText(text:
                                                          'Hủy đơn',
                                                            size: 12,
                                                            color: Colors.white,
                                                          ),
                                                          style: ElevatedButton
                                                              .styleFrom(
                                                            backgroundColor: Colors
                                                                .red[400],
                                                          ),
                                                        ),
                                                      )
                                                          :
                                                      order?.status == true && order?.shippingstatus == 3
                                                          ?
                                                      Container(
                                                        width: 100,
                                                        height: 20,
                                                        child: ElevatedButton(
                                                          onPressed: () {
                                                            Navigator.of(
                                                                context)
                                                                .pushNamed(
                                                                "/rating",
                                                                arguments: {
                                                                  "orderID": order?.orderHeaderId ?? ""
                                                                });
                                                          },
                                                          child: BigText(text:
                                                          'Đánh giá',
                                                            size: 12,
                                                            color: Colors
                                                                .white,
                                                          ),
                                                          style: ElevatedButton
                                                              .styleFrom(
                                                            backgroundColor: Colors.yellow[700]
                                                          ),
                                                        ),
                                                      )
                                                          :
                                                      SizedBox()
                                                    ],
                                                  ),
                                                ],
                                              ),
                                            ),
                                            Divider(thickness: 8,
                                              color: Colors.white,)
                                          ],
                                        )
                                    );
                                  }

                              ),
                            );
                          }
                          if (snapshot.connectionState ==
                              ConnectionState.waiting) {
                            return Center(
                              child: CircularProgressIndicator(
                                color: AppColors.mainColor,
                              ),
                            );
                          }
                          return SizedBox();
                        })
                  ],
                ),
              )
            ],
          )
      ),
    );
  }
  Widget buildCategoryItem(int index, int ordertype, String status, String tabname) {
    bool isSelected = index == selectedIndex;
    return  Padding(
      padding: EdgeInsets.only(left: 10, right: 10, top: 5, bottom: 5),
      child: GestureDetector(
        onTap: () {
          setState(() {
            selectedIndex = index;
          });
          _initStreamOrderList(ordertype, status);
        },
        child: Container(
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(5),
          ),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              BigText(
                text: tabname,
                size: 15,
                color: isSelected ? AppColors.mainColor : Colors.black,
                // Change text color based on isSelected
                maxlines: 2,
              ),
            ],
          ),
        ),
      ),
    );
  }
}

