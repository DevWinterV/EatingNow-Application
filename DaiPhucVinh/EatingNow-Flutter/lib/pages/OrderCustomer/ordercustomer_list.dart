import 'dart:async';

import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/data/Api/OrderService.dart';
import 'package:fam/models/ordercustomerRequest_model.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/app_constants.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import '../../models/ordercustomerResponse_model.dart';

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
    return StreamBuilder<OrderCustomerResponse?>(
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
            return ListView.builder(
                itemCount: snapshot.data?.data?.length ?? 0,
                itemBuilder: (itemBuilder, index){
                  final order = snapshot.data?.data?[index];
                  return ListTile(
                    title: BigText(text: order?.orderHeaderId ?? ""),
                    subtitle: SmallText(text: order?.creationDate ?? "",),
                    iconColor: AppColors.mainColor,
                    trailing: SmallText(text: order?.totalAmt.toString() ?? "",),
                  );
                }
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
    });
  }
}
