import 'dart:async';
import 'package:fam/Widget/Big_text.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/Widget/app_icon.dart';
import 'package:fam/Widget/exandable.dart';
import 'package:fam/models/product_recommended_model.dart';
import 'package:fam/pages/Cart/cartPage.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/dimensions.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:fluttertoast/fluttertoast.dart';

import '../../storage/cartstorage.dart';

class RecommenedFoodDetail extends StatefulWidget {
  const RecommenedFoodDetail({Key? key}) : super(key: key);

  @override
  _RecommenedFoodDetailState createState() => _RecommenedFoodDetailState();
}

class _RecommenedFoodDetailState extends State<RecommenedFoodDetail> {
  late TextEditingController _quantityController = TextEditingController();
  final streamCount = StreamController<int>();
  final streamSum = StreamController<ProductStreamSum>();
  String updateOrAdd = "";
  double suminit = 0;
  int countAvailable = 0;
// Phương thức để thêm một món ăn vào giỏ hàng
  Future<void> _addToCart(CartItem item) async {
    String msgAdd = '';
    if (countAvailable > 0) {
      msgAdd = 'Đã cập nhật sản phẩm vào giỏ hàng';
    } else {
      msgAdd = 'Đã thêm vào giỏ hàng';
    }
    // Lưu lại danh sách giỏ hàng sau khi thêm
    if (await CartStorage.addToCart(item)) {
      Fluttertoast.showToast(
          msg: msgAdd,
          toastLength: Toast.LENGTH_LONG,
          gravity: ToastGravity.BOTTOM_LEFT,
          backgroundColor: AppColors.toastSuccess,
          textColor: Colors.black54,
          timeInSecForIosWeb: 1,
          fontSize: 13);
      Navigator.of(context).pop(true); // true indicates that something has changed
    }
  }
  @override
  void initState(){
    super.initState();
  }
  @override
  void dispose(){
    super.dispose();
    streamCount.close();
    streamSum.close();
  }
  @override
  Widget build(BuildContext context) {
    ProductStreamSum productStreamSum = ProductStreamSum();
    TextEditingController noteController = TextEditingController();
    final arguments =  ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
    final dataProduct =  arguments['data']; // Nhận dữ liệu từ arguments
    countAvailable = dataProduct is CartItem ? dataProduct!.qty : 0;
    productStreamSum.qty = countAvailable;
    productStreamSum.sum = (countAvailable.toDouble() * dataProduct?.price ?? 0) as double;
    updateOrAdd = dataProduct is CartItem ? "Cập nhật vào giỏ hàng" : "Thêm vào giỏ hàng";
    noteController.text = dataProduct is CartItem ? dataProduct.descriptionBuy : "";
    return Scaffold(
      backgroundColor: Colors.white,
      body: CustomScrollView(
        slivers: [
          SliverAppBar(
            toolbarHeight: 70,
            title: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                // GestureDetector(
                //   onTap: () {
                //     Navigator.push(
                //       context,
                //       MaterialPageRoute(builder: (context) => CartPage()),
                //     );
                //   },
                //   child: AppIcon(
                //     icon: Icons.shopping_cart_outlined,
                //     backgroundColor: AppColors.mainColor,
                //     iconColor: Colors.white,
                //   ),
                // ),
              ],
            ),
            bottom: PreferredSize(
              preferredSize: Size.fromHeight(20),
              // TÊN MÓN ĂN
              child: Container(
                // Tên món ăn
                child: Center(
                  child: BigText(
                      size: Dimensions.font26, text: dataProduct!.foodName!),
                ),
                width: double.maxFinite,
                padding: EdgeInsets.only(top: 5, bottom: 10),
                decoration: BoxDecoration(
                    color: Colors.white,
                    borderRadius: BorderRadius.only(
                      topLeft: Radius.circular(Dimensions.radius20),
                      topRight: Radius.circular(Dimensions.radius20),
                    )),
              ),
            ),
            pinned: true,
            backgroundColor: AppColors.yellowColor,
            expandedHeight: 300,
            //hÌNH ẢNH MÓN ĂN
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
          // PHẦN MÔ TẢ MÓN ĂN
          SliverToBoxAdapter(
            child: Column(
              children: [
                Container(
                  child: ExpandableTextWidget(
                      text: dataProduct?.description ?? "..."),
                  margin: EdgeInsets.only(
                      left: Dimensions.width20, right: Dimensions.width20),
                ),
                SizedBox(height: 20),
                // Add some space between description and note input
                Container(
                  margin: EdgeInsets.symmetric(horizontal: Dimensions.width20),
                  child: TextField(
                    controller: noteController,
                    onChanged: (value) {
                        noteController.text = "";
                        noteController.text = value;
                    },
                    decoration: InputDecoration(
                      labelText: 'Ghi chú món ăn',
                      hintText: "Không cay, ít đường, ít tiêu, ...",
                      border: OutlineInputBorder(),
                    ),
                    maxLines: 1,
                    keyboardType: TextInputType.multiline,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
      bottomNavigationBar: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          Container(
            padding: EdgeInsets.only(
              left: Dimensions.width20 * 1.8,
              right: Dimensions.width20 * 1.8,
              top: Dimensions.height30,
              bottom: Dimensions.height30,
            ),
            child: StreamBuilder<int?>(
                stream: streamCount.stream,
                initialData: countAvailable,
                builder: (context, snapshot) {
                  _quantityController.text = snapshot.data.toString();
                  if (snapshot.hasData) {
                    var count = snapshot.data!;
                    return Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        snapshot.data! != 0 ?
                        GestureDetector(
                          onTap: () {
                            if ((count! + countAvailable) > 0) {
                              count = count! - 1;
                              streamCount.sink.add(count!);
                              ProductStreamSum data = ProductStreamSum();
                              data.qty = count;
                              data.sum = (count * dataProduct.price!.toDouble()) as double?;
                              streamSum.sink.add(data);
                            }
                          },
                          child: AppIcon(
                            iconsize: Dimensions.iconSize24,
                            iconColor: Colors.white,
                            backgroundColor: AppColors.mainColor,
                            icon: Icons.remove,
                          ),
                        ): SizedBox(width: 30,),
                        Column(
                          children: [
                            BigText( text:"Đơn giá: " + NumberFormat.currency(
                                locale: 'vi_VN', symbol: '₫')
                                .format(dataProduct?.price ?? 0),
                              color: AppColors.mainColor,
                              size: Dimensions.font16,
                            ),

                            SmallText( text: "Số lượng: ${(snapshot.data).toString()}",
                              color: AppColors.mainColor,
                              size: Dimensions.font16,
                            ),
                          ],
                        ),
                        GestureDetector(
                          onTap: () {
                            count = count! + 1;
                            streamCount.sink.add(count!);
                            ProductStreamSum data = ProductStreamSum();
                            data.qty = count;
                            data.sum = (count * dataProduct.price!.toDouble()) as double?;
                            streamSum.sink.add(data);
                          },
                          child: AppIcon(
                            iconsize: Dimensions.iconSize24,
                            iconColor: Colors.white,
                            backgroundColor: AppColors.mainColor,
                            icon: Icons.add,
                          ),
                        ),
                      ],
                    );
                  }
                  return SizedBox();
                }),
          ),
          StreamBuilder<ProductStreamSum?>(
              stream: streamSum.stream,
              initialData: productStreamSum,
              builder: (context, snapshot) {
                var productSum = snapshot.data!;
                  return Container(
                    height: Dimensions.bottomHeightBar,
                    padding: EdgeInsets.only(
                      top: Dimensions.height30,
                      bottom: Dimensions.height20,
                      left: Dimensions.width20,
                      right: Dimensions.width20,
                    ),
                    decoration: BoxDecoration(
                      color: AppColors.buttonBackqroundColor,
                      borderRadius: BorderRadius.only(
                        topLeft: Radius.circular(Dimensions.radius20 * 2),
                        topRight: Radius.circular(Dimensions.radius20 * 2),
                      ),
                    ),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Container(
                          padding: EdgeInsets.all(Dimensions.width20),
                          decoration: BoxDecoration(
                            borderRadius:
                            BorderRadius.circular(Dimensions.radius20),
                            color: Colors.white,
                          ),
                          child: Icon(
                            Icons.favorite,
                            color: AppColors.mainColor,
                          ),
                        ),
                        Container(
                          padding: EdgeInsets.all(Dimensions.width20),
                          child: GestureDetector(
                            onTap: () {
                              if (productSum.qty! > 0 && dataProduct != null) {
                                double price =  dataProduct!.price!.toDouble();
                                print('dataProduct.userId: ${dataProduct.userId}');
                                print('dataProduct.storename: ${dataProduct.storeName}');
                                CartItem newItem = CartItem(
                                  foodName: dataProduct.foodName ?? "",
                                  categoryId: dataProduct.categoryId ?? 0,
                                  price: price,
                                  qty: productSum.qty!,
                                  foodListId: dataProduct.foodListId ?? 0,
                                  uploadImage:
                                  dataProduct.uploadImage ?? "",
                                  description: dataProduct.description,
                                  descriptionBuy: noteController.text,
                                  userId: dataProduct.userId ?? 0,
                                  storeName: dataProduct.storeName ?? ""
                                );
                                print(newItem.storeName);
                                _addToCart(newItem);
                              }
                            },
                            child: BigText(
                              text: NumberFormat.currency(
                                  locale: 'vi_VN', symbol: '₫')
                                  .format(productSum.sum ??    0) +
                                  " | $updateOrAdd",
                              color: Colors.white,
                              size: Dimensions.font13,
                            ),
                          ),
                          decoration: BoxDecoration(
                            borderRadius:
                            BorderRadius.circular(Dimensions.radius20),
                            color: (productSum.qty!  +  countAvailable) > 0
                                ? AppColors.mainColor
                                : Colors.grey,
                          ),
                        ),
                      ],
                    ),
                  );
              }),
        ],
      ),
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