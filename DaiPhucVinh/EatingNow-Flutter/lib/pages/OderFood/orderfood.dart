import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/data/Api/OrderService.dart';
import 'package:fam/models/OrderRequest.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import 'package:fluttertoast/fluttertoast.dart';

import '../../storage/cartstorage.dart';
import '../../util/Colors.dart';
import '../../util/app_constants.dart';
import '../../util/dimensions.dart';
import '../food/recommened_food_detail.dart';
import '../home/getCurrentLocation_page.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:geolocator/geolocator.dart';
import 'package:url_launcher/url_launcher.dart';



class OrderPage extends StatefulWidget {

  const OrderPage({Key? key}) : super(key: key);

  @override
  _OrderPage createState() => _OrderPage();
}
class _OrderPage extends State<OrderPage> {
  final orderservice = OrderService();//lấy cửa hàng gần nhất
  OrderRequest orderRequest= OrderRequest();
  String addressdelivery = '';
  String nameAddressdelivery ='';
  late  SharedPreferences prefs;
  List<CartItem> cartItem = [];
  void _loadCartItems() async {
    List<CartItem> loadedItems = await CartStorage.getCartItems();
    setState(() {
      cartItem = loadedItems;
    });
  }
  late bool result;

  @override
  initState() {
    super.initState();
    getAddressDelivery();
    setState(() {
      result = false;
    });
    _loadCartItems();
  }
  Future<void> _launchInWebView(Uri url) async {
    try {
      // Sử dụng url_launcher để mở liên kết trong trình duyệt hệ thống
      if (await canLaunch(url.toString())) {
        await launch(url.toString());
      } else {
        throw Exception('Could not launch $url');
      }
    } catch (e) {
      print('Error launching URL: $e');
    }
  }
  void getAddressDelivery() async {
    prefs = await SharedPreferences.getInstance();
    setState(() {
      addressdelivery = prefs.getString('address') ?? '';
      nameAddressdelivery =prefs.getString('name') ?? '';
    });
  }
// Method to reload content
  void reloadContent() {
    _loadCartItems();
    setState(() {
      // Reload your content, update variables, etc.
      result = false; // Reset result to false if needed
    });
  }


