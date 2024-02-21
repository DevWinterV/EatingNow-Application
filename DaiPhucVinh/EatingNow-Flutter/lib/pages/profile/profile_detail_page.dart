import 'dart:io';

import 'package:fam/models/user_account_model.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/dimensions.dart';
import 'package:flutter/material.dart';
import 'package:image_picker/image_picker.dart';
import '../../data/Api/CustomerService.dart';

class ProfileDetailPage extends StatefulWidget {
  @override
  _ProfileDetailPageState createState() => _ProfileDetailPageState();
}

class _ProfileDetailPageState extends State<ProfileDetailPage> {
  late CustomerService customerService;
  TextEditingController _nameController = TextEditingController();
  TextEditingController _phoneController = TextEditingController();
  TextEditingController _emailController = TextEditingController();
  File? _imageFile;

  @override
  void initState() {
    super.initState();
    final arguments =  ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
    final userData =  arguments['data'] as Data;
    // Khởi tạo giá trị cho các TextEditingController nếu có dữ liệu từ userData
     _nameController.text = userData.completeName ??"";
     _phoneController.text = userData.phone ?? "";
     _emailController.text = userData.email ?? "";
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          'Cập nhật thông tin',
          overflow: TextOverflow.ellipsis,
          maxLines: 1,
          style: TextStyle(
            fontSize: Dimensions.font20,
          ),
        ),
        centerTitle: true,
        backgroundColor: AppColors.mainColor,
      ),
      body: Column(
        children: [
          Expanded(
            child: SingleChildScrollView(
              padding: EdgeInsets.all(20.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  _buildImagePicker(),
                  TextField(
                    controller: _nameController,
                    decoration: InputDecoration(
                      labelText: 'Họ tên',
                        hintText: 'Nhập họ tên'
                    ),
                  ),
                  SizedBox(height: 20.0),
                  TextField(
                    controller: _phoneController,
                    decoration: InputDecoration(
                      labelText: 'Số điện thoại',
                      hintText: 'Nhập số điện thoại'
                    ),
                  ),
                  SizedBox(height: 20.0),
                  TextField(
                    controller: _emailController,
                    decoration: InputDecoration(
                      labelText: 'Email',
                        hintText: 'Nhập emall'
                    ),
                  ),
                  SizedBox(height: 20.0),
                  SizedBox(height: 20.0),
                  ElevatedButton(
                    onPressed: () {

                    },
                    child: Text('Lưu thay đổi'),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildImagePicker() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: [
        Text(
          'Hình ảnh',
          style: TextStyle(
            fontSize: 16.0,
            fontWeight: FontWeight.bold,
          ),
        ),
        SizedBox(height: 10.0),
        _imageFile != null
            ? Image.file(
          _imageFile!,
          height: 150.0,
          width: 150.0,
          fit: BoxFit.cover,
        )
            : Placeholder(
          fallbackHeight: 150.0,
          fallbackWidth: 150.0,
        ),
        SizedBox(height: 10.0),
        ElevatedButton(
          onPressed: () async {
            final picker = ImagePicker();
            final pickedImage = await picker.pickImage(source: ImageSource.gallery);
            setState(() {
              _imageFile = File(pickedImage!.path);
            });
          },
          child: Text('Chọn ảnh'),
        ),
      ],
    );
  }
}
