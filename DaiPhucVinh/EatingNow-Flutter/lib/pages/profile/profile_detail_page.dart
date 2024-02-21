import 'dart:io';
import 'package:fam/models/customerReqeust.dart';
import 'package:fam/models/user_account_model.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/app_constants.dart';
import 'package:fam/util/dimensions.dart';
import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';
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
  void initState(){
    super.initState();
    customerService = CustomerService(apiUrl: AppConstants.UpdateInfoCustomer);
  }
  bool isLoading = false;

  void _updateInfo(EN_CustomerRequest request) async {
    setState(() {
      isLoading = true; // Bắt đầu hiển thị vòng tròn load
    });

    final response = await customerService.updateInfoCustomer(request, imageFile : _imageFile);
    if(response.success == true){
      Fluttertoast.showToast(msg: "Cập nhật thông tin thành công !",
          toastLength: Toast.LENGTH_LONG,
          gravity: ToastGravity.BOTTOM_LEFT,
          backgroundColor: AppColors.toastSuccess,
          textColor: Colors.black54,
          timeInSecForIosWeb: 1,
          fontSize: 10
      );
      Navigator.of(context).pop(true);
    }
    else{
      Fluttertoast.showToast(msg: "Đã xảy ra lỗi khi cập nhật thông tin !",
          toastLength: Toast.LENGTH_LONG,
          gravity: ToastGravity.BOTTOM_LEFT,
          backgroundColor: Colors.red[400],
          textColor: Colors.black54,
          timeInSecForIosWeb: 1,
          fontSize: 10
      );
    }

    setState(() {
      isLoading = false; // Kết thúc hiển thị vòng tròn load
    });
  }

  @override
  Widget build(BuildContext context) {
    final arguments =  ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
    final userData =  arguments['data'] as Data;
    String imageprofile = userData.imageProfile ?? "";
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
      body:
      Stack(
        children: [
          Column(
            children: [
              Expanded(
                child: SingleChildScrollView(
                  padding: EdgeInsets.all(20.0),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: [
                      _buildImagePicker(userData, imageprofile),
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
                          if(_nameController.text =="" ||   _phoneController.text =="" ){
                            Fluttertoast.showToast(msg: "Vui lòng điền đủ thông tin!",
                                toastLength: Toast.LENGTH_LONG,
                                gravity: ToastGravity.BOTTOM_LEFT,
                                backgroundColor: Colors.red[400],
                                textColor: Colors.black54,
                                timeInSecForIosWeb: 1,
                                fontSize: 10
                            );
                            return;
                          }
                          final request = EN_CustomerRequest(customerId: userData.customerId ?? "", completeName: _nameController.text, phone: _phoneController.text, email: _emailController.text, status: true, ImageProfile: userData.imageProfile ?? '');
                          _updateInfo(request);
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
          if (isLoading)
            Center(
              child : CircularProgressIndicator(color: AppColors.mainColor,),
            ),
        ],
      ),
    );

  }

  Widget _buildImagePicker(Data userdata, String? imageprofile) {
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
            _imageFile != null && userdata!.imageProfile != null && userdata.imageProfile!.isNotEmpty?
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
        userdata?.imageProfile != null && userdata.imageProfile!.isNotEmpty ?
        Center(
              child: Stack(
                children: [
                  ClipOval(
                    child:
                    Image.network(
                            userdata.imageProfile ?? "",
                            height: 100,
                            width: 100,
                    )
                  ),
                  Positioned(
                    bottom: 0,
                    right: 0,
                    child: Container(
                      width: 23, // Specify the desired width
                      height: 23, // Specify the desired height
                      decoration: BoxDecoration(
                        borderRadius: BorderRadius.circular(15), // Adjust border radius accordingly
                        color: Colors.white60,
                      ),
                      child: GestureDetector(
                        onTap: () {
                          setState(() {
                            userdata.imageProfile = null;
                          });
                        },
                        child: Icon(
                          Icons.delete,
                          size: 20,
                          color: Colors.red,
                        ),
                      ),
                    ),
                  )
                ],
              ),
            )
              :
            SizedBox(),
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
