import 'package:fam/util/Colors.dart';
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
            final item = cartItems[index];
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
                                    _loadCartItems();
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
                )
              ),

            );
          },
        ),
      ),
    );
  }
}


