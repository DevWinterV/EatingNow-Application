import 'package:fam/models/storenearUser.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import '../util/Colors.dart';
import '../util/dimensions.dart';
import 'Big_text.dart';
import 'Icon_and_Text_widget.dart';
import 'RatingStars.dart';
import 'Small_text.dart';

class AppColumn extends StatelessWidget {
  final double rating; // Tỉ lệ đánh giá
  final double distance; // Tỉ lệ đánh giá
  final double time; // Tỉ lệ đánh giá
  final String text;
  const AppColumn({Key?key, required this.text, required this.rating,required this.distance, required this.time}): super(key: key);
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
            SmallText(text: "1287"),
            SizedBox(width: 8),
            SmallText(text: "comment"),
          ],
        ),
        SizedBox(height: Dimensions.height5,),
        //time and distance
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            IconAndTextWidget(icon: Icons.location_on,
                text: distance.toString() +" Km" ,
                iconColor: AppColors.mainColor),
            IconAndTextWidget(icon: Icons.access_time_rounded,
                text: time.toString()+" phút",
                iconColor: AppColors.iconColor2)
          ],
        )
      ],
    );
  }
}
