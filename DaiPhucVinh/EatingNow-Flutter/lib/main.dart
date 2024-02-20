import 'package:fam/data/Api/CustomerService.dart';
import 'package:fam/data/Api/firebase_api.dart';
import 'package:fam/models/LocationData.dart';
import 'package:fam/pages/Cart/cartPage.dart';
import 'package:fam/pages/OderFood/orderfood.dart';
import 'package:fam/pages/food/recommened_food_detail.dart';
import 'package:fam/pages/home/Login.dart';
import 'package:fam/pages/home/getCurrentLocation_page.dart';
import 'package:fam/pages/home/main_food_page.dart';
import 'package:fam/pages/profile/profile_page.dart';
import 'package:fam/pages/store/details_store_page.dart';
import 'package:fam/storage/locationstorage.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/app_constants.dart';
import 'package:firebase_messaging/firebase_messaging.dart';
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
  LocationData? _locationData;
  late LocationStorage locationStorage;

  @override
  void initState() {
    super.initState();
    initializeLocationStorage();
    FirebaseApi().initNotifications();
  }

  Future<void> initializeLocationStorage() async {
    locationStorage = LocationStorage();
    await _loadLocationData();
  }
  // Phương thức để load location data từ SharedPreferences
  Future<void> _loadLocationData() async {
    LocationData? loadedData = await locationStorage.getSavedLocation();
    setState(() {
      _locationData = loadedData;
    });
  }
  @override
  Widget build(BuildContext context) {
    return GetMaterialApp(
      title: AppConstants.APP_NAME,
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: AppColors.mainColor),
        useMaterial3: true,
      ),
      debugShowCheckedModeBanner: false,
      initialRoute: _locationData?.address == null ? '/getlocation' : "/",
      defaultTransition: Transition.rightToLeft,
      getPages: [
        GetPage(
          name: '/',
          page: () => MainFoodPage(),
        ),
        GetPage(
          name: '/login',
          page: () => LoginPage(),
        ),
        GetPage(
          name: '/getlocation',
          page: () => LocationPage(link: ""),
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
          name: '/profiledetail',
          page: () => ProfilePage(),
          // middlewares: [AuthMiddleware()],
        ),
        GetPage(
          name: '/productdetail',
          page: () => RecommenedFoodDetail(),
        ),
        GetPage(
          name: '/storedetail',
          page: () => StoreDetailPage(),
        ),
      ],
    );
  }

}
