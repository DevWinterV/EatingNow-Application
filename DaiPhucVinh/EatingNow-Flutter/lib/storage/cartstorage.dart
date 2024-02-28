import 'dart:convert';
import 'package:shared_preferences/shared_preferences.dart';

import '../models/product_recommended_model.dart';

class CartStorage {
  static const String _key = 'cart_items';
  // Lấy danh sách các món ăn trong giỏ hàng từ SharedPreferences
  static Future<List<CartItem>> getCartItems() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    final cartData = prefs.getString(_key);
    print('cartData $cartData');
    if (cartData != null) {
      final List<dynamic> decodedData = json.decode(cartData);
      final List<CartItem> cartItems = decodedData.map((data) => CartItem.fromJson(data)).toList();
      return cartItems;
    }
    return [];
  }
  // Thêm món ăn vào giỏ hàng
  static Future<bool> addToCart(CartItem newItem) async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    final cartData = prefs.getString(_key);
    List<CartItem> cartItems = [];
    if (cartData != null) {
      final List<dynamic> decodedData = json.decode(cartData);
      cartItems = decodedData.map((data) => CartItem.fromJson(data)).toList();
    }
    // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng hay chưa
    bool found = false;
    for (int i = 0; i < cartItems.length; i++) {
      if (cartItems[i].foodListId == newItem.foodListId) {
        //set lại sô lượng là 0
        cartItems[i].qty = 0;
        // Món ăn đã tồn tại, tăng số lượng
        cartItems[i].qty += newItem.qty;
        cartItems[i].descriptionBuy = newItem.descriptionBuy;
        found = true;
        break;
      }
    }
    print(cartItems);
    // Nếu món ăn chưa tồn tại, thêm nó vào giỏ hàng
    if (!found) {
      cartItems.add(newItem);
    }
    // Lưu lại danh sách giỏ hàng cập nhật
    final updatedCartData = json.encode(cartItems);
    await prefs.setString(_key, updatedCartData);
    return true;
  }
  static Future<bool> UpdateToCart(CartItem newItem) async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    final cartData = prefs.getString(_key);
    List<CartItem> cartItems = [];
    if (cartData != null) {
      final List<dynamic> decodedData = json.decode(cartData);
      cartItems = decodedData.map((data) => CartItem.fromJson(data)).toList();
    }
    bool found = false;
    for (int i = 0; i < cartItems.length; i++) {
      if (cartItems[i].foodListId == newItem.foodListId) {
        // Món ăn đã tồn tại, tăng số lượng
        cartItems[i].qty +=1;
        found = true;
        break;
      }
    }
    // Lưu lại danh sách giỏ hàng cập nhật
    final updatedCartData = json.encode(cartItems);
    await prefs.setString(_key, updatedCartData);
    return true;
  }
  static Future<bool> RemoveToCart(CartItem newItem) async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    final cartData = prefs.getString(_key);
    List<CartItem> cartItems = [];
    if (cartData != null) {
      final List<dynamic> decodedData = json.decode(cartData);
      cartItems = decodedData.map((data) => CartItem.fromJson(data)).toList();
    }
    if(newItem.qty == 1){
      cartItems.removeWhere((item) => item.foodListId == newItem.foodListId); // Xóa món ăn dựa trên ID
    }
    else{
      // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng hay chưa
      for (int i = 0; i < cartItems.length; i++) {
        if (cartItems[i].foodListId == newItem.foodListId) {
          // Món ăn đã tồn tại, tăng số lượng
          cartItems[i].qty-=1;
          break;
        }
      }
    }
    // Lưu lại danh sách giỏ hàng cập nhật
    final updatedCartData = json.encode(cartItems);
    await prefs.setString(_key, updatedCartData);
    return false;
  }
  static Future<bool> RemoveItemToCart(CartItem newItem) async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    final cartData = prefs.getString(_key);
    List<CartItem> cartItems = [];

    if (cartData != null) {
      final List<dynamic> decodedData = json.decode(cartData);
      cartItems = decodedData.map((data) => CartItem.fromJson(data)).toList();
    }
    cartItems.removeWhere((item) => item.foodListId == newItem.foodListId); // Xóa món ăn dựa trên ID
    // Lưu lại danh sách giỏ hàng cập nhật
    final updatedCartData = json.encode(cartItems);
    await prefs.setString(_key, updatedCartData);

    return false;
  }

  static Future<bool> ClearCartByUserId(int UserId) async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    final cartData = prefs.getString(_key);
    List<CartItem> cartItems = [];
    if (cartData != null) {
      final List<dynamic> decodedData = json.decode(cartData);
      cartItems = decodedData.map((data) => CartItem.fromJson(data)).toList();
    }
    cartItems = cartItems.where((element) => element.userId != UserId ).toList();
    // Lưu lại danh sách giỏ hàng cập nhật
    final updatedCartData = json.encode(cartItems);
    await prefs.setString(_key, updatedCartData);
    return true;
  }

  static Future<bool> ClearCart() async {
      SharedPreferences prefs = await SharedPreferences.getInstance();
      List<CartItem> cartItems = [];
      final datajson = json.encode(cartItems);
      await prefs.setString(_key, datajson);
      return true;
  }
}
class CartItem {
  final int foodListId;
  final int categoryId;
  final String foodName;
  final double price;
  int qty;
  final String uploadImage;
  String description;
  String descriptionBuy;
  String storeName;
  int userId;

