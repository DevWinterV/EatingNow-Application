import 'dart:async';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/models/order_details_model.dart';
import 'package:fam/models/ordercustomerResponse_model.dart';
import 'package:flutter/material.dart';
import 'package:flutter/cupertino.dart';
import 'package:intl/intl.dart';
import 'package:url_launcher/url_launcher.dart';
import '../../Widget/Big_text.dart';
import '../../data/Api/OrderService.dart';
import '../../util/Colors.dart';
import '../../util/dimensions.dart';

class OrderDetailsPage extends StatefulWidget {
  const OrderDetailsPage({super.key});

  @override
  State<OrderDetailsPage> createState() => _OrderDetailsPageState();
}

class _OrderDetailsPageState extends State<OrderDetailsPage> {

  late Data order;
  late StreamController<OrderDetailsResponse?> _OrderDetailtreamController;
  final orderservice = OrderService();
  @override
  void initState(){
    super.initState();
    order = Data();
    _OrderDetailtreamController = StreamController<OrderDetailsResponse?>();
    _initStateAsync();
  }

  void _initStateAsync() async {
    await Future.delayed(Duration.zero); // This ensures that the context is available
    final arguments = ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
    setState(() {
      order = arguments['data'] as Data; // Nhận dữ liệu orderId
    });
    print( 'order.creationDate ${order.creationDate ?? ""}');
    _initStreamOrderList();
  }
  _launchGoogleMaps(double latitude, double longitude ) async {
    // Tạo URL dựa trên tọa độ
    String googleMapsUrl = 'https://www.google.com/maps?q=$latitude,$longitude';

    // Kiểm tra xem ứng dụng Google Maps đã được cài đặt trên thiết bị hay không
    if (await canLaunch(googleMapsUrl)) {
      await launch(googleMapsUrl);
    } else {
      // Trường hợp không thể mở ứng dụng Google Maps, bạn có thể hướng dẫn người dùng cài đặt ứng dụng này.
      throw 'Could not launch $googleMapsUrl';
    }
  }

  void _initStreamOrderList() async {
    final orderDetail = await _GetListOrderLineDetails();
    _OrderDetailtreamController.sink.add(orderDetail);
  }

