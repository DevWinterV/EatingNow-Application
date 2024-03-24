import 'dart:js';

import 'package:fam/pages/OderFood/orderfood.dart';
import 'package:fam/pages/OrderCustomer/order_details_page.dart';
import 'package:fam/pages/food/recommened_food_detail.dart';
import 'package:fam/pages/home/Login.dart';
import 'package:fam/pages/home/getCurrentLocation_page.dart';
import 'package:fam/pages/profile/profile_detail_page.dart';
import 'package:fam/pages/profile/profile_page.dart';
import 'package:fam/pages/store/details_store_page.dart';
import 'package:flutter/cupertino.dart';
import 'package:go_router/go_router.dart';
import '../auth.dart';
import '../pages/Cart/cartPage.dart';
import '../pages/OrderCustomer/ordercustomer_list.dart';
import '../pages/home/main_food_page.dart';
import '../pages/search/searchPage.dart';
final router = GoRouter(
    routes:[
      GoRoute(
          redirect: (context, state){
            if(auth.currentUser.peek() == null){
              return "/login";
            }
            return null;
          },
          path: "/",
          builder: (context, state) => MainFoodPage(),
          routes: [
          GoRoute(
              path: "getlocation",
              builder: (context, state) => LocationPage(link: ""),
          ),
          GoRoute(
            path: "order",
            redirect: (context, state){
              if(auth.currentUser.peek() == null){
                return "/login";
              }
              return null;
            },
            builder: (context, state) => OrderPage(),
          ),
          GoRoute(
            path: "orderlist",
            redirect: (context, state){
              if(auth.currentUser.peek() == null){
                return "/login";
              }
              return null;
            },
            builder: (context, state) => OrderCustomerPage(),
            routes: [
              GoRoute(
                  path: "ordedetails",
                  builder: (context, state) => OrderDetailsPage(),

              ),
            ]
          ),
          GoRoute(
            path: "cartdetails",
            // redirect: (context, state){
            //   if(auth.currentUser.peek() == null){
            //     return "/login";
            //   }
            //   return null;
            // },
            builder: (context, state) => CartPage(),
          ),
          GoRoute(
            path: "profiledetail",
            redirect: (context, state){
              if(auth.currentUser.peek() == null){
                return "/login";
              }
              return null;
            },
            builder: (context, state) => ProfilePage(),
          ),
          GoRoute(
            path: "viewprofiledetail",
            redirect: (context, state){
              if(auth.currentUser.peek() == null){
                return "/login";
              }
              return null;
            },
            builder: (context, state) => ProfileDetailPage(),
          ),
          GoRoute(
            path: "productdetail",
            builder: (context, state) => RecommenedFoodDetail(),
          ),
          GoRoute(
            path: "storedetail",
            builder: (context, state) => StoreDetailPage(),
          ),
          GoRoute(
            path: "searchpage",
            builder: (context, state) => SearchFoodPage(),
          ),
        ]
      ),
      GoRoute(
          path: "/login",
          builder: (context, state) => LoginPage(),
      ),
      GoRoute(
        path: "/getlocation",
        builder: (context, state) => LocationPage(link: ""),
      ),
    ]
);
