import 'package:fam/Widget/Small_text.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/dimensions.dart';
import 'package:flutter/material.dart';
import 'package:geolocator/geolocator.dart';
import 'package:flutter_native_splash/flutter_native_splash.dart';
import 'package:intl/intl.dart';
import 'package:permission_handler/permission_handler.dart';
import '../../data/Api/GoogleAPIService.dart';
import '../../models/LocationData.dart';
import '../../storage/cartstorage.dart';
import '../../storage/locationstorage.dart';
import 'package:fluttertoast/fluttertoast.dart';
import '../OderFood/orderfood.dart';
import '../circularprogress/DottedCircularProgressIndicator.dart';


class CartPage extends StatefulWidget {
  @override
  _CartPageState createState() => _CartPageState();
}

class _CartPageState extends State<CartPage> {
  List<CartItem> cartItems = [];

  @override
  void initState() {
    super.initState();
    // Khởi tạo giỏ hàng từ SharedPreferences khi màn hình được tạo
    _loadCartItems();
  }
  // Phương thức để load danh sách món ăn từ SharedPreferences
  void _loadCartItems() async {
    List<CartItem> loadedItems = await CartStorage.getCartItems();
    setState(() {
      cartItems = loadedItems;
    });
  }
  // Hàm tăng số lượng sản phẩm
  void _increaseQuantity(CartItem item) {
    setState(() {
      item.qty += 1 ;
    });
    CartStorage.UpdateToCart(item);
    _loadCartItems();
  }

// Hàm giảm số lượng sản phẩm
  void _decreaseQuantity(CartItem item) {
    CartStorage.RemoveToCart(item);
  }
  // Hàm xóa item cart
  void _removeItemCart(CartItem item) {
    CartStorage.RemoveItemToCart(item);
  }



  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          'Giỏ hàng của tôi',
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
      cartItems.length ==0 ?
      Container(
        child: Center(
          child: Text(
            "Chưa có sản phẩm nào trong giỏ hàng !",
            style: TextStyle(
              fontSize: Dimensions.font20,
              fontWeight: FontWeight.bold,
              color: Colors.grey
            ),
          ),

        ),
      ):
      Container(
        child: ListView.builder(
          itemCount: cartItems.length,
          itemBuilder: (context, index) {
            return Dismissible(
              key: Key(cartItems[index].foodListId.toString()), // Key là một giá trị duy nhất cho mỗi món ăn
              background: Container(
                color: Colors.red, // Màu nền khi vuốt để xóa
                child: Align(
                  alignment: Alignment.centerRight,
                  child: Padding(
                    padding: EdgeInsets.only(right: 16),
                    child: Icon(
                      Icons.delete,
                      color: Colors.white,
                    ),
                  ),
                ),
              ),
              onDismissed: (direction) {
                // Xử lý khi món ăn bị xóa
                _removeItemCart(cartItems[index]);
                setState(() {
                  cartItems.removeAt(index);
                });
              },
              child: GestureDetector(
                onTap: () {
                  Navigator.pushReplacement(
                      context,
                      Navigator.pushNamed(context, "/order", arguments: {'data': cartItems }) as Route<Object?>
                  );
                },
                child:Card(
                  elevation: 2, // Độ nâng của thẻ
                  margin: EdgeInsets.symmetric(horizontal: 16, vertical: 8), // Khoảng cách giữa các thẻ
                  child: Padding(
                    padding: EdgeInsets.all(8),
                    child: ListTile(
                      leading: Image.network(
                        cartItems[index].uploadImage ?? " ",
                        fit: BoxFit.fill,
                        width: 40,
                      ),
                      title: Text(cartItems[index].foodName),
                      subtitle: Row(
                        children: [
                          SmallText(text: 'Số lượng: '+cartItems[index].qty.toString(),color: Colors.black54,),
                          InkWell(
                            onTap: () {
                              _decreaseQuantity(cartItems[index]);
                            },
                            child: Padding(
                              padding: EdgeInsets.all(6), // Điều chỉnh giá trị này để thay đổi kích thước của nút
                              child: Icon(
                                Icons.edit,
                                size: 20,
                                color: AppColors.mainColor,
                              ),
                            ),
                          ),
                        ],
                      ),

/*
                    subtitle: Row(
                      children: [
                        Card(
                          child: InkWell(
                            onTap: () {
                              _decreaseQuantity(cartItems[index]);
                            },
                            child: Padding(

                              padding: EdgeInsets.all(6), // Điều chỉnh giá trị này để thay đổi kích thước của nút
                              child: Icon(
                                Icons.remove,
                                size: 20,

                              ),
                            ),
                          ),
                        ),
                        Padding(
                          padding: EdgeInsets.all(4),
                          // Điều chỉnh giá trị này để thay đổi kích thước của nút
                        ),
                        Card(
                          child: InkWell(
                            onTap: () {
                              _increaseQuantity(cartItems[index]);
                            },
                            child: Padding(

                              padding: EdgeInsets.all(6), // Điều chỉnh giá trị này để thay đổi kích thước của nút
                              child: Icon(
                                Icons.add,
                                size: 20,

                              ),
                            ),
                          ),
                        ),
                      ],
                    ),*/
                      trailing: Text(
                        NumberFormat.currency(locale: 'vi_VN', symbol: '₫')
                            .format(cartItems[index].price * cartItems[index].qty  ?? 0),
                        style: TextStyle(
                          color: Colors.redAccent,
                          fontSize: Dimensions.font13,
                        ),
                      ),
                    ),
                  ),
                ),
              ),

            );
          },
        ),
      ),
    );
  }
}


