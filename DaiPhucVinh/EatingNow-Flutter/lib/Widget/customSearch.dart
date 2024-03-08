import 'dart:async';

import 'package:flutter/material.dart';

class CustomSearchBar extends StatefulWidget {
  final Function(String) onSubmitted;

  const CustomSearchBar({Key? key, required this.onSubmitted}) : super(key: key);

  @override
  _CustomSearchBarState createState() => _CustomSearchBarState();
}

class _CustomSearchBarState extends State<CustomSearchBar> {
  TextEditingController _controller = TextEditingController();
  StreamController<bool> _searchIconVisibilityStreamController =
  StreamController<bool>();

  @override
  void initState() {
    super.initState();
    _controller.addListener(_onTextChanged);
  }

  @override
  void dispose() {
    _controller.dispose();
    _searchIconVisibilityStreamController.close();
    super.dispose();
  }

  void _onTextChanged() {
    final bool isVisible = _controller.text.isNotEmpty;
    _searchIconVisibilityStreamController.add(isVisible);
  }

  @override
  Widget build(BuildContext context) {
    return Container(
        padding: EdgeInsets.symmetric(horizontal: 10),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(10.0),
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Padding(
              padding: EdgeInsets.only(right: 10),
              child: IconButton(
                  onPressed: () {
                    Navigator.of(context).pop();
                  },
                  icon: Icon(Icons.arrow_circle_left)),
            ),
            Expanded(
              child: TextField(
                controller: _controller,
                onSubmitted: widget.onSubmitted,
                style: TextStyle(color: Colors.black),
                decoration: InputDecoration(
                  border: InputBorder.none,
                  hintText: "Nhập ở đây để tìm kiếm",
                  hintStyle: TextStyle(color: Colors.grey),
                  suffixIcon: StreamBuilder<bool>(
                    stream: _searchIconVisibilityStreamController.stream,
                    initialData: false,
                    builder: (context, snapshot) {
                      return Row(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          if (snapshot.data == true)
                            IconButton(
                              icon: Icon(Icons.search, color: Colors.grey),
                              onPressed: () {
                                if (_controller.text.isNotEmpty) {
                                  widget.onSubmitted(_controller.text);
                                }
                              },
                            ),
                          IconButton(
                            icon: Icon(Icons.clear, color: Colors.grey),
                            onPressed: () {
                              _controller.clear();
                            },
                          ),
                        ],
                      );
                    },
                  ),
                ),
              ),
            )
          ],
        ));
  }
}