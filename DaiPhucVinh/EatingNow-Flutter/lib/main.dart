import 'package:fam/Middleware/AuthMiddleware.dart';
import 'package:fam/data/Api/firebase_api.dart';
import 'package:fam/pages/maps/data_handler/AppData.dart';
import 'package:fam/pages/maps/maps_page.dart';
import 'package:fam/pages/rating_food/post_foodRating.dart';
import 'package:permission_handler/permission_handler.dart'
;import 'package:fam/pages/OderFood/orderfood.dart';
import 'package:fam/pages/OrderCustomer/order_details_page.dart';
import 'package:fam/pages/food/recommened_food_detail.dart';
import 'package:fam/pages/home/Login.dart';
import 'package:fam/pages/home/getCurrentLocation_page.dart';
import 'package:fam/pages/profile/profile_detail_page.dart';
import 'package:fam/pages/profile/profile_page.dart';
import 'package:fam/pages/store/details_store_page.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:provider/provider.dart';
import '../pages/Cart/cartPage.dart';
import '../pages/OrderCustomer/ordercustomer_list.dart';
import '../pages/home/main_food_page.dart';
import '../pages/search/searchPage.dart';
import 'package:fam/storage/locationstorage.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/app_constants.dart';
import 'package:fam/util/notificationService.dart';
import 'package:flutter_native_splash/flutter_native_splash.dart';
import 'package:firebase_core/firebase_core.dart';
import 'package:get/get_navigation/src/root/get_material_app.dart';
import 'package:get/get_navigation/src/routes/transitions_type.dart';
import 'Middleware/LocationMiddleware.dart';
import 'firebase_options.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await Firebase.initializeApp(
    options: DefaultFirebaseOptions.currentPlatform,
  );
  FirebaseApi().initNotifications();
  FlutterNativeSplash.preserve(widgetsBinding: WidgetsFlutterBinding.ensureInitialized());
  await NotificationService.init();
  initialization();
  runApp(MultiProvider(
      providers: [
        ChangeNotifierProvider(create: (context) => AppData()),
        // Add other providers if needed
      ],
      child: const MyApp()
  ));
}
// Hàm xóa SplassCreen
void initialization() async {
  FlutterNativeSplash.remove();
}


class MyApp extends StatefulWidget {
  const MyApp({Key? key}) : super(key: key);
  @override
  _MyAppState createState() => _MyAppState();
}

class _MyAppState extends State<MyApp> {
  @override
  Widget build(BuildContext context)  {
    return GetMaterialApp(
      title: AppConstants.APP_NAME,
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: AppColors.mainColor),
        useMaterial3: true,
      ),
      debugShowCheckedModeBanner: false,
        initialRoute: "/",
      defaultTransition: Transition.rightToLeft,
      getPages:  mainRoutes
    );
  }
  final mainRoutes = [
    GetPage(
      name: '/',
      page: () => MainFoodPage(),
      middlewares: [LocationMiddleware()],
    ),
    GetPage(
      name: '/getlocation',
      page: () => LocationPage(link: ""),
    ),
    GetPage(
      name: '/getlocationFromMap',
      page: () => GoogleMapsPage(),
    ),
    GetPage(
      name: '/login',
      page: () => LoginPage(),
    ),
    GetPage(
      name: '/order',
      page: () => OrderPage(),
      middlewares: [AuthMiddleware()],
    ),
    GetPage(
      name: '/orderlist',
      page: () => OrderCustomerPage(),
      middlewares: [AuthMiddleware()],
    ),
    GetPage(
      name: '/cartdetails',
      page: () => CartPage(),
      // middlewares: [AuthMiddleware()],
    ),
    GetPage(
      name: '/profiledetail',
      page: () => ProfilePage(),
      middlewares: [AuthMiddleware()],
    ),
    GetPage(
      name: '/viewprofiledetail',
      page: () => ProfileDetailPage(),
      middlewares: [AuthMiddleware()],
    ),
    GetPage(
      name: '/productdetail',
      page: () => RecommenedFoodDetail(),
    ),
    GetPage(
      name: '/storedetail',
      page: () => StoreDetailPage(),
    ),
    GetPage(
      name: '/ordedetails',
      page: () => OrderDetailsPage(),
    ),
    GetPage(
      name: '/searchpage',
      page: () => SearchFoodPage(),
    ),
    GetPage(
      name: '/rating',
      page: () => PostFoodRating(),
    ),
  ];

}
