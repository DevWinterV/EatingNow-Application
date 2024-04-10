import 'package:fam/models/storenearUser.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:geolocator/geolocator.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../data/Api/GoogleAPIService.dart';
import '../util/Colors.dart';
import '../util/dimensions.dart';
import 'Big_text.dart';
import 'Icon_and_Text_widget.dart';
import 'RatingStars.dart';
import 'Small_text.dart';

class AppColumn extends StatelessWidget {
  final double rating; // Tỉ lệ đánh giá
  final double latitude;
  final double longtitude;
  final double time; // Tỉ lệ đánh giá
  final String text;
  final SharedPreferences prefs;

  const AppColumn({Key?key, required this.text, required this.rating,required this.latitude, required this.time, required this.prefs, required this.longtitude}): super(key: key);
  Future<double> calculateDistanceToStore(double storeLatitude, double storeLongitude) async {
    double distanceInMeters = 0;
    try {
      distanceInMeters = await Geolocator.distanceBetween(
          prefs.getDouble('latitude') ?? 10.3792302, prefs.getDouble('longitude') ?? 105.3872573, storeLatitude, storeLongitude);
    } catch (e) {
      // Xử lý lỗi nếu có
      print("Lỗi khi tính toán khoảng cách: $e");
    }
    return distanceInMeters;
  }

  Future<DistanceAndTime?> calculateDistanceAndTime(String end) async {
    try {
      String start = prefs.getDouble('latitude').toString()+','+prefs.getDouble('longitude').toString();
      final results = await GoogleAPIService().calculateDistanceAndTime(start, end);
      if(results != null){
        return results;
      }
    } catch (e) {
      // Xử lý lỗi nếu có
      print("Lỗi khi tính toán khoảng cách: $e");
    }
    return null;
  }


  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Container(
          height: Dimensions.height40,
          child:
            BigText(text: text, size:  Dimensions.font20,),
            ),
        SizedBox(height: Dimensions.height5,),
        //comment section
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceAround,
          children: [
            Row(
              children: [
                RatingStars(
                  rating: rating,
                  starSize: 15,
                  starColor: AppColors.yellowColor,
                  emptyStarColor: Colors.grey,
                ),
                SizedBox(width: 8),
                SmallText(text: rating.toString()),
                SizedBox(width: 8),
              ],
            ),
            Row(
              children: [
                SmallText(text: "0"),
                SizedBox(width: 8),
                Icon(Icons.comment, size: 15,)
              ],
            )

          ],
        ),
        SizedBox(height: Dimensions.height5,),
        //time and distance
        FutureBuilder<DistanceAndTime?>(
          future: calculateDistanceAndTime(latitude.toString() +","+longtitude.toString()),
          builder: (context, snapshot) {
            if (snapshot.connectionState == ConnectionState.waiting) {
              return SizedBox(); // Show a loading indicator while waiting for the result
            } else if (snapshot.hasError) {
              return SizedBox();
            } else {
            return
              Row(mainAxisAlignment: MainAxisAlignment.spaceEvenly, children: [
                IconAndTextWidget(
                  icon: Icons.location_on,
                  text: snapshot.data?.distance ?? " ",
                  iconColor: AppColors.mainColor,),
                IconAndTextWidget(
                  icon: Icons.access_time_rounded,
                  text: snapshot.data?.time ?? " ",
                  iconColor: AppColors.mainColor,)
              ]
              );
            }},
        ),
      ],
    );
  }
}
