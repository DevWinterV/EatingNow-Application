import 'dart:async';

import 'package:fam/storage/UserAccountstorage.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:signals/signals_flutter.dart';

class Auth{
  final _controller = StreamController<User?>();

  final currentUser = signal<User?>(FirebaseAuth.instance.currentUser ?? null);

  late final isLoggedIn = computed(() => currentUser != null);

  late final currentUserAccount = computed(() => currentUser() ?? null);

  late Connect<User?> _authListener;

  Auth() {
    _authListener = connect(currentUser) << _controller.stream;
  }
  dispose(){
    _authListener.dispose();
    _controller.close();
  }

  login(User userAccount){
    _controller.add(userAccount);
  }

  logout(){
    _controller.add(null);
  }
}
final auth = Auth();
