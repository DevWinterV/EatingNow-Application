import 'dart:async';

import 'package:avatar_glow/avatar_glow.dart';
import 'package:fam/Widget/Small_text.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/dimensions.dart';
import 'package:flutter/material.dart';
import 'package:speech_to_text/speech_to_text.dart' as stt;

class CustomSearchBar extends StatefulWidget {
  final Function(String) onSubmitted;

  const CustomSearchBar({Key? key, required this.onSubmitted}) : super(key: key);

  @override
  _CustomSearchBarState createState() => _CustomSearchBarState();
}

class _CustomSearchBarState extends State<CustomSearchBar> {
  late stt.SpeechToText _speechToText;
  TextEditingController _controller = TextEditingController();
  StreamController<bool> _searchIconVisibilityStreamController =
  StreamController<bool>();
  StreamController<String> _textRecordingStreamController =
  StreamController<String>();
  StreamController<bool> _recordingStreamController =
  StreamController<bool>();

  @override
  void initState() {
    super.initState();
    _controller.addListener(_onTextChanged);
    _searchIconVisibilityStreamController.add(false);
    intiStateSpeech();
  }

  void intiStateSpeech() {
    _speechToText = stt.SpeechToText();
  }

  void updaterecordStream(String val){
    String? spokenText = _speechToText.lastRecognizedWords;
    print(val);
    if (val == "done" || val == "notListening") {
      if (spokenText != "") {
        setState(() {
          _controller.clear();
          _controller.text = spokenText;
        });
        widget.onSubmitted(spokenText);
      }
      _stopSpeech();
    }
  }

  void _listen() async {
    bool available = await _speechToText.initialize(
      onStatus: (val) {
        return updaterecordStream(val);
      },
      onError: (e) =>  onError(e.errorMsg),
    );
    if (available) {
      _recordingStreamController.add(available);
      _speechToText.listen(
          onResult: (val) {
            _controller.text = val.recognizedWords;
            _textRecordingStreamController.add(val.recognizedWords);
          }
      );
    }
  }

  void _stopSpeech() {
    _recordingStreamController.add(false);
    _speechToText.stop();
  }

  void onError(String msg){
    print(msg);
    if(msg == "error_speech_timeout")
      _stopSpeech();
  }

  @override
  void dispose() {
    _controller.dispose();
    _stopSpeech();
    _searchIconVisibilityStreamController.close();
    _recordingStreamController.close();
    _textRecordingStreamController.close();
    super.dispose();
  }

  void _onTextChanged() {
    final bool isVisible = _controller.text.isNotEmpty;
    _searchIconVisibilityStreamController.add(isVisible);
    //_recordingStreamController.add(false);
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
              icon: Icon(Icons.arrow_circle_left),
            ),
          ),
          StreamBuilder<bool>(
            stream: _recordingStreamController.stream,
            initialData: false,
            builder: (context, snapshotRecord) {
              return StreamBuilder<String>(
                stream: _textRecordingStreamController.stream,
                builder: (context, snapshotText) {
                  return Expanded(
                    child: TextField(
                      // canRequestFocus: true,
                      readOnly: snapshotRecord.data ?? false  ,
                      controller: _controller,
                      onSubmitted: widget.onSubmitted,
                      style: TextStyle(color: Colors.black),
                      decoration: InputDecoration(
                        border: InputBorder.none,
                        hintText: snapshotRecord.data == false ? "Tìm sản phẩm, cửa hàng" : "",
                        hintStyle: TextStyle(color: Colors.grey),
                        suffixIcon: StreamBuilder<bool>(
                          stream: _searchIconVisibilityStreamController.stream,
                          initialData: false,
                          builder: (context, snapshotsearch) {
                            return Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              mainAxisSize: MainAxisSize.min,
                              children: [
                                if (snapshotsearch.data == true)
                                  IconButton(
                                    icon: Icon(Icons.search, color: Colors.grey),
                                    onPressed: () {
                                      if (_controller.text.isNotEmpty) {
                                        widget.onSubmitted(_controller.text);
                                      }
                                    },
                                  ),
                                  Container(
                                    height: 36,
                                    width: 40,
                                    child: Center(child: AvatarGlow(
                                      duration: const Duration(milliseconds: 2000),
                                      repeat: true,
                                      glowColor:   snapshotRecord?.data! == true ?  AppColors.mainColor : Colors.white,
                                      glowRadiusFactor: 1,
                                      glowShape: BoxShape.circle,
                                      animate: snapshotRecord?.data! ?? false,
                                      child:  IconButton(
                                        icon: Icon( Icons.mic ,color:  snapshotRecord?.data! == true ? Colors.red : Colors.grey,),
                                        onPressed: () {
                                          if(snapshotRecord.data != null && snapshotRecord.data! == true )
                                            _stopSpeech();
                                          if(snapshotRecord.data == null || snapshotRecord.data! == false  ){
                                            _listen();
                                          }
                                        },
                                      ),),),
                                  ),
                                snapshotRecord.data == false ?
                                IconButton(
                                  icon: Icon(Icons.clear, color: Colors.grey),
                                  onPressed: () {
                                    widget.onSubmitted("");
                                    _controller.clear();
                                  },
                                ) : SizedBox()
                              ],
                            );
                          },
                        ),
                      ),
                    ),
                  );
                },
              );
            },
          ),
        ],
      ),
    );
  }
}

