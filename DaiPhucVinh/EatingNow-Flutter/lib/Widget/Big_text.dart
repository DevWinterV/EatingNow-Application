
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import '../util/dimensions.dart';
class BigText extends StatelessWidget {
  final Color? color;
  final String text;
  final int maxlines;
  double size;
  BigText({
    Key? key,
    this.color = const Color(0xFF332d2b),
    required this.text,
    this.size = 0,
    this.maxlines = 2
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.all(1),
      child: Text(
        text,
        maxLines: maxlines, // Số dòng tối đa
        overflow: TextOverflow.ellipsis, // Sử dụng ellipsis nếu vượt quá 2 dòng
        style: TextStyle(
          fontFamily: 'Roboto',
          color: color,
          fontSize: size == 0 ? Dimensions.font16 : size,
          fontWeight: FontWeight.w500,
        ),
      ),
    );
  }
}
