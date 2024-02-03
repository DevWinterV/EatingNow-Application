import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/data/Api/OrderService.dart';
import 'package:fam/models/LocationData.dart';
import 'package:fam/models/OrderRequest.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import 'package:fluttertoast/fluttertoast.dart';

import '../../storage/cartstorage.dart';
import '../../storage/locationstorage.dart';
import '../../util/Colors.dart';
import '../../util/dimensions.dart';
import '../home/getCurrentLocation_page.dart';
import 'package:url_launcher/url_launcher.dart';



class OrderPage extends StatefulWidget {

  const OrderPage({Key? key}) : super(key: key);

  @override
  _OrderPage createState() => _OrderPage();
}
class _OrderPage extends State<OrderPage> {
  TextEditingController namecontroller = TextEditingController();
  TextEditingController phoneNumbercontroller = TextEditingController();
  late LocationStorage localtionStorge;
  FirebaseAuth _auth = FirebaseAuth.instance;
  final orderservice = OrderService();//lấy cửa hàng gần nhất
  OrderRequest orderRequest= OrderRequest();
  String addressdelivery = '';
  String nameAddressdelivery ='';
  late LocationData locationData;
  List<CartItem> cartItem = [];
  void _loadCartItems() async {
    List<CartItem> loadedItems = await CartStorage.getCartItems();
    setState(() {
      cartItem = loadedItems;
    });
    locationData = await localtionStorge.getSavedLocation();
    print('cartItem $cartItem');
  }
  late bool result;

  @override
  initState() {
    super.initState();
    localtionStorge = LocationStorage();
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
        throw Exception('Không thể di chuyển tới trang $url');
      }
    } catch (e) {
      print('Error launching URL: $e');
    }
  }

  // Hàm xóa item cart
  void _removeItemCart(CartItem item) {
    CartStorage.RemoveItemToCart(item);
  }


