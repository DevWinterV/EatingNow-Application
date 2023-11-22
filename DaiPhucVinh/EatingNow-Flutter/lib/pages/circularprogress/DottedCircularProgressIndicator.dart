import 'dart:math';
import 'package:flutter/material.dart';

class DottedCircularProgressIndicator extends StatefulWidget {
  final double radius;
  final Color color;
  final double dotRadius;
  final int numberOfDots;

  DottedCircularProgressIndicator({
    required this.radius,
    required this.color,
    required this.dotRadius,
    required this.numberOfDots,
  });

  @override
  _DottedCircularProgressIndicatorState createState() =>
      _DottedCircularProgressIndicatorState();
}

class _DottedCircularProgressIndicatorState
    extends State<DottedCircularProgressIndicator>
    with SingleTickerProviderStateMixin {
  late AnimationController _controller;

  @override
  void initState() {
    super.initState();
    _controller = AnimationController(
      vsync: this,
      duration: Duration(seconds: 1),
    )..repeat();
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return CustomPaint(
      size: Size(widget.radius * 2, widget.radius * 2),
      painter: DottedCircularProgressPainter(
        color: widget.color,
        dotRadius: widget.dotRadius,
        numberOfDots: widget.numberOfDots,
        animation: _controller,
      ),
    );
  }
}

class DottedCircularProgressPainter extends CustomPainter {
  final Color color;
  final double dotRadius;
  final int numberOfDots;
  final Animation<double> animation;

  DottedCircularProgressPainter({
    required this.color,
    required this.dotRadius,
    required this.numberOfDots,
    required this.animation,
  }) : super(repaint: animation);

  @override
  void paint(Canvas canvas, Size size) {
    final double angleStep = 2 * pi / numberOfDots;

    for (int i = 0; i < numberOfDots; i++) {
      final double angle = animation.value * 2 * pi - i * angleStep;

      final double cx = size.width / 2;
      final double cy = size.height / 2;

      final double startX = cx + (size.width / 2 - dotRadius) * cos(angle);
      final double startY = cy + (size.height / 2 - dotRadius) * sin(angle);

      final paint = Paint()
        ..color = color
        ..strokeWidth = 2.0
        ..strokeCap = StrokeCap.round;

      canvas.drawCircle(Offset(startX, startY), dotRadius, paint);
    }
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) {
    return true;
  }
}
