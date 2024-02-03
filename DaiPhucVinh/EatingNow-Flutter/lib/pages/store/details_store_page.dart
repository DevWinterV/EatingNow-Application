import 'dart:async';
import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/Widget/app_icon.dart';
import 'package:fam/Widget/exandable.dart';
import 'package:fam/pages/Cart/cartPage.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/dimensions.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:fluttertoast/fluttertoast.dart';

import '../../Widget/Icon_and_Text_widget.dart';
import '../../Widget/RatingStars.dart';
import '../../storage/cartstorage.dart';

class RecommenedFoodDetail extends StatefulWidget {
  const RecommenedFoodDetail({Key? key}) : super(key: key);

  @override
  _RecommenedFoodDetailState createState() => _RecommenedFoodDetailState();
}

class _RecommenedFoodDetailState extends State<RecommenedFoodDetail> {
  // Danh sách nhóm món ăn
  final List<String> foodGroups = ["Món chính", "Tráng miệng", "Nước uống"];

  // Danh sách món ăn theo từng nhóm
  final Map<String, List<String>> foods = {
    "Món chính": ["Cơm gà", "Phở", "Bún riêu"],
    "Tráng miệng": ["Chè", "Bánh flan", "Kem"],
    "Nước uống": ["Nước lọc", "Sinh tố", "Trà sữa"],
  };

