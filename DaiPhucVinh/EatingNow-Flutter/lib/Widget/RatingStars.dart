import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import '../util/Colors.dart';

class RatingStars extends StatelessWidget {
  final double rating;
  final double starSize;
  final Color starColor;
  final Color emptyStarColor;

  RatingStars({
    required this.rating,
    this.starSize = 15,
    this.starColor =Colors.yellow,
    this.emptyStarColor = Colors.grey,
  });

  @override
  Widget build(BuildContext context) {
    int fullStars = rating.floor();
    bool hasHalfStar = (rating - fullStars) >= 0.5;

    return Row(
      mainAxisSize: MainAxisSize.min,
      children: List.generate(5, (index) {
        if (index < fullStars) {
          return Icon(Icons.star, color: starColor, size: starSize);
        } else if (hasHalfStar && index == fullStars) {
          return Icon(Icons.star_half, color: starColor, size: starSize);
        } else {
          return Icon(Icons.star, color: emptyStarColor, size: starSize);
        }
      }),
    );
  }
}