  @override
  Widget build(BuildContext context) {
    //final arguments = ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
  //  final cartItem = arguments['data']; // Access 'your_data' here
    double totalAmount = cartItem.fold(0, (total, item) => total + (item.price * item.qty));
    if (result) {
      reloadContent(); // Automatically call reloadContent when result is true
    }
    return Scaffold(
      appBar: AppBar(
        title: Column(

          //crossAxisAlignment: CrossAxisAlignment.center,
          children: [
          BigText(text:'Chi tiết đơn hàng',size: Dimensions.font26,),
          SmallText(text: 'Khoảng cách tới vị trí bạn: 1km',color: Colors.black,size: Dimensions.font13,)
        ],),
        centerTitle: true, // Để căn giữa tiêu đề trên thanh AppBar
        // Các thuộc tính khác của AppBar
        backgroundColor: AppColors.mainColor, // Màu nền cho AppBar
     ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
           Column(
             mainAxisAlignment: MainAxisAlignment.center, // Đảm bảo dòng text ở giữa
             children: [
               Row(
                 children: [
                   Icon(
                     Icons.error,
                     color: Colors.redAccent, // Màu sắc của biểu tượng "!"
                   ),
                   Text(
                     'Kiểm tra lại thông tin vị trí nhận hàng',
                     style: TextStyle(fontSize: Dimensions.font16, fontStyle: FontStyle.italic,),
                   ),
                 ],
               ),
               Row(
                 children: [
                   Icon(
                     Icons.location_on,
                     color: Colors.green,
                     // Màu sắc của biểu tượng vị trí
                   ),
                   GestureDetector(
                     onTap: () {
                       Navigator.push(
                         context,
                         MaterialPageRoute(builder: (context) => LocationPage(link: "/order")), // truyền vào link cần trở lại khi đã chọn được vị trí
                       );
                     },
                     child: Row(
                       children: [
                         Container(
                           height: Dimensions.height45,
                           width: Dimensions.screenWidth - 110,
                           child:
                           BigText(text: addressdelivery, size: Dimensions.font13,),
                         ),
                         Icon(Icons.arrow_drop_down_rounded)
                       ],
                     ),
                   )

                 ],
               )

             ],
           ),
            Text(
              'Sản phẩm trong đơn hàng:',
              style: TextStyle(fontSize: Dimensions.font16,
                fontWeight: FontWeight.bold,
                 ),
              textAlign: TextAlign.center,
            ),
            SizedBox(height:10),
            for (var item in cartItem)

              Card(
                margin: EdgeInsets.only(bottom: 16),
                child: ListTile(
                  leading: Image.network(
                    item.uploadImage ?? "",
                    width: 80,
                    height: 80,
                    fit: BoxFit.cover,
                  ),
                  title: Text(item.foodName + ' x ${item.qty}'),
                  subtitle: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: <Widget>[

                      BigText(text: NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(item.price * item.qty ?? 0), color: AppColors.mainColor,size: Dimensions.font13,),
                      GestureDetector(
                        onTap: () async {
                          final kq = await Navigator.pushNamed(
                            context,
                            "/productdetail",
                            arguments: {'data': item},
                          );

                          if (kq != null && kq is bool) {
                            setState(() {
                              result = kq;
                            });
                          }
                        },
                        child: Row(
                          children: <Widget>[
                            Icon(
                              Icons.edit,
                              color: Colors.blueAccent,
                              size: 15,
                            ),
                            Text('Chỉnh sửa', style: TextStyle(color: Colors.blueAccent)),
                          ],
                        ),
                      ),                    ],
                  ),
                ),
              ),
            Card(
              margin: EdgeInsets.only(bottom: 16),
              child: ListTile(
                title: Text(
                  'Tổng cộng: ' + NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(totalAmount),
                  style: TextStyle(fontWeight: FontWeight.bold),
                ),
              ),
            ),
            ElevatedButton(
              onPressed: () async {
              try {
              orderRequest.completeName = "Đông Châu";
              orderRequest.formatAddress = "Đại học An Giang";
              orderRequest.nameAddress = "Đại học An Giang";
              orderRequest.orderLine = cartItem;
              orderRequest.customerId = "kTFgvQgpmyYMW1fhyopWYQSQx3C2";
              orderRequest.intoMoney = totalAmount.toInt();
              orderRequest.transportFee = 15000;
              orderRequest.totalAmt = totalAmount.toInt() - 15000;
              orderRequest.userId = 14;
              orderRequest.latitude = 10.3716555;
              orderRequest.longitude = 105.432343;
              orderRequest.recipientName = "Đông";
              orderRequest.recipientPhone = "0766837068";
              orderRequest.payment = null;
              orderRequest.userId = 14;
              orderRequest.payment ="PaymentOnDelivery";

              final responseBody = await orderservice.postOrder(orderRequest);
              print(responseBody.data);
              print(responseBody.success);
              print(responseBody.Message);

              // Xử lý kết quả trả về từ API
              if (responseBody.success == true) {
              // Thực hiện các hành động thành công
                print('Order posted successfully');
                if(responseBody.Message != ""){
                  _launchInWebView(Uri.parse(responseBody.Message?? ""));
                }
                else {
                  Fluttertoast.showToast(msg: "Đặt món ăn thành công",
                      toastLength: Toast.LENGTH_LONG,
                      gravity: ToastGravity.BOTTOM_LEFT,
                      backgroundColor: AppColors.toastSuccess,
                      textColor: Colors.black54,
                      timeInSecForIosWeb: 1,
                      fontSize: 10);
                  Navigator.pushReplacement(
                      context,
                      Navigator.pushNamed(context, "/") as Route<Object?>
                  );
                }
              } else {
              // Xử lý lỗi từ máy chủ
              print('Failed to post order. ${responseBody.Message}');
              }
              } catch (e) {
              // Xử lý lỗi kết nối hoặc lỗi khác
              print('Error posting order: $e');
              }
              },
              child: Text('Đặt đơn'),
            ),
          ],
        ),
      ),
    );
  }


}
