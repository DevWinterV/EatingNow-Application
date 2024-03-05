import 'dart:async';

import 'package:fam/Widget/Big_text.dart';
import 'package:fam/data/Api/OrderService.dart';
import 'package:fam/models/LocationData.dart';
import 'package:fam/models/OrderRequest.dart';
import 'package:fam/models/paymentconfirm_request.dart';
import 'package:fam/signalR/signalR_client.dart';
import 'package:fam/storage/UserAccountstorage.dart';
import 'package:fam/storage/paymentmethod_storage.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_webview_plugin/flutter_webview_plugin.dart';
import 'package:intl/intl.dart';
import 'package:fluttertoast/fluttertoast.dart';
import '../../Widget/Small_text.dart';
import '../../data/Api/firebase_api.dart';
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
  late StreamController<String> streamPaymentmethod;
  late PaymentMethodStorage _paymentMethodStorage;
  late SignalRClient signalRClient;
  late int? userId;
  TextEditingController namecontroller = TextEditingController();
  TextEditingController phoneNumbercontroller = TextEditingController();
  late LocationStorage localtionStorge;
  FirebaseAuth _auth = FirebaseAuth.instance;
  final orderservice = OrderService();//lấy cửa hàng gần nhất

  OrderRequest orderRequest= OrderRequest();
  UserAccountStorage userAccountStorage = UserAccountStorage();
  String addressdelivery = '';
  String nameAddressdelivery ='';
  String paymentMethod = '';
  late LocationData locationData;
  List<CartItem> cartItem = [];


  void _loadPaymentMethod() async {
    final paymentStorage = await _paymentMethodStorage.getSavedPaymentMethod();
    if(paymentStorage == ''){
      orderRequest.payment = "PaymentOnDelivery";
    }else{
      orderRequest.payment = paymentStorage;
    }
    print('orderRequest.payment ${orderRequest.payment}');
    streamPaymentmethod.sink.add(paymentStorage);
  }
  void _loadUserData() async {
    final user = await userAccountStorage.getSavedUserAccount();
    namecontroller.text = user.name;
    if( user.phone != null){
      phoneNumbercontroller.text = user.phone;
    }else{
      phoneNumbercontroller.text = FirebaseAuth.instance.currentUser?.phoneNumber ?? "";
    }
  }
  void _loadCartItems() async {
    locationData = await localtionStorge.getSavedLocation();
    final arguments = ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
    userId = arguments['data'] as int; // Nhận dữ liệu UserId
    List<CartItem> loadedItems = await CartStorage.getCartItems();
    setState(() {
      cartItem = loadedItems.where((element) => element.userId == userId).toList();
    });
  }
  late bool result;

  @override
  initState() {
    super.initState();
    _paymentMethodStorage = PaymentMethodStorage();
    streamPaymentmethod = StreamController<String>();
    FirebaseApi().initNotifications();
    connectToSignalR();
    localtionStorge = LocationStorage();
    setState(() {
      result = false;
    });
    _loadCartItems();
    _loadUserData();
    _loadPaymentMethod();
  }
  void connectToSignalR() async {
    signalRClient = SignalRClient();
    await signalRClient.connectToServer();
    // await signalRClient.setCustomerId(FirebaseAuth.instance.currentUser?.uid ?? "");
  }

  void SendOrderNotificationToUser(String UserId) async {
    await signalRClient.SendOrderNotificationToUser(UserId);

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
  void dispose(){
    super.dispose();

  }

  @override
  Widget build(BuildContext context) {
    double totalAmount = cartItem.fold( 0, (total, item) => total + (item.price * item.qty));
    if (result) {
      reloadContent(); // Automatically call reloadContent when result is true
    }
    return Scaffold(
          appBar: AppBar(
            title: Column(
              children: [
                Text(
                  'Chi tiết đơn hàng',
                  overflow: TextOverflow.ellipsis,
                  maxLines: 1,
                  style: TextStyle(
                    fontSize: Dimensions.font20,
                  ),// Số dòng tối đa hiển thị (có thể điều chỉnh theo nhu cầu của bạn)
                ),
                SmallText(text: cartItem[0].storeName ?? "", size: Dimensions.font16, color: Colors.black)
              ],
            ),
            centerTitle: true, // Để căn giữa tiêu đề trên thanh AppBar
            // Các thuộc tính khác của AppBar
            backgroundColor: AppColors.mainColor, // Màu nền cho AppBar
          ),
          body: SingleChildScrollView(
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
                              'Kiểm tra lại thông tin và địa chỉ nhận hàng của bạn',
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
                        onChanged: (text) async {
                          // Cập nhật lại dữ liệu khi thay đổi
                          final user = UserAccount(userId: _auth.currentUser!.uid, name: namecontroller.text, phone: phoneNumbercontroller.text);
                          await userAccountStorage.saveUserAccount(user);
                        },
                        keyboardType: TextInputType.multiline,
                      ),),
                      // Widget TextField sẽ chỉ chấp nhận số
                      Padding(
                        padding: EdgeInsets.only(top: 5, bottom: 5),
                        child: TextField(
                          controller: phoneNumbercontroller,
                          decoration: InputDecoration(
                            labelText: 'Số điện thoại',
                            hintText: "Nhập số điện thoại nhận hàng ...",
                            border: OutlineInputBorder(),
                          ),
                          onChanged: (text) async {
                            // Cập nhật lại dữ liệu khi thay đổi
                            final user = UserAccount(userId: _auth.currentUser!.uid, name: namecontroller.text, phone: phoneNumbercontroller.text);
                            await userAccountStorage.saveUserAccount(user);
                          },
                          maxLines: 1,
                          keyboardType: TextInputType.phone, // Thay đổi keyboardType thành TextInputType.phone để hiển thị bàn phím số
                          inputFormatters: [FilteringTextInputFormatter.allow(RegExp(r'[0-9]'))], // Áp dụng định dạng để chỉ chấp nhận số
                        ),
                      ),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Text(
                            'Địa chỉ nhận hàng',
                            style: TextStyle(fontSize: Dimensions.font16,
                              fontWeight: FontWeight.bold,
                            ),
                            textAlign: TextAlign.start,
                          ),
                          GestureDetector(
                            onTap: () async {
                              final result = await Navigator.push(
                                context,
                                MaterialPageRoute(builder: (context) => LocationPage(link: "/order")), // truyền vào link cần trở lại khi đã chọn được vị trí
                              );
                              if(result == true){
                                _loadCartItems();
                              }
                            },
                            child: Text("Thay đổi vị trí", style: TextStyle(color: Colors.blue[800], fontSize: 12),),
                          )
                        ],
                      ),
                      Padding(
                          padding: EdgeInsets.only(top: 5, bottom: 5),
                          child: GestureDetector(
                            onTap: () {
                              // final result =  Navigator.push(
                              //   context,
                              //   MaterialPageRoute(builder: (context) => LocationPage(link: "/order")), // truyền vào link cần trở lại khi đã chọn được vị trí
                              // );
                              // print('result $result');
                              // if(result == true){
                              //   setState(() async {
                              //     locationData = await localtionStorge.getSavedLocation();
                              //   });
                              // }
                            },
                            child: Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: [
                                Container(
                                  height: Dimensions.height45,
                                  width: MediaQuery.of(context).size.width - 80,
                                  child: BigText(text: locationData.address, size: Dimensions.font16,),
                                ),
                                Divider(),
                              ],
                            ),

                          )
                      ),
                    ],
                  ),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Text(
                        'Danh sách sản phẩm chọn',
                        style: TextStyle(fontSize: Dimensions.font16,
                          fontWeight: FontWeight.bold,
                        ),
                        textAlign: TextAlign.center,
                      ),
                      Text(
                        '${cartItem.length.toString()} sản phẩm' ,
                        style: TextStyle(fontSize: Dimensions.font13,
                          fontWeight: FontWeight.normal,
                          color: Colors.blue[800],
                        ),
                        textAlign: TextAlign.center,
                      ),
                    ],
                  ),
                  // DANH SÁCH SẢN PHẨM
                  Container(
                      height: MediaQuery.of(context).size.height - 500,
                      // height: 300,
                      child:
                      cartItem.length > 0 ?
                      ListView.builder(
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
                                    SizedBox(height: 1),
                                    Row(
                                      children: [
                                        BigText(text: "Số lượng: ${item.qty}", color: Colors.black, size: Dimensions.font13),
                                        SizedBox(width: 20),
                                        BigText(text: "Đơn giá: ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(item.price ?? 0)}", color: Colors.black, size: Dimensions.font13),
                                      ],
                                    ),
                                    SizedBox(height: 1),
                                    BigText(text: "Thành tiền: ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(item.price * item.qty ?? 0)}", color: AppColors.mainColor, size: Dimensions.font13),
                                    SizedBox(height: 1),
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
                      )
                          :
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
                                "Chưa có sản phẩm trong đơn hàng",
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
                  // THÔNG TIN PHÍ SHIP - TỔNG TIỀN THANH TOÁN
                ],
              ),
            ),
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
            height: 150,
            width: MediaQuery.of(context).size.width,
            child:  Column(
              children: [
                Padding(padding: EdgeInsets.only(left: 12, right: 12, top: 2),
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: [
                      StreamBuilder<String>(
                          stream: streamPaymentmethod.stream,
                          builder: (builder, snapshot){
                            return GestureDetector(
                              onTap: () {
                                showModalBottomSheet(
                                  context: context,
                                  builder: (BuildContext context) {
                                    return Container(
                                      height: 130, // Điều chỉnh chiều cao tùy theo nhu cầu
                                      child: ListView(
                                        children: [
                                          ListTile(
                                            leading: Image.asset(
                                              "assets/image/cod.png",
                                              height: 30,
                                              width: 30,),
                                            title: Text('Thanh toán khi nhận hàng'),
                                            onTap: () async {
                                              await _paymentMethodStorage.savePaymentMethodStorage("PaymentOnDelivery");
                                              _loadPaymentMethod();
                                              Navigator.pop(context);
                                            },
                                          ),
                                          ListTile(
                                            leading: Image.asset(
                                              "assets/image/VNPay_logo.png",
                                              height: 30,
                                              width: 30,),
                                            title: Text('Cổng thanh toán VNPay'),
                                            onTap: () async{
                                              await _paymentMethodStorage.savePaymentMethodStorage("VNPay");
                                              _loadPaymentMethod();
                                              Navigator.pop(context);
                                            },
                                          ),
                                          // Thêm các phần tử danh sách khác tại đây
                                        ],
                                      ),
                                    );
                                  },
                                );
                              },
                              child: Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: [
                                  Text(
                                    'Phương thức thanh toán:',
                                    style: TextStyle(fontWeight: FontWeight.bold),
                                  ),
                                  Text(
                                    snapshot.data == "PaymentOnDelivery" || snapshot.data == "" ?
                                    "Tiền mặt" : snapshot.data == "VNPay" ? "Cổng VNPay" : "Vui lòng chọn",
                                    style: TextStyle(color: Colors.blue[800],),
                                  ),
                                ],
                              ),
                            );
                          }),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Text(
                            'Phí giao hàng: ',
                            style: TextStyle(fontWeight: FontWeight.bold),
                          ),
                          Row(
                            children: [
                              GestureDetector(
                                child: Icon(Icons.info_outline, size: 15),
                                onTap: () {
                                  // Xử lý khi người dùng chạm vào biểu tượng
                                  showDialog(
                                    context: context,
                                    builder: (BuildContext context) {
                                      return AlertDialog(
                                        title: Text("Thông tin phí giao hàng", style: TextStyle(fontSize: Dimensions.font20),),
                                        content:
                                        Container(
                                            height: 100,
                                            child:   Column(
                                              children: [
                                                Text("- Khoảng cách từ cửa hàng đến vị trí nhận hàng của bạn: 2.88 Km."),
                                                Text("- Phí giao hàng cơ bản là (Km * 10.000đ) + 2.000 nếu > 2 Km."),
                                              ],
                                            )
                                        ),
                                        actions: <Widget>[
                                          TextButton(
                                            onPressed: () {
                                              Navigator.of(context).pop(); // Đóng dialog
                                            },
                                            child: Text("Đóng"),
                                          ),
                                        ],
                                      );
                                    },
                                  );
                                },
                              ),
                              SizedBox(width: 2,),
                              Text(
                                NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(12000.0),
                                style: TextStyle(fontWeight: FontWeight.bold),
                              ),
                            ],
                          ),

                        ],
                      ),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Text(
                            'Tổng tạm tính: ',
                            style: TextStyle(fontWeight: FontWeight.bold),
                          ),
                          Text(
                            NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(totalAmount),
                            style: TextStyle(fontWeight: FontWeight.bold),
                          ),
                        ],
                      ),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Text(
                            'Tổng cộng: ' ,
                            style: TextStyle(fontWeight: FontWeight.bold),
                          ),
                          Text(
                            NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(12000 + totalAmount),
                            style: TextStyle(fontWeight: FontWeight.bold),
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
                Padding(
                  padding: EdgeInsets.only(left: 10, right: 10),
                  child:   cartItem.length > 0 ?
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
                        minimumSize: Size(double.infinity, 50), // Đặt kích thước tối thiểu cho nút
                      ),
                      onPressed: () async {
                        if(namecontroller.text == "" || phoneNumbercontroller.text == "" )
                        {
                          Fluttertoast.showToast(msg: "Vui lòng điền đầy đủ thông tin !",
                              toastLength: Toast.LENGTH_LONG,
                              gravity: ToastGravity.BOTTOM_LEFT,
                              backgroundColor: Colors.red[400],
                              textColor: Colors.black54,
                              timeInSecForIosWeb: 2,
                              fontSize: Dimensions.font13);
                          return;
                        }
                        try {
                          orderRequest.TokenApp = await FirebaseApi().getFCMToken();
                          orderRequest.completeName = namecontroller.text ;
                          orderRequest.formatAddress = locationData.address;
                          orderRequest.nameAddress = locationData.name;
                          orderRequest.orderLine = cartItem;
                          orderRequest.customerId = _auth.currentUser!.uid;
                          orderRequest.intoMoney = totalAmount.toInt() + 12000;
                          orderRequest.transportFee = 12000;
                          orderRequest.totalAmt = totalAmount.toInt();
                          orderRequest.latitude = locationData.latitude;
                          orderRequest.longitude = locationData.longitude;
                          orderRequest.recipientName = namecontroller.text;
                          orderRequest.recipientPhone = phoneNumbercontroller.text;
                          orderRequest.userId = userId ?? 0;
                          final responseBody = await orderservice.postOrder(orderRequest);
                          // Xử lý kết quả trả về từ API
                          if (responseBody.success == true) {
                            if(responseBody.Message != ""){
                              final FlutterWebviewPlugin flutterWebviewPlugin = FlutterWebviewPlugin();
                              flutterWebviewPlugin.launch(responseBody.Message ?? "");
                              flutterWebviewPlugin.onUrlChanged.listen((url) async {
                                if (url.contains('vnp_ResponseCode')) {
                                  final params = Uri.parse(url).queryParameters;
                                  if (params['vnp_ResponseCode'] == '00')
                                  {
                                    flutterWebviewPlugin.close();
                                    final payment_request = PaymentTransaction(Vnp_Amount: params['vnp_Amount'] ?? '', Vnp_BankCode: params['vnp_BankCode'] ?? '', Vnp_BankTranNo: params['vnp_BankTranNo'] ?? '', Vnp_CardType:  params['vnp_CardType'] ?? '', Vnp_OrderInfo: params['vnp_OrderInfo'] ?? '', Vnp_PayDate: params['vnp_PayDate'] ?? '', Vnp_ResponseCode:  params['vnp_ResponseCode'] ?? '', Vnp_SecureHash: params['vnp_SecureHash'] ?? '', Vnp_TmnCode: params['vnp_TmnCode'] ?? '', Vnp_TransactionNo: params['vnp_TransactionNo'] ?? '', Vnp_TransactionStatus: params['vnp_TransactionStatus'] ?? '', Vnp_TxnRef: params['vnp_TxnRef'] ?? '', requestOrder: orderRequest);
                                    final responsePayment = await orderservice.PaymentConfirm(payment_request);
                                    if (responsePayment.success) {
                                      await CartStorage.ClearCartByUserId(userId ?? 0);
                                      final user = UserAccount(userId: _auth.currentUser!.uid, name: namecontroller.text, phone: phoneNumbercontroller.text);
                                      await userAccountStorage.saveUserAccount(user);
                                      SendOrderNotificationToUser(userId.toString());
                                      Fluttertoast.showToast(msg: responsePayment.Message ?? "",
                                          toastLength: Toast.LENGTH_LONG,
                                          gravity: ToastGravity.BOTTOM_LEFT,
                                          backgroundColor: AppColors.toastSuccess,
                                          textColor: Colors.black54,
                                          timeInSecForIosWeb: 1,
                                          fontSize: Dimensions.font13);
                                      Navigator.of(context).pop();
                                    }
                                    else{
                                      flutterWebviewPlugin.close();
                                      Fluttertoast.showToast(msg: responsePayment.Message ?? "",
                                          toastLength: Toast.LENGTH_LONG,
                                          gravity: ToastGravity.BOTTOM_LEFT,
                                          backgroundColor: AppColors.toastSuccess,
                                          textColor: Colors.black54,
                                          timeInSecForIosWeb: 1,
                                          fontSize: Dimensions.font13);
                                    }
                                  }
                                  else {
                                    flutterWebviewPlugin.close();
                                    Fluttertoast.showToast(msg: "Đã xảy ra lỗi trong quá trình thanh toán! Vui lòng thử lại.",
                                        toastLength: Toast.LENGTH_LONG,
                                        gravity: ToastGravity.BOTTOM_LEFT,
                                        backgroundColor: AppColors.toastSuccess,
                                        textColor: Colors.black54,
                                        timeInSecForIosWeb: 1,
                                        fontSize: Dimensions.font13);
                                  }
                                }
                              });
                            }
                            else {
                              await CartStorage.ClearCartByUserId(userId ?? 0);
                              final user = UserAccount(userId: _auth.currentUser!.uid, name: namecontroller.text, phone: phoneNumbercontroller.text);
                              await userAccountStorage.saveUserAccount(user);
                              SendOrderNotificationToUser(userId.toString());
                              Fluttertoast.showToast(msg: "Đặt đơn hàng thành công",
                                  toastLength: Toast.LENGTH_LONG,
                                  gravity: ToastGravity.BOTTOM_LEFT,
                                  backgroundColor: AppColors.toastSuccess,
                                  textColor: Colors.black54,
                                  timeInSecForIosWeb: 1,
                                  fontSize: Dimensions.font13);
                              Navigator.of(context).pop();
                            }
                          } else{
                            if(responseBody.CustomData != null){
                              Fluttertoast.showToast(msg: responseBody.Message!,
                                  toastLength: Toast.LENGTH_LONG,
                                  gravity: ToastGravity.BOTTOM_LEFT,
                                  backgroundColor: AppColors.toastSuccess,
                                  textColor: Colors.black54,
                                  timeInSecForIosWeb: 1,
                                  fontSize: Dimensions.font13);
                            }
                          }
                        } catch (e) {
                          // Xử lý lỗi kết nối hoặc lỗi khác
                          Fluttertoast.showToast(msg: "Đã xảy ra lỗi khi đặt đơn hàng",
                              toastLength: Toast.LENGTH_LONG,
                              gravity: ToastGravity.BOTTOM_LEFT,
                              backgroundColor: Colors.red[400],
                              textColor: Colors.black54,
                              timeInSecForIosWeb: 1,
                              fontSize: Dimensions.font13);
                        }
                      },
                      child: Text('Đặt đơn', style: TextStyle(fontSize: Dimensions.font16, fontWeight: FontWeight.normal)),
                    ),
                  )
                      :
                  Center(
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
                      child: Text('Quay về trang chính', style: TextStyle(fontSize: Dimensions.font16),),
                    ), ) ,
                )
              ],
            )// ĐẶT ĐƠN,
        )
      );

  }
}
