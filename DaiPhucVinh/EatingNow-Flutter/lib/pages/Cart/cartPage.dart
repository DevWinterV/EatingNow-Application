import 'package:fam/Widget/Small_text.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/app_constants.dart';
import 'package:fam/util/dimensions.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import '../../Widget/Big_text.dart';
import '../../storage/cartstorage.dart';


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

  // Hàm xóa item cart
  void _removeItemCart(CartItem item) {
    CartStorage.RemoveItemToCart(item);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          'Giỏ hàng',
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
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
                  Image.asset(
                      "assets/image/emptycart.png",
                        height: 100,
                        width: 100,),
                  Text(
                    "Chưa có sản phẩm trong giỏ hàng",
                    style: TextStyle(
                        fontSize: Dimensions.font16,
                        fontWeight: FontWeight.bold,
                        color: Colors.grey
                    ),
            )
          ],
          )
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
                    padding: EdgeInsets.only(right: 10),
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
                      Navigator.popAndPushNamed(context, "/order", arguments: {'data': cartItems }) as Route<Object?>
                  );
                },
                child:
                Card(
                  color: Colors.white,
                  margin: EdgeInsets.only(bottom: 10),
                  child: ListTile(
                    leading: Image.network(
                      cartItems[index].uploadImage ?? "",
                      width: 80,
                      height: 80,
                      fit: BoxFit.cover,
                    ),
                    title: Text( cartItems[index].foodName + ' x ${cartItems[index].qty}'),
                    subtitle: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: <Widget>[

                        BigText(text: NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format( cartItems[index].price *  cartItems[index].qty ?? 0), color: AppColors.mainColor,size: Dimensions.font13,),
                        GestureDetector(
                          onTap: () async {
                            final kq = await Navigator.pushNamed(
                              context,
                              "/productdetail",
                              arguments: {'data':  cartItems[index]},
                            );
                            if(kq == true){
                              _loadCartItems();
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
//                 Card(
//                   color: Colors.white,
//                   elevation: 2, // Độ nâng của thẻ
//                   margin: EdgeInsets.symmetric(horizontal: 16, vertical: 8), // Khoảng cách giữa các thẻ
//                   child: Padding(
//                     padding: EdgeInsets.all(8),
//                     child: ListTile(
//                       leading: Image.network(
//                         cartItems[index].uploadImage ?? " ",
//                         fit: BoxFit.fill,
//                         width: 40,
//                       ),
//                       title: Text(cartItems[index].foodName),
//                       subtitle: Row(
//                         children: [
//                           SmallText(text: 'Số lượng: '+cartItems[index].qty.toString(),color: Colors.black54,),
//                           InkWell(
//                             onTap: () {
//                               _decreaseQuantity(cartItems[index]);
//                             },
//                             child: Padding(
//                               padding: EdgeInsets.all(6), // Điều chỉnh giá trị này để thay đổi kích thước của nút
//                               child: Icon(
//                                 Icons.edit,
//                                 size: 20,
//                                 color: AppColors.mainColor,
//                               ),
//                             ),
//                           ),
//                         ],
//                       ),
//
// /*
//                     subtitle: Row(
//                       children: [
//                         Card(
//                           child: InkWell(
//                             onTap: () {
//                               _decreaseQuantity(cartItems[index]);
//                             },
//                             child: Padding(
//
//                               padding: EdgeInsets.all(6), // Điều chỉnh giá trị này để thay đổi kích thước của nút
//                               child: Icon(
//                                 Icons.remove,
//                                 size: 20,
//
//                               ),
//                             ),
//                           ),
//                         ),
//                         Padding(
//                           padding: EdgeInsets.all(4),
//                           // Điều chỉnh giá trị này để thay đổi kích thước của nút
//                         ),
//                         Card(
//                           child: InkWell(
//                             onTap: () {
//                               _increaseQuantity(cartItems[index]);
//                             },
//                             child: Padding(
//
//                               padding: EdgeInsets.all(6), // Điều chỉnh giá trị này để thay đổi kích thước của nút
//                               child: Icon(
//                                 Icons.add,
//                                 size: 20,
//
//                               ),
//                             ),
//                           ),
//                         ),
//                       ],
//                     ),*/
//                       trailing: Text(
//                         NumberFormat.currency(locale: 'vi_VN', symbol: '₫')
//                             .format(cartItems[index].price * cartItems[index].qty  ?? 0),
//                         style: TextStyle(
//                           color: Colors.redAccent,
//                           fontSize: Dimensions.font13,
//                         ),
//                       ),
//                     ),
//                   ),
//                 ),
              ),

            );
          },
        ),
      ),
    );
  }
}