// Method to reload content
  void reloadContent() {
    _loadCartItems();
    setState(() {
      result = false; // Reset result to false if needed
    });
  }


  @override
  Widget build(BuildContext context) {
    //final arguments = ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
  //  final cartItem = arguments['data']; // Access 'your_data' here
    double totalAmount = cartItem.fold( 0, (total, item) => total + (item.price * item.qty));
    if (result) {
      reloadContent(); // Automatically call reloadContent when result is true
    }
    return Scaffold(
      appBar: AppBar(
        title: Text(
          'Chi tiết đơn hàng',
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
    SingleChildScrollView(
      physics: AlwaysScrollableScrollPhysics(),
      child:     Padding(
        padding: const EdgeInsets.all(10.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Column(
              mainAxisAlignment: MainAxisAlignment.start,
              crossAxisAlignment: CrossAxisAlignment.start,// Đảm bảo dòng text ở giữa
              children: [
                Row(
                  children: [
                    Icon(
                      Icons.error,
                      color: Colors.redAccent,
                      size: 18,// Màu sắc của biểu tượng "!"
                    ),
                    SizedBox(width: 5,),
                    Flexible(
                      child: Text(
                        'Kiểm tra lại thông tin và vị trí nhận hàng của bạn',
                        style: TextStyle(
                          fontSize: Dimensions.font13,
                          fontStyle: FontStyle.normal,
                        ),
                        overflow: TextOverflow.ellipsis,
                        maxLines: 2,
                      ),
                    ),
                  ],
                ),
                Padding(
                  padding: EdgeInsets.only(top: 5, bottom: 5),child:TextField(
                  controller: namecontroller,
                  decoration: InputDecoration(
                    labelText: 'Tên người nhận',
                    hintText: "Nhập tên người nhận hàng ...",
                    border: OutlineInputBorder(),
                  ),
                  maxLines: 1,
                  keyboardType: TextInputType.multiline,
                ),),
                Padding(
                  padding: EdgeInsets.only(top: 5, bottom: 5),child:TextField(
                  controller: phoneNumbercontroller,
                  decoration: InputDecoration(
                    labelText: 'Số điện thoại',
                    hintText: "Nhập số điện thoại nhận hàng ...",
                    border: OutlineInputBorder(),
                  ),
                  maxLines: 1,
                  keyboardType: TextInputType.multiline,
                ),),
                Text(
                  'Địa chỉ nhận hàng',
                  style: TextStyle(fontSize: Dimensions.font16,
                    fontWeight: FontWeight.bold,
                  ),
                  textAlign: TextAlign.start,
                ),
                Padding(
                    padding: EdgeInsets.only(top: 5, bottom: 5),
                    child: GestureDetector(
                      onTap: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(builder: (context) => LocationPage(link: "/order")), // truyền vào link cần trở lại khi đã chọn được vị trí
                        );
                      },
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Container(
                            height: Dimensions.height45,
                            width: Dimensions.screenWidth - 95,
                            child: BigText(text: locationData.address, size: Dimensions.font16,),
                          ),
                          Divider(),
                          Icon(
                            Icons.arrow_drop_down_rounded,
                            size: 40,
                          ),
                        ],
                      ),

                    )
                ),
              ],
            ),
            Text(
              'Sản phẩm trong đơn hàng',
              style: TextStyle(fontSize: Dimensions.font16,
                fontWeight: FontWeight.bold,
              ),
              textAlign: TextAlign.center,
            ),
            Container(
              height: cartItem.length.toDouble() * 130,
              child: ListView.builder(
                itemCount: cartItem.length,
                itemBuilder: (BuildContext context, int index) {
                  var item = cartItem[index];
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
                        title: Text(item.foodName),
                        subtitle: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: <Widget>[
                            SizedBox(height: 5),
                            Row(
                              children: [
                                BigText(text: "Số lượng: ${item.qty}", color: Colors.black, size: Dimensions.font13),
                                SizedBox(width: 20),
                                BigText(text: "Đơn giá: ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(item.price ?? 0)}", color: Colors.black, size: Dimensions.font13),
                              ],
                            ),
                            SizedBox(height: 5),
                            BigText(text: "Thành tiền: ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(item.price * item.qty ?? 0)}", color: AppColors.mainColor, size: Dimensions.font13),
                            SizedBox(height: 10),
                            Row(
                              children: [
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
                                        size: 18,
                                      ),
                                      SizedBox(width: 5),
                                      BigText(text: "Chỉnh sửa", color: Colors.blueAccent, size: Dimensions.font13),
                                    ],
                                  ),
                                ),
                                SizedBox(width: 20),
                                GestureDetector(
                                  onTap: () async {
                                    _removeItemCart(item);
                                    _loadCartItems();
                                  },
                                  child: Row(
                                    children: <Widget>[
                                      Icon(
                                        Icons.delete,
                                        color: Colors.redAccent,
                                        size: 18,
                                      ),
                                      SizedBox(width: 5),
                                      BigText(text: "Xóa", color: Colors.redAccent, size: Dimensions.font13),
                                    ],
                                  ),
                                ),
                              ],
                            ),
                          ],
                        ),
                      ),
                    );
                },
              ),
            ),
            Padding(padding: EdgeInsets.all(0),
              child: Column(
                children: [
                  Row(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: [
                      Text(
                        'Phí giao hàng: ' + NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(12000.0),
                        style: TextStyle(fontWeight: FontWeight.bold),
                      ),
                    ],
                  ),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: [
                      Text(
                        'Tổng tạm tính: ' + NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(totalAmount),
                        style: TextStyle(fontWeight: FontWeight.bold),
                      ),
                    ],
                  ),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: [
                      Text(
                        'Tổng cộng: ' + NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(12000 + totalAmount),
                        style: TextStyle(fontWeight: FontWeight.bold),
                      ),
                    ],
                  ),
                ],
              ),
            ),
            cartItem.length > 0 ?
            Center(
              child:
              ElevatedButton(
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
                ),
                onPressed: () async {
                  try {
                    orderRequest.completeName = "Đông Châu";
                    orderRequest.formatAddress = locationData.address;
                    orderRequest.nameAddress = locationData.name;
                    orderRequest.orderLine = cartItem;
                    orderRequest.customerId = _auth.currentUser!.uid;
                    orderRequest.intoMoney = totalAmount.toInt();
                    orderRequest.transportFee = 12000;
                    orderRequest.totalAmt = totalAmount.toInt() - 12000;
                    orderRequest.userId = 14;
                    orderRequest.latitude = locationData.latitude;
                    orderRequest.longitude = locationData.longitude;
                    orderRequest.recipientName = namecontroller.text;
                    orderRequest.recipientPhone = phoneNumbercontroller.text;
                    orderRequest.payment = null;
                    orderRequest.userId = 15;
                    orderRequest.payment ="PaymentOnDelivery";

                    final responseBody = await orderservice.postOrder(orderRequest);

                    // Xử lý kết quả trả về từ API
                    if (responseBody.success == true) {
                      if(responseBody.Message != ""){
                        _launchInWebView(Uri.parse(responseBody.Message?? ""));
                      }
                      else {
                        await CartStorage.ClearCart();
                        Fluttertoast.showToast(msg: "Đặt món ăn thành công",
                            toastLength: Toast.LENGTH_LONG,
                            gravity: ToastGravity.BOTTOM_LEFT,
                            backgroundColor: AppColors.toastSuccess,
                            textColor: Colors.black54,
                            timeInSecForIosWeb: 1,
                            fontSize: 10);
                        Navigator.of(context).popAndPushNamed("/");
                      }
                    } else{
                      if(responseBody.CustomData != null){
                        Fluttertoast.showToast(msg: responseBody.Message!,
                            toastLength: Toast.LENGTH_LONG,
                            gravity: ToastGravity.BOTTOM_LEFT,
                            backgroundColor: AppColors.toastSuccess,
                            textColor: Colors.black54,
                            timeInSecForIosWeb: 1,
                            fontSize: 10);
                      }
                    }
                  } catch (e) {
                    print(e);
                    // Xử lý lỗi kết nối hoặc lỗi khác
                    Fluttertoast.showToast(msg: "Đã xảy ra lỗi khi đặt đơn hàng",
                        toastLength: Toast.LENGTH_LONG,
                        gravity: ToastGravity.BOTTOM_LEFT,
                        backgroundColor: Colors.red[400],
                        textColor: Colors.black54,
                        timeInSecForIosWeb: 1,
                        fontSize: 10);
                  }
                },
                child: Text('Đặt đơn'),
              ),
            )
                : Center(
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
                ),
                onPressed: () {
                  Navigator.of(context).pop();
                },
                child: Text('Trang chủ'),
              ),
            )
          ],
        ),
      ),
    )
    );
  }


}
