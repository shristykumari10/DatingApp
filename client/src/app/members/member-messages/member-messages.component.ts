import { Component, inject, input, OnInit, output, ViewChild } from '@angular/core';
import { MessageService } from '../../_service/message.service';

import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent {
  @ViewChild('messageForm') messageForm?: NgForm;
   messageService = inject(MessageService);
  username = input.required<string>();
  
   messageContent = '';
 
 

sendMessage(){
  this.messageService.sendMessage(this.username(), this.messageContent).then(()=> {
    this.messageForm?.reset();
  })
      
     
    
  
}


 
}
