import 'dart:async';

import 'package:fam/Widget/Small_text.dart';
import 'package:flutter/material.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/dimensions.dart';
import 'package:intl/intl.dart';
import '../../Widget/Big_text.dart';
import '../../storage/cartstorage.dart';


class CartPage extends StatefulWidget {
  @override
  _CartPageState createState() => _CartPageState();
}

class _CartPageState extends State<CartPage> {
  StreamController<bool?> _checkSelectEditController = StreamController<bool?>.broadcast();
  Stream<bool?> get checkSelectEditStream => _checkSelectEditController.stream;
  StreamController<List<StoreChecked>?> _checkSelectedListStoreIdController = StreamController<List<StoreChecked>?>.broadcast();
  Stream<List<StoreChecked>?> get checkSelectedListStoreIdStream => _checkSelectedListStoreIdController.stream;

  StreamController<List<StoreChecked>?> _checkSelectedListStoreIdRemoveController = StreamController<List<StoreChecked>?>.broadcast();
  Stream<List<StoreChecked>?> get checkSelectedListStoreIdRemoveStream => _checkSelectedListStoreIdRemoveController.stream;

  List<StoreUserCart> groupedCart = [];
  List<CartItem> cartItems = [];

  @override
  void dispose() {
    _checkSelectEditController.close();
    _checkSelectedListStoreIdController.close();
    super.dispose();
  }

  @override
  void initState() {
    super.initState();
    // Khởi tạo giỏ hàng từ SharedPreferences khi màn hình được tạo
    _loadCartItems();
  }

  void updateCheckSelectEditController(bool? newData) {
    _checkSelectEditController.add(newData);
  }

  void updatecheckSelectedListStoreIdControlle(List<StoreChecked>? newList) {
    _checkSelectedListStoreIdController.add(newList);
  }

  void updatecheckSelectedListStoreIdRemoveControlle(List<StoreChecked>? newList) {
    _checkSelectedListStoreIdRemoveController.add(newList);
  }
  // Phương thức để load danh sách món ăn từ SharedPreferences
  void _loadCartItems() async {
    List<StoreUserCart> groupedCartfunct = [];
    List<CartItem> loadedItems = await CartStorage.getCartItems();
    setState(() {
      cartItems = loadedItems;
    });

    cartItems.forEach((item) {
      // Kiểm tra xem cặp cửa hàng và người dùng đã tồn tại trong danh sách chưa
      var existingPair = groupedCartfunct.firstWhere((pair) =>
      pair.userChecked.nameStore == item.storeName && pair.userChecked.userId == item.userId,
          orElse: () => StoreUserCart(
            userChecked:
            StoreChecked(
                userId: 0,
                nameStore: 'notfound'
            ),
            items: [],
          ));

      // Nếu không tìm thấy, thêm một cặp mới
      if (existingPair.userChecked.userId == 0) {
        groupedCartfunct.add(StoreUserCart(
          items: [item],
          userChecked:   StoreChecked(
            nameStore: item.storeName?? "",
            userId: item.userId,
          ),
        ));
      } else {
        // Nếu đã tồn tại, thêm mục vào danh sách mục của cặp đó
        existingPair.items.add(item);
      }
    });
    setState(() {
      groupedCart = groupedCartfunct;
    });
    print(groupedCart);
    _checkSelectedListStoreIdRemoveController.add([]);
    _checkSelectedListStoreIdController.sink.add(groupedCart.map((e) => e.userChecked).toList());
  }

  // Hàm xóa item cart
  void _removeItemCart(CartItem item) {
    CartStorage.RemoveItemToCart(item);
    _loadCartItems();
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
        actions: [
          StreamBuilder<List<StoreChecked>?>(
              stream: _checkSelectedListStoreIdRemoveController.stream,
              builder: (builder, snapshot){
                if(snapshot.data != null && snapshot.data!.length > 0 ){
                  //final newList = snapshot.data!.map((e) => e.ischecked == true).toList();
                  final listStoreChecked = snapshot.data!.where((e) => e.ischecked == true).toList();
                  // Kiểm tra xem danh sách newList có ít nhất một phần tử là true hay không
                  if(listStoreChecked.length > 0) {
                    return GestureDetector(
                      onTap: (){
                        //Xóa các đơn đã chọn
                        showDialog(
                          context: context,
                          builder: (builder) {
                            return AlertDialog(
                              backgroundColor: Colors.white,
                              title: BigText(text: "Bạn có muốn các đơn hàng đã chọn không ?"),
                              actions: [
                                ElevatedButton(
                                  onPressed: () async {
                                    listStoreChecked.forEach((element) async {
                                      await CartStorage.ClearCartByUserId(element.userId);
                                    });
                                    _loadCartItems();
                                    // Gọi hàm xóa các đơn hàng đã chọn
                                    Navigator.of(context).pop(); // Đóng hộp thoại sau khi thực hiện xong
                                  },
                                  child: Text("Đồng ý",style: TextStyle(color: Colors.white),),
                                  style: ElevatedButton.styleFrom(
                                      padding: EdgeInsets.only(right: 20.0, left: 20.0),
                                      shape: RoundedRectangleBorder(
                                        borderRadius: BorderRadius.circular(10.0),
                                      ),
                                    backgroundColor: Colors.green
                                  ),
                                ),
                                ElevatedButton(
                                  onPressed: () {
                                    Navigator.of(context).pop(); // Đóng hộp thoại khi nhấn nút "Đóng"
                                  },
                                  child: Text("Đóng", style: TextStyle(color: Colors.black),),
                                  style: ElevatedButton.styleFrom(
                                      padding: EdgeInsets.only(right: 20.0, left: 20.0),
                                      shape: RoundedRectangleBorder(
                                        borderRadius: BorderRadius.circular(10.0),
                                      ),
                                      backgroundColor: Colors.white
                                  ),
                                ),
                              ],
                            );
                          },
                        );
                      },
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: [
                          BigText(text: "Xóa", color: Colors.red,),
                          SizedBox(width: 20,),
                        ],
                      ),
                    );
                  }
                }
                return SizedBox();
              }
          )
        ],

