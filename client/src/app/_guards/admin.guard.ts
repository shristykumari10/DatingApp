import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_service/account.service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

export const adminGuard: CanActivateFn = (route, state) => {

  const accountService = inject(AccountService);
  const Toastr = inject(ToastrService);

  if(accountService.roles().includes('Admin') || accountService.roles().includes('Moderator')){
    return true;
  }
  else{
    Toastr.error('You cannot enter this area');
    return false
  }
 
};
