import 'package:flutter/material.dart';

class CustomSearchBar extends StatefulWidget {
  final Function(String) onSubmitted;

  const CustomSearchBar({Key? key, required this.onSubmitted}) : super(key: key);

  @override
  _CustomSearchBarState createState() => _CustomSearchBarState();
}

class _CustomSearchBarState extends State<CustomSearchBar> {
  TextEditingController _controller = TextEditingController();

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
            child:  IconButton(
                onPressed: (){
                  Navigator.of(context).pop();
                },
                icon: Icon(Icons.arrow_circle_left)
            ),
          ),
          Expanded(
            child:  TextField(
            controller: _controller,
            onSubmitted: widget.onSubmitted,
            style: TextStyle(color: Colors.black),
            decoration: InputDecoration(
              border: InputBorder.none,
              hintText: "Nhập ở đây để tìm kiếm ...",
              hintStyle: TextStyle(color: Colors.grey),
              suffixIcon: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  IconButton(
                    icon: Icon(Icons.clear, color: Colors.grey),
                    onPressed: () {
                      _controller.clear();
                    },
                  ),
                ],
              ),
            ),
          ),)
        ],
      )
    );
  }
}