  @override
  void initState(){
    super.initState();
  }
  @override
  void dispose(){
    super.dispose();
  }
  @override
  Widget build(BuildContext context) {
    final arguments =  ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
    final dataProduct =  arguments['data']; // Nhận dữ liệu store
    return Scaffold(
      backgroundColor: Colors.white,
      body: CustomScrollView(
        slivers: [
          SliverAppBar(
            toolbarHeight: 70,
            title: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                IconButton(onPressed: (){
                  // Quay lại 
                    Navigator.of(context).pop();
                },
                    icon: Icon(Icons.arrow_circle_left_rounded, color: AppColors.mainColor)),
                IconButton(onPressed: (){
                    // tìm kiếm ...
                },
                    icon: Icon(Icons.search_rounded, color: AppColors.mainColor)),
              ],
            ),
            bottom: PreferredSize(
              preferredSize: Size.fromHeight(100),
              // THONG TIN CỬA HÀNG
              child: Container(
                child: Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      // TEN CUA HANG
                      BigText(
                          size: Dimensions.font26, text: "Cơm tấm Long Xuyên"),
                      // DANH GIA, KHOANG CÁCH, THOI GIAN
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          // Đánh giá sao cửa hàng
                          RatingStars(
                            rating: 4.5,
                            starSize: 15,
                            starColor: AppColors.yellowColor,
                            emptyStarColor: Colors.grey,
                          ),
                          // Khoảng cách
                          IconAndTextWidget(
                            icon: Icons.location_on,
                            text: "1.9 Km",
                            iconColor: AppColors.mainColor,),
                          // Thời gian
                          IconAndTextWidget(
                            icon: Icons.timer,
                            text: "2 phút",
                            iconColor: AppColors.mainColor,),
                        ],
                      ),
                      // THOG TIN DIA CHI - SO DIEN THOAI
                      Column(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: [
                          IconAndTextWidget(
                            icon: Icons.location_on_outlined,
                            text: "Long Xuyên, An Giang",
                            iconColor: AppColors.mainColor,),
                          IconAndTextWidget(
                            icon: Icons.phone,
                            text: "0766837068",
                            iconColor: AppColors.mainColor,),
                        ],
                      )
                    ],
                  )
                ),
                width: double.maxFinite,
                padding: EdgeInsets.only(top: 5, bottom: 10),
                decoration: BoxDecoration(
                    color: Colors.white,
                    borderRadius: BorderRadius.only(
                      topLeft: Radius.circular(Dimensions.radius15),
                      topRight: Radius.circular(Dimensions.radius15),
                    )),
              ),
            ),
            pinned: true,
            backgroundColor: AppColors.yellowColor,
            expandedHeight: 300,
            //hÌNH ẢNH CỬA HÀNG
            flexibleSpace: FlexibleSpaceBar(
              background: dataProduct?.uploadImage != null
                  ? Image.network(
                dataProduct?.uploadImage ?? "",
                width: double.maxFinite,
                fit: BoxFit.cover,
              )
                  : Image.asset(
                "assets/image/logoEN.png",
                width: double.maxFinite,
                fit: BoxFit.cover,
              ),
            ),
          ),
          // MENU CUA HANG
          SliverToBoxAdapter(
            child: Column(
              children: [
            ListView.builder(
            scrollDirection: Axis.horizontal, // Chiều ngang
              itemCount: foodGroups.length,
              itemBuilder: (context, index) {
                String group = foodGroups[index];
                List<String> groupFoods = foods![group]!;

                return Container(
                  margin: EdgeInsets.all(8.0),
                  width: 200.0, // Độ rộng của mỗi nhóm
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        group,
                        style: TextStyle(fontSize: 18.0, fontWeight: FontWeight.bold),
                      ),
                      SizedBox(height: 8.0),
                      Container(
                        height: 150.0, // Độ cao của danh sách món ăn trong mỗi nhóm
                        child: ListView.builder(
                          itemCount: groupFoods.length,
                          itemBuilder: (context, foodIndex) {
                            return ListTile(
                              title: Text(groupFoods[foodIndex]),
                            );
                          },
                        ),
                      ),
                    ],
                  ),
                );
              },
            )
              ],
            ),
          ),
        ],
      ),
      // bottomNavigationBar: Column(
      //   mainAxisSize: MainAxisSize.min,
      //   children: [
      //     Container(
      //       padding: EdgeInsets.only(
      //         left: Dimensions.width20 * 1.8,
      //         right: Dimensions.width20 * 1.8,
      //         top: Dimensions.height30,
      //         bottom: Dimensions.height30,
      //       ),
      //       child: StreamBuilder<int?>(
      //           stream: streamCount.stream,
      //           initialData: countAvailable,
      //           builder: (context, snapshot) {
      //             _quantityController.text = snapshot.data.toString();
      //             if (snapshot.hasData) {
      //               var count = snapshot.data!;
      //               return Row(
      //                 mainAxisAlignment: MainAxisAlignment.spaceBetween,
      //                 children: [
      //                   snapshot.data! != 0 ?
      //                   GestureDetector(
      //                     onTap: () {
      //                       if ((count! + countAvailable) > 0) {
      //                         count = count! - 1;
      //                         streamCount.sink.add(count!);
      //                         ProductStreamSum data = ProductStreamSum();
      //                         data.qty = count;
      //                         data.sum = (count * dataProduct.price!.toDouble()) as double?;
      //                         streamSum.sink.add(data);
      //                       }
      //                     },
      //                     child: AppIcon(
      //                       iconsize: Dimensions.iconSize24,
      //                       iconColor: Colors.white,
      //                       backgroundColor: AppColors.mainColor,
      //                       icon: Icons.remove,
      //                     ),
      //                   ): SizedBox(width: 30,),
      //                   Column(
      //                     children: [
      //                       BigText( text:"Đơn giá: " + NumberFormat.currency(
      //                           locale: 'vi_VN', symbol: '₫')
      //                           .format(dataProduct?.price ?? 0),
      //                         color: AppColors.mainColor,
      //                         size: Dimensions.font16,
      //                       ),
      //
      //                       SmallText( text: "Số lượng: ${(snapshot.data).toString()}",
      //                         color: AppColors.mainColor,
      //                         size: Dimensions.font16,
      //                       ),
      //                     ],
      //                   ),
      //                   GestureDetector(
      //                     onTap: () {
      //                       count = count! + 1;
      //                       streamCount.sink.add(count!);
      //                       ProductStreamSum data = ProductStreamSum();
      //                       data.qty = count;
      //                       data.sum = (count * dataProduct.price!.toDouble()) as double?;
      //                       streamSum.sink.add(data);
      //                     },
      //                     child: AppIcon(
      //                       iconsize: Dimensions.iconSize24,
      //                       iconColor: Colors.white,
      //                       backgroundColor: AppColors.mainColor,
      //                       icon: Icons.add,
      //                     ),
      //                   ),
      //                 ],
      //               );
      //             }
      //             return SizedBox();
      //           }),
      //     ),
      //
      //     StreamBuilder<ProductStreamSum?>(
      //         stream: streamSum.stream,
      //         initialData: productStreamSum,
      //         builder: (context, snapshot) {
      //           var productSum = snapshot.data!;
      //           return Container(
      //             height: Dimensions.bottomHeightBar,
      //             padding: EdgeInsets.only(
      //               top: Dimensions.height30,
      //               bottom: Dimensions.height20,
      //               left: Dimensions.width20,
      //               right: Dimensions.width20,
      //             ),
      //             decoration: BoxDecoration(
      //               color: AppColors.buttonBackqroundColor,
      //               borderRadius: BorderRadius.only(
      //                 topLeft: Radius.circular(Dimensions.radius20 * 2),
      //                 topRight: Radius.circular(Dimensions.radius20 * 2),
      //               ),
      //             ),
      //             child: Row(
      //               mainAxisAlignment: MainAxisAlignment.spaceBetween,
      //               children: [
      //                 Container(
      //                   padding: EdgeInsets.all(Dimensions.width20),
      //                   decoration: BoxDecoration(
      //                     borderRadius:
      //                     BorderRadius.circular(Dimensions.radius20),
      //                     color: Colors.white,
      //                   ),
      //                   child: Icon(
      //                     Icons.favorite,
      //                     color: AppColors.mainColor,
      //                   ),
      //                 ),
      //                 Container(
      //                   padding: EdgeInsets.all(Dimensions.width20),
      //                   child: GestureDetector(
      //                     onTap: () {
      //                       if (productSum.qty! > 0 && dataProduct != null) {
      //                         double price =  dataProduct!.price!.toDouble();
      //                         CartItem newItem = CartItem(
      //                           foodName: dataProduct.foodName ?? "",
      //                           categoryId: dataProduct.categoryId ?? 0,
      //                           price: price,
      //                           qty: productSum.qty!,
      //                           foodListId: dataProduct.foodListId ?? 0,
      //                           uploadImage:
      //                           dataProduct.uploadImage ?? "",
      //                           description: dataProduct.description,
      //                           descriptionBuy: noteController.text,
      //                         );
      //                         _addToCart(newItem);
      //                       }
      //                     },
      //                     child: BigText(
      //                       text: NumberFormat.currency(
      //                           locale: 'vi_VN', symbol: '₫')
      //                           .format(productSum.sum ??    0) +
      //                           " | $updateOrAdd",
      //                       color: Colors.white,
      //                       size: Dimensions.font13,
      //                     ),
      //                   ),
      //                   decoration: BoxDecoration(
      //                     borderRadius:
      //                     BorderRadius.circular(Dimensions.radius20),
      //                     color: (productSum.qty!  +  countAvailable) > 0
      //                         ? AppColors.mainColor
      //                         : Colors.grey,
      //                   ),
      //                 ),
      //               ],
      //             ),
      //           );
      //         }),
      //   ],
      // ),
    );
  }
}

class ProductStreamSum{
  int? qty;
  double? sum;
  ProductStreamSum(){
    qty = 0;
    sum =0;
  }
}