  CartItem({
    required this.foodListId,
    required this.categoryId,
    required this.foodName,
    required this.price,
    required this.qty,
    required this.uploadImage,
    required this.description,
    required this.descriptionBuy,
    required this.storeName,
    required this.userId
  });

  // Hàm để ánh xạ dữ liệu từ CartItem sang DataProduct
  DataProduct toDataProduct() {
    return DataProduct(
      foodListId: foodListId,
      foodName: foodName,
      price: price.toInt(), // Đảm bảo định dạng giá phù hợp
      qty: qty,
      uploadImage: uploadImage,
      category: null,
      categoryId: categoryId,
      description: descriptionBuy,
      userId: userId,
      status: null,
    );
  }

  // Chuyển đổi dữ liệu CartItem thành một Map
  Map<String, dynamic> toJson() => {
    'foodListId': foodListId,
    'foodName': foodName,
    'price': price,
    'qty': qty,
    'uploadImage': uploadImage,
    'description': description,
    'descriptionBuy': descriptionBuy,
    'categoryId': categoryId,
    'storeName': storeName,
    'userId': userId
  };

  // Chuyển đổi dữ liệu CartItem thành một Map
  Map<String, dynamic> toJsonSentServer() => {
    'foodListId': foodListId,
    'foodName': foodName,
    'price': price.toInt(),
    'qty': qty,
    'uploadImage': uploadImage,
    'description': descriptionBuy,
    'categoryId': categoryId
  };

  // Tạo một đối tượng CartItem từ Map
  factory CartItem.fromJson(Map<String, dynamic> json) {
    return CartItem(
      foodListId: json['foodListId'],
      categoryId: json['categoryId'],
      foodName: json['foodName'],
      price: json['price'],
      qty: json['qty'],
      uploadImage: json['uploadImage'],
      description: json['description'],
      descriptionBuy:  json['descriptionBuy'],
      storeName: json['storeName'],
      userId: json['userId']
    );
  }
  String toString() {
    return "Cart item {" +
        "foodListId=" + foodListId.toString() +
        ", categoryId=" + categoryId.toString() +
        ", foodName='" + foodName + '\'' +
        ", price=" + price.toString() +
        ", qty=" + qty.toString() +
        ", uploadImage='" + uploadImage + '\'' +
        ", description='" + description + '\'' +
        ", descriptionBuy='" + descriptionBuy + '\'' +
        '}';
  }
}
class StoreUserCart {
  String nameStore;
  int userId;
  List<CartItem> items;

  StoreUserCart({
    required this.nameStore,
    required this.userId,
    required this.items,
  });
}