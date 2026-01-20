# API Testing Examples

Sample HTTP requests for testing the Tasty Treat API.

## Users

### Create User
```http
POST /api/users
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "role": "Customer",
  "password": "password123",
  "phoneNo": "123-456-7890",
  "address": "123 Main St"
}
```

### Update User
```http
PUT /api/users/1
Content-Type: application/json

{
  "name": "John Updated",
  "phoneNo": "987-654-3210"
}
```

### Get User by Email
```http
GET /api/users/email/john@example.com
```

## Items

### Create Item
```http
POST /api/items
Content-Type: application/json

{
  "name": "Margherita Pizza",
  "category": "Pizza",
  "basePrice": 12.99,
  "basePriceUnit": "per item",
  "description": "Classic pizza with tomato and mozzarella"
}
```

### Get Items by Category
```http
GET /api/items/category/Pizza
```

## Customization Options

### Create Customization Option
```http
POST /api/customizationoptions
Content-Type: application/json

{
  "itemId": 1,
  "name": "Extra Cheese",
  "type": "Topping",
  "additionalPrice": 2.50
}
```

### Get Options for Item
```http
GET /api/customizationoptions/item/1
```

## Orders

### Create Order
```http
POST /api/orders
Content-Type: application/json

{
  "customerId": 1,
  "status": "Pending",
  "deliveryAddress": "123 Main St",
  "specialInstructions": "Ring doorbell twice",
  "totalAmount": 25.99
}
```

### Get Orders by Customer
```http
GET /api/orders/customer/1
```

### Get Orders by Status
```http
GET /api/orders/status/Pending
```

## Order Items

### Create Order Item
```http
POST /api/orderitems
Content-Type: application/json

{
  "orderId": 1,
  "itemId": 1,
  "customizationOptionId": 1,
  "quantity": 2,
  "orderItemPrice": 30.98,
  "isAvailable": true
}
```

### Get Order Items for Order
```http
GET /api/orderitems/order/1
```

## Reviews

### Create Review
```http
POST /api/reviews
Content-Type: application/json

{
  "orderId": 1,
  "itemId": 1,
  "customerId": 1,
  "comment": "Excellent pizza! Will order again.",
  "rating": 5
}
```

### Get Reviews for Item
```http
GET /api/reviews/item/1
```

### Get Reviews by Customer
```http
GET /api/reviews/customer/1
```

## Chat Messages

### Create Message
```http
POST /api/chatmessages
Content-Type: application/json

{
  "senderId": 1,
  "msgTxt": "Hello, I have a question about my order",
  "isRead": false
}
```

### Get Unread Messages
```http
GET /api/chatmessages/unread
```

### Mark Message as Read
```http
PUT /api/chatmessages/1
Content-Type: application/json

{
  "isRead": true
}
```

## Deliveries

### Create Delivery
```http
POST /api/deliveries
Content-Type: application/json

{
  "orderId": 1,
  "deliveryPersonId": 2,
  "deliveryStatus": "Assigned",
  "deliveryNotes": "Handle with care"
}
```

### Update Delivery Status
```http
PUT /api/deliveries/1
Content-Type: application/json

{
  "deliveryStatus": "Picked Up",
  "pickedUpAt": "2026-01-19T10:30:00Z"
}
```

### Get Deliveries by Delivery Person
```http
GET /api/deliveries/delivery-person/2
```

## Instant Quotes

### Create Quote
```http
POST /api/instantquotes
Content-Type: application/json

{
  "customerId": 1,
  "items": "[{\"itemId\":1,\"quantity\":2},{\"itemId\":3,\"quantity\":1}]",
  "tax": 2.50,
  "discount": 0.00,
  "deliveryFee": 5.00,
  "estimatedPrice": 33.48
}
```

### Convert Quote to Order
```http
PUT /api/instantquotes/1
Content-Type: application/json

{
  "convertedOrderId": 1
}
```

## Payments

### Create Payment
```http
POST /api/payments
Content-Type: application/json

{
  "orderId": 1,
  "transactionId": "TXN-123456789",
  "paymentMethod": "Credit Card",
  "amount": 33.48,
  "paymentStatus": "Completed"
}
```

### Get Payment by Transaction ID
```http
GET /api/payments/transaction/TXN-123456789
```

### Update Payment Status
```http
PUT /api/payments/1
Content-Type: application/json

{
  "paymentStatus": "Refunded"
}
```

## Testing with cURL

### Example: Create a User
```bash
curl -X POST https://localhost:7xxx/api/users \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Jane Smith",
    "email": "jane@example.com",
    "role": "Customer",
    "password": "secure123",
    "phoneNo": "555-0123"
  }'
```

### Example: Get All Items
```bash
curl -X GET https://localhost:7xxx/api/items
```

### Example: Update Order Status
```bash
curl -X PUT https://localhost:7xxx/api/orders/1 \
  -H "Content-Type: application/json" \
  -d '{
    "status": "Completed"
  }'
```

## Testing with PowerShell

### Example: Create an Item
```powershell
$body = @{
    name = "Caesar Salad"
    category = "Salads"
    basePrice = 8.99
    basePriceUnit = "per item"
    description = "Fresh romaine lettuce with Caesar dressing"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7xxx/api/items" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

### Example: Get Orders by Status
```powershell
Invoke-RestMethod -Uri "https://localhost:7xxx/api/orders/status/Pending" `
    -Method Get
```

## Common Status Values

### Order Status
- Pending
- Confirmed
- Preparing
- Ready
- Out for Delivery
- Delivered
- Cancelled

### Delivery Status
- Assigned
- Picked Up
- In Transit
- Delivered
- Failed

### Payment Status
- Pending
- Processing
- Completed
- Failed
- Refunded

## Response Codes

- `200 OK` - Successful GET, PUT operations
- `201 Created` - Successful POST operations
- `204 No Content` - Successful DELETE operations
- `400 Bad Request` - Invalid request data
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

## Notes

- Replace `https://localhost:7xxx` with your actual API URL
- All endpoints support both JSON and XML formats
- Timestamps are in UTC format
- IDs are auto-generated integers
- Passwords should be hashed in production (not implemented in base version)
