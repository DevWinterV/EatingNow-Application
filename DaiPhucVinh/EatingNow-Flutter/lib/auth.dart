import 'dart:async';

import 'package:fam/storage/UserAccountstorage.dart';
import 'package:signals/signals_flutter.dart';

class Auth{
  final _controller = StreamController<UserAccount?>();

  final currentUser = signal<UserAccount?>(null);

  late final isLoggedIn = computed(() => currentUser != null);

  late final currentUserAccount = computed(() => currentUser() ?? null);

  late Connect<UserAccount?> _authListener;

  Auth() {
    _authListener = connect(currentUser) << _controller.stream;
  }
  dispose(){
    _authListener.dispose();
    _controller.close();
  }

  login(UserAccount userAccount){
    _controller.add(userAccount);
  }

  logout(){
    _controller.add(null);
  }
}
final auth = Auth();