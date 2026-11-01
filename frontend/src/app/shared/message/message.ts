import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-message',
  standalone: true,
  templateUrl: './message.html',
  styleUrl: './message.css',
})
export class Message {
  @Input() message = '';
  @Input() type: 'error' | 'success' = 'error';
}