  Future<OrderDetailsResponse?> _GetListOrderLineDetails() async{
    print('order.orderHeaderId ${order.orderHeaderId}');
    final response = await orderservice.GetListOrderLineDetails(order.orderHeaderId ?? "");
    if(response != null){
      return response;
    }
    return null;
  }
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Column(
          children: [
            Text(
              'Xem chi tiết đơn đặt hàng',
              overflow: TextOverflow.ellipsis,
              maxLines: 1,
              style: TextStyle(
                fontSize: Dimensions.font20,
              ), // Số dòng tối đa hiển thị (có thể điều chỉnh theo nhu cầu của bạn)
            ),
            SmallText(text: order.orderHeaderId ?? "", size: Dimensions.font13, color: Colors.black,)
          ],
        ),
        centerTitle: true, // Để căn giữa tiêu đề trên thanh AppBar
        // Các thuộc tính khác của AppBar
        backgroundColor: AppColors.mainColor, // Màu nền cho AppBar
      ),
      body:  RefreshIndicator(
          color: AppColors.iconColor1,
          onRefresh: () async{
          },
          child:  SingleChildScrollView(
            child:
            Column(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                Container(
                    decoration: BoxDecoration(
                      color: AppColors.buttonBackqroundColor,
                      // borderRadius: BorderRadius.only(
                      //   topLeft: Radius.circular(Dimensions.radius15 * 1),
                      //   topRight: Radius.circular(Dimensions.radius15 * 1),
                      // ),
                    ),
                    height: 130,
                    width: MediaQuery.of(context).size.width,
                    child:  Column(
                      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Padding(padding: EdgeInsets.only(left: 12, right: 12, top: 5, bottom:5),
                          child: Column(
                            children: [
                              Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    'Người nhận: ',
                                    style: TextStyle(fontWeight: FontWeight.bold),
                                  ),
                                  Text(
                                    order.recipientName ?? "",
                                    style: TextStyle(fontWeight: FontWeight.bold),
                                  ),
                                ],
                              ),
                              Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: [
                                  Text(
                                    'Số điện thoại: ',
                                    style: TextStyle(fontWeight: FontWeight.bold),
                                  ),
                                  Text(
                                    order?.recipientPhone  ?? "",
                                    style: TextStyle(
                                      fontWeight: FontWeight.bold,
                                    ),
                                  ),
                                ],
                              ),
                              Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: [
                                  Text(
                                    'Địa chỉ nhận: ',
                                    style: TextStyle(fontWeight: FontWeight.bold),
                                  ),
                                  GestureDetector(
                                    onTap: () {
                                      _launchGoogleMaps(order.latitude?? 0.0, order.longitude?? 0.0);
                                    },
                                    child: SmallText(
                                      text: 'Xem trên bản đồ',
                                      color: Colors.blue[400],
                                      size: Dimensions.font13,
                                    ),
                                  )                                ],
                              ),
                              BigText(
                                  text:
                                  order.formatAddress  ??  "",
                                  size: Dimensions.font14,
                                  ),
                            ],
                          ),
                        ),
                      ],
                    )// ĐẶT ĐƠN,
                ),
                StreamBuilder<OrderDetailsResponse?>(
                    initialData: null,
                    stream: _OrderDetailtreamController.stream,
                    builder: (builder, snapshot){
                      if(snapshot.connectionState == ConnectionState.waiting){
                        return Center(
                          child: CircularProgressIndicator(
                            color: AppColors.mainColor,
                          ),
                        );
                      }
                      if(snapshot.hasData && snapshot.data!.data!.length! > 0 ){
                        final orderDetails = snapshot.data!.data;
                        return Column(
                          children: [
                            Padding(
                              padding: EdgeInsets.only(top: 5,bottom: 5, left: 12, right: 12),
                              child:
                              Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: [
                                  Text(
                                    'Sản phẩm trong đơn: ',
                                    style: TextStyle(fontWeight: FontWeight.bold),
                                  ),
                                  Text(
                                    '${orderDetails?.length.toString() ?? ""} sản phẩm',
                                    style: TextStyle(
                                        fontWeight: FontWeight.normal,
                                        fontSize: 12
                                    ),
                                  ),
                                ],
                              ),
                            ),

                            Padding(
                                padding: EdgeInsets.only(top: 0,bottom: 5),
                                child:   Container(
                                  height: MediaQuery.of(context).size.height - 440,
                                  child:
                                  orderDetails!.length > 0 ?
                                  ListView.builder(
                                    itemCount: orderDetails.length ,
                                    itemBuilder: (BuildContext context, int index) {
                                      var item = orderDetails[index];
                                      final totalItem = (item.qty ?? 0) * (item.price ?? 0);
                                      return
                                        Card(
                                          color: Colors.white,
                                          margin: EdgeInsets.only(bottom: 10),
                                          child: ListTile(
                                            leading: Image.network(
                                              item.uploadImage ?? "",
                                              width: 80,
                                              height: 80,
                                              fit: BoxFit.cover,
                                            ),
                                            title: Text(item.foodName ?? ""),
                                            subtitle: Column(
                                              crossAxisAlignment: CrossAxisAlignment.start,
                                              children: <Widget>[
                                                SizedBox(height: 1),
                                                Row(
                                                  children: [
                                                    BigText(text: "Số lượng: ${item.qty}", color: Colors.black, size: Dimensions.font13),
                                                    SizedBox(width: 20),
                                                    BigText(text: "Đơn giá: ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(item.price ?? 0)}", color: Colors.black, size: Dimensions.font13),
                                                  ],
                                                ),
                                                item.description != "" ?
                                                    Column(
                                                      children: [
                                                        SizedBox(height: 1),
                                                        BigText(text: "Ghi chú: ${item.description }", color: Colors.black, size: Dimensions.font13),
                                                      ],
                                                    ) : SizedBox(),
                                                SizedBox(height: 1),
                                                BigText(text: "Thành tiền: ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(totalItem)}", color: AppColors.mainColor, size: Dimensions.font13),
                                              ],
                                            ),
                                          ),
                                        );
                                    },
                                  ) :
                                  Center(
                                      child: Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        crossAxisAlignment: CrossAxisAlignment.center,
                                        children: [
                                          Image.asset(
                                            "assets/image/emptycart.png",
                                            height: 100,
                                            width: 100,),
                                          Text(
                                            "Không có sản phẩm trong đơn",
                                            style: TextStyle(
                                                fontSize: Dimensions.font16,
                                                fontWeight: FontWeight.bold,
                                                color: Colors.grey
                                            ),
                                          )
                                        ],
                                      )
                                  )
                              ),
                            )
                          ],
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
      bottomNavigationBar:
        Container(
          decoration: BoxDecoration(
            color: AppColors.buttonBackqroundColor,
            borderRadius: BorderRadius.only(
              topLeft: Radius.circular(Dimensions.radius15 * 1),
              topRight: Radius.circular(Dimensions.radius15 * 1),
            ),
          ),
          height: 200,
          width: MediaQuery.of(context).size.width,
          child:  Column(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Padding(padding: EdgeInsets.only(left: 12, right: 12, top: 5),
                child: Column(
                  children: [
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          'Đặt lúc: ',
                          style: TextStyle(fontWeight: FontWeight.bold),
                        ),
                        Text(
                          DateFormat('dd/MM/yyyy HH:mm:ss').format(DateTime.parse(order?.creationDate ?? "").toLocal()),
                          style: TextStyle(fontWeight: FontWeight.bold),
                        ),
                      ],
                    ),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          'Trạng thái: ',
                          style: TextStyle(fontWeight: FontWeight.bold),
                        ),
                        Text(
                          order?.status == true ? "Đã xác nhận" : "Chờ xác nhận",
                          style: TextStyle(
                            fontWeight: FontWeight.bold,
                            color: order?.status == true ? Colors.green : Colors.red,
                          ),
                        ),
                      ],
                    ),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          'Thanh toán: ',
                          style: TextStyle(fontWeight: FontWeight.bold),
                        ),
                        BigText(
                            text:
                            order?.paymentStatusID == 2 ? 'Đã thanh toán Online' : "Thanh toán khi nhận hàng",
                            size: Dimensions.font14,
                            color:  order?.paymentStatusID == 2 ? Colors.green : Colors.red
                        ),
                      ],
                    ),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          'Phí giao hàng: ',
                          style: TextStyle(fontWeight: FontWeight.bold),
                        ),
                        Text(
                          NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(order.transportFee ?? 0),
                          style: TextStyle(fontWeight: FontWeight.bold),
                        ),
                      ],
                    ),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          'Tổng tiền sản phẩm: ',
                          style: TextStyle(fontWeight: FontWeight.bold),
                        ),
                        Text(
                          NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(order.totalAmt ?? 0),
                          style: TextStyle(fontWeight: FontWeight.bold),
                        ),
                      ],
                    ),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          'Tổng cộng: ' ,
                          style: TextStyle(fontWeight: FontWeight.bold,
                              color: Colors.red
                          ),
                        ),
                        Text(
                          NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(order.intoMoney ?? 0),
                          style: TextStyle(
                              fontWeight: FontWeight.bold,
                            color: Colors.red
                          ),
                        ),
                      ],
                    ),
                  ],
                ),
              ),
              Padding(
                child: Center(
                  child:ElevatedButton(
                    style: ElevatedButton.styleFrom(
                      // Customize the background color
                      primary: AppColors.mainColor,
                      // Customize the text color
                      onPrimary: Colors.white,
                      // Add other customizations as needed
                      padding: EdgeInsets.only(right: 20.0, left: 20.0),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(10.0),
                      ),
                      minimumSize: Size(double.infinity, 50), // Đặt kích thước tối thiểu cho nút
                    ),
                    onPressed: () {
                      Navigator.of(context).pop();
                    },
                    child: Text('Theo dõi tình trạng giao hàng', style: TextStyle(fontSize: Dimensions.font16),),
                  ), ) ,
                padding: EdgeInsets.only(left: 10, right: 10)
              )
            ],
          )// ĐẶT ĐƠN,
      )
    );
  }
}