        centerTitle: true, // Để căn giữa tiêu đề trên thanh AppBar
        // Các thuộc tính khác của AppBar
        // Các thuộc tính khác của AppBar
        backgroundColor: AppColors.mainColor, // Màu nền cho AppBar
      ),
      body:
      // Chưa có sản phẩm trong giỏ hàng
      groupedCart.length == 0 ?
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
      )
          :
      // đã có sản phẩm trong giỏ hàng
      Container(
        child:
        StreamBuilder<List<StoreChecked>?>(
          initialData: groupedCart.map((e) => e.userChecked).toList(),
          stream: checkSelectedListStoreIdStream,
          builder: (context, snapshot) {
            return ListView.builder(
              itemCount: groupedCart.length,
              itemBuilder: (context, index) {
                final storeUserCart = groupedCart[index];
                final storeChecked = snapshot.data?[index];
                double totalAmount = groupedCart[index].items.fold( 0, (total, item) => total + (item.price * item.qty));
                return Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Padding(
                        padding: const EdgeInsets.all(8.0),
                        child:
                        Column(
                          children: [
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: [
                                Checkbox(
                                  value:  storeChecked?.ischecked,
                                  onChanged: (value) {
                                    snapshot.data?[index].ischecked = !storeChecked!.ischecked;
                                    updatecheckSelectedListStoreIdControlle(snapshot.data);
                                    updatecheckSelectedListStoreIdRemoveControlle(snapshot.data);
                                  },
                                ),
                                Text(
                                  storeUserCart.userChecked.nameStore ?? "",
                                  style: TextStyle(fontSize: 17, fontWeight: FontWeight.bold),
                                ),
                                GestureDetector(
                                  onTap: (){
                                    Navigator.pushReplacement(
                                        context,
                                        Navigator.popAndPushNamed(context, "/order", arguments: {'data': storeUserCart.items[0].userId }) as Route<Object?>
                                    );
                                  },
                                  child:  Text(
                                    'Đặt đơn',
                                    style: TextStyle(fontSize: 15, fontWeight: FontWeight.normal, color: Colors.blue[700]),
                                  ),
                                )
                              ],
                            ),
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: [
                                Text(
                                  '${groupedCart[index].items.length.toString()} sản phẩm',
                                  style: TextStyle(fontSize: 14, fontWeight: FontWeight.normal),
                                ),
                                Text(
                                  'Tổng tiền: ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(totalAmount ?? 0)}',
                                  style: TextStyle(fontSize: 14, fontWeight: FontWeight.normal),
                                ),
                              ],
                            )
                          ],
                        )
                    ),
                    Padding(
                      padding: const EdgeInsets.only(left: 8, right: 8),
                      child: Container(
                        child: ListView.builder(
                          shrinkWrap: true,
                          physics: NeverScrollableScrollPhysics(),
                          itemCount: storeUserCart.items.length,
                          itemBuilder: (context, itemIndex) {
                            final item = storeUserCart.items[itemIndex];
                            return Dismissible(
                              key: Key(item.foodListId.toString()), // Key là một giá trị duy nhất cho mỗi món ăn
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
                                _removeItemCart(item);
                              },
                              child: GestureDetector(
                                  onTap: () {
                                    Navigator.pushReplacement(
                                        context,
                                        Navigator.popAndPushNamed(context, "/order", arguments: {'data': storeUserCart.items[0].userId }) as Route<Object?>
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
                    ),
                    Divider(
                      thickness: 5, // Adjust the thickness of the divider as needed
                      color: Colors.white,
                    ),
                  ],
                );
              },
            );
          },
        )

      ),
    );
  }
}


