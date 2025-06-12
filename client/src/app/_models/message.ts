export interface Message {
  id: number
  senderId: number
  senderUsername: string
  senderPhotoUrl: string
  recipientPhotoUrl: string
  recipientUsername: string
  content: string
  dataRead?: Date
  messageSent: Date
  recipientId: number
}