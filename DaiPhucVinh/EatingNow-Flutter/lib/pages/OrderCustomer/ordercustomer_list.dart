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
  final orderservice = OrderService();
  @override
  void initState(){
    super.initState();
    _OrderListStreamController = StreamController<OrderCustomerResponse?>();
    _initStreamOrderList();
  }

  void _initStreamOrderList() async {
     final orderlist = await _takeorderBycustomer();
     _OrderListStreamController.sink.add(orderlist);
     NotificationService.onclickNotification.add("null");
     NotificationService.onclickNotificationBack.add("null");
  }

  Future<OrderCustomerResponse?> _takeorderBycustomer() async{
    final request = OrderCustomerRequest(
      customerId: FirebaseAuth.instance.currentUser?.uid ?? "",
      orderType: 0,
      status: ""
    );
    final response = await orderservice.TakeOrderByCustomer(request);
    if(response != null){
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
      body:  RefreshIndicator(
          color: AppColors.iconColor1,
          onRefresh: () async{
            _initStreamOrderList();
          },
            child:  SingleChildScrollView(
                  child:
                  Column(
                    children: [
                      StreamBuilder<OrderCustomerResponse?>(
                          initialData: null,
                          stream: _OrderListStreamController.stream,
                          builder: (builder, snapshot){
                            if(snapshot.connectionState == ConnectionState.waiting){
                              return Center(
                                child: CircularProgressIndicator(
                                  color: AppColors.mainColor,
                                ),
                              );
                            }
                            if(!snapshot.hasData || snapshot.data!.data!.length! == 0 ){
                              return Center(
                                  child:
                                  Column(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    children: [
                                      Image.asset(
                                        "assets/image/empty-box.png",
                                        height: 100,
                                        width: 100,),
                                      BigText(text: "Bạn hiện chưa đặt đơn hàng nào", color: Colors.grey,),
                                    ],
                                  )
                              );
                            }
                            if(snapshot.hasData && snapshot.data!.data!.length! > 0 ){
                              return Container(
                                height: snapshot.data!.data!.length! * 156,
                                child: ListView.builder(
                                    physics: NeverScrollableScrollPhysics(),
                                    itemCount: snapshot.data?.data?.length ?? 0,
                                    itemBuilder: (itemBuilder, index){
                                      final order = snapshot.data?.data?[index];
                                      return Padding(padding: EdgeInsets.only(top: 5, bottom: 5),
                                          child:  Column(
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
                                                  padding: EdgeInsets.only(left: 8, right: 8),
                                                  child: Row(
                                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                                    crossAxisAlignment: CrossAxisAlignment.center,
                                                    children: [
                                                      Column(
                                                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                                        crossAxisAlignment: CrossAxisAlignment.start,
                                                        children: [
                                                          Container(
                                                            width: 200,
                                                            child:BigText(text: order?.orderHeaderId ?? "", size: Dimensions.font16,),
                                                          ),
                                                          SmallText(
                                                            text: "Đặt lúc: ${DateFormat('dd/MM/yyyy HH:mm:ss').format(DateTime.parse(order?.creationDate ?? "").toLocal())}" ,
                                                            color: Colors.black,
                                                          ),
                                                          Row(
                                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                                            children: [
                                                              BigText(text:
                                                              'Tổng tiền: ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(order?.intoMoney ?? 0)}',
                                                                  size: Dimensions.font13,
                                                                  color: Colors.red[400]
                                                              ),
                                                            ],
                                                          ),
                                                          BigText(
                                                              text:
                                                              order?.paymentStatusID == 2 ? 'Đã thanh toán Online' : "Thanh toán khi nhận hàng",
                                                              size: Dimensions.font13,
                                                              color:  order?.paymentStatusID == 2 ? Colors.green : Colors.red
                                                          ),
                                                        ],
                                                      ),
                                                      Column(
                                                        crossAxisAlignment: CrossAxisAlignment.center,
                                                        children: [
                                                          SmallText(text: order?.status == true ? "Đã xác nhận" : "Chờ xác nhận" , color: order?.status == true ? Colors.green : Colors.red, size: Dimensions.font13,),
                                                          SizedBox(height: 6,),
                                                          Container(
                                                            height: 20,
                                                            width: 100,
                                                            child:  ElevatedButton(
                                                              onPressed: (){
                                                                Navigator.of(context).pushNamed("/ordedetails", arguments: {"data": order});
                                                              },
                                                              child: BigText(text:
                                                              'Chi tiết ',
                                                                  size: Dimensions.font13,
                                                                  color: Colors.white
                                                              ),
                                                              style: ElevatedButton.styleFrom(
                                                                backgroundColor: Colors.blue,
                                                                textStyle: TextStyle(
                                                                    color: Colors.white
                                                                ),
                                                              ),
                                                            ),
                                                          ),
                                                          SizedBox(height: 6,),
                                                          order?.status != true ?
                                                          Container(
                                                            width: 100,
                                                            height: 20,
                                                            child:  ElevatedButton(
                                                              onPressed: (){
                                                                Navigator.of(context).pushNamed("/ordedetails", arguments: {"data": order});
                                                              },
                                                              child:  BigText(text:
                                                              'Hủy đơn',
                                                                size: Dimensions.font13,
                                                                color: Colors.white,
                                                              ),
                                                              style: ElevatedButton.styleFrom(
                                                                backgroundColor: Colors.red[400],
                                                              ),
                                                            ),
                                                          )
                                                              :
                                                          SizedBox(),
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
                            if(snapshot.connectionState == ConnectionState.waiting){
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
        ),
    );
  }
}
