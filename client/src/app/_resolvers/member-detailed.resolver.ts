import { ResolveFn } from '@angular/router';
import { MembersService } from '../_service/members.service';
import { inject } from '@angular/core';
import { Member } from '../_models/member';

export const memberDetailedResolver: ResolveFn<Member | null> = (route, state) => {

  const memberService = inject(MembersService)

  const username = route.paramMap.get('username');
  if(!username) return null
  return memberService.getMember(username);
};
