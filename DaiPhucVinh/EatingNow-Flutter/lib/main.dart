
import 'package:fam/pages/Cart/cartPage.dart';
import 'package:fam/pages/OderFood/orderfood.dart';
import 'package:fam/pages/food/popular_food_detail.dart';
import 'package:fam/pages/food/recommened_food_detail.dart';
import 'package:fam/pages/home/getCurrentLocation_page.dart';
import 'package:fam/pages/home/Login.dart';
import 'package:fam/pages/home/main_food_page.dart';
import 'package:fam/storage/cartstorage.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:flutter_native_splash/flutter_native_splash.dart';
import 'package:firebase_core/firebase_core.dart';
import 'Middleware/AuthMiddleware.dart';
import 'firebase_options.dart';

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await Firebase.initializeApp(
    options: DefaultFirebaseOptions.currentPlatform,
  );
  FlutterNativeSplash.preserve(widgetsBinding: WidgetsFlutterBinding.ensureInitialized());
  runApp(MyApp());
}
class MyApp extends StatefulWidget {
  const MyApp({Key? key}) : super(key: key);

  @override
  _MyAppState createState() => _MyAppState();
}

class _MyAppState extends State<MyApp> {
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

/*
  @override
  Widget build(BuildContext context) {
    return GetMaterialApp(
      title: 'Eating Now',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.deepPurple),
        useMaterial3: true,
      ),
      debugShowCheckedModeBanner: false,
      initialRoute: '/login', // Mặc định
      routes: {
        '/': (context) => MainFoodPage(), // trang chính
        '/login': (context) => LoginPage(), // Lấy vị trí
        '/getlocation': (context) => LocationPage(link: ""), // Lấy vị trí
        '/order': (context) => OrderPage(),// Đặt hàng
        '/cartdetails': (context) => CartPage(),// Chi tiết giỏ hàng
        '/productdetail': (context) => RecommenedFoodDetail(),// Chi tiết giỏ hàng

        // Define more routes as needed
      },
    );
  }*/
  @override
  Widget build(BuildContext context) {
    return GetMaterialApp(
      title: 'XpressEat',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.deepPurple),
        useMaterial3: true,
      ),
      debugShowCheckedModeBanner: false,
      initialRoute: '/getlocation',
      defaultTransition: Transition.rightToLeft,
      getPages: [
        GetPage(
          name: '/',
          page: () => MainFoodPage(),
       //   middlewares: [AuthMiddleware()],
        ),
        GetPage(
          name: '/login',
          page: () => LoginPage(),
        ),
        GetPage(
          name: '/getlocation',
          page: () => LocationPage(link: ""),
       //   middlewares: [AuthMiddleware()],
        ),
        GetPage(
          name: '/order',
          page: () => OrderPage(),
          middlewares: [AuthMiddleware()],
        ),
        GetPage(
          name: '/cartdetails',
          page: () => CartPage(),
          middlewares: [AuthMiddleware()],
        ),
        GetPage(
          name: '/productdetail',
          page: () => RecommenedFoodDetail(),
       //   middlewares: [AuthMiddleware()],
        ),
      ],
    );
  }
}
