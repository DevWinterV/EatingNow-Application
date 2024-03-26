import 'dart:async';

import 'package:fam/Widget/Big_text.dart';
import 'package:fam/data/Api/FoodRatingService.dart';
import 'package:fam/models/foodRatingResponse.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';

import '../../Widget/Small_text.dart';
import '../../util/Colors.dart';
import '../../util/dimensions.dart';

class PostFoodRating extends StatefulWidget {
  const PostFoodRating({super.key});

  @override
  State<PostFoodRating> createState() => _PostFoodRatingState();
}

class _PostFoodRatingState extends State<PostFoodRating> {
  late FoodRatingService foodRatingService;
  final  _streamDataRating = StreamController<FoodRatingResponse?>.broadcast();
  Stream<FoodRatingResponse?> get getstreamDataRatingController => _streamDataRating.stream;
  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    initService();
    WidgetsBinding.instance.addPostFrameCallback((_) {
      final arguments = ModalRoute.of(context)!.settings.arguments;
      if (arguments != null) {
        initStream(arguments);
      }
    });

  }
  void initService(){
    foodRatingService = FoodRatingService();
  }
  void initStream(arguments) async {
    String OrderID = arguments['orderID'] as String;
    final response = await foodRatingService.TakeFoodsRatingByOrderHeaderId(OrderID);
    if(response != null &&  response!.success == true){
      _streamDataRating.sink.add(response);
    }
    else{
      _streamDataRating.sink.add(null);
    }
  }


  @override
  void dispose() {
    // TODO: implement dispose
    super.dispose();
    _streamDataRating.close();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Column(
          children: [
            Text(
              'Đánh giá sản phẩm',
              overflow: TextOverflow.ellipsis,
              maxLines: 1,
              style: TextStyle(
                fontSize: Dimensions.font20,
              ), // Số dòng tối đa hiển thị (có thể điều chỉnh theo nhu cầu của bạn)
            ),
          ],
        ),
        centerTitle: true, // Để căn giữa tiêu đề trên thanh AppBar
        // Các thuộc tính khác của AppBar
        backgroundColor: AppColors.mainColor, // Màu nền cho AppBar
      ),
      body:
      Stack(
        children: [
          Column(
            children: [
              Expanded(
                child: SingleChildScrollView(
                  padding: EdgeInsets.all(10.0),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: [
                      StreamBuilder(
                          stream: getstreamDataRatingController,
                          builder: (builder, snapshot){
                            print(snapshot.data);
                            if(snapshot.connectionState == ConnectionState.waiting){
                                return Center(
                                  child: CircularProgressIndicator(color: AppColors.mainColor,),
                                );
                              }
                              if(snapshot.data == null){
                                return Center(
                                  child: CircularProgressIndicator(color: AppColors.mainColor,),
                                );
                              }
                              if(snapshot.hasData || snapshot.data!.data!.length > 0){
                                  return Container(
                                    // height: Dimensions.screenHeight,
                                    height: snapshot.data!.data!.length! * 210,
                                    child: ListView.builder(
                                        physics: NeverScrollableScrollPhysics(),
                                        itemCount: snapshot.data!.data?.length ?? 0,
                                        itemBuilder: (itemBuilder, index){
                                          final itemRating = snapshot.data!.data?[index] ?? null;
                                          final _streamStartRating = StreamController<double>();
                                            return Padding(
                                              padding: const EdgeInsets.all(2.0),
                                              child: Container(
                                                decoration: BoxDecoration(
                                                  color: AppColors.buttonBackqroundColor,
                                                  borderRadius: BorderRadius.only(
                                                    topLeft: Radius.circular(Dimensions.radius15 * 1),
                                                    topRight: Radius.circular(Dimensions.radius15 * 1),
                                                    bottomLeft: Radius.circular(Dimensions.radius15 * 1),
                                                    bottomRight: Radius.circular(Dimensions.radius15 * 1),
                                                  ),
                                                ),
                                                height: 180,
                                                width: MediaQuery.of(context).size.width,
                                                child:  StreamBuilder<double>(
                                                  initialData: itemRating?.rating ?? 0,
                                                  stream: _streamStartRating.stream,
                                                  builder: (BuildContext context, AsyncSnapshot<double> snapshot) {
                                                    return Column(
                                                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                                      children: [
                                                        BigText(text: itemRating?.foodName ?? "", size: Dimensions.font20),
                                                        Row(
                                                          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                                                          children: List.generate( 5, (index) {
                                                            return IconButton(
                                                              onPressed: () {
                                                                _streamStartRating.sink.add(index + 1);
                                                                itemRating?.rating = index + 1;
                                                              },
                                                              icon: Icon(
                                                                index < snapshot!.data!
                                                                    ? Icons.star
                                                                    : Icons.star_border_outlined,
                                                                color: index < snapshot!.data! ? Colors.orange : Colors.grey,
                                                              ),
                                                            );
                                                          }),
                                                        ),
                                                        Padding(
                                                          padding: EdgeInsets.only(left: 5, right: 5, ),
                                                          child: TextField(
                                                            controller: TextEditingController()..text = itemRating!.comment ?? "", // Set giá trị ban đầu
                                                            onChanged: (value){
                                                              itemRating!.comment = value;
                                                            },
                                                            decoration: InputDecoration(
                                                              hintText: 'Mô tả đánh giá của bạn (không bắt buộc)',
                                                              border: OutlineInputBorder(),
                                                              contentPadding: EdgeInsets.symmetric(vertical: 10.0), // Điều chỉnh chiều cao
                                                            ),
                                                          ),
                                                        ),
                                                        Padding(
                                                          padding: EdgeInsets.only(left: 5, right: 5 ,bottom: 2),
                                                          child: ElevatedButton(
                                                              style: ElevatedButton.styleFrom(
                                                                // Customize the background color
                                                                foregroundColor: Colors.white,
                                                                backgroundColor: AppColors.mainColor,
                                                                // Add other customizations as needed
                                                                padding: EdgeInsets.only(right: 20.0, left: 20.0),
                                                                shape: RoundedRectangleBorder(
                                                                  borderRadius: BorderRadius.circular(10.0),
                                                                ),
                                                                minimumSize: Size(double.infinity, 50), // Đặt kích thước tối thiểu cho nút
                                                              ),
                                                              onPressed: snapshot.data  ==  0 ? null :  () async {
                                                                if(itemRating!.reviewed == false){
                                                                  itemRating!.reviewed = true;
                                                                  final response  = await foodRatingService.CreateFoodRating(itemRating!.toRequest());
                                                                  if(response.success == true){
                                                                    // Đã gửi dữ liệu thành công
                                                                    Fluttertoast.showToast(msg: "Cảm ơn bạn đã gửi đánh giá ${itemRating?.foodName} cho cửa hàng",
                                                                        toastLength: Toast.LENGTH_LONG,
                                                                        gravity: ToastGravity.BOTTOM_LEFT,
                                                                        backgroundColor: AppColors.toastSuccess,
                                                                        textColor: Colors.black54,
                                                                        timeInSecForIosWeb: 1,
                                                                        fontSize: Dimensions.font13);
                                                                  }
                                                                }
                                                                else{
                                                                  final response  = await foodRatingService.UpdateFoodRating(itemRating!.toRequest());
                                                                  if(response.success == true){
                                                                    // Đã gửi dữ liệu thành công
                                                                    Fluttertoast.showToast(msg: "Cập nhật lại đánh giá thành công",
                                                                        toastLength: Toast.LENGTH_LONG,
                                                                        gravity: ToastGravity.BOTTOM_LEFT,
                                                                        backgroundColor: AppColors.toastSuccess,
                                                                        textColor: Colors.black54,
                                                                        timeInSecForIosWeb: 1,
                                                                        fontSize: Dimensions.font13);
                                                                  }
                                                                }
                                                              },
                                                              child:
                                                              itemRating!.reviewed == false ?
                                                              Text('Đánh giá', style: TextStyle(fontSize: Dimensions.font16),)
                                                                  :
                                                              Text('Gửi lại đánh giá', style: TextStyle(fontSize: Dimensions.font16),)
                                                          ),
                                                        ),
                                                      ],
                                                    );
                                                  },
                                                ),
                                              ),
                                            );
                                        }),
                                  );
                              }
                              return Center(
                                child: SmallText(text: "Chưa tải được dữ liệu đánh giá",color: Colors.black, size: Dimensions.font14,),
                              );
                          })
                    ],
                  ),
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }
}



