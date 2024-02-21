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
  Widget build(BuildContext context) {
    final arguments =  ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
    final userData =  arguments['data'] as Data;
    // Khởi tạo giá trị cho các TextEditingController nếu có dữ liệu từ userData
    _nameController.text = userData.completeName ??"";
    _phoneController.text = userData.phone ?? "";
    _emailController.text = userData.email ?? "";
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
                  _buildImagePicker(userData),
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
                    onPressed: () async {

                    },
                    child: Text(
                      "Lưu thay đổi",
                      style: TextStyle(
                        color: Colors.black,
                        fontSize: Dimensions.font16,
                      ),
                    ),
                    style: ElevatedButton.styleFrom(
                      primary: AppColors.mainColor,
                      minimumSize: Size(double.infinity, 43),
                    ),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildImagePicker(Data userdata) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Text(
              'Hình ảnh',
              style: TextStyle(
                fontSize: 16.0,
                fontWeight: FontWeight.bold,
              ),
            ),
            _imageFile != null && userdata!.imageProfile != null ?
            GestureDetector(
              onTap: (){
                setState(() {
                  _imageFile = null;
                });
              },
              child: Text("Sử dụng ảnh cũ",
                style: TextStyle(
                  color: Colors.blue
              ),),
            ) : SizedBox(),
            _imageFile != null ?
            GestureDetector(
              onTap: (){
                setState(() {
                  _imageFile = null;
                });
              },
              child: Text("Hủy chọn ảnh",
                style: TextStyle(
                    color: Colors.red
                ),),
            ) : SizedBox(),
          ],
        ),
        SizedBox(height: 10.0),
        _imageFile != null
            ?
          Image.file(
            _imageFile!,
            fit: BoxFit.contain,
            height: 100,
            width: 100,
          )
            :
        userdata.imageProfile != null ?
        ClipOval(
          child:
          Image.network(
              userdata.imageProfile ?? "",
            height: 100,
            width: 100,
          ),
        ) : SizedBox(),
        SizedBox(height: 10.0),
        ElevatedButton(
          onPressed: () async {
            final picker = ImagePicker();
            final pickedImage = await picker.pickImage(source: ImageSource.gallery);
            setState(() {
              _imageFile = File(pickedImage!.path);
            });
          },
          child: Text(
            "Chọn ảnh",
            style: TextStyle(
              color: Colors.black,
              fontSize: Dimensions.font16,
            ),
          ),
          style: ElevatedButton.styleFrom(
            primary: AppColors.mainColor,
            minimumSize: Size(double.infinity, 43),
          ),
        ),
      ],
    );
  }
}
