import { HttpInterceptorFn } from '@angular/common/http';
import { AccountService } from '../_service/account.service';
import { inject } from '@angular/core';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
const accountService = inject(AccountService)

if(accountService.currentUser())
{
  req= req.clone({
    setHeaders:
    {
      Authorization: `bearer ${accountService.currentUser()?.token}`
    }
  })
}

  return next(req);
};
